namespace X3Platform.Plugins.Bugs.DAL.IBatis
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Data;

  [DataObject]
  public class BugCommentProvider : IBugCommentProvider
  {
    /// <summary>配置</summary>
    private BugConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Bug_Comment";

    public BugCommentProvider()
    {
      this.configuration = BugConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }

    // -------------------------------------------------------
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(BugCommentInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">BugCommentInfo 实例详细信息</param>
    /// <returns>BugCommentInfo 实例详细信息</returns>
    public BugCommentInfo Save(BugCommentInfo param)
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

    #region 函数:Insert(BugCommentInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">BugCommentInfo 实例的详细信息</param>
    public void Insert(BugCommentInfo param)
    {
      param.AccountId = KernelContext.Current.User.Id;

      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
    }
    #endregion

    #region 函数:Update(BugCommentInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">BugCommentInfo 实例的详细信息</param>
    public void Update(BugCommentInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">标识,多个以逗号隔开</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids)) { return; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="param">BugCommentInfo Id号</param>
    /// <returns>返回一个 BugCommentInfo 实例的详细信息</returns>
    public BugCommentInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return this.ibatisMapper.QueryForObject<BugCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 BugCommentInfo 实例的详细信息</returns>
    public IList<BugCommentInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      whereClause = (string.IsNullOrEmpty(whereClause)) ? " 1=1 ORDER BY CreatedDate " : whereClause;

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<BugCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
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
    /// <returns>返回一个列表实例</returns> 
    public IList<BugCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql());
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<BugCommentInfo> list = this.ibatisMapper.QueryForList<BugCommentInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">BugCommentInfo 实例详细信息</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion
  }
}
