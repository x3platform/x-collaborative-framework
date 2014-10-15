#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :ActiveDirectoryConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

using System;
using System.IO;

using X3Platform.Configuration;

namespace X3Platform.ActiveDirectory.Configuration
{
    /// <summary>配置视图</summary>
    public class ActiveDirectoryConfigurationView : XmlConfigurationView<ActiveDirectoryConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.ActiveDirectory.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = "ActiveDirectory";

        #region ��̬属性:Instance
        private static volatile ActiveDirectoryConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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

        #region ���캯��:ActiveDirectoryConfigurationView()
        /// <summary>���캯��</summary>
        private ActiveDirectoryConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载配置信息</summary>
        public override void Reload()
        {
            base.Reload();

            // ��������Ϣ���ص�ȫ�ֵ�������
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
                    // 属性名称
                    string propertyName = "IntegratedMode";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_IntegratedMode = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_IntegratedMode = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_IntegratedMode))
                    {
                        m_IntegratedMode = "Off";
                    }
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
                    // 属性名称
                    string propertyName = "Domain";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_Domain = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_Domain = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_Domain))
                    {
                        m_Domain = "yourdomain.com";

                    }
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
                    // 属性名称
                    string propertyName = "SuffixEmailDomain";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_SuffixEmailDomain = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_SuffixEmailDomain = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_SuffixEmailDomain))
                    {
                        m_SuffixEmailDomain = "@www.yourdomain.com";
                    }
                }

                return m_SuffixEmailDomain;
            }
        }
        #endregion

        #region 属性:SuffixDistinguishedName
        private string m_SuffixDistinguishedName = string.Empty;

        /// <summary>Active Directory ����Ψһ���Ƶĺ�׺</summary>
        public string SuffixDistinguishedName
        {
            get
            {
                if (string.IsNullOrEmpty(m_SuffixDistinguishedName))
                {
                    // ��������
                    string propertyName = "SuffixDistinguishedName";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_SuffixDistinguishedName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_SuffixDistinguishedName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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
                    // ��������
                    string propertyName = "LDAPPath";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_LDAPPath = KernelConfigurationView.Instance.ReplaceKeyValue(
                            KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_LDAPPath = KernelConfigurationView.Instance.ReplaceKeyValue(
                            this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_LDAPPath))
                    {
                        m_LDAPPath = "LDAP://127.0.0.1/DC=yourdomain,DC=com";
                    }
                }

                return m_LDAPPath;
            }
        }
        #endregion

        #region 属性:LoginName
        private string m_LoginName = string.Empty;

        /// <summary>Active Directory ��¼��</summary>
        public string LoginName
        {
            get
            {
                if (string.IsNullOrEmpty(m_LoginName))
                {
                    // ��������
                    string propertyName = "LoginName";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_LoginName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_LoginName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_LoginName))
                    {
                        m_LoginName = "yourdomain\administrator";
                    }
                }

                return m_LoginName;
            }
        }
        #endregion

        #region 属性:Password
        private string m_Password = string.Empty;

        /// <summary>Active Directory ����</summary>
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(m_Password))
                {
                    // ��������
                    string propertyName = "Password";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_Password = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_Password = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_Password))
                    {
                        m_Password = "123456";
                    }
                }

                return m_Password;
            }
        }
        #endregion

        #region 属性:CorporationOrganizationFolderRoot
        private string m_CorporationOrganizationFolderRoot = string.Empty;

        /// <summary>Active Directory ��˾��֯���ŵĸ�Ŀ¼ (������ɫ)</summary>
        public string CorporationOrganizationFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationOrganizationFolderRoot))
                {
                    // ��������
                    string propertyName = "CorporationOrganizationFolderRoot";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_CorporationOrganizationFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_CorporationOrganizationFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_CorporationOrganizationFolderRoot))
                    {
                        m_CorporationOrganizationFolderRoot = "��֯�ṹ";
                    }
                }

                return m_CorporationOrganizationFolderRoot;
            }
        }
        #endregion

        #region 属性:CorporationUserFolderRoot
        private string m_CorporationUserFolderRoot = string.Empty;

        /// <summary>Active Directory ��˾�û����ŵĸ�Ŀ¼</summary>
        public string CorporationUserFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationUserFolderRoot))
                {
                    // ��������
                    string propertyName = "CorporationUserFolderRoot";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_CorporationUserFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_CorporationUserFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_CorporationUserFolderRoot))
                    {
                        m_CorporationUserFolderRoot = "��֯�û�";
                    }
                }

                return m_CorporationUserFolderRoot;
            }
        }
        #endregion

        #region 属性:CorporationGroupFolderRoot
        private string m_CorporationGroupFolderRoot = string.Empty;

        /// <summary>Active Directory ��˾Ⱥ�����ŵĸ�Ŀ¼</summary>
        public string CorporationGroupFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationGroupFolderRoot))
                {
                    // ��������
                    string propertyName = "CorporationGroupFolderRoot";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_CorporationGroupFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_CorporationGroupFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_CorporationGroupFolderRoot))
                    {
                        m_CorporationGroupFolderRoot = "����Ⱥ��";
                    }
                }

                return m_CorporationGroupFolderRoot;
            }
        }
        #endregion

        #region 属性:CorporationRoleFolderRoot
        private string m_CorporationRoleFolderRoot = string.Empty;

        /// <summary>Active Directory ��˾��ɫ���ŵĸ�Ŀ¼ (����֯�ṹ�еĽ�ɫ����)</summary>
        public string CorporationRoleFolderRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_CorporationRoleFolderRoot))
                {
                    // ��������
                    string propertyName = "CorporationRoleFolderRoot";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_CorporationRoleFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                                KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_CorporationRoleFolderRoot = KernelConfigurationView.Instance.ReplaceKeyValue(
                                this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_CorporationRoleFolderRoot))
                    {
                        m_CorporationRoleFolderRoot = "ͨ�ý�ɫ";
                    }
                }

                return m_CorporationRoleFolderRoot;
            }
        }
        #endregion

        #region 属性:NewlyCreatedAccountPassword
        private string m_NewlyCreatedAccountPassword = string.Empty;

        /// <summary>Active Directory �½��ʺŵ�Ĭ������</summary>
        public string NewlyCreatedAccountPassword
        {
            get
            {
                if (string.IsNullOrEmpty(m_NewlyCreatedAccountPassword))
                {
                    // ��������
                    string propertyName = "NewlyCreatedAccountPassword";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_NewlyCreatedAccountPassword = KernelConfigurationView.Instance.ReplaceKeyValue(
                                KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_NewlyCreatedAccountPassword = KernelConfigurationView.Instance.ReplaceKeyValue(
                                this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(m_NewlyCreatedAccountPassword))
                    {
                        m_NewlyCreatedAccountPassword = "123456";
                    }
                }

                return m_NewlyCreatedAccountPassword;
            }
        }
        #endregion  
    }
}
