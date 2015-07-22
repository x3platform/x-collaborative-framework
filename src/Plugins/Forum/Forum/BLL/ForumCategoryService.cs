#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumCategoryService.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;

  using X3Platform.Apps;
  using X3Platform.CategoryIndexes;
  using X3Platform.Membership;
  using X3Platform.Membership.Model;
  using X3Platform.Security.Authority;
  using X3Platform.Spring;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IBLL;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Membership.Scope;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  public class ForumCategoryService : IForumCategoryService
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>数据提供器</summary>
    private IForumCategoryProvider provider = null;

    #region 构造函数:ForumCategoryService()
    /// <summary>构造函数</summary>
    public ForumCategoryService()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IForumCategoryProvider>(typeof(IForumCategoryProvider));
    }
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(ForumCategoryInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumCategoryInfo"/>详细信息</returns>
    public ForumCategoryInfo Save(ForumCategoryInfo param)
    {
      return this.provider.Save(param);
    }
    #endregion

    #region 函数:Delete(string id)
    /// <summary>删除记录</summary>
    /// <param name="id">实例的标识</param>
    public void Delete(string id)
    {
      this.provider.Delete(id);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
    public ForumCategoryInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region 函数:FindOneByCategoryIndex(string categoryIndex)
    /// <summary>查询某条记录</summary>
    /// <param name="categoryIndex">分类索引</param>
    /// <returns>返回实例<see cref="DengBaoCategoryInfo"/>的详细信息</returns>
    public ForumCategoryInfo FindOneByCategoryIndex(string categoryIndex)
    {
      return provider.FindOneByCategoryIndex(categoryIndex);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
    public IList<ForumCategoryInfo> FindAll()
    {
      return this.FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
    public IList<ForumCategoryInfo> FindAll(string whereClause)
    {
      return this.FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
    public IList<ForumCategoryInfo> FindAll(string whereClause, int length)
    {
      return this.provider.FindAll(whereClause, length);
    }
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
    public IList<ForumCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例<see cref="ForumCategoryQueryInfo"/></returns>
    public IList<ForumCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录.</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      return this.provider.IsExist(id);
    }
    #endregion

    #region 函数:FetchCategoryIndex(string isStatus)
    ///<summary>根据分类状态获取分类索引</summary>
    ///<param name="isStatus">状态</param>
    ///<returns>分类索引对象</returns>
    public ICategoryIndex FetchCategoryIndex(string isStatus)
    {
      string whereClause = string.Empty;
      if (isStatus == "1")
      {
        whereClause = " Status = 1 ";
      }
      return provider.FetchCategoryIndex(this.BindAuthorizationScopeSQL(whereClause));
    }
    #endregion

    #region 函数:IsAnonymous(string id);
    /// <summary>
    /// 查询是否允许匿名发布
    /// </summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsAnonymous(string id)
    {
      bool result = true;

      ForumCategoryInfo categoryinfo = this.FindOne(id);
      if (categoryinfo.Anonymous == 0)
      {
        result = false;
      }
      return result;
    }
    #endregion

    #region 函数:IsExistByParent(string id)
    /// <summary>查询是否存在相关的记录.</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExistByParent(string id)
    {
      return provider.IsExistByParent(id);
    }
    #endregion

    #region 函数:IsCategoryAdministrator(string categoryId)
    /// <summary>判断当前用户是否是该板块的管理员</summary>
    /// <param name="categoryId">版块编号</param>
    /// <returns>操作结果</returns>
    public bool IsCategoryAdministrator(string categoryId)
    {
      return this.provider.IsCategoryAdministrator(categoryId);
    }
    #endregion

    #region 函数:IsCategoryAuthority(string categoryId)
    /// <summary>根据版块标识查询当前用户是否拥有该权限</summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public bool IsCategoryAuthority(string categoryId)
    {
      return this.provider.IsCategoryAuthority(categoryId);
    }
    #endregion

    #region 函数:GetCategoryAdminName(string categoryId)
    /// <summary>
    /// 查询版主名称
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="applicationTag"></param>
    /// <returns></returns>
    public string GetCategoryAdminName(string categoryId)
    {
      return this.provider.GetCategoryAdminName(categoryId);
    }
    #endregion

    // -------------------------------------------------------
    // 授权范围管理
    // -------------------------------------------------------

    #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
    /// <summary>判断用户是否拥应用菜单有权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <param name="account">帐号</param>
    /// <returns>布尔值</returns>
    public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
    {
      return this.provider.HasAuthority(entityId, authorityName, account);
    }
    #endregion

    #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    /// <summary>配置应用菜单的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <param name="scopeText">权限范围的文本</param>
    public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    {
      this.provider.BindAuthorizationScopeObjects(entityId, authorityName, scopeText);
    }
    #endregion

    #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
    /// <summary>查询应用菜单的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <returns></returns>
    public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
    {
      return this.provider.GetAuthorizationScopeObjects(entityId, authorityName);
    }
    #endregion

    // -------------------------------------------------------
    // 权限
    // -------------------------------------------------------

    #region 私有函数:BindAuthorizationScopeSQL(string whereClause)
    /// <summary>绑定SQL查询条件</summary>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <returns></returns>
    private string BindAuthorizationScopeSQL(string whereClause)
    {
      // 验证管理员身份
      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, ForumConfiguration.ApplicationName))
      {
        IAccountInfo account = KernelContext.Current.User;
        // string tableName = ForumUtility.ToDataTablePrefix(applicationTag) + "_Category_Scope";
        string tableName = "tb_Forum_Category_Scope";
        string scope = MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeEntitySQL(
            tableName,
            account.Id,
            ContactType.Default,
            "00000000-0000-0000-0000-000000000003,00000000-0000-0000-0000-000000000001");

        scope = @"( T.Id IN ( " + scope + " ) ) ";

        if (whereClause.IndexOf(scope) == -1)
        {
          whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
        }
      }

      return whereClause;
    }
    #endregion
  }
}
