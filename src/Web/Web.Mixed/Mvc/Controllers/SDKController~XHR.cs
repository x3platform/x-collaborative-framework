
namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.IO;

    using X3Platform.Drawing.Captcha;

    /// <summary>搜索引擎优化辅助工具包</summary>
    public sealed partial class SDKController
    {
        #region 函数:XHR()
        /// <summary>Ajax 调试程序</summary>
        /// <returns></returns>
        public ActionResult XHR()
        {
            return View("/views/main/sdk/xhr.cshtml");
        }
        #endregion

        #region 函数:JSONP()
        /// <summary>JSONP 调试程序</summary>
        /// <returns></returns>
        public ActionResult JSONP()
        {
            return View("/views/main/sdk/jsonp.cshtml");
        }
        #endregion
    }
}
