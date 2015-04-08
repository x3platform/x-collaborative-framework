namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using System.Data;
    #endregion

    /// <summary></summary>
    public class EntityMetaDataService : IEntityMetaDataService
    {
        /// <summary>数据提供器</summary>
        private IEntityMetaDataProvider provider = null;

        #region 构造函数:EntityMetaDataService()
        /// <summary>构造函数</summary>
        public EntityMetaDataService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            provider = objectBuilder.GetObject<IEntityMetaDataProvider>(typeof(IEntityMetaDataProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityMetaDataInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(EntityMetaDataInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityMetaDataInfo"/>详细信息</returns>
        public EntityMetaDataInfo Save(EntityMetaDataInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
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
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByEntitySchemaId(string entitySchemaId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaId(string entitySchemaId)
        {
            return this.provider.FindAllByEntitySchemaId(entitySchemaId);
        }
        #endregion

        #region 函数:FindAllByEntitySchemaName(string entitySchemaName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entitySchemaName">实体结构名称</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaName(string entitySchemaName)
        {
            return this.provider.FindAllByEntitySchemaName(entitySchemaName);
        }
        #endregion

        #region 函数:FindAllByEntityClassName(string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public IList<EntityMetaDataInfo> FindAllByEntityClassName(string entityClassName)
        {
            return this.provider.FindAllByEntityClassName(entityClassName);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="EntityMetaDataInfo"/></returns>
        public IList<EntityMetaDataInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
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

        // -------------------------------------------------------
        // 业务数据管理
        // -------------------------------------------------------

        #region 函数:GenerateSQL(string entitySchemaId, string sqlType)
        /// <summary>生成实体业务数据的相关SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="sqlType">SQL类型 create | read | update | delete </param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public string GenerateSQL(string entitySchemaId, string sqlType)
        {
            return this.GenerateSQL(entitySchemaId, sqlType, 0);
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
            return this.provider.GenerateSQL(entitySchemaId, sqlType, effectScope);
        }
        #endregion

        #region 函数:ExecuteNonQuerySQL(string entitySchemaId, string sqlType, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="sqlType">SQL类型 create | update | delete </param>
        /// <param name="args">参数</param>
        /// <returns>0 成功 1 失败</returns>
        public int ExecuteNonQuerySQL(string entitySchemaId, string sqlType)
        {
            string SQL = this.GenerateSQL(entitySchemaId, "read");

            return this.ExecuteNonQuerySQL(SQL);
        }
        #endregion

        #region 函数:GenerateSQL(string entitySchemaId, string sqlType)
        /// <summary>生成SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="sqlType">SQL类型 create | read | update | delete </param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        public int ExecuteNonQuerySQL(string entitySchemaId, string sqlType, Dictionary<string, string> args)
        {
            return this.ExecuteNonQuerySQL(entitySchemaId, sqlType, new Dictionary<string, string>());
        }
        #endregion

        #region 函数:ExecuteNonQuerySQL(string sql)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>0 成功 1 失败</returns>
        public int ExecuteNonQuerySQL(string sql)
        {
            return this.ExecuteNonQuerySQL(sql, new Dictionary<string, string>());
        }
        #endregion

        #region 函数:ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>0 成功 1 失败</returns>
        public int ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        {
            return this.provider.ExecuteNonQuerySQL(sql, args);
        }
        #endregion

        #region 函数:ExecuteReaderSQL(string entitySchemaId, int effectScope)
        /// <summary>执行SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="effectScope">作用范围 1 普通字段 | 2 大数据字段</param>
        /// <returns>返回业务数据信息</returns>
        public DataTable ExecuteReaderSQL(string entitySchemaId, int effectScope)
        {
            return this.ExecuteReaderSQL(entitySchemaId, effectScope, new Dictionary<string, string>());
        }
        #endregion

        #region 函数:ExecuteReaderSQL(string entitySchemaId, int effectScope, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="effectScope">作用范围 0 普通字段 | 1 大数据字段</param>
        /// <param name="args">参数</param>
        /// <returns>返回业务数据信息</returns>
        public DataTable ExecuteReaderSQL(string entitySchemaId, int effectScope, Dictionary<string, string> args)
        {
            string SQL = this.GenerateSQL(entitySchemaId, "read", effectScope);

            return this.ExecuteReaderSQL(SQL, args);
        }
        #endregion

        #region 函数:ExecuteReaderSQL(string sql)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回业务数据信息</returns>
        public DataTable ExecuteReaderSQL(string sql)
        {
            return this.ExecuteReaderSQL(sql, new Dictionary<string, string>());
        }
        #endregion

        #region 函数:ExecuteReaderSQL(string sql, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>返回业务数据信息</returns>
        public DataTable ExecuteReaderSQL(string sql, Dictionary<string, string> args)
        {
            return this.provider.ExecuteReaderSQL(sql, args);
        }
        #endregion

        #region 函数:AnalyzeConditionSQL(string sql)
        /// <summary>分析判断条件SQL</summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>判断条件是否成立</returns>
        public bool AnalyzeConditionSQL(string sql)
        {
            return this.AnalyzeConditionSQL(sql, new Dictionary<string, string>());
        }
        #endregion

        #region 函数:AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        /// <summary>分析判断条件SQL</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>判断条件是否成立</returns>
        public bool AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        {
            return this.provider.AnalyzeConditionSQL(sql, args);
        }
        #endregion
    }
}
