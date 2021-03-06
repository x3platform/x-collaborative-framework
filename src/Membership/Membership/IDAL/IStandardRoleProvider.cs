﻿// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardRoleProvider.cs
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
    [SpringObject("X3Platform.Membership.IDAL.IStandardRoleProvider")]
    public interface IStandardRoleProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IStandardRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardRoleInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardRoleInfo"/>详细信息</returns>
        IStandardRoleInfo Save(IStandardRoleInfo param);
        #endregion

        #region 函数:Insert(IStandardRoleInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IStandardRoleInfo"/>详细信息</param>
        void Insert(IStandardRoleInfo param);
        #endregion

        #region 函数:Update(IStandardRoleInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IStandardRoleInfo"/>详细信息</param>
        void Update(IStandardRoleInfo param);
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
        /// <returns>返回实例<see cref="IStandardRoleInfo"/>的详细信息</returns>
        IStandardRoleInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardRoleInfo"/>的详细信息</returns>
        IList<IStandardRoleInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        IList<IStandardRoleInfo> FindAllByParentId(string parentId);
        #endregion

        #region 函数:FindAllByStandardOrganizationUnitId(string standardOrganizationUnitId)
        /// <summary>递归查询某个标准组织下面所有标准角色</summary>
        /// <param name="standardOrganizationUnitId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IStandardRoleInfo> FindAllByStandardOrganizationUnitId(string standardOrganizationUnitId);
        #endregion

        #region 函数:FindAllByType(int standardRoleType)
        /// <summary>根据标准角色类型查询所有相关记录</summary>
        /// <param name="standardRoleType">标准角色类型</param>
        /// <returns>返回所有实例<see cref="IStandardRoleInfo"/>的详细信息</returns>
        IList<IStandardRoleInfo> FindAllByType(int standardRoleType);
        #endregion

        #region 函数:FindAllByCatalogItemId(string CatalogItemId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="CatalogItemId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        IList<IStandardRoleInfo> FindAllByCatalogItemId(string CatalogItemId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        IList<IStandardRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">标准角色名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标准角色标识</param>
        /// <param name="name">标准角色名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:SyncFromPackPage(IStandardRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">标准角色信息</param>
        int SyncFromPackPage(IStandardRoleInfo param);
        #endregion

        #region 函数:GetKeyStandardRoles(int type)
        /// <summary>获取所有关键标准角色</summary>
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        IList<IStandardRoleInfo> GetKeyStandardRoles();
        #endregion

        #region 函数:GetKeyStandardRoles(int standardRoleType)
        /// <summary>获取某种关键标准角色</summary>
        /// <param name="standardRoleType">标准角色类型</param>
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        IList<IStandardRoleInfo> GetKeyStandardRoles(int standardRoleType);
        #endregion

    }
}
