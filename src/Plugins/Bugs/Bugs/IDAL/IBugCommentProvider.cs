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

namespace X3Platform.Plugins.Bugs.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IDAL.IBugCommentProvider")]
    public interface IBugCommentProvider
	{
		// -------------------------------------------------------
		// 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">BugCommentInfo Id号</param>
		/// <returns>返回一个 BugCommentInfo 实例的详细信息</returns>
        BugCommentInfo FindOne(string id);
		#endregion

		#region 函数:FindAll(string whereClause,int length)
		/// <summary>查询所有相关记录</summary>
		/// <param name="whereClause">SQL 查询条件</param>
		/// <param name="length">条数</param>
		/// <returns>返回所有 BugCommentInfo 实例的详细信息</returns>
        IList<BugCommentInfo> FindAll(string whereClause, int length);
		#endregion

		// -------------------------------------------------------
        // 保存 添加 修改 删除
		// -------------------------------------------------------

        #region 函数:Save(BugCommentInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">BugCommentInfo 实例详细信息</param>
        /// <returns>BugCommentInfo 实例详细信息</returns>
        BugCommentInfo Save(BugCommentInfo param);
        #endregion

		#region 函数:Insert(BugCommentInfo param)
		/// <summary>添加记录</summary>
		/// <param name="param">BugCommentInfo 实例的详细信息</param>
		void Insert(BugCommentInfo param);
		#endregion

		#region 函数:Update(BugCommentInfo param)
		/// <summary>修改记录</summary>
		/// <param name="param">BugCommentInfo 实例的详细信息</param>
		void Update(BugCommentInfo param);
		#endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开</param>
		void Delete(string ids);
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
        /// <returns>返回一个列表实例</returns> 
        IList<BugCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
		/// <param name="param">BugCommentInfo 实例详细信息</param>
		/// <returns>布尔值</returns>
		bool IsExist(string id);
		#endregion

	}
}
