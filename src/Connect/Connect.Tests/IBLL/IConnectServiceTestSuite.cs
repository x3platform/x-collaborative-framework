using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Connect.Configuration;
using X3Platform.Connect.Model;

namespace X3Platform.Connect.TestSuite.IBLL
{
    /// <summary></summary>
    [TestClass]
    public class IConnectServiceTestSuite
    {
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        [TestInitialize()]
        public void Initialize()
        {
            ConnectConfigurationView.LoadInstance(fullConfigPath);
        }

        [TestMethod]
        public void TestGetPages()
        {
            int rowCount = -1;

            IList<ConnectInfo> list = ConnectContext.Instance.ConnectService.GetPages(0, 10, string.Empty, string.Empty,out rowCount);

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestFindAll()
        {
            IList<ConnectInfo> list = ConnectContext.Instance.ConnectService.FindAll();

            Assert.IsNotNull(list);
        }
    }
}
