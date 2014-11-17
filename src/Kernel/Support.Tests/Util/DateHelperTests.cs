
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Ajax.Json;
using X3Platform.Util;
using System;

namespace X3Platform.Tests.Util
{
    /// <summary>测试时间处理辅助类</summary>
    [TestClass]
    public class DateHelperTests
    {
        [TestMethod]
        public void TestGetTimestamp()
        {
            // Date {Fri Nov 14 2014 17:00:21 GMT+0800} == 1415955621
            var Timestamp = DateHelper.GetTimestamp(new DateTime(2014, 11, 14, 17, 0, 21));

            Assert.AreEqual(1415955621, Timestamp);
        }

        [TestMethod]
        public void TestToDateTime()
        {
            // Date {Fri Nov 14 2014 17:00:21 GMT+0800} == 1415955621
            var datetime = DateHelper.ToDateTime(1415955621);

            Assert.AreEqual(new DateTime(2014, 11, 14, 17, 0, 21), datetime);
        }
    }
}