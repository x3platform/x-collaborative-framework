namespace X3Platform.Email.Client
{
    using System;
    using System.Net.Mail;

    using Common.Logging;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    /// <summary>�����ʼ��Ŀͻ��������Ļ���</summary>
    public class EmailClientContext : CustomPlugin
    {
        #region ����:Name
        public override string Name
        {
            get { return "�ʼ��ͻ���"; }
        }
        #endregion

        #region ����:Instance
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
                // �������ݷ������
                this.provider = SpringContext.Instance.GetObject<IEmailClientProvider>(typeof(IEmailClientProvider));
            }
            catch (Exception ex)
            {
                KernelContext.Log.Error(ex.Message, ex);
            }
        }

        /// <summary>�����ʼ�</summary>
        /// <param name="message"></param>
        public void Send(MailMessage message)
        {
            provider.Send(message);
        }

        public void Send(string toAddress, string subject, string content)
        {
            provider.Send(toAddress, subject, content);
        }
        /// <summary>�����ʼ�</summary>
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

        /// <summary>�����ʼ�</summary>
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
