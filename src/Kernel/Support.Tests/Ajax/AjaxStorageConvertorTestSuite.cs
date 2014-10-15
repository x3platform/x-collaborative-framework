using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Ajax;
using X3Platform.Ajax.Configuration;
using System.Xml;

namespace X3Platform.TestSuite.Ajax
{
    [TestClass]
    public class AjaxStorageConvertorTestSuite
    {
        [TestMethod]
        public void TestParse()
        {
            string outString = null;

            AccountInfo targetObject = new AccountInfo();

            targetObject.Id = "10";

            targetObject.Name = "Max";

            targetObject.IP = "127.0.0.1";

            outString = AjaxStorageConvertor.Parse(targetObject);

            Assert.IsNotNull(outString);

            // 
            // �����б�����
            //

            List<AccountInfo> list = new List<AccountInfo>();

            list.Add(new AccountInfo("10", "ff", "127.0.0.1"));

            list.Add(new AccountInfo("20", "ff3", "127.0.0.1"));

            list.Add(new AccountInfo("30", "ff3", "127.0.0.1"));

            outString = AjaxStorageConvertor.Parse(list);

            Assert.IsNotNull(outString);
        }

        [TestMethod]
        public void Deserialize()
        {
            AccountInfo targetObject = new AccountInfo();

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                    + "<ajaxStorage>"
                    + "<id><![CDATA[10]]></id>"
                    + "<name><![CDATA[Max]]></name>"
                    + "<ip><![CDATA[127.0.0.1]]></ip>"
                    + "<createDate><![CDATA[2008-01-01]]></createDate>"
                    + "</ajaxStorage>";

            targetObject = (AccountInfo)AjaxStorageConvertor.Deserialize(targetObject, xml);

            Assert.AreEqual(targetObject.Id, "10");
            Assert.AreEqual(targetObject.Name, "Max");
            Assert.AreEqual(targetObject.IP, "127.0.0.1");
            Assert.AreEqual(targetObject.CreateDate, new DateTime(2008, 1, 1));
        }

        [TestMethod]
        public void TestFetch()
        {
            string result = null;

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                   + "<ajaxStorage>"
                   + "<id><![CDATA[10]]></id>"
                   + "<name><![CDATA[Max]]></name>"
                   + "<ip><![CDATA[127.0.0.1]]></ip>"
                   + "<createDate><![CDATA[2008-01-01]]></createDate>"
                   + "</ajaxStorage>";
            
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            result = AjaxStorageConvertor.Fetch("name", xml);

            Assert.AreEqual(result,"Max");

            result = AjaxStorageConvertor.Fetch("ajaxStorage", "name", xml);

            Assert.AreEqual(result, "Max");

            result = AjaxStorageConvertor.Fetch("ajaxStorage", "updateDate", doc);

            Assert.IsNull(result);
        }
    }

    //
    // ���Զ���
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

        #region ����:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region ����:Name
        private string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region ����:IP
        private string m_IP;

        /// <summary>��Ա��IP��ַ</summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        #endregion

        #region ����:CreateDate
        private DateTime m_CreateDate = DateTime.Now;

        /// <summary>����ʱ��</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
