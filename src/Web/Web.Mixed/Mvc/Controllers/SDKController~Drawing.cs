namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.IO;

    using X3Platform.Drawing.Captcha;
    using X3Platform.Security;

    /// <summary>在线开发辅助工具包</summary>
    public sealed partial class SDKController : Controller
    {
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

        #region 函数:QRCode()
        /// <summary>Base64 加密</summary>
        /// <returns></returns>
        public ActionResult QRCode()
        {
            return View("/views/main/sdk/qrcode.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult QRCode(object args)
        {
            var text = ViewBag.OriginalText = Request["text"];

            var ciphertext = ViewBag.Ciphertext = string.IsNullOrEmpty(text) ? string.Empty : Encrypter.EncryptSHA1(text);

            return View("/views/main/sdk/qrcode.cshtml");
        }
        #endregion
    }
}
