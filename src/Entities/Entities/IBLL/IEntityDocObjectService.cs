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
    [SpringObject("X3Platform.Entities.IBLL.IEntityDocObjectService")]
    public interface IEntityDocObjectService
    {
        // -------------------------------------------------------
        // 保存
        // -------------------------------------------------------

        #region 函数:Save(string customTableName, IEntityDocObjectInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="IEntityDocObjectInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityDocObjectInfo"/>详细信息</returns>
        IEntityDocObjectInfo Save(string customTableName, IEntityDocObjectInfo param);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        IList<IEntityDocObjectInfo> FindAll(string customTableName, string whereClause, int length);
        #endregion

        #region 函数:FindAllByDocToken(string customTableName, string docToken)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="docToken">文档全局标识</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken);
        #endregion

        #region 函数:FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="docToken">文档全局标识</param>
        /// <param name="mapper">数据结果映射器</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string customTableName, string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string customTableName, string id);
        #endregion
    }
}
