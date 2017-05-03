namespace X3Platform.Connect.Jobs
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Xml;

    using Common.Logging;

    using Quartz;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Json;
    using X3Platform.Messages;
    using X3Platform.Util;

    /// <summary>同步队列作业</summary>
    public class SyncQueueJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private bool isSync = false;

        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            if (isSync)
            {
                return;
            }
            else
            {
                isSync = true;

                try
                {
                    logger.Info("Fetching...");

                    ConnectionFactory factory = new ConnectionFactory();

                    /*
                    IMessageQueueObject queue = new RabbitQueueObject<ConnectCallInfo>(ConnectConfigurationView.Instance.MessageQueueHostName,
                        ConnectConfigurationView.Instance.MessageQueuePort,
                        ConnectConfigurationView.Instance.MessageQueueUsername,
                        ConnectConfigurationView.Instance.MessageQueuePassword,
                        ConnectConfigurationView.Instance.MessageQueueName);
                    */

                    factory.HostName = ConnectConfigurationView.Instance.MessageQueueHostName;
                    factory.Port = ConnectConfigurationView.Instance.MessageQueuePort;
                    factory.UserName = ConnectConfigurationView.Instance.MessageQueueUsername;
                    factory.Password = ConnectConfigurationView.Instance.MessageQueuePassword;

                    using (IConnection connection = factory.CreateConnection())
                    {
                        using (IModel channel = connection.CreateModel())
                        {
                            ConnectCallInfo data = new ConnectCallInfo();

                            while (true)
                            {
                                BasicGetResult result = channel.BasicGet(ConnectConfigurationView.Instance.MessageQueueName, false);

                                if (result == null)
                                {
                                    break;
                                }
                                else
                                {
                                    bool t = result.Redelivered;

                                    byte[] bytes = result.Body;

                                    XmlDocument doc = new XmlDocument();

                                    doc.LoadXml(Encoding.UTF8.GetString(bytes));

                                    data.Deserialize(doc.DocumentElement);

                                    // 回复确认
                                    channel.BasicAck(result.DeliveryTag, false);

                                    // return data;
                                    if (data != null)
                                    {
                                        ConnectContext.Instance.ConnectCallService.Save(data);

                                        logger.Info("id:" + data.Id + " insert success.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Info(ex);
                }
                finally
                {
                    this.isSync = false;
                }
            }
        }
    }
}