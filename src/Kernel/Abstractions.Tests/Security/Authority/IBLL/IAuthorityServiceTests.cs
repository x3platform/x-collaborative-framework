namespace X3Platform.Tests.Security.Authority.IBLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;
    using System.Xml;

    using NUnit.Framework;

    using X3Platform.IBatis.Common.Utilities;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.IBatis.DataMapper.Configuration;
    using X3Platform.IBatis.DataMapper.Scope;

    using X3Platform.Configuration;

    using X3Platform.DigitalNumber;
    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.Model;
    using X3Platform.DigitalNumber.IBLL;
    using X3Platform.DigitalNumber.IDAL;
    using X3Platform.Util;
    using X3Platform.Security.Authority;
    using X3Platform.Data;

    /// <summary></summary>
    [TestFixture]
    public class IAuthorityServiceTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Test]
        public void FindAll()
        {
            IList<AuthorityInfo> list = AuthorityContext.Instance.AuthorityService.FindAll();

            Assert.IsNotNull(list);
        }

        [Test]
        public void TestGetPaging()
        {
            int rowCount = -1;

            DataQuery query = new DataQuery();

            IList<AuthorityInfo> list = AuthorityContext.Instance.AuthorityService.GetPaging(0, 10, query, out rowCount);

            Assert.IsNotNull(list);

            Assert.AreNotEqual(rowCount, -1);
        }
    }
}