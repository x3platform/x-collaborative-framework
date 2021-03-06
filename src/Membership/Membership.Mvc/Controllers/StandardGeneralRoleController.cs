﻿namespace X3Platform.Membership.Mvc.Controllers
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

    public class StandardGeneralRoleController : CustomController
    {
        private string APPLICATION_NAME = "Membership";

        #region 函数:List()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult List()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            // 角色
            return View("/views/main/membership/standard-general-role-list.cshtml");
        }
        #endregion

        #region 函数:Mapping()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Mapping()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            // 角色
            return View("/views/main/membership/standard-general-role-mapping.cshtml");
        }
        #endregion
    }
}
