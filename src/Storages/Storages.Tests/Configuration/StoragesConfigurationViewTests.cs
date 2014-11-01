namespace X3Platform.Storages.Tests.Configuration
{
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Storages.Configuration;
    using System;
    using X3Platform.IBatis.DataMapper;

    [TestClass]
    public class StoragesConfigurationViewTests
    {
        [TestMethod]
        public void TestInit()
        {
            StoragesConfiguration configuration = StoragesConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        /// <summary>测试初始化 IBatis 配置信息是否成功</summary>
        [TestMethod]
        // [DeploymentItem("MySql.Data.dll")]
        public void TestCreateMapper()
        {
            StoragesConfiguration configuration = StoragesConfigurationView.Instance.Configuration;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            Console.WriteLine(ibatisMapping);

            ISqlMapper ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
