namespace X3Platform.Connect.Tests.IBLL
{
    using NUnit.Framework;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Data;
    using X3Platform.Connect;
    using X3Platform.Membership;
    using X3Platform.Util;
    using X3Platform.DigitalNumber;

    /// <summary></summary>
    [TestFixture]
    public class IConnectServiceTests
    {
        [Test]
        public void TestSave()
        {
            Assert.IsNotNull(KernelContext.Current.AuthenticationManagement);
            Assert.IsNotNull(KernelContext.Current.User);

            ConnectInfo param = new ConnectInfo();

            param.Id = "test-" + DateHelper.GetTimestamp();

            param.AppKey = Guid.NewGuid().ToString();

            param.Description = DateTime.Now.ToString();

            param.CreatedDate = DateTime.Now;

            param = ConnectContext.Instance.ConnectService.Save(param);

            Assert.IsNotNull(param);
        }

        [Test]
        public void TestFindAll()
        {
            IList<ConnectInfo> list = ConnectContext.Instance.ConnectService.FindAll();

            Assert.IsNotNull(list);
        }

        [Test]
        public void TestGetPaging()
        {
            int rowCount = -1;

            var query = new DataQuery();

            IList<ConnectInfo> list = ConnectContext.Instance.ConnectService.GetPaging(0, 10, query, out rowCount);

            Assert.IsNotNull(list);
        }
    }
}
