using System;

using Xunit;

using X3Platform.Ajax.Net;
using X3Platform.Configuration;

namespace X3Platform.Tests.Ajax.Net
{
    /// <summary></summary>
    public class AjaxRequestTests
    {
        public string hostName = KernelConfigurationView.Instance.HostName;

        [Fact]
        public void TestSend()
        {
            AjaxRequestData reqeustData = new AjaxRequestData();

            string outString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            outString += "<root>";
            outString += "<loginName><![CDATA[test]]></loginName>";
            outString += "<password><![CDATA[test]]></password>";
            outString += "</root>";

            reqeustData.ActionUri = new Uri("http://passport.x3platform.com/membership.auth.aspx");

            reqeustData.Args.Add("returnType", "xml");
            reqeustData.Args.Add("xml", outString);

            AjaxRequest.Request(reqeustData);
        }
    }
}
