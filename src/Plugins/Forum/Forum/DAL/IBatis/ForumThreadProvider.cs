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
  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Membership;
  using X3Platform.AttachmentStorage;
  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class ForumThreadProvider : IForumThreadProvider
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Forum_Thread";

    #region 构造函数:ForumThreadProvider()
    /// <summary>构造函数</summary>
    public ForumThreadProvider()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

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

    #region 函数:Save(ForumThreadInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumThreadInfo"/>详细信息</returns>
    public ForumThreadInfo Save(ForumThreadInfo param)
    {
      if (!this.IsExist(param.Id))
      {
        this.Insert(param);
      }
      else
      {
        this.Update(param);
      }

      // 将帖子数据同步到评论表
      ForumContext.Instance.ForumCommentService.Save(new ForumCommentInfo(){
        Id = param.Id,
        AccountId = param.AccountId,
        AccountName = param.AccountName,
        ThreadId = param.Id,
        Content = param.Content,
        Anonymous = param.Anonymous,
        IP = param.IP
      });

      return param;
    }
    #endregion

    #region 函数:Insert(ForumThreadInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
    public void Insert(ForumThreadInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);

      // 重建编号
      this.GenerateCode(new string[] { DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString() });
    }
    #endregion

    #region 函数:Update(ForumThreadInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
    public void Update(ForumThreadInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">标识,多个以逗号隔开.</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids)) { return; }

      try
      {
        this.ibatisMapper.BeginTransaction();

        Dictionary<string, object> args = new Dictionary<string, object>();

        // 删除主题信息
        args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

        this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", this.tableName)), args);

        this.ibatisMapper.CommitTransaction();
      }
      catch
      {
        this.ibatisMapper.RollBackTransaction();
        throw;
      }
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));

      return this.ibatisMapper.QueryForObject<ForumThreadInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
    }
    #endregion

    #region 函数:FindOneByCode(string code)
    /// <summary>查询某条记录</summary>
    /// <param name="code">编号</param>
    /// <returns>实例<see cref="ForumThreadInfo"/>详细信息</returns>
    public ForumThreadInfo FindOneByCode(string code)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Code", StringHelper.ToSafeSQL(code));

      return this.ibatisMapper.QueryForObject<ForumThreadInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCode", this.tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

      return this.ibatisMapper.QueryForObject<ForumThreadInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByNew", this.tableName)), args);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public IList<ForumThreadInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<ForumThreadInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
    }
    #endregion

    #region 函数:FindAllQueryObject(DataQuery query)
    /// <summary>查询所有相关记录</summary>
    /// <param name="query">数据查询参数</param>
    /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
    public IList<ForumThreadQueryInfo> FindAllQueryObject(DataQuery query)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" IsTop DESC, UpdateDate DESC "));
      args.Add("Length", query.Length);

      return this.ibatisMapper.QueryForList<ForumThreadQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllQueryObject", this.tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" IsTop DESC, UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumThreadInfo> list = this.ibatisMapper.QueryForList<ForumThreadInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

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
    /// <returns>返回一个列表实例<see cref="ForumThreadInfo"/></returns>
    public IList<ForumThreadQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" IsTop DESC, UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumThreadQueryInfo> list = this.ibatisMapper.QueryForList<ForumThreadQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", this.tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录.</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:IsExistByCategory(string id)
    /// <summary>根据版块查询是否存在相关记录</summary>
    /// <param name="?">版块表示</param>
    /// <returns>布尔值</returns>
    public bool IsExistByCategory(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" CategoryId = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExistByCategory", this.tableName)), args)) == 0) ? false : true;
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
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("AccountId", id);

      return this.ibatisMapper.QueryForObject<int>(StringHelper.ToProcedurePrefix(string.Format("{0}_QueryCount", tableName)), args);
    }
    #endregion

    #region 函数:GetStorageList(string id, string className)
    /// <summary>
    /// 根据实体编号实体类名查询附件信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="className"></param>
    /// <returns>附件集合</returns>
    public IList<IAttachmentFileInfo> GetStorageList(string id, string className)
    {
      return AttachmentStorageContext.Instance.AttachmentFileService.FindAllByEntityId(className, id);
    }
    #endregion

    #region 函数:SetEssential(string id, string isEssential)
    /// <summary>设置精华贴</summary>
    /// <param name="id"></param>
    /// <param name="isEssential"></param>
    public int SetEssential(string id, string isEssential)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));
      args.Add("IsEssential", StringHelper.ToSafeSQL(isEssential));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetEssential", this.tableName)), args);

      return 0;
    }
    #endregion

    #region 函数:SetTop(string id, string isTop)
    /// <summary>设置置顶</summary>
    /// <param name="id"></param>
    /// <param name="isTop"></param>
    public int SetTop(string id, string isTop)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));
      args.Add("IsTop", StringHelper.ToSafeSQL(isTop));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetTop", this.tableName)), args);

      return 0;
    }
    #endregion

    #region 函数:SetUp(string id)
    /// <summary>顶一下</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public int SetUp(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetUp", this.tableName)), args);

      return 0;
    }
    #endregion

    #region 函数:SetClick(string id)
    /// <summary>设置帖子点击数</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public int SetClick(string id)
    {
      try
      {
        Dictionary<string, object> args = new Dictionary<string, object>();

        args.Add("Id", StringHelper.ToSafeSQL(id));

        this.ibatisMapper.BeginTransaction();
        this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetClick", this.tableName)), args);
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
    // 编号和存储节点管理
    // -------------------------------------------------------

    #region 私有函数:GenerateCode(string[] options)
    /// <summary>生成编号</summary>
    /// <param name="options">参数 [0]:Year | [1]:Month</param>
    /// <remarks>注:不能跨月份计算</remarks>
    private void GenerateCode(string[] options)
    {
      // 流水号生成规则
      // 按月份信息生成流水号
      DateTime date = new DateTime(Convert.ToInt32(options[0]), Convert.ToInt32(options[1]), 1);

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("BeginDate", date.ToString("yyyy-MM-01"));
      args.Add("EndDate", date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_GenerateCode", this.tableName)), args);
    }
    #endregion

    #region RebuildCode()
    /// <summary>重建编号</summary>
    public int RebuildCode()
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_RebuildCode", this.tableName)), args);

      return 0;
    }
    #endregion

    #region RebuildStorageNodeIndex()
    /// <summary>重建存储节点索引</summary>
    public int RebuildStorageNodeIndex()
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_RebuildStorageNodeIndex", this.tableName)), args);

      return 0;
    }
    #endregion
  }
}
