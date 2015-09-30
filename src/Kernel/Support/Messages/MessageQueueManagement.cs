namespace X3Platform.Messages
{
    #region Using Libraries
    using System.Linq;
    using System.Collections.Generic;

    using Common.Logging;
    #endregion

    /// <summary>消息队列管理器</summary>
    public static class MessageQueueManagement
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>日志记</summary>
        public static ILog Log
        {
            get { return logger; }
        }

        /// <summary>消息队列列表</summary>
        private static volatile IList<IMessageQueueObject> list = new List<IMessageQueueObject>();

        /// <summary>锁</summary>
        private static object lockObject = new object();

        /// <summary>设置消息队列</summary>
        public static void SetMessageQueueInstance(IMessageQueueObject queue)
        {
            list.Add(queue);
        }

        /// <summary>获取消息队列</summary>
        public static IMessageQueueObject GetMessageQueueInstance(string queueName)
        {
            return list.Where(item => item.QueueName == queueName).FirstOrDefault();
        }
    }
}
