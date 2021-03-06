﻿namespace X3Platform.Apps.Tests
{
    using NUnit.Framework;

    using NMock;
    
    using System.Configuration;

    using X3Platform.Membership;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.Model;
    
    /// <summary></summary>
    [TestFixture]
    public class SecurityTokenManagerTests
    {
        //初始化mockery
        private MockFactory factory = new MockFactory();

        // -------------------------------------------------------
        // 测试内容
        // -------------------------------------------------------

        [Test]
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
            // accountService = new AccountService(mockCurrencyService); //用mock对象初始化accountService
            //    // 模拟mossadmin帐号
            //    administrator = new TestAccountInfo("00000000-0000-0000-0000-000000001001", "mossadmin", "mossadmin(模拟)");

            // member = new TestAccountInfo("00000000-0000-0000-0000-000000001000", "mossguest", "mossguest(模拟)");

            IAccountInfo administrator = administorMock.MockObject;
            IAccountInfo member = memberMock.MockObject;

            Assert.AreEqual(administrator.Id, "00000000-0000-0000-0000-000000001001");
            Assert.AreEqual(administrator.LoginName, "admin");

            Assert.AreEqual(administrator.Id, "00000000-0000-0000-0000-000000001001");
            Assert.AreEqual(administrator.LoginName, "admin");

            ApplicationInfo application = AppsContext.Instance.ApplicationService.FindOne(ConfigurationManager.AppSettings["appKey"]);

            result = AppsSecurity.IsAdministrator(administrator, application.ApplicationName);
            Assert.IsTrue(result);

            result = AppsSecurity.IsAdministrator(member, application.ApplicationName);
            Assert.IsFalse(result);
        }
    }
}
