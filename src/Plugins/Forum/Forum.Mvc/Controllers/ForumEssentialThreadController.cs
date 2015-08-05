namespace X3Platform.Plugins.Forum.Mvc.Controllers
{
  using System;
  using System.Web;
  using System.Web.Mvc;

  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Mvc.Controllers;
  using X3Platform.Util;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  using X3Platform.Plugins.Forum.Configuration;

  [LoginFilter]
  public class ForumEssentialThreadController : CustomController
  {
    #region 函数:List()
    /// <summary>列表</summary>
    /// <returns></returns>
    public ActionResult List()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, ForumConfiguration.ApplicationName);

      return View("/views/main/forum/forum-essential-thread-list.cshtml");
    }
    #endregion
  }
}
