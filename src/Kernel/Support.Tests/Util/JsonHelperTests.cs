
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X3Platform.Tests.Util
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using X3Platform.Ajax.Json;
    using X3Platform.Util;

    /// <summary></summary>
    [TestClass]
    public class JsonHelperTests
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