using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;

using Common.Logging;

using X3Platform.Data;
using System.Globalization;
using System.Threading;
using X3Platform.Configuration;
using X3Platform.Globalization;

namespace X3Platform.WebSiteV10
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      // 配置内置的路由 包括: /customizes/ | /account/
      routes.Add("inner-route", new X3Platform.Web.Mvc.RequestRoute());
      // 配置 /api/ 路由
      routes.Add("api-methods", new X3Platform.Web.APIs.Mvc.APIRoute());
      // 配置 /connect/ 路由
      routes.Add("connect-request-route", new X3Platform.Connect.Mvc.RequestRoute());
      // 配置 /membership/ 路由
      routes.Add("membership-request-route", new X3Platform.Membership.Mvc.RequestRoute());
      // 配置 /applications/ 路由
      routes.Add("applications-request-route", new X3Platform.Apps.Mvc.RequestRoute());
      // 配置 /entities/ 路由
      routes.Add("entities-request-route", new X3Platform.Entities.Mvc.RequestRoute());
      // 配置 /attachment/ 路由
      routes.Add("attachment-storage-request-route", new X3Platform.AttachmentStorage.Mvc.RequestRoute());
      // 配置 /tasks/ 路由
      routes.Add("tasks-request-route", new X3Platform.Tasks.Mvc.RequestRoute());
      // 配置 /bugs/ 路由
      routes.Add("bugs-request-route", new X3Platform.Plugins.Bugs.Mvc.RequestRoute());
      // 配置 /forum/ 路由
      routes.Add("forum-request-route", new X3Platform.Plugins.Forum.Mvc.RequestRoute());

      // 默认主页
      routes.MapRoute(
        "Home",// Route name
        "",// URL with parameters
        new { controller = "Home", action = "Index" }, // Parameter defaults
        new string[] { "X3Platform.Web.Mvc.Controllers" }
      );

      // 默认地址
      routes.MapRoute(
          "Default", // Route name
          "{controller}/{action}/{id}", // URL with parameters
          new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
          new string[] { "X3Platform.Web.Mvc.Controllers" }
      );
    }

    protected void Application_Start()
    {
      logger.Info(I18n.Strings["application starting"]);
      
      AreaRegistration.RegisterAllAreas();

      RegisterGlobalFilters(GlobalFilters.Filters);

      RegisterRoutes(RouteTable.Routes);

      I18nScript.Instance.Init();
    }

    protected void Application_EndRequest(object sender, EventArgs e)
    {
      // logger.Info("application end request");
    }

    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {
      // 设置当前语言信息
      if (HttpContext.Current.Session != null)
      {
        CultureInfo culture = (CultureInfo)this.Session["CurrentCulture"];

        if (culture == null)
        {
          string cultureName = KernelConfigurationView.Instance.CultureName;

          if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length > 0)
          {
            cultureName = HttpContext.Current.Request.UserLanguages[0];
          }

          culture = new CultureInfo(cultureName);

          this.Session["CurrentCulture"] = culture;
        }

        Thread.CurrentThread.CurrentUICulture = culture;

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture.Name);
      }
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      // 获取错误
      Exception ex = Server.GetLastError().GetBaseException();

      logger.Error(ex);

      if (ex is GenericSqlConnectionException)
      {
        Response.Write(ex.Message);
        Response.End();
      }
    }
  }
}