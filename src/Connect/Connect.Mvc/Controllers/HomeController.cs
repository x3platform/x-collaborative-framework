namespace X3Platform.Connect.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using X3Platform.Configuration;
    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.Web.Mvc.Attributes;
    using X3Platform.Web.Mvc.Controllers;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Docs;
    using Util;
    [LoginFilter]
    public class HomeController : CustomController
    {
        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ConnectConfiguration.ApplicationName];

            return View("/views/main/connect/connect-list.cshtml");
        }
        #endregion

        #region 函数:Form(string options)
        /// <summary>表单内容界面</summary>
        /// <returns></returns>
        public ActionResult Form(string options)
        {
            JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

            string applicationName = JsonHelper.GetDataValue(request, "applicationName", ConnectConfiguration.ApplicationName);

            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[applicationName];

            // 管理员身份标记
            bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

            // -------------------------------------------------------
            // 业务数据处理
            // -------------------------------------------------------
            
            // 实体数据标识
            string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();
            // 文档编辑模式
            DocEditMode docEditMode = DocEditMode.Unkown;
            // 实体数据信息
            ConnectInfo param = null;

            if (string.IsNullOrEmpty(id))
            {
                param = new ConnectInfo();

                param.Id = param.AppKey = DigitalNumberContext.Generate("Key_Guid");
                param.AppSecret = Guid.NewGuid().ToString().Substring(0, 8);

                param.IsInternal = false;

                // 设置编辑模式【新建】
                docEditMode = DocEditMode.New;
            }
            else
            {
                param = ConnectContext.Instance.ConnectService.FindOne(id);

                if (param == null)
                {
                    ApplicationError.Write(404);
                }

                // 设置编辑模式【编辑】
                docEditMode = DocEditMode.Edit;
            }

            // -------------------------------------------------------
            // 数据加载
            // -------------------------------------------------------

            ViewBag.Title = string.Format("{0}-{1}-{2}", (string.IsNullOrEmpty(param.Name) ? "新应用" : param.Name), application.ApplicationDisplayName, this.SystemName);

            // 加载当前业务实体数据
            ViewBag.entityClassName = KernelContext.ParseObjectType(param.GetType());
            // 加载当前业务实体数据
            ViewBag.param = param;
            // 加载当前文档编辑模式
            ViewBag.docEditMode = docEditMode;

            // 视图
            return View("/views/main/connect/connect-form.cshtml");
        }
        #endregion

        #region 函数:Detail(string options)
        /// <summary>详细内容界面</summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public ActionResult Detail(string options)
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ConnectConfiguration.ApplicationName];

            // 管理员身份标记
            bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

            JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

            // 实体数据标识
            string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

            ConnectInfo param = null;

            if (!string.IsNullOrEmpty(id))
            {
                param = ConnectContext.Instance.ConnectService.FindOne(id);
            }

            if (param == null)
            {
                ApplicationError.Write(404);
            }

            // -------------------------------------------------------
            // 数据加载
            // -------------------------------------------------------

            ViewBag.Title = string.Format("{0}-{1}-{2}", param.Name, application.ApplicationDisplayName, this.SystemName);

            // 加载当前业务实体数据
            ViewBag.entityClassName = KernelContext.ParseObjectType(param.GetType());
            // 加载当前业务实体数据
            ViewBag.param = param;

            return View("/views/main/connect/connect-detail.cshtml");
        }
        #endregion

        #region 函数:Overview(string options)
        /// <summary>详细内容界面</summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public ActionResult Overview(string options)
        {
            JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

            string applicationName = JsonHelper.GetDataValue(request, "applicationName", ConnectConfiguration.ApplicationName);
            
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[applicationName];

            // 管理员身份标记
            bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

            // 实体数据标识
            string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

            ConnectInfo param = null;

            if (!string.IsNullOrEmpty(id))
            {
                param = ConnectContext.Instance.ConnectService.FindOne(id);
            }

            if (param == null)
            {
                ApplicationError.Write(404);
            }

            // -------------------------------------------------------
            // 数据加载
            // -------------------------------------------------------

            ViewBag.Title = string.Format("{0}-{1}-{2}", param.Name, application.ApplicationDisplayName, this.SystemName);

            // 加载当前业务实体数据
            ViewBag.entityClassName = KernelContext.ParseObjectType(param.GetType());
            // 加载当前业务实体数据
            ViewBag.param = param;

            return View("/views/main/connect/connect-overview.cshtml");
        }
        #endregion
    }
}
