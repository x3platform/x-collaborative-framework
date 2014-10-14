//=============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :PagingHelper.cs
//
// Summary      :
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2010-01-01
//
//=============================================================================

using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;
using System.Collections;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Dynamic;
using System;
using System.Collections.ObjectModel;
using X3Platform.Ajax.Net;
using X3Platform.Ajax.Json;
using System.Diagnostics;

namespace X3Platform.Tests.Utility
{
    /// <summary></summary>
    [TestClass]
    public class StringHelperTestSuite
    {
        [TestMethod]
        public void TestTo16DigitGuid()
        {
            string result = string.Empty;

            for (int i = 0; i < 100; i++)
            {
                result = StringHelper.To16DigitGuid();

                Trace.WriteLine(result + "(" + result.Length + ")");
                // Assert.IsTrue(result.Length == 16);
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
                // Assert.IsTrue(result.Length == 16);
            }
        }
    }
}
