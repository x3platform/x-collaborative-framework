namespace X3Platform.Tests.Ajax.Net
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Ajax.Net;
    using X3Platform.Configuration;
    using X3Platform.Security;
    using X3Platform.Json;

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

            //string outString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            //outString += "<root>";
            //outString += "<loginName><![CDATA[test]]></loginName>";
            //outString += "<password><![CDATA[test]]></password>";
            //outString += "</root>";
            // https://passport.x3platform.com/api/connect.auth.authorize
            reqeustData.ActionUri = new Uri("http://local.x3platform.com/api/connect.auth.authorize.aspx?clientId=52cf89ba-7db5-4e64-9c64-3c868b6e7a99&responseType=json");
            // redirectUri=http://project.x3platform.com/sso.aspx
            // reqeustData.Args.Add("returnType", "xml");
            // reqeustData.Args.Add("clientId", "52cf89ba-7db5-4e64-9c64-3c868b6e7a99");
            reqeustData.Args.Add("loginName", "ruanyu@feinno.com");
            reqeustData.Args.Add("password", Encrypter.EncryptSHA1("2014@feinno"));
            // reqeustData.Args.Add("xml", "");
            // reqeustData.Args.Add("responseType", "token");
            // reqeustData.Args.Add("redirectUri", "http://project.x3platform.com/sso.aspx");

            var response = AjaxRequest.Request(reqeustData, "POST");

            var reader = new JsonReader(response);
            
            var o =reader.Value;

            Assert.IsNotNull(response);
        }
    }
}
