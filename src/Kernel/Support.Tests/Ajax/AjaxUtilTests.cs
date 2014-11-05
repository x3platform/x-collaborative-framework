using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Ajax;
using X3Platform.Ajax.Configuration;
using System.Xml;
using X3Platform.Util;

namespace X3Platform.Tests.Ajax
{
    [TestClass]
    public class AjaxUtilTests
    {
        [TestMethod]
        public void TestParse()
        {
            string outString = null;

            AccountInfo targetObject = new AccountInfo();

            targetObject.Id = "10";

            targetObject.Name = "x3platform";

            targetObject.IP = "127.0.0.1";

            outString = AjaxUtil.Parse(targetObject);

            Assert.IsNotNull(outString);

            // 
            // 测试列表类型
            //

            IList<AccountInfo> list = new List<AccountInfo>();

            list.Add(new AccountInfo("10", "ff", "127.0.0.1"));

            list.Add(new AccountInfo("20", "ff3", "127.0.0.1"));

            list.Add(new AccountInfo("30", "ff3", "127.0.0.1"));

            outString = AjaxUtil.Parse(list);

            Assert.IsNotNull(outString);
        }

        [TestMethod]
        public void Deserialize()
        {
            AccountInfo targetObject = new AccountInfo();

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                    + "<ajaxStorage>"
                    + "<id><![CDATA[9999]]></id>"
                    + "<name><![CDATA[x3platform]]></name>"
                    + "<ip><![CDATA[127.0.0.1]]></ip>"
                    + "<createDate><![CDATA[2015-01-01]]></createDate>"
                    + "</ajaxStorage>";

            targetObject = (AccountInfo)AjaxUtil.Deserialize(targetObject, xml);

            Assert.AreEqual("9999", targetObject.Id);
            Assert.AreEqual("x3platform", targetObject.Name);
            Assert.AreEqual("127.0.0.1", targetObject.IP);
            Assert.AreEqual(new DateTime(2015, 1, 1), targetObject.CreateDate);
        }

        [TestMethod]
        public void TestFetch()
        {
            string result = null;

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                   + "<ajaxStorage>"
                   + "<id><![CDATA[10]]></id>"
                   + "<name><![CDATA[x3platform]]></name>"
                   + "<ip><![CDATA[127.0.0.1]]></ip>"
                   + "<createDate><![CDATA[2010-01-01]]></createDate>"
                   + "<paging><pageSize><![CDATA[10]]></pageSize><pageIndex><![CDATA[100]]></pageIndex></paging>"
                   + "</ajaxStorage>";

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            result = XmlHelper.Fetch("name", xml);

            Assert.AreEqual(result, "x3platform");

            result = XmlHelper.Fetch("paging", "pageSize", xml);

            Assert.AreEqual("10", result);

            result = XmlHelper.Fetch("paging", "pageIndex", doc);

            Assert.AreEqual("100", result);
        }
    }

    //
    // 测试对象
    //

    public class AccountInfo
    {
        public AccountInfo() { }

        public AccountInfo(string id, string name, string ip)
        {
            this.Id = id;
            this.Name = name;
            this.IP = ip;
        }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:IP
        private string m_IP;

        /// <summary>会员的IP地址</summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate = DateTime.Now;

        /// <summary>创建时间</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
