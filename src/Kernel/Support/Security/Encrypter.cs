namespace X3Platform.Security
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Cryptography;
    using System.Text;
    using System.Collections;
    #endregion

    /// <summary>加密器</summary>
    public sealed class Encrypter
    {
        private static DESCryptoServiceProvider desCryptoProvider;

        static Encrypter()
        {
            desCryptoProvider = new DESCryptoServiceProvider();

            //
            // 设置小于255的数字.
            //
            desCryptoProvider.Key = new byte[8] { 67, 18, 08, 14, 22, 234, 46, 43 };

            desCryptoProvider.IV = new byte[8] { 44, 23, 33, 44, 66, 77, 88, 99 };
        }

        #region 函数:SortAndConcat(params string[] values)
        ///<summary>将字符串数组排序后拼接成一个文本信息</summary>
        ///<param name="values">任意多个文本信息</param>
        ///<returns>拼接后的文本</returns>
        public static string SortAndConcat(params string[] values)
        {
            ArrayList list = new ArrayList(values);

            list.Sort();

            return string.Concat(list.ToArray());
        }
        #endregion

        //-------------------------------------------------------
        // MD5 - Message-Digest Algorithm 5
        // http://zh.wikipedia.org/zh-cn/MD5
        //-------------------------------------------------------

        #region 函数:EncryptMD5(string text)
        ///<summary>加密-MD5方式</summary>
        ///<param name="text">文本</param>
        ///<returns>加密后的文本</returns>
        public static string EncryptMD5(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(text));

            return Encoding.Default.GetString(result);
        }
        #endregion

        //-------------------------------------------------------
        // MD5 - Message-Digest Algorithm 5
        // http://zh.wikipedia.org/zh-cn/SHA1
        //-------------------------------------------------------

        #region 函数:EncryptSHA1(string text)
        ///<summary>加密-SHA1方式</summary>
        ///<param name="text">文本</param>
        ///<returns>加密后的文本</returns>
        public static string EncryptSHA1(string text)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] result = sha1.ComputeHash(Encoding.Default.GetBytes(text));

            return Encoding.Default.GetString(result);
        }
        #endregion

        //-------------------------------------------------------
        // DES - Data Encryption Standard
        // http://en.wikipedia.org/wiki/Data_Encryption_Standard
        //-------------------------------------------------------

        #region 函数:EncryptDES(string text)
        ///<summary>加密-DES方式</summary>
        ///<param name="text">文本</param>
        ///<returns>加密后的文本</returns>
        public static string EncryptDES(string text)
        {
            return EncryptDES(text, desCryptoProvider.Key, desCryptoProvider.IV);
        }
        #endregion

        #region 函数:EncryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        ///<summary>加密-DES方式</summary>
        ///<param name="text">文本</param>
        ///<param name="rgbKey">加密的Key</param>
        ///<param name="rgbIV">初始化向量</param>
        ///<returns>加密后的文本</returns>
        public static string EncryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        {
            byte[] result = null;

            byte[] buffer = Encoding.ASCII.GetBytes(text);

            using (ICryptoTransform encryptor = desCryptoProvider.CreateEncryptor(rgbKey, rgbIV))
            {
                result = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                encryptor.Dispose();
            }

            return BitConverter.ToString(result);
        }
        #endregion

        #region 函数:DecryptDES(string text)
        ///<summary>加密-DES方式</summary>
        ///<param name="text">文本</param>
        public static string DecryptDES(string text)
        {
            return DecryptDES(text, desCryptoProvider.Key, desCryptoProvider.IV);
        }
        #endregion

        #region 函数:DecryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        ///<summary>解密-DES方式</summary>
        ///<param name="text">文本</param>
        ///<param name="rgbKey">Key</param>
        ///<param name="rgbIV">初始化向量</param>
        ///<returns></returns>
        public static string DecryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        {
            byte[] result = null;

            string[] data = text.Split('-');

            byte[] buffer = new byte[data.Length];

            Int32Converter converter = new Int32Converter();

            for (int i = 0; i < data.Length; i++)
            {
                buffer[i] = Convert.ToByte(converter.ConvertFromInvariantString(string.Format("0x{0}", data[i])).ToString());
            }

            using (ICryptoTransform decryptor = desCryptoProvider.CreateDecryptor(rgbKey, rgbIV))
            {
                result = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            return Encoding.ASCII.GetString(result);
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

        /// <summary>默认的加密密钥</summary>
        private const string ASECryptoKey = "12345678901234567890123456789abc";

        #region 函数:EncryptAES(string text)
        ///<summary>加密-AES方式</summary>
        ///<param name="text">文本</param>
        ///<returns>加密后的文本</returns>
        public static string EncryptAES(string text)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(ASECryptoKey);

            return EncryptAES(text, key);
        }
        #endregion

        #region 函数:EncryptAES(string text, byte[] key)
        /// <summary>加密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string EncryptAES(string text, byte[] key)
        {
            byte[] result = null;

            byte[] buffer = UTF8Encoding.UTF8.GetBytes(text);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.Key = key;
            rijndaelManaged.Mode = CipherMode.ECB;
            rijndaelManaged.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor())
            {
                result = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            return Convert.ToBase64String(result, 0, result.Length);
        }
        #endregion

        #region 函数:DecryptAES(string text)
        ///<summary>解密-AES方式</summary>
        /// <param name="text">文本</param>
        public static string DecryptAES(string text)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(ASECryptoKey);

            return DecryptAES(text, key);
        }
        #endregion

        #region 函数:DecryptAES(string text, byte[] key)
        ///<summary>解密-AES方式</summary>
        /// <param name="text">文本</param>
        /// <param name="key">密钥</param>
        public static string DecryptAES(string text, byte[] key)
        {
            byte[] result = null;

            byte[] buffer = Convert.FromBase64String(text);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.Key = key;
            rijndaelManaged.Mode = CipherMode.ECB;
            rijndaelManaged.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor())
            {
                result = cTransform.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            return UTF8Encoding.UTF8.GetString(result);
        }
        #endregion
    }
}
