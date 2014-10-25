namespace X3Platform.Tests.Security
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Security;

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

        /// <summary>测试SHA1加密</summary>
        [TestMethod]
        public void TestEncryptSHA1()
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            string text = "hello";

            string result = Encrypter.EncryptDES(text, DES.Key, DES.IV);

            result = Encrypter.DecryptDES(result, DES.Key, DES.IV);

            Assert.AreEqual(text, result, false);
        }

        /// <summary>测试DES解密</summary>
        [TestMethod]
        public void TestDecryptDES()
        {
            string text = "hi";

            string result = Encrypter.EncryptDES(text);

            result = Encrypter.DecryptDES(result);

            Assert.AreEqual(text, result, false);
        }

        /// <summary>测试AES加密</summary>
        [TestMethod]
        public void TestEncryptAES()
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789000");

            string text = "hello";

            string result = Encrypter.EncryptAES(text, key);

            result = Encrypter.DecryptAES(result, key);

            Assert.AreEqual(text, result, false);
        }

        /// <summary>测试AES解密</summary>
        [TestMethod]
        public void TestDecryptAES()
        {
            string text = "hi";

            string result = Encrypter.EncryptAES(text);

            result = Encrypter.DecryptAES(result);

            Assert.AreEqual(text, result, false);
        }
    }
}
