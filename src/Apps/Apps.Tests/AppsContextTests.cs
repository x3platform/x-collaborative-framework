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
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.Model;
    using X3Platform.Apps.IBLL;

    /// <summary></summary>
    [TestClass]
    public class AppsContextTestSuite
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [TestMethod]
        public void TestReload()
        {
            AppsContext.Instance.Restart();

            Assert.IsNotNull(AppsContext.Instance.ApplicationService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationEventService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationFeatureService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationFeatureDateLimitService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationPackageService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationPackageLogService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationSettingGroupService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationSettingService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationMenuService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationMethodService);
        }
    }
}
