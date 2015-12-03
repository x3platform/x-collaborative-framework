namespace X3Platform.Drawing.Captcha.Mvc.Controllers
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using X3Platform.Drawing.Captcha;
    using X3Platform.Util;

    /// <summary></summary>
    public class CaptchaController : Controller
    {
        /// <summary></summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StringBuilder outString = new StringBuilder();

            string base64String = string.Empty;

            string key = Request["key"];
            int width = Request["width"] == null ? 100 : Convert.ToInt32(Request["width"]);
            int height = Request["height"] == null ? 50 : Convert.ToInt32(Request["height"]);

            // 初始化验证码
            Captcha captcha = new Captcha(new
            {
                Width = width, // image width in pixels
                Height = height, // image height in pixels
               // Foreground = "black", // font color; html color (#RRGGBB) or System.Drawing.Color
                Background = Color.White, // background color; html color (#RRGGBB) or System.Drawing.Color
                // KeyLength = 5, // key length
                Waves = true, // enable waves filter (distortions)
                Overlay = true // enable overlaying
            });

            using (MemoryStream stream = new MemoryStream())
            {
                captcha.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                byte[] buffer = new byte[stream.Length];

                stream.Position = 0;
                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();

                base64String = Convert.ToBase64String(buffer);
            }

            if (string.IsNullOrEmpty(key))
            {
                Session["captcha"] = captcha.Key;
            }
            else
            {
                Session["captcha-" + key] = captcha.Key;
            }

            return Content("{\"data\":{\"width\":" + captcha.Image.Width + ",\"height\":" + captcha.Image.Height + ",\"base64\":\"" + base64String + "\"},\"message\":{\"returnCode\":0,\"value\":\"创建成功。\"}}");
        }
    }
}
