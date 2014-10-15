using System;
using System.IO;
using System.Xml;

using X3Platform.Configuration;

namespace X3Platform.Services.Configuration
{
    /// <summary>������ͼ</summary>
    public class ServicesConfigurationView
    {
        /// <summary>�����ļ���Ĭ��·��.</summary>
        private const string configFile = "config\\X3Platform.Services.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = "Services";

        #region ��̬����::Instance
        private static volatile ServicesConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static ServicesConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ServicesConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ����:ConfigFilePath
        private string configFilePath = null;

        /// <summary>�����ļ�������·��.</summary>
        public string ConfigFilePath
        {
            get { return configFile; }
        }
        #endregion

        /// <summary>�����ļ�����޸ĵ�ʱ��</summary>
        private DateTime lastModifiedTime;

        ///// <summary>�����ļ��ļ�����.</summary>
        //private ConfigurationFileWatcher watcher = null;

        //private IConfigurationSource configurationSource;

        #region ����:Configuration
        private ServicesConfiguration configuration = null;

        /// <summary>������Ϣ</summary>
        public ServicesConfiguration Configuration
        {
            get { return this.configuration; }
        }
        #endregion

        #region ���캯��:ServicesConfigurationView()
        /// <summary>���캯��</summary>
        private ServicesConfigurationView()
            : this(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion

        #region ���캯��:ServicesConfigurationView(string path)
        /// <summary>���캯��</summary>
        public ServicesConfigurationView(string path)
        {
            this.configFilePath = path;

            Load(path);

            //FileSystemEventHandler handler = new FileSystemEventHandler(OnChanged);

            //watcher = new ConfigurationFileWatcher(path, handler);
        }
        #endregion

        #region �¼�:OnChanged(object sender, FileSystemEventArgs e)
        /// <summary>�����ļ�����ʱ�������¼�</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>��������ļ���������</remarks>
        protected void OnChanged(object sender, FileSystemEventArgs e)
        {
            // ��������������Ϣ.
            ServicesConfigurationView.Instance.Reload();

            // ��¼�����ļ�����ʱ��
            this.lastModifiedTime = File.GetLastWriteTime(configFilePath);
        }
        #endregion

        #region ����:Reload()
        /// <summary>���¼���������Ϣ</summary>
        public void Reload()
        {
            Load(this.configFilePath);

            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region ����:Load(string path)
        /// <summary>����������Ϣ</summary>
        private void Load(string path)
        {
            if (File.Exists(path))
            {
                //FileConfigurationSource configurationSource = new FileConfigurationSource(path);

                //this.configurationSource = configurationSource;
                ServicesConfiguration configuration = new ServicesConfiguration();

                using (XmlTextReader reader = new XmlTextReader(this.ConfigFilePath))
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(reader);

                    // ���ؼ���:Keys
                    XmlNodeList nodes = doc.SelectNodes(string.Format(@"configuration/{0}/keys/add", ServicesConfiguration.SectionName));

                    foreach (XmlNode node in nodes)
                    {
                        configuration.Keys.Add(new NameValueConfigurationElement(node.Attributes["name"].Value, node.Attributes["value"].Value));
                    }

                    // ���ؼ���:SpecialWords
                    nodes = doc.SelectNodes(string.Format(@"configuration/{0}/services/add", ServicesConfiguration.SectionName));

                    foreach (XmlNode node in nodes)
                    {
                        configuration.Services.Add(new NameTypeConfigurationElement(node.Attributes["name"].Value, node.Attributes["value"].Value));
                    }

                    reader.Close();
                }

                this.configuration = configuration;
            }
        }
        #endregion

        #region ����:LoadInstance(string fullConfigPath)
        /// <summary>ͨ�������ļ�����ʵ��</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new ServicesConfigurationView(fullConfigPath);
        }
        #endregion

        #region ����:Save()
        /// <summary>����������Ϣ</summary>
        public void Save()
        {
            //if (this.configurationSource != null)
            //{
            //    this.configurationSource.Add(new FileConfigurationParameter(this.configFilePath), ServicesConfiguration.SectionName, this.Configuration.Clone());
            //}
        }
        #endregion

        //-------------------------------------------------------
        // �Զ�������
        //-------------------------------------------------------

        #region ����:ServiceName
        private string m_ServiceName = string.Empty;

        /// <summary>ģ����֤������</summary>
        public string ServiceName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceName))
                {
                    // ��������
                    string propertyName = "ServiceName";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_ServiceName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_ServiceName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_ServiceName))
                    {
                        m_ServiceName = "Elane X Unkown Service";
                    }
                }

                return m_ServiceName;
            }
        }
        #endregion

        #region ����:ServiceDisplayName
        private string m_ServiceDisplayName = string.Empty;

        /// <summary>�������ʾ����</summary>
        public string ServiceDisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceDisplayName))
                {
                    // ��������
                    string propertyName = "ServiceDisplayName";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_ServiceDisplayName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_ServiceDisplayName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_ServiceDisplayName))
                    {
                        m_ServiceDisplayName = this.ServiceName;
                    }
                }

                return m_ServiceDisplayName;
            }
        }
        #endregion

        #region ����:ServiceDescription
        private string m_ServiceDescription = string.Empty;

        /// <summary>�����������Ϣ</summary>
        public string ServiceDescription
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceDescription))
                {
                    // ��������
                    string propertyName = "ServiceDescription";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_ServiceDescription = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_ServiceDescription = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_ServiceDescription))
                    {
                        m_ServiceDescription = string.Format("{0}δ��д�κ�������Ϣ", this.ServiceName);
                    }
                }

                return m_ServiceDescription;
            }
        }
        #endregion

        #region ����:ServiceLoginName
        private string m_ServiceLoginName = string.Empty;

        /// <summary>�����¼��ʹ�õ��ʺ�</summary>
        public string ServiceLoginName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceLoginName))
                {
                    // ��������
                    string propertyName = "ServiceLoginName";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_ServiceLoginName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_ServiceLoginName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_ServiceLoginName))
                    {
                        m_ServiceLoginName = "administrator";
                    }
                }

                return m_ServiceLoginName;
            }
        }
        #endregion

        #region ����:ServicePassword
        private string m_ServicePassword = string.Empty;

        /// <summary>�����¼��ʹ�õ�����</summary>
        public string ServicePassword
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServicePassword))
                {
                    // ��������
                    string propertyName = "ServicePassword";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_ServicePassword = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_ServicePassword = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_ServicePassword))
                    {
                        m_ServicePassword = "111111";
                    }
                }

                return m_ServicePassword;
            }
        }
        #endregion

        #region ����:TimerInterval
        private int m_TimerInterval = 0;

        /// <summary>����ʱ����Ĭ��ʱ����</summary>
        public int TimerInterval
        {
            get
            {
                if (m_TimerInterval == 0)
                {
                    try
                    {
                        // ��������
                        string propertyName = "TimerInterval";
                        // ����ȫ������
                        string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                        if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                        {
                            m_TimerInterval = Convert.ToInt32(KernelConfigurationView.Instance.ReplaceKeyValue(
                                      KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value));
                        }
                        else if (this.Configuration.Keys[propertyName] != null)
                        {
                            m_TimerInterval = Convert.ToInt32(KernelConfigurationView.Instance.ReplaceKeyValue(
                               this.Configuration.Keys[propertyName].Value));
                        }

                        // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                        if (m_TimerInterval == 0)
                        {
                            m_TimerInterval = 300;
                        }
                    }
                    catch
                    {
                        m_TimerInterval = 300;
                    }
                }

                return m_TimerInterval;
            }
        }
        #endregion

        #region ����:TrackRunTime
        private string m_TrackRunTime = string.Empty;

        /// <summary>��������ʱ��Ϣ</summary>
        public bool TrackRunTime
        {
            get
            {
                if (string.IsNullOrEmpty(m_TrackRunTime))
                {
                    // ��������
                    string propertyName = "TrackRunTime";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_TrackRunTime = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_TrackRunTime = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_TrackRunTime))
                    {
                        m_TrackRunTime = "OFF";
                    }
                }

                return m_TrackRunTime.ToUpper() != "ON" ? true : false;
            }
        }
        #endregion

        #region ����:TcpPort
        private int m_TcpPort = 0;

        /// <summary>Tcp�˿�</summary>
        public int TcpPort
        {
            get
            {
                if (string.IsNullOrEmpty(m_TrackRunTime))
                {
                    // ��������
                    string propertyName = "TcpPort";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_TcpPort = Convert.ToInt32(KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value));
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_TcpPort = Convert.ToInt32(KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value));
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (m_TcpPort == 0)
                    {
                        m_TcpPort = 12345;
                    }
                }

                return m_TcpPort;
            }
        }
        #endregion

    }
}
