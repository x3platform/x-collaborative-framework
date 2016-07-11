// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ICatalogProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Model;
using X3Platform.Data;

namespace X3Platform.Membership.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.ICatalogProvider")]
    public interface ICatalogProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(CatalogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="CatalogInfo"/>详细信息</param>
        /// <returns>实例<see cref="CatalogInfo"/>详细信息</returns>
        CatalogInfo Save(CatalogInfo param);
        #endregion

        #region 函数:Insert(CatalogInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="CatalogInfo"/>详细信息</param>
        void Insert(CatalogInfo param);
        #endregion

        #region 函数:Update(CatalogInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="CatalogInfo"/>详细信息</param>
        void Update(CatalogInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="CatalogInfo"/>的详细信息</returns>
        CatalogInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="CatalogInfo"/>的详细信息</returns>
        IList<CatalogInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="CatalogInfo"/></returns>
        IList<CatalogInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
