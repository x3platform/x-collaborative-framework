
using System;
using System.Xml.Serialization;

namespace X3Platform.Email.Client.Configuration
{
    /// <summary>�ʼ� Smtp ������������Ϣ</summary>
    public class EmailSmtp
    {
        #region ����:Host
        private string m_Host;

        /// <summary>��������ַ</summary>
        public string Host
        {
            get { return this.m_Host; }
            set { this.m_Host = value; }
        }
        #endregion

        #region ����:Port
        private int m_Port;

        /// <summary>�˿�</summary>
        public int Port
        {
            get { return this.m_Port; }
            set { this.m_Port = value; }
        }
        #endregion

        #region ����:EnableSsl
        private bool m_EnableSsl;

        /// <summary>����SSL</summary>
        public bool EnableSsl
        {
            get { return this.m_EnableSsl; }
            set { this.m_EnableSsl = value; }
        }
        #endregion

        #region ����:UseDefaultCredentials
        private bool m_UseDefaultCredentials;

        /// <summary>���� DefaultCredentials �Ƿ�������һ����</summary>
        public bool UseDefaultCredentials
        {
            get { return this.m_UseDefaultCredentials; }
            set { this.m_UseDefaultCredentials = value; }
        }
        #endregion

        #region ����:Username
        private string m_Username;

        /// <summary>�û���</summary>
        public string Username
        {
            get { return this.m_Username; }
            set { this.m_Username = value; }
        }
        #endregion

        #region ����:Password
        private string m_Password;

        /// <summary>����</summary>
        public string Password
        {
            get { return this.m_Password; }
            set { this.m_Password = value; }
        }
        #endregion

        #region ����:DefaultSenderEmailAddress
        private string m_DefaultSenderEmailAddress;

        /// <summary>Ĭ�Ϸ����ʼ��������ַ</summary>
        public string DefaultSenderEmailAddress
        {
            get { return this.m_DefaultSenderEmailAddress; }
            set { this.m_DefaultSenderEmailAddress = value; }
        }
        #endregion
    }
}
