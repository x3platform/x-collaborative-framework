#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IFavoriteService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Favorite.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Plugins.Favorite.Model;
    using X3Platform.CategoryIndexes;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Favorite.IBLL.IFavoriteService")]
    public interface IFavoriteService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FavoriteInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(FavoriteInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="FavoriteInfo"/>详细信息</param>
        /// <returns>实例<see cref="FavoriteInfo"/>详细信息</returns>
        FavoriteInfo Save(FavoriteInfo param);
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
        /// <returns>返回实例<see cref="FavoriteInfo"/>的详细信息</returns>
        FavoriteInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="FavoriteInfo"/>的详细信息</returns>
        IList<FavoriteInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="FavoriteInfo"/>的详细信息</returns>
        IList<FavoriteInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="FavoriteInfo"/>的详细信息</returns>
        IList<FavoriteInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="FavoriteInfo"/></returns>
        IList<FavoriteInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:SetClick(string id)
        /// <summary>
        /// 修改访收藏夹问量
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool SetClick(string id);
        #endregion

        #region 函数:FetchCategoryIndex(string accountIds)
        ///<summary>根据用户标识获取类别索引</summary>
        ///<param name="accountIds">用户标识</param>
        ///<returns>类别索引对象</returns>
        ICategoryIndex FetchCategoryIndex(string accountIds);
        #endregion

        #region 函数:FetchCategoryIndex(string accountIds, string prefixCategoryIndex)
        ///<summary>根据用户标识获取类别索引</summary>
        ///<param name="accountIds">用户标识</param>
        ///<param name="prefixCategoryIndex">类别索引前缀</param>
        ///<returns>类别索引对象</returns>
        ICategoryIndex FetchCategoryIndex(string accountIds, string prefixCategoryIndex);
        #endregion
    }
}
