#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :TaskQueue.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

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

    /// <summary>��������</summary>
    public class TaskQueue : MessageQueueObject
    {
        /// <summary>��Ϣ����</summary>
        private MessageQueue queue = null;

        /// <summary>��</summary>
        private object lockObject = new object();
        
        #region ���캯��:TaskQueue()
        /// <summary>��������</summary>
        public TaskQueue()
            : base(TasksConfigurationView.Instance.MessageQueueMachineName, TasksConfigurationView.Instance.MessageQueueName)
        {
        }
        #endregion

        #region ����:Receive()
        /// <summary>��������</summary>
        /// <returns></returns>
        public override IMessageObject Receive()
        {
            // ������ʽ
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

                    Message message = queue.Receive(new TimeSpan(0, 0, 10)); // �ȴ�10��
                    
                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(message.Body.ToString());

                    data.Deserialize(doc.DocumentElement);

                    return data;
                }
                catch (Exception)
                {
                    this.Close();
                }
            }

            return null;
        }
        #endregion
    }
}
