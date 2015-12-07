namespace X3Platform.Plugins.Forum.DAL.IBatis
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Data;

  /// <summary></summary>
  [DataObject]
  public class ForumCommentProvider : IForumCommentProvider
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Forum_Comment";

    #region 构造函数:ForumCommentProvider()
    /// <summary>构造函数</summary>
    public ForumCommentProvider()
    {
      configuration = ForumConfigurationView.Instance.Configuration;

      ibatisMapping = configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // 添加 删除 修改
    // -------------------------------------------------------

    #region 函数:Save(ForumCommentInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumCommentInfo"/>详细信息</returns>
    public ForumCommentInfo Save(ForumCommentInfo param)
    {
      if (!IsExist(param.Id))
      {
        Insert(param);
      }
      else
      {
        Update(param);
      }

      return (ForumCommentInfo)param;
    }
    #endregion

    #region 函数:Insert(ForumCommentInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
    public void Insert(ForumCommentInfo param)
    {
      this.ibatisMapper.BeginTransaction();

      try
      {
        this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);

        ForumThreadInfo threadInfo = ForumContext.Instance.ForumThreadService.FindOne(param.ThreadId);

        threadInfo.CommentCount = ForumContext.Instance.ForumCommentService.GetCommentCount(param.ThreadId);

        // 查询最后回帖信息
        string lastCommentInfo = ForumContext.Instance.ForumCommentService.GetLastComment(param.ThreadId);

        if (!string.IsNullOrEmpty(lastCommentInfo))
        {
          string[] info = lastCommentInfo.Split(',');

          threadInfo.LatestCommentAccountId = info[0];
          threadInfo.LatestCommentAccountName = info[1];
        }

        ForumContext.Instance.ForumThreadService.Save(threadInfo);

        this.ibatisMapper.CommitTransaction();
      }
      catch
      {
        this.ibatisMapper.RollBackTransaction();
        throw;
      }
    }
    #endregion

    #region 函数:Update(ForumCommentInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
    public void Update(ForumCommentInfo param)
    {
      ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">标识,多个以逗号隔开.</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids))
        return;

      try
      {
        ibatisMapper.BeginTransaction();

        Dictionary<string, object> args = new Dictionary<string, object>();
        // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
        // 删除回复信息
        args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));
        ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", tableName)), args);

        args = new Dictionary<string, object>();
        // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
        // 删除对某回帖指向
        args.Add("WhereClause", string.Format(" ReplyCommentId IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));
        ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(BackInfo)", tableName)), args);

        ibatisMapper.CommitTransaction();
      }
      catch (Exception)
      {
        ibatisMapper.RollBackTransaction();
        throw new Exception();
      }
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumCommentInfo"/>的详细信息</returns>
    public ForumCommentInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("Id", StringHelper.ToSafeSQL(id));

      ForumCommentInfo param = ibatisMapper.QueryForObject<ForumCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindOne(string id, string threaId)
    /// <summary>
    /// 查询回帖信息
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="threaId">主题编号</param>
    /// <returns>返回信息</returns>
    public ForumCommentInfo FindOne(string id, string threaId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("Id", id);
      args.Add("ThreadId", threaId);

      ForumCommentInfo param = ibatisMapper.QueryForObject<ForumCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByTheadId", tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumCommentInfo"/>的详细信息</returns>
    public IList<ForumCommentInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      IList<ForumCommentInfo> list = ibatisMapper.QueryForList<ForumCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

      return list;
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
    /// <returns>返回一个列表实例<see cref="ForumCommentInfo"/></returns>
    public IList<ForumCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" CreatedDate ASC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumCommentInfo> list = ibatisMapper.QueryForList<ForumCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

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
    /// <returns>返回一个列表实例<see cref="ForumCommentQueryInfo"/></returns>
    public IList<ForumCommentQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" CreatedDate ASC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumCommentQueryInfo> list = ibatisMapper.QueryForList<ForumCommentQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", tableName)), args);

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
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id, true)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:GetCommentCount(string id)
    /// <summary>
    /// 根据主题查询回帖数
    /// </summary>
    /// <param name="id">标识</param>
    /// <returns>回帖数</returns>
    public int GetCommentCount(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("TheadId", id);

      return ibatisMapper.QueryForObject<int>(StringHelper.ToProcedurePrefix(string.Format("{0}_QueryCount", tableName)), args);
    }
    #endregion

    #region 函数:DeleteByTheadId(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">实例的标识,多条记录以逗号分开</param>
    public void DeleteByTheadId(string ids)
    {
      try
      {
        ibatisMapper.BeginTransaction();

        Dictionary<string, object> args = new Dictionary<string, object>();
        // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
        // 删除回复信息
        args.Add("WhereClause", string.Format(" ThreadId IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));
        ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", tableName)), args);
        ibatisMapper.CommitTransaction();
      }
      catch (Exception ex)
      {
        ibatisMapper.RollBackTransaction();
        throw ex;
      }
    }
    #endregion

    #region 函数:GetLastComment(string id)
    /// <summary>
    /// 根据帖子编号查询最后回帖信息
    /// </summary>
    /// <param name="id">帖子编号</param>
    /// <returns>最后回帖信息</returns>
    public string GetLastComment(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("ThreadId", id);

      ForumCommentInfo param = ibatisMapper.QueryForObject<ForumCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetLastComment", tableName)), args);

      string result = string.Empty;
      if (param != null)
      {
        result = param.AccountId + "," + (param.Anonymous == 1 ? "匿名" : param.AccountName);
      }
      return result;
    }
    #endregion
  }
}
