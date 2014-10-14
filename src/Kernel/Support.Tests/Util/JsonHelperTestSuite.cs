//=============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :PagingHelper.cs
//
// Summary      :
//
// Author       :X3Platform
//
// Date			:2010-01-01
//
//=============================================================================


using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Ajax.Json;
using X3Platform.Util;

namespace X3Platform.Tests.Utility
{
    /// <summary></summary>
    [TestClass]
    public class JsonHelperTestSuite
    {
        [TestMethod]
        public void TestToHashtable()
        {
            string text = "{\"name\": \"Max\",\"msn\": \"ruanyu1983@msn.com\"}";

            Hashtable table = JsonHelper.ToHashtable(text);

            text = "{\"ajaxStorage\":{\"name\": \"Max\",\"msn\": \"ruanyu1983@msn.com\"}}";

            table = JsonHelper.ToHashtable(text);
        }

        [TestMethod]
        public void TestToHashtable1()
        {
            string text = "{\"ajaxStorage\":{\"name\": \"Max\",\"msn\": \"ruanyu1983@msn.com\"}}";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // serializer.RegisterConverters.Serialize();

            Dictionary<string, object> JsonData = serializer.Deserialize<Dictionary<string, object>>(text);
        }

        [TestMethod]
        public void TestToDynamic()
        {
            string json = "{name:'hooyes',pwd:'hooyespwd',books:{a:'��¥��',b:'ˮ䰴�',c:{arr:['����','������']}},arr:['good','very good']}";

            JsonObject result = JsonObjectConverter.Deserialize(json);
        }

        [TestMethod]
        public void TestToSafeJson()
        {
            string result = StringHelper.ToSafeJson("����");

            Assert.IsNotNull(result);
        }
    }
}