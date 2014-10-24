namespace X3Platform.Tasks.Tests.Configuration
{
    // 同时支持 MSTest 和 NUnit
    #if NUNIT 
    using NUnit.Framework; 
    using TestClass = NUnit.Framework.TestFixtureAttribute; 
    using TestMethod = NUnit.Framework.TestAttribute; 
    using TestInitialize = NUnit.Framework.SetUpAttribute; 
    using TestCleanup = NUnit.Framework.TearDownAttribute; 
    using TestContext = System.Object; 
    using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute; 
    using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
    #else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
    #endif

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;

    using X3Platform.Tasks.Configuration;
    using X3Platform.IBatis.DataMapper;

    /// <summary></summary>
    [TestClass]
    public class TasksConfigurationViewTests
    {
        /// <summary>测试初始化配置信息是否成功</summary>
        [TestMethod]
        public void TestInit()
        {
            TasksConfiguration configuration = TasksConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
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
