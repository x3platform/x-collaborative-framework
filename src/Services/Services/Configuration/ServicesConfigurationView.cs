using System;
using System.IO;
using System.Xml;

using X3Platform.Configuration;

namespace X3Platform.Services.Configuration
{
    /// <summary>配置视图</summary>
    public class ServicesConfigurationView
    {
        /// <summary>配置文件的默认路径.</summary>
        private const string configFile = "config\\X3Platform.Services.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = "Services";

        #region 静态属性::Instance
        private static volatile ServicesConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
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

        #region 属性:ConfigFilePath
        private string configFilePath = null;

        /// <summary>配置文件的完整路径.</summary>
        public string ConfigFilePath
        {
            get { return configFile; }
        }
        #endregion

        /// <summary>配置文件最后修改的时间</summary>
        private DateTime lastModifiedTime;

        ///// <summary>配置文件的监听器.</summary>
        //private ConfigurationFileWatcher watcher = null;

        //private IConfigurationSource configurationSource;

        #region 属性:Configuration
        private ServicesConfiguration configuration = null;

        /// <summary>配置信息</summary>
        public ServicesConfiguration Configuration
        {
            get { return this.configuration; }
        }
        #endregion

        #region 构造函数:ServicesConfigurationView()
        /// <summary>构造函数</summary>
        private ServicesConfigurationView()
            : this(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion

        #region 构造函数:ServicesConfigurationView(string path)
        /// <summary>构造函数</summary>
        public ServicesConfigurationView(string path)
        {
            this.configFilePath = path;

            Load(path);

            //FileSystemEventHandler handler = new FileSystemEventHandler(OnChanged);

            //watcher = new ConfigurationFileWatcher(path, handler);
        }
        #endregion

        #region 事件:OnChanged(object sender, FileSystemEventArgs e)
        /// <summary>配置文件更改时触发此事件</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>如果配置文件发生更改</remarks>
        protected void OnChanged(object sender, FileSystemEventArgs e)
        {
            // 重新载入配置信息.
            ServicesConfigurationView.Instance.Reload();

            // 记录配置文件更新时间
            this.lastModifiedTime = File.GetLastWriteTime(configFilePath);
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载配置信息</summary>
        public void Reload()
        {
            Load(this.configFilePath);

            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region 函数:Load(string path)
        /// <summary>加载配置信息</summary>
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

                    // 加载集合:Keys
                    XmlNodeList nodes = doc.SelectNodes(string.Format(@"configuration/{0}/keys/add", ServicesConfiguration.SectionName));

                    foreach (XmlNode node in nodes)
                    {
                        configuration.Keys.Add(new NameValueConfigurationElement(node.Attributes["name"].Value, node.Attributes["value"].Value));
                    }

                    // 加载集合:SpecialWords
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

        #region 函数:LoadInstance(string fullConfigPath)
        /// <summary>通过配置文件载入实例</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new ServicesConfigurationView(fullConfigPath);
        }
        #endregion

        #region 函数:Save()
        /// <summary>保存配置信息</summary>
        public void Save()
        {
            //if (this.configurationSource != null)
            //{
            //    this.configurationSource.Add(new FileConfigurationParameter(this.configFilePath), ServicesConfiguration.SectionName, this.Configuration.Clone());
            //}
        }
        #endregion

        //-------------------------------------------------------
        // 自定义属性
        //-------------------------------------------------------

        #region 属性:ServiceName
        private string m_ServiceName = string.Empty;

        /// <summary>模拟验证的密码</summary>
        public string ServiceName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceName))
                {
                    // 属性名称
                    string propertyName = "ServiceName";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_ServiceName))
                    {
                        m_ServiceName = "Elane X Unkown Service";
                    }
                }

                return m_ServiceName;
            }
        }
        #endregion

        #region 属性:ServiceDisplayName
        private string m_ServiceDisplayName = string.Empty;

        /// <summary>服务的显示名称</summary>
        public string ServiceDisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceDisplayName))
                {
                    // 属性名称
                    string propertyName = "ServiceDisplayName";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_ServiceDisplayName))
                    {
                        m_ServiceDisplayName = this.ServiceName;
                    }
                }

                return m_ServiceDisplayName;
            }
        }
        #endregion

        #region 属性:ServiceDescription
        private string m_ServiceDescription = string.Empty;

        /// <summary>服务的描述信息</summary>
        public string ServiceDescription
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceDescription))
                {
                    // 属性名称
                    string propertyName = "ServiceDescription";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_ServiceDescription))
                    {
                        m_ServiceDescription = string.Format("{0}未填写任何描述信息", this.ServiceName);
                    }
                }

                return m_ServiceDescription;
            }
        }
        #endregion

        #region 属性:ServiceLoginName
        private string m_ServiceLoginName = string.Empty;

        /// <summary>服务登录所使用的帐号</summary>
        public string ServiceLoginName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServiceLoginName))
                {
                    // 属性名称
                    string propertyName = "ServiceLoginName";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_ServiceLoginName))
                    {
                        m_ServiceLoginName = "administrator";
                    }
                }

                return m_ServiceLoginName;
            }
        }
        #endregion

        #region 属性:ServicePassword
        private string m_ServicePassword = string.Empty;

        /// <summary>服务登录所使用的密码</summary>
        public string ServicePassword
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServicePassword))
                {
                    // 属性名称
                    string propertyName = "ServicePassword";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_ServicePassword))
                    {
                        m_ServicePassword = "111111";
                    }
                }

                return m_ServicePassword;
            }
        }
        #endregion

        #region 属性:TimerInterval
        private int m_TimerInterval = 0;

        /// <summary>服务定时器的默认时间间隔</summary>
        public int TimerInterval
        {
            get
            {
                if (m_TimerInterval == 0)
                {
                    try
                    {
                        // 属性名称
                        string propertyName = "TimerInterval";
                        // 属性全局名称
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

                        // 如果配置文件里没有设置，设置一个默认值。
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

        #region 属性:TrackRunTime
        private string m_TrackRunTime = string.Empty;

        /// <summary>跟踪运行时信息</summary>
        public bool TrackRunTime
        {
            get
            {
                if (string.IsNullOrEmpty(m_TrackRunTime))
                {
                    // 属性名称
                    string propertyName = "TrackRunTime";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_TrackRunTime))
                    {
                        m_TrackRunTime = "OFF";
                    }
                }

                return m_TrackRunTime.ToUpper() != "ON" ? true : false;
            }
        }
        #endregion

        #region 属性:TcpPort
        private int m_TcpPort = 0;

        /// <summary>Tcp端口</summary>
        public int TcpPort
        {
            get
            {
                if (string.IsNullOrEmpty(m_TrackRunTime))
                {
                    // 属性名称
                    string propertyName = "TcpPort";
                    // 属性全局名称
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

                    // 如果配置文件里没有设置，设置一个默认值。
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
