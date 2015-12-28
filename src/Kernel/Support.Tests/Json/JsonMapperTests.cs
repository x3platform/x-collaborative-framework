using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Data;
using X3Platform.Json;

namespace X3Platform.Tests.Json
{
    [TestClass]
    public class JsonMapperTests
    {
        [TestMethod]
        public void TestToDynamicObject()
        {
            string json = ResourceStringLoader.LoadString("X3Platform.Tests.defaults.test_json");

            // string json = "{\"id\":\"0001\",\"name\":\"x3platform\",\"age\":5,\"groups\":[0,1,2,3],\"createDate\":\"1970-01-01\"}";

            dynamic obj = JsonMapper.ToDynamicObject(json);

            Assert.AreEqual("0001", obj.id);
            Assert.AreEqual("x3platform", obj.name);
            Assert.AreEqual(true, obj.locking);
            Assert.AreEqual(5, obj.age);
            Assert.AreEqual(null, obj.nullValue);
            Assert.AreEqual("x3", obj.objectValue.name);
            Assert.AreEqual(0, obj.groups[0]);
            Assert.AreEqual(1, obj.groups[1]);
            Assert.AreEqual(2, obj.groups[2]);
            Assert.AreEqual(3, obj.groups[3]);
           
            string result = AjaxUtil.Parse<dynamic>(obj);

            string result1 = JsonMapper.ToJson(obj);

            Assert.AreEqual("a1", obj.name);

            TestInfo t = new TestInfo();

            t.Id = "a";

            dynamic obj1 = JsonMapper.ToDynamicObject(t);
        }
    }

    public class TestInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// �������ʺ�ID
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string CategoryIndex { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ͼ���ַ
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// ���״̬
        /// </summary>
        public short AuditFlag { get; set; }

        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// ����ԱId
        /// </summary>
        public long AdminId { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public string AdminMemo { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// ��Ա��ϵ
        /// </summary>
        // public IList<CircleMemberRelationInfo> MemberRelations { get; set; }
    }
}
