namespace X3Platform.Plugins.Forum.Mvc.Controllers
{
  using System;
  using System.Web;
  using System.Web.Mvc;

  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.Configuration;
  using X3Platform.DigitalNumber;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Mvc.Controllers;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.Model;
  using System.Text;

  public class HomeController : CustomController
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult List()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      return View("/views/main/forum/default.cshtml");
    }
    #endregion

    #region 函数:Detail(string options)
    /// <summary>详细内容界面</summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public ActionResult Detail(string options)
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);

      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      // 管理员身份标记
      bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      // 实体数据标识
      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

      ForumThreadInfo param = null;

      if (!string.IsNullOrEmpty(id))
      {
        param = ForumContext.Instance.ForumThreadService.FindOne(id);
      }

      // 判断帖子信息是否存在，不存在就提示错误信息
      if (param == null)
      {
        // 提示信息不存在
        ApplicationError.Write(404);
      }

      // 是否是发布状态
      if (param.Status != 1)
      {
        return Redirect("forum/form?id=" + param.Id);
      }

      // 是否拥有查看帖子的权限
      if (!ForumContext.Instance.ForumCategoryService.IsCategoryAuthority(param.CategoryId) && !isAdminToken)
      {
        // 提示未经授权
        ApplicationError.Write(401);
      }

      // 修改浏览量
      param.Click++;

      ForumContext.Instance.ForumThreadService.SetClick(id);

      // -------------------------------------------------------
      // 数据加载
      // -------------------------------------------------------

      // 设置标题
      ViewBag.Title = string.Format("{0} - {1} - {2}", param.Title, application.ApplicationDisplayName, this.SystemName);

      // 个人信息
      ForumMemberInfo member = ForumContext.Instance.ForumMemberService.FindOneByAccountId(this.Account.Id);
      ViewBag.member = member;

      string categoryAdminName = param.CategoryAdminName;
      if (string.IsNullOrEmpty(categoryAdminName))
      {
        categoryAdminName = "网站管理员";
      }
      else
      {
        categoryAdminName = categoryAdminName.Substring(0, categoryAdminName.Length - 1);
      }

      // 版主名称
      ViewBag.categoryAdminName = categoryAdminName;

      // 是否是版主
      bool categoryAdministrator = false;

      if (isAdminToken == false)
      {
        categoryAdministrator = ForumContext.Instance.ForumCategoryService.IsCategoryAdministrator(param.CategoryId);
      }
      ViewBag.isAdminToken = isAdminToken;
      ViewBag.categoryAdministrator = categoryAdministrator;
      ViewBag.param = param;
      ViewBag.nowDate = DateTime.Now;
      // ViewBag.uploadFileWizard= KernelConfigurationView.Instance.UploadFileWizard;

      // 推荐给某人需要url地址
      // string requestUrl = Request.Url.ToString();
      // ViewBag.url = requestUrl.Substring(requestUrl.IndexOf("/forum/"));
      ViewBag.url = Request.Url.PathAndQuery;

      // 回帖编号
      ViewBag.CommentId = DigitalNumberContext.Generate("Key_Guid");

      ViewBag.CreatedDate = param.CreatedDate.ToString("yyyy-MM-dd HH:mm");

      string dayCountStr = AppsContext.Instance.ApplicationSettingService.GetValue(application.Id, string.Empty, "编辑有效天数").Trim().ToLower();
      ViewBag.dayCountStr = dayCountStr;

      /*
      
      // 开启企业模式
      string openEnterprise = AppsContext.Instance.ApplicationSettingService.GetValue(application.Id, string.Empty, "开启企业模式").Trim().ToLower();
      // 论坛电梯所需参数
      string showFloor = Request.Params["showFloor"];

      if (!string.IsNullOrEmpty(showFloor))
      {
        int rowIndex = 0;
        if (openEnterprise == "on" && info.Id != showFloor)
        {
          rowIndex = ForumContext.Instance.ForumCommentService.GetEnterpriseRowIndex(info.Id, showFloor, applicationTag);
        }
        else
        {
          rowIndex = ForumContext.Instance.ForumCommentService.GetRowIndex(info.Id, showFloor, applicationTag);
        }
        rowIndex = rowIndex == 0 ? 1 : rowIndex;
        int currentPage = (rowIndex / pageSize) + (rowIndex % pageSize > 0 ? 1 : 0);

        context.Put("currentPage", currentPage);
        context.Put("showFloor", showFloor);
      }
      */

      // 查看页面拷贝
      string appsStr1 = AppsContext.Instance.ApplicationSettingService.GetValue(application.Id, string.Empty, "帖子信息禁止拷贝");
      string appsStr2 = AppsContext.Instance.ApplicationSettingService.GetValue(application.Id, string.Empty, "管理员拷贝帖子信息");

      ViewBag.forbidCopy = appsStr1.Trim().ToLower() == "on" && appsStr2.Trim().ToLower() == "off" ? 1 : 0;

      return View("/views/main/forum/forum-thread-detail.cshtml");
    }
    #endregion

    private string RenderThreadView(string threadId)
    {
      StringBuilder outString = new StringBuilder();

      return string.Empty;
    }

    private string RenderCommentView(string threadId, string pageSize)
    {
      StringBuilder outString = new StringBuilder();

      return string.Empty;
    }
  }
}
