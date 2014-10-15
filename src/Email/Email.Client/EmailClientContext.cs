using System;
using System.Net.Mail;

using Common.Logging;

using X3Platform.Plugins;
using X3Platform.Spring;

namespace X3Platform.Email.Client
{
    /// <summary>发送邮件的客户端上下文环境</summary>
    public class EmailClientContext : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 属性:Name
        public override string Name
        {
            get { return "邮件客户端"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile EmailClientContext instance = null;

        private static object lockObject = new object();

        public static EmailClientContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new EmailClientContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        private IEmailClientProvider provider = null;

        private EmailClientContext()
        {
            Reload();
        }

        public void Reload()
        {
            try
            {
                // 创建数据服务对象
                this.provider = SpringContext.Instance.GetObject<IEmailClientProvider>(typeof(IEmailClientProvider));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        /// <summary>发送邮件</summary>
        /// <param name="message"></param>
        public void Send(MailMessage message)
        {
            provider.Send(message);
        }

        public void Send(string toAddress, string subject, string content)
        {
            provider.Send(toAddress, subject, content);
        }
        /// <summary>发送邮件</summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        public void Send(string fromAddress, string toAddress, string subject, string content)
        {
            provider.Send(fromAddress, toAddress, subject, content);
        }

        public void Send(string toAddress, string subject, string content, EmailFormat format)
        {
            provider.Send(toAddress, subject, content, format);
        }

        /// <summary>发送邮件</summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="format"></param>
        public void Send(string fromAddress, string toAddress, string subject, string content, EmailFormat format)
        {
            provider.Send(fromAddress, toAddress, subject, content, format);
        }
    }
}
