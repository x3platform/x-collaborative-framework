namespace X3Platform.Drawing.Captcha.Web.Ajax
{
    #region Using Libraries
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml;
    using X3Platform.Security.VerificationCode;
    using X3Platform.Util;
    #endregion

    /// <summary>验证码</summary>
    public sealed class CaptchaWrapper
    {
        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>生成流水号</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string base64String = string.Empty;

            string key = XmlHelper.Fetch("key", doc);

            string widthText = XmlHelper.Fetch("width", doc);
            int width = string.IsNullOrEmpty(widthText) ? 100 : Convert.ToInt32(widthText);

            string heightText = XmlHelper.Fetch("height", doc);
            int height = string.IsNullOrEmpty(heightText) ? 50 : Convert.ToInt32(heightText);

            // 初始化验证码
            Captcha captcha = new Captcha(new
            {
                Width = width, // image width in pixels
                Height = height, // image height in pixels
                // Foreground = Color.Black, // font color; html color (#RRGGBB) or System.Drawing.Color
                Background = Color.White, // background color; html color (#RRGGBB) or System.Drawing.Color
                KeyLength = 5, // key length
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
                HttpContext.Current.Session["captcha"] = captcha.Key;
            }
            else
            {
                HttpContext.Current.Session["captcha-" + key] = captcha.Key;
            }

            return "{\"data\":{\"width\":" + captcha.Image.Width + ",\"height\":" + captcha.Image.Height + ",\"base64\":\"" + base64String + "\"},\"message\":{\"returnCode\":0,\"value\":\"创建成功。\"}}";
        }
        #endregion
    }
}
