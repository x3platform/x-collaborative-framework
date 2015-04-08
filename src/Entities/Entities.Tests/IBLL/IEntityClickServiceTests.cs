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
    public class IEntityClickServiceTestSuite
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
        public void TestSave()
        {
            string customTableName = "tb_Entity_Click";

            IEntityClickInfo param = new EntityClickInfo();

            // 实体信息的标识
            ((EntityClickInfo)param).EntityId = "00000000-0000-0000-0000-000000000001";
            // 实体信息的类名全称
            ((EntityClickInfo)param).EntityClassName = KernelContext.ParseObjectType(typeof(EntityOperationLogInfo));
            // 操作人帐号标识
            ((EntityClickInfo)param).AccountId = "00000000-0000-0000-0000-000000001001";

            param = EntitiesManagement.Instance.EntityClickService.Save(customTableName, param);

            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void TestIncrement()
        {
            string customTableName = "tb_Entity_Click";

            string entityId = "00000000-0000-0000-0000-000000000001";

            string entityClassName = KernelContext.ParseObjectType(typeof(EntityOperationLogInfo));

            string accountId = "00000000-0000-0000-0000-000000001001";

            EntitiesManagement.Instance.EntityClickService.Increment(customTableName, entityId, entityClassName, accountId);
        }

        [TestMethod]
        public void TestFindAllByEntityId()
        {
            string customTableName = "tb_Entity_Click";

            string entityId = "00000000-0000-0000-0000-000000000001";

            string entityClassName = KernelContext.ParseObjectType(typeof(EntityOperationLogInfo));

            IList<IEntityClickInfo> list = EntitiesManagement.Instance.EntityClickService.FindAllByEntityId(customTableName, entityId, entityClassName);

            Assert.IsNotNull(list);
        }
    }
}
