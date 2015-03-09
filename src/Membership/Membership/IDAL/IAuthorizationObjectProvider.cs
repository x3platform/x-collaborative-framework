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

namespace X3Platform.Membership.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform;
    using X3Platform.Spring;
    using X3Platform.Membership.Scope;
    using X3Platform.Membership.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IAuthorizationObjectProvider")]
    public interface IAuthorizationObjectProvider
    {
        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        void BeginTransaction();
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        void CommitTransaction();
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>查询授权对象信息</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        DataTable Filter(int startIndex, int pageSize, DataQuery query, out int rowCount);
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
        /// <summary>新增实体对象的权限信息</summary>
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
        /// <summary>查询应用的权限信息</summary>
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
