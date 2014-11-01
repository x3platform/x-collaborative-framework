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
    using X3Platform.Util;
    #endregion

    /// <summary>流水号配置视图</summary>
    public class WebConfigurationView : XmlConfigurationView<WebConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Web.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = "Web";

        #region 静态属性:Instance
        private static volatile WebConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
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

        #region 构造函数:WebConfigurationView()
        /// <summary>构造函数</summary>
        private WebConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 将配置信息加载到全局的配置中
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
        // 自定义属性
        // -------------------------------------------------------

        #region 属性:ServerName
        private string m_ServerName = string.Empty;

        /// <summary>服务器名称: 当服务器不以域名方式访问时, 必须需要指定服务器IP地址.</summary>
        public string ServerName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ServerName))
                {
                    // 读取配置信息
                    this.m_ServerName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "ServerName", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_ServerName = StringHelper.NullOrEmptyTo(this.m_ServerName, "127.0.0.1");

                    if (!this.m_ServerName.EndsWith(";"))
                    {
                        this.m_ServerName = this.m_ServerName + ";";
                    }
                }

                return this.m_ServerName;
            }
        }
        #endregion

        #region 属性:Layout
        private string m_Layout = string.Empty;

        /// <summary>网站页面的布局</summary>
        public string Layout
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Layout))
                {
                    // 读取配置信息
                    this.m_Layout = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "Layout", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_Layout = StringHelper.NullOrEmptyTo(this.m_Layout, "CollaborationPlatform");
                }

                return this.m_Layout;
            }
        }
        #endregion

        #region 属性:SiteThemeName
        private string m_SiteThemeName = null;

        /// <summary>网站页面的主题名称 default | dynamic </summary>
        public string SiteThemeName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SiteThemeName))
                {
                    // 读取配置信息
                    this.m_SiteThemeName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SiteThemeName", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_SiteThemeName = StringHelper.NullOrEmptyTo(this.m_SiteThemeName, "default");
                }

                return this.m_SiteThemeName;
            }
        }
        #endregion

        #region 属性:RenderTemplatePath
        private string m_RenderTemplatePath = string.Empty;

        /// <summary>网站页面的整体输出模板</summary>
        public string RenderTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RenderTemplatePath))
                {
                    // 读取配置信息
                    this.m_RenderTemplatePath = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "RenderTemplatePath", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_RenderTemplatePath = StringHelper.NullOrEmptyTo(this.m_RenderTemplatePath, "sites/default/render.vm");
                }

                return this.m_RenderTemplatePath;
            }
        }
        #endregion

        #region 属性:RenderHeadTemplatePath
        private string m_RenderHeadTemplatePath = string.Empty;

        /// <summary>网站页面的头部模板</summary>
        public string RenderHeadTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RenderHeadTemplatePath))
                {
                    // 读取配置信息
                    this.m_RenderHeadTemplatePath = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "RenderHeadTemplatePath", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_RenderHeadTemplatePath = StringHelper.NullOrEmptyTo(this.m_RenderHeadTemplatePath, "sites/default/head.vm");
                }

                return this.m_RenderHeadTemplatePath;
            }
        }
        #endregion

        #region 属性:RenderBodyTemplatePath
        private string m_RenderBodyTemplatePath = string.Empty;

        /// <summary>网站页面的主体模板</summary>
        public string RenderBodyTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RenderBodyTemplatePath))
                {
                    // 读取配置信息
                    this.m_RenderBodyTemplatePath = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "RenderBodyTemplatePath", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_RenderBodyTemplatePath = StringHelper.NullOrEmptyTo(this.m_RenderBodyTemplatePath, "sites/default/body.vm");
                }

                return this.m_RenderBodyTemplatePath;
            }
        }
        #endregion

        #region 属性:SignUpUrl
        private string m_SignUpUrl = string.Empty;

        /// <summary>注册页面地址</summary>
        public string SignUpUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignUpUrl))
                {
                    // 读取配置信息
                    this.m_SignUpUrl = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SignUpUrl", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_SignUpUrl = StringHelper.NullOrEmptyTo(this.m_SignUpUrl, "/account/signup");
                }

                return this.m_SignUpUrl;
            }
        }
        #endregion

        #region 属性:SignInUrl
        private string m_SignInUrl = string.Empty;

        /// <summary>登录页面地址</summary>
        public string SignInUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignInUrl))
                {
                    // 读取配置信息
                    this.m_SignInUrl = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SignInUrl", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_SignInUrl = StringHelper.NullOrEmptyTo(this.m_SignInUrl, "/account/signin?returnUrl={0}");
                }

                return this.m_SignInUrl;
            }
        }
        #endregion

        #region 属性:SignOutUrl
        private string m_SignOutUrl = string.Empty;

        /// <summary>退出页面地址</summary>
        public string SignOutUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignOutUrl))
                {
                    // 读取配置信息
                    this.m_SignOutUrl = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SignOutUrl", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_SignOutUrl = StringHelper.NullOrEmptyTo(this.m_SignOutUrl, "/account/signout");
                }

                return this.m_SignOutUrl;
            }
        }
        #endregion

        #region 属性:UploadWizardUrl
        private string m_UploadWizardUrl = string.Empty;

        /// <summary>上传文件向导地址</summary>
        public string UploadWizardUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_UploadWizardUrl))
                {
                    // 读取配置信息
                    this.m_UploadWizardUrl = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "UploadWizardUrl", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_UploadWizardUrl = StringHelper.NullOrEmptyTo(this.m_SignOutUrl, "/files/upload-wizard");
                }

                return this.m_UploadWizardUrl;
            }
        }
        #endregion
    }
}
