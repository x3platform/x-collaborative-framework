namespace X3Platform.Security
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Cryptography;
    using System.Text;
    using System.Collections;
    using X3Platform.Security.Configuration;
    #endregion

    /// <summary>加密器</summary>
    public sealed class Encrypter
    {
        static Encrypter() { }

        #region 函数:SortAndConcat(params string[] values)
        /// <summary>将字符串数组排序后拼接成一个文本信息</summary>
        /// <param name="values">任意多个文本信息</param>
        /// <returns>拼接后的文本</returns>
        public static string SortAndConcat(params string[] values)
        {
            ArrayList list = new ArrayList(values);

            list.Sort();

            return string.Concat(list.ToArray());
        }
        #endregion

        #region 函数:ToCiphertext(byte[] result, CiphertextFormat format)
        /// <summary>将加密后的二进制数据转为某种格式的文本信息</summary>
        /// <param name="result">加密后的二进制结果数据</param>
        /// <param name="format">密文格式</param>
        /// <returns>格式化后的的加密文本</returns>
        public static string ToCiphertext(byte[] result, CiphertextFormat format)
        {
            switch (format)
            {
                case CiphertextFormat.Base64String:
                    return Convert.ToBase64String(result, 0, result.Length);

                case CiphertextFormat.HexString:
                    return BitConverter.ToString(result).ToLower();

                case CiphertextFormat.HexStringWhitoutHyphen:
                    return BitConverter.ToString(result).Replace("-", string.Empty).ToLower();

                default:
                    return string.Empty;
            }
        }
        #endregion

        #region 函数:FromCiphertext(string ciphertext, CiphertextFormat format)
        /// <summary>将某种格式的密文转为加密后的二进制数据</summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="format">密文格式</param>
        /// <returns>加密后的二进制数据</returns>
        public static byte[] FromCiphertext(string ciphertext, CiphertextFormat format)
        {
            byte[] result = null;

            if (format == CiphertextFormat.Base64String)
            {
                result = Convert.FromBase64String(ciphertext);
            }
            else if (format == CiphertextFormat.HexString)
            {
                string[] data = ciphertext.Split('-');

                result = new byte[data.Length];

                Int32Converter converter = new Int32Converter();

                for (int i = 0; i < data.Length; i++)
                {
                    result[i] = Convert.ToByte(converter.ConvertFromInvariantString(string.Format("0x{0}", data[i])).ToString());
                }
            }
            else if (format == CiphertextFormat.HexStringWhitoutHyphen)
            {
                result = new byte[ciphertext.Length / 2];

                Int32Converter converter = new Int32Converter();

                for (int i = 0; i < ciphertext.Length; i = i + 2)
                {
                    result[i / 2] = Convert.ToByte(converter.ConvertFromInvariantString(string.Format("0x{0}", ciphertext.Substring(i, 2)).ToString()));
                }
            }

            return result;
        }
        #endregion

        //-------------------------------------------------------
        // MD5 - Message-Digest Algorithm 5
        // http://zh.wikipedia.org/zh-cn/MD5
        //-------------------------------------------------------

        #region 函数:EncryptMD5(string text)
        /// <summary>加密-MD5方式</summary>
        /// <param name="text">文本</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptMD5(string text)
        {
            var format = (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), SecurityConfigurationView.Instance.MD5CryptoCiphertextFormat);

            return EncryptMD5(text, format);
        }
        #endregion

        #region 函数:EncryptMD5(string text, CiphertextFormat format)
        /// <summary>加密-MD5方式</summary>
        /// <param name="text">文本</param>
        /// <param name="format">密文格式</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptMD5(string text, CiphertextFormat format)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(text));

            return ToCiphertext(result, format);
        }
        #endregion

        //-------------------------------------------------------
        // SHA1 - Secure Hash Standard 1
        // http://zh.wikipedia.org/zh-cn/SHA1
        //-------------------------------------------------------

        #region 函数:EncryptSHA1(string text)
        /// <summary>加密-SHA1方式</summary>
        /// <param name="text">文本</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptSHA1(string text)
        {
            var format = (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), SecurityConfigurationView.Instance.SHA1CryptoCiphertextFormat);

            return EncryptSHA1(text, format);
        }
        #endregion

        #region 函数:EncryptSHA1(string text, CiphertextFormat format)
        /// <summary>加密-SHA1方式</summary>
        /// <param name="text">文本</param>
        /// <param name="format">密文格式</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptSHA1(string text, CiphertextFormat format)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] result = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));

            return ToCiphertext(result, format);
        }
        #endregion

        //-------------------------------------------------------
        // DES - Data Encryption Standard
        // http://en.wikipedia.org/wiki/Data_Encryption_Standard
        //-------------------------------------------------------

        #region 函数:EncryptDES(string text)
        /// <summary>加密-DES方式</summary>
        /// <param name="text">文本</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptDES(string text)
        {
            return EncryptDES(text, SecurityConfigurationView.Instance.DESCryptoKey, SecurityConfigurationView.Instance.DESCryptoIV);
        }
        #endregion

        #region 函数:EncryptDES(string text, string key, string iv)
        /// <summary>加密-DES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptDES(string text, string key, string iv)
        {
            var format = (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), SecurityConfigurationView.Instance.DESCryptoCiphertextFormat);

            return EncryptDES(text, UTF8Encoding.UTF8.GetBytes(key), UTF8Encoding.UTF8.GetBytes(iv), format);
        }
        #endregion

        #region 函数:EncryptDES(string text, string key, string iv, CiphertextFormat format)
        /// <summary>加密-DES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="format">密文格式</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptDES(string text, string key, string iv, CiphertextFormat format)
        {
            return EncryptDES(text, UTF8Encoding.UTF8.GetBytes(key), UTF8Encoding.UTF8.GetBytes(iv), format);
        }
        #endregion

        #region 函数:EncryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        /// <summary>加密-DES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="rgbKey">加密的Key</param>
        /// <param name="rgbIV">初始化向量</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptDES(string text, byte[] rgbKey, byte[] rgbIV, CiphertextFormat format)
        {
            byte[] result = null;

            byte[] buffer = UTF8Encoding.UTF8.GetBytes(text);

            DESCryptoServiceProvider desCryptoProvider = new DESCryptoServiceProvider();

            using (ICryptoTransform encryptor = desCryptoProvider.CreateEncryptor(rgbKey, rgbIV))
            {
                result = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                encryptor.Dispose();
            }

            return ToCiphertext(result, format);
        }
        #endregion

        #region 函数:DecryptDES(string text)
        /// <summary>加密-DES方式</summary>
        /// <param name="text">文本</param>
        public static string DecryptDES(string text)
        {
            return DecryptDES(text, SecurityConfigurationView.Instance.DESCryptoKey, SecurityConfigurationView.Instance.DESCryptoIV);
        }
        #endregion

        #region 函数:DecryptDES(string text, string key, string iv)
        /// <summary>解密-DES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的文本</returns>
        public static string DecryptDES(string text, string key, string iv)
        {
            var format = (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), SecurityConfigurationView.Instance.DESCryptoCiphertextFormat);

            return DecryptDES(text, UTF8Encoding.UTF8.GetBytes(key), UTF8Encoding.UTF8.GetBytes(iv), format);
        }
        #endregion

        #region 函数:DecryptDES(string text, byte[] rgbKey, byte[] rgbIV, CiphertextFormat format)
        /// <summary>解密-DES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="rgbKey">Key</param>
        /// <param name="rgbIV">初始化向量</param>
        /// <returns></returns>
        public static string DecryptDES(string text, byte[] rgbKey, byte[] rgbIV, CiphertextFormat format)
        {
            byte[] result = null;

            byte[] buffer = FromCiphertext(text, format);

            DESCryptoServiceProvider desCryptoProvider = new DESCryptoServiceProvider();

            using (ICryptoTransform decryptor = desCryptoProvider.CreateDecryptor(rgbKey, rgbIV))
            {
                result = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            return UTF8Encoding.UTF8.GetString(result);
        }
        #endregion

        //-------------------------------------------------------
        // ASE - The Advanced Encryption Standard
        // http://en.wikipedia.org/wiki/Advanced_Encryption_Standard
        //-------------------------------------------------------
        //
        // 默认的密钥 长度
        //
        // 16位的字符串 => 128位 (1900 01 01 00 00 00 00)
        // 24位的字符串 => 192位
        // 32位的字符串 => 256位 (默认)
        //
        //-------------------------------------------------------
        // CryptoJS ASE 加密方式
        // CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(text), 
        //   CryptoJS.enc.Utf8.parse(key), 
        //   { 
        //     keySize: 128 / 8, 
        //     iv: CryptoJS.enc.Utf8.parse(iv), 
        //     mode: CryptoJS.mode.CBC, 
        //     padding: CryptoJS.pad.Pkcs7 
        //   }).toString();
        //-------------------------------------------------------

        #region 函数:EncryptAES(string text)
        /// <summary>加密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptAES(string text)
        {
            return EncryptAES(text, SecurityConfigurationView.Instance.AESCryptoKey, SecurityConfigurationView.Instance.AESCryptoIV);
        }
        #endregion

        #region 函数:EncryptAES(string text, string key, string iv)
        /// <summary>加密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptAES(string text, string key, string iv)
        {
            var format = (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), SecurityConfigurationView.Instance.AESCryptoCiphertextFormat);

            return EncryptAES(text, UTF8Encoding.UTF8.GetBytes(key), UTF8Encoding.UTF8.GetBytes(iv), format);
        }
        #endregion

        #region 函数:EncryptAES(string text, string key, string iv, CiphertextFormat format)
        /// <summary>加密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="format">密文格式</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptAES(string text, string key, string iv, CiphertextFormat format)
        {
            return EncryptAES(text, UTF8Encoding.UTF8.GetBytes(key), UTF8Encoding.UTF8.GetBytes(iv), format);
        }
        #endregion

        #region 函数:EncryptAES(string text, byte[] key, byte[] iv)
        /// <summary>加密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="format">密文格式</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptAES(string text, byte[] key, byte[] iv, CiphertextFormat format)
        {
            byte[] result = null;

            byte[] buffer = UTF8Encoding.UTF8.GetBytes(text);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor())
            {
                result = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            return ToCiphertext(result, format);
        }
        #endregion

        #region 函数:DecryptAES(string text)
        /// <summary>解密-AES方式</summary>
        /// <param name="text">文本</param>
        public static string DecryptAES(string text)
        {
            return DecryptAES(text, SecurityConfigurationView.Instance.AESCryptoKey, SecurityConfigurationView.Instance.AESCryptoIV);
        }
        #endregion

        #region 函数:DecryptAES(string text, string key, string iv)
        /// <summary>解密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的文本</returns>
        public static string DecryptAES(string text, string key, string iv)
        {
            var format = (CiphertextFormat)Enum.Parse(typeof(CiphertextFormat), SecurityConfigurationView.Instance.AESCryptoCiphertextFormat);

            return DecryptAES(text, UTF8Encoding.UTF8.GetBytes(key), UTF8Encoding.UTF8.GetBytes(iv), format);
        }
        #endregion

        #region 函数:DecryptAES(string text, byte[] key, byte[] iv, CiphertextFormat format)
        /// <summary>解密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的文本</returns>
        public static string DecryptAES(string text, byte[] key, byte[] iv, CiphertextFormat format)
        {
            var mode = (CipherMode)Enum.Parse(typeof(CipherMode), SecurityConfigurationView.Instance.AESCryptoMode);
            var padding = (PaddingMode)Enum.Parse(typeof(PaddingMode), SecurityConfigurationView.Instance.AESCryptoPadding);

            return DecryptAES(text, key, iv, format, mode, padding);
        }
        #endregion

        #region 函数:DecryptAES(string text, byte[] key, byte[] iv, CiphertextFormat format)
        /// <summary>解密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的文本</returns>
        public static string DecryptAES(string text, byte[] key, byte[] iv, CiphertextFormat format, CipherMode mode, PaddingMode padding)
        {
            byte[] result = null;

            byte[] buffer = FromCiphertext(text, format);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.Key = key;
            rijndaelManaged.IV = key;
            rijndaelManaged.Mode = mode;
            rijndaelManaged.Padding = padding;

            using (ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor())
            {
                result = cTransform.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            return UTF8Encoding.UTF8.GetString(result);
        }
        #endregion
    }
}
