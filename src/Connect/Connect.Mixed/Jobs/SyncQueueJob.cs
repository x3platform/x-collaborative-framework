namespace X3Platform.Connect.Jobs
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using Common.Logging;
    using Quartz;
    using RabbitMQ.Client;
    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Json;
    using X3Platform.Messages;
    using X3Platform.Util;

    /// <summary>同步队列作业</summary>
    public class SyncQueueJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            logger.Info("Fetching...");

            IMessageQueueObject queue = new RabbitQueueObject<ConnectCallInfo>(ConnectConfigurationView.Instance.MessageQueueHostName,
                ConnectConfigurationView.Instance.MessageQueuePort,
                ConnectConfigurationView.Instance.MessageQueueUsername,
                ConnectConfigurationView.Instance.MessageQueuePassword,
                ConnectConfigurationView.Instance.MessageQueueName);

            ConnectCallInfo param = (ConnectCallInfo)queue.Receive();

            if (param != null)
            {
                ConnectContext.Instance.ConnectCallService.Save(param);

                logger.Info("id:" + param.Id + " insert success.");
            }
        }
    }
}