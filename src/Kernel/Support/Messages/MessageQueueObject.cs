#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :MessageQueueObject.cs
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Messaging;
    using System.Threading;
    using System.Xml;
    #endregion

    /// <summary>��Ϣ���ж���</summary>
    public class MessageQueueObject : IMessageQueueObject, IDisposable
    {
        private MessageQueue queue = null;

        private object lockObject = new object();

        private int maxWorkerThreadCount = 100;

        private WaitHandle[] waitHandles = null;

        private MessageQueueObject() { }

        public MessageQueueObject(string queueName)
            : this(@".\private$", queueName)
        {
        }

        public MessageQueueObject(string machineName, string queueName)
        {
            if (string.IsNullOrEmpty(machineName) || (string.IsNullOrEmpty(queueName)))
            {
                throw new ArgumentNullException();
            }

            this.m_MachineName = machineName;

            this.m_QueueName = queueName;

            this.m_Enabled = true;

            waitHandles = new WaitHandle[maxWorkerThreadCount];
        }

        #region 属性:MachineName
        private string m_MachineName = @".\private$";

        /// <summary>��������</summary>
        public string MachineName
        {
            get { return m_MachineName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Invalid value (must contain non-whitespace characters)");
                }

                lock (lockObject)
                {
                    m_MachineName = value;

                    InitializeQueue();
                }
            }
        }
        #endregion

        #region 属性:QueueName
        private string m_QueueName = string.Empty;

        /// <summary>��������</summary>
        public string QueueName
        {
            get { return m_QueueName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Invalid value (must contain non-whitespace characters)");
                }

                lock (lockObject)
                {
                    m_QueueName = value;

                    InitializeQueue();
                }
            }
        }
        #endregion

        #region 属性:QueuePath
        /// <summary>����·��</summary>
        public string QueuePath
        {
            get { return string.Format(@"{0}\{1}", this.MachineName, this.QueueName); }
        }
        #endregion

        #region 属性:Enabled
        private bool m_Enabled = false;

        /// <summary>���п���</summary>
        public bool Enabled
        {
            get { return m_Enabled; }
        }
        #endregion

        /// <summary>��ʼ������</summary>
        private void InitializeQueue()
        {
            lock (lockObject)
            {
                if (queue != null)
                {
                    try
                    {
                        queue.Close();
                    }
                    catch
                    {
                    }

                    queue = null;
                }

                try
                {
                    // ������Ϣ����
                    if (!MessageQueue.Exists(this.QueuePath))
                    {
                        MessageQueue.Create(this.QueuePath);
                    }
                }
                catch
                {
                }

                try
                {
                    queue = new MessageQueue(this.QueuePath);

                    queue.SetPermissions("everyone", MessageQueueAccessRights.FullControl);

                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                    // queue.Formatter = new BinaryMessageFormatter();
                }
                catch (Exception ex)
                {
                    try
                    {
                        queue.Close();
                    }
                    catch
                    {
                    }

                    queue = null;

                    throw new ApplicationException("���ܴ򿪶�����Ϣ ��ϸ��ַ:\"" + this.QueuePath + "\": " + ex.GetType().FullName + ": " + ex.Message);
                }
            }
        }

        /// <summary>��ȡ��Դ</summary>
        public MessageQueue GetQueueInstance()
        {
            InitializeQueue();

            return queue;
        }

        /// <summary>�ر�</summary>
        public void Close()
        {
            if (queue != null)
            {
                try
                {
                    queue.Close();
                }
                catch
                {
                }

                queue = null;
            }
        }

        /// <summary>��������</summary>
        /// <param name="data"></param>
        public virtual void Send(IMessageObject data)
        {
            if (data == null)
            {
                return;
            }

            lock (lockObject)
            {
                try
                {
                    if (queue == null)
                    {
                        this.queue = this.GetQueueInstance();
                    }

                    Message message = new Message();

                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                    message.Priority = MessagePriority.Normal; // ��Ϣ���ȼ�

                    message.Recoverable = true; // �ɻ��յ�

                    message.Body = data.Serializable();

                    queue.Send(message);
                }
                catch (Exception ex)
                {
                    this.Close();

                    throw ex;
                }
            }
        }

        /// <summary>��������</summary>
        /// <returns></returns>
        public virtual IMessageObject Receive()
        {
            // ������ʽ
            IMessageObject data = null;

            lock (lockObject)
            {
                if (queue == null)
                {
                    InitializeQueue();
                }

                try
                {
                    data = new MessageObject();

                    Message message = queue.Receive(new TimeSpan(0, 0, 10)); // �ȴ�10��

                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(message.Body.ToString());

                    data.Deserialize(doc.DocumentElement);

                    return data;
                }
                catch
                {
                    this.Close();
                }
            }

            return null;
        }

        /// <summary>��ʼ���������߳�</summary>
        public void StartListen()
        {
            this.queue.ReceiveCompleted += new ReceiveCompletedEventHandler(MessageQueueReceiveCompleted);

            // �첽��ʽ������
            for (int i = 0; i < maxWorkerThreadCount; i++)
            {
                // Begin asynchronous operations.
                waitHandles[i] = this.queue.BeginReceive().AsyncWaitHandle;
            }
        }

        /// <summary>ֹͣ���������߳�</summary>
        public void StopListen()
        {
            for (int i = 0; i < waitHandles.Length; i++)
            {
                try
                {
                    waitHandles[i].Close();
                }
                catch
                {
                    Console.WriteLine("---waitHandles[i].Close() Error!-----");
                }
            }

            try
            {
                // Specify to wait for all operations to return.
                WaitHandle.WaitAll(waitHandles, 1000, false);
            }
            catch
            {
                //���Դ���
            }
        }

        private void MessageQueueReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            // Connect to the queue.
            MessageQueue messageQueue = (MessageQueue)sender;

            // End the asynchronous Receive operation.
            Message message = messageQueue.EndReceive(e.AsyncResult);

            //Util.ProcessMo((IMessageObject)(m.Body));

            //if (Util.isRunning)
            //{
            //    // Restart the asynchronous Receive operation.
            //    messageQueue.BeginReceive();
            //}

            return;
        }

        #region 属性:Dispose()
        public void Dispose()
        {
            lock (lockObject)
            {
                this.Close();
            }
        }
        #endregion
    }
}
