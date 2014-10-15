// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityOperationLogService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Entities.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IBLL.IEntityOperationLogService")]
    public interface IEntityOperationLogService
    {
        // -------------------------------------------------------
        // 保存
        // -------------------------------------------------------

        #region 函数:Save(EntityOperationLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityOperationLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityOperationLogInfo"/>详细信息</returns>
        EntityOperationLogInfo Save(EntityOperationLogInfo param);
        #endregion

        #region 函数:Save(string customTableName, EntityOperationLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityOperationLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityOperationLogInfo"/>详细信息</returns>
        EntityOperationLogInfo Save(string customTableName, EntityOperationLogInfo param);
        #endregion
        
        #region 函数:Delete(string entityId, string entityClassName)
        /// <summary>删除所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        int Delete(string entityId, string entityClassName);
        #endregion

        #region 函数:Delete(string entityId, string entityClassName, int operationType)
        /// <summary>删除所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        int Delete(string entityId, string entityClassName, int operationType);
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName)
        /// <summary>删除所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        int Delete(string customTableName, string entityId, string entityClassName);
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName, int operationType)
        /// <summary>删除所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        int Delete(string customTableName, string entityId, string entityClassName, int operationType);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        IList<EntityOperationLogInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityClickInfo"/>的详细信息</returns>
        IList<EntityOperationLogInfo> FindAll(string customTableName, string whereClause, int length);
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        IList<EntityOperationLogInfo> FindAllByEntityId(string entityId, string entityClassName);
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName, int operationType)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        IList<EntityOperationLogInfo> FindAllByEntityId(string entityId, string entityClassName, int operationType);
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        IList<EntityOperationLogInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName);
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName, int operationType)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        IList<EntityOperationLogInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, int operationType);
        #endregion
    }
}
