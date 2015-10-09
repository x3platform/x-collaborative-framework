namespace X3Platform.Messages
{
    #region Using Libraries
    using System;
    using System.Text;
    using System.Xml;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using X3Platform.MessageQueue.Configuration;
    #endregion

    /// <summary>Rabbit消息队列对象</summary>
    public class RabbitQueueObject<T> : IMessageQueueObject, IDisposable where T : IMessageObject, new()
    {
        private ConnectionFactory factory = null;
        private IConnection connection = null;
        private IModel channel = null;

        private object lockObject = new object();

        private RabbitQueueObject() { }

        /// <summary></summary>
        /// <param name="queueName"></param>
        public RabbitQueueObject(string queueName)
            : this(MessageQueueConfigurationView.Instance.HostName, MessageQueueConfigurationView.Instance.Port, MessageQueueConfigurationView.Instance.Username, MessageQueueConfigurationView.Instance.Password, queueName)
        {
        }

        /// <summary></summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="queueName"></param>
        public RabbitQueueObject(string hostName, int port, string username, string password, string queueName)
        {
            if (string.IsNullOrEmpty(hostName) || (string.IsNullOrEmpty(queueName)))
            {
                throw new ArgumentNullException();
            }

            this.m_MachineName = hostName;

            this.m_QueueName = queueName;

            this.factory = new ConnectionFactory();

            factory.HostName = hostName;
            factory.Port = port;
            factory.UserName = username;
            factory.Password = password;

            this.m_Enabled = true;
        }

        #region 属性:MachineName
        private string m_MachineName = @"localhost";

        /// <summary>机器名称</summary>
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
                    m_MachineName = this.factory.HostName = value;

                    InitializeQueue();
                }
            }
        }
        #endregion

        #region 属性:QueueName
        private string m_QueueName = string.Empty;

        /// <summary>队列名称</summary>
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
        /// <summary>队列路径</summary>
        public string QueuePath
        {
            get { return string.Format(@"{0}\{1}", this.MachineName, this.QueueName); }
        }
        #endregion

        #region 属性:Enabled
        private bool m_Enabled = false;

        /// <summary>队列可用</summary>
        public bool Enabled
        {
            get { return m_Enabled; }
        }
        #endregion

        /// <summary>初始化队列</summary>
        private void InitializeQueue()
        {
            lock (lockObject)
            {
                if (this.connection != null)
                {
                    try
                    {
                        this.connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageQueueManagement.Log.Error(ex.Message, ex);
                    }

                    this.connection = null;
                }

                try
                {
                    this.connection = this.factory.CreateConnection();

                    this.channel = this.connection.CreateModel();
                }
                catch (Exception ex)
                {
                    MessageQueueManagement.Log.Error(ex.Message, ex);
                }
            }
        }

        /// <summary>关闭</summary>
        public void Close()
        {
            if (this.channel != null)
            {
                try
                {
                    this.channel.Close();
                }
                catch (Exception ex)
                {
                    MessageQueueManagement.Log.Error(ex.Message, ex);
                }

                this.channel = null;
            }


            if (this.connection != null)
            {
                try
                {
                    this.connection.Close();
                }
                catch (Exception ex)
                {
                    MessageQueueManagement.Log.Error(ex.Message, ex);
                }

                this.connection = null;
            }
        }

        /// <summary>发送数据</summary>
        /// <param name="data"></param>
        public virtual void Send(IMessageObject data)
        {
            if (data == null) { return; }

            lock (lockObject)
            {
                if (this.channel == null)
                {
                    InitializeQueue();
                }

                try
                {
                    // 在 MQ 上定义一个持久化队列，如果名称相同不会重复创建
                    channel.QueueDeclare(this.QueueName, true, false, false, null);

                    byte[] bytes = Encoding.UTF8.GetBytes(data.Serializable());

                    // 设置消息持久化
                    IBasicProperties properties = channel.CreateBasicProperties();

                    properties.DeliveryMode = 2;

                    channel.BasicPublish(string.Empty, this.QueueName, properties, bytes);
                }
                catch (Exception ex)
                {
                    MessageQueueManagement.Log.Error(ex.Message, ex);
                }
            }
        }

        /// <summary>接收数据</summary>
        /// <returns></returns>
        public virtual IMessageObject Receive()
        {
            // 阻塞方式
            IMessageObject data = null;

            lock (lockObject)
            {
                if (this.channel == null)
                {
                    InitializeQueue();
                }

                try
                {
                    data = new T();

                    //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                    channel.QueueDeclare(this.QueueName, true, false, false, null);

                    // 输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                    channel.BasicQos(0, 1, false);

                    // 在队列上定义一个消费者
                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);

                    // 消费队列，并设置应答模式为程序主动应答
                    channel.BasicConsume(this.QueueName, false, consumer);

                    //阻塞函数，获取队列中的消息
                    BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                    byte[] bytes = ea.Body;

                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(Encoding.UTF8.GetString(bytes));

                    data.Deserialize(doc.DocumentElement);

                    //回复确认
                    channel.BasicAck(ea.DeliveryTag, false);

                    return data;
                }
                catch (Exception ex)
                {
                    MessageQueueManagement.Log.Error(ex.Message, ex);
                }
            }

            return null;
        }

        #region 函数:Dispose()
        public void Dispose()
        {
            // 关闭相关对象
            this.channel.Close();
            this.connection.Close();

            // 释放相关对象
            this.channel = null;
            this.connection = null;
            this.factory = null;
        }
        #endregion
    }
}
