namespace X3Platform.Apps.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class ApplicationMethodProvider : IApplicationMethodProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_Method";

        #region 构造函数:ApplicationMethodProvider()
        /// <summary>构造函数</summary>
        public ApplicationMethodProvider()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

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

        #region 函数:Save(ApplicationMethodInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationMethodInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationMethodInfo"/>详细信息</returns>
        public ApplicationMethodInfo Save(ApplicationMethodInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationMethodInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationMethodInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationMethodInfo"/>详细信息</param>
        public void Insert(ApplicationMethodInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationMethodInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationMethodInfo"/>详细信息</param>
        public void Update(ApplicationMethodInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public ApplicationMethodInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationMethodInfo param = this.ibatisMapper.QueryForObject<ApplicationMethodInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">名称</param>
        /// <returns>返回实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public ApplicationMethodInfo FindOneByName(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", StringHelper.ToSafeSQL(name));

            ApplicationMethodInfo param = this.ibatisMapper.QueryForObject<ApplicationMethodInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用唯一标识</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAllByApplicationId(string applicationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" ApplicationId = '{0}' ", StringHelper.ToSafeSQL(applicationId)));
            args.Add("OrderBy", "Name");
            args.Add("Length", 0);

            return this.ibatisMapper.QueryForList<ApplicationMethodInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAllByApplicationName(string applicationName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationName">应用名称</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAllByApplicationName(string applicationName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '{0}' ) ", StringHelper.ToSafeSQL(applicationName)));
            args.Add("OrderBy", "Name");
            args.Add("Length", 0);

            return this.ibatisMapper.QueryForList<ApplicationMethodInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="ApplicationMethodInfo"/>实例的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = null;

            if (query.Variables["scence"] == "FetchNeededSyncData")
            {
                DateTime beginDate = Convert.ToDateTime(query.Where["BeginDate"]);
                DateTime endDate = Convert.ToDateTime(query.Where["EndDate"]);

                whereClause = string.Format(" ModifiedDate BETWEEN '{0}' AND '{1}' ", beginDate, endDate);

                args.Add("WhereClause", whereClause);

            }
            else
            {
                whereClause = query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } });

                args.Add("WhereClause", whereClause);
            }

            string orderBy = query.GetOrderBySql(" ModifiedDate DESC ");

            args.Add("OrderBy", orderBy);
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<ApplicationMethodInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
        /// <returns>返回一个列表实例<see cref="ApplicationMethodInfo"/></returns>
        public IList<ApplicationMethodInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = null;

            if (query.Variables["scence"] == "Query")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " ( T.Code LIKE '%" + searchText + "%' OR T.Name LIKE '%" + searchText + "%' ) ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<ApplicationMethodInfo> list = this.ibatisMapper.QueryForList<ApplicationMethodInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

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

        #region 函数:IsExistCode(string code)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="code">代码</param>
        /// <returns>布尔值</returns>
        public bool IsExistCode(string code)
        {
            if (string.IsNullOrEmpty(code)) { throw new Exception("实例代码不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Code = '{0}' ", StringHelper.ToSafeSQL(code)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationMethodInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用请求路由信息</param>
        public void SyncFromPackPage(ApplicationMethodInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
        }
        #endregion
    }
}
