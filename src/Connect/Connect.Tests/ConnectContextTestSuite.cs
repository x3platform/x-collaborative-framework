using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Connect.Model;
using X3Platform.Connect.IBLL;
using X3Platform.Ajax.Net;

namespace X3Platform.Connect.TestSuite
{
    /// <summary></summary>
    [TestClass]
    public class ConnectContextTestSuite
    {
        public ConnectContextTestSuite()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 其他测试属性
        //
        // 您可以在编写测试时使用下列其他属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前使用 TestInitialize 运行代码 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在运行每个测试之后使用 TestCleanup 运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestReload()
        {
            IConnectService instance = ConnectContext.Instance.ConnectService;

            //instance = ConnectEngine.GetConnectProvider("Current Connect");

            ConnectInfo param = new ConnectInfo();

            param.Description = DateTime.Now.ToString();

            param.CreateDate = DateTime.Now;

            param = instance.Save(param);

            System.Console.WriteLine(instance.FindOne(param.Id).Description);

            System.Console.WriteLine();
        }

        [TestMethod]
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
