namespace X3Platform.Apps.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;

  using X3Platform.Apps;
  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.DigitalNumber;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Controllers;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Apps.Model;
  using X3Platform.Util;

  [LoginFilter]
  public class ApplicationController : CustomController
  {
    private string APPLICATION_NAME = "ApplicationManagement";

    #region 函数:List()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult List()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      // 组织
      return View("/views/main/applications/application-list.cshtml");
    }
    #endregion

    #region 函数:Form()
    /// <summary>表单</summary>
    /// <returns></returns>
    public ActionResult Form(string options)
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      // -------------------------------------------------------
      // 数据加载
      // -------------------------------------------------------

      JsonData request = JsonMapper.ToObject(options);

      string id = JsonHelper.GetDataValue(request, "id");

      ApplicationInfo param = new ApplicationInfo();

      if (string.IsNullOrEmpty(id))
      {
        param.Id = DigitalNumberContext.Generate("Key_Guid");
        param.Status = 1;
      }
      else
      {
        param = AppsContext.Instance.ApplicationService.FindOne(id);
      }

      ViewBag.param = param;

      // -------------------------------------------------------
      // 视图加载
      // -------------------------------------------------------

      return View("/views/main/applications/application-form.cshtml");
    }
    #endregion
  }
}
