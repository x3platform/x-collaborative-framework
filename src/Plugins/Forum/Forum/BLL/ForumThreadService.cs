// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ForumThreadService.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Plugins.Forum.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Data;

  using X3Platform.Apps;
  using X3Platform.AttachmentStorage;
  using X3Platform.Membership;
  using X3Platform.Membership.Model;
  using X3Platform.Security.Authority;
  using X3Platform.Spring;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IBLL;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  public class ForumThreadService : IForumThreadService
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>数据提供器</summary>
    private IForumThreadProvider provider = null;

    #region 构造函数:ForumThreadService()
    /// <summary>构造函数</summary>
    public ForumThreadService()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IForumThreadProvider>(typeof(IForumThreadProvider));
    }
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(ForumThreadInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumThreadInfo"/>详细信息</returns>
    public ForumThreadInfo Save(ForumThreadInfo param)
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

      ForumContext.Instance.ForumCommentService.DeleteByTheadId(id);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public ForumThreadInfo FindOne(string id)
    {
      // 查询信息
      return this.provider.FindOne(id);
    }
    #endregion

    #region 函数:FindOneByCode(string code)
    /// <summary>查询某条记录</summary>
    /// <param name="code">编号</param>
    /// <returns>实例<see cref="ForumThreadInfo"/>详细信息</returns>
    public ForumThreadInfo FindOneByCode(string code)
    {
      return this.provider.FindOneByCode(code);
    }
    #endregion

    #region 函数:FindOneByNew(string accountId)
    /// <summary>
    /// 查询用户最新发帖
    /// </summary>
    /// <param name="accountId">用户标识</param>
    /// <returns>返回实例</returns>
    public ForumThreadInfo FindOneByNew(string accountId)
    {
      return this.provider.FindOneByNew(accountId);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public IList<ForumThreadInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public IList<ForumThreadInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public IList<ForumThreadInfo> FindAll(string whereClause, int length)
    {
      return this.provider.FindAll(this.BindAuthorizationScopeSQL(whereClause), length);
    }
    #endregion

    #region 函数:FindAllQueryObject(DataQuery query)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public IList<ForumThreadQueryInfo> FindAllQueryObject(DataQuery query)
    {
      return this.provider.FindAllQueryObject(query);
      // return this.provider.FindAllQueryObject(this.BindAuthorizationScopeSQL(whereClause), length);
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
    /// <returns>返回一个列表实例<see cref="ForumThreadInfo"/></returns>
    public IList<ForumThreadInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      // return this.provider.GetPaging(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
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
    /// <returns>返回一个列表实例<see cref="ForumThreadInfo"/></returns>
    public IList<ForumThreadQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
      // return this.provider.GetQueryObjectPaging(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
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

    #region 函数:IsExistByCategory(string id)
    /// <summary>
    /// 根据版块查询是否存在相关记录
    /// </summary>
    /// <param name="?">版块表示</param>
    /// <returns>布尔值</returns>
    public bool IsExistByCategory(string id)
    {
      return this.provider.IsExistByCategory(id);
    }
    #endregion

    #region 函数:GetTheadCount(string id)
    /// <summary>
    /// 根据用户查询发帖数
    /// </summary>
    /// <param name="id">标识</param>
    /// <returns>回帖数</returns>
    public int GetTheadCount(string id)
    {
      return this.provider.GetTheadCount(id);
    }
    #endregion

    #region 函数:GetStorageList(string id,string className)
    /// <summary>
    /// 根据实体编号实体类名查询附件信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="className"></param>
    /// <returns>附件集合</returns>
    public IList<IAttachmentFileInfo> GetStorageList(string id, string className)
    {
      return this.provider.GetStorageList(id, className);
    }
    #endregion

    #region 函数:SetEssential(string id,string isEssential)
    /// <summary>
    /// 设置或取消精华贴
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isEssential"></param>
    public int SetEssential(string id, string isEssential)
    {
      return this.provider.SetEssential(id, isEssential);
    }
    #endregion

    #region 函数:SetTop(string id,string isTop)
    /// <summary>设置置顶</summary>
    /// <param name="id"></param>
    /// <param name="isTop"></param>
    public int SetTop(string id, string isTop)
    {
      return this.provider.SetTop(id, isTop);
    }
    #endregion

    #region 函数:SetUp(string id)
    /// <summary>顶一下</summary>
    /// <param name="id">标识</param>
    /// <returns></returns>
    public int SetUp(string id)
    {
      return this.provider.SetUp(id);
    }
    #endregion

    #region 函数:SetClick(string id)
    /// <summary>设置帖子点击数</summary>
    /// <param name="id">标识</param>
    /// <returns></returns>
    public int SetClick(string id)
    {
      return this.provider.SetClick(id);
    }
    #endregion

    // -------------------------------------------------------
    // 编号和存储节点管理
    // -------------------------------------------------------

    #region RebuildCode()
    /// <summary>重建编号</summary>
    public int RebuildCode()
    {
      return this.provider.RebuildCode();
    }
    #endregion

    #region RebuildCode()
    /// <summary>重建存储节点索引</summary>
    public int RebuildStorageNodeIndex()
    {
      return this.provider.RebuildStorageNodeIndex();
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

        scope = @"( AccountId = ##" + account.Id + "## OR T.CategoryId IN ( " + scope + " ) ) ";

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
