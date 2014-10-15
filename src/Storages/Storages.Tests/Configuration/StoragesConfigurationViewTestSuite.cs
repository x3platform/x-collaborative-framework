namespace X3Platform.Storages.TestSuite.Configuration
{
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Storages.Configuration;

    [TestClass]
    public class StoragesConfigurationViewTestSuite
    {
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        [TestInitialize()]
        public void Initialize()
        {
            // StoragesConfigurationView.LoadInstance(fullConfigPath);
        }

        [TestMethod]
        public void TestLoad()
        {
            StoragesConfiguration configuration = StoragesConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }
    }
}
