namespace X3Platform.Plugins.Bugs.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Xml;

  using X3Platform.Ajax;
  using X3Platform.Util;

  using X3Platform.Plugins.Bugs.IBLL;
  using X3Platform.Plugins.Bugs.Model;
    using X3Platform.Globalization;
  #endregion

  public class BugCommentWrapper : ContextWrapper
  {
    private IBugCommentService service = BugContext.Instance.BugCommentService; // 数据服务

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      BugCommentInfo param = new BugCommentInfo();

      param = (BugCommentInfo)AjaxUtil.Deserialize(param, doc);

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
      string ids = XmlHelper.Fetch("id", doc);

      this.service.Delete(ids);

      return GenericException.Serialize(0, I18n.Strings["msg_delete_success"]);
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

      BugCommentInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<BugCommentInfo>(param) + ",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindAll(XmlDocument doc)
    /// <summary>获取列表内容</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindAll(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = XmlHelper.Fetch("whereClause", doc);
      int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

      IList<BugCommentInfo> list = this.service.FindAll(whereClause, length);

      outString.Append("{\"data\":" + AjaxUtil.Parse<BugCommentInfo>(list) + ",");

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

      PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"));

      int rowCount = -1;

      IList<BugCommentInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<BugCommentInfo>(list) + ",");

      outString.Append("\"paging\":" + paging + ",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion
  }
}