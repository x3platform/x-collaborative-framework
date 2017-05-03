namespace X3Platform.Configuration
{
    using System;
    using System.Configuration;
    using System.Xml;
    using System.IO;
    using System.Globalization;
    using System.Reflection;

    using Common.Logging;

    /// <summary>Xml配置信息</summary>
    public abstract class XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>获取配置区的名称</summary>
        public abstract string GetSectionName();

        #region 属性:Initialized
        private bool m_Initialized = false;

        /// <summary>是否成功初始化</summary>
        /// <remarks>根据内置的配置文件或者外部的配置文件成功初始化</remarks>
        public bool Initialized
        {
            set { this.m_Initialized = value; }
            get { return this.m_Initialized; }
        }
        #endregion

        #region 属性:Keys
        private NameValueConfigurationCollection m_Keys = new NameValueConfigurationCollection();

        /// <summary>键值配置信息</summary>
        public NameValueConfigurationCollection Keys
        {
            get { return this.m_Keys; }
        }
        #endregion

        #region 函数:Configure(XmlElement element)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="element">配置节点的Xml元素</param>
        public virtual void Configure(XmlElement element)
        {
            try
            {
                // 加载键值配置信息
                XmlConfiguratonOperator.SetKeyValues(this.m_Keys, element.SelectNodes(@"keys/add"));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion

        #region 函数:Configure(string configFilePath, string configNodeXPath)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="configFilePath">配置文件</param>
        /// <param name="configNodeXPath">配置节点的 XPath 表达式</param>
        public void Configure(string configFilePath, string configNodeXPath)
        {
            if (File.Exists(configFilePath))
            {
                // XmlDocument 的 Load 方法会锁定文件信息，所以使用 XmlReader 对象来释放资源。
                using (XmlReader reader = XmlReader.Create(configFilePath))
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(reader);

                    XmlNode node = doc.SelectSingleNode(configNodeXPath);

                    if (node != null && node.NodeType == XmlNodeType.Element)
                    {
                        this.Configure((XmlElement)node);
                    }
                }
            }
        }
        #endregion
    }
}
