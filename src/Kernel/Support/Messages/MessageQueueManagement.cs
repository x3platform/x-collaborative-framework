#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :MessageQueueManagement.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Messages
{
    #region Using Libraries
    using System.Linq;
    using System.Collections.Generic;
    #endregion

    /// <summary>消息队列管理器</summary>
    public static class MessageQueueManagement
    {
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
