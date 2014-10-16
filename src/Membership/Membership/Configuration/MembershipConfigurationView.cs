// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :MembershipConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Xml;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>������ͼ</summary>
    public class MembershipConfigurationView : XmlConfigurationView<MembershipConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Membership.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = MembershipConfiguration.ApplicationName;

        #region 静态属性::Instance
        private static volatile MembershipConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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

        #region ���캯��:MembershipConfigurationView()
        /// <summary>���캯��</summary>
        private MembershipConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region 属性:Reload()
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

        #region 属性:SingleSignOn
        private string m_SingleSignOn = string.Empty;

        /// <summary>������¼</summary>
        public string SingleSignOn
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SingleSignOn))
                {
                    // ��ȡ������Ϣ
                    this.m_SingleSignOn = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SingleSignOn",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_SingleSignOn = StringHelper.NullOrEmptyTo(this.m_SingleSignOn, "Off");

                    this.m_SingleSignOn = this.m_SingleSignOn.ToUpper();
                }

                return this.m_SingleSignOn;
            }
        }
        #endregion

        #region 属性:SsoAuthUrl
        private string m_SsoAuthUrl = string.Empty;

        /// <summary>������¼��֤��ַ</summary>
        public string SsoAuthUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SsoAuthUrl))
                {
                    // ��ȡ������Ϣ
                    this.m_SsoAuthUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SsoAuthUrl",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_SsoAuthUrl = StringHelper.NullOrEmptyTo(this.m_SsoAuthUrl, KernelConfigurationView.Instance.HostName);
                }

                return this.m_SsoAuthUrl;
            }
        }
        #endregion

        #region 属性:SsoSessionUrl
        private string m_SsoSessionUrl = string.Empty;

        /// <summary>������¼�Ự��ַ</summary>
        public string SsoSessionUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SsoSessionUrl))
                {
                    // ��ȡ������Ϣ
                    this.m_SsoSessionUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SsoSessionUrl",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_SsoSessionUrl = StringHelper.NullOrEmptyTo(this.m_SsoSessionUrl, KernelConfigurationView.Instance.HostName + "api/sessions.account.read.aspx?key={0}");
                }

                return this.m_SsoSessionUrl;
            }
        }
        #endregion

        #region 属性:SsoIdentityCookieToken
        private string m_SsoIdentityCookieToken = string.Empty;

        /// <summary>������¼������֤��Cookie�ļ�</summary>
        public string SsoIdentityCookieToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SsoIdentityCookieToken))
                {
                    // ��ȡ������Ϣ
                    this.m_SsoIdentityCookieToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SsoIdentityCookieToken",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_SsoIdentityCookieToken = StringHelper.NullOrEmptyTo(this.m_SsoIdentityCookieToken, "session$sso$token");
                }

                return this.m_SsoIdentityCookieToken;
            }
        }
        #endregion

        #region 属性:PasswordEncryption
        private string m_PasswordEncryption = string.Empty;

        /// <summary>�������ܷ�ʽ</summary>
        public string PasswordEncryption
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordEncryption))
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordEncryption = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordEncryption",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PasswordEncryption = StringHelper.NullOrEmptyTo(this.m_PasswordEncryption, "None");
                }

                return this.m_PasswordEncryption;
            }
        }
        #endregion

        #region 属性:PasswordPolicyRules
        private string m_PasswordPolicyRules = string.Empty;

        /// <summary>�������Թ���</summary>
        public string PasswordPolicyRules
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordPolicyRules))
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyRules = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyRules",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PasswordPolicyRules = StringHelper.NullOrEmptyTo(this.m_PasswordPolicyRules, "None");
                }

                return this.m_PasswordPolicyRules;
            }
        }
        #endregion

        #region 属性:PasswordPolicyMinimumLength
        private int m_PasswordPolicyMinimumLength = -1;

        /// <summary>������С���ȹ���</summary>
        public int PasswordPolicyMinimumLength
        {
            get
            {
                if (this.m_PasswordPolicyMinimumLength == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyMinimumLength = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyMinimumLength",
                        this.Configuration.Keys));

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>���������ַ��ظ���������</summary>
        public int PasswordPolicyCharacterRepeatedTimes
        {
            get
            {
                if (this.m_PasswordPolicyCharacterRepeatedTimes == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyCharacterRepeatedTimes = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyCharacterRepeatedTimes",
                        this.Configuration.Keys));

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>�����ʹ������</summary>
        public int PasswordPolicyMaximumUsefulLife
        {
            get
            {
                if (this.m_PasswordPolicyMaximumUsefulLife == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyMaximumUsefulLife = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyMaximumUsefulLife",
                        this.Configuration.Keys));

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>���û��״ε�½ʱ����ǿ���޸Ĺ���</summary>
        public string PasswordPolicyForcedChangePasswordForFirstLogin
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordPolicyForcedChangePasswordForFirstLogin))
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyForcedChangePasswordForFirstLogin = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyForcedChangePasswordForFirstLogin",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PasswordPolicyForcedChangePasswordForFirstLogin = StringHelper.NullOrEmptyTo(this.m_PasswordPolicyRules, "Off");

                    this.m_PasswordPolicyForcedChangePasswordForFirstLogin = this.m_PasswordPolicyForcedChangePasswordForFirstLogin.ToUpper();
                }

                return this.m_PasswordPolicyForcedChangePasswordForFirstLogin;
            }
        }
        #endregion

        #region 属性:PasswordPolicyForcedChangePasswordForMaximumUsePeriod
        private string m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = string.Empty;

        /// <summary>���û����볬���ʹ������ʱ����ǿ���޸Ĺ���</summary>
        public string PasswordPolicyForcedChangePasswordForMaximumUsePeriod
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod))
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyForcedChangePasswordForMaximumUsePeriod",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = StringHelper.NullOrEmptyTo(this.m_PasswordPolicyRules, "Off");

                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod = this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod.ToUpper();
                }

                return this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriod;
            }
        }
        #endregion

        #region 属性:PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays
        private int m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays = -1;

        /// <summary>���û����볬���ʹ������ǰ����N�������޸Ĺ���</summary>
        public int PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays
        {
            get
            {
                if (this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PasswordPolicyForcedChangePasswordForMaximumUsePeriodOffsetDays",
                        this.Configuration.Keys));

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>�����ֶ������Զ�������ϵ�绰��Ϣ</summary>
        public string AutoBindingCertifiedTelephone
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingCertifiedTelephone))
                {
                    // ��ȡ������Ϣ
                    this.m_AutoBindingCertifiedTelephone = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingCertifiedTelephone",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AutoBindingCertifiedTelephone = StringHelper.NullOrEmptyTo(this.m_AutoBindingCertifiedTelephone, "None");
                }

                return this.m_AutoBindingCertifiedTelephone;
            }
        }
        #endregion

        #region 属性:AutoBindingCertifiedEmail
        private string m_AutoBindingCertifiedEmail = string.Empty;

        /// <summary>�����ֶ������Զ�������ϵ�ʼ���Ϣ</summary>
        public string AutoBindingCertifiedEmail
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingCertifiedEmail))
                {
                    // ��ȡ������Ϣ
                    this.m_AutoBindingCertifiedEmail = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingCertifiedEmail",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AutoBindingCertifiedEmail = StringHelper.NullOrEmptyTo(this.m_AutoBindingCertifiedEmail, "None");
                }

                return this.m_AutoBindingCertifiedEmail;
            }
        }
        #endregion

        #region 属性:AutoBindingOrganizationByPrimaryKey
        private string m_AutoBindingOrganizationByPrimaryKey = string.Empty;

        /// <summary>�����ֶ������Զ�������֯��Ϣ</summary>
        public string AutoBindingOrganizationByPrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingOrganizationByPrimaryKey))
                {
                    // ��ȡ������Ϣ
                    this.m_AutoBindingOrganizationByPrimaryKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingOrganizationByPrimaryKey",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AutoBindingOrganizationByPrimaryKey = StringHelper.NullOrEmptyTo(this.m_AutoBindingOrganizationByPrimaryKey, "None");
                }

                return this.m_AutoBindingOrganizationByPrimaryKey;
            }
        }
        #endregion

        #region 属性:AutoBindingJobByPrimaryKey
        private string m_AutoBindingJobByPrimaryKey = string.Empty;

        /// <summary>ģ����֤������</summary>
        public string AutoBindingJobByPrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingJobByPrimaryKey))
                {
                    // ��ȡ������Ϣ
                    this.m_AutoBindingJobByPrimaryKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingJobByPrimaryKey",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AutoBindingJobByPrimaryKey = StringHelper.NullOrEmptyTo(this.m_AutoBindingJobByPrimaryKey, "None");

                    this.m_AutoBindingJobByPrimaryKey = this.m_AutoBindingJobByPrimaryKey.ToUpper();
                }

                return this.m_AutoBindingJobByPrimaryKey;
            }
        }
        #endregion

        #region 属性:AutoBindingJobGradeByPrimaryKey
        private string m_AutoBindingJobGradeByPrimaryKey = string.Empty;

        /// <summary>ģ����֤������</summary>
        public string AutoBindingJobGradeByPrimaryKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AutoBindingJobGradeByPrimaryKey))
                {
                    // ��ȡ������Ϣ
                    this.m_AutoBindingJobGradeByPrimaryKey = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AutoBindingJobGradeByPrimaryKey",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AutoBindingJobGradeByPrimaryKey = StringHelper.NullOrEmptyTo(this.m_AutoBindingJobGradeByPrimaryKey, "None");

                    this.m_AutoBindingJobGradeByPrimaryKey = this.m_AutoBindingJobGradeByPrimaryKey.ToUpper();
                }

                return this.m_AutoBindingJobGradeByPrimaryKey;
            }
        }
        #endregion

        #region 属性:MockAuthenticationPassword
        private string m_MockAuthenticationPassword = string.Empty;

        /// <summary>ģ����֤������</summary>
        public string MockAuthenticationPassword
        {
            get
            {
                if (string.IsNullOrEmpty(m_MockAuthenticationPassword))
                {
                    // ��ȡ������Ϣ
                    this.m_MockAuthenticationPassword = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MockAuthenticationPassword",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_MockAuthenticationPassword = StringHelper.NullOrEmptyTo(this.m_MockAuthenticationPassword, string.Format("{0}@administrator", DateTime.Now.Year));
                }

                return m_MockAuthenticationPassword;
            }
        }
        #endregion

        #region 属性:AdministratorIdentityCookieToken
        private string m_AdministratorIdentityCookieToken = string.Empty;

        /// <summary>����Ա��ʶ</summary>
        public string AdministratorIdentityCookieToken
        {
            get
            {
                if (string.IsNullOrEmpty(m_AdministratorIdentityCookieToken))
                {
                    // ��ȡ������Ϣ
                    this.m_AdministratorIdentityCookieToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AdministratorIdentityCookieToken",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AdministratorIdentityCookieToken = StringHelper.NullOrEmptyTo(this.m_AdministratorIdentityCookieToken, "administratorIdentity");
                }

                return this.m_AdministratorIdentityCookieToken;
            }
        }
        #endregion

        #region 属性:AccountIdentityCookieToken
        private string m_AccountIdentityCookieToken = string.Empty;

        /// <summary>��Ա��ʶ</summary>
        public string AccountIdentityCookieToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AccountIdentityCookieToken))
                {
                    // ��ȡ������Ϣ
                    this.m_AccountIdentityCookieToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AccountIdentityCookieToken",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_AccountIdentityCookieToken = StringHelper.NullOrEmptyTo(this.m_AccountIdentityCookieToken, "session$token");
                }

                return this.m_AccountIdentityCookieToken;
            }
        }
        #endregion

        #region 属性:AccountGrantOffsetDays
        private int m_AccountGrantOffsetDays = -1;

        /// <summary>�û�ί������ƫ������</summary>
        public int AccountGrantOffsetDays
        {
            get
            {
                if (this.m_AccountGrantOffsetDays == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_AccountGrantOffsetDays = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AccountGrantOffsetDays",
                        this.Configuration.Keys));

                    if (this.m_AccountGrantOffsetDays == -1)
                    {
                        // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                        this.m_AccountGrantOffsetDays = 0;
                    }
                }

                return this.m_AccountGrantOffsetDays;
            }
        }
        #endregion

        #region 属性:DefaultPassword
        private string m_DefaultPassword = string.Empty;

        /// <summary>Ĭ�ϳ�ʼ����</summary>
        public string DefaultPassword
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultPassword))
                {
                    // ��ȡ������Ϣ
                    this.m_DefaultPassword = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultPassword",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_DefaultPassword = StringHelper.NullOrEmptyTo(this.m_DefaultPassword, "{Random}");
                }

                return this.m_DefaultPassword;
            }
        }
        #endregion

        #region 属性:DefaultOrganizationId
        private string m_DefaultOrganizationId = string.Empty;

        /// <summary>Ĭ�ϵ���֯��ʶ</summary>
        public string DefaultOrganizationId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultOrganizationId))
                {
                    // ��ȡ������Ϣ
                    this.m_DefaultOrganizationId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultOrganizationId",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_DefaultOrganizationId = StringHelper.NullOrEmptyTo(this.m_DefaultOrganizationId, Guid.Empty.ToString());
                }

                return this.m_DefaultOrganizationId;
            }
        }
        #endregion

        #region 属性:DefaultRoleId
        private string m_DefaultRoleId = string.Empty;

        /// <summary>Ĭ�ϵĽ�ɫ��ʶ</summary>
        public string DefaultRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultRoleId))
                {
                    // ��ȡ������Ϣ
                    this.m_DefaultRoleId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultRoleId",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_DefaultRoleId = StringHelper.NullOrEmptyTo(this.m_DefaultRoleId, Guid.Empty.ToString());
                }

                return this.m_DefaultRoleId;
            }
        }
        #endregion

        #region 属性:DefaultStandardRoleId
        private string m_DefaultStandardRoleId = string.Empty;

        /// <summary>Ĭ�ϵı�׼��ɫ��ʶ</summary>
        public string DefaultStandardRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultStandardRoleId))
                {
                    // ��ȡ������Ϣ
                    this.m_DefaultStandardRoleId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultStandardRoleId",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_DefaultStandardRoleId = StringHelper.NullOrEmptyTo(this.m_DefaultStandardRoleId, Guid.Empty.ToString());
                }

                return this.m_DefaultStandardRoleId;
            }
        }
        #endregion

        #region 属性:DefaultGeneralRoleId
        private string m_DefaultGeneralRoleId = string.Empty;

        /// <summary>Ĭ�ϵ�ͨ�ý�ɫ��ʶ</summary>
        public string DefaultGeneralRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultGeneralRoleId))
                {
                    // ��ȡ������Ϣ
                    this.m_DefaultGeneralRoleId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultGeneralRoleId",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_DefaultGeneralRoleId = StringHelper.NullOrEmptyTo(this.m_DefaultGeneralRoleId, Guid.Empty.ToString());
                }

                return this.m_DefaultGeneralRoleId;
            }
        }
        #endregion

        #region 属性:DefaultScopeText
        private string m_DefaultScopeText = string.Empty;

        /// <summary>Ĭ�ϵ�Ȩ�޷�Χ</summary>
        public string DefaultScopeText
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DefaultScopeText))
                {
                    // ��ȡ������Ϣ
                    this.m_DefaultScopeText = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultScopeText",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_DefaultScopeText = StringHelper.NullOrEmptyTo(this.m_DefaultScopeText, "Role#00000000-0000-0000-0000-000000000000#������");
                }

                return this.m_DefaultScopeText;
            }
        }
        #endregion

        #region 属性:PackageServiceDisplayName
        private string m_PackageServiceDisplayName = string.Empty;

        /// <summary>ͬ��������Ӧʱ��ʾ������</summary>
        public string PackageServiceDisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageServiceDisplayName))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageServiceDisplayName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageServiceDisplayName",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageServiceDisplayName = StringHelper.NullOrEmptyTo(this.m_PackageServiceDisplayName, "��Ա��Ȩ������ͬ������ϵͳ");
                }

                return this.m_PackageServiceDisplayName;
            }
        }
        #endregion

        #region 属性:PackageServiceValidateSecurityToken
        private string m_PackageServiceValidateSecurityToken = string.Empty;

        /// <summary>ͬ��������֤��ȫ��ʶ</summary>
        public string PackageServiceValidateSecurityToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageServiceValidateSecurityToken))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageServiceValidateSecurityToken = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageServiceValidateSecurityToken",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageServiceValidateSecurityToken = StringHelper.NullOrEmptyTo(this.m_PackageServiceValidateSecurityToken, "Off");

                    this.m_PackageServiceValidateSecurityToken = this.m_PackageServiceValidateSecurityToken.ToUpper();
                }

                return this.m_PackageServiceValidateSecurityToken;
            }
        }
        #endregion

        #region 属性:PackageStoragePathRoot
        private string m_PackageStoragePathRoot = string.Empty;

        /// <summary>ͬ�����°��ļ��и�Ŀ¼λ��</summary>
        public string PackageStoragePathRoot
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStoragePathRoot))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStoragePathRoot = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStoragePathRoot",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageStoragePathRoot = StringHelper.NullOrEmptyTo(this.m_PackageStoragePathRoot, KernelConfigurationView.Instance.ApplicationPathRoot + "packages\\");
                }

                return this.m_PackageStoragePathRoot;
            }
        }
        #endregion

        #region 属性:PackageStorageOutputApplicationId
        private string m_PackageStorageOutputApplicationId = string.Empty;

        /// <summary>ͬ�����°�����ʱʹ�õ� ApplicationId</summary>
        public string PackageStorageOutputApplicationId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageOutputApplicationId))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStorageOutputApplicationId = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageOutputApplicationId",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageStorageOutputApplicationId = StringHelper.NullOrEmptyTo(this.m_PackageStorageOutputApplicationId, "default");
                }

                return this.m_PackageStorageOutputApplicationId;
            }
        }
        #endregion

        #region 属性:PackageStorageOutputPackageTypeValues
        private string m_PackageStorageOutputPackageTypeValues = string.Empty;

        /// <summary>ͬ�����°����������ݰ�����ֵ</summary>
        public string PackageStorageOutputPackageTypeValues
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageOutputPackageTypeValues))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStorageOutputPackageTypeValues = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageOutputPackageTypeValues",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageStorageOutputPackageTypeValues = StringHelper.NullOrEmptyTo(this.m_PackageStorageOutputPackageTypeValues, "organization,role,user");
                }

                return this.m_PackageStorageOutputPackageTypeValues;
            }
        }
        #endregion

        #region 属性:PackageStorageOutputMaxRowCount
        private int m_PackageStorageOutputMaxRowCount = 0;

        /// <summary>ͬ�����°�����ʱÿ�����ݰ���������¼��</summary>
        public int PackageStorageOutputMaxRowCount
        {
            get
            {
                if (this.m_PackageStorageOutputMaxRowCount == 0)
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStorageOutputMaxRowCount = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageOutputMaxRowCount",
                        this.Configuration.Keys));

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>ͬ�����°��ļ��鿴��ַǰ׺</summary>
        public string PackageStorageViewUrlPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageViewUrlPrefix))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStorageViewUrlPrefix = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageViewUrlPrefix",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageStorageViewUrlPrefix = StringHelper.NullOrEmptyTo(this.m_PackageStorageViewUrlPrefix, KernelConfigurationView.Instance.HostName);
                }

                return this.m_PackageStorageViewUrlPrefix;
            }
        }
        #endregion

        #region 属性:PackageStorageNoticeMode
        private string m_PackageStorageNoticeMode = string.Empty;

        /// <summary>ͬ�����°��仯֪ͨģʽ</summary>
        public string PackageStorageNoticeMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageNoticeMode))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStorageNoticeMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageNoticeMode",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageStorageNoticeMode = StringHelper.NullOrEmptyTo(this.m_PackageStorageNoticeMode, "Off");

                    this.m_PackageStorageNoticeMode = this.m_PackageStorageNoticeMode.ToUpper();
                }

                return this.m_PackageStorageNoticeMode;
            }
        }
        #endregion

        #region 属性:PackageStorageNoticeScope
        private string m_PackageStorageNoticeScope = string.Empty;

        /// <summary>ͬ�����°��仯֪ͨ��Χ</summary>
        public string PackageStorageNoticeScope
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PackageStorageNoticeScope))
                {
                    // ��ȡ������Ϣ
                    this.m_PackageStorageNoticeScope = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PackageStorageNoticeScope",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_PackageStorageNoticeScope = StringHelper.NullOrEmptyTo(this.m_PackageStorageNoticeScope, "Account#00000000-0000-0000-0000-000000000000#��������Ա");
                }

                return this.m_PackageStorageNoticeScope;
            }
        }
        #endregion
    }
}
