namespace X3Platform.Web.Customizes.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Data.Common;
  using System.Text;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Web.Configuration;
  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IDAL;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class CustomizeWidgetInstanceProvider : ICustomizeWidgetInstanceProvider
  {
    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Customize_WidgetInstance";

    #region 构造函数:CustomizeWidgetInstanceProvider()
    /// <summary>构造函数</summary>
    public CustomizeWidgetInstanceProvider()
    {
      this.ibatisMapping = WebConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(CustomizeWidgetInstanceInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
    /// <returns>CustomizeWidgetInstanceInfo 实例详细信息</returns>
    public CustomizeWidgetInstanceInfo Save(CustomizeWidgetInstanceInfo param)
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

    #region 函数:Insert(CustomizeWidgetInstanceInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">CustomizeWidgetInstanceInfo 实例的详细信息</param>
    public void Insert(CustomizeWidgetInstanceInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region 函数:Update(CustomizeWidgetInstanceInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">CustomizeWidgetInstanceInfo 实例的详细信息</param>
    public void Update(CustomizeWidgetInstanceInfo param)
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
    /// <param name="param">CustomizeWidgetInstanceInfo Id号</param>
    /// <returns>返回一个 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
    public CustomizeWidgetInstanceInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      CustomizeWidgetInstanceInfo param = this.ibatisMapper.QueryForObject<CustomizeWidgetInstanceInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

      return param;
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
    public IList<CustomizeWidgetInstanceInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      IList<CustomizeWidgetInstanceInfo> list = this.ibatisMapper.QueryForList<CustomizeWidgetInstanceInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

      return list;
    }
    #endregion

    // -------------------------------------------------------
    // 分页
    // -------------------------------------------------------


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
    public IList<CustomizeWidgetInstanceInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
      args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<CustomizeWidgetInstanceInfo> list = this.ibatisMapper.QueryForList<CustomizeWidgetInstanceInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">CustomizeWidgetInstanceInfo 实例详细信息</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        throw new Exception("实例标识不能为空。");
      }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

      return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
    }
    #endregion

    #region 函数:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
    /// <summary>删除未绑定的部件实例</summary>
    /// <param name="pageId">页面名称</param>
    /// <param name="bindingWidgetInstanceIds">绑定的部件标识</param>
    /// <returns>布尔值</returns>
    public int RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" PageId = '{0}' AND Id NOT IN ('{1}') ", StringHelper.ToSafeSQL(pageId), StringHelper.ToSafeSQL(bindingWidgetInstanceIds).Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

      return 0;
    }
    #endregion
  }
}
