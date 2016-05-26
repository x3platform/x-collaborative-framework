namespace X3Platform.Experiments.Tests.Security.Authority
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using NUnit.Framework;

    using NMock;

    using X3Platform.Security;
    using X3Platform.Security.Authority;
    using X3Platform.Membership;

    /// <summary>权限检测</summary>
    [TestFixture]
    public class AuthorityCheckerTestSuite
    {
        /// <summary>测试DES加密</summary>
        [Test]
        public void TestCheck()
        {
            /*
            Mockery mocks = new Mockery();  //初始化mocks
            //setup
            // AuthorityInfo authority = (AuthorityInfo)mocks.CreateMock(typeof(AuthorityInfo));

            IAccountInfo mockAccount = mocks.NewMock<IAccountInfo>(); 

            Expect.Once.On(mockAccount).GetProperty("Id").Will(Return.Value("00000000-0000-0000-0000-000000100000"));

            // IRoleInfo role = mocks.CreateMock(typeof(IRoleInfo)) as IRoleInfo;

            // List<IRoleInfo> roles = new List<IRoleInfo>();

            // record with expectation model
            // roles.Add(role);

            // Expect.On(authority).Call(authority.Name).Return("1233");

            // Expect.Call(account.Name).SetPropertyWithArgument("1233");

            // 
            // mocks.ReplayAll();
            // account.Id = "1243";
            
            bool result = false;

            result = AuthorityChecker.HasRole(mockAccount.Id, "政府业务团队_成员");

            Assert.IsTrue(result);
            
            mocks.VerifyAllExpectationsHaveBeenMet();
       */ }

        /// <summary>测试DES加密</summary>
        [Test]
        public void TestGetAuthorities()
        {
            // IAccountInfo account = null;

            // AuthorityChecker.GetAuthorities();
        }

    }
}
