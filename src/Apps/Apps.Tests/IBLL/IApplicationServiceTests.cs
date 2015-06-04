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

  using X3Platform.Data;

  using X3Platform.Apps.Model;
  using X3Platform.Apps.Configuration;

  /// <summary></summary>
  [TestClass]
  public class IApplicationServiceTests
  {
    //-------------------------------------------------------
    // 测试内容
    //-------------------------------------------------------

    [TestMethod]
    public void TestSave()
    {
    }

    [TestMethod]
    public void TestFindOne()
    {
      // 测试应用配置 标识:52cf89ba-7db5-4e64-9c64-3c868b6e7a99
      ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOne(ConfigurationManager.AppSettings["appKey"]);

      Assert.IsNotNull(param);
    }

    [TestMethod]
    public void FindOneByApplicationName()
    {
      // 测试应用配置 标识:52cf89ba-7db5-4e64-9c64-3c868b6e7a99
      ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOneByApplicationName("Test");

      Assert.AreEqual(param.Id, ConfigurationManager.AppSettings["appKey"]);
    }

    [TestMethod]
    public void TestGetPaging()
    {
      int rowCount = -1;

      DataQuery query = new DataQuery();

      IList<ApplicationInfo> list = AppsContext.Instance.ApplicationService.GetPaging(0, 10, query, out rowCount);
    }

    [TestMethod]
    public void TestGetAuthorizationScopeObjects()
    {
      AppsContext.Instance.ApplicationService.GetAuthorizationScopeObjects("52cf89ba-7db5-4e64-9c64-3c868b6e7a99", "应用_默认_管理员");
    }
  }
}
