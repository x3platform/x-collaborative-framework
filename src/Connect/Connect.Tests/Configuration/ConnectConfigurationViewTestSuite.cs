using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Data.Common;
using System.Text;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Connect.Configuration;
using X3Platform.IBatis.DataMapper;

namespace X3Platform.Connect.TestSuite.Configuration
{
    [TestClass]
    public class ConnectConfigurationViewTestSuite
    {
        /// <summary>��������·��</summary>
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        /// <summary>������ÿ������֮ǰʹ�� TestInitialize ���д���</summary>
        [TestInitialize()]
        public void Initialize()
        {
            ConnectConfigurationView.LoadInstance(fullConfigPath);
        }

        /// <summary>������ÿ������֮��ʹ�� TestCleanup ���д���</summary>
        [TestCleanup()]
        public void Cleanup()
        {
        }

        #region ������������
        //
        // �������ڱ�д����ʱʹ��������������:
        //
        // ���������еĵ�һ������֮ǰʹ�� ClassInitialize ���д���
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // �����е����в��Զ�������֮��ʹ�� ClassCleanup ���д���
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // ������ÿ������֮ǰʹ�� TestInitialize ���д��� 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // ������ÿ������֮��ʹ�� TestCleanup ���д���
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //-------------------------------------------------------
        // ��������
        //-------------------------------------------------------

        [TestMethod]
        public void TestLoad()
        {
            ConnectConfiguration configuration = ConnectConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
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
