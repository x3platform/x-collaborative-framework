// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :Encrypter.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Security
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Security.Cryptography;
    using System.ComponentModel;

    /// <summary>������</summary>
    public sealed class Encrypter
    {
        private static DESCryptoServiceProvider desCryptoProvider;

        static Encrypter()
        {
            desCryptoProvider = new DESCryptoServiceProvider();

            //
            // ����С��255������.
            //
            desCryptoProvider.Key = new byte[8] { 67, 18, 08, 14, 22, 234, 46, 43 };

            desCryptoProvider.IV = new byte[8] { 44, 23, 33, 44, 66, 77, 88, 99 };
        }

        //-------------------------------------------------------
        // DES - Data Encryption Standard
        // http://en.wikipedia.org/wiki/Data_Encryption_Standard
        //-------------------------------------------------------

        #region ����:EncryptDES(string text)
        ///<summary>����-DES��ʽ</summary>
        ///<param name="text">�ı�</param>
        public static string EncryptDES(string text)
        {
            return EncryptDES(text, desCryptoProvider.Key, desCryptoProvider.IV);
        }
        #endregion

        #region ����:EncryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        ///<summary>����-DES��ʽ</summary>
        ///<param name="text">�ı�</param>
        ///<param name="rgbKey">���ܵ�Key</param>
        ///<param name="rgbIV">��ʼ������</param>
        ///<returns>���ܺ����ı�</returns>
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

        #region ����:DecryptDES(string text)
        ///<summary>����-DES��ʽ</summary>
        ///<param name="text">�ı�</param>
        public static string DecryptDES(string text)
        {
            return DecryptDES(text, desCryptoProvider.Key, desCryptoProvider.IV);
        }
        #endregion

        #region ����:DecryptDES(string text, byte[] rgbKey, byte[] rgbIV)
        ///<summary>����-DES��ʽ</summary>
        ///<param name="text">�ı�</param>
        ///<param name="rgbKey">Key</param>
        ///<param name="rgbIV">��ʼ������</param>
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
        // Ĭ�ϵ���Կ ����
        //
        // 16λ���ַ��� => 128λ (1900 01 01 00 00 00 00)
        // 24λ���ַ��� => 192λ
        // 32λ���ַ��� => 256λ (Ĭ��)
        //

        /// <summary>Ĭ�ϵļ�����Կ</summary>
        private const string ASECryptoKey = "12345678901234567890123456789abc";

        #region ����:EncryptAES(string text)
        ///<summary>����-AES��ʽ</summary>
        ///<param name="text">�ı�</param>
        public static string EncryptAES(string text)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(ASECryptoKey);

            return EncryptAES(text, key);
        }
        #endregion

        #region ����:EncryptAES(string text, byte[] key)
        /// <summary>����-AES��ʽ</summary>
        /// <param name="text">�ı�</param>
        /// <param name="key">��Կ</param>
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

        #region ����:DecryptAES(string text)
        ///<summary>����-AES��ʽ</summary>
        /// <param name="text">�ı�</param>
        public static string DecryptAES(string text)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(ASECryptoKey);

            return DecryptAES(text, key);
        }
        #endregion

        #region ����:DecryptAES(string text, byte[] key)
        ///<summary>����-AES��ʽ</summary>
        /// <param name="text">�ı�</param>
        /// <param name="key">��Կ</param>
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
