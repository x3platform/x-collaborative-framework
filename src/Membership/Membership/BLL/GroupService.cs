namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.LDAP;
    using X3Platform.LDAP.Configuration;
    using X3Platform.Configuration;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Spring;
    using System.Text;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    public class GroupService : IGroupService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IGroupProvider provider = null;

        #region 构造函数:GroupService()
        /// <summary>构造函数</summary>
        public GroupService()
              {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IGroupProvider>(typeof(IGroupProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IGroupInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="IGroupInfo"/>详细信息</returns>
        public IGroupInfo Save(IGroupInfo param)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IGroupInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                {
                    originalObject = param;
                }

                this.SyncToLDAP(param, originalObject.GlobalName, originalObject.CatalogItemId);
            }

            // 设置组织全路径
            param.FullPath = this.CombineFullPath(param.Name, param.CatalogItemId);

            // 设置唯一识别名称
            param.DistinguishedName = this.CombineDistinguishedName(param.Name, param.CatalogItemId);

            this.provider.Save(param);

            if (param != null)
            {
                string groupId = param.Id;

                // 绑定新的关系
                if (!string.IsNullOrEmpty(groupId))
                {
                    // 1.移除非默认成员关系
                    MembershipManagement.Instance.GroupService.ClearupRelation(groupId);

                    // 2.设置新的关系
                    foreach (IAccountInfo item in param.Members)
                    {
                        MembershipManagement.Instance.GroupService.AddRelation(item.Id, groupId);
                    }
                }
            }

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IGroupInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IList<IGroupInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IList<IGroupInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IList<IGroupInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户所在的所有群组信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IGroupInfo"/>实例的详细信息</returns>
        public IList<IGroupInfo> FindAllByAccountId(string accountId)
        {
            return this.provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IList<IGroupInfo> FindAllByCatalogItemId(string CatalogItemId)
        {
            return this.provider.FindAllByCatalogItemId(CatalogItemId);
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IGroupInfo"/></returns>
        public IList<IGroupInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">群组名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">群组全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return this.provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">群组标识</param>
        /// <param name="name">群组名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            return this.provider.Rename(id, name);
        }
        #endregion

        #region 函数:CombineFullPath(string name, string CatalogItemId)
        /// <summary>角色全路径</summary>
        /// <param name="name">通用角色名称</param>
        /// <param name="catalogItemId">所属类别标识</param>
        /// <returns></returns>
        public string CombineFullPath(string name, string catalogItemId)
        {
            string path = MembershipManagement.Instance.CatalogItemService.GetCatalogItemPathByCatalogItemId(catalogItemId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 函数:CombineDistinguishedName(string name, string CatalogItemId)
        /// <summary>通用角色唯一名称</summary>
        /// <param name="name">通用角色名称</param>
        /// <param name="catalogItemId">所属类别标识</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name, string catalogItemId)
        {
            string path = MembershipManagement.Instance.CatalogItemService.GetLDAPOUPathByCatalogItemId(catalogItemId);

            return string.Format("CN={0},{1}{2}", name, path, LDAPConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">帐户标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetGlobalName(string id, string globalName)
        {
            if (string.IsNullOrEmpty(globalName))
            {
                // 对象【${Id}】全局名称不能为空。
                return 1;
            }

            if (IsExistGlobalName(globalName))
            {
                return 2;
            }

            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 对象【${Id}】不存在。
                return 3;
            }

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                /*
                IGroupInfo originalObject = FindOne(id);

                if (originalObject != null)
                {
                    // 由于外部系统直接同步到人员及权限管理的数据库中，
                    // 所以 Active Directory 上不会直接创建相关对象，需要手工设置全局名称并创建相关对象。
                    if (!string.IsNullOrEmpty(originalObject.GlobalName) 
                        && LDAPManagement.Instance.Group.IsExistName(originalObject.GlobalName))
                    {
                        LDAPManagement.Instance.Group.Rename(originalObject.GlobalName, globalName);
                    }
                    else
                    {
                        LDAPManagement.Instance.Group.Add(globalName, MembershipManagement.Instance.OrganizationUnitService.GetLDAPOUPathByOrganizationUnitId(originalObject.Id));
                    }
                }
                 */
            }

            return this.provider.SetGlobalName(id, globalName);
        }
        #endregion

        #region 函数:SetExchangeStatus(string id, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="id">群组标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string id, int status)
        {
            return this.provider.SetExchangeStatus(id, status);
        }
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"group\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<groups>");

            foreach (IGroupInfo item in list)
            {
                outString.Append(((GroupInfo)item).Serializable());
            }

            outString.Append("</groups>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 属性:SyncToLDAP(IGroupInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">角色信息</param>
        public int SyncToLDAP(IGroupInfo param)
        {
            return SyncToLDAP(param, param.GlobalName, param.CatalogItemId);
        }
        #endregion

        #region 函数:SyncToLDAP(IGroupInfo param, string originalGlobalName, string originalCatalogItemId)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">角色信息</param>
        /// <param name="originalGlobalName">原始的全局名称</param>
        /// <param name="originalGroupTreeNodeId">原始的分组标识</param>
        public int SyncToLDAP(IGroupInfo param, string originalGlobalName, string originalCatalogItemId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.Name))
                {
                    // 角色【${Name}】名称为空，请配置相关信息。
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // 角色【${GlobalName}】名称为空，请配置相关信息。
                    return 1;
                }
                else
                {
                    // 1.原始的全局名称不为空。
                    // 2.Active Directory 上有相关对象。
                    if (!string.IsNullOrEmpty(originalGlobalName)
                        && LDAPManagement.Instance.Group.IsExistName(originalGlobalName))
                    {
                        if (param.GlobalName != originalGlobalName)
                        {
                            // 角色【${Name}】的名称发生改变。
                            LDAPManagement.Instance.Group.Rename(originalGlobalName, param.GlobalName);
                        }

                        if (param.CatalogItemId != originalCatalogItemId)
                        {
                            // 角色【${Name}】所属的组织发生变化。
                            LDAPManagement.Instance.Group.MoveTo(param.GlobalName,
                                MembershipManagement.Instance.CatalogItemService.GetLDAPOUPathByCatalogItemId(param.CatalogItemId));
                        }

                        return 0;
                    }
                    else
                    {
                        string parentPath = MembershipManagement.Instance.CatalogItemService.GetLDAPOUPathByCatalogItemId(param.CatalogItemId);

                        LDAPManagement.Instance.Group.Add(param.GlobalName, parentPath);

                        // 角色【${Name}】创建成功。
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(IGroupInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">群组信息</param>
        public int SyncFromPackPage(IGroupInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和群组关系
        // -------------------------------------------------------

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountGroupRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            return this.provider.FindAllRelationByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllRelationByGroupId(string groupId)
        /// <summary>根据群组查询相关帐号的关系</summary>
        /// <param name="groupId">群组标识</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountGroupRelationInfo> FindAllRelationByGroupId(string groupId)
        {
            return this.provider.FindAllRelationByGroupId(groupId);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string groupId)
        /// <summary>添加帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        public int AddRelation(string accountId, string groupId)
        {
            return AddRelation(accountId, groupId, DateTime.Now, DateTime.MaxValue);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                // 帐号标识不能为空
                return 1;
            }

            if (string.IsNullOrEmpty(groupId))
            {
                // 群组标识不能为空
                return 2;
            }

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IGroupInfo group = MembershipManagement.Instance.GroupService[groupId];

                // 帐号对象、帐号的全局名称、帐号的登录名、群组对象、群组的全局名称等不能为空。
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && group != null && !string.IsNullOrEmpty(group.GlobalName))
                {
                    LDAPManagement.Instance.Group.AddRelation(account.LoginName, LDAPSchemaClassType.User, group.Name);
                }
            }

            return this.provider.AddRelation(accountId, groupId, beginDate, endDate);
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string groupId, DateTime endDate)
        /// <summary>续约帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string groupId, DateTime endDate)
        {
            return this.provider.ExtendRelation(accountId, groupId, endDate);
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string groupId)
        /// <summary>移除帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        public int RemoveRelation(string accountId, string groupId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IGroupInfo group = MembershipManagement.Instance.GroupService[groupId];

                // 帐号对象、帐号的全局名称、帐号的登录名、群组对象、群组的全局名称等不能为空。
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && group != null && !string.IsNullOrEmpty(group.GlobalName))
                {
                    LDAPManagement.Instance.Group.RemoveRelation(account.LoginName, LDAPSchemaClassType.User, group.GlobalName);
                }
            }

            return this.provider.RemoveRelation(accountId, groupId);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的群组关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountGroupRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountGroupRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.GroupId);
                }

                return 0;
            }
            else
            {
                return this.provider.RemoveExpiredRelation(accountId);
            }
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号所有相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountGroupRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountGroupRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.GroupId);
                }

                return 0;
            }
            else
            {
                return this.provider.RemoveAllRelation(accountId);
            }
        }
        #endregion

        #region 函数:ClearupRelation(string groupId)
        /// <summary>清理群组与帐号的关系</summary>
        /// <param name="groupId">群组标识</param>
        public int ClearupRelation(string groupId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountGroupRelationInfo> list = this.FindAllRelationByGroupId(groupId);

                foreach (IAccountGroupRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.GroupId);
                }

                return 0;
            }
            else
            {
                return this.provider.ClearupRelation(groupId);
            }
        }
        #endregion
    }
}
