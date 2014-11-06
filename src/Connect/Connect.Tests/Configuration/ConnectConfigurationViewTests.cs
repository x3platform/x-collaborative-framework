#region Using Testing Libraries
#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestContext = System.Object;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

using NMock;
#endregion

namespace X3Platform.Connect.Tests.Configuration
{
    using System;

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
