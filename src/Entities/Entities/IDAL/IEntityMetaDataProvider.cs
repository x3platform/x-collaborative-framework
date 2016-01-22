namespace X3Platform.Entities.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IDAL.IEntityMetaDataProvider")]
    public interface IEntityMetaDataProvider
    {
        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        void BeginTransaction();
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        void CommitTransaction();
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(EntityMetaDataInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityMetaDataInfo"/>详细信息</returns>
        EntityMetaDataInfo Save(EntityMetaDataInfo param);
        #endregion

        #region 函数:Insert(EntityMetaDataInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        void Insert(EntityMetaDataInfo param);
        #endregion

        #region 函数:Update(EntityMetaDataInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="EntityMetaDataInfo"/>详细信息</param>
        void Update(EntityMetaDataInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        EntityMetaDataInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        IList<EntityMetaDataInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByEntitySchemaId(string entitySchemaId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        IList<EntityMetaDataInfo> FindAllByEntitySchemaId(string entitySchemaId);
        #endregion

        #region 函数:FindAllByEntitySchemaName(string entitySchemaName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entitySchemaName">实体结构名称</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        IList<EntityMetaDataInfo> FindAllByEntitySchemaName(string entitySchemaName);
        #endregion

        #region 函数:FindAllByEntityClassName(string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        IList<EntityMetaDataInfo> FindAllByEntityClassName(string entityClassName);
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
        /// <returns>返回一个列表实例<see cref="EntityMetaDataInfo"/></returns> 
        IList<EntityMetaDataInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        // -------------------------------------------------------
        // 业务数据管理
        // -------------------------------------------------------

        #region 函数:GenerateSQL(string entitySchemaId, string sqlType, int effectScope)
        /// <summary>生成实体业务数据的相关SQL语句</summary>
        /// <param name="entitySchemaId">实体结构标识</param>
        /// <param name="sqlType">SQL类型 create | read | update | delete </param>
        /// <param name="effectScope">作用范围 0 普通字段 | 1 大数据字段</param>
        /// <returns>返回所有实例<see cref="EntityMetaDataInfo"/>的详细信息</returns>
        string GenerateSQL(string entitySchemaId, string sqlType, int effectScope);
        #endregion

        #region 函数:ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>0 成功 1 失败</returns>
        int ExecuteNonQuerySQL(string sql, Dictionary<string, string> args);
        #endregion

        #region 函数:ExecuteSQL(string sql, Dictionary<string, string> args)
        /// <summary>执行SQL语句</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>返回业务数据信息</returns>
        DataTable ExecuteReaderSQL(string sql, Dictionary<string, string> args);
        #endregion

        #region 函数:AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        /// <summary>分析判断条件SQL</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>判断条件是否成立</returns>
        bool AnalyzeConditionSQL(string sql, Dictionary<string, string> args);
        #endregion
    }
}
