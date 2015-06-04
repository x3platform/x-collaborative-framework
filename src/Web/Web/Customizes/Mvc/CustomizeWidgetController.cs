namespace X3Platform.Web.Mvc.Controllers
{
  using System.Web.Mvc;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Web.Configuration;
  using X3Platform.Web.Mvc.Attributes;

  /// <summary>部件信息</summary>
  public sealed class CustomizeWidgetController : CustomController
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    [LoginFilter]
    public ActionResult List()
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

      return View("/views/main/customizes/customize-widget-list.cshtml");
    }
    #endregion
  }
}
