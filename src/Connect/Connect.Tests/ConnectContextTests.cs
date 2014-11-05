namespace X3Platform.Connect.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Ajax.Net;

    using X3Platform.Connect.Model;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect;

    /// <summary></summary>
    [TestClass]
    public class ConnectContextTests
    {
        [TestMethod]
        public void TestReload()
        {
            Assert.IsNotNull(ConnectContext.Instance.ConnectService);
        }

        // [TestMethod]
        public void TestOAuth2()
        {
            // 1.获取 authorization_code 授权码

            // http://x10.x3platform.com/api/connect.oauth.authorize.aspx?client_id=05ce2febad3eeaab116a8fc307bcc001&redirect_uri=http://password.x3platform.com/ouath/douban-o.aspx&response_type=code&scope=shuo_basic_r,shuo_basic_w
            // http://x10.x3platform.com/api/connect.oauth.authorize.aspx?client_id=05ce2febad3eeaab116a8fc307bcc001
            // &redirect_uri=http://password.x3platform.com/ouath/douban-o.aspx
            // &response_type=code
            // &scope=shuo_basic_r,shuo_basic_w

            AjaxRequestData requestData = new AjaxRequestData();

            requestData.ActionUri = new Uri("http://x10.x3platform.com/api/connect.oauth.authorize.aspx");
            requestData.Args.Add("client_id", "12345");
            requestData.Args.Add("redirect_uri", "http://password.x3platform.com/api/connect.oauth.token.aspx?authoriz_code=");
            requestData.Args.Add("response_type", "code");
            requestData.Args.Add("scope", "shuo_basic_r,shuo_basic_w");

            string result = AjaxRequest.Request(requestData);

            // 2.获取access_token

            requestData.ActionUri = new Uri("https://x10.x3platform.com/api/connect.oauth.token.aspx");
            requestData.Args.Add("client_id", "12345");
            requestData.Args.Add("redirect_uri", "http://password.x3platform.com/api/connect.auth.getAccessToken.aspx?authoriz_code=");
            requestData.Args.Add("response_type", "code");
            requestData.Args.Add("scope", "shuo_basic_r,shuo_basic_w");

            AjaxRequest.Request(requestData);
        }
    }
}
