using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using X3Platform.Configuration;
using System.IO;
using System.Diagnostics;
using X3Platform.Util;

namespace X3Platform.Tests.Xml
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class XslTransformTestSuite
    {
        #region 属性:TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        [TestMethod]
        public void TestToXml()
        {
            string json = @"{'ajaxStorage':[{'id':'5', 'date':'2009-05-16 08:03:13', 'thread':'Agent: adapter run thread for test 'TestWrite' with id 'f7ba2c2b-69c2-4148-8c1f-16552fd543ce'', 'level':'WARN', 'message':'warn', 'exception':'System.Exception' },{'id':'4', 'date':'2009-05-16 08:03:13', 'thread':'Agent: adapter run thread for test 'TestWrite' with id 'f7ba2c2b-69c2-4148-8c1f-16552fd543ce'', 'level':'DEBUG', 'message':'debug', 'exception':'System.Exception' },{'id':'3', 'date':'2009-05-16 08:03:13', 'thread':'Agent: adapter run thread for test 'TestWrite' with id 'f7ba2c2b-69c2-4148-8c1f-16552fd543ce'', 'level':'INFO', 'message':'info', 'exception':'System.Exception' },{'id':'2', 'date':'2009-05-16 08:03:13', 'thread':'Agent: adapter run thread for test 'TestWrite' with id 'f7ba2c2b-69c2-4148-8c1f-16552fd543ce'', 'level':'FATAL', 'message':'fatal', 'exception':'System.Exception: System.Exception: 发生了一个致命错误\u000d\u000a' },{'id':'1', 'date':'2009-05-16 08:03:13', 'thread':'Agent: adapter run thread for test 'TestWrite' with id 'f7ba2c2b-69c2-4148-8c1f-16552fd543ce'', 'level':'ERROR', 'message':'error', 'exception':'System.Exception: System.Exception: 发生了一个异常\u000d\u000a' }],'referencePages':{'pageSize':'10','rowIndex':'0','rowCount':'5','pageSize':'10','pageCount':'1','firstPage':'1','previousPage':'1','nextPage':'1','lastPage':'1'},'message':{'returnCode':0,'value':'查询成功。'}}";

            int index = 0;

            if (json.IndexOf("{'ajaxStorage':") == 0)
            {
                index = json.IndexOf("'message':{'returnCode'");

                string message = "{" + json.Substring(index, json.Length - index - 1) + "}";

                json = json.Remove(index - 1);

                index = json.IndexOf("'referencePages':{'pageSize'");

                string pages = "{" + json.Substring(index, json.Length - index) + "}";

                json = json.Remove(index - 1);

                index = json.IndexOf("'ajaxStorage':");

                string ajaxStorage = "{" + json.Substring(index, json.Length - index) + "}";

                XmlHelper.ToXml(ajaxStorage);
            }
            else
            {
            }
        }

        [TestMethod]
        public void TestTransform()
        {
            // string testPathRoot = KernelConfigurationView.Instance.MSTestPathRoot;
            string testPathRoot = "";
            XslCompiledTransform xslt = new XslCompiledTransform();

            // XsltSettings setting = new XsltSettings(true, true);

            xslt.Load(testPathRoot + "/xml/test.xsl");//装入xsl文件名

            XmlDocument doc = new XmlDocument();

            doc.Load(testPathRoot + "/xml/test.xml");
//            doc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
//<Tasks>
//    <Task Type=""1"" ReadFlag=""0"">
//        <TaskName>安装游戏</TaskName>
//        <UserName/>
//        <GameID>777</GameID>
//        <GameName>小红帽</GameName>
//        <LocalInstallPath>C:\\Documents and Settings\\Administrator\\桌面\\演示版本\\BBM</LocalInstallPath>
//        <TotalBytes>0</TotalBytes>
//        <AddedTime>2008-05-15 18:17:23</AddedTime>  
//    </Task>
//</Tasks>");

            TextWriter text;

            System.IO.MemoryStream memoryStream = new MemoryStream();

            XmlWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);//创建一个xml文档
            //XmlWriter xmlWriter = new XmlTextWriter(text);//创建一个xml文档

            xslt.Transform(doc, xmlWriter);

            xmlWriter.Close();

            // xslt.OutputSettings.Encoding;

            Trace.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }
}
