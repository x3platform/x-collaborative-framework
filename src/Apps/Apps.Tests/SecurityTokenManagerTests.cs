#region Using Testing Libraries
#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestContext = System.Object;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

using NMock;
#endregion

namespace X3Platform.Apps.Tests
{
    using System.Configuration;

    using X3Platform.Membership;

    using X3Platform.Apps.Configuration;

    /// <summary></summary>
    [TestClass]
    public class SecurityTokenManagerTests
    {
        //初始化mockery
        private MockFactory factory = new MockFactory();

        // -------------------------------------------------------
        // 测试内容
        // -------------------------------------------------------

        [TestMethod]
        public void TestIsAdministrator()
        {
            bool result = false;

            Mock<IAccountInfo> administorMock = this.factory.CreateMock<IAccountInfo>(); //产生一个mock对象

            administorMock.Expects.Between(0, 5).GetProperty(m => m.Id, "00000000-0000-0000-0000-000000001001");
            administorMock.Expects.Between(0, 5).GetProperty(m => m.LoginName, "admin");
            administorMock.Expects.Between(0, 5).GetProperty(m => m.Name, "超级管理员(模拟)");

            Mock<IAccountInfo> memberMock = this.factory.CreateMock<IAccountInfo>();

            memberMock.Expects.Between(0, 5).GetProperty(m => m.Id, "00000000-0000-0000-0000-000000001000");
            memberMock.Expects.Between(0, 5).GetProperty(m => m.LoginName, "guest");
            memberMock.Expects.Between(0, 5).GetProperty(m => m.Name, "guest(模拟)");

            //
            // 测试会议管理
            // 
            //accountService = new AccountService(mockCurrencyService); //用mock对象初始化accountService
            //    // 模拟mossadmin帐号
            //    administrator = new TestAccountInfo("00000000-0000-0000-0000-000000001001", "mossadmin", "mossadmin(模拟)");

            //member = new TestAccountInfo("00000000-0000-0000-0000-000000001000", "mossguest", "mossguest(模拟)");

            IAccountInfo administrator = administorMock.MockObject;
            IAccountInfo member = memberMock.MockObject;

            Assert.AreEqual(administrator.Id, "00000000-0000-0000-0000-000000001001");
            Assert.AreEqual(administrator.LoginName, "admin");

            Assert.AreEqual(administrator.Id, "00000000-0000-0000-0000-000000001001");
            Assert.AreEqual(administrator.LoginName, "admin");

            result = AppsSecurity.IsAdministrator(administrator, "Meeting");
            Assert.IsTrue(result);

            result = AppsSecurity.IsAdministrator(member, "Meeting");
            Assert.IsFalse(result);

            //
            // 测试工作联系
            //

            result = AppsSecurity.IsAdministrator(administrator, "WorkRelation");
            Assert.IsTrue(result);

            result = AppsSecurity.IsAdministrator(member, "WorkRelation");
            Assert.IsFalse(result);
        }
    }
}
