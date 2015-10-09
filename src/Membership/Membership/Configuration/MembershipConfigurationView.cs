namespace X3Platform.Membership.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Xml;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>人员及权限管理的配置视图</summary>
    public class MembershipConfigurationView : XmlConfigurationView<MembershipConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Membership.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = MembershipConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile MembershipConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static MembershipConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new MembershipConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:MembershipConfigurationView()
        /// <summary>构造函数</summary>
        private MembershipConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region 属性:Reload()
        /// <summary>重新加载配置信息</summary>
        public override void Reload()
        {
            base.Reload();

            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义属性
        // -------------------------------------------------------

        #region 属性:DataTablePrefix
        private string m_DataTablePrefix = string.Empty;

        /// <summary>数据表前缀</summary>
        public string DataTablePrefix
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DataTablePrefix))
                {
                    // 属性名称
                    string propertyName = "DataTablePrefix";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_DataTablePrefix = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_DataTablePrefix = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(this.m_DataTablePrefix))
                    {
                        this.m_DataTablePrefix = "Empty";
                    }
                }

                return this.m_DataTablePrefix == "Empty" ? string.Empty : this.m_DataTablePrefix;
            }
        }
        #endregion

        #region 属性:SingleSignOn
        private string m_SingleSignOn = string.Empty;

        /// <summary>单点登录</summary>
        public string SingleSignOn
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SingleSignOn))
                {
                    // 读取配置信息
                    this.m_SingleSignOn = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SingleSignOn",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SingleSignOn = StringHelper.NullOrEmptyTo(this.m_SingleSignOn, "Off");

                    this.m_SingleSignOn = this.m_SingleSignOn.ToUpper();
                }

                return this.m_SingleSignOn;
            }
        }
        #endregion

        #region 属性:SsoAuthUrl
        private string m_SsoAuthUrl = string.Empty;

        /// <summary>单点登录验证地址</summary>
        public string SsoAuthUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SsoAuthUrl))
                {
                    // 读取配置信息
                    this.m_SsoAuthUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SsoAuthUrl",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SsoAuthUrl = StringHelper.NullOrEmptyTo(this.m_SsoAuthUrl, KernelConfigurationView.Instance.HostName);
                }

                return this.m_SsoAuthUrl;
            }
        }
        #endregion

        #region 属性:SsoSessionUrl
        private string m_SsoSessionUrl = string.Empty;

        /// <summary>单点登录会话地址</summary>
        public string SsoSessionUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SsoSessionUrl))
                {
                    // 读取配置信息
                    this.m_SsoSessionUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SsoSessionUrl",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SsoSessionUrl = StringHelper.NullOrEmptyTo(this.m_SsoSessionUrl, KernelConfigurationView.Instance.HostName + "api/sessions.account.read.aspx?key={0}");
                }

                return this.m_SsoSessionUrl;
            }
        }
        #endregion

        #region 属性:SsoIdentityCookieToken
        private string m_SsoIdentityCookieToken = string.Empty;

        /// <summary>单点登录身份验证的Cookie的键</summary>
        public string SsoIdentityCookieToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SsoIdentityCookieToken))
                {
                    // 读取配置信息
                    this.m_SsoIdentityCookieToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SsoIdentityCookieToken",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SsoIdentityCookieToken = StringHelper.NullOrEmptyTo(this.m_SsoIdentityCookieToken, "session$sso$token");
                }

                return this.m_SsoIdentityCookieToken;
            }
        }
        #endregion

        #region 属性:PasswordEncryption
        private string m_PasswordEncryption = string.Empty;

        /// <summary>密码加密方式</summary>
        public string PasswordEncryption
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordEncryption))
                {
                    // 读取配置信息
                    this.m_PasswordEncryption = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordEncryption",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PasswordEncryption = StringHelper.NullOrEmptyTo(this.m_PasswordEncryption, "None");
                }

                return this.m_PasswordEncryption;
            }
        }
        #endregion

        #region 属性:PasswordEncryptionKey
        private string m_PasswordEncryptionKey = string.Empty;

        /// <summary>密码可逆加密方式的密钥</summary>
        public string PasswordEncryptionKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordEncryptionKey))
                {
                    // 读取配置信息
                    this.m_PasswordEncryptionKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordEncryptionIV",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PasswordEncryptionKey = StringHelper.NullOrEmptyTo(this.m_PasswordEncryptionKey, Guid.Empty.ToString("N"));
                }

                return this.m_PasswordEncryptionKey;
            }
        }
        #endregion

        #region 属性:PasswordEncryptionIV
        private string m_PasswordEncryptionIV = string.Empty;

        /// <summary>密码可逆加密方式的初始化向量</summary>
        public string PasswordEncryptionIV
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordEncryptionIV))
                {
                    // 读取配置信息
                    this.m_PasswordEncryptionIV = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordEncryptionIV",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PasswordEncryptionIV = StringHelper.NullOrEmptyTo(this.m_PasswordEncryptionIV, "00000000");
                }

                return this.m_PasswordEncryptionKey;
            }
        }
        #endregion

        #region 属性:PasswordPolicyRules
        private string m_PasswordPolicyRules = string.Empty;

        /// <summary>密码策略规则</summary>
        public string PasswordPolicyRules
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordPolicyRules))
                {
                    // 读取配置信息
                    this.m_PasswordPolicyRules = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyRules",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PasswordPolicyRules = StringHelper.NullOrEmptyTo(this.m_PasswordPolicyRules, "None");
                }

                return this.m_PasswordPolicyRules;
            }
        }
        #endregion

        #region 属性:PasswordPolicyMinimumLength
        private int m_PasswordPolicyMinimumLength = -1;

        /// <summary>密码最小长度规则</summary>
        public int PasswordPolicyMinimumLength
        {
            get
            {
                if (this.m_PasswordPolicyMinimumLength == -1)
                {
                    // 读取配置信息
                    this.m_PasswordPolicyMinimumLength = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyMinimumLength",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_PasswordPolicyMinimumLength == -1)
                    {
                        this.m_PasswordPolicyMinimumLength = 0;
                    }
                }

                return this.m_PasswordPolicyMinimumLength;
            }
        }
        #endregion

        #region 属性:PasswordPolicyCharacterRepeatedTimes
        private int m_PasswordPolicyCharacterRepeatedTimes = -1;

        /// <summary>密码相邻字符重复次数规则</summary>
        public int PasswordPolicyCharacterRepeatedTimes
        {
            get
            {
                if (this.m_PasswordPolicyCharacterRepeatedTimes == -1)
                {
                    // 读取配置信息
                    this.m_PasswordPolicyCharacterRepeatedTimes = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyCharacterRepeatedTimes",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_PasswordPolicyCharacterRepeatedTimes == -1)
                    {
                        this.m_PasswordPolicyCharacterRepeatedTimes = 0;
                    }
                }

                return this.m_PasswordPolicyCharacterRepeatedTimes;
            }
        }
        #endregion

        #region 属性:PasswordPolicyMaximumUsefulLife
        private int m_PasswordPolicyMaximumUsefulLife = -1;

        /// <summary>密码最长使用期限</summary>
        public int PasswordPolicyMaximumUsefulLife
        {
            get
            {
                if (this.m_PasswordPolicyMaximumUsefulLife == -1)
                {
                    // 读取配置信息
                    this.m_PasswordPolicyMaximumUsefulLife = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyMaximumUsefulLife",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_PasswordPolicyMaximumUsefulLife == -1)
                    {
                        this.m_PasswordPolicyMaximumUsefulLife = 0;
                    }
                }

                return this.m_PasswordPolicyMaximumUsefulLife;
            }
        }
        #endregion

        #region 属性:PasswordPolicyForcedChangePasswordForFirstLogin
        private string m_PasswordPolicyForcedChangePasswordForFirstLogin = string.Empty;

        /// <summary>当用户首次登陆时密码强制修改规则</summary>
        public string PasswordPolicyForcedChangePasswordForFirstLogin
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordPolicyForcedChangePasswordForFirstLogin))
                {
                    // 读取配置信息
                    this.m_PasswordPolicyForcedChangePasswordForFirstLogin = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyForcedChangePasswordForFirstLogin",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PasswordPolicyForcedChangePasswordForFirstLogin = StringHelper.NullOrEmptyTo(this.m_PasswordPolicyRules, "Off");

                    this.m_PasswordPolicyForcedChangePasswordForFirstLogin = this.m_PasswordPolicyForcedChangePasswordForFirstLogin.ToUpper();
                }

                return this.m_PasswordPolicyForcedChangePasswordForFirstLogin;
            }
        }
        #endregion

        #region 属性:PasswordPolicyForcedChangePasswordForMaximumUsePeriod
        private string m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = string.Empty;

        /// <summary>当用户密码超过最长使用期限时密码强制修改规则</summary>
        public string PasswordPolicyForcedChangePasswordForMaximumUsePeriod
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod))
                {
                    // 读取配置信息
                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyForcedChangePasswordForMaximumUsePeriod",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = StringHelper.NullOrEmptyTo(this.m_PasswordPolicyRules, "Off");

                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod.ToUpper();
                }

                return this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod;
            }
        }
        #endregion

        #region 属性:PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays
        private int m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays = -1;

        /// <summary>当用户密码超过最长使用期限前提醒N天密码修改规则</summary>
        public int PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays
        {
            get
            {
                if (this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays == -1)
                {
                    // 读取配置信息
                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays == -1)
                    {
                        this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays = 0;
                    }
                }

                return this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays;
            }
        }
        #endregion

        #region 属性:AutoBindingCertifiedTelephone
        private string m_AutoBindingCertifiedTelephone = string.Empty;

        /// <summary>根据字段数据自动绑定联系电话信息</summary>
        public string AutoBindingCertifiedTelephone
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingCertifiedTelephone))
                {
                    // 读取配置信息
                    this.m_AutoBindingCertifiedTelephone = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingCertifiedTelephone",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AutoBindingCertifiedTelephone = StringHelper.NullOrEmptyTo(this.m_AutoBindingCertifiedTelephone, "None");
                }

                return this.m_AutoBindingCertifiedTelephone;
            }
        }
        #endregion

        #region 属性:AutoBindingCertifiedEmail
        private string m_AutoBindingCertifiedEmail = string.Empty;

        /// <summary>根据字段数据自动绑定联系邮件信息</summary>
        public string AutoBindingCertifiedEmail
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingCertifiedEmail))
                {
                    // 读取配置信息
                    this.m_AutoBindingCertifiedEmail = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingCertifiedEmail",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AutoBindingCertifiedEmail = StringHelper.NullOrEmptyTo(this.m_AutoBindingCertifiedEmail, "None");
                }

                return this.m_AutoBindingCertifiedEmail;
            }
        }
        #endregion

        #region 属性:AutoBindingOrganizationUnitByPrimaryKey
        private string m_AutoBindingOrganizationUnitByPrimaryKey = string.Empty;

        /// <summary>根据字段数据自动绑定组织信息</summary>
        public string AutoBindingOrganizationUnitByPrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingOrganizationUnitByPrimaryKey))
                {
                    // 读取配置信息
                    this.m_AutoBindingOrganizationUnitByPrimaryKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingOrganizationUnitByPrimaryKey",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AutoBindingOrganizationUnitByPrimaryKey = StringHelper.NullOrEmptyTo(this.m_AutoBindingOrganizationUnitByPrimaryKey, "None");
                }

                return this.m_AutoBindingOrganizationUnitByPrimaryKey;
            }
        }
        #endregion

        #region 属性:AutoBindingJobByPrimaryKey
        private string m_AutoBindingJobByPrimaryKey = string.Empty;

        /// <summary>模拟验证的密码</summary>
        public string AutoBindingJobByPrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingJobByPrimaryKey))
                {
                    // 读取配置信息
                    this.m_AutoBindingJobByPrimaryKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingJobByPrimaryKey",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AutoBindingJobByPrimaryKey = StringHelper.NullOrEmptyTo(this.m_AutoBindingJobByPrimaryKey, "None");

                    this.m_AutoBindingJobByPrimaryKey = this.m_AutoBindingJobByPrimaryKey.ToUpper();
                }

                return this.m_AutoBindingJobByPrimaryKey;
            }
        }
        #endregion

        #region 属性:AutoBindingJobGradeByPrimaryKey
        private string m_AutoBindingJobGradeByPrimaryKey = string.Empty;

        /// <summary>模拟验证的密码</summary>
        public string AutoBindingJobGradeByPrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingJobGradeByPrimaryKey))
                {
                    // 读取配置信息
                    this.m_AutoBindingJobGradeByPrimaryKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingJobGradeByPrimaryKey",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AutoBindingJobGradeByPrimaryKey = StringHelper.NullOrEmptyTo(this.m_AutoBindingJobGradeByPrimaryKey, "None");

                    this.m_AutoBindingJobGradeByPrimaryKey = this.m_AutoBindingJobGradeByPrimaryKey.ToUpper();
                }

                return this.m_AutoBindingJobGradeByPrimaryKey;
            }
        }
        #endregion

        #region 属性:MockAuthenticationPassword
        private string m_MockAuthenticationPassword = string.Empty;

        /// <summary>模拟验证的密码</summary>
        public string MockAuthenticationPassword
        {
            get
            {
                if (string.IsNullOrEmpty(m_MockAuthenticationPassword))
                {
                    // 读取配置信息
                    this.m_MockAuthenticationPassword = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MockAuthenticationPassword",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_MockAuthenticationPassword = StringHelper.NullOrEmptyTo(this.m_MockAuthenticationPassword, string.Format("{0}@administrator", DateTime.Now.Year));
                }

                return m_MockAuthenticationPassword;
            }
        }
        #endregion

        #region 属性:AdministratorIdentityCookieToken
        private string m_AdministratorIdentityCookieToken = string.Empty;

        /// <summary>管理员标识</summary>
        public string AdministratorIdentityCookieToken
        {
            get
            {
                if (string.IsNullOrEmpty(m_AdministratorIdentityCookieToken))
                {
                    // 读取配置信息
                    this.m_AdministratorIdentityCookieToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AdministratorIdentityCookieToken",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AdministratorIdentityCookieToken = StringHelper.NullOrEmptyTo(this.m_AdministratorIdentityCookieToken, "administratorIdentity");
                }

                return this.m_AdministratorIdentityCookieToken;
            }
        }
        #endregion

        #region 属性:AccountIdentityCookieToken
        private string m_AccountIdentityCookieToken = string.Empty;

        /// <summary>成员标识</summary>
        public string AccountIdentityCookieToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AccountIdentityCookieToken))
                {
                    // 读取配置信息
                    this.m_AccountIdentityCookieToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AccountIdentityCookieToken",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_AccountIdentityCookieToken = StringHelper.NullOrEmptyTo(this.m_AccountIdentityCookieToken, "session$token");
                }

                return this.m_AccountIdentityCookieToken;
            }
        }
        #endregion

        #region 属性:AccountGrantOffsetDays
        private int m_AccountGrantOffsetDays = -1;

        /// <summary>用户委托数据偏移天数</summary>
        public int AccountGrantOffsetDays
        {
            get
            {
                if (this.m_AccountGrantOffsetDays == -1)
                {
                    // 读取配置信息
                    this.m_AccountGrantOffsetDays = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AccountGrantOffsetDays",
                        this.Configuration.Keys));

                    if (this.m_AccountGrantOffsetDays == -1)
                    {
                        // 如果配置文件里没有设置，设置一个默认值。
                        this.m_AccountGrantOffsetDays = 0;
                    }
                }

                return this.m_AccountGrantOffsetDays;
            }
        }
        #endregion

        #region 属性:DefaultPassword
        private string m_DefaultPassword = string.Empty;

        /// <summary>默认初始密码</summary>
        public string DefaultPassword
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultPassword))
                {
                    // 读取配置信息
                    this.m_DefaultPassword = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultPassword",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_DefaultPassword = StringHelper.NullOrEmptyTo(this.m_DefaultPassword, "{Random}");
                }

                return this.m_DefaultPassword;
            }
        }
        #endregion

        #region 属性:DefaultOrganizationUnitId
        private string m_DefaultOrganizationUnitId = string.Empty;

        /// <summary>默认的组织标识</summary>
        public string DefaultOrganizationUnitId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultOrganizationUnitId))
                {
                    // 读取配置信息
                    this.m_DefaultOrganizationUnitId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultOrganizationUnitId",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_DefaultOrganizationUnitId = StringHelper.NullOrEmptyTo(this.m_DefaultOrganizationUnitId, Guid.Empty.ToString());
                }

                return this.m_DefaultOrganizationUnitId;
            }
        }
        #endregion

        #region 属性:DefaultRoleId
        private string m_DefaultRoleId = string.Empty;

        /// <summary>默认的角色标识</summary>
        public string DefaultRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultRoleId))
                {
                    // 读取配置信息
                    this.m_DefaultRoleId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultRoleId",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_DefaultRoleId = StringHelper.NullOrEmptyTo(this.m_DefaultRoleId, Guid.Empty.ToString());
                }

                return this.m_DefaultRoleId;
            }
        }
        #endregion

        #region 属性:DefaultStandardRoleId
        private string m_DefaultStandardRoleId = string.Empty;

        /// <summary>默认的标准角色标识</summary>
        public string DefaultStandardRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultStandardRoleId))
                {
                    // 读取配置信息
                    this.m_DefaultStandardRoleId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultStandardRoleId",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_DefaultStandardRoleId = StringHelper.NullOrEmptyTo(this.m_DefaultStandardRoleId, Guid.Empty.ToString());
                }

                return this.m_DefaultStandardRoleId;
            }
        }
        #endregion

        #region 属性:DefaultGeneralRoleId
        private string m_DefaultGeneralRoleId = string.Empty;

        /// <summary>默认的通用角色标识</summary>
        public string DefaultGeneralRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultGeneralRoleId))
                {
                    // 读取配置信息
                    this.m_DefaultGeneralRoleId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultGeneralRoleId",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_DefaultGeneralRoleId = StringHelper.NullOrEmptyTo(this.m_DefaultGeneralRoleId, Guid.Empty.ToString());
                }

                return this.m_DefaultGeneralRoleId;
            }
        }
        #endregion

        #region 属性:DefaultScopeText
        private string m_DefaultScopeText = string.Empty;

        /// <summary>默认的权限范围</summary>
        public string DefaultScopeText
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultScopeText))
                {
                    // 读取配置信息
                    this.m_DefaultScopeText = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultScopeText",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_DefaultScopeText = StringHelper.NullOrEmptyTo(this.m_DefaultScopeText, "Role#00000000-0000-0000-0000-000000000000#所有人");
                }

                return this.m_DefaultScopeText;
            }
        }
        #endregion

        #region 属性:PackageServiceDisplayName
        private string m_PackageServiceDisplayName = string.Empty;

        /// <summary>同步服务响应时显示的名称</summary>
        public string PackageServiceDisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageServiceDisplayName))
                {
                    // 读取配置信息
                    this.m_PackageServiceDisplayName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageServiceDisplayName",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageServiceDisplayName = StringHelper.NullOrEmptyTo(this.m_PackageServiceDisplayName, "人员及权限数据同步管理系统");
                }

                return this.m_PackageServiceDisplayName;
            }
        }
        #endregion

        #region 属性:PackageServiceValidateSecurityToken
        private string m_PackageServiceValidateSecurityToken = string.Empty;

        /// <summary>同步服务验证安全标识</summary>
        public string PackageServiceValidateSecurityToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageServiceValidateSecurityToken))
                {
                    // 读取配置信息
                    this.m_PackageServiceValidateSecurityToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageServiceValidateSecurityToken",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageServiceValidateSecurityToken = StringHelper.NullOrEmptyTo(this.m_PackageServiceValidateSecurityToken, "Off");

                    this.m_PackageServiceValidateSecurityToken = this.m_PackageServiceValidateSecurityToken.ToUpper();
                }

                return this.m_PackageServiceValidateSecurityToken;
            }
        }
        #endregion

        #region 属性:PackageStoragePathRoot
        private string m_PackageStoragePathRoot = string.Empty;

        /// <summary>同步更新包文件夹根目录位置</summary>
        public string PackageStoragePathRoot
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStoragePathRoot))
                {
                    // 读取配置信息
                    this.m_PackageStoragePathRoot = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStoragePathRoot",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageStoragePathRoot = StringHelper.NullOrEmptyTo(this.m_PackageStoragePathRoot, KernelConfigurationView.Instance.ApplicationPathRoot + "packages\\");
                }

                return this.m_PackageStoragePathRoot;
            }
        }
        #endregion

        #region 属性:PackageStorageOutputApplicationId
        private string m_PackageStorageOutputApplicationId = string.Empty;

        /// <summary>同步更新包输出时使用的 ApplicationId</summary>
        public string PackageStorageOutputApplicationId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageOutputApplicationId))
                {
                    // 读取配置信息
                    this.m_PackageStorageOutputApplicationId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageOutputApplicationId",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageStorageOutputApplicationId = StringHelper.NullOrEmptyTo(this.m_PackageStorageOutputApplicationId, "default");
                }

                return this.m_PackageStorageOutputApplicationId;
            }
        }
        #endregion

        #region 属性:PackageStorageOutputPackageTypeValues
        private string m_PackageStorageOutputPackageTypeValues = string.Empty;

        /// <summary>同步更新包输出的数据包类型值</summary>
        public string PackageStorageOutputPackageTypeValues
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageOutputPackageTypeValues))
                {
                    // 读取配置信息
                    this.m_PackageStorageOutputPackageTypeValues = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageOutputPackageTypeValues",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageStorageOutputPackageTypeValues = StringHelper.NullOrEmptyTo(this.m_PackageStorageOutputPackageTypeValues, "organization,role,user");
                }

                return this.m_PackageStorageOutputPackageTypeValues;
            }
        }
        #endregion

        #region 属性:PackageStorageOutputMaxRowCount
        private int m_PackageStorageOutputMaxRowCount = 0;

        /// <summary>同步更新包输出时每个数据包的最大记录数</summary>
        public int PackageStorageOutputMaxRowCount
        {
            get
            {
                if (this.m_PackageStorageOutputMaxRowCount == 0)
                {
                    // 读取配置信息
                    this.m_PackageStorageOutputMaxRowCount = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageOutputMaxRowCount",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_PackageStorageOutputMaxRowCount == 0)
                    {
                        this.m_PackageStorageOutputMaxRowCount = 50;
                    }
                }

                return this.m_PackageStorageOutputMaxRowCount;
            }
        }
        #endregion

        #region 属性:PackageStorageViewUrlPrefix
        private string m_PackageStorageViewUrlPrefix = string.Empty;

        /// <summary>同步更新包文件查看地址前缀</summary>
        public string PackageStorageViewUrlPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageViewUrlPrefix))
                {
                    // 读取配置信息
                    this.m_PackageStorageViewUrlPrefix = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageViewUrlPrefix",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageStorageViewUrlPrefix = StringHelper.NullOrEmptyTo(this.m_PackageStorageViewUrlPrefix, KernelConfigurationView.Instance.HostName);
                }

                return this.m_PackageStorageViewUrlPrefix;
            }
        }
        #endregion

        #region 属性:PackageStorageNoticeMode
        private string m_PackageStorageNoticeMode = string.Empty;

        /// <summary>同步更新包变化通知模式</summary>
        public string PackageStorageNoticeMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageNoticeMode))
                {
                    // 读取配置信息
                    this.m_PackageStorageNoticeMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageNoticeMode",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageStorageNoticeMode = StringHelper.NullOrEmptyTo(this.m_PackageStorageNoticeMode, "Off");

                    this.m_PackageStorageNoticeMode = this.m_PackageStorageNoticeMode.ToUpper();
                }

                return this.m_PackageStorageNoticeMode;
            }
        }
        #endregion

        #region 属性:PackageStorageNoticeScope
        private string m_PackageStorageNoticeScope = string.Empty;

        /// <summary>同步更新包变化通知范围</summary>
        public string PackageStorageNoticeScope
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageNoticeScope))
                {
                    // 读取配置信息
                    this.m_PackageStorageNoticeScope = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageNoticeScope",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_PackageStorageNoticeScope = StringHelper.NullOrEmptyTo(this.m_PackageStorageNoticeScope, "Account#00000000-0000-0000-0000-000000000000#超级管理员");
                }

                return this.m_PackageStorageNoticeScope;
            }
        }
        #endregion
    }
}
