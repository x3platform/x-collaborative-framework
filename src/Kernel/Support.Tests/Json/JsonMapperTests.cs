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
        /// 创建人帐号ID
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类索引
        /// </summary>
        public string CategoryIndex { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public short AuditFlag { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 管理员Id
        /// </summary>
        public long AdminId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string AdminMemo { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 成员关系
        /// </summary>
        // public IList<CircleMemberRelationInfo> MemberRelations { get; set; }
    }
}
