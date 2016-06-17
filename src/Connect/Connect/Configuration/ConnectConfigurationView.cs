namespace X3Platform.Connect.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;
    using X3Platform.Configuration;
    using X3Platform.MessageQueue.Configuration;
    using X3Platform.Util;
    #endregion

    /// <summary>Ӧ�����ӹ����������ͼ</summary>
    public class ConnectConfigurationView : XmlConfigurationView<ConnectConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Connect.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = ConnectConfiguration.ApplicationName;

        #region ��̬����:Instance
        private static volatile ConnectConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static ConnectConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:ConnectConfigurationView()
        /// <summary>���캯��</summary>
        private ConnectConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region ����:Reload()
        /// <summary>���¼���������Ϣ</summary>
        public override void Reload()
        {
            base.Reload();

            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ�������
        // -------------------------------------------------------

        #region ����:ApiHostName
        private string m_ApiHostName = string.Empty;

        /// <summary>API������</summary>
        public string ApiHostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApiHostName))
                {
                    // ��ȡ������Ϣ
                    this.m_ApiHostName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "ApiHostName",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_ApiHostName = StringHelper.NullOrEmptyTo(this.m_ApiHostName, KernelConfigurationView.Instance.HostName);

                    this.m_ApiHostName = this.m_ApiHostName.ToLower();
                }

                return this.m_ApiHostName;
            }
        }
        #endregion

        #region ����:SessionTimerInterval
        private int m_SessionTimerInterval = -1;

        /// <summary>�Ự��ʱ��ִ��ʱ����(��λ:����)</summary>
        public int SessionTimerInterval
        {
            get
            {
                if (this.m_SessionTimerInterval == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_SessionTimerInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimerInterval",
                        this.Configuration.Keys));

                    if (this.m_SessionTimerInterval == -1)
                    {
                        // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                        this.m_SessionTimerInterval = 15;
                    }
                }

                return this.m_SessionTimerInterval;
            }
        }
        #endregion

        #region ����:SessionTimeLimit
        private double m_SessionTimeLimit = 0;

        /// <summary>�Ựʱ������ (��λ:��)</summary>
        public double SessionTimeLimit
        {
            get
            {
                if (this.m_SessionTimeLimit == 0)
                {
                    // ��ȡ������Ϣ
                    this.m_SessionTimeLimit = Convert.ToDouble(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimeLimit",
                        this.Configuration.Keys));

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    if (this.m_SessionTimeLimit == 0)
                    {
                        this.m_SessionTimeLimit = 86400;
                    }
                }

                return this.m_SessionTimeLimit;
            }
        }
        #endregion

        #region ����:TrackingCall
        private string m_TrackingCall = string.Empty;

        /// <summary>�������ӵ���</summary>
        public string TrackingCall
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_TrackingCall))
                {
                    // ��ȡ������Ϣ
                    this.m_TrackingCall = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "TrackingCall",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_TrackingCall = StringHelper.NullOrEmptyTo(this.m_TrackingCall, "NO");

                    this.m_TrackingCall = this.m_TrackingCall.ToUpper();
                }

                return this.m_TrackingCall;
            }
        }
        #endregion

        #region ����:MessageQueueMode
        private string m_MessageQueueMode = string.Empty;

        /// <summary>��Ϣ����ģʽ</summary>
        public string MessageQueueMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueMode))
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueueMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueMode",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_MessageQueueMode = StringHelper.NullOrEmptyTo(this.m_MessageQueueMode, "OFF");

                    this.m_MessageQueueMode = this.m_MessageQueueMode.ToUpper();
                }

                return this.m_MessageQueueMode;
            }
        }
        #endregion

        #region ����:MessageQueueHostName
        private string m_MessageQueueHostName = string.Empty;

        /// <summary>��Ϣ���л�������</summary>
        public string MessageQueueHostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueHostName))
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueueHostName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueHostName",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_MessageQueueHostName = StringHelper.NullOrEmptyTo(this.m_MessageQueueHostName, MessageQueueConfigurationView.Instance.HostName);
                }

                return this.m_MessageQueueHostName;
            }
        }
        #endregion

        #region ����:MessageQueuePort
        private int m_MessageQueuePort = 0;

        /// <summary>��Ϣ���ж˿�</summary>
        public int MessageQueuePort
        {
            get
            {
                if (this.m_MessageQueuePort == 0)
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueuePort = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueuePort",
                        this.Configuration.Keys));

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (this.m_MessageQueuePort == 0)
                    {
                        this.m_MessageQueuePort = MessageQueueConfigurationView.Instance.Port;
                    }
                }

                return this.m_MessageQueuePort;
            }
        }
        #endregion

        #region ����:MessageQueueUsername
        private string m_MessageQueueUsername = string.Empty;

        /// <summary>��Ϣ��������</summary>
        public string MessageQueueUsername
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueUsername))
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueueUsername = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueUsername",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_MessageQueueUsername = StringHelper.NullOrEmptyTo(this.m_MessageQueueUsername, MessageQueueConfigurationView.Instance.Username);
                }

                return this.m_MessageQueueUsername;
            }
        }
        #endregion

        #region ����:MessageQueuePassword
        private string m_MessageQueuePassword = string.Empty;

        /// <summary>��Ϣ��������</summary>
        public string MessageQueuePassword
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueuePassword))
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueuePassword = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueuePassword",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_MessageQueuePassword = StringHelper.NullOrEmptyTo(this.m_MessageQueuePassword, MessageQueueConfigurationView.Instance.Password);
                }

                return this.m_MessageQueuePassword;
            }
        }
        #endregion

        #region ����:MessageQueueName
        private string m_MessageQueueName = string.Empty;

        /// <summary>��Ϣ��������</summary>
        public string MessageQueueName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueName))
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueueName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueName",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_MessageQueueName = StringHelper.NullOrEmptyTo(this.m_MessageQueueName, "connect-call-queue");
                }

                return this.m_MessageQueueName;
            }
        }
        #endregion

        #region ����:MessageQueueReceivingInterval
        private int m_MessageQueueReceivingInterval = 0;

        /// <summary>��Ϣ���н���ʱ����(��λ:��)</summary>
        public int MessageQueueReceivingInterval
        {
            get
            {
                if (this.m_MessageQueueReceivingInterval == 0)
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueueReceivingInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueReceivingInterval",
                        this.Configuration.Keys));

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (this.m_MessageQueueReceivingInterval == 0)
                    {
                        this.m_MessageQueueReceivingInterval = 120;
                    }
                }

                return this.m_MessageQueueReceivingInterval;
            }
        }
        #endregion
    }
}
