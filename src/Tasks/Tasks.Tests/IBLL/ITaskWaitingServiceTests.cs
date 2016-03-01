namespace X3Platform.Tasks.Tests.IBLL
{
    using NUnit.Framework;
    
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;

    using X3Platform.Tasks.Model;
    using X3Platform.Util;
    
    /// <summary>定时任务服务测试说明</summary>
    [TestFixture]
    public class ITaskWaitingServiceTests
    {
        /// <summary>测试保存功能</summary>
        [Test]
        public void TestSave()
        {
            string applicationId = "00000000-0000-0000-0000-000000000001";

            string taskCode = StringHelper.ToGuid();

            string id = StringHelper.ToGuid();

            if (TasksContext.Instance.TaskWaitingService.IsExist(id))
            {
                throw new Exception("任务已存在, 测试失败.");
            }

            //
            // 新增定时任务
            //

            TaskWaitingInfo param = new TaskWaitingInfo();

            param.Id = id;

            param.ApplicationId = applicationId;
            param.TaskCode = taskCode;
            param.Title = string.Format("测试任务-编号{0}", taskCode);
            param.Content = DateTime.Now.ToString();
            param.Type = "1";
            param.Tags = "测试";
            param.SenderId = "00000000-0000-0000-0000-000000000001";

            param.AddReceiver("00000000-0000-0000-0000-000000100000");

            param.CreateDate = DateTime.Now;

            param.TriggerTime = DateTime.Now.AddDays(3);

            param = TasksContext.Instance.TaskWaitingService.Save(param);

            //
            // 修改任务
            //

            Assert.AreNotEqual(param.Content, "任务内容被修改.");

            param.Content = "任务内容被修改.";

            param = TasksContext.Instance.TaskWaitingService.Save(param);

            param = TasksContext.Instance.TaskWaitingService.FindOne(id);

            Assert.AreEqual(param.Content, "任务内容被修改.");

            //
            // 清理
            //
            if (TasksContext.Instance.TaskWaitingService.IsExist(id))
            {
                TasksContext.Instance.TaskWaitingService.Delete(id);
            }

            param = TasksContext.Instance.TaskWaitingService.FindOne(id);

            Assert.IsNull(param);
        }

        /// <summary>测试查找功能</summary>
        [Test]
        public void TestFind()
        {
            //IList<TaskWaitingItemInfo> list = TasksContext.Instance.TaskWaitingService.FindAll();

            //TaskWaitingItemInfo temp1, temp2;

            //foreach (TaskWaitingItemInfo item in list)
            //{
            //    temp1 = TasksContext.Instance.TaskWaitingService.FindOne(item.Id);
            //    temp2 = TasksContext.Instance.TaskWaitingService.FindOneByTaskCode(item.ApplicationId, item.TaskCode);

            //    Assert.AreEqual(item.Id, temp1.Id);
            //    Assert.AreEqual(item.Id, temp2.Id);

            //    Assert.AreSame(temp1, temp2);
            //}
        }

        /// <summary>测试发送任务</summary>
        [Test]
        public void TestSend()
        {
            string applicationId = "00000000-0000-0000-0000-000000000001";

            string taskCode = StringHelper.ToGuid();

            TasksContext.Instance.TaskWaitingService.Send(applicationId, taskCode, "4", string.Format("测试任务-编号{0}", taskCode), DateTime.Now.ToString(), "测试", "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000100000", DateTime.Now.AddDays(3));

            TaskWaitingInfo task = TasksContext.Instance.TaskWaitingService.FindOneByTaskCode(applicationId, taskCode);

            Assert.IsNotNull(task);

            if (TasksContext.Instance.TaskWaitingService.IsExistTaskCode(applicationId, taskCode))
            {
                TasksContext.Instance.TaskWaitingService.DeleteByTaskCode(applicationId, taskCode);
            }
        }
    }
}
