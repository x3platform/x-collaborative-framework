namespace X3Platform.Email.Client.Ajax
{
    using System;
    using System.Text;
    using System.Web;
    using System.Net.Mail;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Configuration;
    using X3Platform.Messages;

    using X3Platform.Email.Client.Configuration;
    using X3Platform.Util;
    using X3Platform.Security.VerificationCode;
    
    public sealed class EmailClientWrapper : ContextWrapper
    {

        #region 函数:GetSmtpServer(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetSmtpServer(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            var emailSmtp = EmailClientConfigurationView.Instance.Configuration.EmailSmtp;

            outString.Append("{\"data\":");

            outString.Append("{");
            outString.Append("\"host\":\"" + emailSmtp.Host + "\",");
            outString.Append("\"port\":\"" + emailSmtp.Port + "\",");
            outString.Append("\"enableSsl\":\"" + emailSmtp.EnableSsl + "\",");
            outString.Append("\"smtpMode\":\"" + emailSmtp.UseDefaultCredentials + "\",");
            outString.Append("\"username\":\"" + emailSmtp.Username + "\",");
            outString.Append("\"password\":\"" + emailSmtp.Password + "\",");
            outString.Append("\"defaultSenderEmailAddress\":\"" + emailSmtp.DefaultSenderEmailAddress + "\" ");
            outString.Append("}");

            outString.Append(",\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SetSmtpServer(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SetSmtpServer(XmlDocument doc)
        {
            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        //#region 函数:FindSmtpServers(XmlDocument doc)
        ///// <summary></summary>
        ///// <param name="doc">Xml 文档对象</param>
        ///// <returns>返回操作结果</returns>
        //public string FindSmtpServers(XmlDocument doc)
        //{
        //    StringBuilder outString = new StringBuilder();

        //    NameTypeConfigurationElementCollection<EmailClientProviderData, CustomEmailClientProviderData> list = EmailClientConfigurationView.Instance.Configuration.EmailClientProviders;

        //    outString.Append("{\"data\":[");

        //    foreach (NameTypeConfigurationElement item in list)
        //    {
        //        if (!(item is GeneralEmailClientProviderData))
        //            continue;

        //        GeneralEmailClientProviderData server = (GeneralEmailClientProviderData)item;

        //        outString.Append("{");
        //        outString.Append("\"name\":\"" + server.Name + "\", ");
        //        outString.Append("\"defaultSenderEmailAddress\":\"" + server.DefaultSenderEmailAddress + "\",");
        //        outString.Append("\"host\":\"" + server.Defaulthost + "\",");
        //        outString.Append("\"port\":\"" + server.Defaultport + "\",");
        //        outString.Append("\"enableSsl\":\"" + server.DefaultenableSsl + "\",");
        //        outString.Append("\"username\":\"" + server.Username + "\",");
        //        outString.Append("\"password\":\"" + server.Password + "\",");
        //        outString.Append("\"smtpMode\":\"" + server.DefaultSmtpMode + "\" ");
        //        outString.Append("},");
        //    }

        //    if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
        //        outString = outString.Remove(outString.Length - 1, 1);

        //    outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

        //    return outString.ToString();
        //}
        //#endregion

        #region 函数:SendMail(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SendMail(XmlDocument doc)
        {
            string from = XmlHelper.Fetch("from", doc), to = XmlHelper.Fetch("to", doc);

            string subject = XmlHelper.Fetch("subject", doc), body = XmlHelper.Fetch("body", doc);

            bool isBodyHtml = Convert.ToBoolean(XmlHelper.Fetch("isBodyHtml", doc));

            MailMessage message = new MailMessage(from, to, subject, body);

            message.IsBodyHtml = isBodyHtml;

            EmailClientContext.Instance.Send(message);

            return "{\"message\":{\"returnCode\":0,\"value\":\"发送成功。\"}}";
        }
        #endregion

        #region 函数:TestSmtpServer(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string TestSmtpServer(XmlDocument doc)
        {
            string host = XmlHelper.Fetch("host", doc);
            string port = XmlHelper.Fetch("port", doc);
            string enableSsl = XmlHelper.Fetch("enableSsl", doc);
            string username = XmlHelper.Fetch("username", doc);
            string password = XmlHelper.Fetch("password", doc);

            string fromEmailAddress = XmlHelper.Fetch("fromEmailAddress", doc);
            string toEmailAddress = XmlHelper.Fetch("toEmailAddress", doc);

            MessageObject message = SmtpClientTester.Test(host, port, enableSsl, username, password, fromEmailAddress, toEmailAddress);

            return "{\"message\":" + message + "}";
        }
        #endregion
    }
}
