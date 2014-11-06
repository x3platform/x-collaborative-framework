namespace X3Platform.Tests.Ajax.Net
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Ajax.Net;
    using X3Platform.Configuration;

    /// <summary></summary>    
    [TestClass]
    public class AjaxRequestTests
    {
        public string hostName = KernelConfigurationView.Instance.HostName;

        // [Ignore]
        [TestMethod]
        public void TestSend()
        {
            AjaxRequestData reqeustData = new AjaxRequestData();

            string outString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            outString += "<root>";
            outString += "<loginName><![CDATA[test]]></loginName>";
            outString += "<password><![CDATA[test]]></password>";
            outString += "</root>";
            // https://passport.x3platform.com/api/connect.auth.authorize
            reqeustData.ActionUri = new Uri("http://passport.x3platform.com/membership.auth.aspx");

            reqeustData.Args.Add("returnType", "xml");
            reqeustData.Args.Add("xml", outString);

            AjaxRequest.Request(reqeustData);
        }
    }
}
