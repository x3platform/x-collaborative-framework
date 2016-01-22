namespace X3Platform.Apps.DAL.IBatis
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Apps.IDAL;
  using X3Platform.Apps.Configuration;
  using X3Platform.Apps.Model;
  using X3Platform.Data;

  /// <summary></summary>
  [DataObject]
  public class ApplicationEventProvider : IApplicationEventProvider
  {
    private AppsConfiguration configuration = null;

    private string ibatisMapping = null;

    private ISqlMapper ibatisMapper = null;

    private string tableName = "tb_Application_Event";

    /// <summary></summary>
    public ApplicationEventProvider()
    {
      this.configuration = AppsConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }

    // -------------------------------------------------------
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(ApplicationEventInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ApplicationEventInfo"/>详细信息</param>
    /// <returns>实例<see cref="ApplicationEventInfo"/>详细信息</returns>
    public ApplicationEventInfo Save(ApplicationEventInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Save", tableName)), param);

      return this.FindOne(param.Id);
    }
    #endregion

    #region 函数:Delete(string id)
    /// <summary>删除记录</summary>
    /// <param name="id">实例的标识</param>
    public void Delete(string id)
    {
        if (string.IsNullOrEmpty(id)) { return; }

        Dictionary<string, object> args = new Dictionary<string, object>();

        args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

        this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="param">ApplicationEventInfo Id号</param>
    /// <returns>返回一个 实例<see cref="ApplicationEventInfo"/>详细信息</returns>
    public ApplicationEventInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      ApplicationEventInfo param = this.ibatisMapper.QueryForObject<ApplicationEventInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 实例<see cref="ApplicationEventInfo"/>详细信息</returns>
    public IList<ApplicationEventInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      IList<ApplicationEventInfo> list = this.ibatisMapper.QueryForList<ApplicationEventInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

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
    /// <param name="query">数据查询参数</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表<see cref="ApplicationEventInfo"/></returns> 
    public IList<ApplicationEventInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Description", "LIKE" } }));
      args.Add("OrderBy", query.GetOrderBySql(" Date DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ApplicationEventInfo> list = this.ibatisMapper.QueryForList<ApplicationEventInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">实例<see cref="ApplicationEventInfo"/>详细信息</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion
  }
}
