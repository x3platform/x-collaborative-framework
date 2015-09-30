namespace X3Platform.Messages
{
    #region Using Libraries
    using System;
    using System.Linq;
    using System.Messaging;
    using System.Threading;
    using System.Collections.Generic;
    #endregion

    /// <summary>消息队列对象接口</summary>
    public interface IMessageQueueObject
    {
        /// <summary>机器名称</summary>
        string MachineName { get; set; }

        /// <summary>队列名称</summary>
        string QueueName { get; set; }

        /// <summary>队列路径</summary>
        string QueuePath { get; }

        /// <summary>队列可用</summary>
        bool Enabled { get; }

        /// <summary>发送数据</summary>
        /// <param name="data">数据对象</param>
        void Send(IMessageObject data);

        /// <summary>接收数据</summary>
        IMessageObject Receive();
    }
}
