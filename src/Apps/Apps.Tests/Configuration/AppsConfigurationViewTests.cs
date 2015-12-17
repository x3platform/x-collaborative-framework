namespace X3Platform.Apps.Tests.Configuration
{
    using NUnit.Framework;

    using System;

    using X3Platform.IBatis.DataMapper;

    using X3Platform.Apps.Configuration;

    [TestFixture]
    public class AppsConfigurationViewTests
    {
        //-------------------------------------------------------
        // ≤‚ ‘ƒ⁄»›
        //-------------------------------------------------------

        [Test]
        public void TestInit()
        {
            var configuration = AppsConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        [Test]
        public void TestCreateMapper()
        {
            AppsConfiguration configuration = AppsConfigurationView.Instance.Configuration;

            ISqlMapper ibatisMapper = null;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
