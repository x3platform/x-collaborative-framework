//=============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityImplementationProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
//=============================================================================

namespace X3Platform.Entities.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using System.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IDAL.IEntityImplementationProvider")]
    public interface IEntityImplementationProvider
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

        //-------------------------------------------------------
        // 保存 添加 修改 删除
        //-------------------------------------------------------

        #region 函数:Save(string customTableName, EntityImplementationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityImplementationInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityImplementationInfo"/>详细信息</returns>
        EntityImplementationInfo Save(string customTableName, EntityImplementationInfo param);
        #endregion

        #region 函数:Insert(string customTableName, EntityImplementationInfo param)
        /// <summary>添加记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityImplementationInfo"/>详细信息</param>
        void Insert(string customTableName, EntityImplementationInfo param);
        #endregion

        #region 函数:Update(string customTableName, EntityImplementationInfo param)
        /// <summary>修改记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityImplementationInfo"/>详细信息</param>
        void Update(string customTableName, EntityImplementationInfo param);
        #endregion

        #region 函数:Delete(string customTableName, string id)
        /// <summary>删除记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">记录标识</param>
        void Delete(string customTableName, string id);
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName)
        /// <summary>删除记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        void Delete(string customTableName, string entityId, string entityClassName);
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityImplementationInfo"/>的详细信息</returns>
        IList<EntityImplementationInfo> FindAll(string customTableName, string whereClause, int length);
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityImplementationInfo"/>的详细信息</returns>
        IList<EntityImplementationInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName);
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:IsExist(string customTableName, string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string customTableName, string id);
        #endregion
    }
}
