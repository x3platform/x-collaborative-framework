namespace X3Platform.Plugins.Forum.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Text;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;
  using X3Platform.Security.Authority;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.CategoryIndexes;
  using X3Platform.Membership.Scope;
  using X3Platform.Membership;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class ForumCategoryProvider : IForumCategoryProvider
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Forum_Category";

    #region 构造函数:ForumCategoryProvider()
    /// <summary>构造函数</summary>
    public ForumCategoryProvider()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // 事务支持
    // -------------------------------------------------------

    #region 函数:BeginTransaction()
    /// <summary>启动事务</summary>
    public void BeginTransaction()
    {
      this.ibatisMapper.BeginTransaction();
    }
    #endregion

    #region 函数:BeginTransaction(IsolationLevel isolationLevel)
    /// <summary>启动事务</summary>
    /// <param name="isolationLevel">事务隔离级别</param>
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
      this.ibatisMapper.BeginTransaction(isolationLevel);
    }
    #endregion

    #region 函数:CommitTransaction()
    /// <summary>提交事务</summary>
    public void CommitTransaction()
    {
      this.ibatisMapper.CommitTransaction();
    }
    #endregion

    #region 函数:RollBackTransaction()
    /// <summary>回滚事务</summary>
    public void RollBackTransaction()
    {
      this.ibatisMapper.RollBackTransaction();
    }
    #endregion

    // -------------------------------------------------------
    // 添加 删除 修改
    // -------------------------------------------------------

    #region 函数:Save(ForumCategoryInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumCategoryInfo"/>详细信息</returns>
    public ForumCategoryInfo Save(ForumCategoryInfo param)
    {
      if (!IsExist(param.Id))
      {
        Insert(param);
      }
      else
      {
        Update(param);
      }

      return param;
    }
    #endregion

    #region 函数:Insert(ForumCategoryInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
    public void Insert(ForumCategoryInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region 函数:Update(ForumCategoryInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">实例<see cref="ForumCategoryInfo"/>详细信息</param>
    public void Update(ForumCategoryInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_UpdateThread", tableName)), param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">标识,多个以逗号隔开.</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids)) { return; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));

      return this.ibatisMapper.QueryForObject<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
    }
    #endregion

    #region 函数:FindOneByCategoryIndex(string categoryIndex)
    /// <summary>查询某条记录</summary>
    /// <param name="categoryIndex">分类索引</param>
    /// <returns>返回实例<see cref="DengBaoCategoryInfo"/>的详细信息</returns>
    public ForumCategoryInfo FindOneByCategoryIndex(string categoryIndex)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("CategoryIndex", StringHelper.ToSafeSQL(categoryIndex));

      return ibatisMapper.QueryForObject<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCategoryIndex", tableName)), args);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumCategoryInfo"/>的详细信息</returns>
    public IList<ForumCategoryInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumCategoryInfo> list = this.ibatisMapper.QueryForList<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumCategoryQueryInfo> list = this.ibatisMapper.QueryForList<ForumCategoryQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录.</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        throw new Exception("实例标识不能为空。");
      }

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:IsExistByParent(string id)
    /// <summary>查询是否存在相关的记录.</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExistByParent(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        throw new Exception("实例标识不能为空。");
      }

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" ParentId = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:FetchCategoryIndex(string whereClause)
    /// <summary>根据分类状态获取分类索引</summary>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="applicationTag">程序标识</param>
    /// <returns>分类索引对象</returns>
    public ICategoryIndex FetchCategoryIndex(string whereClause)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      //ApplicationInfo application = AppsContext.Instance.ApplicationService["Forum"];
      //if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      //{
      //    StringBuilder bindScope = new StringBuilder();
      //    bindScope.Append(string.Format(" Id IN (SELECT distinct EntityId from {0}_Category_Scope S,view_AuthorizationObject_Account A", ForumUtility.ToDataTablePrefix(applicationTag)));
      //    bindScope.Append(" where  s.AuthorizationObjectId=a.AuthorizationObjectId");
      //    bindScope.Append(" and s.AuthorizationObjectType = a.AuthorizationObjectType");
      //    bindScope.Append(" and a.AccountId = '" + KernelContext.Current.User.Id + "')");
      //    args.Add("BindScope", bindScope);
      //}
      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

      IList<string> list = ibatisMapper.QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_FetchCategoryIndex", tableName)), args);

      CategoryIndexWriter writer = new CategoryIndexWriter("选择类别");

      foreach (string item in list)
      {
        if (item != "")
        {
          writer.Read(item);
        }
      }

      return writer.Write();
    }
    #endregion

    #region 函数:IsCategoryAdministrator(string categoryId)
    /// <summary>
    /// 判断当前用户是否是该板块的管理员
    /// <param name="categoryId">版块编号</param>
    /// <returns>操作结果</returns>
    public bool IsCategoryAdministrator(string categoryId)
    {
      AuthorityInfo moderator = AuthorityContext.Instance.AuthorityService["应用_默认_管理员"];

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("CategoryId", StringHelper.ToSafeSQL(categoryId));
      args.Add("AccountId", StringHelper.ToSafeSQL(KernelContext.Current.User.Id));
      args.Add("AuthorityId", StringHelper.ToSafeSQL(moderator.Id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsCategoryAdministrator", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:IsCategoryAuthority(string categoryId)
    /// <summary>根据版块标识查询当前用户是否拥有该权限</summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public bool IsCategoryAuthority(string categoryId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("CategoryId", StringHelper.ToSafeSQL(categoryId));
      args.Add("AccountId", StringHelper.ToSafeSQL(KernelContext.Current.User.Id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsCategoryAuthority", tableName)), args)) == 0) ? false : true;
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
      ForumCategoryInfo info = this.FindOne(categoryId);
      string categoryAdminName = string.Empty;
      if (info != null)
      {
        // categoryAdminName = info.ModeratorScopeObjectView;
      }
      return categoryAdminName;

    }
    #endregion

    // -------------------------------------------------------
    // 授权范围管理
    // -------------------------------------------------------

    #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
    /// <summary>判断用户是否拥数据权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <param name="account">帐号</param>
    /// <returns>布尔值</returns>
    public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
    {
      return MembershipManagement.Instance.AuthorizationObjectService.HasAuthority(
          this.ibatisMapper.CreateGenericSqlCommand(),
          string.Format("{0}_Scope", this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(ForumCategoryInfo)),
          authorityName,
          account);
    }
    #endregion

    #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    /// <summary>配置数据的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <param name="scopeText">权限范围的文本</param>
    public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    {
      MembershipManagement.Instance.AuthorizationObjectService.BindAuthorizationScopeObjects(
          this.ibatisMapper.CreateGenericSqlCommand(),
          string.Format("{0}_Scope", this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(ForumCategoryInfo)),
          authorityName,
          scopeText);
    }
    #endregion

    #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
    /// <summary>查询应用的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <returns></returns>
    public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
    {
      return MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeObjects(
          this.ibatisMapper.CreateGenericSqlCommand(),
          string.Format("{0}_Scope", this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(ForumCategoryInfo)),
          authorityName);
    }
    #endregion
  }
}
