namespace X3Platform.AttachmentStorage.Tests.Configuration
{
    using NUnit.Framework;

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    using X3Platform.IBatis.DataMapper;

    using X3Platform.AttachmentStorage.Configuration;

    [TestFixture]
    public class AttachmentStorageConfigurationViewTests
    {
        //-------------------------------------------------------
        // ≤‚ ‘ƒ⁄»›
        //-------------------------------------------------------

        [Test]
        public void TestInit()
        {
            AttachmentStorageConfiguration configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);

            Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
            Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
        }

        [Test]
        public void TestCreateMapper()
        {
            AttachmentStorageConfiguration configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            ISqlMapper ibatisMapper = null;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
