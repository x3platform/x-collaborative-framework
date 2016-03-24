namespace X3Platform.Tests.Util
{
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Util;
    using X3Platform.CacheBuffer;

    /// <summary></summary>
    [TestClass]
    public class CachingManagerTests
    {
        [TestMethod]
        public void TestAdd()
        {
            CachingManager.Add("a", 123);

            CachingManager.Get("a");
        }
    }
}
