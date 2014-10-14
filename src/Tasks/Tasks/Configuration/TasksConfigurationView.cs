namespace X3Platform.Tasks.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>����������ͼ</summary>
    public class TasksConfigurationView : XmlConfigurationView<TasksConfiguration>
    {
        private const string configFile = "config\\X3Platform.Tasks.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = TasksConfiguration.ApplicationName;

        #region ��̬����::Instance
        private static volatile TasksConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static TasksConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TasksConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:TasksConfigurationView()
        /// <summary>���캯��</summary>
        private TasksConfigurationView()
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

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_MessageQueueMode = StringHelper.NullOrEmptyTo(this.m_MessageQueueMode, "OFF");

                    this.m_MessageQueueMode = this.m_MessageQueueMode.ToUpper();
                }

                return this.m_MessageQueueMode;
            }
        }
        #endregion

        #region ����:MessageQueueMachineName
        private string m_MessageQueueMachineName = string.Empty;

        /// <summary>��Ϣ���л�������</summary>
        public string MessageQueueMachineName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueMachineName))
                {
                    // ��ȡ������Ϣ
                    this.m_MessageQueueMachineName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueMachineName",
                        this.Configuration.Keys);

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_MessageQueueMachineName = StringHelper.NullOrEmptyTo(this.m_MessageQueueMachineName, @".\private$");
                }

                return this.m_MessageQueueMachineName;
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

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_MessageQueueName = StringHelper.NullOrEmptyTo(this.m_MessageQueueName, "tasks$default$queue");
                }

                return this.m_MessageQueueName;
            }
        }
        #endregion

        #region ����:PrefixTargetUrl
        private string m_PrefixTargetUrl = string.Empty;

        /// <summary>������Ϣ�ĵ�ַǰ׺</summary>
        public string PrefixTargetUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PrefixTargetUrl))
                {
                    // ��ȡ������Ϣ
                    this.m_PrefixTargetUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PrefixTargetUrl",
                        this.Configuration.Keys);

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PrefixTargetUrl = StringHelper.NullOrEmptyTo(this.m_PrefixTargetUrl, KernelConfigurationView.Instance.HostName);
                }

                return this.m_PrefixTargetUrl;
            }
        }
        #endregion

        #region ����:ClientRefreshInterval
        private int m_ClientRefreshInterval = 0;

        /// <summary>�ͻ���ˢ�¼��</summary>
        public int ClientRefreshInterval
        {
            get
            {
                if (this.m_ClientRefreshInterval == 0)
                {
                    // ��ȡ������Ϣ
                    this.m_ClientRefreshInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "ClientRefreshInterval",
                        this.Configuration.Keys));

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (this.m_ClientRefreshInterval == 0)
                    {
                        this.m_ClientRefreshInterval = 10;
                    }
                }

                return this.m_ClientRefreshInterval;
            }
        }
        #endregion
    }
}
