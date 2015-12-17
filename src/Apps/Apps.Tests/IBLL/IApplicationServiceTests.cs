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
    public class IApplicationServiceTests
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
            ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOne(ConfigurationManager.AppSettings["appKey"]);

            Assert.IsNotNull(param);
        }

        [Test]
        public void FindOneByApplicationName()
        {
            // 测试应用配置 标识:52cf89ba-7db5-4e64-9c64-3c868b6e7a99
            ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOneByApplicationName("Test");

            Assert.AreEqual(param.Id, ConfigurationManager.AppSettings["appKey"]);
        }

        [Test]
        public void TestGetPaging()
        {
            int rowCount = -1;

            DataQuery query = new DataQuery();

            IList<ApplicationInfo> list = AppsContext.Instance.ApplicationService.GetPaging(0, 10, query, out rowCount);
        }

        [Test]
        public void TestGetAuthorizationScopeObjects()
        {
            AppsContext.Instance.ApplicationService.GetAuthorizationScopeObjects("52cf89ba-7db5-4e64-9c64-3c868b6e7a99", "应用_默认_管理员");
        }
    }
}
