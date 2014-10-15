using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Elane.X.Entities.IBLL;
using Elane.X.Entities.Configuration;

namespace Elane.X.Entities.TestSuite
{
    /// <summary></summary>
    [TestClass]
    public class EntitiesManagementTestSuite
    {
        /// <summary>测试配置路径</summary>
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        /// <summary>在运行每个测试之前使用 TestInitialize 运行代码</summary>
        [TestInitialize()]
        public void Initialize()
        {
            EntitiesConfigurationView.LoadInstance(fullConfigPath);
        }

        /// <summary>在运行每个测试之后使用 TestCleanup 运行代码</summary>
        [TestCleanup()]
        public void Cleanup()
        {
        }

        #region 其他测试属性
        //
        // 您可以在编写测试时使用下列其他属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前使用 TestInitialize 运行代码 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在运行每个测试之后使用 TestCleanup 运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [TestMethod]
        public void TestReload()
        {
            try
            {
                EntitiesManagement.Instance.Restart();

                Assert.IsNotNull(EntitiesManagement.Instance.EntityClickService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntityDocObjectService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntityDraftService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntityImplementationService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntityLifeHistoryService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntityMetaDataService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntityOperationLogService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntitySchemaService);
                Assert.IsNotNull(EntitiesManagement.Instance.EntitySnapshotService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
