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
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CategoryIndexes;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Web.Component.Combobox;

    using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IBLL.IBugCategoryService")]
    public interface IBugCategoryService
    {
        #region 索引:this[string id]
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BugCategoryInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(BugCategoryInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例详细信息</param>
        /// <returns></returns>
        BugCategoryInfo Save(BugCategoryInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>逻辑删除记录</summary>
        /// <param name="id">实例的标识</param>
        int Delete(string id);
        #endregion

        #region 函数:CanDelete(string id)
        /// <summary>判断类别是否能够被物理删除</summary>
        /// <param name="id">实例的标识</param>
        /// <returns></returns>
        bool CanDelete(string id);
        #endregion

        #region 函数:Remove(string id)
        /// <summary>物理删除记录</summary>
        /// <param name="id">实例的类别标识</param>
        int Remove(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>
        /// 查询某条记录
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        BugCategoryInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">类别索引</param>
        /// <returns>返回实例<see cref="BugCategoryInfo"/>的详细信息</returns>
        BugCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region 函数:FindAll()
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <returns></returns>
        IList<BugCategoryInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns></returns>
        IList<BugCategoryInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns></returns>
        IList<BugCategoryInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllQueryObject(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="BugCategoryQueryInfo"/>的详细信息</returns>
        IList<BugCategoryQueryInfo> FindAllQueryObject(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计.</param>
        /// <param name="pageSize">每页显示的数据条数</param>
        /// <param name="whereClause">WHERE 查询条件.</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <param name="rowCount">如何条件的数据总行数</param>
        /// <returns>返回一个列表实例<see cref="BugCategoryInfo"/></returns>
        IList<BugCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="BugCategoryQueryInfo"/></returns>
        IList<BugCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>
        /// 查询是否存在相关的记录
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:SetStatus(string id, int status)
        /// <summary>
        /// 停用/启用类别
        /// </summary>
        /// <param name="id">新闻类别标识</param>
        /// <param name="status">1将停用的类别启用，0将在用的类别停用</param>
        /// <returns></returns>
        bool SetStatus(string id, int status);
        #endregion

        #region 函数:GetComboboxByWhereClause(string whereClause, string selectedValue)
        /// <summary>获取拉框数据源</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="selectedValue">默认选择的值</param>
        /// <returns></returns>
        IList<ComboboxItem> GetComboboxByWhereClause(string whereClause, string selectedValue);
        #endregion

        #region 函数:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        /// <summary>获取异步生成的树</summary>
        /// <param name="treeName">树</param>
        /// <param name="parentId">父级节点标识</param>
        /// <param name="url">链接地址</param>
        /// <param name="enabledLeafClick">只允许点击叶子节点</param>
        /// <param name="elevatedPrivileges">提升权限</param>
        /// <returns>布尔值</returns>
        DynamicTreeView GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges);
        #endregion

        #region 函数:GetDynamicTreeNodes(string searchPath, string whereClause)
        /// <summary>获取树的节点列表</summary>
        /// <param name="searchPath">查询路径</param>
        /// <param name="whereClause">SQL条件</param>
        /// <returns>树的节点</returns>
        IList<DynamicTreeNode> GetDynamicTreeNodes(string searchPath, string whereClause);
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>判断用户是否拥有实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号信息</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(string entityId, string authorityName, IAccountInfo account);
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>绑定实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName);
        #endregion
    }
}
