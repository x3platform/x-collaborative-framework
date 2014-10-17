#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IStorageNodeService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Storages.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Storages.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Storages.IBLL.IStorageNodeService")]
    public interface IStorageNodeService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IStorageNode this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IStorageNode param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStorageNode"/>详细信息</param>
        /// <returns>实例<see cref="IStorageNode"/>详细信息</returns>
        IStorageNode Save(IStorageNode param);
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
        /// <returns>返回实例<see cref="IStorageNode"/>的详细信息</returns>
        IStorageNode FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IStorageNode"/>的详细信息</returns>
        IList<IStorageNode> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IStorageNode"/>的详细信息</returns>
        IList<IStorageNode> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStorageNode"/>的详细信息</returns>
        IList<IStorageNode> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllBySchemaId(string schemaId)
        /// <summary>根据存储架构名称查询所有相关记录</summary>
        /// <param name="schemaId">存储架构标识</param>
        /// <returns>返回所有实例<see cref="IStorageNode"/>的详细信息</returns>
        IList<IStorageNode> FindAllBySchemaId(string schemaId);
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
        /// <returns>返回一个列表实例<see cref="IStorageNode"/></returns>
        IList<IStorageNode> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
