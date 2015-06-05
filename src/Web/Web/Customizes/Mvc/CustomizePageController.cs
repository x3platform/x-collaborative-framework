namespace X3Platform.Web.Customizes.Mvc.Controllers
{
  using System.Web.Mvc;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Json;
  using X3Platform.Web.Configuration;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Mvc.Controllers;

  /// <summary>自定义内容信息</summary>
  [LoginFilter]
  public sealed class CustomizePageController : CustomController
  {
    #region 函数:List()
    /// <summary>列表</summary>
    /// <returns></returns>
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

      return View("/views/main/customizes/customize-page-list.cshtml");
    }
    #endregion

    #region 函数:Form()
    /// <summary>表单</summary>
    /// <returns></returns>
    public ActionResult Form(string options)
    {
      // 测试页面 customizes/customize-page/form?id=442049bb-9bb3-49cc-8a30-2454a56c770e

      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[WebConfiguration.APP_NAME_CUSTOMIZES];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      // -------------------------------------------------------
      // 业务数据处理
      // -------------------------------------------------------

      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      // 实体数据标识
      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

      var customizePage = ViewBag.customizePage = CustomizeContext.Instance.CustomizePageService.FindOne(id);

      return View("/views/main/customizes/customize-page-form.cshtml");
    }
    #endregion
  }
}
