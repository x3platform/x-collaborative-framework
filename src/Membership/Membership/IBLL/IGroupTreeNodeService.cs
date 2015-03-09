// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IGroupTreeNodeService.cs
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

namespace X3Platform.Membership.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IGroupTreeNodeService")]
    public interface IGroupTreeNodeService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GroupTreeNodeInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(GroupTreeNodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="GroupTreeNodeInfo"/>详细信息</param>
        /// <returns>实例<see cref="GroupTreeNodeInfo"/>详细信息</returns>
        GroupTreeNodeInfo Save(GroupTreeNodeInfo param);
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
        /// <returns>返回实例<see cref="GroupTreeNodeInfo"/>的详细信息</returns>
        GroupTreeNodeInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="GroupTreeNodeInfo"/>的详细信息</returns>
        IList<GroupTreeNodeInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="GroupTreeNodeInfo"/>的详细信息</returns>
        IList<GroupTreeNodeInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="GroupTreeNodeInfo"/>的详细信息</returns>
        IList<GroupTreeNodeInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父节点标识</param>
        /// <returns>返回所有实例<see cref="GroupTreeNodeInfo"/>的详细信息</returns>
        IList<GroupTreeNodeInfo> FindAllByParentId(string parentId);
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
        /// <returns>返回一个列表实例<see cref="GroupTreeNodeInfo"/></returns>
        IList<GroupTreeNodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetGroupTreeNodePathByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>根据分组类别节点标识计算类别的全路径</summary>
        /// <param name="groupTreeNodeId">分组类别节点标识</param>
        /// <returns></returns>
        string GetGroupTreeNodePathByGroupTreeNodeId(string groupTreeNodeId);
        #endregion

        #region 函数:GetActiveDirectoryOUPathByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>根据分组类别节点标识计算 Active Directory OU 路径</summary>
        /// <param name="groupTreeNodeId">分组类别节点标识</param>
        /// <returns></returns>
        string GetActiveDirectoryOUPathByGroupTreeNodeId(string groupTreeNodeId);
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion
    }
}
