
namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.IO;

    using X3Platform.Drawing.Captcha;
    using X3Platform.Security;
    using X3Platform.Util;
    using X3Platform.DigitalNumber;

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

        #region 函数:XHRSignature()
        /// <summary>异步请求的签名</summary>
        /// <returns></returns>
        public ActionResult XHRSignature()
        {
            string appSecret = Request.QueryString["appSecret"];
            string timestamp = ViewBag.timestamp = Request.QueryString["timestamp"] == null ? DateHelper.GetTimestamp().ToString() : Request.QueryString["timestamp"];
            string nonce = ViewBag.nonce = Request.QueryString["nonce"] == null ? DigitalNumberContext.Generate("Key_Nonce") : Request.QueryString["nonce"];

            if (!string.IsNullOrEmpty(appSecret))
            {
                ViewBag.appSignature = Encrypter.EncryptSHA1(Encrypter.SortAndConcat(appSecret, timestamp, nonce));
            }
            return View("/views/main/sdk/xhrsignature.cshtml");
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
