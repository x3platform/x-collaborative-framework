namespace X3Platform.Tests.Security.Authority.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.IBatis.DataMapper;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.Security.Authority.Configuration;
    using System.Diagnostics;

    /// <summary></summary>
    [TestClass]
    public class AuthorityConfigurationViewTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        /// <summary>测试初始化配置信息是否成功</summary>
        [TestMethod]
        public void TestInit()
        {
            AuthorityConfiguration configuration = AuthorityConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        /// <summary>测试初始化 IBatis 配置信息是否成功</summary>
        [TestMethod]
        public void TestCreateMapper()
        {
            AuthorityConfiguration configuration = AuthorityConfigurationView.Instance.Configuration;

            ISqlMapper ibatisMapper = null;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
