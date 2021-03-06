﻿namespace X3Platform.Web.Customizes.Mvc.Controllers
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
    public sealed class CustomizeLayoutController : CustomController
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

            return View("/views/main/customizes/customize-layout-list.cshtml");
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

            CustomizeLayoutInfo param = null;

            if (string.IsNullOrEmpty(id))
            {
                param = new CustomizeLayoutInfo();

                param.Id = DigitalNumberContext.Generate("Key_Guid");

                param.CreateDate = param.UpdateDate = DateTime.Now;
            }
            else
            {
                param = CustomizeContext.Instance.CustomizeLayoutService.FindOne(id);
            }

            ViewBag.param = param;

            return View("/views/main/customizes/customize-layout-form.cshtml");
        }
        #endregion
    }
}
