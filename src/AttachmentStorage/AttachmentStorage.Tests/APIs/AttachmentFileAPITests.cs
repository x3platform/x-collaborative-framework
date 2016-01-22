namespace X3Platform.AttachmentStorage.Tests.APIs
{
    using NUnit.Framework;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using X3Platform.Ajax.Net;
    using X3Platform.Json;
    using X3Platform.Apps;

    /// <summary></summary>
    [TestFixture]
    public class AttachmentFileAPITests
    {
        /// <summary>测试应用标识</summary>
        private string appKey = ConfigurationManager.AppSettings["appKey"];
        /// <summary>测试应用密钥</summary>
        private string appSecret = ConfigurationManager.AppSettings["appSecret"];
        /// <summary>测试API宿主服务器前缀</summary>
        private string apiHostPrefix = ConfigurationManager.AppSettings["apiHostPrefix"];

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Category("Manual Testing")]
        [Category("APIs")]
        [Test]
        public void TestFindAll()
        {
            // 测试帐号id
            // string accountId = "52cf89ba-7db5-4e64-9c64-3c868b6e7a99";

            AjaxRequestData reqeustData = new AjaxRequestData();

            reqeustData.ActionUri = new Uri(apiHostPrefix + "/api/attachment.file.findAll.aspx");

            dynamic token = JsonMapper.ToDynamicObject(SecurityTokenManager.CreateSecurityToken(appKey));

            reqeustData.Args.Add("clientId", token.clientId);
            reqeustData.Args.Add("clientSignature", token.clientSignature);
            reqeustData.Args.Add("timestamp", token.timestamp);
            reqeustData.Args.Add("nonce", token.nonce);

            reqeustData.Args.Add("entityId", "test_1418141958");
            reqeustData.Args.Add("entityClassName", "X3Platform.AttachmentStorage.AttachmentParentObject, X3Platform.AttachmentStorage");

            var responseText = AjaxRequest.Request(reqeustData);

            dynamic response = JsonMapper.ToDynamicObject(responseText);

            Assert.AreEqual(0, response.message.returnCode);
        }

        [Category("Manual Testing")]
        [Category("APIs")]
        [Test]
        public void TestUpload()
        {
            // 测试帐号id
            // string accountId = "52cf89ba-7db5-4e64-9c64-3c868b6e7a99";

            AjaxRequestData reqeustData = new AjaxRequestData();

            reqeustData.ActionUri = new Uri(apiHostPrefix + "/api/attachment.upload.aspx");

            var content = ResourceStringLoader.LoadString("X3Platform.AttachmentStorage.Tests.data.api.attachment.upload.json");

            var responseText = AjaxRequest.Request(reqeustData, "POST", "application/x-www-form-urlencoded", content);

            dynamic response = JsonMapper.ToDynamicObject(responseText);

            Assert.AreEqual("0", response["mobileRespHeader"]["respCode"].ToString());
        }
    }
}
