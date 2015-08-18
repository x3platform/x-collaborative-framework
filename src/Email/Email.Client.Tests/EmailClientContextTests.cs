using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X3Platform.Email.Client.TestSuite
{
    [TestClass]
    public class EmailClientContextTests
    {
        [TestMethod]
        public void TestSend()
        {
            EmailClientContext.Instance.Send("service@x3platform.com", "yoshow@qq.com",
                string.Format("Date:{0}", DateTime.Now),
                string.Format("{0} this email come from default provider", DateTime.Now));
        }
    }
}
