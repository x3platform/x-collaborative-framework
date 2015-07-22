// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IForumCommentProvider.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Plugins.Forum.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Plugins.Forum.Model;
  using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Forum.IDAL.IForumCommentProvider")]
    public interface IForumCommentProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ForumCommentInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
        /// <returns>实例<see cref="ForumCommentInfo"/>详细信息</returns>
        ForumCommentInfo Save(ForumCommentInfo param);
        #endregion

        #region 函数:Insert(ForumCommentInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
        void Insert(ForumCommentInfo param);
        #endregion

        #region 函数:Update(ForumCommentInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
        void Update(ForumCommentInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        #region 函数:DeleteByTheadId(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void DeleteByTheadId(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ForumCommentInfo"/>的详细信息</returns>
        ForumCommentInfo FindOne(string id);
        #endregion

        #region 函数:FindOne(string id,string theadId)
        /// <summary>
        /// 查询回帖信息
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="threaId">主题编号</param>
        /// <returns>返回信息</returns>
        ForumCommentInfo FindOne(string id, string threaId);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ForumCommentInfo"/>的详细信息</returns>
        IList<ForumCommentInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ForumCommentInfo"/></returns>
        IList<ForumCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ForumCommentQueryInfo"/></returns>
        IList<ForumCommentQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetCommentCount(string id)
        /// <summary>
        /// 根据主题查询回帖数
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>回帖数</returns>
        int GetCommentCount(string id);
        #endregion

        #region 函数:GetLastComment(string id)
        /// <summary>
        /// 根据帖子编号查询最后回帖信息
        /// </summary>
        /// <param name="id">帖子编号</param>
        /// <returns>最后回帖信息</returns>
        string GetLastComment(string id);
        #endregion
    }
}
