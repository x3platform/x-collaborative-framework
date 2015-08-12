using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Plugins.Bugs.Model;
using X3Platform.Plugins.Bugs.IBLL;

namespace X3Platform.Plugins.Bugs.Tests
{
  /// <summary></summary>
  [TestClass]
  public class BugContextTests
  {
    //-------------------------------------------------------
    // 测试内容
    //-------------------------------------------------------

    /// <summary>测试 加载</summary>
    [TestMethod]
    public void TestRestart()
    {
      BugContext.Instance.Restart();

      Assert.IsNotNull(BugContext.Instance.BugService);
      Assert.IsNotNull(BugContext.Instance.BugCategoryService);
      Assert.IsNotNull(BugContext.Instance.BugCommentService);
      Assert.IsNotNull(BugContext.Instance.BugHistoryService);
    }
  }
}
