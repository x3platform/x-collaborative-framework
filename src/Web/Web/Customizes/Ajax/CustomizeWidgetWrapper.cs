namespace X3Platform.Web.Customizes.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Xml;

  using X3Platform.Ajax;
  using X3Platform.Util;

  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IBLL;
  #endregion

  /// <summary>部件实例</summary>
  public sealed class CustomizeWidgetWrapper : ContextWrapper
  {
    private ICustomizeWidgetService service = CustomizeContext.Instance.CustomizeWidgetService; // 数据服务

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("save")]
    public string Save(XmlDocument doc)
    {
      CustomizeWidgetInfo param = new CustomizeWidgetInfo();

      param = (CustomizeWidgetInfo)AjaxUtil.Deserialize(param, doc);

      service.Save(param);

      return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
    }
    #endregion

    #region 函数:Delete(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("delete")]
    public string Delete(XmlDocument doc)
    {
      string ids = XmlHelper.Fetch("ids", doc);

      service.Delete(ids);

      return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(XmlDocument doc)
    /// <summary>获取详细信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("findOne")]
    public string FindOne(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string id = XmlHelper.Fetch("id", doc);

      CustomizeWidgetInfo param = service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizeWidgetInfo>(param) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindAll(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("findAll")]
    public string FindAll(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = XmlHelper.Fetch("whereClause", doc);

      int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

      IList<CustomizeWidgetInfo> list = this.service.FindAll(whereClause, length);

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizeWidgetInfo>(list) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(XmlDocument doc)
    /// <summary>获取分页内容</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns> 
    [AjaxMethod("getPages")]
    public string GetPaging(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

      int rowCount = -1;

      IList<CustomizeWidgetInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizeWidgetInfo>(list) + ",");
      outString.Append("\"paging\":" + paging + ",");
      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
      outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
      outString.Append("\"total\":" + paging.RowCount + ",");
      outString.Append("\"success\":1,");
      outString.Append("\"msg\":\"success\"}");

      return outString.ToString();
    }
    #endregion
  }
}
