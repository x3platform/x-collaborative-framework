// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :WebConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Web.Configuration
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Security;
    using System.Xml;
    using System.Xml.Serialization;
    
    using X3Platform.Configuration;
    #endregion

    /// <summary>������ͼ</summary>
    public class WebConfigurationView : XmlConfigurationView<WebConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Web.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = "Web";

        #region 静态属性::Instance
        private static volatile WebConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static WebConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new WebConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:WebConfigurationView()
        /// <summary>���캯��</summary>
        private WebConfigurationView()
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

        #region 属性:Layout
        private string m_Layout = string.Empty;

        /// <summary>������Ϣ</summary>
        public string Layout
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Layout))
                {
                    // ��������
                    string propertyName = "Layout";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_Layout = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_Layout = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_Layout))
                    {
                        this.m_Layout = "CollaborationPlatform";
                    }
                }

                return this.m_Layout;
            }
        }
        #endregion

        #region 属性:ServerName
        private string m_ServerName = string.Empty;

        /// <summary>����������</summary>
        public string ServerName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ServerName))
                {
                    // ��������
                    string propertyName = "ServerName";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_ServerName = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_ServerName = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_ServerName))
                    {
                        this.m_ServerName = "127.0.0.1";
                    }
                }

                return this.m_ServerName;
            }
        }
        #endregion

        #region 属性:SignUpUrl
        private string m_SignUpUrl = string.Empty;

        /// <summary>ע��ҳ����ַ</summary>
        public string SignUpUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignUpUrl))
                {
                    // ��������
                    string propertyName = "SignUpUrl";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_SignUpUrl = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_SignUpUrl = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_SignUpUrl))
                    {
                        this.m_SignUpUrl = "/account/signup";
                    }
                }

                return this.m_SignUpUrl;
            }
        }
        #endregion

        #region 属性:SignInUrl
        private string m_SignInUrl = string.Empty;

        /// <summary>��¼ҳ����ַ</summary>
        public string SignInUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignInUrl))
                {
                    // ��������
                    string propertyName = "SignInUrl";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_SignInUrl = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_SignInUrl = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_SignInUrl))
                    {
                        this.m_SignInUrl = "/account/signin?returnUrl={0}";
                    }
                }

                return this.m_SignInUrl;
            }
        }
        #endregion

        #region 属性:SignOutUrl
        private string m_SignOutUrl = string.Empty;

        /// <summary>�˳�ҳ����ַ</summary>
        public string SignOutUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignOutUrl))
                {
                    // ��������
                    string propertyName = "SignOutUrl";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_SignOutUrl = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_SignOutUrl = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_SignOutUrl))
                    {
                        this.m_SignOutUrl = "/account/signout";
                    }
                }

                return this.m_SignOutUrl;
            }
        }
        #endregion

        #region 属性:SiteThemeName
        private string m_SiteThemeName = null;

        /// <summary>վ������������</summary>
        public string SiteThemeName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SiteThemeName))
                {
                    // ��������
                    string propertyName = "SiteThemeName";
                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_SiteThemeName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_SiteThemeName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_SiteThemeName))
                    {
                        this.m_SiteThemeName = "default";
                    }
                }

                return this.m_SiteThemeName;
            }
        }
        #endregion

        #region 属性:RenderHeadTemplatePath
        private string m_RenderHeadTemplatePath = string.Empty;

        /// <summary>[ͷ��]��������ģ��</summary>
        public string RenderHeadTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RenderHeadTemplatePath))
                {
                    // ��������
                    string propertyName = "RenderHeadTemplatePath";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_RenderHeadTemplatePath = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_RenderHeadTemplatePath = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_RenderHeadTemplatePath))
                    {
                        this.m_RenderHeadTemplatePath = "sites/default/head.vm";
                    }
                }

                return this.m_RenderHeadTemplatePath;
            }
        }
        #endregion

        #region 属性:RenderBodyTemplatePath
        private string m_RenderBodyTemplatePath = string.Empty;

        /// <summary>[����]��������ģ��</summary>
        public string RenderBodyTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RenderBodyTemplatePath))
                {
                    // ��������
                    string propertyName = "RenderBodyTemplatePath";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_RenderBodyTemplatePath = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_RenderBodyTemplatePath = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_RenderBodyTemplatePath))
                    {
                        this.m_RenderBodyTemplatePath = "sites/default/body.vm";
                    }
                }

                return this.m_RenderBodyTemplatePath;
            }
        }
        #endregion

        #region 属性:RenderTemplatePath
        private string m_RenderTemplatePath = string.Empty;

        /// <summary>[����]��������ģ��</summary>
        public string RenderTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RenderTemplatePath))
                {
                    // ��������
                    string propertyName = "RenderTemplatePath";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_RenderTemplatePath = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_RenderTemplatePath = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_RenderTemplatePath))
                    {
                        this.m_RenderTemplatePath = "sites/default/render.vm";
                    }
                }

                return this.m_RenderTemplatePath;
            }
        }
        #endregion
    }
}
