namespace X3Platform.Apps.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class ApplicationMenuProvider : IApplicationMenuProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_Menu";

        #region 构造函数:ApplicationMenuProvider()
        /// <summary>构造函数</summary>
        public ApplicationMenuProvider()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

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

        #region 函数:Save(ApplicationMenuInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationMenuInfo"/>详细信息</returns>
        public ApplicationMenuInfo Save(ApplicationMenuInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationMenuInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationMenuInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        public void Insert(ApplicationMenuInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Menu_Key_Code");

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationMenuInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        public void Update(ApplicationMenuInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return;
            }

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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        public ApplicationMenuInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationMenuInfo param = this.ibatisMapper.QueryForObject<ApplicationMenuInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        public IList<ApplicationMenuInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.FixSQL(StringHelper.ToSafeSQL(whereClause), "MySQL"));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<ApplicationMenuInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAllQueryObject(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuQueryInfo"/>的详细信息</returns>
        public IList<ApplicationMenuQueryInfo> FindAllQueryObject(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationMenuQueryInfo> list = this.ibatisMapper.QueryForList<ApplicationMenuQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllQueryObject", tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMenuInfo"/></returns>
        public IList<ApplicationMenuInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationMenuInfo> list = this.ibatisMapper.QueryForList<ApplicationMenuInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMenuQueryInfo"/></returns>
        public IList<ApplicationMenuQueryInfo> GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationMenuQueryInfo> list = this.ibatisMapper.QueryForList<ApplicationMenuQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>判断用户是否拥数据权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
        {
            return MembershipManagement.Instance.AuthorizationObjectService.HasAuthority(
                // string.Format("sys_config.{0}_Scope", this.tableName),
                string.Format("{0}_Scope", this.tableName),
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationMenuInfo)),
                authorityName,
                account);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>配置应用的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            MembershipManagement.Instance.AuthorizationObjectService.BindAuthorizationScopeObjects(
                // string.Format("sys_config.{0}_Scope", this.tableName),
                string.Format("{0}_Scope", this.tableName),
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationMenuInfo)),
                authorityName,
                scopeText);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeObjects(
                // string.Format("sys_config.{0}_Scope", this.tableName),
                string.Format("{0}_Scope", this.tableName),
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationMenuInfo)),
                authorityName);
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationMenuInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用菜单信息</param>
        public void SyncFromPackPage(ApplicationMenuInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
        }
        #endregion
    }
}
