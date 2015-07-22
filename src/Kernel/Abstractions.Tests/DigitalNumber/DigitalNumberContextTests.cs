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

  using X3Platform.DigitalNumber;

  using X3Platform.DigitalNumber.Configuration;

  /// <summary></summary>
  [TestClass]
  public class DigitalNumberContextTests
  {
    //-------------------------------------------------------
    // ≤‚ ‘ƒ⁄»›
    //-------------------------------------------------------

    /// <summary>≤‚ ‘ º”‘ÿ</summary>
    [TestMethod]
    public void TestLoad()
    {
      Assert.IsNotNull(DigitalNumberContext.Instance.DigitalNumberService);
    }
  }
}
