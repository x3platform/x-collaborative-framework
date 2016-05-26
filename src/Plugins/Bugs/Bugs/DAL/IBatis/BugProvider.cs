namespace X3Platform.Plugins.Bugs.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Scope;
    using X3Platform.Util;

    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Plugins.Bugs.IDAL;
    using X3Platform.Plugins.Bugs.Model;

    [DataObject]
    public class BugProvider : IBugProvider
    {
        /// <summary>配置</summary>
        private BugConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Bug";

        public BugProvider()
        {
            this.configuration = BugConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:CreateGenericSqlCommand()
        /// <summary>创建通用SQL命令对象</summary>
        public GenericSqlCommand CreateGenericSqlCommand()
        {
            return this.ibatisMapper.CreateGenericSqlCommand();
        }
        #endregion

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
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(BugInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">BugInfo 实例详细信息</param>
        /// <returns>BugInfo 实例详细信息</returns>
        public BugInfo Save(BugInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(BugInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">BugInfo 实例的详细信息</param>
        public void Insert(BugInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(BugInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">BugInfo 实例的详细信息</param>
        public void Update(BugInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public int Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return 1; }

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                //  删除问题的附属信息
                args.Add("BugId", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_History_DeleteByBugId", this.tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Comment_DeleteByBugId", this.tableName)), args);

                //  删除问题的实体信息
                args.Add("Id", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch
            {
                this.ibatisMapper.RollBackTransaction();
                throw;
            }

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">BugInfo Id号</param>
        /// <returns>返回一个 BugInfo 实例的详细信息</returns>
        public BugInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 函数:FindOneByCode(string code)
        /// <summary>查询某条记录</summary>
        /// <param name="code">问题编号</param>
        /// <returns>返回一个 BugInfo 实例的详细信息</returns>
        public BugInfo FindOneByCode(string code)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Code", StringHelper.ToSafeSQL(code));

            return this.ibatisMapper.QueryForObject<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCode", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 BugInfo 实例的详细信息</returns>
        public IList<BugInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            whereClause = (string.IsNullOrEmpty(whereClause)) ? " 1=1 ORDER BY CreatedDate " : whereClause;

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<BugInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = null;

            if (query.Variables["scence"] == "Query")
            {
                string docStatus = StringHelper.ToSafeSQL(query.Where["DocStatus"].ToString(), true);
                string code = StringHelper.ToSafeSQL(query.Where["Code"].ToString(), true);
                string subject = StringHelper.ToSafeSQL(query.Where["Subject"].ToString());
                string categoryId = StringHelper.ToSafeSQL(query.Where["CategoryId"].ToString(), true);

                whereClause = " T.Status=1 AND T.DocStatus!='Draft' AND T.DocStatus!='Abandon' ";

                if (!string.IsNullOrEmpty(docStatus))
                {
                    whereClause += " AND T.DocStatus = '" + docStatus + "' ";
                }

                if (!string.IsNullOrEmpty(code))
                {
                    whereClause += " AND T.Code LIKE '%" + code + "%' ";
                }

                if (!string.IsNullOrEmpty(subject))
                {
                    whereClause += " AND T.Subject LIKE '%" + subject + "%' ";
                }

                if (!string.IsNullOrEmpty(categoryId))
                {
                    whereClause += " AND T.CategoryId = '" + categoryId + "' ";
                }

                args.Add("WhereClause", whereClause);
            }
            else if (query.Variables["scence"] == "QueryMyList")
            {
                string accountId = query.Variables["accountId"];

                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());
                string status = StringHelper.ToSafeSQL(query.Where["Status"].ToString(), true);

                whereClause += " ( T.AccountId = '" + accountId + "' OR T.AssignToAccountId = '" + accountId + "' ) ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " AND T.Title LIKE '%" + searchText + "%'  OR T.Code LIKE '%" + searchText + "%' ";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    whereClause += " AND T.Status IN (" + status + ") ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Title", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<BugInfo> list = this.ibatisMapper.QueryForList<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<BugQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = null;

            if (query.Variables["scence"] == "Query")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString(), true);
                string categoryIndex = StringHelper.ToSafeSQL(query.Where["CategoryIndex"].ToString(), true);
                string status = StringHelper.ToSafeSQL(query.Where["Status"].ToString(), true);

                whereClause = " 1 = 1 ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " AND ( T.Code LIKE '%" + searchText + "%' OR T.Title LIKE '%" + searchText + "%' OR T.Content LIKE '%" + searchText + "%' OR T.AccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE '%" + searchText + "%' OR AccountLoginName LIKE '%" + searchText + "%' ) OR T.AssignToAccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE '%" + searchText + "%' OR AccountLoginName LIKE '%" + searchText + "%' ) ) ";
                }

                if (!string.IsNullOrEmpty(categoryIndex))
                {
                    whereClause += " AND CategoryId IN ( SELECT Id FROM tb_Bug_Category WHERE CategoryIndex LIKE '" + categoryIndex.Replace("\\", "\\\\") + "%' ) ";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    whereClause += " AND T.Status = " + status + " ";
                }

                // 移除 1 = 1
                if (whereClause.IndexOf(" 1 = 1  AND ") > -1)
                {
                    whereClause = whereClause.Replace(" 1 = 1  AND ", string.Empty);
                }

                args.Add("WhereClause", whereClause);
            }
            else if (query.Variables["scence"] == "QueryMyList")
            {
                string accountId = query.Variables["accountId"];

                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());
                string status = StringHelper.ToSafeSQL(query.Where["Status"].ToString(), true);

                whereClause += " ( T.AccountId = '" + accountId + "' OR T.AssignToAccountId = '" + accountId + "' ) ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " AND T.Title LIKE '%" + searchText + "%'  OR T.Code LIKE '%" + searchText + "%' ";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    whereClause += " AND T.Status IN (" + status + ") ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Title", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<BugQueryInfo> list = this.ibatisMapper.QueryForList<BugQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">BugInfo 实例详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        // -------------------------------------------------------
        // 权限设置
        // -------------------------------------------------------

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询实体对象的权限信息</summary> 
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            string scopeText = null;

            IList<IAuthorizationScope> result = new List<IAuthorizationScope>();

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(entityId));

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScope", this.tableName)), args);

            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(row["AuthorizationObjectType"].ToString()) && !string.IsNullOrEmpty(row["AuthorizationObjectId"].ToString()))
                {
                    scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + ";";
                }
            }

            return MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);
        }
        #endregion
    }
}
