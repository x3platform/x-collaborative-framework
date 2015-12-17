namespace X3Platform.Apps.Tests.IBLL
{
    using NUnit.Framework;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using X3Platform.Data;

    using X3Platform.Apps.Model;
    using X3Platform.Apps.Configuration;

    /// <summary></summary>
    [TestFixture]
    public class IApplicationOptionServiceTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Test]
        public void TestSave()
        {
        }

        [Test]
        public void TestFindOne()
        {
            // 测试应用配置 标识:52cf89ba-7db5-4e64-9c64-3c868b6e7a99
            ApplicationOptionInfo param = AppsContext.Instance.ApplicationOptionService.FindOne("Domain");

            Assert.IsNotNull(param);
            Assert.AreEqual(param.Value, "x3platform.com");
        }

        [Test]
        public void TestGetPaging()
        {
            int rowCount = -1;

            DataQuery query = new DataQuery();

            IList<ApplicationOptionInfo> list = AppsContext.Instance.ApplicationOptionService.GetPaging(0, 10, query, out rowCount);

            Assert.IsTrue(rowCount >= 0);
        }
    }
}
