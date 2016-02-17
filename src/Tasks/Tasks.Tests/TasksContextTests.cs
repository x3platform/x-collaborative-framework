namespace X3Platform.Tasks.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using NUnit.Framework;
    using X3Platform.Tasks.Model;
    using X3Platform.Util;
    using X3Platform.Tasks.Configuration;
    using System.Configuration;

    /// <summary>任务服务测试说明</summary>
    [TestFixture]
    public class TasksContextTests
    {
        /// <summary></summary>
        [Test]
        public void TestLoad()
        {
            Assert.IsNotNull(TasksContext.Instance.TaskWorkService);
            Assert.IsNotNull(TasksContext.Instance.TaskWorkReceiverService);
            Assert.IsNotNull(TasksContext.Instance.TaskWaitingService);
            Assert.IsNotNull(TasksContext.Instance.TaskWorkReceiverService);
        }
    }
}
