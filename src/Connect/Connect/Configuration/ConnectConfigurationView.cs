namespace X3Platform.Connect.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>������ͼ</summary>
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

        #region ����:SessionTimeLimit
        private int m_SessionTimeLimit = 0;

        /// <summary>�Ựʱ������ (��λ:��)</summary>
        public int SessionTimeLimit
        {
            get
            {
                if (this.m_SessionTimeLimit == 0)
                {
                    // ��ȡ������Ϣ
                    this.m_SessionTimeLimit = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
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
    }
}
