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
  public class ForumMemberProvider : IForumMemberProvider
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Forum_Member";

    #region 构造函数:ForumMemberProvider()
    /// <summary>构造函数</summary>
    public ForumMemberProvider()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // 添加 删除 修改
    // -------------------------------------------------------

    #region 函数:Save(ForumMemberInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumMemberInfo"/>详细信息</returns>
    public ForumMemberInfo Save(ForumMemberInfo param)
    {
      if (!this.IsExist(param.AccountId))
      {
        this.Insert(param);
      }
      else
      {
        this.Update(param);
      }

      return param;
    }
    #endregion

    #region 函数:Insert(ForumMemberInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
    public void Insert(ForumMemberInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
    }
    #endregion

    #region 函数:Update(ForumMemberInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
    public void Update(ForumMemberInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">标识,多个以逗号隔开.</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids))
      {
        return;
      }

      try
      {
        ibatisMapper.BeginTransaction();

        Dictionary<string, object> args = new Dictionary<string, object>();
        // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
        // 删除主题信息
        args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));
        this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", this.tableName)), args);

        ibatisMapper.CommitTransaction();
      }
      catch (Exception ex)
      {
        ibatisMapper.RollBackTransaction();
        throw ex;
      }
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public ForumMemberInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("Id", StringHelper.ToSafeSQL(id));

      ForumMemberInfo param = this.ibatisMapper.QueryForObject<ForumMemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindOneByAccountId(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public ForumMemberInfo FindOneByAccountId(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("AccountId", StringHelper.ToSafeSQL(id));

      return this.ibatisMapper.QueryForObject<ForumMemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByAccountId", this.tableName)), args);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public IList<ForumMemberInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      IList<ForumMemberInfo> list = this.ibatisMapper.QueryForList<ForumMemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);

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
    /// <returns>返回一个列表实例<see cref="ForumMemberInfo"/></returns>
    public IList<ForumMemberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumMemberInfo> list = this.ibatisMapper.QueryForList<ForumMemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

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
      if (string.IsNullOrEmpty(id))
      {
        throw new Exception("实例标识不能为空。");
      }

      bool isExist = true;

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" AccountId = '{0}' ", StringHelper.ToSafeSQL(id)));

      isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;

      return isExist;
    }
    #endregion

    #region 函数:SetIconPath(string id)
    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="id">用户编号</param>
    /// <returns>布尔值</returns>
    public void SetIconPath(string id)
    {
      ForumMemberInfo param = new ForumMemberInfo();
      // param.ApplicationTag = applicationTag;
      param.AccountId = id;
      param.IconPath = id + "_120x120.png";

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetIconPath", this.tableName)), param);
    }
    #endregion

    #region 函数:SetPoint(string id, int score)
    /// <summary>
    /// 增加积分
    /// </summary>
    /// <param name="id">用户编号</param>
    /// <param name="score">积分</param>
    public void SetPoint(string id, int score)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("AccountId", id);
      args.Add("Score", score);

      ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetPoint", tableName)), args);
    }
    #endregion

    #region 函数:SetThreadCount(string id)
    /// <summary>
    /// 增加帖子数
    /// </summary>
    /// <param name="id">用户编号</param>
    public void SetThreadCount(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("AccountId", id);

      ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetThreadCount", tableName)), args);
    }
    #endregion

    #region 函数:SetFollowCount(string id, int value)
    /// <summary>增加关注数</summary>
    /// <param name="id">用户编号</param>
    /// <param name="value">关注数值</param>
    public void SetFollowCount(string id, int value)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
      args.Add("AccountId", id);
      args.Add("Value", value);

      ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetFollowCount", tableName)), args);
    }
    #endregion

    #region 函数:UpdateMemberInfo(string applicationTag)
    /// <summary>
    /// 同步论坛member信息
    /// </summary>
    /// <param name="applicationTag">论坛模块标识</param>
    /// <returns></returns>
    public bool UpdateMemberInfo(string applicationTag)
    {
      bool flag = true;

      try
      {
        ibatisMapper.BeginTransaction();

        Dictionary<string, object> args = new Dictionary<string, object>();
        // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));
        ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_UpdateMemberInfo", this.tableName)), args);
        ibatisMapper.CommitTransaction();
      }
      catch (Exception)
      {
        flag = false;
        this.ibatisMapper.RollBackTransaction();
        throw new Exception();
      }

      return flag;
    }
    #endregion
  }
}
