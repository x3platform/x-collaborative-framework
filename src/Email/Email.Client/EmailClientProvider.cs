namespace X3Platform.Email.Client
{
    using System;
    using System.Net;
    using System.Net.Mail;

    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Email.Client.Configuration;

    public class EmailClientProvider : IEmailClientProvider
    {
        private SmtpClient smtp = null;

        private string defaultSenderEmailAddress = null;

        public EmailClientProvider()
        {
            EmailClientConfiguration configuration = EmailClientConfigurationView.Instance.Configuration;

            this.smtp = new SmtpClient(configuration.EmailSmtp.Host);
            // this.smtp.Host = configuration.EmailSmtp.Host;
            this.smtp.Port = configuration.EmailSmtp.Port;
            this.smtp.EnableSsl = configuration.EmailSmtp.EnableSsl;

            if (!string.IsNullOrEmpty(configuration.EmailSmtp.Username))
            {
                this.smtp.UseDefaultCredentials = configuration.EmailSmtp.UseDefaultCredentials;

                this.smtp.Credentials = new NetworkCredential(configuration.EmailSmtp.Username, configuration.EmailSmtp.Password);
            }

            // this.smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            this.defaultSenderEmailAddress = configuration.EmailSmtp.DefaultSenderEmailAddress;
        }

        /// <summary>Create a new <see cref="EmailClientProvider"/> instance.</summary>
        /// <param name="host">邮件内容</param>
        public EmailClientProvider(string host, string defaultSenderEmailAddress)
        {
            this.smtp = new SmtpClient(host);

            this.defaultSenderEmailAddress = defaultSenderEmailAddress;
        }

        /// <summary>Create a new <see cref="EmailClientProvider"/> instance.</summary>
        /// <param name="host">邮件内容</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public EmailClientProvider(string host, string username, string password, bool useDefaultCredentials, string defaultSenderEmailAddress)
            : this(host, defaultSenderEmailAddress)
        {
            if (!string.IsNullOrEmpty(username))
            {
                smtp.UseDefaultCredentials = useDefaultCredentials;

                smtp.Credentials = new NetworkCredential(username, password);
            }
        }

        /// <summary>Create a new <see cref="EmailClientProvider"/> instance.</summary>
        /// <param name="host">邮件服务器</param>
        /// <param name="enableSsl">支持SSL</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public EmailClientProvider(string host, string enableSsl, string username, string password, bool useDefaultCredentials, string defaultSenderEmailAddress)
            : this(host, username, password, useDefaultCredentials, defaultSenderEmailAddress)
        {
            if (enableSsl.ToUpper() == "ON" || enableSsl.ToUpper() == "TRUE")
                smtp.EnableSsl = true;
        }

        public void Send(System.Net.Mail.MailMessage message)
        {
            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                KernelContext.Log.Error(ex.Message, ex);
            }
        }

        public void Send(string toAddress, string subject, string content)
        {
            Send(defaultSenderEmailAddress, toAddress, subject, content, EmailFormat.Text);
        }

        public void Send(string fromAddress, string toAddress, string subject, string content)
        {
            Send(fromAddress, toAddress, subject, content, EmailFormat.Text);
        }

        public void Send(string toAddress, string subject, string content, EmailFormat format)
        {
            Send(defaultSenderEmailAddress, toAddress, subject, content, format);
        }

        public void Send(string fromAddress, string toAddress, string subject, string content, EmailFormat format)
        {
            MailMessage message = new MailMessage(fromAddress, toAddress);

            message.Subject = subject;

            message.Body = content;

            message.IsBodyHtml = (format == EmailFormat.Html) ? true : false;

            this.Send(message);
        }
    }
}
