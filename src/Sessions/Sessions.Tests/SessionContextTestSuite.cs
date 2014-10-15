namespace X3Platform.Email.Client.TestSuite
{
    using System;

    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;
    using X3Platform.Sessions.Configuration;
    using X3Platform.Sessions;

    [TestClass]
    public class SessionsContextTestSuite
    {
        string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        [TestInitialize()]
        public void Initialize()
        {
             // SessionsConfigurationView.Instance.LoadInstance(fullConfigPath);
        }

        /// <summary></summary>
        [TestMethod]
        public void TestLoad()
        {
            Assert.IsNotNull(SessionContext.Instance.AccountCacheService);
        }
    }
}
