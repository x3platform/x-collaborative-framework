using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Messages;

namespace X3Platform.Email.Client.TestSuite
{
    [TestClass]
    public class SmtpClientTesterTestSuite
    {
        [TestMethod]
        public void TestSmtpServerByLongfor()
        {
            MessageObject message = SmtpClientTester.Test("mail.longhu.net", "25", "Off", string.Empty, string.Empty, "admin@webexweb.longhu.net", "ruanyu@x3platform.com");

            Assert.AreEqual(message.ReturnCode, "0");
        }

        [TestMethod]
        public void TestSmtpServerByGmail()
        {
            MessageObject message = SmtpClientTester.Test("smtp.gmail.com", "465", "ON", "ruanyu1983@gmail.com", "ruanyu", "ruanyu1983@gmail.com", "ruanyu83@gmail.com");

            Assert.AreEqual(message.ReturnCode, "0");
        }

        [TestMethod]
        public void TestSmtpServerByQQ()
        {
            MessageObject message = SmtpClientTester.Test("smtp.qq.com", "25", "ON", "yoshow@qq.com", "ruanyu", "yoshow@qq.com", "ruanyu83@gmail.com");
        
            Assert.AreEqual(message.ReturnCode, "0");
        }
    }
}
