using System;
using System.Web.Mvc;
using System.Web.Security;

using X3Platform.Apps;
using X3Platform.Configuration;
using X3Platform.Json;
using X3Platform.Location.IPQuery;
using X3Platform.Membership;
using X3Platform.Membership.Authentication;
using System.Drawing.Imaging;
using X3Platform.Drawing.Captcha;
using System.IO;

namespace X3Platform.Web.Mvc.Controllers
{
    /// <summary>在线开发辅助工具包</summary>
    public sealed class SDKController : Controller
    {
        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("/views/main/platform/default.cshtml");
        }
        #endregion

        #region 函数:UserAgent()
        /// <summary>客户端的 UserAgent 信息</summary>
        /// <returns></returns>
        public ActionResult UserAgent()
        {
            // 用户代理信息
            var ua = ViewBag.UserAgent = Request.UserAgent;
            // 客户端信息
            var client = ViewBag.Client = X3Platform.Web.UserAgents.Parser.GetDefault().Parse(ua);
            // 设备类型
            var deviceType = ViewBag.DeviceType = X3Platform.Web.UserAgents.DeviceTypeParser.Parse(client);

            return View("/views/main/sdk/useragent.cshtml");
        }
        #endregion

        #region 函数:Captcha()
        /// <summary>验证码</summary>
        /// <returns></returns>
        public ActionResult Captcha()
        {
            Captcha captcha = new Captcha(new
            {
                waves = true
            });

            ViewBag.Image = captcha.Image;

            using (MemoryStream stream = new MemoryStream())
            {
                captcha.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                byte[] buffer = new byte[stream.Length];

                stream.Position = 0;
                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();

                ViewBag.ImageBase64String = Convert.ToBase64String(buffer);
            }

            return View("/views/main/sdk/captcha.cshtml");
        }
        #endregion

        #region 函数:Sha1()
        /// <summary>客户端的 UserAgent 信息</summary>
        /// <returns></returns>
        public ActionResult Sha1()
        {
            return View("/views/main/sdk/sha1.cshtml");
        }
        #endregion

        #region 函数:MD5()
        /// <summary>客户端的 UserAgent 信息</summary>
        /// <returns></returns>
        public ActionResult MD5()
        {
            return View("/views/main/sdk/useragent.cshtml");
        }
        #endregion
    }
}
