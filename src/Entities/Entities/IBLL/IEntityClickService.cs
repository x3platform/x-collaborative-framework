namespace X3Platform.Entities.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IBLL.IEntityClickService")]
    public interface IEntityClickService
    {
        // -------------------------------------------------------
        // 保存
        // -------------------------------------------------------

        #region 函数:Save(IEntityClickInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityClickInfo"/>详细信息</returns>
        IEntityClickInfo Save(IEntityClickInfo param);
        #endregion

        #region 函数:Save(string customTableName, IEntityClickInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityClickInfo"/>详细信息</returns>
        IEntityClickInfo Save(string customTableName, IEntityClickInfo param);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        IList<IEntityClickInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        IList<IEntityClickInfo> FindAll(string customTableName, string whereClause, int length);
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        IList<IEntityClickInfo> FindAllByEntityId(string entityId, string entityClassName);
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName);
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="mapper">数据结果映射器</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string entityId, string entityClassName, string accountId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string entityId, string entityClassName, string accountId);
        #endregion

        #region 函数:IsExist(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string customTableName, string entityId, string entityClassName, string accountId);
        #endregion

        #region 函数:Increment(string entityId, string entityClassName, string accountId)
        /// <summary>自增实体数据的点击数</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        int Increment(string entityId, string entityClassName, string accountId);
        #endregion

        #region 函数:Increment(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>自增实体数据的点击数</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        int Increment(string customTableName, string entityId, string entityClassName, string accountId);
        #endregion

        #region 函数:CalculateClickCount(string entityId, string entityClassName)
        /// <summary>计算实体数据的点击数</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>点击数</returns>
        int CalculateClickCount(string entityId, string entityClassName);
        #endregion

        #region 函数:CalculateClickCount(string tableName, string entityId, string entityClassName)
        /// <summary>计算实体数据的点击数</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>点击数</returns>
        int CalculateClickCount(string customTableName, string entityId, string entityClassName);
        #endregion
    }
}
