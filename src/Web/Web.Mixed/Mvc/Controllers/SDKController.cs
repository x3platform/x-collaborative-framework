namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.Util;
    using X3Platform.Markdown;
    using X3Platform.Configuration;
    using System.Collections.Generic;
    using X3Platform.Velocity;
    using X3Platform.Web.Configuration;
    using X3Platform.Entities;

    /// <summary>在线开发辅助工具包</summary>
    public sealed partial class SDKController : Controller
    {
        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("/views/main/sdk/default.cshtml");
        }
        #endregion

        #region 函数:Doc()
        /// <summary>系统接口文档</summary>
        /// <returns></returns>
        public ActionResult Doc()
        {
            string applicationName = Request.QueryString["applicationName"];

            string methodName = ViewBag.methodName = Request.QueryString["methodName"];

            string entityClassName = ViewBag.methodName = Request.QueryString["entityClassName"];

            if (!string.IsNullOrEmpty(applicationName))
            {
                ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService.FindOneByApplicationName(applicationName);

                if (application == null)
                {
                    ApplicationError.Write(404);
                }

                if (string.IsNullOrEmpty(methodName))
                {
                    // 查看某个应用下的方法列表
                    ViewBag.methods = AppsContext.Instance.ApplicationMethodService.FindAllByApplicationName(applicationName);
                }
                else
                {
                    // 查看某个应用下的某个方法信息
                    ViewBag.method = AppsContext.Instance.ApplicationMethodService.FindOneByName(methodName);

                    // Create new markdown instance
                    Markdown mark = new Markdown();

                    // Run parser
                    ViewBag.detail = mark.Transform(ViewBag.method.Detail);
                }
            }
            else if (!string.IsNullOrEmpty(methodName))
            {
                // 查看方法详细信息
                ViewBag.method = AppsContext.Instance.ApplicationMethodService.FindOneByName(methodName);
                ViewBag.application = AppsContext.Instance.ApplicationService.FindOne(ViewBag.method.ApplicationId);

                // Create new markdown instance
                Markdown mark = new Markdown();

                // Run parser
                ViewBag.detail = mark.Transform(ViewBag.method.Detail);
            }
            else if (!string.IsNullOrEmpty(entityClassName))
            {
                // 查看实体类详细信息
                ViewBag.entitySchema = EntitiesManagement.Instance.EntitySchemaService.FindOneByEntityClassName(entityClassName);

                if (ViewBag.entitySchema != null)
                {
                    ViewBag.metaDatas = EntitiesManagement.Instance.EntityMetaDataService.FindAllByEntityClassName(entityClassName);
                }
            }
            else
            {
                IList<ApplicationInfo> applications = ViewBag.applications = AppsContext.Instance.ApplicationService.FindAll();

                VelocityContext context = new VelocityContext();

                context.Put("applications", applications);

                ViewBag.detail = VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/layouts/sdk-doc-default-content.vm");
            }

            return View("/views/main/sdk/doc.cshtml");
        }
        #endregion

        #region 函数:Uuid()
        /// <summary>UUID生成工具</summary>
        /// <returns></returns>
        public ActionResult Uuid()
        {
            ViewBag.UUID = Guid.NewGuid();

            return View("/views/main/sdk/uuid.cshtml");
        }
        #endregion

        #region 函数:Password()
        /// <summary>密码生成工具</summary>
        /// <returns></returns>
        public ActionResult Password()
        {
            ViewBag.StrongPassword = GeneratePassword();

            return View("/views/main/sdk/password.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Password(object args)
        {
            ViewBag.StrongPassword = GeneratePassword();

            return View("/views/main/sdk/password.cshtml");
        }
        #endregion

        private string GeneratePassword()
        {
            // 生成一个八位长度的字符串
            string password = Guid.NewGuid().ToString().Substring(0, 5);

            Random random = new Random();

            password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("!@#$", 1));

            password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("abcdefghijkmnpqrstuvwxyz", 1));

            password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("ABCDEFGHJKLMNPQRSTUVWXYZ", 1));

            return password;
        }
    }
}
