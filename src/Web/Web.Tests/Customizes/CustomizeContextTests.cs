namespace X3Platform.Web.Tests.Customize
{
  using System;
  using System.Text;
  using System.Collections.Generic;
  using System.Diagnostics;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using X3Platform.Web.Customizes;

  /// <summary></summary>
  [TestClass]
  public class CustomizeContextTests
  {
    [TestMethod]
    public void TestRestart()
    {
      CustomizeContext.Instance.Restart();

      Assert.IsNotNull(CustomizeContext.Instance.CustomizePageService);
      Assert.IsNotNull(CustomizeContext.Instance.CustomizeWidgetZoneService);
      Assert.IsNotNull(CustomizeContext.Instance.CustomizeWidgetService);
      Assert.IsNotNull(CustomizeContext.Instance.CustomizeWidgetInstanceService);
    }
  }
}
