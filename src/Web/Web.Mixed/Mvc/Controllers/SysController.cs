namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Security;
    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.Configuration;
    using X3Platform.Json;
    using X3Platform.Location.IPQuery;
    using X3Platform.Membership;
    using X3Platform.Membership.Authentication;
    using X3Platform.Web.Mvc.Attributes;

    [LoginFilter]
    public sealed class SysController : CustomController
    {
        const string APPLICATION_NAME = "ConfigurationManagement";

        #region 函数:Environment()
        /// <summary>系统环境</summary>
        /// <returns></returns>
        public ActionResult Environment()
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

            //服务器名称和地址

            IPHostEntry server = Dns.GetHostEntry(Dns.GetHostName());

            ViewBag.HostName = server.HostName.ToString();

            ViewBag.Address = server.AddressList.GetValue(0).ToString();

            //操作系统版本信息
            ViewBag.OSVersion = System.Environment.OSVersion.ToString();

            // .NET Framework 版本号
            ViewBag.FrameworkVersion = System.Environment.Version.ToString();

            // 系统文件夹目录
            ViewBag.SystemDirectory = System.Environment.SystemDirectory;

            // 系统文件夹目录
            ViewBag.ProcessorCount = System.Environment.ProcessorCount;
            ViewBag.Is64BitOperatingSystem = System.Environment.Is64BitOperatingSystem;

            //系统已运行时间

            int timeCounter = System.Environment.TickCount;

            timeCounter = timeCounter / 1000;

            ViewBag.TickTime = ((timeCounter / 3600) % 60).ToString() + "小时" + ((timeCounter / 60) % 60).ToString() + "分钟" + (timeCounter % 60).ToString() + "秒";

            // 当前登录的帐号

            ViewBag.UserName = System.Environment.UserName;

            //检测已安装字体
            /*
            StringBuilder outString = new StringBuilder();

            InstalledFontCollection fontCollection = new InstalledFontCollection();

            FontFamily[] fontFamily = fontCollection.Families;

            for (int i = 0; i < fontFamily.Length; i++)
            {
              switch (fontFamily[i].Name)
              {
                //	Arial | Courier New | Georgia | 宋体 | 黑体
                case "Arial":
                  outString.Append(" [Arial] ");
                  break;
                case "Courier New":
                  outString.Append(" [Courier New] ");
                  break;
                case "Georgia":
                  outString.Append(" [Georgia] ");
                  break;
                case "Trebuchet MS":
                  outString.Append(" [Trebuchet MS] ");
                  break;
                case "宋体":
                  outString.Append(" [宋体] ");
                  break;
                case "黑体":
                  outString.Append(" [黑体] ");
                  break;
                default:
                  break;
              }
            }

            ViewBag.FontStyle = outString.ToString();
            */
            //主机域名

            ViewBag.HostName = Request.ServerVariables["HTTP_HOST"];

            //端口

            ViewBag.ServerPort = Request.ServerVariables["SERVER_PORT"];

            //站点文件夹

            ViewBag.PhysicalPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];

            ViewBag.configuration = KernelConfigurationView.Instance;

            return View("~/views/main/sys/environment.cshtml");
        }
        #endregion

        #region 函数:Variables()
        /// <summary>环境变量</summary>
        /// <returns></returns>
        public ActionResult Variables()
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

            ViewBag.options = KernelConfigurationView.Instance.Configuration.Keys;

            return View("/views/main/sys/variables.cshtml");
        }
        #endregion

        #region 函数:Caches()
        /// <summary>缓存设置</summary>
        /// <returns></returns>
        public ActionResult Caches()
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

            return View("~/views/main/sys/caches.cshtml");
        }
        #endregion

        #region 函数:Sessions()
        /// <summary>会话设置</summary>
        /// <returns></returns>
        public ActionResult Sessions()
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
            // 刷新缓存数据
            // -------------------------------------------------------

            string reset = Request.QueryString["reset"];

            if (reset == "1")
            {
                KernelContext.Current.AuthenticationManagement.ResetSessions();
            }

            string removeKey = Request.QueryString["removeKey"];

            if (!string.IsNullOrEmpty(removeKey))
            {
                KernelContext.Current.AuthenticationManagement.RemoveSession(removeKey);
                Response.Redirect("/sys/sessions");
                Response.End();
            }

            // -------------------------------------------------------
            // 加载数据
            // -------------------------------------------------------

            StringBuilder outString = new StringBuilder();

            IDictionary<string, IAccountInfo> dictionary = ViewBag.dictionary = KernelContext.Current.AuthenticationManagement.GetSessions();

            return View("/views/main/sys/sessions.cshtml");
        }
        #endregion

        #region 函数:Authorities()
        /// <summary>权限参数设置</summary>
        /// <returns></returns>
        public ActionResult Authorities()
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
            // 加载数据
            // -------------------------------------------------------

            return View("/views/main/sys/authorities.cshtml");
        }
        #endregion

        #region 函数:DigitalNumber()
        /// <summary>流水号设置</summary>
        /// <returns></returns>
        public ActionResult DigitalNumber()
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
            // 加载数据
            // -------------------------------------------------------

            return View("/views/main/sys/digital-number.cshtml");
        }
        #endregion

        #region 函数:EmailClient()
        /// <summary>邮箱设置</summary>
        /// <returns></returns>
        public ActionResult EmailClient()
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
            // 加载数据
            // -------------------------------------------------------

            return View("/views/main/sys/email-client.cshtml");
        }
        #endregion

        #region 函数:EmailHistory()
        /// <summary>邮件发送日志</summary>
        /// <returns></returns>
        public ActionResult EmailHistory()
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
            // 加载数据
            // -------------------------------------------------------

            return View("/views/main/sys/email-history.cshtml");
        }
        #endregion

        #region 函数:VerificationCodeHistory()
        /// <summary>验证码发送日志</summary>
        /// <returns></returns>
        public ActionResult VerificationCodeHistory()
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
            // 加载数据
            // -------------------------------------------------------

            return View("/views/main/sys/verification-code-history.cshtml");
        }
        #endregion
    }
}
