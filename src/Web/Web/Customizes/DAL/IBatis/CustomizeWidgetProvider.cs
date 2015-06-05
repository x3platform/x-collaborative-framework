namespace X3Platform.Web.Customizes.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Web.Configuration;
  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IDAL;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class CustomizeWidgetProvider : ICustomizeWidgetProvider
  {
    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Customize_Widget";

    #region 构造函数:CustomizeWidgetProvider()
    /// <summary>构造函数</summary>
    public CustomizeWidgetProvider()
    {
      this.ibatisMapping = WebConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

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
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(CustomizeWidgetInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">CustomizeWidgetInfo 实例详细信息</param>
    /// <returns>CustomizeWidgetInfo 实例详细信息</returns>
    public CustomizeWidgetInfo Save(CustomizeWidgetInfo param)
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

    #region 函数:Insert(CustomizeWidgetInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">CustomizeWidgetInfo 实例的详细信息</param>
    public void Insert(CustomizeWidgetInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region 函数:Update(CustomizeWidgetInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">CustomizeWidgetInfo 实例的详细信息</param>
    public void Update(CustomizeWidgetInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
    }
    #endregion

    #region 函数:Delete(string id)
    /// <summary>删除记录</summary>
    /// <param name="id">标识</param>
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
    /// <param name="param">CustomizeWidgetInfo Id号</param>
    /// <returns>返回一个 CustomizeWidgetInfo 实例的详细信息</returns>
    public CustomizeWidgetInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return this.ibatisMapper.QueryForObject<CustomizeWidgetInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
    }
    #endregion

    #region 函数:FindOneByName(string name)
    /// <summary>查询某条记录</summary>
    /// <param name="name">页面名称</param>
    /// <returns>返回一个 CustomizeWidgetInfo 实例的详细信息</returns>
    public CustomizeWidgetInfo FindOneByName(string name)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Name", name);

      return this.ibatisMapper.QueryForObject<CustomizeWidgetInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 CustomizeWidgetInfo 实例的详细信息</returns>
    public IList<CustomizeWidgetInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<CustomizeWidgetInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
    /// <returns>返回一个列表实例</returns> 
    public IList<CustomizeWidgetInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      if (query.Variables["scence"] == "Query")
      {
        string searhText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

        if (string.IsNullOrEmpty(searhText))
        {
          args.Add("WhereClause", string.Empty);
        }
        else
        {
          args.Add("WhereClause", " Name LIKE '%" + searhText + "%' OR Title LIKE '%" + searhText + "%' ");
        }
      }
      else
      {
        args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
      }

      args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<CustomizeWidgetInfo> list = this.ibatisMapper.QueryForList<CustomizeWidgetInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">CustomizeWidgetInfo 实例详细信息</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:IsExistName(string name)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="name">页面名称</param>
    /// <returns>布尔值</returns>
    public bool IsExistName(string name)
    {
      if (string.IsNullOrEmpty(name)) { throw new Exception("名称不能为空。"); }
      
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Name = '{1}' ", StringHelper.ToSafeSQL(name)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:GetOptionHtml(string id)
    /// <summary>获取属性编辑框Html文本</summary>
    /// <param name="id">标识</param>
    /// <returns>Html文本</returns>
    public string GetOptionHtml(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));

      return this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetOptionHtml", tableName)), args).ToString();
    }
    #endregion
  }
}
