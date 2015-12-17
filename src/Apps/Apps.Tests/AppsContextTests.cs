namespace X3Platform.Apps.Tests
{
    using NUnit.Framework;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.Model;
    using X3Platform.Apps.IBLL;

    /// <summary></summary>
    [TestFixture]
    public class AppsContextTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Test]
        public void TestRestart()
        {
            AppsContext.Instance.Restart();

            Assert.IsNotNull(AppsContext.Instance.ApplicationService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationEventService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationFeatureService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationFeatureDateLimitService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationSettingGroupService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationSettingService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationMenuService);
            Assert.IsNotNull(AppsContext.Instance.ApplicationMethodService);
        }
    }
}
