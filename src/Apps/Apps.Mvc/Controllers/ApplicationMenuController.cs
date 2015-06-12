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
  using X3Platform.Apps.Model;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Util;

  [LoginFilter]
  public class ApplicationMenuController : CustomController
  {
    private string APPLICATION_NAME = "ApplicationManagement";

    #region 函数:List()
    /// <summary>列表</summary>
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

      return View("/views/main/applications/application-menu-list.cshtml");
    }
    #endregion

    #region 函数:Form(string options)
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

      ApplicationMenuInfo param = new ApplicationMenuInfo();

      if (string.IsNullOrEmpty(id))
      {
        string applicationId = JsonHelper.GetDataValue(request, "applicationId", "00000000-0000-0000-0000-000000000001");
        string menuId = JsonHelper.GetDataValue(request, "menuId", "00000000-0000-0000-0000-000000000000");
        string menuType = JsonHelper.GetDataValue(request, "menuType", "00000000-0000-0000-0000-000000000000");

        param.Id = DigitalNumberContext.Generate("Key_Guid");
        param.ApplicationId = applicationId;
        param.ParentId = menuId;
        param.MenuType = menuType;
        param.Status = 1;
      }
      else
      {
        param = AppsContext.Instance.ApplicationMenuService.FindOne(id);
      }

      ViewBag.param = param;

      return View("/views/main/applications/application-menu-form.cshtml");
    }
    #endregion
  }
}
