using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Plugins.Bugs.Configuration;
using X3Platform.Plugins.Bugs.Model;
using X3Platform.Web.Customizes;
using X3Platform.Plugins.Bugs.Customizes;
using System.Configuration;

namespace X3Platform.Plugins.Bugs.Tests.Customizes
{
  /// <summary></summary>
  [TestClass]
  public class BugWidgetTests
  {
    [TestMethod]
    public void TestLoad()
    {
      IWidget widget = (IWidget)KernelContext.CreateObject(KernelContext.ParseObjectType(typeof(BugWidget)));

      widget.Load("{}");
    }

    [TestMethod]
    public void TestParseHtml()
    {
      IWidget widget = (IWidget)KernelContext.CreateObject(KernelContext.ParseObjectType(typeof(BugWidget)));

      widget.Load("{}");

      string result = widget.ParseHtml();

      Assert.IsNotNull(result);
    }
  }
}
