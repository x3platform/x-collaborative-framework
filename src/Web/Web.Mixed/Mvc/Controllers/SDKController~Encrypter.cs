namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using X3Platform.Security;
    using X3Platform.Security.Configuration;
    using System.Security.Cryptography;
    using X3Platform.Util;
    using X3Platform.Configuration;

    /// <summary>在线开发辅助工具包</summary>
    public sealed partial class SDKController
    {
        #region 函数:MD5()
        /// <summary>MD5 加密</summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MD5()
        {
            return View("/views/main/sdk/md5.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MD5(object args)
        {
            var text = ViewBag.OriginalText = Request["text"];

            var ciphertext = ViewBag.Ciphertext = string.IsNullOrEmpty(text) ? string.Empty : Encrypter.EncryptMD5(text);

            return View("/views/main/sdk/md5.cshtml");
        }
        #endregion

        #region 函数:Sha1()
        /// <summary>Sha1 加密</summary>
        /// <returns></returns>
        public ActionResult Sha1()
        {
            return View("/views/main/sdk/sha1.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sha1(object args)
        {
            var text = ViewBag.OriginalText = Request["text"];

            var ciphertext = ViewBag.Ciphertext = string.IsNullOrEmpty(text) ? string.Empty : Encrypter.EncryptSHA1(text);

            return View("/views/main/sdk/sha1.cshtml");
        }
        #endregion

        #region 函数:Base64()
        /// <summary>Base64 加密</summary>
        /// <returns></returns>
        public ActionResult Base64()
        {
            return View("/views/main/sdk/base64.cshtml");
        }
        #endregion

        #region 函数:Base64Image()
        /// <summary>Base64 加密</summary>
        /// <returns></returns>
        public ActionResult Base64Image()
        {
            return View("/views/main/sdk/base64image.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Base64(object args)
        {
            // 方法 加密|解密
            var methodName = Request["methodName"];

            // 原文
            var originalText = ViewBag.OriginalText = Request["originalText"];
            // 密文
            var ciphertext = ViewBag.Ciphertext = Request["ciphertext"];

            if (methodName == "encrypt" && !string.IsNullOrEmpty(originalText))
            {
                ViewBag.Ciphertext = StringHelper.ToBase64(originalText);
            }
            else if (methodName == "decrypt" && !string.IsNullOrEmpty(ciphertext))
            {
                ViewBag.OriginalText = StringHelper.FromBase64(ciphertext);
            }

            return View("/views/main/sdk/base64.cshtml");
        }
        #endregion

        #region 函数:AES()
        /// <summary>AES 加密</summary>
        /// <returns></returns>
        public ActionResult AES()
        {
            return View("/views/main/sdk/aes.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AES(object args)
        {
            // 方法 加密|解密
            var methodName = Request["methodName"];

            // 原文
            var originalText = ViewBag.OriginalText = Request["originalText"];
            // 密文
            var ciphertext = ViewBag.Ciphertext = Request["ciphertext"];

            // 密钥
            var cryptoKey = string.IsNullOrEmpty(Request["cryptoKey"]) ? SecurityConfigurationView.Instance.AESCryptoKey : Request["cryptoKey"];
            // 密钥偏移量
            var cryptoIV = string.IsNullOrEmpty(Request["cryptoIV"]) ? SecurityConfigurationView.Instance.AESCryptoIV : Request["cryptoIV"];
            // 加密结果编码方式
            var cryptoMode = string.IsNullOrEmpty(Request["cryptoMode"]) ? SecurityConfigurationView.Instance.AESCryptoMode : Request["cryptoMode"];
            // 加密结果编码方式
            var cryptoPadding = string.IsNullOrEmpty(Request["cryptoPadding"]) ? SecurityConfigurationView.Instance.AESCryptoPadding : Request["cryptoPadding"];
            // 加密结果编码方式
            var cryptoCiphertextFormat = string.IsNullOrEmpty(Request["cryptoCiphertextFormat"]) ? SecurityConfigurationView.Instance.AESCryptoCiphertextFormat : Request["cryptoCiphertextFormat"];

            if (methodName == "encrypt" && !string.IsNullOrEmpty(originalText))
            {
                ViewBag.Ciphertext = Encrypter.EncryptAES(originalText, cryptoKey, cryptoIV,
                    (CipherMode)Enum.Parse(typeof(CipherMode), cryptoMode),
                    (PaddingMode)Enum.Parse(typeof(PaddingMode), cryptoPadding),
                    (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), cryptoCiphertextFormat));
            }
            else if (methodName == "decrypt" && !string.IsNullOrEmpty(ciphertext))
            {
                ViewBag.OriginalText = Encrypter.DecryptAES(ciphertext, cryptoKey, cryptoIV,
                    (CipherMode)Enum.Parse(typeof(CipherMode), cryptoMode),
                    (PaddingMode)Enum.Parse(typeof(PaddingMode), cryptoPadding),
                    (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), cryptoCiphertextFormat));
            }

            return View("/views/main/sdk/aes.cshtml");
        }
        #endregion
    }
}
