using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Plugins.Bugs.Configuration;
using X3Platform.Plugins.Bugs.Model;
using X3Platform.Data;

namespace X3Platform.Plugins.Bugs.Tests.IBLL
{
  /// <summary></summary>
  [TestClass]
  public class BugServiceTests
  {
    [TestMethod]
    public void TestGetPaging()
    {
      int rowCount = -1;
      
      DataQuery query = new DataQuery();

      IList<BugInfo> list = BugContext.Instance.BugService.GetPaging(0, 10, query, out rowCount);

      Assert.IsNotNull(list);
    }

    [TestMethod]
    public void TestFindAll()
    {
      IList<BugInfo> list = BugContext.Instance.BugService.FindAll();

      Assert.IsNotNull(list);
    }
  }
}
