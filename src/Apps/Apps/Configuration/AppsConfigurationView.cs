#region Using Libraries
using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;
using X3Platform.Util;
#endregion

namespace X3Platform.Apps.Configuration
{
    /// <summary>应用配置视图</summary>
    public class AppsConfigurationView : XmlConfigurationView<AppsConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Apps.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = "Apps";

        #region 静态属性:Instance
        private static volatile AppsConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static AppsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AppsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:AppsConfigurationView()
        /// <summary>构造函数</summary>
        private AppsConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
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

        #region 属性:Administrators
        private string m_Administrators = string.Empty;

        /// <summary>系统超级管理员信息</summary>
        public string Administrators
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Administrators))
                {
                    // 属性名称
                    string propertyName = "Administrators";

                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_Administrators = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_Administrators = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(this.m_Administrators))
                    {
                        this.m_Administrators = "#";
                    }
                }

                return this.m_Administrators;
            }
        }
        #endregion

        #region 属性:HiddenStartMenu
        private string m_HiddenStartMenu = string.Empty;

        /// <summary>隐藏开始菜单</summary>
        public string HiddenStartMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HiddenStartMenu))
                {
                    // 读取配置信息
                    this.m_HiddenStartMenu = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "HiddenStartMenu",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_HiddenStartMenu = StringHelper.NullOrEmptyTo(this.m_HiddenStartMenu, "Off");

                    this.m_HiddenStartMenu = this.m_HiddenStartMenu.ToUpper();
                }

                return this.m_HiddenStartMenu;
            }
        }
        #endregion

        #region 属性:HiddenTopMenu
        private string m_HiddenTopMenu = string.Empty;

        /// <summary>隐藏顶部菜单</summary>
        public string HiddenTopMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HiddenTopMenu))
                {
                    // 读取配置信息
                    this.m_HiddenTopMenu = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "HiddenTopMenu",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_HiddenTopMenu = StringHelper.NullOrEmptyTo(this.m_HiddenTopMenu, "Off");

                    this.m_HiddenTopMenu = this.m_HiddenTopMenu.ToUpper();
                }

                return this.m_HiddenTopMenu;
            }
        }
        #endregion

        #region 属性:HiddenShortcutMenu
        private string m_HiddenShortcutMenu = string.Empty;

        /// <summary>隐藏快捷方式菜单</summary>
        public string HiddenShortcutMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HiddenShortcutMenu))
                {
                    // 读取配置信息
                    this.m_HiddenShortcutMenu = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "HiddenShortcutMenu",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_HiddenShortcutMenu = StringHelper.NullOrEmptyTo(this.m_HiddenShortcutMenu, "Off");

                    this.m_HiddenShortcutMenu = this.m_HiddenShortcutMenu.ToUpper();
                }

                return this.m_HiddenShortcutMenu;
            }
        }
        #endregion
    }
}
