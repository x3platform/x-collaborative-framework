namespace X3Platform.Web.Customizes.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Web.Configuration;
  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IDAL;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class CustomizePageProvider : ICustomizePageProvider
  {
    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Customize_Page";

    public CustomizePageProvider()
    {
      this.ibatisMapping = WebConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }

    public CustomizePageInfo this[string index]
    {
      get { return this.FindOne(index); }
    }

    // -------------------------------------------------------
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(CustomizePageInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">CustomizePageInfo 实例详细信息</param>
    /// <returns>CustomizePageInfo 实例详细信息</returns>
    public CustomizePageInfo Save(CustomizePageInfo param)
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

    #region 函数:Insert(CustomizePageInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">CustomizePageInfo 实例的详细信息</param>
    public void Insert(CustomizePageInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region 函数:Update(CustomizePageInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">CustomizePageInfo 实例的详细信息</param>
    public void Update(CustomizePageInfo param)
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
    /// <param name="param">CustomizePageInfo Id号</param>
    /// <returns>返回一个 CustomizePageInfo 实例的详细信息</returns>
    public CustomizePageInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return this.ibatisMapper.QueryForObject<CustomizePageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
    }
    #endregion

    #region 函数:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
    /// <summary>查询某条记录</summary>
    /// <param name="authorizationObjectType">授权对象类别</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <param name="name">页面名称</param>
    /// <returns>返回一个 CustomizePageInfo 实例的详细信息</returns>
    public CustomizePageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
      args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));
      args.Add("Name", name);

      CustomizePageInfo param = this.ibatisMapper.QueryForObject<CustomizePageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
    public IList<CustomizePageInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<CustomizePageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
    public IList<CustomizePageInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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

      IList<CustomizePageInfo> list = this.ibatisMapper.QueryForList<CustomizePageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">CustomizePageInfo 实例详细信息</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="authorizationObjectType">授权对象类别</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <param name="name">页面名称</param>
    /// <returns>布尔值</returns>
    public bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" AuthorizationObjectType = '{0}' AND AuthorizationObjectId = '{1}' AND Name = '{2}' ", StringHelper.ToSafeSQL(authorizationObjectType), StringHelper.ToSafeSQL(authorizationObjectId), StringHelper.ToSafeSQL(name)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region 函数:GetHtml(string name)
    /// <summary>获取Html文本</summary>
    /// <param name="name">页面名称</param>
    /// <returns>Html文本</returns>
    public string GetHtml(string name)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name, true)));

      CustomizePageInfo param = this.ibatisMapper.QueryForObject<CustomizePageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetHtml", tableName)), args);

      return param == null ? string.Empty : param.Html;
    }
    #endregion

    #region 函数:GetHtml(string name, string authorizationObjectType, string authorizationObjectId)
    /// <summary>获取Html文本</summary>
    /// <param name="name">页面名称</param>
    /// <param name="authorizationObjectType">授权对象类别</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <returns>Html文本</returns>
    public string GetHtml(string name, string authorizationObjectType, string authorizationObjectId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Name = '{0}' AND AuthorizationObjectType = '{1}' AND AuthorizationObjectId = '{2}' ",
        StringHelper.ToSafeSQL(name, true), StringHelper.ToSafeSQL(authorizationObjectType, true), StringHelper.ToSafeSQL(authorizationObjectId, true)));

      CustomizePageInfo param = this.ibatisMapper.QueryForObject<CustomizePageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetHtml", tableName)), args);

      return param == null ? string.Empty : param.Html;
    }
    #endregion
  }
}
