// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Apps.DAL.MySQL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    using X3Platform.Util;

    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [DataObject]
    public class ApplicationProvider : IApplicationProvider
    {
        private AppsConfiguration configuration = null;

        private string ibatisMapping = null;

        private ISqlMapper ibatisMapper = null;

        private string tableName = "tb_Application";

        /// <summary></summary>
        public ApplicationProvider()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
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
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">ApplicationInfo 实例详细信息</param>
        /// <returns>ApplicationInfo 实例详细信息</returns>
        public ApplicationInfo Save(ApplicationInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">ApplicationInfo 实例的详细信息</param>
        public void Insert(ApplicationInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Key_Code");

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">ApplicationInfo 实例的详细信息</param>
        public void Update(ApplicationInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">ApplicationInfo Id号</param>
        /// <returns>返回一个 ApplicationInfo 实例的详细信息</returns>
        public ApplicationInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<ApplicationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByApplicationName(string applicationName)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationName">applicationName</param>
        /// <returns>返回一个 ApplicationInfo 实例的详细信息</returns>
        public ApplicationInfo FindOneByApplicationName(string applicationName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationName", applicationName);

            return this.ibatisMapper.QueryForObject<ApplicationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByApplicationName", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 ApplicationInfo 实例的详细信息</returns>
        public IList<ApplicationInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationInfo> list = this.ibatisMapper.QueryForList<ApplicationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>根据帐号所属的标准角色信息对应的应用系统的功能点, 查询此帐号有权限启用的应用系统信息.</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        public IList<ApplicationInfo> FindAllByAccountId(string accountId)
        {
            // AccountId => tb_Application_Feature
            string whereClause = string.Format(@"
    Id IN ( SELECT ApplicationId FROM [tb_Application_Feature] WHERE Id IN (
        SELECT DISTINCT EntityId FROM view_AuthorizationObject_Account View1, tb_Application_Feature_Scope Scope
        WHERE 
            View1.AccountId = ##{0}##
            AND View1.AuthorizationObjectId = Scope.AuthorizationObjectId
            AND View1.AuthorizationObjectType = Scope.AuthorizationObjectType)) 
", accountId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByRoleIds(string roleIds)
        /// <summary>根据角色所属的标准角色信息对应的应用系统的功能点, 查询此帐号有权限启用的应用系统信息.</summary>
        /// <param name="roleIds">角色标识</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        public IList<ApplicationInfo> FindAllByRoleIds(string roleIds)
        {
            // RoleIds => tb_Application_Feature
            string whereClause = string.Format(@"
    Id IN ( SELECT ApplicationId FROM [tb_Application_Feature] WHERE Id IN (
        SELECT DISTINCT EntityId FROM tb_Application_Feature_Scope Scope WHERE 
        ( AuthorizationObjectType = 'Role' AND AuthorizationObjectId IN ( SELECT Id FROM tb_Role WHERE Id=##{0}##))
        AND ( AuthorizationObjectType = 'StandardRole' AND AuthorizationObjectId IN ( SELECT StandardRoleId FROM tb_Role WHERE Id=##{0}##))))
", StringHelper.ToSafeSQL(roleIds).Replace(",", "##,##"));

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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<ApplicationInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { 
               { "Code", "LIKE" }, { "ApplicationName", "LIKE" }, { "ApplicationDisplayName", "LIKE" } 
            }));
            args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

            args.Add("RowCount", 0);

            IList<ApplicationInfo> list = this.ibatisMapper.QueryForList<ApplicationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">ApplicationInfo 实例详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:HasAuthority(string accountId, string applicationId, string authorityName)
        /// <summary>判断用户是否拥应用有权限信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string accountId, string applicationId, string authorityName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("AccountId", accountId);
            args.Add("ApplicationId", applicationId);
            args.Add("AuthorityId", authority.Id);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasAuthority", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string applicationId, string authorityName, string scopeText)
        /// <summary>配置应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string applicationId, string authorityName, string scopeText)
        {
            if (!string.IsNullOrEmpty(applicationId))
            {
                Dictionary<string, object> args = new Dictionary<string, object>();

                AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

                args.Add("ApplicationId", applicationId);
                args.Add("AuthorityId", authority.Id);

                // 移除老的权限列表
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveAuthorizationScopeObjects", tableName)), args);

                string[] list = scopeText.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                // 设置新的权限范围
                foreach (string item in list)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        args["AuthorizationObjectType"] = item.Split('#')[0].ToString().Substring(0, 1).ToUpper() + item.Split('#')[0].ToString().Substring(1);
                        args["AuthorizationObjectId"] = item.Split('#')[1].ToString();

                        this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddAuthorizationScopeObject", tableName)), args);
                    }
                }
            }
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string applicationId, string authorityName)
        /// <summary>查询应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string applicationId, string authorityName)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                new Exception("应用标识不允许为空.");
            }

            string scopeText = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ApplicationId", applicationId);

            args.Add("AuthorityId", authority.Id);

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScopeObjects", tableName)), args);

            foreach (DataRow row in table.Rows)
            {
                scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + ";";
            }

            IList<MembershipAuthorizationScopeObject> list = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用信息</param>
        public void SyncFromPackPage(ApplicationInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
        }
        #endregion
    }
}
