using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Elane.X.Entities.Configuration;
using System.Configuration;
using Elane.X.Spring.Configuration;
using Elane.X.Utility;
using Elane.X.Entities.Model;

namespace Elane.X.Entities.TestSuite.IBLL
{
    /// <summary></summary>
    [TestClass]
    public class IEntityLifeHistoryServiceTestSuite
    {
        /// <summary>测试配置路径</summary>
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        /// <summary>垃圾桶</summary>
        public Dictionary<string, EntityLifeHistoryInfo> trash = new Dictionary<string, EntityLifeHistoryInfo>();

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
        public void TestSave()
        {
            EntityLifeHistoryInfo param = new EntityLifeHistoryInfo();

            param.Id = StringHelper.ToGuid();

            param = EntitiesManagement.Instance.EntityLifeHistoryService.Save(param);

            Assert.IsNotNull(param);

            trash.Add(param.Id, param);
        }

        [TestMethod]
        public void TestFindOne()
        {
            EntityLifeHistoryInfo param = new EntityLifeHistoryInfo();

            param.Id = StringHelper.ToGuid();

            EntitiesManagement.Instance.EntityLifeHistoryService.Save(param);

            param = EntitiesManagement.Instance.EntityLifeHistoryService.FindOne(param.Id);

            Assert.IsNotNull(param);

            trash.Add(param.Id, param);
        }

        [TestMethod]
        public void TestFindAll()
        {
            IList<EntityLifeHistoryInfo> list = EntitiesManagement.Instance.EntityLifeHistoryService.FindAll();

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestGetPages()
        {
            int rowCount = -1;

            IList<EntityLifeHistoryInfo> list = EntitiesManagement.Instance.EntityLifeHistoryService.GetPages(0, 10, string.Empty, string.Empty, out rowCount);

            Assert.IsNotNull(list);
        }
    }
}
