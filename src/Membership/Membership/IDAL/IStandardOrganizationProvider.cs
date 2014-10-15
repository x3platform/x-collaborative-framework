// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardOrganizationProvider.cs
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
    [SpringObject("X3Platform.Membership.IDAL.IStandardOrganizationProvider")]
    public interface IStandardOrganizationProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IStandardOrganizationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardOrganizationInfo"/>详细信息</returns>
        IStandardOrganizationInfo Save(IStandardOrganizationInfo param);
        #endregion

        #region 函数:Insert(IStandardOrganizationInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationInfo"/>详细信息</param>
        void Insert(IStandardOrganizationInfo param);
        #endregion

        #region 函数:Update(IStandardOrganizationInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationInfo"/>详细信息</param>
        void Update(IStandardOrganizationInfo param);
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
        /// <returns>返回实例<see cref="IStandardOrganizationInfo"/>的详细信息</returns>
        IStandardOrganizationInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardOrganizationInfo"/>的详细信息</returns>
        IList<IStandardOrganizationInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        IList<IStandardOrganizationInfo> FindAllByParentId(string parentId);
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
        /// <returns>返回一个列表实例<see cref="IStandardOrganizationInfo"/></returns>
        IList<IStandardOrganizationInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">标准组织名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">标准组织单位全局名称</param>
        /// <returns>布尔值</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标准组织标识</param>
        /// <param name="name">标准组织名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">标准组织标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetGlobalName(string id, string globalName);
        #endregion

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标准组织标识</param>
        /// <param name="parentId">父级组织标识</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetParentId(string id, string parentId);
        #endregion

        #region 函数:SyncFromPackPage(IStandardOrganizationInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">组织信息</param>
        int SyncFromPackPage(IStandardOrganizationInfo param);
        #endregion
    }
}
