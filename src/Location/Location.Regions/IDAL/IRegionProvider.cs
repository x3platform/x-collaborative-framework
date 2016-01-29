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

namespace X3Platform.Location.Regions.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Location.Regions.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Location.Regions.IDAL.IRegionProvider")]
    public interface IRegionProvider
    {
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">RegionInfo Id号</param>
        /// <returns>返回一个 实例<see cref="RegionInfo"/>的详细信息</returns>
        RegionInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有 实例<see cref="RegionInfo"/>的详细信息</returns>
        IList<RegionInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(RegionInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>详细信息</param>
        /// <returns>实例<see cref="RegionInfo"/>详细信息</returns>
        RegionInfo Save(RegionInfo param);
        #endregion

        #region 函数:Insert(RegionInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>的详细信息</param>
        void Insert(RegionInfo param);
        #endregion

        #region 函数:Update(RegionInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>的详细信息</param>
        void Update(RegionInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
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
        /// <param name="param">实例<see cref="RegionInfo"/>详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
