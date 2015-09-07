namespace X3Platform.Connect.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;

  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Controllers;
  using X3Platform.Web.Mvc.Attributes;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  
  using X3Platform.Connect.Configuration;

  [LoginFilter]
  public class ConnectSettingController : CustomController
  {
    #region 函数:List()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult List()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ConnectConfiguration.ApplicationName];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      // 视图
      return View("/views/main/connect/connect-setting-list.cshtml");
    }
    #endregion
  }
}
