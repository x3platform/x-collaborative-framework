namespace X3Platform.Sessions.TestSuite.Configuration
{
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Sessions.Configuration;

    [TestClass]
    public class SessionsConfigurationViewTestSuite
    {
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        [TestInitialize()]
        public void Initialize()
        {
            // SessionsConfigurationView.LoadInstance(fullConfigPath);
        }

        [TestMethod]
        public void TestLoad()
        {
            SessionsConfiguration configuration = SessionsConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }
    }
}
