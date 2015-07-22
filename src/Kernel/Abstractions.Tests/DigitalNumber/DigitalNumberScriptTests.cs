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

namespace X3Platform.Tests.DigitalNumber
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Text;

  using X3Platform.DigitalNumber;
  using X3Platform.DigitalNumber.Configuration;

  /// <summary></summary>
  [TestClass]
  public class DigitalNumberScriptTests
  {
    //-------------------------------------------------------
    // 测试内容
    //-------------------------------------------------------

    /// <summary>测试执行脚本是否运行正确</summary>
    [TestMethod]
    public void TestRunScript()
    {
      string result = null;

      int seed = 0;

      seed = 99;

      result = DigitalNumberScript.RunScript("{dailyIncrement:seed:6}", new DateTime(2000, 1, 1), ref seed);

      Assert.AreEqual("000001", result);

      seed = 99;

      result = DigitalNumberScript.RunScript("{dailyIncrement:seed:6}", DateTime.Now, ref seed);

      Assert.AreEqual("000100", result);
    }
  }
}
