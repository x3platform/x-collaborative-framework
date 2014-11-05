namespace X3Platform.Connect.Tests.Configuration
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.IBatis.DataMapper;

    using X3Platform.Connect.Configuration;

    [TestClass]
    public class ConnectConfigurationViewTests
    {
        //-------------------------------------------------------
        // ≤‚ ‘ƒ⁄»›
        //-------------------------------------------------------

        [TestMethod]
        public void TestInit()
        {
            ConnectConfiguration configuration = ConnectConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        [TestMethod]
        public void TestCreateMapper()
        {
            ConnectConfiguration configuration = ConnectConfigurationView.Instance.Configuration;

            ISqlMapper ibatisMapper = null;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
