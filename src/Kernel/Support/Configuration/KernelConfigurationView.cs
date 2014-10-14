// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :KernelConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Configuration
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Reflection;

    using X3Platform.Data;
    using X3Platform.Security;
    using X3Platform.Util;
    using X3Platform.Web;

    /// <summary>������ͼ</summary>
    public class KernelConfigurationView
    {
        #region ��̬����:Instance
        private static volatile KernelConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>
        /// ʵ��
        /// </summary>
        public static KernelConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new KernelConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ����:Configuration
        private KernelConfiguration configurationSource;

        /// <summary>����������Ϣ</summary>
        public KernelConfiguration Configuration
        {
            get { return this.configurationSource; }
        }
        #endregion

        #region ���캯��:KernelConfigurationView()
        /// <summary>���캯��</summary>
        public KernelConfigurationView()
        {
            this.LoadOptions();
        }
        #endregion

        private string m_KernelAssemblyName = null;

        private string GetKernelAssemblyName()
        {
            if (string.IsNullOrEmpty(this.m_KernelAssemblyName))
            {
                this.m_KernelAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            }

            return this.m_KernelAssemblyName;
        }

        #region ���캯��:LoadOptions()
        /// <summary>����ѡ��</summary>
        public void LoadOptions()
        {
            // ���������ļ���Ϣ
            this.configurationSource = (KernelConfiguration)ConfigurationManager.GetSection(KernelConfiguration.SectionName);

            if (this.configurationSource == null)
            {
                this.configurationSource = new KernelConfiguration();

                // ����Ĭ�������ļ�
                string configurationFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "config\\X3Platform.config";

                if (File.Exists(configurationFilePath))
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(configurationFilePath);

                    XmlNodeList nodes = doc.SelectNodes("configuration/" + KernelConfiguration.SectionName + "/key");

                    foreach (XmlNode node in nodes)
                    {
                        this.AddKeyValue(node.Attributes["name"].Value, node.Attributes["value"].Value, true);
                    }
                }
            }

            if (ConfigurationManager.ConnectionStrings[this.DataSourceName] != null)
            {
                try
                {
                    // �������ݿ��Զ���ѡ����Ϣ
                    GenericSqlCommand command = new GenericSqlCommand(this.DataSourceName);

                    DataTable table = command.ExecuteQueryForDataTable("SELECT Name, Value FROM tb_Application_Option WHERE Status = 1;");

                    foreach (DataRow row in table.Rows)
                    {
                        this.AddKeyValue(row[0].ToString(), row[1].ToString(), true);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region ����:Save()
        /// <summary>����������Ϣ</summary>
        public void Save()
        {
            string path = null;

            if (HttpContext.Current == null)
            {
                path = string.Format("{0}.config", Process.GetCurrentProcess().MainModule.FileName);
            }
            else
            {
                path = VirtualPathHelper.GetPhysicalPath("~/web.config");
            }

            this.Serialize(path, KernelConfiguration.SectionName, this);
        }
        #endregion

        #region ����:Serialize(string path, string sectionName, object value)
        /// <summary>���л�������Ϣ</summary>
        public void Serialize(string path, string sectionName, object value)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode node;

            doc.Load(path);

            node = doc.GetElementsByTagName(sectionName)[0];

            node.InnerXml = XmlHelper.ToXml(value);

            doc.Save(path);
        }
        #endregion

        #region ����:ReplaceKeyValue(string text)
        /// <summary>�滻����ռλ��Ϊʵ�ʵ�ֵ</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReplaceKeyValue(string text)
        {
            // -------------------------------------------------------
            // ռλ����ʽ, ${ApplicationPathRoot} | ${HostName}
            // -------------------------------------------------------

            if (text.IndexOf("${") > -1)
            {
                foreach (KernelConfigurationKey key in this.Configuration.Keys)
                {
                    text = text.Replace("${" + key.Name + "}", key.Value);
                }
            }

            return text;
        }
        #endregion

        #region ����:AddKeyValues(string configGlobalPrefix, NameValueConfigurationCollection list, bool forceUpdate)
        /// <summary></summary>
        /// <param name="configGlobalPrefix"></param>
        /// <param name="list"></param>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
        public void AddKeyValues(string configGlobalPrefix, NameValueConfigurationCollection list, bool forceUpdate)
        {
            foreach (NameValueConfigurationElement item in list)
            {
                AddKeyValue(string.Format("{0}.{1}", configGlobalPrefix, item.Name), item.Value, forceUpdate);
            }
        }
        #endregion

        #region ����:AddKeyValue(string name, string value)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void AddKeyValue(string name, string value)
        {
            AddKeyValue(name, value, false);
        }
        #endregion

        #region ����:AddKeyValue(string name, string value, bool forceUpdate)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="forceUpdate">ǿ�Ƹ���</param>
        /// <returns></returns>
        public void AddKeyValue(string name, string value, bool forceUpdate)
        {
            if (this.Configuration.Keys.ContainsKey(name) && forceUpdate)
            {
                this.Configuration.Keys[name] = new KernelConfigurationKey(name, value);
            }
            else
            {
                this.Configuration.Keys.Add(new KernelConfigurationKey(name, value));
            }
        }
        #endregion

        #region ����:GetKeyValue(string configGlobalPrefix, string propertyName, NameValueConfigurationCollection defaultOptions)
        /// <summary>��ȡ������Ϣ��ֵ</summary>
        /// <param name="configGlobalPrefix">������Ϣ��ȫ��ǰ׺</param>
        /// <param name="propertyName">������������</param>
        /// <param name="defaultOptions">Ĭ������ѡ��</param>
        /// <returns></returns>
        public string GetKeyValue(string configGlobalPrefix, string propertyName, NameValueConfigurationCollection defaultOptions)
        {
            string returnValue = null;

            // ����ȫ������
            string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

            if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
            {
                returnValue = KernelConfigurationView.Instance.ReplaceKeyValue(KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
            }
            else if (defaultOptions[propertyName] != null)
            {
                returnValue = KernelConfigurationView.Instance.ReplaceKeyValue(defaultOptions[propertyName].Value);
            }

            return returnValue;
        }
        #endregion

        // -------------------------------------------------------
        // �Զ�������
        // -------------------------------------------------------

        #region ����:SystemName
        private string m_SystemName = string.Empty;

        /// <summary>ϵͳ����</summary>
        public string SystemName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SystemName))
                {
                    if (this.Configuration.Keys["SystemName"] == null)
                    {
                        this.m_SystemName = "Unkown System";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("SystemName", this.m_SystemName));
                    }
                    else
                    {
                        this.m_SystemName = this.Configuration.Keys["SystemName"].Value;
                    }
                }

                return this.m_SystemName;
            }
        }
        #endregion

        #region ����:SystemStatus
        private string m_SystemStatus = string.Empty;

        /// <summary>ϵͳ״̬����ѡ��ֵ�� Development(����ģʽ) | Test(����ģʽ) | Production(����ģʽ)</summary>
        public string SystemStatus
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SystemStatus))
                {
                    if (this.Configuration.Keys["SystemStatus"] == null)
                    {
                        this.m_SystemStatus = "Debug";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("SystemStatus", this.m_SystemStatus));
                    }
                    else
                    {
                        this.m_SystemStatus = this.Configuration.Keys["SystemStatus"].Value;
                    }
                }

                return this.m_SystemStatus;
            }
        }
        #endregion

        #region ����:HostName
        private string m_HostName = string.Empty;

        /// <summary>Ӧ�÷���������</summary>
        public string HostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HostName))
                {
                    if (this.Configuration.Keys["HostName"] == null)
                    {
                        this.m_HostName = "localhost";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("HostName", this.m_HostName));
                    }
                    else
                    {
                        this.m_HostName = this.Configuration.Keys["HostName"].Value;
                    }
                }

                return this.m_HostName;
            }
        }
        #endregion

        #region ����:FileHostName
        private string m_FileHostName = string.Empty;

        /// <summary>�ļ�����������</summary>
        public string FileHostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_FileHostName))
                {
                    if (this.Configuration.Keys["FileHostName"] == null)
                    {
                        // �����ļ�������Ĭ�ϵ�ַ����������ַ��ͬ��
                        this.m_FileHostName = this.HostName;

                        this.Configuration.Keys.Add(new KernelConfigurationKey("FileHostName", this.m_FileHostName));
                    }
                    else
                    {
                        this.m_FileHostName = this.Configuration.Keys["FileHostName"].Value;
                    }
                }

                return this.m_FileHostName;
            }
        }
        #endregion

        #region ����:StaticFileHostName
        private string m_StaticFileHostName = string.Empty;

        /// <summary>��̬�ļ�����������</summary>
        public string StaticFileHostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_StaticFileHostName))
                {
                    if (this.Configuration.Keys["StaticFileHostName"] == null)
                    {
                        // ���þ�̬�ļ�������Ĭ�ϵ�ַ����������ַ��ͬ��
                        this.m_StaticFileHostName = this.HostName;

                        this.Configuration.Keys.Add(new KernelConfigurationKey("StaticFileHostName", this.m_StaticFileHostName));
                    }
                    else
                    {
                        this.m_StaticFileHostName = this.Configuration.Keys["StaticFileHostName"].Value;
                    }
                }

                return this.m_StaticFileHostName;
            }
        }
        #endregion

        #region ����:SafeRefererHosts
        private string m_SafeRefererHosts = string.Empty;

        /// <summary>��ȫ�ĵ����÷�����</summary>
        public string SafeRefererHosts
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SafeRefererHosts))
                {
                    if (this.Configuration.Keys["SafeRefererHosts"] == null)
                    {
                        this.m_SafeRefererHosts = string.Empty;

                        this.Configuration.Keys.Add(new KernelConfigurationKey("SafeRefererHosts", this.m_SafeRefererHosts));
                    }
                    else
                    {
                        this.m_SafeRefererHosts = this.Configuration.Keys["SafeRefererHosts"].Value;
                    }
                }

                return this.m_SafeRefererHosts;
            }
        }
        #endregion

        #region ����:Domain
        private string m_Domain = string.Empty;

        /// <summary>����Ϣ</summary>
        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Domain))
                {
                    if (this.Configuration.Keys["Domain"] == null)
                    {
                        if (HttpContext.Current == null
                            || HttpContext.Current.Request == null
                            || HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == null)
                        {
                            this.m_Domain = "localhost";
                        }
                        else
                        {
                            this.m_Domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                        }

                        this.Configuration.Keys.Add(new KernelConfigurationKey("Domain", this.m_Domain));
                    }
                    else
                    {
                        this.m_Domain = this.Configuration.Keys["Domain"].Value;
                    }
                }

                return this.m_Domain;
            }
        }
        #endregion

        #region ����:NetworkCredentialDomain
        private string m_NetworkCredentialDomain = string.Empty;

        /// <summary>������֤����������Ϣ</summary>
        public string NetworkCredentialDomain
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_NetworkCredentialDomain))
                {
                    if (this.Configuration.Keys["NetworkCredentialDomain"] == null)
                    {
                        this.m_NetworkCredentialDomain = "x3platform";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("NetworkCredentialDomain", this.m_NetworkCredentialDomain));
                    }
                    else
                    {
                        this.m_NetworkCredentialDomain = this.Configuration.Keys["NetworkCredentialDomain"].Value;
                    }
                }

                return this.m_NetworkCredentialDomain;
            }
        }
        #endregion

        #region ����:Version
        private string m_Version = string.Empty;

        /// <summary>�汾</summary>
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Version))
                {
                    if (this.Configuration.Keys["Version"] == null)
                    {
                        this.m_Version = "0.0.0.0";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("Version", this.m_Version));
                    }
                    else
                    {
                        this.m_Version = this.Configuration.Keys["Version"].Value;
                    }
                }

                return this.m_Version;
            }
        }
        #endregion

        #region ����:Author
        private string m_Author = string.Empty;

        /// <summary>����</summary>
        public string Author
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Author))
                {
                    if (this.Configuration.Keys["Author"] == null)
                    {
                        this.m_Author = this.WebmasterEmail;

                        this.Configuration.Keys.Add(new KernelConfigurationKey("Author", this.m_Author));
                    }
                    else
                    {
                        this.m_Author = this.Configuration.Keys["Author"].Value;
                    }
                }

                return this.m_Author;
            }
        }
        #endregion

        #region ����:WebmasterEmail
        private string m_WebmasterEmail = string.Empty;

        /// <summary>����Ա�ʼ���ַ</summary>
        public string WebmasterEmail
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_WebmasterEmail))
                {
                    if (this.Configuration.Keys["WebmasterEmail"] == null)
                    {
                        this.m_WebmasterEmail = "ruanyu@live.com";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("WebmasterEmail", this.m_WebmasterEmail));
                    }
                    else
                    {
                        this.m_WebmasterEmail = this.Configuration.Keys["WebmasterEmail"].Value;
                    }
                }

                return this.m_WebmasterEmail;
            }
        }
        #endregion

        #region ����:AuthenticationManagementType
        private string m_AuthenticationManagementType = string.Empty;

        /// <summary>��֤��������</summary>
        public string AuthenticationManagementType
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AuthenticationManagementType))
                {
                    if (this.Configuration.Keys["AuthenticationManagementType"] == null)
                    {
                        this.m_AuthenticationManagementType = this.GetKernelAssemblyName() + ".Membership.Authentication.HttpAuthenticationManagement," + this.GetKernelAssemblyName() + ".Membership";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("AuthenticationManagementType", this.m_AuthenticationManagementType));
                    }
                    else
                    {
                        this.m_AuthenticationManagementType = this.Configuration.Keys["AuthenticationManagementType"].Value;
                    }
                }

                return this.m_AuthenticationManagementType;
            }
        }
        #endregion

        #region ����:ApplicationPathRoot
        private string m_ApplicationPathRoot = string.Empty;

        /// <summary>����·������ַ</summary>
        public string ApplicationPathRoot
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationPathRoot))
                {
                    // System.Windows.Forms.Application.StartupPath 
                    // System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase
                    if (this.Configuration.Keys["ApplicationPathRoot"] == null)
                    {
                        this.m_ApplicationPathRoot = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationPathRoot", this.m_ApplicationPathRoot));
                    }
                    else
                    {
                        this.m_ApplicationPathRoot = this.Configuration.Keys["ApplicationPathRoot"].Value;
                    }
                }

                return this.m_ApplicationPathRoot;
            }
        }
        #endregion

        #region ����:ApplicationTempPathRoot
        private string m_ApplicationTempPathRoot = string.Empty;

        /// <summary>����·����ʱ�ļ��и���ַ</summary>
        public string ApplicationTempPathRoot
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationTempPathRoot))
                {
                    if (this.Configuration.Keys["ApplicationTempPathRoot"] == null)
                    {
                        // ����Ӧ����ʱĿ¼��Ĭ��·��ΪӦ��Ŀ¼�µ�temp�ļ��С�
                        this.m_ApplicationTempPathRoot = Path.Combine(this.ApplicationPathRoot, "temp");

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationTempPathRoot", this.m_ApplicationTempPathRoot));
                    }
                    else
                    {
                        this.m_ApplicationTempPathRoot = this.Configuration.Keys["ApplicationTempPathRoot"].Value;
                    }
                }

                return this.m_ApplicationTempPathRoot;
            }
        }
        #endregion

        #region ����:ApplicationSpringConfigFilePath
        private string m_ApplicationSpringConfigFilePath = string.Empty;

        /// <summary>Ӧ�ó���Spring�����ļ�·��</summary>
        public string ApplicationSpringConfigFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationSpringConfigFilePath))
                {
                    if (this.Configuration.Keys["ApplicationSpringConfigFilePath"] == null)
                    {
                        this.m_ApplicationSpringConfigFilePath = Path.Combine(this.ApplicationPathRoot, "config\\X3Platform.Spring.config");

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationSpringConfigFilePath", this.m_ApplicationSpringConfigFilePath));
                    }
                    else
                    {
                        this.m_ApplicationSpringConfigFilePath = this.Configuration.Keys["ApplicationSpringConfigFilePath"].Value;
                    }
                }

                return this.m_ApplicationSpringConfigFilePath;
            }
        }
        #endregion

        #region ����:ApplicationHomePage
        private string m_ApplicationHomePage = string.Empty;

        /// <summary>Ӧ�ó�����ҳ</summary>
        public string ApplicationHomePage
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationHomePage))
                {
                    if (this.Configuration.Keys["ApplicationHomePage"] == null)
                    {
                        this.m_ApplicationHomePage = "/apps/pages/default.aspx";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationHomePage", this.m_ApplicationHomePage));
                    }
                    else
                    {
                        this.m_ApplicationHomePage = this.Configuration.Keys["ApplicationHomePage"].Value;
                    }
                }

                return this.m_ApplicationHomePage;
            }
        }
        #endregion

        #region ����:ApplicationIconPath
        private string m_ApplicationIconPath = string.Empty;

        /// <summary>Ӧ�ó���ͼ��</summary>
        public string ApplicationIconPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationIconPath))
                {
                    if (this.Configuration.Keys["ApplicationIconPath"] == null)
                    {
                        this.m_ApplicationIconPath = "/resources/images/corporation/default/corporation.logo.png";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationIconPath", this.m_ApplicationIconPath));
                    }
                    else
                    {
                        this.m_ApplicationIconPath = this.Configuration.Keys["ApplicationIconPath"].Value;
                    }
                }

                return this.m_ApplicationIconPath;
            }
        }
        #endregion

        #region ����:ApplicationClientId
        private string m_ApplicationClientId = string.Empty;

        /// <summary>Ӧ�ÿͻ��˵��õ� AppKey</summary>
        public string ApplicationClientId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationClientId))
                {
                    if (this.Configuration.Keys["ApplicationClientId"] == null)
                    {
                        this.m_ApplicationClientId = Guid.NewGuid().ToString("N");

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationClientId", this.m_ApplicationClientId));
                    }
                    else
                    {
                        this.m_ApplicationClientId = this.Configuration.Keys["ApplicationClientId"].Value;
                    }
                }

                return this.m_ApplicationClientId;
            }
        }
        #endregion

        #region ����:ApplicationClientSecret
        private string m_ApplicationClientSecret = string.Empty;

        /// <summary>Ӧ�ÿͻ��˵��õ� AppSecret</summary>
        public string ApplicationClientSecret
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationClientSecret))
                {
                    if (this.Configuration.Keys["ApplicationClientSecret"] == null)
                    {
                        this.m_ApplicationClientSecret = StringHelper.ToRandom(6);

                        this.Configuration.Keys.Add(new KernelConfigurationKey("ApplicationClientSecret", this.m_ApplicationClientSecret));
                    }
                    else
                    {
                        this.m_ApplicationClientSecret = this.Configuration.Keys["ApplicationClientSecret"].Value;
                    }
                }

                return this.m_ApplicationClientSecret;
            }
        }
        #endregion

        #region ����:ApplicationClientSignature
        private string m_ApplicationClientSignature = string.Empty;

        /// <summary>Ӧ�ÿͻ��˵��õ� AppKey</summary>
        public string ApplicationClientSignature
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApplicationClientSignature))
                {
                    byte[] key = UTF8Encoding.UTF8.GetBytes(this.ApplicationClientId);

                    this.m_ApplicationClientSignature = Encrypter.EncryptAES(this.ApplicationClientSecret, key);

                    this.m_ApplicationClientSignature = this.m_ApplicationClientSignature.Replace("+", "_");
                }

                return this.m_ApplicationClientSignature;
            }
        }
        #endregion

        #region ����:DataSourceName
        private string m_DataSourceName = string.Empty;

        /// <summary>Ӧ�ó������������ݿ�����</summary>
        public string DataSourceName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DataSourceName))
                {
                    if (this.Configuration.Keys["DataSourceName"] == null)
                    {
                        this.m_DataSourceName = "ConnectionString";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("DataSourceName", this.m_DataSourceName));
                    }
                    else
                    {
                        this.m_DataSourceName = this.Configuration.Keys["DataSourceName"].Value;
                    }
                }

                return this.m_DataSourceName;
            }
        }
        #endregion

        #region ����:DataSourceType
        private string m_DataSourceType = string.Empty;

        /// <summary>Ӧ�ó������������ݿ�����</summary>
        public string DataSourceType
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DataSourceType))
                {
                    if (this.Configuration.Keys["DataSourceName"] == null)
                    {
                        this.m_DataSourceType = "MySql";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("DataSourceType", this.m_DataSourceName));
                    }
                    else
                    {
                        this.m_DataSourceType = this.Configuration.Keys["DataSourceType"].Value;
                    }
                }

                return this.m_DataSourceType;
            }
        }
        #endregion

        #region ����:UploadFileWizard
        private string m_UploadFileWizard = string.Empty;

        /// <summary>�ϴ��ļ�����</summary>
        public string UploadFileWizard
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_UploadFileWizard))
                {
                    if (this.Configuration.Keys["UploadFileWizard"] == null)
                    {
                        this.m_UploadFileWizard = "/uploads/core/default.aspx";

                        this.Configuration.Keys.Add(new KernelConfigurationKey("UploadFileWizard", this.m_UploadFileWizard));
                    }
                    else
                    {
                        this.m_UploadFileWizard = this.Configuration.Keys["UploadFileWizard"].Value;
                    }
                }

                return this.m_UploadFileWizard;
            }
        }
        #endregion

        #region ����:MSTestPathRoot
        private string m_MSTestPathRoot = string.Empty;

        /// <summary>����Ŀ¼��ַ</summary>
        public string MSTestPathRoot
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MSTestPathRoot))
                {
                    if (this.Configuration.Keys["MSTestPathRoot"] == null)
                    {
                        this.m_MSTestPathRoot = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                        this.Configuration.Keys.Add(new KernelConfigurationKey("MSTestPathRoot", this.m_MSTestPathRoot));
                    }
                    else
                    {
                        this.m_MSTestPathRoot = this.Configuration.Keys["MSTestPathRoot"].Value;
                    }
                }

                return this.m_MSTestPathRoot;
            }
        }
        #endregion
    }
}
