#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityDocObjectProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IDAL.IEntityDocObjectProvider")]
    public interface IEntityDocObjectProvider
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
