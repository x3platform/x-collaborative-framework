using System.Web.Mvc;

namespace X3Platform.Web.Mvc.Controllers
{
    /// <summary>公共组件调试</summary>
    public sealed class SharedController : CustomController
    {
        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var path = Request.QueryString["path"];
            
            if (string.IsNullOrEmpty(path))
            {
                return View("/views/shared/default.cshtml");
            }
            else
            {
                return View("/views/shared/" + path + ".cshtml");
            }
        }
        #endregion
    }
}
