namespace X3Platform.Plugins.Bugs.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;

  using X3Platform.CategoryIndexes;
  using X3Platform.IBatis.DataMapper;
  using X3Platform.Security.Authority;
  using X3Platform.Membership.Scope;
  using X3Platform.Membership;
  using X3Platform.Web.Component.Combobox;
  using X3Platform.Util;

  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class BugCategoryProvider : IBugCategoryProvider
  {
    /// <summary>配置</summary>
    private BugConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Bug_Category";

    #region 构造函数:BugCategoryProvider()
    /// <summary>构造函数</summary>
    public BugCategoryProvider()
    {
      this.configuration = BugConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
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
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(BugCategoryInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例详细信息</param>
    /// <returns></returns>
    public BugCategoryInfo Save(BugCategoryInfo param)
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

    #region 函数:Insert(BugCategoryInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">实例的详细信息</param>
    public void Insert(BugCategoryInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
    }
    #endregion

    #region 函数:Update(BugCategoryInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">实例的详细信息</param>
    public void Update(BugCategoryInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
    }
    #endregion

    #region 函数:CanDelete(string id)
    /// <summary>判断新闻类别是否能够被删除</summary>
    /// <param name="id">新闻类别标识</param>
    /// <returns>true：可删除；false：不能删除。</returns>
    public bool CanDelete(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("CategoryId", StringHelper.ToSafeSQL(id));

      return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CanDelete", this.tableName)), args) == 0) ? true : false;
    }
    #endregion

    #region 函数:Delete(string id)
    /// <summary>逻辑删除记录</summary>
    /// <param name="id">实例的标识</param>
    public int Delete(string id)
    {
      if (string.IsNullOrEmpty(id)) { return 1; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

      return 0;
    }
    #endregion

    #region 函数:Remove(string id)
    /// <summary>物理删除记录</summary>
    /// <param name="id">类别标识</param>
    public int Remove(string id)
    {
      try
      {
        this.ibatisMapper.BeginTransaction();

        Dictionary<string, object> args = new Dictionary<string, object>();

        args.Add("Id", StringHelper.ToSafeSQL(id));

        this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Scope_RemoveAll", this.tableName)), args);
        this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Remove", this.tableName)), args);

        this.ibatisMapper.CommitTransaction();
      }
      catch
      {
        this.ibatisMapper.RollBackTransaction();

        throw;
      }

      return 0;
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例的详细信息</returns>
    public BugCategoryInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      BugCategoryInfo param = this.ibatisMapper.QueryForObject<BugCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindOneByCategoryIndex(string categoryIndex)
    /// <summary>查询某条记录</summary>
    /// <param name="categoryIndex">类别索引</param>
    /// <returns>返回实例<see cref="BugCategoryInfo"/>的详细信息</returns>
    public BugCategoryInfo FindOneByCategoryIndex(string categoryIndex)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("CategoryIndex", categoryIndex);

      BugCategoryInfo param = this.ibatisMapper.QueryForObject<BugCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCategoryIndex", this.tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>
    /// 查询所有相关记录
    /// </summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns></returns>
    public IList<BugCategoryInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      IList<BugCategoryInfo> list = this.ibatisMapper.QueryForList<BugCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);

      return list;
    }
    #endregion

    #region 函数:FindAllQueryObject(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="BugCategoryQueryInfo"/>的详细信息</returns>
    public IList<BugCategoryQueryInfo> FindAllQueryObject(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      IList<BugCategoryQueryInfo> list = this.ibatisMapper.QueryForList<BugCategoryQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllQueryObject", this.tableName)), args);

      return list;
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计.</param>
    /// <param name="pageSize">每页显示的数据行数</param>
    /// <param name="whereClause">WHERE 查询条件.</param>
    /// <param name="orderBy">ORDER BY 排序条件.</param>
    /// <param name="rowCount">符合条件的数据总行数</param>
    /// <returns></returns>
    public IList<BugCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "CategoryIndex", "LIKE" }, { "Status", "IN" } }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<BugCategoryInfo> list = this.ibatisMapper.QueryForList<BugCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

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
    /// <returns>返回一个列表实例<see cref="BugCategoryQueryInfo"/></returns>
    public IList<BugCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "CategoryIndex", "LIKE" }, { "Status", "IN" } }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<BugCategoryQueryInfo> list = this.ibatisMapper.QueryForList<BugCategoryQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", this.tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>
    /// 查询是否存在相关的记录
    /// </summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:SetStatus(string id, int status)
    /// <summary>设置类别状态(停用/启用)</summary>
    /// <param name="id">新闻类别标识</param>
    /// <param name="status">1将停用的类别启用，0将在用的类别停用</param>
    /// <returns></returns>
    public bool SetStatus(string id, int status)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);
      args.Add("Status", status);

      return (this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", this.tableName)), args) == 1 ? true : false);
    }
    #endregion

    #region 函数:CanUpdatePrefixalCode(string categoryId)
    /// <summary>
    /// 判断是否能够修改指定类别的编号前缀（如果类别已经被引用，则编号前缀不允许修改）
    /// </summary>
    /// <param name="categoryId">新闻类别标识</param>
    /// <returns>true：能修改；false：不能被修改。</returns>
    public bool CanUpdatePrefixalCode(string categoryId)
    {
      return false;
    }
    #endregion

    #region 函数:GetComboboxByWhereClause(string whereClause, string selectedValue)
    /// <summary>获取拉框数据源</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="selectedValue">默认选择的值</param>
    /// <returns></returns>
    public IList<ComboboxItem> GetComboboxByWhereClause(string whereClause, string selectedValue)
    {
      IList<ComboboxItem> list = new List<ComboboxItem>();

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

      DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetComboboxByWhereClause", this.tableName)), args);

      ComboboxItem item = null;

      foreach (DataRow row in table.Rows)
      {
        item = new ComboboxItem(row[0].ToString(), row[1].ToString());

        if (!string.IsNullOrEmpty(selectedValue) && row["Value"].ToString() == selectedValue)
        {
          item.Selected = true;
        }

        list.Add(item);
      }

      return list;

      //string statusCondition = "";

      //Dictionary<string, object> args = new Dictionary<string, object>();

      //args.Add("AccountId", KernelContext.Current.User.Id);

      //if (status == "1")
      //{
      //    return this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetComboboxByWhereClause1", this.tableName)), args);
      //}
      //else
      //{
      //    if (!string.IsNullOrEmpty(status))
      //    {
      //        statusCondition = " and Status = ##" + status + "## ";
      //    }

      //    args.Add("statusCondition", StringHelper.ToSafeSQL(statusCondition));

      //    return this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetComboboxByWhereClause", this.tableName)), args);
      //}
    }
    #endregion

    #region 函数:GetDynamicTreeNodes(string searchPath, string whereClause)
    /// <summary>获取树的节点列表</summary>
    /// <param name="searchPath">树的节点查询路径</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <returns>树的节点</returns>
    public IList<DynamicTreeNode> GetDynamicTreeNodes(string searchPath, string whereClause)
    {
      IList<DynamicTreeNode> list = new List<DynamicTreeNode>();

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("SearchPath", searchPath);
      args.Add("WhereClause", whereClause);

      DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetDynamicTreeView", this.tableName)), args);

      DynamicTreeNode node = null;

      string idPrefix = string.IsNullOrEmpty(searchPath) ? string.Empty : searchPath.Replace(@"\", @"$") + '$';

      foreach (DataRow row in table.Rows)
      {
        string name = row["Name"].ToString();

        if (string.IsNullOrEmpty(name.TrimEnd('$')))
        {
          continue;
        }
        else
        {
          node = new DynamicTreeNode();

          node.Id = idPrefix + name;
          node.Name = name.TrimEnd('$');

          if (name.IndexOf('$') == name.Length - 1)
          {
            // 末级节点设置
            node.Token = row["Token"].ToString();
            node.CategoryIndex = row["CategoryIndex"].ToString();
            node.HasChildren = false;
          }
          else
          {
            // 普通树节点设置
            node.Token = string.Empty;
            node.CategoryIndex = string.Empty;
            node.HasChildren = true;
          }

          list.Add(node);
        }
      }

      return list;
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
      string dataTablePrefix = BugConfigurationView.Instance.DataTablePrefix;

      return MembershipManagement.Instance.AuthorizationObjectService.HasAuthority(
          string.Format("{0}{1}_Scope", dataTablePrefix, this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(BugCategoryInfo)),
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
      string dataTablePrefix = BugConfigurationView.Instance.DataTablePrefix;

      MembershipManagement.Instance.AuthorizationObjectService.BindAuthorizationScopeObjects(
          string.Format("{0}{1}_Scope", dataTablePrefix, this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(BugCategoryInfo)),
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
      string dataTablePrefix = BugConfigurationView.Instance.DataTablePrefix;

      return MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeObjects(
          string.Format("{0}{1}_Scope", dataTablePrefix, this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(BugCategoryInfo)),
          authorityName);
    }
    #endregion
  }
}
