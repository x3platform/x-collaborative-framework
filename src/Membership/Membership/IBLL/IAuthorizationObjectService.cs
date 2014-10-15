#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    
    using X3Platform.Data;
    using X3Platform.Spring;
    using X3Platform.Membership.Scope;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IAuthorizationObjectService")]
    public interface IAuthorizationObjectService
    {
        #region 索引:this[string authorizationObjectType, string authorizationObjectId]
        /// <summary>索引</summary>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns></returns>
        IAuthorizationObject this[string authorizationObjectType, string authorizationObjectId] { get; }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string authorizationObjectType, string authorizationObjectId)
        /// <summary>查询某条授权对象信息</summary>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>返回一个<see cref="IAuthorizationObject"/>实例的详细信息</returns>
        IAuthorizationObject FindOne(string authorizationObjectType, string authorizationObjectId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:Filter(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>查询授权对象信息</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        DataTable Filter(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExistName(string authorizationObjectName)
        /// <summary>检测是否存在相关的授权对象，授权对象名称不能重复。</summary>
        /// <param name="authorizationObjectName">授权对象名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string authorizationObjectName);
        #endregion

        #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectName)
        /// <summary>检测是否存在相关的授权对象，授权对象名称不能重复。</summary>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectName">授权对象名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string authorizationObjectType, string authorizationObjectName);
        #endregion

        #region 函数:GetInstantiatedAuthorizationObjects(string corporationId, IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        /// <summary>获取实例化的授权对象</summary>
        /// <param name="corporationId">公司标识</param>
        /// <param name="authorizationScopeObjects">授权范围对象</param>
        /// <returns></returns>
        IList<IAuthorizationObject> GetInstantiatedAuthorizationObjects(string corporationId, IList<MembershipAuthorizationScopeObject> authorizationScopeObjects);
        #endregion

        #region 函数:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号信息</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account);
        #endregion

        #region 函数:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId);
        #endregion

        #region 函数:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号信息</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account);
        #endregion

        #region 函数:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId);
        #endregion

        #region 函数:AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>添加实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText);
        #endregion

        #region 函数:AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>添加实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText);
        #endregion

        #region 函数:RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        void RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        void RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText);
        #endregion

        #region 函数:BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:GetAuthorizationScopeObjectText(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的文本</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        string GetAuthorizationScopeObjectText(string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:GetAuthorizationScopeObjectText(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的文本</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        string GetAuthorizationScopeObjectText(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:GetAuthorizationScopeObjectView(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的视图</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        string GetAuthorizationScopeObjectView(string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:GetAuthorizationScopeObjectView(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的视图</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        string GetAuthorizationScopeObjectView(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName);
        #endregion

        #region 函数:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds)
        /// <summary>获取实体对象标识SQL语句</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="authorityIds">权限标识</param>
        /// <param name="accountId">用户标识</param>
        /// <param name="contactType">联系人对象</param>
        /// <returns></returns>
        string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds);
        #endregion

        #region 函数:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        /// <summary>获取实体对象标识SQL语句</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="accountId">用户标识</param>
        /// <param name="contactType">联系人对象</param>
        /// <param name="authorityIds">权限标识</param>
        /// <param name="entityIdDataColumnName">实体类标识的数据列名</param>
        /// <param name="entityClassNameDataColumnName">实体类名称的数据列名</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns></returns>
        string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName);
        #endregion
 }
}
