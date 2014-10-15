namespace Elane.X.Entities.TestSuite.IBLL
{
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Elane.X.Utility;

    using Elane.X.Entities.Configuration;
    using Elane.X.Entities.Model;
    using Elane.X.DigitalNumber;

    /// <summary></summary>
    [TestClass]
    public class IEntityMetaDataServiceTestSuite
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
            // 保存推荐/催办记录
            EntityOperationLogInfo param = new EntityOperationLogInfo();

            // 实体信息的标识
            param.EntityId = "00000000-0000-0000-0000-000000000001";
            // 实体信息的类名全称
            param.EntityClassName = KernelContext.ParseObjectType(typeof(EntityOperationLogInfo));
            // 操作人帐号标识
            param.AccountId = "00000000-0000-0000-0000-000000001001";
            // 操作类型:1.推荐 2.催办
            param.OperationType = 1;
            // 接收对象标识
            param.ToAuthorizationObjectId = "00000000-0000-0000-0000-000000001001";
            // 接收对象类型: Account Role 等
            param.ToAuthorizationObjectType = "Account";
            // 推荐原因 \ 催办理由
            param.Reason = "测试信息";

            param = EntitiesManagement.Instance.EntityOperationLogService.Save("tb_Entity_OperationLog", param);

            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void TestAnalyzeConditionSQL()
        {
            // SELECT 0 FROM tb_News WHERE Id = '05defd25-c595-419d-8eb0-8ccc6f5d0b80' and AccountId IN ( '123' ) 
            string SQL = " SELECT 0 FROM tb_News WHERE Id = '05defd25-c595-419d-8eb0-8ccc6f5d0b80' AND Title IN ( '123' ) ";

            Assert.IsTrue(EntitiesManagement.Instance.EntityMetaDataService.AnalyzeConditionSQL(SQL));
        }
    }
}
