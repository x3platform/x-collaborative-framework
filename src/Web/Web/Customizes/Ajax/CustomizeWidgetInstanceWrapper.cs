namespace X3Platform.Web.Customizes.Ajax
{
  #region Using Libraries
  using System.Xml;
  using System.Collections.Generic;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Ajax.Json;
  using X3Platform.Util;

  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IBLL;
  #endregion

  /// <summary>部件实例</summary>
  public sealed class CustomizeWidgetInstanceWrapper : ContextWrapper
  {
    private ICustomizeWidgetInstanceService service = CustomizeContext.Instance.CustomizeWidgetInstanceService; // 数据服务

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      CustomizeWidgetInstanceInfo param = new CustomizeWidgetInstanceInfo();

      param = (CustomizeWidgetInstanceInfo)AjaxUtil.Deserialize(param, doc);

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

      CustomizeWidgetInstanceInfo param = service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizeWidgetInstanceInfo>(param) + ",");

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

      IList<CustomizeWidgetInstanceInfo> list = service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizeWidgetInstanceInfo>(list) + ",");
      outString.Append("\"paging\":" + paging + ",");
      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
      outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
      outString.Append("\"total\":" + paging.RowCount + ",");
      outString.Append("\"success\":1,");
      outString.Append("\"msg\":\"success\"}");

      return outString.ToString();
    }
    #endregion

    #region 函数:Create(XmlDocument doc)
    /// <summary>创建部件实例</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Create(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string id = XmlHelper.Fetch("id", doc);

      string pageId = XmlHelper.Fetch("pageId", doc);

      string widgetName = XmlHelper.Fetch("widgetName", doc);

      CustomizeWidgetInstanceInfo param = service.FindOne(id);

      if (param == null)
      {
        param = new CustomizeWidgetInstanceInfo();

        param = (CustomizeWidgetInstanceInfo)AjaxUtil.Deserialize(param, doc);

        this.service.SetPageAndWidget(param, pageId, widgetName);

        // 设置部件默认选项
        CustomizeWidgetInfo widget = CustomizeContext.Instance.CustomizeWidgetService.FindOneByName(widgetName);

        param.Height = widget.Height;
        param.Width = widget.Width;
        param.Options = widget.Options;

        this.service.Save(param);
      }

      outString.Append("{\"data\":" + AjaxUtil.Parse<CustomizeWidgetInstanceInfo>(param) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"创建成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:SetOptions(XmlDocument doc)
    /// <summary>设置选项信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string SetOptions(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string options = XmlHelper.Fetch("options", doc);

      string id = XmlHelper.Fetch("id", doc);

      CustomizeWidgetInstanceInfo param = this.service.FindOne(id);

      if (param == null)
      {
        return "{\"message\":{\"returnCode\":1,\"value\":\"未找到相关部件【" + id + "】实例。\"}}";
      }

      param.Options = options;

      this.service.Save(param);

      return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
    }
    #endregion

    #region 函数:GetOptionHtml(XmlDocument doc)
    /// <summary>获取属性编辑框HTML代码</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetOptionHtml(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string id = XmlHelper.Fetch("id", doc);

      CustomizeWidgetInstanceInfo param = this.service.FindOne(id);

      if (param == null)
      {
        return "{\"message\":{\"returnCode\":1,\"value\":\"未找到相关部件【" + id + "】实例。\"}}";
      }

      string optionHtml = this.service.GetOptionHtml(id).Replace("${Id}", id);

      optionHtml = ParseHtml(optionHtml, param.Options);

      outString.Append("{\"data\":\"" + StringHelper.ToSafeJson(optionHtml) + "\",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    private string ParseHtml(string optionHtml, string json)
    {
      if (string.IsNullOrEmpty(json))
      {
        return optionHtml;
      }

      JsonObject options = JsonObjectConverter.Deserialize(json);

      foreach (string key in options.Keys)
      {
        optionHtml = optionHtml.Replace("${" + key + "}", ((JsonPrimary)options[key]).Value.ToString());
      }

      return optionHtml;
    }
  }
}
