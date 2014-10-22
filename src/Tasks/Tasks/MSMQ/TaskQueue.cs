namespace X3Platform.Tasks.MSMQ
{
    #region Using Libraries
    using System;
    using System.Messaging;
    using System.Xml;

    using X3Platform.Messages;
    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary>任务队列</summary>
    public class TaskQueue : MessageQueueObject
    {
        /// <summary>消息队列</summary>
        private MessageQueue queue = null;

        /// <summary>锁</summary>
        private object lockObject = new object();

        #region 构造函数:TaskQueue()
        /// <summary>任务队列</summary>
        public TaskQueue()
            : base(TasksConfigurationView.Instance.MessageQueueMachineName, TasksConfigurationView.Instance.MessageQueueName)
        {
        }
        #endregion

        #region 函数:Receive()
        /// <summary>接收数据</summary>
        /// <returns></returns>
        public override IMessageObject Receive()
        {
            // 阻塞方式
            IMessageObject data = null;

            lock (lockObject)
            {
                if (queue == null)
                {
                    this.queue = this.GetQueueInstance();
                }

                try
                {
                    data = new TaskInfo();

                    // 如果消息队列为空时, 将会导致无限期占用线程, 设置等待10秒钟Receive()函数无响应，则返回空值。
                    Message message = queue.Receive(new TimeSpan(0, 0, 10));

                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(message.Body.ToString());

                    data.Deserialize(doc.DocumentElement);

                    return data;
                }
                catch (MessageQueueException messageQueueException)
                {
                    if (messageQueueException.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                    {
                        // 等待10秒钟Receive()函数无响应，则返回空值。
                        return null;
                    }

                    throw;
                }
                catch
                {
                    this.Close();
                    throw;
                }
            }
        }
        #endregion
    }
}
