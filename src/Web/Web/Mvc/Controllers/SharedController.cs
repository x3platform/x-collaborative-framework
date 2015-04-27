namespace X3Platform.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;

    /// <summary>公共组件调试</summary>
    public sealed class SharedController : CustomController
    {
        private string APPLICATION_NAME = "Wizards";

        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            var path = Request.QueryString["path"];
            
            if (string.IsNullOrEmpty(path))
            {
                return View("~/views/shared/default.cshtml");
            }
            else
            {
                return View("~/views/shared/" + path + ".cshtml");
            }
        }
        #endregion
    }
}
