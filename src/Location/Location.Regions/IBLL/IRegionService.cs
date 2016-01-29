#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Location.Regions.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Location.Regions.Model;
    using X3Platform.Data;
    using X3Platform.CategoryIndexes;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Location.Regions.IBLL.IRegionService")]
    public interface IRegionService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        RegionInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(RegionInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>详细信息</param>
        /// <returns>实例<see cref="RegionInfo"/>详细信息</returns>
        RegionInfo Save(RegionInfo param);
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
        /// <returns>返回一个 实例<see cref="RegionInfo"/>的详细信息</returns>
        RegionInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 实例<see cref="RegionInfo"/>的详细信息</returns>
        IList<RegionInfo> FindAll();
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
        /// <returns>返回一个列表实例</returns> 
        IList<RegionInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">实例<see cref="RegionInfo"/>详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick)
        /// <summary>获取异步生成的树</summary>
        /// <param name="treeName">树</param>
        /// <param name="parentId">父级节点标识</param>
        /// <param name="url">链接地址</param>
        /// <param name="enabledLeafClick">只允许点击叶子节点</param>
        /// <returns>树</returns>
        DynamicTreeView GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick);
        #endregion
    }
}
