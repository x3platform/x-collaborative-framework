// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IGeneralRoleProvider.cs
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

namespace X3Platform.Membership.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IGeneralRoleProvider")]
    public interface IGeneralRoleProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(GeneralRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="GeneralRoleInfo"/>详细信息</param>
        /// <returns>实例<see cref="GeneralRoleInfo"/>详细信息</returns>
        GeneralRoleInfo Save(GeneralRoleInfo param);
        #endregion

        #region 函数:Insert(GeneralRoleInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="GeneralRoleInfo"/>详细信息</param>
        void Insert(GeneralRoleInfo param);
        #endregion

        #region 函数:Update(GeneralRoleInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="GeneralRoleInfo"/>详细信息</param>
        void Update(GeneralRoleInfo param);
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
        /// <returns>返回实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        GeneralRoleInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        IList<GeneralRoleInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        IList<GeneralRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId);
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
        /// <returns>返回一个列表实例<see cref="GeneralRoleInfo"/></returns>
        IList<GeneralRoleInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">通用角色名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">通用角色全局名称</param>
        /// <returns>布尔值</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">群组标识</param>
        /// <param name="name">群组名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">通用角色标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetGlobalName(string id, string globalName);
        #endregion

        #region 函数:SyncFromPackPage(GeneralRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">通用角色信息</param>
        int SyncFromPackPage(GeneralRoleInfo param);
        #endregion
    }
}
