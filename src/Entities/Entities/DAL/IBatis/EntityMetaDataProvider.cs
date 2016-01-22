namespace X3Platform.Entities.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class EntityMetaDataProvider : IEntityMetaDataProvider
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Entity_MetaData";

        #region 构造函数:EntityMetaDataProvider()
        /// <summary>构造函数</summary>
        public EntityMetaDataProvider()
        {
            this.configuration = EntitiesConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
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

        #region 函数:Save(EntityMetaDataInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityMetaDataInfo"/>详细信息</returns>
        public EntityMetaDataInfo Save(EntityMetaDataInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (EntityMetaDataInfo)param;
        }
        #endregion

        #region 函数:Insert(EntityMetaDataInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        public void Insert(EntityMetaDataInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(EntityMetaDataInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        public void Update(EntityMetaDataInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
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
        /// <returns>返回实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public EntityMetaDataInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            EntityMetaDataInfo param = this.ibatisMapper.QueryForObject<EntityMetaDataInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<EntityMetaDataInfo> list = this.ibatisMapper.QueryForList<EntityMetaDataInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByEntitySchemaId(string entitySchemaId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaId(string entitySchemaId)
        {
            string whereClause = string.Format(" EntitySchemaId = ##{0}## ", entitySchemaId);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByEntitySchemaName(string entitySchemaName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entitySchemaName">实体结构名称</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaName(string entitySchemaName)
        {
            string whereClause = string.Format(" EntitySchemaId IN ( SELECT Id FROM tb_Entity_Schema WHERE EntitySchemaName = ##{0}## ) ", entitySchemaName);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByEntityClassName(string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAllByEntityClassName(string entityClassName)
        {
            string whereClause = string.Format(" EntitySchemaId IN ( SELECT Id FROM tb_Entity_Schema WHERE EntityClassName = ##{0}## ) ", entityClassName);

            return this.FindAll(whereClause, 0);
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
        /// <returns>返回一个列表实例<see cref="EntitySchemaInfo"/></returns> 
        public IList<EntityMetaDataInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = "";

            if (query.Variables["scence"] == "Query")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause = " Name LIKE '" + searchText + "' ";
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

            IList<EntityMetaDataInfo> list = this.ibatisMapper.QueryForList<EntityMetaDataInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空.");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:GenerateSQL(string entitySchemaId, string sqlType, int effectScope)
        /// <summary>生成实体业务数据的相关SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="sqlType">SQL类型 create | read | update | delete </param>
        /// <param name="effectScope">作用范围 0 普通字段 | 1 大数据字段</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public string GenerateSQL(string entitySchemaId, string sqlType, int effectScope)
        {
            StringBuilder outString = new StringBuilder();

            return outString.ToString();
        }
        #endregion

        #region 函数:ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>0 成功 1 失败</returns>
        public int ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 函数:ExecuteSQL(string sql, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>返回业务数据信息</returns>
        public DataTable ExecuteReaderSQL(string sql, Dictionary<string, string> args)
        {
            //this.ibatisMapper.DataSource.DbProvider
            //using(SqlConnection conn = new SqlConnection()){
            //}
            //return db;
            return null;
        }
        #endregion

        #region 函数:AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        /// <summary>分析判断条件SQL</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>判断条件是否成立</returns>
        public bool AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        {
            GenericSqlCommand command = new GenericSqlCommand(this.ibatisMapper.DataSource.ConnectionString, this.ibatisMapper.DataSource.DbProvider.Name);

            return (command.ExecuteScalar(CommandType.Text, StringHelper.ToSafeSQL(sql)).ToString() == "0") ? true : false;
        }
        #endregion
    }
}
