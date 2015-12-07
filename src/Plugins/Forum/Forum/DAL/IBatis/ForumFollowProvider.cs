namespace X3Platform.Plugins.Forum.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;
  using X3Platform.DigitalNumber;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class ForumFollowProvider : IForumFollowProvider
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Forum_Follow_Account";

    #region 构造函数:ForumFollowProvider()
    /// <summary>构造函数</summary>
    public ForumFollowProvider()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // 添加 删除
    // -------------------------------------------------------
    #region 函数:Insert(string accountId, string followAccountId)
    /// <summary>
    /// 添加关注用户
    /// </summary>
    /// <param name="accountId">主标识</param>
    /// <param name="followAccountId">添加标识</param>
    public void Insert(string accountId, string followAccountId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("Id", DigitalNumberContext.Generate("Key_Guid"));
      args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
      args.Add("FollowAccountId", StringHelper.ToSafeSQL(followAccountId));

      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), args);
    }
    #endregion

    #region 函数:Delete(string accountId, string followAccountId)
    /// <summary>
    /// 删除关注用户
    /// </summary>
    /// <param name="accountId">主标识</param>
    /// <param name="followAccountId">添加标识</param>
    public void Delete(string accountId, string followAccountId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" AccountId = '{0}' AND FollowAccountId = '{1}'", StringHelper.ToSafeSQL(accountId), StringHelper.ToSafeSQL(followAccountId)));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", this.tableName)), args);
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
    /// <returns>返回一个列表实例<see cref="ForumMemberInfo"/></returns>
    public IList<ForumFollowInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      //orderBy = string.IsNullOrEmpty(orderBy) ? " ModifiedDate DESC" : orderBy;

      //args.Add("StartIndex", startIndex);
      //args.Add("PageSize", pageSize);
      //args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      //args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));
      //args.Add("AccountId", KernelContext.Current.User.Id);

      //args.Add("RowCount", 0);

      IList<ForumFollowInfo> list = this.ibatisMapper.QueryForList<ForumFollowInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

      rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

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
    /// <returns>返回一个列表实例<see cref="ForumFollowQueryInfo"/></returns>
    public IList<ForumFollowQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      //orderBy = string.IsNullOrEmpty(orderBy) ? " ModifiedDate DESC" : orderBy;

      //args.Add("StartIndex", startIndex);
      //args.Add("PageSize", pageSize);
      //args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      //args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));
      //args.Add("AccountId", KernelContext.Current.User.Id);

      //args.Add("RowCount", 0);

      IList<ForumFollowQueryInfo> list = this.ibatisMapper.QueryForList<ForumFollowQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", this.tableName)), args);

      rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

      return list;
    }
    #endregion

    #region 函数:IsExist(string accountId, string followAccountId)
    /// <summary>
    /// 判断是否已经关注用户
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="followAccountId"></param>
    /// <returns></returns>
    public bool IsExist(string accountId, string followAccountId)
    {
      bool isExist = true;

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" AccountId = '{0}' AND FollowAccountId = '{1}'", StringHelper.ToSafeSQL(accountId), StringHelper.ToSafeSQL(followAccountId)));

      isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;

      return isExist;
    }
    #endregion

    #region 函数:IsMutual(string FollowAccountId)
    /// <summary>
    /// 查看是否相互关注
    /// </summary>
    /// <param name="FollowAccountId">被关注人标识</param>
    /// <returns></returns>
    public bool IsMutual(string FollowAccountId)
    {
      bool isMutual = false;

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("AccountId", StringHelper.ToSafeSQL(KernelContext.Current.User.Id));
      args.Add("FollowAccountId", StringHelper.ToSafeSQL(FollowAccountId));

      isMutual = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsMutual", this.tableName)), args) == 2) ? true : false;

      return isMutual;
    }
    #endregion

    #region 函数:GetFollowCount(string accountId)
    /// <summary>
    /// 根据用户标识查询用户人气
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public int GetFollowCount(string accountId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("AccountId", accountId);

      return ibatisMapper.QueryForObject<int>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetFollowCount", tableName)), args);
    }
    #endregion
  }
}
