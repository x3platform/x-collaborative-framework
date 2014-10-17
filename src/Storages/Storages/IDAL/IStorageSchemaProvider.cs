#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IStorageSchemaProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Storages.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Storages.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Storages.IDAL.IStorageSchemaProvider")]
    public interface IStorageSchemaProvider
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

        #region 函数:Save(StorageSchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="StorageSchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="StorageSchemaInfo"/>详细信息</returns>
        StorageSchemaInfo Save(StorageSchemaInfo param);
        #endregion

        #region 函数:Insert(StorageSchemaInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="StorageSchemaInfo"/>详细信息</param>
        void Insert(StorageSchemaInfo param);
        #endregion

        #region 函数:Update(StorageSchemaInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="StorageSchemaInfo"/>详细信息</param>
        void Update(StorageSchemaInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        StorageSchemaInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByApplicationId(string applicationId)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        StorageSchemaInfo FindOneByApplicationId(string applicationId);
        #endregion

        #region 函数:FindOneByApplicationName(string applicationName)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationName">所属应用名称</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        StorageSchemaInfo FindOneByApplicationName(string applicationName);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        IList<StorageSchemaInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="StorageSchemaInfo"/></returns>
        IList<StorageSchemaInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
