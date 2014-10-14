using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Security;
using System.Security.Cryptography;

namespace X3Platform.Tests.Security
{
    /// <summary>
    /// 加密解密测试
    /// </summary>
    [TestClass]
    public class EncrypterTestSuite
    {
        public EncrypterTestSuite()
        {
        }

        #region 其他测试属性
        //
        // 您可以在编写测试时使用下列其他属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前使用 TestInitialize 运行代码 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在运行每个测试之后使用 TestCleanup 运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>测试DES加密</summary>
        [TestMethod]
        public void TestEncryptDES()
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
