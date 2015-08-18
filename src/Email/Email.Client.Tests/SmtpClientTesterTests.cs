using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Messages;

namespace X3Platform.Email.Client.TestSuite
{
  [TestClass]
  public class SmtpClientTesterTestSuite
  {
    [TestMethod]
    public void TestSmtpServerByGmail()
    {
      var settings = ConfigurationManager.AppSettings;

      MessageObject message = SmtpClientTester.Test("smtp.gmail.com", "465", "ON", settings["gmail_mail_username"], settings["gmail_mail_password"], settings["gmail_mail_from"], settings["gmail_mail_to"]);

      Assert.AreEqual(message.ReturnCode, "0");
    }

    [TestMethod]
    public void TestSmtpServerByQQ()
    {
      var settings = ConfigurationManager.AppSettings;

      MessageObject message = SmtpClientTester.Test("smtp.qq.com", "25", "ON", "yoshow@qq.com", settings["qq_mail_password"], "yoshow@qq.com", "ruanyu83@gmail.com");

      Assert.AreEqual(message.ReturnCode, "0");
    }

    [TestMethod]
    public void TestSmtpServerByExmail()
    {
      var settings = ConfigurationManager.AppSettings;

      MessageObject message = SmtpClientTester.Test("smtp.exmail.qq.com", "25", "ON", "service@x3platform.com", settings["exmail_mail_passwoed"], "service@x3platform.com", "yoshow@qq.com");

      Assert.AreEqual(message.ReturnCode, "0");
    }
  }
}
