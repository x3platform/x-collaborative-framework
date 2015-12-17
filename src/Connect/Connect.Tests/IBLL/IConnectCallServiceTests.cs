namespace X3Platform.Connect.Tests.IBLL
{
    using NUnit.Framework;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    
    using Quartz;
    
    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Data;
    using X3Platform.Connect;
    using X3Platform.Membership;
    using X3Platform.Util;
    using X3Platform.DigitalNumber;
    using X3Platform.Messages;
    using X3Platform.Spring;
    using X3Platform.Connect.Jobs;

    /// <summary></summary>
    [TestFixture]
    public class IConnectCallServiceTests
    {
        [Test]
        public void TestSave()
        {
            Assert.IsNotNull(KernelContext.Current.AuthenticationManagement);
            Assert.IsNotNull(KernelContext.Current.User);

            ConnectCallInfo param = new ConnectCallInfo();

            param.Id = "test-" + DateHelper.GetTimestamp();

            param.AppKey = Guid.NewGuid().ToString();

            param = ConnectContext.Instance.ConnectCallService.Save(param);

            Assert.IsNotNull(param);
        }

        [Test]
        public void TestFindAll()
        {
            IList<ConnectCallInfo> list = ConnectContext.Instance.ConnectCallService.FindAll();

            Assert.IsNotNull(list);
        }

        [Test]
        public void TestGetPaging()
        {
            int rowCount = -1;

            var query = new DataQuery();

            IList<ConnectCallInfo> list = ConnectContext.Instance.ConnectCallService.GetPaging(0, 10, query, out rowCount);

            Assert.IsNotNull(list);
        }
    }
}
