
namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.IO;

    using X3Platform.Drawing.Captcha;

    /// <summary>搜索引擎优化辅助工具包</summary>
    public sealed partial class SDKController
    {
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

        #region 函数:IP()
        /// <summary>客户端的 UserAgent 信息</summary>
        /// <returns></returns>
        public ActionResult IP()
        {
            // 用户代理信息
            var ua = ViewBag.UserAgent = Request.UserAgent;
            // 客户端信息
            var client = ViewBag.Client = X3Platform.Web.UserAgents.Parser.GetDefault().Parse(ua);
            // 设备类型
            var deviceType = ViewBag.DeviceType = X3Platform.Web.UserAgents.DeviceTypeParser.Parse(client);

            return View("/views/main/sdk/ip.cshtml");
        }
        #endregion

        #region 函数:PR()
        /// <summary>客户端的 UserAgent 信息</summary>
        /// <returns></returns>
        public ActionResult PR()
        {
            // 用户代理信息
            var ua = ViewBag.UserAgent = Request.UserAgent;
            // 客户端信息
            var client = ViewBag.Client = X3Platform.Web.UserAgents.Parser.GetDefault().Parse(ua);
            // 设备类型
            var deviceType = ViewBag.DeviceType = X3Platform.Web.UserAgents.DeviceTypeParser.Parse(client);

            return View("/views/main/sdk/seo/ip.cshtml");
        }
        #endregion

        #region 函数:Whois()
        /// <summary>验证码</summary>
        /// <returns></returns>
        public ActionResult Whois()
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

        #region 函数:words()
        /// <summary>关键字</summary>
        /// <returns></returns>
        public ActionResult words()
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
    }
}
