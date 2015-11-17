namespace X3Platform.Web.Customizes.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Json;
    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customizes.Model;
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

            CustomizePageInfo param = null;

            if (string.IsNullOrEmpty(id))
            {
                param = new CustomizePageInfo();

                param.Id = param.Name = DigitalNumberContext.Generate("Key_Guid");

                param.Html = CustomizeContext.Instance.CustomizeLayoutService.GetHtml("default");

                param.CreateDate = param.UpdateDate = DateTime.Now;
            }
            else
            {
                param = CustomizeContext.Instance.CustomizePageService.FindOne(id);
            }

            ViewBag.param = param;

            return View("/views/main/customizes/customize-page-form.cshtml");
        }
        #endregion

        #region 函数:Detail(string options)
        /// <summary>详细内容界面</summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public ActionResult Detail(string options)
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[WebConfiguration.APP_NAME_CUSTOMIZES];

            // -------------------------------------------------------
            // 业务数据处理
            // -------------------------------------------------------

            JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

            // 实体数据标识
            string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

            // 加载当前业务实体数据
            var param = ViewBag.param = CustomizeContext.Instance.CustomizePageService.FindOne(id);

            ViewBag.outputHtml = CustomizeContext.Instance.CustomizePageService.GetHtml(param.Name);

            return View("/views/main/customizes/customize-page-detail.cshtml");
        }
        #endregion

    }
}
