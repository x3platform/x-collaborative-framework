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

    /// <summary></summary>
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void TestTo16DigitGuid()
        {
            string result = string.Empty;

            for (int i = 0; i < 100; i++)
            {
                result = StringHelper.To16DigitGuid();

                Trace.WriteLine(result + "(" + result.Length + ")");
                Assert.IsTrue(result.Length >= 16);
            }
        }

        [TestMethod]
        public void TestInt64Guid()
        {
            string result = string.Empty;

            for (int i = 0; i < 100; i++)
            {
                result = StringHelper.ToInt64Guid();

                Trace.WriteLine(result + "(" + result.Length + ")");
                Assert.IsTrue(result.Length == 19);
            }
        }

        /// <summary></summary>
        [TestMethod]
        public void TestToSafeIds()
        {
            string result = string.Empty;

            result = StringHelper.ToSafeIds("1000");
            Assert.AreEqual(result, "1000");

            result = StringHelper.ToSafeIds("1000,'2000'");
            Assert.AreEqual(result, "1000,2000");

            result = StringHelper.ToSafeIds("1234-1234-12345678;,'2000'");
            Assert.AreEqual(result, "1234-1234-12345678,2000");
        }

        [TestMethod]
        public void TestToSUID()
        {
            Debug.WriteLine(StringHelper.ToSID());

            Debug.WriteLine(StringHelper.ToSID(3));

            Debug.WriteLine(StringHelper.ToSID(15));

            Debug.WriteLine(StringHelper.ToSID(16));

            Debug.WriteLine(StringHelper.ToSID(22));
        }

        /// <summary></summary>
        [TestMethod]
        public void TestCamelToUnderline()
        {
            string result = string.Empty;

            result = StringHelper.CamelToUnderline("TestToSafeIds");
            Assert.AreEqual(result, "test_to_safe_ids");
        }
    }
}
