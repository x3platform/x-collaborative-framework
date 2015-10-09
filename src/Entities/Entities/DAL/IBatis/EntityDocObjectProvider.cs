namespace X3Platform.Entities.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using X3Platform.Entities;

    /// <summary>实体文档对象</summary>
    [DataObject]
    public class EntityDocObjectProvider : IEntityDocObjectProvider
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Entity_DocObject";

        #region 构造函数:EntityDocObjectProvider()
        /// <summary>构造函数</summary>
        public EntityDocObjectProvider()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
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

        //-------------------------------------------------------
        // 保存
        //-------------------------------------------------------

        #region 函数:Save(string customTableName, IEntityDocObjectInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="IEntityDocObjectInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityDocObjectInfo"/>详细信息</returns>
        public IEntityDocObjectInfo Save(string customTableName, IEntityDocObjectInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("Id", StringHelper.ToSafeSQL(param.Id));
            args.Add("DocTitle", StringHelper.ToSafeSQL(param.DocTitle));
            args.Add("DocToken", StringHelper.ToSafeSQL(param.DocToken));
            args.Add("DocVersion", param.DocVersion);
            args.Add("DocStatus", StringHelper.ToSafeSQL(param.DocStatus));

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Save", tableName)), param);

            return param;
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        public IList<IEntityDocObjectInfo> FindAll(string customTableName, string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IEntityDocObjectInfo> list = ibatisMapper.QueryForList<IEntityDocObjectInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByDocToken(string customTableName, string docToken)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="docToken">文档全局标识</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        public IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken)
        {
            string whereClause = string.Format(" DocToken = ##{0}## ORDER BY CreatedDate ", docToken);

            return FindAll(customTableName, whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="docToken">文档全局标识</param>
        /// <param name="mapper">数据结果映射器</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        public IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper)
        {
            string whereClause = string.Format(" {0} = ##{1}## ORDER BY CreatedDate ", mapper["DocToken"].DataColumnName, docToken);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("DataColumnSql", StringHelper.ToSafeSQL(mapper.GetDataColumnSql()));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", 0);

            return ibatisMapper.QueryForList<IEntityDocObjectInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:IsExist(string customTableName, string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string customTableName, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion
    }
}
