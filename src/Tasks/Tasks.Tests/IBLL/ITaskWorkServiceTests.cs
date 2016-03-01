namespace X3Platform.Tasks.Tests.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using NUnit.Framework;

    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Tasks.Model;

    /// <summary>任务服务测试说明</summary>
    [TestFixture]
    public class ITaskWorkServiceTests
    {
        // 应用标识
        string appKey = ConfigurationManager.AppSettings["appKey"];
        // 应用密钥
        string appSecret = ConfigurationManager.AppSettings["appSecret"];
        // API宿主服务器前缀
        public string apiHostPrefix = ConfigurationManager.AppSettings["apiHostPrefix"];

        /// <summary>测试保存功能</summary>
        [Test]
        public void TestSave()
        {
            string taskCode = DigitalNumberContext.Generate("Key_Guid");

            string id = DigitalNumberContext.Generate("Key_Guid");

            if (TasksContext.Instance.TaskWorkService.IsExist(id))
            {
                throw new Exception("任务已存在, 测试失败.");
            }

            // -------------------------------------------------------
            // 新增任务
            // -------------------------------------------------------

            TaskWorkInfo param = new TaskWorkInfo();

            param.Id = id;

            param.ApplicationId = appKey;
            param.TaskCode = taskCode;
            param.Title = string.Format("测试任务 - 编号{0}", taskCode);
            param.Content = DateTime.Now.ToString();
            param.Type = "1";
            param.Tags = "测试";
            param.SenderId = "00000000-0000-0000-0000-000000000001";

            param.AddReceiver("00000000-0000-0000-0000-000000000001");
            param.AddReceiver("00000000-0000-0000-0000-000000000002");
            param.AddReceiver("00000000-0000-0000-0000-000000000003");
            param.AddReceiver("00000000-0000-0000-0000-000000000004");

            param.CreateDate = DateTime.Now;

            param = TasksContext.Instance.TaskWorkService.Save(param);

            // -------------------------------------------------------
            // 修改任务
            // -------------------------------------------------------

            Assert.AreNotEqual(param.Content, "http://localhost/api/test/");

            param.Content = "http://localhost/api/test/";

            param = TasksContext.Instance.TaskWorkService.Save(param);

            param = TasksContext.Instance.TaskWorkService.FindOne(id);

            Assert.AreEqual(param.Content, "http://localhost/api/test/");

            // -------------------------------------------------------
            // 清理
            // -------------------------------------------------------

            if (TasksContext.Instance.TaskWorkService.IsExist(id))
            {
                TasksContext.Instance.TaskWorkService.Delete(id);
            }

            param = TasksContext.Instance.TaskWorkService.FindOne(id);

            Assert.IsNull(param);
        }

        /// <summary>测试查找功能</summary>
        [Test]
        public void TestFind()
        {
            IList<TaskWorkItemInfo> list = TasksContext.Instance.TaskWorkService.FindAll();

            TaskWorkInfo temp1, temp2;

            foreach (TaskWorkItemInfo item in list)
            {
                temp1 = TasksContext.Instance.TaskWorkService.FindOne(item.Id);
                temp2 = TasksContext.Instance.TaskWorkService.FindOneByTaskCode(item.ApplicationId, item.TaskCode);

                Assert.AreEqual(item.Id, temp1.Id);
                Assert.AreEqual(item.Id, temp2.Id);

                Assert.AreSame(temp1, temp2);
            }
        }

        /// <summary>测试发送任务</summary>
        [Test]
        public void TestSend()
        {
            var applicationId = "00000000-0000-0000-0000-000000000001";

            var taskCode = DigitalNumberContext.Generate("Key_Guid");

            var notificationOptions = "";

            TasksContext.Instance.TaskWorkService.Send(applicationId, taskCode, "4", string.Format("test-send-id-{0}", taskCode), DateTime.Now.ToString(), "测试", "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002");

            TaskWorkInfo result = TasksContext.Instance.TaskWorkService.FindOneByTaskCode(applicationId, taskCode);

            Assert.IsNotNull(result);

            // 发送带提醒功能的
            taskCode = DigitalNumberContext.Generate("Key_Guid");

            TasksContext.Instance.TaskWorkService.Send(applicationId, taskCode, "4", string.Format("test-send-id-{0}", taskCode), DateTime.Now.ToString(), "测试", "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", notificationOptions);

            result = TasksContext.Instance.TaskWorkService.FindOneByTaskCode(applicationId, taskCode);

            Assert.IsNotNull(result);
        }

        /// <summary>测试批量发送任务</summary>
        [Test]
        public void TestSendRange()
        {
            string applicationId = "00000000-0000-0000-0000-000000000001";

            string taskCode = StringHelper.ToGuid();

            TasksContext.Instance.TaskWorkService.SendRange(applicationId, taskCode, "4", string.Format("test-send-range-id-{0}", taskCode), DateTime.Now.ToString(), "测试", "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000001,00000000-0000-0000-0000-000000000002,00000000-0000-0000-0000-000000000002,00000000-0000-0000-0000-000000000003");

            TaskWorkInfo result = TasksContext.Instance.TaskWorkService.FindOneByTaskCode(applicationId, taskCode);

            Assert.IsNotNull(result);
        }

        /// <summary>测试发送任务</summary>
        [Test]
        public void TestNotification()
        {
            string applicationId = "00000000-0000-0000-0000-000000000001";

            string taskCode = StringHelper.ToGuid();

            // 发送人标识
            string senderId = "00000000-0000-0000-0000-000000000001";

            // 接收人标识
            string receiverId = "00000000-0000-0000-0000-000000100000";

            TaskWorkInfo task = new TaskWorkInfo();

            task.Id = StringHelper.ToGuid();

            task.ApplicationId = applicationId;
            task.TaskCode = taskCode;
            task.Title = "测试通知";
            task.Content = "{Url}";
            task.Type = "4";
            task.Tags = "测试";
            task.SenderId = senderId;

            if (!string.IsNullOrEmpty(receiverId))
            {
                task.AddReceiver(receiverId);
            }

            task.CreateDate = DateTime.Now;

            // string notificationOptions = "{\"SMS\":{\"validationType\":\"用户注册\"},\"Getui\":{}}";
            string notificationOptions = "{\"Getui\":{}}";

            TasksContext.Instance.TaskWorkService.Notification(task, receiverId, notificationOptions);
        }

        /// <summary>测试设置任务结束</summary>
        [Test]
        public void TestSetFinished()
        {
            string applicationId = "00000000-0000-0000-0000-000000000001";

            string taskCode = StringHelper.ToGuid();

            // 发送人标识
            string senderId = "00000000-0000-0000-0000-000000000001";

            string receiverId = "00000000-0000-0000-0000-000000100000";

            DateTime sendTime = DateTime.Now.AddDays(-1);

            TasksContext.Instance.TaskWorkService.Send(applicationId, taskCode, "4", string.Format("test-set-finished-id-{0}", taskCode), DateTime.Now.ToString(), "测试", senderId, receiverId);

            TasksContext.Instance.TaskWorkService.SetFinished(applicationId, taskCode);

            TaskWorkInfo task = TasksContext.Instance.TaskWorkService.FindOneByTaskCode(applicationId, taskCode);

            Assert.IsNotNull(task);

            foreach (TaskWorkReceiverInfo receiver in task.ReceiverGroup)
            {
                Assert.AreEqual(receiver.Status, 1);
                Assert.IsTrue(receiver.FinishTime > sendTime);
            }

            if (TasksContext.Instance.TaskWorkService.IsExistTaskCode(applicationId, taskCode))
            {
                TasksContext.Instance.TaskWorkService.DeleteByTaskCode(applicationId, taskCode);
            }
        }

        /// <summary>测试获取任务标签列表</summary>
        [Test]
        public void TestGetTaskTags()
        {
            IList<string> list = null;

            // 获取全部标签
            list = TasksContext.Instance.TaskWorkService.GetTaskTags();
            Assert.IsTrue(list.Count > 0);

            // 获取关键字匹配的标签
            list = TasksContext.Instance.TaskWorkService.GetTaskTags("测试");
            Assert.IsTrue(list.Count > 0);
        }
    }
}
