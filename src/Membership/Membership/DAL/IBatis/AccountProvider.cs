namespace X3Platform.Membership.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Scope;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class AccountProvider : IAccountProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Account";

        /// <summary></summary>
        public AccountProvider()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(AccountInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">IAccountInfo 实例详细信息</param>
        /// <returns>IAccountInfo 实例详细信息</returns>
        public IAccountInfo Save(IAccountInfo param)
        {
            if (string.IsNullOrEmpty(param.Id) || !this.IsExist(param.Id))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(IAccountInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">IAccountInfo 实例的详细信息</param>
        public void Insert(IAccountInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_Account_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);

            MembershipManagement.Instance.MemberService.Save(new MemberInfo(param.Id, param.Id));
        }
        #endregion

        #region 函数:Update(AccountInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">IAccountInfo 实例的详细信息</param>
        public void Update(IAccountInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);

            // 刷新相关对象更新时间
            this.RefreshModifiedDate(param.Id);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">帐号标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            id = StringHelper.ToSafeSQL(id, true);

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                // 删除帐号关系信息
                args.Add("AccountId", id);

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountGroup", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountRole", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountAssignedJob", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountOrganizationUnit", tableName)), args);

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountGrant", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountLog", tableName)), args);

                // 删除帐号信息
                args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch (DataException ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AccountInfo Id号</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            IAccountInfo param = this.ibatisMapper.QueryForObject<IAccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">帐号的全局名称</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByGlobalName(string globalName)
        {
            string whereClause = string.Format(" GlobalName = ##{0}## ", StringHelper.ToSafeSQL(globalName));

            IList<IAccountInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 函数:FindOneByLoginName(string loginName)
        /// <summary>查询某条记录</summary>
        /// <param name="loginName">登陆名</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByLoginName(string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);

            return this.ibatisMapper.QueryForObject<AccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByLoginName", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByCertifiedTelephone(string certifiedTelephone)
        /// <summary>根据已验证的手机号查询某条记录</summary>
        /// <param name="certifiedTelephone">已验证的手机号</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByCertifiedTelephone(string certifiedTelephone)
        {
            string whereClause = string.Format(" CertifiedTelephone = ##{0}## ", StringHelper.ToSafeSQL(certifiedTelephone, true));

            IList<IAccountInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 函数:FindOneByCertifiedEmail(string certifiedEmail)
        /// <summary>根据已验证的邮箱地址查询某条记录</summary>
        /// <param name="certifiedEmail">已验证的邮箱地址</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IAccountInfo FindOneByCertifiedEmail(string certifiedEmail)
        {
            string whereClause = string.Format(" CertifiedEmail = ##{0}## ", StringHelper.ToSafeSQL(certifiedEmail, true));

            IList<IAccountInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 IAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAll(string whereClause, int length)
        {
            IList<IAccountInfo> results = new List<IAccountInfo>();

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<AccountInfo> list = this.ibatisMapper.QueryForList<AccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            foreach (IAccountInfo item in list)
            {
                results.Add(item);
            }

            return results;
        }
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId)
        /// <summary>查询某个用户所在的所有组织单位</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByOrganizationUnitId(string organizationId)
        {
            string whereClause = string.Format(" Id IN ( SELECT AccountId FROM tb_Account_OrganizationUnit WHERE OrganizationUnitId = ##{0}## ) ", organizationId);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId, bool defaultOrganizationUnitRelation)
        /// <summary>查询某个组织下的所有相关帐号</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="defaultOrganizationUnitRelation">默认组织关系</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByOrganizationUnitId(string organizationId, bool defaultOrganizationUnitRelation)
        {
            if (defaultOrganizationUnitRelation)
            {
                string whereClause = string.Format(" Id IN ( SELECT AccountId FROM tb_Member WHERE OrganizationUnitId = ##{0}## ) ", organizationId);

                return this.FindAll(whereClause, 0);
            }
            else
            {
                return this.FindAllByOrganizationUnitId(organizationId);
            }
        }
        #endregion

        #region 函数:FindAllByRoleId(string roleId)
        /// <summary>查询某个角色下的所有相关帐号</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByRoleId(string roleId)
        {
            string whereClause = string.Format(" Id IN ( SELECT AccountId FROM tb_Account_Role WHERE RoleId = ##{0}## ) ", roleId);

            return FindAll(whereClause, 0);
        }

        #endregion

        #region 函数:FindAllByGroupId(string groupId)
        /// <summary>查询某个群组下的所有相关帐号</summary>
        /// <param name="groupId">群组标识</param>
        /// <returns>返回一个 IIAccountInfo 实例的详细信息</returns>
        public IList<IAccountInfo> FindAllByGroupId(string groupId)
        {
            string whereClause = string.Format(" Id IN ( SELECT AccountId FROM tb_Account_Group WHERE GroupId = ##{0}##) ", groupId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllWithoutMemberInfo(int length)
        /// <summary>返回所有没有成员信息的帐号信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindAllWithoutMemberInfo(int length)
        {
            string whereClause = " Id NOT IN ( SELECT AccountId FROM tb_Member ) ";

            return FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindForwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        /// <summary>返回所有正向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        {
            string whereClause = string.Format(@" 
Id IN ( SELECT AccountId FROM tb_Account_Role WHERE RoleId IN ( 
            SELECT Id FROM tb_Role WHERE
                OrganizationUnitId = dbo.func_GetDepartmentIdByOrganizationUnitId( ##{0}## , {1} ) 
                AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority >= 40 )
)) ", organizationId, level);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindBackwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        /// <summary>返回所有反向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        public IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationUnitId(string organizationId, int level)
        {
            string whereClause = string.Format(@" 
Id IN ( SELECT AccountId FROM tb_Account_Role WHERE Role IN ( 
            SELECT Id FROM tb_Role WHERE
                OrganizationUnitId IN ( dbo.func_GetDepartmentIdByOrganizationUnitId( ##{0}## , ( dbo.func_GetDepartmentLevelByOrganizationUnitId( ##{0}## ) - {1} ) ) )
                AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority >= 40 )
)) ", organizationId, level);

            return FindAll(whereClause, 0);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            if (query.Variables["scence"] == "Query")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

                args.Add("WhereClause", " ( T.Name LIKE '%" + searchText + "%' OR T.GlobalName LIKE '%" + searchText + "%' OR T.LoginName LIKE '%" + searchText + "%' ) ");
            }
            else if (query.Variables["scence"] == "QueryByOrganizationUnitId")
            {
                args.Add("WhereClause", " T.Id IN ( SELECT AccountId FROM tb_Account_Role WHERE RoleId IN ( SELECT Id FROM tb_Role WHERE OrganizationUnitId = '" + StringHelper.ToSafeSQL(query.Where["OrganizationUnitId"].ToString()) + "' ) ) ");
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" OrderId, ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<IAccountInfo> list = this.ibatisMapper.QueryForList<IAccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">登录名</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistLoginNameAndGlobalName(string loginName, string nickName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="loginName">登录名</param>
        /// <param name="name">姓名</param>
        /// <returns>布尔值</returns>
        public bool IsExistLoginNameAndGlobalName(string loginName, string name)
        {
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(name)) { throw new Exception("实例登录名或姓名不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" LoginName = '{0}' AND Name = '{0}' ", StringHelper.ToSafeSQL(loginName), StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistLoginName(string loginName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="loginName">登录名</param>
        /// <returns>布尔值</returns>
        public bool IsExistLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName)) { throw new Exception("实例登录名不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" LoginName = '{0}' ", StringHelper.ToSafeSQL(loginName)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例姓名不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">人员全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            if (string.IsNullOrEmpty(globalName)) { throw new Exception("实例全局名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" GlobalName = '{0}' ", StringHelper.ToSafeSQL(globalName)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistCertifiedTelephone(string certifiedTelephone)
        /// <summary>检测是否存在相关的手机号</summary>
        /// <param name="certifiedTelephone">已验证的手机号</param>
        /// <returns>布尔值</returns>
        public bool IsExistCertifiedTelephone(string certifiedTelephone)
        {
            if (string.IsNullOrEmpty(certifiedTelephone)) { throw new Exception("手机号不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" CertifiedTelephone = '{0}' ", StringHelper.ToSafeSQL(certifiedTelephone)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistCertifiedEmail(string certifiedEmail)
        /// <summary>检测是否存在相关的邮箱</summary>
        /// <param name="certifiedEmail">已验证的邮箱</param>
        /// <returns>布尔值</returns>
        public bool IsExistCertifiedEmail(string certifiedEmail)
        {
            if (string.IsNullOrEmpty(certifiedEmail)) { throw new Exception("邮箱地址不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" CertifiedEmail = '{0}' ", StringHelper.ToSafeSQL(certifiedEmail)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">帐号标识</param>
        /// <param name="name">帐号名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("Name", StringHelper.ToSafeSQL(name));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", tableName)), args);

            // 刷新相关对象更新时间
            this.RefreshModifiedDate(id);

            return 0;
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("GlobalName", StringHelper.ToSafeSQL(globalName));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetGlobalName", tableName)), args);

            // 刷新相关对象更新时间
            RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:GetPassword(string loginName)
        /// <summary>获取密码(管理员)</summary>
        /// <param name="loginName">帐号</param>
        /// <returns>密码</returns>
        public string GetPassword(string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", StringHelper.ToSafeSQL(loginName));

            return this.ibatisMapper.QueryForText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPassword", tableName)), args);
        }
        #endregion

        #region 函数:GetPasswordChangedDate(string loginName)
        /// <summary>获取密码更新时间</summary>
        /// <param name="loginName">帐号</param>
        public DateTime GetPasswordChangedDate(string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", StringHelper.ToSafeSQL(loginName));

            return Convert.ToDateTime(this.ibatisMapper.QueryForText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPasswordChangedDate", tableName)), args));
        }
        #endregion

        #region 函数:SetPassword(string accountId, string password)
        /// <summary>设置帐号密码(管理员)</summary>
        /// <param name="accountId">编号</param>
        /// <param name="password">密码</param>
        /// <returns>修改成功, 返回 0, 旧密码不匹配, 返回 1.</returns>
        public int SetPassword(string accountId, string password)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("Password", StringHelper.ToSafeSQL(password));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetPassword", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetLoginName(string accountId, string loginName)
        /// <summary>设置登录名</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="loginName">登录名</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetLoginName(string accountId, string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("LoginName", StringHelper.ToSafeSQL(loginName));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetLoginName", tableName)), args);

            // 刷新相关对象更新时间
            this.RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:SetCertifiedTelephone(string accountId, string telephone)
        /// <summary>设置已验证的联系电话</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="telephone">联系电话</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetCertifiedTelephone(string accountId, string telephone)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("CertifiedTelephone", StringHelper.ToSafeSQL(telephone));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCertifiedTelephone", tableName)), args);

            // 刷新相关对象更新时间
            this.RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:SetCertifiedEmail(string accountId, string email)
        /// <summary>设置已验证的邮箱</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="email">邮箱</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetCertifiedEmail(string accountId, string email)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("CertifiedEmail", StringHelper.ToSafeSQL(email));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCertifiedEmail", tableName)), args);

            // 刷新相关对象更新时间
            this.RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        /// <summary>设置已验证的头像</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="avatarVirtualPath">头像的虚拟路径</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("CertifiedAvatar", StringHelper.ToSafeSQL(avatarVirtualPath));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCertifiedAvatar", tableName)), args);

            // 刷新相关对象更新时间
            this.RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string accountId, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("EnableExchangeEmail", status);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetExchangeStatus", tableName)), args);

            // 刷新相关对象更新时间
            RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:SetStatus(string accountId, int status)
        /// <summary>设置状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetStatus(string accountId, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("Status", status);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", tableName)), args);

            // 刷新相关对象更新时间
            RefreshModifiedDate(accountId);

            return 0;
        }
        #endregion

        #region 函数:SetIPAndLoginDate(string accountId, string ip, string loginDate)
        /// <summary>设置登录名</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="ip">登录IP</param>
        /// <param name="loginDate">登录时间</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetIPAndLoginDate(string accountId, string ip, string loginDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("IP", ip);
            args.Add("LoginDate", loginDate);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetIPAndLoginDate", tableName)), args);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 普通用户功能
        // -------------------------------------------------------

        #region 函数:ConfirmPassword(string accountId, string passwordType, string password)
        /// <summary>确认密码</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="passwordType">密码类型: default 默认, query 查询密码, trader 交易密码</param>
        /// <param name="password">密码</param>
        /// <returns>返回值: 0 成功 | 1 失败</returns>
        public int ConfirmPassword(string accountId, string passwordType, string password)
        {
            if (string.IsNullOrEmpty(password)) { throw new Exception("���벻��Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("PasswordType", StringHelper.ToSafeSQL(passwordType));
            args.Add("Password", StringHelper.ToSafeSQL(password));

            if (passwordType == "trader")
            {
                return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_ConfirmTraderPassword", tableName)), args)) == 0) ? 1 : 0;
            }
            else if (passwordType == "query")
            {
                return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_ConfirmQueryPassword", tableName)), args)) == 0) ? 1 : 0;
            }
            else
            {
                return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_ConfirmPassword", tableName)), args)) == 0) ? 1 : 0;
            }
        }
        #endregion

        #region 函数:LoginCheck(string loginName, string password)
        /// <summary>登陆检测</summary>
        /// <param name="loginName">帐号</param>
        /// <param name="password">密码</param>
        /// <returns>IAccountInfo 实例</returns>
        public IAccountInfo LoginCheck(string loginName, string password)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);
            args.Add("Password", password);

            return this.ibatisMapper.QueryForObject<IAccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_LoginCheck", tableName)), args);
        }
        #endregion

        #region 函数:ChangeBasicInfo(IAccount param)
        /// <summary>修改基本信息</summary>
        /// <param name="param">IAccount 实例的详细信息</param>
        public void ChangeBasicInfo(IAccountInfo param)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region 函数:ChangePassword(string loginName, string password, string originalPassword)
        /// <summary>修改密码</summary>
        /// <param name="loginName">编号</param>
        /// <param name="password">新密码</param>
        /// <param name="originalPassword">原始密码</param>
        /// <returns>旧密码不匹配,返回1.</returns>
        public int ChangePassword(string loginName, string password, string originalPassword)
        {
            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);
            args.Add("Password", originalPassword);

            isExist = (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CheckPassword", tableName)), args)) == 0) ? false : true;

            if (isExist)
            {
                args.Clear();

                args.Add("LoginName", loginName);
                args.Add("Password", password);

                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_ChangePassword", tableName)), args);

                return 0;
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 函数:RefreshModifiedDate(string accountId)
        /// <summary>刷新帐号的更新时间</summary>
        /// <param name="accountId">帐户标识</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int RefreshModifiedDate(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Refresh_Table_Account", tableName)), args);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Refresh_Table_Member", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(IAccountInfo account)
        /// <summary>获取帐号相关的权限对象</summary>
        /// <param name="account">IAccount 实例的详细信息</param>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(IAccountInfo account)
        {
            string scopeText = null;

            IList<IAuthorizationScope> result = new List<IAuthorizationScope>();

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId = '{0}' ", StringHelper.ToSafeSQL(account.Id)));

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScopesByAccount", tableName)), args);

            foreach (DataRow row in table.Rows)
            {
                scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + "#" + row["AuthorizationObjectName"] + ";";
            }

            return MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);
        }
        #endregion

        #region 函数:SyncFromPackPage(MemberInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">帐号信息</param>
        public int SyncFromPackPage(IAccountInfo param)
        {
            // 此版本只同步姓名和帐号状态，不同步登录名。

            string accountId = param.Id;

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            if (param.RoleRelations.Count > 0)
            {
                // 1.设置默认角色信息
                IMemberInfo member = MembershipManagement.Instance.MemberService.FindOne(param.Id);

                if (member.Role != null)
                {
                    IRoleInfo defaultRole = MembershipManagement.Instance.RoleService[member.Role.Id];

                    if (defaultRole != null)
                    {
                        MembershipManagement.Instance.RoleService.SetDefaultRelation(accountId, member.Role.Id);

                        MembershipManagement.Instance.OrganizationUnitService.SetDefaultRelation(accountId, member.Role.OrganizationUnitId);
                    }
                }

                // 2.移除非默认角色关系
                MembershipManagement.Instance.RoleService.RemoveNondefaultRelation(accountId);

                // 3.移除非默认组织关系
                MembershipManagement.Instance.OrganizationUnitService.RemoveNondefaultRelation(accountId);

                // 4.设置新的关系
                foreach (IAccountRoleRelationInfo item in param.RoleRelations)
                {
                    MembershipManagement.Instance.RoleService.AddRelation(accountId, item.RoleId);

                    // 根据角色设置组织关系

                    IRoleInfo role = MembershipManagement.Instance.RoleService.FindOne(item.RoleId);

                    // [容错]如果角色信息为空，中止相关组织设置
                    if (role == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(role.OrganizationUnitId))
                    {
                        MembershipManagement.Instance.OrganizationUnitService.AddRelation(accountId, role.OrganizationUnitId);

                        MembershipManagement.Instance.OrganizationUnitService.AddParentRelations(accountId, role.OrganizationUnitId);
                    }
                }

                // 5.再次设置默认角色信息
                if (member.Role != null)
                {
                    MembershipManagement.Instance.RoleService.SetDefaultRelation(accountId, member.Role.Id);

                    MembershipManagement.Instance.OrganizationUnitService.SetDefaultRelation(accountId, member.Role.OrganizationUnitId);
                }

                //
                // 设置群组关系
                //

                // 1.移除群组关系
                MembershipManagement.Instance.GroupService.RemoveAllRelation(accountId);

                // 2.设置新的关系
                foreach (IAccountGroupRelationInfo item in param.GroupRelations)
                {
                    MembershipManagement.Instance.GroupService.AddRelation(accountId, item.GroupId);
                }

            }

            return 0;
        }
        #endregion
    }
}