namespace X3Platform.Tasks.Tests.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Tasks.Configuration;
    using X3Platform.IBatis.DataMapper;

    /// <summary></summary>
    [TestClass]
    public class TasksConfigurationViewTests
    {
        /// <summary>测试初始化配置信息是否成功</summary>
        [TestMethod]
        [DeploymentItem("MySql.Data.dll")]
        public void TestInit()
        {
            TasksConfiguration configuration = TasksConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        /// <summary>测试初始化 IBatis 配置信息是否成功</summary>
        [TestMethod]
        [DeploymentItem("MySql.Data.dll")]
        public void TestCreateMapper()
        {
            TasksConfiguration configuration = TasksConfigurationView.Instance.Configuration;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            Console.WriteLine(ibatisMapping);

            ISqlMapper ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
