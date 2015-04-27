using System;
using System.Web.Mvc;
using System.Web.Security;

using X3Platform.Apps;
using X3Platform.Configuration;
using X3Platform.Json;
using X3Platform.Location.IPQuery;
using X3Platform.Membership;
using X3Platform.Membership.Authentication;
using X3Platform.Web.Mvc.Attributes;

namespace X3Platform.Web.Mvc.Controllers
{
    [LoginFilter]
    public sealed class PlatformController : CustomController
    {
        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("~/views/main/platform/default.cshtml");
        }
        #endregion
    }
}
