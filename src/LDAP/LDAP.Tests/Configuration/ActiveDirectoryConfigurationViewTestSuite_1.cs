
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Elane.X.ActiveDirectory.Configuration;
using System.Configuration;


namespace Elane.X.ActiveDirectory.TestSuite.Configuration
{
    /// <summary></summary>
    [TestClass]
    public class ActiveDirectoryConfigurationViewTestSuite
    {
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        [TestInitialize()]
        public void Initialize()
        {
            ActiveDirectoryConfigurationView.LoadInstance(fullConfigPath);
        }

        /// <summary>验证配置文档加载</summary>
        [TestMethod]
        public void TestLoad()
        {
            ActiveDirectoryConfiguration configuration = ActiveDirectoryConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }
    }
}
