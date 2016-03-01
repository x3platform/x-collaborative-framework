namespace X3Platform.Tasks.Tests.APIs
{
    using System;
    using System.Configuration;

    using NUnit.Framework;

    using X3Platform.Ajax.Net;
    using X3Platform.DigitalNumber;

    /// <summary></summary>
    [TestFixture]
    public class TaskProxyTests
    {
        // 应用标识
        string appKey = ConfigurationManager.AppSettings["appKey"];
        // 应用密钥
        string appSecret = ConfigurationManager.AppSettings["appSecret"];
        // API宿主服务器前缀
        public string apiHostPrefix = ConfigurationManager.AppSettings["apiHostPrefix"];

        /// <summary>发送测试任务信息</summary>
        /// <returns></returns>
        private string SendTestTask()
        {
            Uri actionUri = new Uri(this.apiHostPrefix + "/api/task.send.aspx?client_id=" + appKey + "&client_secret=" + appSecret);

            string taskCode = DigitalNumberContext.Generate("Key_Guid");

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <applicationId>{0}</applicationId>
    <taskCode>{1}</taskCode>
    <title>测试待办</title>
    <content>http://www.google.com</content>
    <type>1</type>
    <tags>测试标签</tags>
    <senderId>00000000-0000-0000-0000-000000000001</senderId>
    <!-- 接收人范围 -->
    <receivers>
        <!-- 接收人信息 1 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test1</loginName>
            <!-- 是否已完成 -->
            <isFinished>0</isFinished>
            <!-- 完成时间 -->
            <finishTime >2000-01-01 00:00:00</finishTime>
        </receiver>
        <!-- 接收人信息 2 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test2</loginName>
            <!-- 是否已完成 -->
            <isFinished>0</isFinished>
            <!-- 完成时间 -->
            <finishTime >2000-01-01 00:00:00</finishTime>
        </receiver>
        <!-- 接收人信息 3 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test3</loginName>
            <!-- 是否已完成 -->
            <isFinished>0</isFinished>
            <!-- 完成时间 -->
            <finishTime >2000-01-01 00:00:00</finishTime>
        </receiver>
        <!-- 接收人信息 4 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test4</loginName>
            <!-- 是否已完成 -->
            <isFinished>0</isFinished>
            <!-- 完成时间 -->
            <finishTime >2000-01-01 00:00:00</finishTime>
        </receiver>
    </receivers>
    <!-- 创建时间 -->
    <createDate>{2}</createDate>
</request>
", appKey, taskCode, DateTime.Now);

            // 发送请求信息
            AjaxRequestData reqeustData = new AjaxRequestData();

            reqeustData.ActionUri = actionUri;
            reqeustData.Args.Add("xml", xml);

            string result = AjaxRequest.Request(reqeustData);

            if (result == "{\"message\":{\"returnCode\":0,\"value\":\"发送成功。\"}}")
            {
                return taskCode;
            }
            else
            {
                throw new Exception(result);
            }
        }

        /// <summary>删除测试任务信息</summary>
        private void DeleteTestTask(string taskCode)
        {
            Uri actionUri = new Uri(this.apiHostPrefix + "/api/task.delete.aspx?client_id=" + appKey + "&client_secret=" + appSecret);

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <applicationId>{0}</applicationId>
    <taskCode>{1}</taskCode>
</request>
", appKey, taskCode);

            // 发送请求信息
            AjaxRequestData reqeustData = new AjaxRequestData();

            reqeustData.ActionUri = actionUri;
            reqeustData.Args.Add("xml", xml);

            string result = AjaxRequest.Request(reqeustData);

            if (result != "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}")
            {
                throw new Exception(result);
            }
        }

        [Test]
        public void TestSend()
        {
            Uri actionUri = new Uri(this.apiHostPrefix + "/api/task.send.aspx?client_id=" + appKey + "&client_secret=" + appSecret);

            string taskCode = DigitalNumberContext.Generate("Key_Guid");

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <applicationId>{0}</applicationId>
    <taskCode>{1}</taskCode>
    <title>测试待办</title>
    <content>http://www.google.com</content>
    <type>1</type>
    <tags>测试标签</tags>
    <senderId>00000000-0000-0000-0000-000000000001</senderId>
    <!-- 接收人范围 -->
    <receivers>
        <!-- 接收人信息 1 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test1</loginName>
        </receiver>
        <!-- 接收人信息 2 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test2</loginName>
        </receiver>
        <!-- 接收人信息 3 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test3</loginName>
        </receiver>
        <!-- 接收人信息 4 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test4</loginName>
        </receiver>
    </receivers>
    <!-- 创建时间 -->
    <createDate>{2}</createDate>
</request>
", appKey, DigitalNumberContext.Generate("Key_Guid"), DateTime.Now);

            // 发送请求信息
            AjaxRequestData reqeustData = new AjaxRequestData();

            reqeustData.ActionUri = actionUri;
            reqeustData.Args.Add("xml", xml);

            string result = AjaxRequest.Request(reqeustData);
        }

        [Test]
        public void TestSetFinished()
        {
            Uri actionUri = new Uri(this.apiHostPrefix + "/api/task.setUsersFinished.aspx?client_id=" + appKey + "&client_secret=" + appSecret);

            string taskCode = this.SendTestTask();

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <applicationId>{0}</applicationId>
    <taskCode>{1}</taskCode>
</request>
", appKey, taskCode);

            try
            {
                // 发送请求信息
                AjaxRequestData reqeustData = new AjaxRequestData();

                reqeustData.ActionUri = actionUri;
                reqeustData.Args.Add("xml", xml);

                string result = AjaxRequest.Request(reqeustData);

                Assert.AreEqual(result, "{message:{\"returnCode\":0,\"value\":\"设置成功。\"}}");
            }
            catch
            {
                throw;
            }

            this.DeleteTestTask(taskCode);
        }

        /// <summary></summary>
        [Test]
        public void SetUsersFinished()
        {
            Uri actionUri = new Uri(this.apiHostPrefix + "/api/task.setUsersFinished.aspx?client_id=" + appKey + "&client_secret=" + appSecret);

            string taskCode = this.SendTestTask();

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <applicationId>{0}</applicationId>
    <taskCode>{1}</taskCode>
    <!-- 接收人范围 -->
    <receivers>
        <!-- 接收人信息 1 -->
        <receiver>
            <!-- 接收人登录名信息 -->
            <loginName>test4</loginName>
        </receiver>
    </receivers>
</request>
", appKey, taskCode);

            try
            {
                // 发送请求信息
                AjaxRequestData reqeustData = new AjaxRequestData();

                reqeustData.ActionUri = actionUri;
                reqeustData.Args.Add("xml", xml);

                string result = AjaxRequest.Request(reqeustData);

                Assert.AreEqual(result, "{message:{\"returnCode\":0,\"value\":\"设置成功。\"}}");
            }
            catch
            {
                throw;
            }

            this.DeleteTestTask(taskCode);
        }

        /// <summary></summary>
        [Test]
        public void TestDelete()
        {
            Uri actionUri = new Uri(this.apiHostPrefix + "/api/task.delete.aspx?client_id=" + appKey + "&client_secret=" + appSecret);

            // string taskCode = this.SendTestTask();
            string taskCode = "48bab272-65a9-4b7f-af6f-4f15561cc74f";

            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <applicationId>{0}</applicationId>
    <taskCode>{1}</taskCode>
</request>
", appKey, taskCode);

            // 发送请求信息
            AjaxRequestData reqeustData = new AjaxRequestData();

            reqeustData.ActionUri = actionUri;
            reqeustData.Args.Add("xml", xml);

            string result = AjaxRequest.Request(reqeustData);

            Assert.AreEqual(result, "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}");
        }
    }
}
