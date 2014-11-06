#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FilenName    :FavoriteConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Favorite.Configuration
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.IO;

    using X3Platform.Configuration;

    using X3Platform.Configuration;
    #endregion

    /// <summary>配置视图</summary>
    public class FavoriteConfigurationView : XmlConfigurationView<FavoriteConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Plugins.Favorite.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = FavoriteConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile FavoriteConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static FavoriteConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new FavoriteConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:FavoriteConfigurationView()
        /// <summary>构造函数</summary>
        private FavoriteConfigurationView()
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

        //-------------------------------------------------------
        // 自定义属性
        //-------------------------------------------------------

        #region 属性:DataSourceName
        private string m_DataSourceName = string.Empty;

        /// <summary>数据源名称</summary>
        public string DataSourceName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DataSourceName))
                {
                    // 属性名称
                    string propertyName = "DataSourceName";

                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", FavoriteConfiguration.ApplicationName, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_DataSourceName = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_DataSourceName = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里未设置则设置一个默认值
                    if (string.IsNullOrEmpty(this.m_DataSourceName))
                    {
                        this.m_DataSourceName = "#";
                    }
                }

                return this.m_DataSourceName;
            }
        }
        #endregion
    }
}
