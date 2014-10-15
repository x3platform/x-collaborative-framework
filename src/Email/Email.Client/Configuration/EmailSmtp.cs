
using System;
using System.Xml.Serialization;

namespace X3Platform.Email.Client.Configuration
{
    /// <summary>邮件 Smtp 服务器配置信息</summary>
    public class EmailSmtp
    {
        #region 属性:Host
        private string m_Host;

        /// <summary>服务器地址</summary>
        public string Host
        {
            get { return this.m_Host; }
            set { this.m_Host = value; }
        }
        #endregion

        #region 属性:Port
        private int m_Port;

        /// <summary>端口</summary>
        public int Port
        {
            get { return this.m_Port; }
            set { this.m_Port = value; }
        }
        #endregion

        #region 属性:EnableSsl
        private bool m_EnableSsl;

        /// <summary>启用SSL</summary>
        public bool EnableSsl
        {
            get { return this.m_EnableSsl; }
            set { this.m_EnableSsl = value; }
        }
        #endregion

        #region 属性:UseDefaultCredentials
        private bool m_UseDefaultCredentials;

        /// <summary>控制 DefaultCredentials 是否随请求一起发送</summary>
        public bool UseDefaultCredentials
        {
            get { return this.m_UseDefaultCredentials; }
            set { this.m_UseDefaultCredentials = value; }
        }
        #endregion

        #region 属性:Username
        private string m_Username;

        /// <summary>用户名</summary>
        public string Username
        {
            get { return this.m_Username; }
            set { this.m_Username = value; }
        }
        #endregion

        #region 属性:Password
        private string m_Password;

        /// <summary>密码</summary>
        public string Password
        {
            get { return this.m_Password; }
            set { this.m_Password = value; }
        }
        #endregion

        #region 属性:DefaultSenderEmailAddress
        private string m_DefaultSenderEmailAddress;

        /// <summary>默认发送邮件的邮箱地址</summary>
        public string DefaultSenderEmailAddress
        {
            get { return this.m_DefaultSenderEmailAddress; }
            set { this.m_DefaultSenderEmailAddress = value; }
        }
        #endregion
    }
}
