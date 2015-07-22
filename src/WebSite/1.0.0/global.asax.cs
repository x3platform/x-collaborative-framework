using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;

using Common.Logging;

using X3Platform.Data;

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
            // 配置 /applications/ 路由
            routes.Add("applications-route", new X3Platform.Apps.Mvc.RequestRoute());
            // 配置内置的路由 包括: /forum/
            routes.Add("forum-route", new X3Platform.Plugins.Forum.Mvc.RequestRoute());

            // 管理界面
            routes.MapRoute(
                // Route name
                "Home",
                // URL with parameters                         
                "",
                // Parameter defaults
                new { controller = "Home", action = "Index" },
                // NamespacesD:\github\x-collaborative-framework\src\WebSite\1.0.0\views\main\applications\
                new string[] { "X3Platform.Web.Mvc.Controllers" }
            );

            // 帐号基本操作
            routes.MapRoute(
                "帐号基本操作", // Route name
                "account/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
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
            logger.Info("application starting");

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // logger.Info("application end request");
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