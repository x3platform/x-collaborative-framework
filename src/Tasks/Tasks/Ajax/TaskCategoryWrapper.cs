namespace X3Platform.Tasks.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Tasks.IBLL;
  using X3Platform.Tasks.Model;
  using X3Platform.Util;
  using X3Platform.DigitalNumber;
  using X3Platform.Apps;
  using X3Platform.Tasks.Configuration;
    using X3Platform.Globalization;
  #endregion

  /// <summary></summary>
  public class TaskCategoryWrapper : ContextWrapper
  {
    private ITaskCategoryService service = TasksContext.Instance.TaskCategoryService;

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      TaskCategoryInfo param = new TaskCategoryInfo();

      param = (TaskCategoryInfo)AjaxUtil.Deserialize(param, doc);

      this.service.Save(param);

      return GenericException.Serialize(0, I18n.Strings["msg_save_success"]);
    }
    #endregion

    #region 函数:Delete(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Delete(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      if (this.service.CanDelete(id))
      {
        this.service.Delete(id);

        return GenericException.Serialize(0, I18n.Strings["msg_delete_success"]);
      }
      else
      {
        return "{\"message\":{\"returnCode\":1,\"value\":\"此类别下还有相关业务数据，不能被删除。\"}}";
      }
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

      TaskCategoryInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(param) + ",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindAll(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindAll(XmlDocument doc)
    {
      IList<TaskCategoryInfo> list = this.service.FindAll();

      StringBuilder outString = new StringBuilder();

      outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(list) + ",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

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

      // 设置当前用户权限
      if (XmlHelper.Fetch("su", doc) == "1" && AppsSecurity.IsAdministrator(KernelContext.Current.User, TasksConfiguration.ApplicationName))
      {
        paging.Query.Variables["elevatedPrivileges"] = "1";
      }

      paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

      int rowCount = -1;

      IList<TaskCategoryInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(list) + ",");
      outString.Append("\"paging\":" + paging + ",");
      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
      outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
      outString.Append("\"total\":" + paging.RowCount + ",");
      outString.Append("\"success\":1,");
      outString.Append("\"msg\":\"success\"}");

      return outString.ToString();
    }
    #endregion

    #region 函数:IsExist(XmlDocument doc)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string IsExist(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      bool result = this.service.IsExist(id);

      return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
    }
    #endregion

    #region 函数:CreateNewObject(XmlDocument doc)
    /// <summary>创建新的对象</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string CreateNewObject(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      TaskCategoryInfo param = new TaskCategoryInfo();

      param.Id = DigitalNumberContext.Generate("Key_Guid");

      param.Status = 1;

      param.UpdateDate = param.CreateDate = DateTime.Now;

      outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(param) + ",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:SetStatus(XmlDocument doc)
    /// <summary>设置类别状态(停用/启用)</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string SetStatus(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      int status = Convert.ToInt32(XmlHelper.Fetch("status", doc));

      if (this.service.SetStatus(id, status))
      {
        return "{message:{\"returnCode\":0,\"value\":\"操作成功。\"}}";
      }
      else
      {
        return "{message:{\"returnCode\":1,\"value\":\"操作失败。\"}}";
      }
    }
    #endregion
  }
}
