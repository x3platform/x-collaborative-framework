namespace X3Platform.Web.Customizes.Ajax
{
  #region Using Libraries
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Util;

  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IBLL;
  #endregion

  /// <summary>页面</summary>
  public sealed class CustomizePageWrapper : ContextWrapper
  {
    private ICustomizePageService service = CustomizeContext.Instance.CustomizePageService; // 数据服务

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      CustomizePageInfo param = new CustomizePageInfo();

      param = (CustomizePageInfo)AjaxUtil.Deserialize(param, doc);

      service.Save(param);

      return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
    }
    #endregion

    #region 函数:Delete(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Delete(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      service.Delete(id);

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
    public string FindOne(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string id = XmlHelper.Fetch("id", doc);

      CustomizePageInfo param = service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizePageInfo>(param) + ",");

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
    public string GetPaging(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

      int rowCount = -1;

      IList<CustomizePageInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizePageInfo>(list) + ",");
      outString.Append("\"paging\":" + paging + ",");
      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
      outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
      outString.Append("\"total\":" + paging.RowCount + ",");
      outString.Append("\"success\":1,");
      outString.Append("\"msg\":\"success\"}");

      return outString.ToString();
    }
    #endregion


    #region 函数:Reset(XmlDocument doc)
    /// <summary>获取分页内容</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns> 
    public string Reset(XmlDocument doc)
    {
      CustomizePageInfo param = new CustomizePageInfo();

      param = (CustomizePageInfo)AjaxUtil.Deserialize(param, doc);

      param.Html = CustomizeContext.Instance.CustomizeWidgetZoneService.GetHtml(param.Name);

      this.service.Save(param);

      return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
    }
    #endregion
  }
}
