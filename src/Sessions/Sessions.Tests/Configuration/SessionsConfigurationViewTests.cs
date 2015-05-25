namespace X3Platform.Sessions.Tests.Configuration
{
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Sessions.Configuration;
    using X3Platform.IBatis.DataMapper;

    [TestClass]
    public class SessionsConfigurationViewTests
    {
        [TestMethod]
        public void TestInit()
        {
            SessionsConfiguration configuration = SessionsConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        [TestMethod]
        public void TestCreateMapper()
        {
            SessionsConfiguration configuration = SessionsConfigurationView.Instance.Configuration;

            ISqlMapper ibatisMapper = null;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
