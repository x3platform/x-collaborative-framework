using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Elane.X.Entities.Configuration;
using Elane.X.IBatis.DataMapper;

namespace Elane.X.Entities.TestSuite.Configuration
{
    [TestClass]
    public class EntitiesConfigurationViewTestSuite
    {
        /// <summary>��������·��</summary>
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        /// <summary>������ÿ������֮ǰʹ�� TestInitialize ���д���</summary>
        [TestInitialize()]
        public void Initialize()
        {
            EntitiesConfigurationView.LoadInstance(fullConfigPath);
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

        // -------------------------------------------------------
        // ��������
        // -------------------------------------------------------

        [TestMethod]
        public void TestLoad()
        {
            EntitiesConfiguration configuration = EntitiesConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }

        [TestMethod]
        public void TestCreateMapper()
        {
            EntitiesConfiguration configuration = EntitiesConfigurationView.Instance.Configuration;

            ISqlMapper ibatisMapper = null;

            string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

            Assert.IsNotNull(ibatisMapper);
        }
    }
}
