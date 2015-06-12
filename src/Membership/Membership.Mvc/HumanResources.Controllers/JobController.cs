namespace X3Platform.Membership.Mvc.HumanResources.Controllers
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

    public class JobController : CustomController
    {
        private string APPLICATION_NAME = "HumanResources";

        #region 函数:List()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult List()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            // 组织
            return View("/views/main/membership/account-list.cshtml");
        }
        #endregion
    }
}
