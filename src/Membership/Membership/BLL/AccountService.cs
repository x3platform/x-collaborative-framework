namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.LDAP;
    using X3Platform.LDAP.Configuration;
    using X3Platform.Collections;
    using X3Platform.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Scope;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary>帐号服务</summary>
    public class AccountService : IAccountService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IAccountProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, IDictionary<string, IAccountInfo>> dictionary
            = new Dictionary<string, IDictionary<string, IAccountInfo>>() { 
                { "id", new SyncDictionary<string, IAccountInfo>() },
                { "loginName", new SyncDictionary<string, IAccountInfo>() }
            };

        #region 构造函数:AccountService()
        /// <summary>构造函数</summary>
        public AccountService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IAccountProvider>(typeof(IAccountProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">帐号标识</param>
        /// <returns></returns>
        public IAccountInfo this[string id]
        {
            get { return this.provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 缓存管理
        // -------------------------------------------------------

        /// <summary></summary>
        /// <returns></returns>
        public int CreateCache()
        {
            this.dictionary = new Dictionary<string, IDictionary<string, IAccountInfo>>() { 
                { "id", new SyncDictionary<string, IAccountInfo>() },
                { "loginName", new SyncDictionary<string, IAccountInfo>() }
            };

            return 0;
        }

        /// <summary></summary>
        /// <returns></returns>
        public int ClearCache()
        {
            this.dictionary["id"].Clear();
            this.dictionary["loginName"].Clear();

            return 0;
        }

        /// <summary></summary>
        /// <returns></returns>
        public void AddCacheItem(object item)
        {
            if (item is IAccountInfo)
            {
                this.AddCacheItem((IAccountInfo)item);
            }
        }

        private void AddCacheItem(IAccountInfo item)
        {
            if (!string.IsNullOrEmpty(item.Id))
            {
                if (this.dictionary["id"].ContainsKey(item.Id))
                {
                    this.dictionary["id"].Add(item.Id, item);
                }
                else
                {
                    this.dictionary["id"][item.Id] = item;
                }
            }

            if (!string.IsNullOrEmpty(item.LoginName))
            {
                if (this.dictionary["loginName"].ContainsKey(item.LoginName))
                {
                    this.dictionary["loginName"].Add(item.LoginName, item);
                }
                else
                {
                    this.dictionary["loginName"][item.LoginName] = item;
                }
            }
        }

        /// <summary></summary>
        /// <returns></returns>
        public void RemoveCacheItem(object item)
        {
            if (item is IAccountInfo)
            {
                this.RemoveCacheItem((IAccountInfo)item);
            }
        }

        private void RemoveCacheItem(IAccountInfo item)
        {
            if (!string.IsNullOrEmpty(item.Id))
            {
                if (this.dictionary["id"].ContainsKey(item.Id))
                {
                    this.dictionary["id"].Remove(item.Id);
                }
            }

            if (!string.IsNullOrEmpty(item.LoginName))
            {
                if (this.dictionary["loginName"].ContainsKey(item.LoginName))
                {
                    this.dictionary["loginName"].Remove(item.LoginName);
                }
            }
        }

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(AccountInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">IAccountInfo 实例详细信息</param>
        /// <returns>IAccountInfo 实例详细信息</returns>
        public IAccountInfo Save(IAccountInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new Exception("实例标识不能为空。"); }

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo originalObject = this.FindOne(param.Id);

                if (originalObject == null) { originalObject = param; }

                this.SyncToLDAP(param, originalObject.GlobalName, originalObject.Status);
            }

            param.DistinguishedName = CombineDistinguishedName(param.Name);

            param = this.provider.Save(param);

            if (param != null)
            {
                string accountId = param.Id;

                // 绑定新的关系
                if (!string.IsNullOrEmpty(accountId))
                {
                    // -------------------------------------------------------
                    // 设置角色关系
                    // -------------------------------------------------------

                    // 1.移除非默认角色关系
                    MembershipManagement.Instance.RoleService.RemoveNondefaultRelation(accountId);

                    // -------------------------------------------------------
                    // 根据角色设置组织关系
                    // -------------------------------------------------------

                    // 1.移除非默认角色关系
                    MembershipManagement.Instance.OrganizationUnitService.RemoveNondefaultRelation(accountId);

                    // 2.设置新的关系
                    foreach (IAccountRoleRelationInfo item in param.RoleRelations)
                    {
                        MembershipManagement.Instance.RoleService.AddRelation(accountId, item.RoleId);

                        MembershipManagement.Instance.OrganizationUnitService.AddRelation(accountId, item.GetRole().OrganizationUnitId);

                        MembershipManagement.Instance.OrganizationUnitService.AddParentRelations(accountId, item.GetRole().OrganizationUnitId);
                    }

                    // -------------------------------------------------------
                    // 设置群组关系
                    // -------------------------------------------------------

                    // 1.移除群组关系
                    MembershipManagement.Instance.GroupService.RemoveAllRelation(accountId);

                    // 2.设置新的关系
                    foreach (IAccountGroupRelationInfo item in param.GroupRelations)
                    {
                        MembershipManagement.Instance.GroupService.AddRelation(accountId, item.GroupId);
                    }
                }
            }

            // 保存数据后, 更新缓存信息
            param = this.provider.FindOne(param.Id);

            if (param != null)
            {
                this.RemoveCacheItem(param);

                this.AddCacheItem(param);
            }

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">帐号标识</param>
        public void Delete(string id)
        {
            IAccountInfo originalObject = this.FindOne(id);

            // 删除缓存
            if (originalObject != null)
            {
                this.RemoveCacheItem(originalObject);
            }

            // 删除数据库记录
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AccountInfo Id号</param>
        /// <returns>返回一个 IAccountInfo 实例的详细信息</returns>
        public IAccountInfo FindOne(string id)
        {
            IAccountInfo param = null;

            // 查找缓存数据
            if (this.dictionary["id"].ContainsKey(id))
            {
                param = this.dictionary["id"][id];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            if (param == null)
            {
                param = this.provider.FindOne(id);

                if (param != null)
                {
                    this.AddCacheItem(param);
                }
            }

            return param;
        }
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">帐号的全局名称</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByGlobalName(string globalName)
        {
            return this.provider.FindOneByGlobalName(globalName);
        }
        #endregion

        #region 函数:FindOneByLoginName(string loginName)
        /// <summary>查询某条记录</summary>
        /// <param name="loginName">登陆名</param>
        /// <returns>返回一个 IAccountInfo 实例的详细信息</returns>
        public IAccountInfo FindOneByLoginName(string loginName)
        {
            IAccountInfo param = null;

            // 查找缓存数据
            if (this.dictionary["loginName"].ContainsKey(loginName))
            {
                param = this.dictionary["loginName"][loginName];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            if (param == null)
            {
                param = this.provider.FindOneByLoginName(loginName);

                if (param != null)
                {
                    this.AddCacheItem(param);
                }
            }

            return param;
        }
        #endregion

        #region 函数:FindOneByCertifiedMobile(string certifiedMobile)
        /// <summary>根据已验证的手机号查询某条记录</summary>
        /// <param name="certifiedMobile">已验证的手机号</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByCertifiedMobile(string certifiedMobile)
        {
            return this.provider.FindOneByCertifiedMobile(certifiedMobile);
        }
        #endregion

        #region 函数:FindOneByCertifiedEmail(string certifiedEmail)
        /// <summary>根据已验证的邮箱地址查询某条记录</summary>
        /// <param name="certifiedEmail">已验证的邮箱地址</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByCertifiedEmail(string certifiedEmail)
        {
            return this.provider.FindOneByCertifiedEmail(certifiedEmail);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 IAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 IAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 IAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId)
        /// <summary>查询某个用户所在的所有组织单位</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByOrganizationUnitId(string organizationId)
        {
            return this.provider.FindAllByOrganizationUnitId(organizationId);
        }
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId, bool defaultOrganizationUnitRelation)
        /// <summary>查询某个组织下的所有相关帐号</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="defaultOrganizationUnitRelation">默认组织关系</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByOrganizationUnitId(string organizationId, bool defaultOrganizationUnitRelation)
        {
            return this.provider.FindAllByOrganizationUnitId(organizationId, defaultOrganizationUnitRelation);
        }
        #endregion

        #region 函数:FindAllByRoleId(string roleId)
        /// <summary>查询某个角色下的所有相关帐号</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByRoleId(string roleId)
        {
            return this.provider.FindAllByRoleId(roleId);
        }
        #endregion

        #region 属性:FindAllByGroupId(string groupId)
        /// <summary>��ѯĳ��Ⱥ���µ����������ʺ�</summary>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <returns>����һ�� IIAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByGroupId(string groupId)
        {
            return this.provider.FindAllByGroupId(groupId);
        }
        #endregion

        #region 函数:FindAllWithoutMemberInfo(int length)
        /// <summary>返回所有没有成员信息的帐号信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindAllWithoutMemberInfo(int length)
        {
            return this.provider.FindAllWithoutMemberInfo(length);
        }
        #endregion

        #region 函数:FindForwardLeaderAccountsByOrganizationUnitId(string organizationId)
        /// <summary>返回所有正向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationUnitId(string organizationId)
        {
            return this.provider.FindForwardLeaderAccountsByOrganizationUnitId(organizationId, 1);
        }
        #endregion

        #region 函数:FindForwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        /// <summary>返回所有正向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        {
            return this.provider.FindForwardLeaderAccountsByOrganizationUnitId(organizationId, level);
        }
        #endregion

        #region 函数:FindBackwardLeaderAccountsByOrganizationUnitId(string organizationId)
        /// <summary>返回所有反向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationUnitId(string organizationId)
        {
            return this.provider.FindBackwardLeaderAccountsByOrganizationUnitId(organizationId, 1);
        }
        #endregion

        #region 函数:FindBackwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        /// <summary>返回所有反向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        {
            return this.provider.FindBackwardLeaderAccountsByOrganizationUnitId(organizationId, level);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        public IList<IAccountInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out  rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录.</summary>
        /// <param name="id">帐号标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistLoginNameAndGlobalName(string loginName, string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="loginName">登录名</param>
        /// <param name="globalName">姓名</param>
        /// <returns>布尔值</returns>
        public bool IsExistLoginNameAndGlobalName(string loginName, string globalName)
        {
            bool result = this.provider.IsExistLoginNameAndGlobalName(loginName, globalName);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("LoginName", loginName));

                if (!result)
                {
                    result = Convert.ToBoolean(IsExistFieldValue("GlobalName", globalName));
                }
            }

            return result;
        }
        #endregion

        #region 函数:IsExistLoginName(string loginName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="loginName">登录名</param>
        /// <returns>布尔值</returns>
        public bool IsExistLoginName(string loginName)
        {
            bool result = this.provider.IsExistLoginName(loginName);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("LoginName", loginName));
            }

            return result;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">姓名</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            bool result = this.provider.IsExistName(name);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("Name", name));
            }

            return result;
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">组织单位全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            bool result = this.provider.IsExistGlobalName(globalName);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("GlobalName", globalName));
            }

            return result;
        }
        #endregion

        #region 函数:IsExistCertifiedMobile(string certifiedMobile)
        /// <summary>检测是否存在相关的手机号</summary>
        /// <param name="certifiedMobile">已验证的手机号</param>
        /// <returns>布尔值</returns>
        public bool IsExistCertifiedMobile(string certifiedMobile)
        {
            bool result = this.provider.IsExistCertifiedMobile(certifiedMobile);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("CertifiedMobile", certifiedMobile));
            }

            return result;
        }
        #endregion

        #region 函数:IsExistCertifiedEmail(string certifiedEmail)
        /// <summary>检测是否存在相关的邮箱</summary>
        /// <param name="certifiedEmail">已验证的邮箱</param>
        /// <returns>布尔值</returns>
        public bool IsExistCertifiedEmail(string certifiedEmail)
        {
            bool result = this.provider.IsExistCertifiedEmail(certifiedEmail);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("CertifiedEmail", certifiedEmail));
            }

            return result;
        }
        #endregion

        #region 函数:IsExistFieldValue(string fieldName, string fieldValue)
        /// <summary>检测是否存在相关的字段的值</summary>
        /// <param name="fieldName">字段的名称</param>
        /// <param name="fieldValue">字段的值</param>
        public virtual string IsExistFieldValue(string fieldName, string fieldValue)
        {
            return "False";
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">帐号标识</param>
        /// <param name="name">帐号名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 不存在对象
                return 1;
            }

            return this.provider.Rename(id, name);
        }
        #endregion

        #region 函数:CreateEmptyAccount(string accountId)
        /// <summary>创建空的帐号信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns></returns>
        public IAccountInfo CreateEmptyAccount(string accountId)
        {
            AccountInfo param = new AccountInfo();

            param.Id = accountId;

            param.Locking = 0;

            param.Status = -1;

            param.ModifiedDate = param.CreatedDate = DateTime.Now;

            return param;
        }
        #endregion

        #region 函数:CombineDistinguishedName(string name)
        /// <summary>组合唯一名称</summary>
        /// <param name="name">帐号标识</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name)
        {
            //CN=${姓名},OU=组织用户,DC=lhwork,DC=net

            return string.Format("CN={0},OU={1}{2}", name,
                LDAPConfigurationView.Instance.CorporationUserFolderRoot,
                LDAPConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        // -------------------------------------------------------
        // 管理员功能
        // -------------------------------------------------------

        #region 函数:SetGlobalName(string accountId, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetGlobalName(string accountId, string globalName)
        {
            if (IsExistGlobalName(globalName))
            {
                return 1;
            }

            // 检测是否存在对象
            if (!IsExist(accountId))
            {
                // 对象【${Id}】不存在。
                return 2;
            }

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                // 同步 Active Directory 帐号全局名称
                IAccountInfo account = FindOne(accountId);

                if (account != null && !string.IsNullOrEmpty(account.LoginName))
                {
                    // 由于外部系统直接同步到人员及权限管理的数据库中，
                    // 所以 Active Directory 上不会直接创建相关对象，需要手工设置全局名称并创建相关对象。

                    if (LDAPManagement.Instance.User.IsExistLoginName(account.LoginName))
                    {
                        LDAPManagement.Instance.User.Rename(account.LoginName, globalName);
                    }
                    else
                    {
                        // 如果未创建相关帐号，则创建相关帐号。
                        LDAPManagement.Instance.User.Add(account.LoginName, globalName, string.Empty, string.Empty);

                        LDAPManagement.Instance.User.SetStatus(account.LoginName, account.Status == 1 ? true : false);

                        // LDAP 添加用户到所有人组和所在的部门组

                        LDAPManagement.Instance.Group.AddRelation(account.LoginName, LDAPSchemaClassType.User, "所有人");
                    }
                }
            }

            return this.provider.SetGlobalName(accountId, globalName);
        }
        #endregion

        #region 函数:GetPassword(string loginName)
        /// <summary>获取密码</summary>
        /// <param name="loginName">帐号</param>
        public string GetPassword(string loginName)
        {
            return this.provider.GetPassword(loginName);
        }
        #endregion

        #region 函数:GetPasswordStrength(string loginName)
        /// <summary>获取密码强度</summary>
        /// <param name="loginName">帐号</param>
        /// <returns>0 正常强度密码 | 1 低强度密码 | 2 小于最小密码长度的密码 | 3 纯数字组成的密码 | 9 默认密码</returns>
        public int GetPasswordStrength(string loginName)
        {
            string password = this.GetPassword(loginName);

            // 如果系统采用的密码加密方式是不可逆的密码, 此方法无效.
            password = MembershipManagement.Instance.PasswordEncryptionManagement.Decrypt(password);

            return this.ValidatePasswordPolicy(password);
        }
        #endregion

        #region 函数:GetPasswordChangedDate(string loginName)
        /// <summary>获取密码更新时间</summary>
        /// <param name="loginName">帐号</param>
        public DateTime GetPasswordChangedDate(string loginName)
        {
            return this.provider.GetPasswordChangedDate(loginName);
        }
        #endregion

        #region 函数:SetPassword(int accountId, string password)
        /// <summary>设置帐号密码.(管理员)</summary>
        /// <param name="accountId">编号</param>
        /// <param name="password">密码</param>
        /// <returns>修改成功, 返回 0, 旧密码不匹配, 返回 1.</returns>
        public int SetPassword(string accountId, string password)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                // 同步 Active Directory 帐号状态
                IAccountInfo account = FindOne(accountId);

                if (account != null && !string.IsNullOrEmpty(account.LoginName)
                    && !string.IsNullOrEmpty(password))
                {
                    LDAPManagement.Instance.User.SetPassword(account.LoginName, password);
                }
            }

            return this.provider.SetPassword(accountId, password);
        }
        #endregion

        #region 函数:SetLoginName(string accountId, string loginName)
        /// <summary>设置登录名</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="loginName">登录名</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetLoginName(string accountId, string loginName)
        {
            return this.provider.SetLoginName(accountId, loginName);
        }
        #endregion

        #region 函数:SetCertifiedMobile(string accountId, string telephone)
        /// <summary>设置已验证的联系电话</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="telephone">联系电话</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetCertifiedMobile(string accountId, string telephone)
        {
            return this.provider.SetCertifiedMobile(accountId, telephone);
        }
        #endregion

        #region 函数:SetCertifiedEmail(string accountId, string email)
        /// <summary>设置已验证的邮箱</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="email">邮箱</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetCertifiedEmail(string accountId, string email)
        {
            return this.provider.SetCertifiedEmail(accountId, email);
        }
        #endregion

        #region 函数:SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        /// <summary>设置已验证的头像</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="avatarVirtualPath">头像的虚拟路径</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        {
            return this.provider.SetCertifiedAvatar(accountId, avatarVirtualPath);
        }
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string accountId, int status)
        {
            return this.provider.SetExchangeStatus(accountId, status);
        }
        #endregion

        #region 函数:SetStatus(string accountId, int status)
        /// <summary>设置帐号状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetStatus(string accountId, int status)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                // 同步 Active Directory 帐号状态
                IAccountInfo account = FindOne(accountId);

                if (account != null
                    && !string.IsNullOrEmpty(account.LoginName))
                {
                    LDAPManagement.Instance.User.SetStatus(account.LoginName, status == 1 ? true : false);
                }
            }

            return this.provider.SetStatus(accountId, status);
        }
        #endregion

        #region 函数:SetIPAndLoginDate(string accountId, string ip, DateTime loginDate)
        /// <summary>设置登录名</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="ip">登录IP</param>
        /// <param name="loginDate">登录时间</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetIPAndLoginDate(string accountId, string ip, DateTime loginDate)
        {
            return this.provider.SetIPAndLoginDate(accountId, ip, loginDate);
        }
        #endregion

        // -------------------------------------------------------
        // 普通用户功能
        // -------------------------------------------------------

        #region 属性:ValidatePasswordPolicy(string password)
        /// <summary>验证密码是否符合密码策略</summary>
        /// <param name="password">密码</param>
        /// <returns>0 表示成功 1</returns>
        public int ValidatePasswordPolicy(string password)
        {
            byte[] buffer = System.Text.Encoding.Default.GetBytes(password);

            string passwordPolicyRules = MembershipConfigurationView.Instance.PasswordPolicyRules;
            int passwordPolicyMinimumLength = MembershipConfigurationView.Instance.PasswordPolicyMinimumLength;
            int passwordPolicyCharacterRepeatedTimes = MembershipConfigurationView.Instance.PasswordPolicyCharacterRepeatedTimes;

            bool flag = false;
            int charCode = -1;

            if (passwordPolicyRules.IndexOf("[Number]") > -1)
            {
                flag = false;
                charCode = -1;

                // charCode 48 - 57
                for (var i = 0; i < buffer.Length; i++)
                {
                    charCode = buffer[i];

                    if (charCode >= 48 && charCode <= 57)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    // 2 必须包含一个【0～9】数字。
                    return 2;
                }
            }

            if (passwordPolicyRules.IndexOf("[Character]") > -1)
            {
                flag = false;
                charCode = -1;

                for (var i = 0; i < buffer.Length; i++)
                {
                    charCode = buffer[i];

                    if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    // 3 必须包含一个【A～Z或a～z】字符。
                    return 3;
                }
            }

            if (passwordPolicyRules.IndexOf("[SpecialCharacter]") > -1)
            {
                flag = false;
                charCode = -1;

                // ! 33 # 35 $ 36 @ 64
                for (var i = 0; i < buffer.Length; i++)
                {
                    charCode = buffer[i];

                    if (charCode == 33 || charCode == 35 || charCode == 36 || charCode == 64)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    // 4 必须包含一个【# $ @ !】特殊字符。
                    return 4;
                }
            }

            if (passwordPolicyMinimumLength > 0 && buffer.Length < passwordPolicyMinimumLength)
            {
                // 5 密码长度必须大于【' + passwordPolicyMinimumLength + '】'。
                return 5;
            }

            if (passwordPolicyCharacterRepeatedTimes > 1 && buffer.Length > passwordPolicyCharacterRepeatedTimes)
            {
                // 判断字符连续出现的次数
                var repeatedTimes = 1;

                for (var i = 0; i < buffer.Length - passwordPolicyCharacterRepeatedTimes; i++)
                {
                    charCode = buffer[i];

                    repeatedTimes = 1;

                    for (var j = 1; j < passwordPolicyCharacterRepeatedTimes; j++)
                    {
                        if (charCode == buffer[i + j])
                        {
                            repeatedTimes++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (repeatedTimes >= passwordPolicyCharacterRepeatedTimes)
                    {
                        // 在密码中相邻字符重复次数不能超过【' + passwordPolicyCharacterRepeatedTimes + '】次。;
                        return 6;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 函数:ConfirmPassword(string accountId, string passwordType, string password)
        /// <summary>确认密码</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="passwordType">密码类型: default 默认, query 查询密码, trader 交易密码</param>
        /// <param name="password">密码</param>
        /// <returns>返回值: 0 成功 | 1 失败</returns>
        public int ConfirmPassword(string accountId, string passwordType, string password)
        {
            return this.provider.ConfirmPassword(accountId, passwordType, password);
        }
        #endregion

        #region 函数:LoginCheck(string loginName, string password)
        /// <summary>登陆检测</summary>
        /// <param name="loginName">帐号</param>
        /// <param name="password">密码</param>
        /// <returns>IAccountInfo 实例</returns>
        public IAccountInfo LoginCheck(string loginName, string password)
        {
            return this.provider.LoginCheck(loginName, password);
        }
        #endregion

        #region 函数:ChangeBasicInfo(IAccount param)
        /// <summary>修改基本信息</summary>
        /// <param name="param">IAccount 实例的详细信息</param>
        public void ChangeBasicInfo(IAccountInfo param)
        {
            this.provider.ChangeBasicInfo(param);
        }
        #endregion

        #region 函数:ChangePassword(string loginName, string newPassword, string originalPassword)
        /// <summary>修改密码</summary>
        /// <param name="loginName">编号</param>
        /// <param name="password">新密码</param>
        /// <param name="originalPassword">原始密码</param>
        /// <returns>旧密码不匹配，返回0.</returns>
        public int ChangePassword(string loginName, string password, string originalPassword)
        {
            int result = this.provider.ChangePassword(loginName,
                            password,
                            originalPassword);

            if (result == 0 && LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                // 同步 Active Directory 帐号状态
                IAccountInfo account = this.FindOneByLoginName(loginName);

                if (account != null && !string.IsNullOrEmpty(account.LoginName)
                    && !string.IsNullOrEmpty(password))
                {
                    LDAPManagement.Instance.User.SetPassword(account.LoginName, password);
                }
            }

            return result;
        }
        #endregion

        #region 函数:RefreshModifiedDate(string accountId)
        /// <summary>刷新帐号的更新时间</summary>
        /// <param name="accountId">帐户标识</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int RefreshModifiedDate(string accountId)
        {
            return this.provider.RefreshModifiedDate(accountId);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(IAccount account)
        /// <summary>获取帐号相关的权限对象</summary>
        /// <param name="account">IAccount 实例的详细信息</param>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(IAccountInfo account)
        {
            return this.provider.GetAuthorizationScopeObjects(account);
        }
        #endregion

        #region 函数:SyncToLDAP(IAccountInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">帐号信息</param>
        public int SyncToLDAP(IAccountInfo param)
        {
            return SyncToLDAP(param, param.GlobalName, param.Status);
        }
        #endregion

        #region 函数:SyncToLDAP(IAccountInfo param, string originalGlobalName, int originalStatus)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">帐号信息</param>
        /// <param name="originalGlobalName">原始全局名称</param>
        /// <param name="originalStatus">原始状态</param>
        public int SyncToLDAP(IAccountInfo param, string originalGlobalName, int originalStatus)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.LoginName))
                {
                    // 用户【${Name}(${LoginName})】登录名为空，请配置相关信息。
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // 用户【${Name}(${LoginName})】全局名称为空，请配置相关信息。
                    return 2;
                }
                else
                {
                    // 1.原始的全局名称和登录名都不为空。
                    // 2.Active Directory 上有相关对象。
                    if (!(string.IsNullOrEmpty(originalGlobalName) || string.IsNullOrEmpty(param.LoginName))
                        && LDAPManagement.Instance.User.IsExistLoginName(param.LoginName))
                    {
                        // 如果已存在相关帐号，同步全局名称和帐号状态。

                        if (param.GlobalName != originalGlobalName)
                        {
                            LDAPManagement.Instance.User.Rename(param.LoginName, param.GlobalName);
                        }

                        LDAPManagement.Instance.User.SetStatus(param.LoginName, param.Status == 1 ? true : false);
                    }
                    else
                    {
                        if (LDAPManagement.Instance.User.IsExist(param.LoginName, param.GlobalName))
                        {
                            // "用户【${Name}(${LoginName})】的全局名称已被其他人创建，请设置相关配置。
                            return 3;
                        }
                        else if (param.Status == 0)
                        {
                            // "用户【${Name}(${LoginName})】的帐号为【禁用】状态，如果需要创建 Active Directory 帐号，请设置相关配置。
                            return 4;
                        }
                        else
                        {
                            // 如果未创建相关帐号，则创建相关帐号。
                            LDAPManagement.Instance.User.Add(param.LoginName, param.GlobalName, string.Empty, string.Empty);

                            LDAPManagement.Instance.User.SetStatus(param.LoginName, param.Status == 1 ? true : false);

                            // LDAP 添加用户到所有人组和所在的部门组

                            LDAPManagement.Instance.Group.AddRelation(param.LoginName, LDAPSchemaClassType.User, "所有人");

                            // "用户【${Name}(${LoginName})】创建成功。
                            return 0;
                        }
                    }
                }
            }

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(IMemberInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">帐号信息</param>
        public int SyncFromPackPage(IAccountInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion
    }
}