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
    
    public sealed class EmailClientWrapper : ContextWrapper
    {
        #region ����:SetSmtpServer(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string SetSmtpServer(XmlDocument doc)
        {
            return "{\"message\":{\"returnCode\":0,\"value\":\"���óɹ���\"}}";
        }
        #endregion

        //#region ����:GetDefaultSmtpServer(XmlDocument doc)
        ///// <summary></summary>
        ///// <param name="doc">Xml �ĵ�����</param>
        ///// <returns>���ز������</returns>
        //public string GetDefaultSmtpServer(XmlDocument doc)
        //{
        //    StringBuilder outString = new StringBuilder();

        //    NameTypeConfigurationElementCollection<EmailClientProviderData, CustomEmailClientProviderData> list = EmailClientConfigurationView.Instance.Configuration.EmailClientProviders;

        //    string defaultProviderName = EmailClientConfigurationView.Instance.DefaultEmailClientProvider;

        //    outString.Append("{\"ajaxStorage\":");

        //    foreach (NameTypeConfigurationElement item in list)
        //    {
        //        if (defaultProviderName != item.Name || !(item is GeneralEmailClientProviderData))
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
        //        outString.Append("}");
        //    }

        //    outString.Append(",\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

        //    return outString.ToString();
        //}
        //#endregion

        //#region ����:FindSmtpServers(XmlDocument doc)
        ///// <summary></summary>
        ///// <param name="doc">Xml �ĵ�����</param>
        ///// <returns>���ز������</returns>
        //public string FindSmtpServers(XmlDocument doc)
        //{
        //    StringBuilder outString = new StringBuilder();

        //    NameTypeConfigurationElementCollection<EmailClientProviderData, CustomEmailClientProviderData> list = EmailClientConfigurationView.Instance.Configuration.EmailClientProviders;

        //    outString.Append("{\"ajaxStorage\":[");

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

        //    outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

        //    return outString.ToString();
        //}
        //#endregion

        #region ����:SendMail(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string SendMail(XmlDocument doc)
        {
            string from = AjaxStorageConvertor.Fetch("from", doc), to = AjaxStorageConvertor.Fetch("to", doc);

            string subject = AjaxStorageConvertor.Fetch("subject", doc), body = AjaxStorageConvertor.Fetch("body", doc);

            bool isBodyHtml = Convert.ToBoolean(AjaxStorageConvertor.Fetch("isBodyHtml", doc));

            MailMessage message = new MailMessage(from, to, subject, body);

            message.IsBodyHtml = isBodyHtml;

            EmailClientContext.Instance.Send(message);

            return "{\"message\":{\"returnCode\":0,\"value\":\"���ͳɹ���\"}}";
        }
        #endregion

        #region ����:TestSmtpServer(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string TestSmtpServer(XmlDocument doc)
        {
            string host = AjaxStorageConvertor.Fetch("host", doc);
            string port = AjaxStorageConvertor.Fetch("port", doc);
            string enableSsl = AjaxStorageConvertor.Fetch("enableSsl", doc);
            string username = AjaxStorageConvertor.Fetch("username", doc);
            string password = AjaxStorageConvertor.Fetch("password", doc);

            string fromEmailAddress = AjaxStorageConvertor.Fetch("fromEmailAddress", doc);
            string toEmailAddress = AjaxStorageConvertor.Fetch("toEmailAddress", doc);

            MessageObject message = SmtpClientTester.Test(host, port, enableSsl, username, password, fromEmailAddress, toEmailAddress);

            return "{\"message\":" + message + "}";
        }
        #endregion
    }
}
