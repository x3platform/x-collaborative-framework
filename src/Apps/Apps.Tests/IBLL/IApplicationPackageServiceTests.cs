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

namespace X3Platform.Apps.Tests.IBLL
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using X3Platform.DigitalNumber;
    using X3Platform.Configuration;
    using X3Platform.Util;

    using X3Platform.Apps.Model;
    using X3Platform.Apps.Configuration;
    
    /// <summary></summary>
    [TestClass]
    public class IApplicationPackageServiceTestSuite
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [TestMethod]
        public void TestSave()
        {
            // 测试应用配置 标识:00000000-0000-0000-0000-000000000000
            ApplicationPackageInfo param = new ApplicationPackageInfo();

            param.Id = "test_" + DateHelper.GetTimestamp();
            param.ApplicationId = ConfigurationManager.AppSettings["appKey"];
            param.Direction = "In";
            param.BeginDate = new DateTime(2000, 1, 1);
            param.EndDate = new DateTime(2000, 1, 1);

            param = AppsContext.Instance.ApplicationPackageService.Save(param);

            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void TestFindOne()
        {
            ApplicationPackageInfo param = new ApplicationPackageInfo();

            param.Id = "test_" + DateHelper.GetTimestamp();
            param.ApplicationId = ConfigurationManager.AppSettings["appKey"];
            param.Direction = "In";
            param.BeginDate = new DateTime(2000, 1, 1);
            param.EndDate = new DateTime(2000, 1, 1);

            param = AppsContext.Instance.ApplicationPackageService.Save(param);

            Assert.IsNotNull(param);

            // 测试应用配置 标识:00000000-0000-0000-0000-000000000000
            param = AppsContext.Instance.ApplicationPackageService.FindOne(param.Id);

            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void TestGetPages()
        {
            int rowCount = -1;

            IList<ApplicationPackageInfo> list = AppsContext.Instance.ApplicationPackageService.GetPaging(0, 10, string.Empty, string.Empty, out rowCount);

            Assert.IsNotNull(list);
        }

        [Ignore]
        [TestMethod]
        public void TestCreateReceivedPackage()
        {
            string applicationId = "test";

            string path = "TestFiles\\2011\\1Q\\3\\1419.xml";

            XmlDocument doc = new XmlDocument();

            doc.Load(path);

            int result = AppsContext.Instance.ApplicationPackageService.CreateReceivedPackage(applicationId, 1419, path, doc);

            Assert.IsTrue(result == 0);
        }
    }
}
