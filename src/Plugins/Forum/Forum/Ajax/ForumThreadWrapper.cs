namespace X3Platform.Plugins.Forum.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Tasks;
  using X3Platform.DigitalNumber;
  using X3Platform.Util;
  using X3Platform.AttachmentStorage;

  using X3Platform.Plugins.Forum.IBLL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Location.IPQuery;
  using X3Platform.Apps.Model;
  using X3Platform.Configuration;
  using X3Platform.Apps;
  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Ajax.Net;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  public class ForumThreadWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IForumThreadService service = ForumContext.Instance.ForumThreadService;

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      ForumThreadInfo param = new ForumThreadInfo();

      param = (ForumThreadInfo)AjaxUtil.Deserialize(param, doc);

      if (string.IsNullOrEmpty(param.Title.Trim()))
      {
        return "{\"message\":{\"returnCode\":\"1\",\"value\":\"帖子标题不能为空。\"}}";
      }

      param.Id = param.Id == "" ? Guid.NewGuid().ToString() : param.Id;

      param.CommentCount = ForumContext.Instance.ForumCommentService.GetCommentCount(param.Id);

      // 是否有附件
      param.AttachmentFileCount = AttachmentStorageContext.Instance.AttachmentFileService.FindAllByEntityId(KernelContext.ParseObjectType(typeof(ForumThreadInfo)), param.Id).Count;

      // 查询最后回帖信息
      string lastCommentInfo = ForumContext.Instance.ForumCommentService.GetLastComment(param.Id);

      if (!string.IsNullOrEmpty(lastCommentInfo))
      {
        string[] info = lastCommentInfo.Split(',');
        param.LatestCommentAccountId = info[0];
        param.LatestCommentAccountName = info[1];
      }

      if (!string.IsNullOrEmpty(param.UpdateHistoryLog))
      {
        param.UpdateHistoryLog = "该内容已在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "被" + KernelContext.Current.User.Name + "编辑过。";
      }

      param.IP = IPQueryContext.GetClientIP();

      bool isNewObject = !this.service.IsExist(param.Id);

      // 保存操作
      this.service.Save(param);

      string message = "保存成功。";

      if (param.Status == 1)
      {
        ForumCategoryInfo categoryInfo = ForumContext.Instance.ForumCategoryService.FindOne(param.CategoryId);

        if (categoryInfo != null)
        {
          if (this.service.IsExist(param.Id) && !string.IsNullOrEmpty(param.UpdateHistoryLog))
          {
            message = "帖子修改成功。";
          }
          else
          {
            int score = categoryInfo == null ? 0 : categoryInfo.PublishThreadPoint;
            ForumContext.Instance.ForumMemberService.SetPoint(param.AccountId, score);
            ForumContext.Instance.ForumMemberService.SetThreadCount(param.AccountId);

            message = "帖子发布成功。";

            if (param.Anonymous == 0)
            {
              // 发送 Timeline 数据
              ApplicationInfo application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

              Uri actionUri = new Uri(KernelConfigurationView.Instance.HostName + "/api/timeline.save.aspx?client_id=" + application.Id + "&client_secret=" + application.ApplicationSecret);

              string taskCode = DigitalNumberContext.Generate("Key_Guid");

              string content = string.Format("发布了帖子【{0}】。<a href=\"{1}/forum/article/{2}.aspx\" target=\"_blank\" >网页链接</a>", param.Title, KernelConfigurationView.Instance.HostName, param.Id);

              string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <id><![CDATA[{0}]]></id>
    <applicationId><![CDATA[{1}]]></applicationId>
    <accountId><![CDATA[{2}]]></accountId>
    <content><![CDATA[{3}]]></content>
</request>
", taskCode, application.Id, KernelContext.Current.User.Id, content);

              // 发送请求信息
              AjaxRequestData reqeustData = new AjaxRequestData();

              reqeustData.ActionUri = actionUri;
              reqeustData.Args.Add("xml", xml);

              AjaxRequest.RequestAsync(reqeustData, null);
            }
          }
        }
      }

      return "{\"message\":{\"returnCode\":\"0\",\"value\":\"" + message + "\"}}";
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

      return "{message:{\"returnCode\":1,\"value\":\"删除成功。\"}}";
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

      ForumThreadInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<ForumThreadInfo>(param) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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

      DataQuery query = DataQuery.Create(XmlHelper.Fetch("query", doc, "xml"));

      string whereClause = XmlHelper.Fetch("whereClause", doc);

      int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

      IList<ForumThreadQueryInfo> list = this.service.FindAllQueryObject(query);

      outString.Append("{\"data\":" + AjaxUtil.Parse<ForumThreadQueryInfo>(list) + ",");

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

      IList<ForumThreadQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<ForumThreadQueryInfo>(list) + ",");
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

    #region 函数:IsExistByCategory(XmlDocument doc)
    /// <summary>根据版块查询是否存在相关的记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string IsExistByCategory(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("ids", doc);
      string applicationTag = XmlHelper.Fetch("applicationTag", doc);

      int code = 0;
      string result = "";

      if (ForumContext.Instance.ForumCategoryService.IsExistByParent(id) == true)
      {
        result = "该板块下有子版块，不能删除！";
      }
      else
      {
        if (this.service.IsExistByCategory(id) == true)
        {
          result = "该板块下有主题，不能删除！";
        }
      }

      if (result != "")
      {
        code = 1;
      }

      return "{\"message\":{\"returnCode\":" + code + ",\"value\":\"" + result + "\"}}";
    }
    #endregion

    #region 函数:SetEssential(XmlDocument doc)
    /// <summary>
    /// 设置精华帖或取消
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public string SetEssential(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);
      string isEssential = XmlHelper.Fetch("isEssential", doc);
      string applicationTag = XmlHelper.Fetch("applicationTag", doc);

      this.service.SetEssential(id, isEssential);

      return "{\"message\":{\"returnCode\":" + isEssential + ",\"value\":\"操作成功！\"}}";
    }
    #endregion

    #region 函数:SetTop(XmlDocument doc)
    /// <summary>
    /// 设置置顶或取消
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public string SetTop(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);
      string isTop = XmlHelper.Fetch("isTop", doc);
      string applicationTag = XmlHelper.Fetch("applicationTag", doc);

      this.service.SetTop(id, isTop);

      return "{\"message\":{\"returnCode\":" + isTop + ",\"value\":\"操作成功！\"}}";
    }
    #endregion

    #region 函数 RcommendToUser(XmlDocument doc)
    /// <summary>推荐给某人</summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public string RecommendToUser(XmlDocument doc)
    {
      string name = XmlHelper.Fetch("name", doc);
      string category = XmlHelper.Fetch("category", doc);
      string sendTaskUrl = XmlHelper.Fetch("sendTaskUrl", doc);
      string recommendAuthorizationObjectText = XmlHelper.Fetch("recommendAuthorizationObjectText", doc);

      string[] recommendAuthorizationObjectTextArray = recommendAuthorizationObjectText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < recommendAuthorizationObjectTextArray.Length; i++)
      {
        string[] authorizationArray = recommendAuthorizationObjectTextArray[i].Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

        //待办
        string applicationId = "00000000-0000-0000-0000-000000000178";
        string type = "4";
        string senderId = KernelContext.Current.User.Id;
        string receiverId = authorizationArray[1];
        string title = string.Format("[推荐]{0}推荐了{1}[{2}]", KernelContext.Current.User.Name, name, category);
        string content = sendTaskUrl;

        TasksContext.Instance.TaskWorkService.Send(applicationId, DigitalNumberContext.Generate("Key_Guid"), type, title, content, category, senderId, receiverId);
      }

      return ("{\"data\":\"\",\"message\":{\"returnCode\":0,\"value\":\"推荐成功。\"}}");
    }
    #endregion
  }
}
