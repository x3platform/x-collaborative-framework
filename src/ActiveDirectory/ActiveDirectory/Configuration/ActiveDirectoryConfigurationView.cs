namespace X3Platform.ActiveDirectory.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;
    #endregion

    /// <summary>配置视图</summary>
    public class ActiveDirectoryConfigurationView : XmlConfigurationView<ActiveDirectoryConfiguration>
    {
        /// <summary>配置文件的默认路径.</summary>
        private const string configFile = "config\\X3Platform.ActiveDirectory.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = "ActiveDirectory";

        #region 静态属性::Instance
        private static volatile ActiveDirectoryConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static ActiveDirectoryConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ActiveDirectoryConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:ActiveDirectoryConfigurationView()
        /// <summary>构造函数</summary>
        private ActiveDirectoryConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 基类初始化后会默认执行 Reload() 函数
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载配置信息</summary>
        public override void Reload()
        {
            base.Reload();

            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义属性
        //-------------------------------------------------------

        #region 属性:IntegratedMode
        private string m_IntegratedMode = string.Empty;

        /// <summary>Active Directory 集成模式</summary>
        public string IntegratedMode
        {
            get
            {
                if (string.IsNullOrEmpty(m_IntegratedMode))
                {
                    // 读取配置信息
                    this.m_IntegratedMode = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "IntegratedMode", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_IntegratedMode = StringHelper.NullOrEmptyTo(this.m_IntegratedMode, "Off");
                }

                return m_IntegratedMode.ToUpper();
            }
        }
        #endregion

        #region 属性:Domain
        private string m_Domain = string.Empty;

        /// <summary>Active Directory 域</summary>
        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(m_Domain))
                {
                    // 读取配置信息
                    this.m_Domain = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "Domain", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_Domain = StringHelper.NullOrEmptyTo(this.m_Domain, "yourdomain.com");
                }

                return m_Domain;
            }
        }
        #endregion

        #region 属性:SuffixEmailDomain
        private string m_SuffixEmailDomain = string.Empty;

        /// <summary>Active Directory 邮箱的后缀域名</summary>
        public string SuffixEmailDomain
        {
            get
            {
                if (string.IsNullOrEmpty(m_SuffixEmailDomain))
                {
                    // 读取配置信息
                    this.m_SuffixEmailDomain = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SuffixEmailDomain", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_SuffixEmailDomain = StringHelper.NullOrEmptyTo(this.m_SuffixEmailDomain, "@yourdomain.com");
                }

                return m_SuffixEmailDomain;
            }
        }
        #endregion

        #region 属性:SuffixDistinguishedName
        private string m_SuffixDistinguishedName = string.Empty;

        /// <summary>Active Directory 对象唯一名称的后缀</summary>
        public string SuffixDistinguishedName
        {
            get
            {
                if (string.IsNullOrEmpty(m_SuffixDistinguishedName))
                {
                    // 读取配置信息
                    this.m_SuffixDistinguishedName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SuffixDistinguishedName", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    if (string.IsNullOrEmpty(m_SuffixDistinguishedName))
                    {
                        if (string.IsNullOrEmpty(KernelConfigurationView.Instance.Domain))
                        {
                            m_SuffixDistinguishedName = string.Format(",DC={0},DC={1}", "domain", "com");
                        }
                        else
                        {
                            string[] domain = KernelConfigurationView.Instance.Domain.Split('.');

                            m_SuffixDistinguishedName = string.Format(",DC={0},DC={1}", domain[0], domain[1]);
                        }
                    }
                }

                return m_SuffixDistinguishedName;
            }
        }
        #endregion

        #region 属性:LDAPPath
        private string m_LDAPPath = string.Empty;

        /// <summary>ActiveDirectory LDAP Path</summary>
        public string LDAPPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_LDAPPath))
                {
                    // 读取配置信息
                    this.m_LDAPPath = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "LDAPPath", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_LDAPPath = StringHelper.NullOrEmptyTo(this.m_LDAPPath, "LDAP://127.0.0.1/DC=yourdomain,DC=com");
                }

                return this.m_LDAPPath;
            }
        }
        #endregion

        #region 属性:LoginName
        private string m_LoginName = string.Empty;

        /// <summary>Active Directory 登录名</summary>
        public string LoginName
        {
            get
            {
                if (string.IsNullOrEmpty(m_LoginName))
                {
                    // 读取配置信息
                    this.m_LoginName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "LoginName", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_LoginName = StringHelper.NullOrEmptyTo(this.m_LoginName, "yourdomain\administrator");
                }

                return m_LoginName;
            }
        }
        #endregion

        #region 属性:Password
        private string m_Password = string.Empty;

        /// <summary>Active Directory 密码</summary>
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(m_Password))
                {
                    // 读取配置信息
                    this.m_Password = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "Password", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    if (string.IsNullOrEmpty(this.m_Password))
                    {
                        this.m_Password = "000000";
                    }
                }

                return m_Password;
            }
        }
        #endregion

        #region 属性:CorporationOrganizationFolderRoot
        private string m_CorporationOrganizationFolderRoot = string.Empty;

        /// <summary>Active Directory 公司组织存放的根目录 (包括角色)</summary>
        public string CorporationOrganizationFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationOrganizationFolderRoot))
                {
                    // 读取配置信息
                    this.m_CorporationOrganizationFolderRoot = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CorporationOrganizationFolderRoot", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_CorporationOrganizationFolderRoot = StringHelper.NullOrEmptyTo(this.m_CorporationOrganizationFolderRoot, "CorporationOrganizationals");
                }

                return m_CorporationOrganizationFolderRoot;
            }
        }
        #endregion

        #region 属性:CorporationUserFolderRoot
        private string m_CorporationUserFolderRoot = string.Empty;

        /// <summary>Active Directory 公司用户存放的根目录</summary>
        public string CorporationUserFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationUserFolderRoot))
                {
                    // 读取配置信息
                    this.m_CorporationUserFolderRoot = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CorporationUserFolderRoot", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_CorporationUserFolderRoot = StringHelper.NullOrEmptyTo(this.m_CorporationUserFolderRoot, "CorporationUsers");
                }

                return m_CorporationUserFolderRoot;
            }
        }
        #endregion

        #region 属性:CorporationGroupFolderRoot
        private string m_CorporationGroupFolderRoot = string.Empty;

        /// <summary>Active Directory 公司群组存放的根目录</summary>
        public string CorporationGroupFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationGroupFolderRoot))
                {
                    // 读取配置信息
                    this.m_CorporationGroupFolderRoot = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CorporationGroupFolderRoot", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_CorporationGroupFolderRoot = StringHelper.NullOrEmptyTo(this.m_CorporationGroupFolderRoot, "CorporationGroups");
                }

                return m_CorporationGroupFolderRoot;
            }
        }
        #endregion

        #region 属性:CorporationRoleFolderRoot
        private string m_CorporationRoleFolderRoot = string.Empty;

        /// <summary>Active Directory 公司角色存放的根目录 (非组织结构中的角色数据)</summary>
        public string CorporationRoleFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationRoleFolderRoot))
                {
                    // 读取配置信息
                    this.m_CorporationRoleFolderRoot = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CorporationRoleFolderRoot", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_CorporationRoleFolderRoot = StringHelper.NullOrEmptyTo(this.m_CorporationRoleFolderRoot, "CorporationRoles");
                }

                return m_CorporationRoleFolderRoot;
            }
        }
        #endregion

        #region 属性:NewlyCreatedAccountPassword
        private string m_NewlyCreatedAccountPassword = string.Empty;

        /// <summary>Active Directory 新建帐号的默认密码</summary>
        public string NewlyCreatedAccountPassword
        {
            get
            {
                if (string.IsNullOrEmpty(m_NewlyCreatedAccountPassword))
                {
                    // 读取配置信息
                    this.m_NewlyCreatedAccountPassword = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "NewlyCreatedAccountPassword", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_NewlyCreatedAccountPassword = StringHelper.NullOrEmptyTo(this.m_NewlyCreatedAccountPassword, "000000");
                }

                return m_NewlyCreatedAccountPassword;
            }
        }
        #endregion
    }
}
