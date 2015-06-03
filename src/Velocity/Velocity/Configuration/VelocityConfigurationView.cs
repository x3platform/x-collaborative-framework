
using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;
using X3Platform.Util;

namespace X3Platform.Velocity.Configuration
{
    /// <summary>Velocity 的配置视图</summary>
    public class VelocityConfigurationView : XmlConfigurationView<VelocityConfiguration>
    {
        /// <summary>配置文件的默认路径.</summary>
        private const string configFile = "config\\X3Platform.Velocity.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = VelocityConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile VelocityConfigurationView instance = null;

        private static object lockObject = new object();

        public static VelocityConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new VelocityConfigurationView();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 构造函数:VelocityConfigurationView()
        /// <summary>构造函数</summary>
        private VelocityConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
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

        // -------------------------------------------------------
        // 自定义属性
        // -------------------------------------------------------

        #region 属性:TemplatePath
        private string m_TemplatePath = string.Empty;

        /// <summary>模板路径</summary>
        public string TemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_TemplatePath))
                {
                    // 读取配置信息
                    this.m_TemplatePath = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "TemplatePath",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_TemplatePath = StringHelper.NullOrEmptyTo(this.m_TemplatePath,
                        KernelConfigurationView.Instance.ApplicationPathRoot + "views\\templates");

                    if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        this.m_TemplatePath = this.m_TemplatePath.Replace("\\", "/");
                    }
                }

                return this.m_TemplatePath;
            }
        }
        #endregion

        #region 属性:TemplateCacheMode
        private string m_TemplateCacheMode = string.Empty;

        /// <summary>模板缓存状态</summary>
        public string TemplateCacheMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_TemplateCacheMode))
                {
                    // 读取配置信息
                    this.m_TemplateCacheMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "TemplateCacheMode",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_TemplateCacheMode = StringHelper.NullOrEmptyTo(this.m_TemplateCacheMode, "Off");

                    this.m_TemplateCacheMode = this.m_TemplateCacheMode.ToUpper();
                }

                return this.m_TemplateCacheMode;
            }
        }
        #endregion
    }
}