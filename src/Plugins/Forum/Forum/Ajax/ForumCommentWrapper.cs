namespace X3Platform.Plugins.Forum.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.DigitalNumber;
  using X3Platform.Util;
  using X3Platform.AttachmentStorage;

  using X3Platform.Plugins.Forum.IBLL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Location.IPQuery;
    using X3Platform.Globalization;
  #endregion

  /// <summary></summary>
  public class ForumCommentWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IForumCommentService service = ForumContext.Instance.ForumCommentService;

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      ForumCommentInfo param = new ForumCommentInfo();

      param = (ForumCommentInfo)AjaxUtil.Deserialize(param, doc);
      param.IP = IPQueryContext.GetClientIP();
      param.AccountId = KernelContext.Current.User.Id;
      param.AccountName = KernelContext.Current.User.Name;
      param.ModifiedDate = System.DateTime.Now;
      param.CreatedDate = System.DateTime.Now;

      // 是否有附件
      param.AttachmentFileCount = AttachmentStorageContext.Instance.AttachmentFileService.FindAllByEntityId(KernelContext.ParseObjectType(typeof(ForumCommentInfo)), param.Id).Count;

      string categoryId = XmlHelper.Fetch("categoryId", doc);

      // 保存操作
      this.service.Save(param);

      string tipInfo = "保存成功!";

      ForumCategoryInfo categoryInfo = ForumContext.Instance.ForumCategoryService.FindOne(categoryId);

      if (categoryInfo != null)
      {
        int score = categoryInfo.PublishCommentPoint;
        ForumContext.Instance.ForumMemberService.SetPoint(param.AccountId, 1);
        tipInfo = "回复帖子成功，积分+" + score + "！";
      }

      return "{\"message\":{\"returnCode\":0,\"value\":\"" + tipInfo + "\",\"commentId\":\"" + Guid.NewGuid().ToString() + "\"}}";
    }
    #endregion

    #region 函数:Delete(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Delete(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      this.service.Delete(id);

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
      string applicationTag = XmlHelper.Fetch("applicationTag", doc);

      ForumCommentInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCommentInfo>(param) + ",");

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
      StringBuilder outString = new StringBuilder();

      string whereClause = XmlHelper.Fetch("whereClause", doc);
      int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));
      string applicationTag = XmlHelper.Fetch("applicationTag", doc);

      IList<ForumCommentInfo> list = this.service.FindAll(whereClause, length);

      outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCommentInfo>(list) + ",");

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

      int rowCount = -1;

      IList<ForumCommentQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCommentQueryInfo>(list) + ",");
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
      string applicationTag = XmlHelper.Fetch("applicationTag", doc);

      bool result = this.service.IsExist(id);

      return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
    }
    #endregion
  }
}
