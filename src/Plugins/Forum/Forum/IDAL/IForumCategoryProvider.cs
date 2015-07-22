#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :IForumCategoryProvider.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Plugins.Forum.Model;
    using X3Platform.CategoryIndexes;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Forum.IDAL.IForumCategoryProvider")]
    public interface IForumCategoryProvider
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
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ForumCategoryInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
        /// <returns>实例<see cref="ForumCategoryInfo"/>详细信息</returns>
        ForumCategoryInfo Save(ForumCategoryInfo param);
        #endregion

        #region 函数:Insert(ForumCategoryInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
        void Insert(ForumCategoryInfo param);
        #endregion

        #region 函数:Update(ForumCategoryInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
        void Update(ForumCategoryInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
        ForumCategoryInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">分类索引</param>
        /// <returns>返回实例<see cref="DengBaoCategoryInfo"/>的详细信息</returns>
        ForumCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
        IList<ForumCategoryInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="ForumCategoryInfo"/></returns>
        IList<ForumCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ForumCategoryQueryInfo"/></returns>
        IList<ForumCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistByParent(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExistByParent(string id);
        #endregion

        #region 函数:FetchCategoryIndex(string whereClause)
        /// <summary>根据分类状态获取分类索引</summary>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="applicationTag">程序标识</param>
        /// <returns>分类索引对象</returns>
        ICategoryIndex FetchCategoryIndex(string whereClause);
        #endregion

        #region 函数:IsCategoryAdministrator(string categoryId)
        /// <summary>
        /// 判断当前用户是否是该板块的管理员
        /// <param name="categoryId">版块编号</param>
        /// <returns>操作结果</returns>
        bool IsCategoryAdministrator(string categoryId);
        #endregion

        #region 函数:IsCategoryAuthority(string categoryId)
        /// <summary>
        /// 根据版块标识查询当前用户是否拥有该权限
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        bool IsCategoryAuthority(string categoryId);
        #endregion

        #region 函数:GetCategoryAdminName(string categoryId)
        /// <summary>
        /// 查询版主名称
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="applicationTag"></param>
        /// <returns></returns>
        string GetCategoryAdminName(string categoryId);
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
