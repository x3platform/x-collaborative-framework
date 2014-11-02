namespace X3Platform.Tests.Security
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Security;
    using System.IO;

    /// <summary>加密解密测试</summary>
    [TestClass]
    public class EncrypterTests
    {
        /// <summary>测试排序拼接文本信息</summary>
        [TestMethod]
        public void TestSortAndConcat()
        {
            string result = null;

            result = Encrypter.SortAndConcat("b", "a", "c");

            Assert.AreEqual(result, "abc");

            result = Encrypter.SortAndConcat("b", "12345", "abc");

            Assert.AreEqual(result, "12345abcb");
        }

        /// <summary>测试MD5加密</summary>
        [TestMethod]
        public void TestEncryptMD5()
        {
            string result = null;

            // 验证数字
            result = Encrypter.EncryptMD5("12345");

            Assert.AreEqual(result, "827ccb0eea8a706c4c34a16891f84e7b");

            // 验证字母
            result = Encrypter.EncryptMD5("abcde");

            Assert.AreEqual(result, "ab56b4d92b40713acc5af89985d4b786");

            // 验证中文字符
            result = Encrypter.EncryptMD5("中文数据校验");

            Assert.AreEqual(result, "73d6e0b9fdb0ce5ffd5f62f5047d9269");

            // 验证混合
            result = Encrypter.EncryptMD5("12345abcde中文数据校验");

            Assert.AreEqual(result, "44d3f4d008cd557f877bcb44b5c07237");
        }

        /// <summary>测试SHA1加密</summary>
        [TestMethod]
        public void TestEncryptSHA1()
        {
            string result = null;

            // 验证数字
            result = Encrypter.EncryptSHA1("12345");

            Assert.AreEqual(result, "8cb2237d0679ca88db6464eac60da96345513964");

            // 验证字母
            result = Encrypter.EncryptSHA1("abcde");

            Assert.AreEqual(result, "03de6c570bfe24bfc328ccd7ca46b76eadaf4334");

            // 验证中文字符
            result = Encrypter.EncryptSHA1("中文数据校验");

            Assert.AreEqual(result, "10b5eda71a6464ff3135076856311e0248224f4b");

            // 验证混合
            result = Encrypter.EncryptSHA1("12345abcde中文数据校验");

            Assert.AreEqual(result, "51367648c0adce27cb2dd50bf79fe54a9e64702c");
        }

        /// <summary>测试DES解密</summary>
        [TestMethod]
        public void TestDecryptDES()
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes("12345678");

            byte[] iv = UTF8Encoding.UTF8.GetBytes("12345678");

            string text = "123456";

            string result = Encrypter.EncryptDES(text, key, iv, CiphertextFormat.Base64String);

            Assert.AreEqual("HUX+7VtHgb0=", result);

            result = Encrypter.DecryptDES(result);

            Assert.AreEqual(text, result, false);
        }

        /// <summary>测试AES加密</summary>
        [TestMethod]
        public void TestEncryptAES()
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes("1234567812345678");

            byte[] iv = UTF8Encoding.UTF8.GetBytes("1234567812345678");
            //
            string result = Encrypter.EncryptAES("123456");

            Assert.AreEqual("2eDiseYiSX62qk/WS/ZDmg==", result);

            // 验证数字
            result = Encrypter.EncryptAES("123456", key, iv, CiphertextFormat.Base64String);

            Assert.AreEqual("2eDiseYiSX62qk/WS/ZDmg==", result);

            // 验证字母
            result = Encrypter.EncryptAES("abcde", key, iv, CiphertextFormat.Base64String);

            Assert.AreEqual("yWH3n12SyLYEcLoArfP3dg==", result);
        }

        /// <summary>测试AES解密</summary>
        [TestMethod]
        public void TestDecryptAES()
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes("1234567812345678");

            byte[] iv = UTF8Encoding.UTF8.GetBytes("1234567812345678");

            string result = Encrypter.DecryptAES("2eDiseYiSX62qk/WS/ZDmg==", key, iv, CiphertextFormat.Base64String);

            Assert.AreEqual("123456", result);

            result = Encrypter.DecryptAES("d9e0e2b1e622497eb6aa4fd64bf6439a", key, iv, CiphertextFormat.HexStringWhitoutHyphen);

            Assert.AreEqual("123456", result);
        }
    }
}