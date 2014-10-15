using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X3Platform.Email.Client.TestSuite
{
    [TestClass]
    public class EmailClientContextTestSuite
    {
        [TestMethod]
        public void TestSend()
        {
            EmailClientContext.Instance.Send("service@x3platform.com", "yoshow@qq.com",
                string.Format("Date:{0}", DateTime.Now),
                string.Format("{0} this email come from default provider", DateTime.Now));

            //EmailClientContext.Instance["General Email Client"].Send("yoshow@qq.com", "ruanyu83@gmail.com",
            //    string.Format("hello {0}", DateTime.Now),
            //    string.Format("{0} this email come from default provider", DateTime.Now));
        }

        [TestMethod]
        public void TestChinaLMTCSend()
        {
            //EmailClientContext.Instance["ChinaLMTC Email Client"].Send("webmaster@kanf.cn", "ruanyu83@gmail.com",
            //    string.Format("Date:{0}", DateTime.Now),
            //    string.Format("{0} this email come from ChinaLMTC email provider", DateTime.Now));
        }

        [TestMethod]
        public void TestGmailSend()
        {
            //EmailClientContext.Instance["Gmail Email Client"].Send("ruanyu1983@gmail.com", "ruanyu83@gmail.com",
            //    string.Format("Date:{0}", DateTime.Now),
            //    string.Format("{0} this email come from Gmail email provider", DateTime.Now));
        }

        /// <summary>QQ ” œ‰</summary>
        [TestMethod]
        public void TestQQMailSend()
        {
            //EmailClientContext.Instance["QQ Email Client"].Send("yoshow@qq.com", "ruanyu83@gmail.com",
            //    string.Format("Date:{0}", DateTime.Now),
            //    string.Format("{0} this email come from QQ email provider", DateTime.Now));
        }

        [TestMethod]
        public void TestKanFSend()
        {
            //EmailClientFactory.GetEmailClientProvider("KanF Email Client").Send("webmaster@kanf.cn", "ruanyu83@gmail.com",
            //    string.Format("Date:{0}", DateTime.Now),
            //    string.Format("{0} this email come from KanF email provider", DateTime.Now));
        }
    }
}
