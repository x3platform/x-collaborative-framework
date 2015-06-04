using System.Web.Mvc;

namespace X3Platform.Web.Customizes.Mvc.Controllers
{
  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Web.Configuration;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Mvc.Controllers;

  /// <summary>自定义内容信息</summary>
  public sealed class CustomizeContentController : CustomController
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    [LoginFilter]
    public ActionResult Index()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[WebConfiguration.APP_NAME_CUSTOMIZES];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      return View("/views/main/customizes/customize-content-list.cshtml");
    }
    #endregion
  }
}
