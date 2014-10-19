namespace X3Platform.AttachmentStorage.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    #endregion

    /// <summary>附件存储管理配置视图</summary>
    public class AttachmentStorageConfigurationView : XmlConfigurationView<AttachmentStorageConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.AttachmentStorage.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = AttachmentStorageConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile AttachmentStorageConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static AttachmentStorageConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AttachmentStorageConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:AttachmentStorageConfigurationView()
        /// <summary>构造函数</summary>
        private AttachmentStorageConfigurationView()
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

        #region 属性:PhysicalUploadFolder
        private string m_PhysicalUploadFolder = string.Empty;

        /// <summary>上传文件夹物理路径</summary>
        public string PhysicalUploadFolder
        {
            get
            {
                if (string.IsNullOrEmpty(m_PhysicalUploadFolder))
                {
                    // 属性名称
                    string propertyName = "PhysicalUploadFolder";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_PhysicalUploadFolder = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_PhysicalUploadFolder = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_PhysicalUploadFolder))
                    {
                        m_PhysicalUploadFolder = string.Format(@"{0}{1}\", KernelConfigurationView.Instance.ApplicationPathRoot, "uploads");
                    }
                }

                return m_PhysicalUploadFolder;
            }
        }
        #endregion

        #region 属性:PhysicalUploadFolderRule
        private string m_PhysicalUploadFolderRule = string.Empty;

        /// <summary>上传文件夹物理路径规则</summary>
        public string PhysicalUploadFolderRule
        {
            get
            {
                if (string.IsNullOrEmpty(m_PhysicalUploadFolderRule))
                {
                    // 属性名称
                    string propertyName = "PhysicalUploadFolderRule";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_PhysicalUploadFolderRule = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_PhysicalUploadFolderRule = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_PhysicalUploadFolderRule))
                    {
                        m_PhysicalUploadFolderRule = @"folder\year\quarter\month\";
                    }
                }

                return m_PhysicalUploadFolderRule;
            }
        }
        #endregion

        #region 属性:VirtualUploadFolder
        private string m_VirtualUploadFolder = string.Empty;

        /// <summary>上传文件夹虚拟路径</summary>
        public string VirtualUploadFolder
        {
            get
            {
                if (string.IsNullOrEmpty(m_VirtualUploadFolder))
                {
                    // 属性名称
                    string propertyName = "VirtualUploadFolder";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_VirtualUploadFolder = KernelConfigurationView.Instance.ReplaceKeyValue(
                                  KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_VirtualUploadFolder = KernelConfigurationView.Instance.ReplaceKeyValue(
                           this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_VirtualUploadFolder))
                    {
                        m_VirtualUploadFolder = @"/uploads/";
                    }
                }

                return m_VirtualUploadFolder;
            }
        }
        #endregion

        #region 属性:IdentityFormat
        private string m_IdentityFormat = string.Empty;

        /// <summary>生成图片名称的格式</summary>
        public string IdentityFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_IdentityFormat))
                {
                    // 属性名称
                    string propertyName = "IdentityFormat";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_IdentityFormat = KernelConfigurationView.Instance.ReplaceKeyValue(
                           KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_IdentityFormat = KernelConfigurationView.Instance.ReplaceKeyValue(
                            this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_IdentityFormat))
                    {
                        m_IdentityFormat = "guid";
                    }
                }

                return m_IdentityFormat;
            }
        }
        #endregion

        #region 属性:AnonymousUpload
        private string m_AnonymousUpload = string.Empty;

        /// <summary>匿名上传文件</summary>
        public string AnonymousUpload
        {
            get
            {
                if (string.IsNullOrEmpty(m_AnonymousUpload))
                {
                    // 属性名称
                    string propertyName = "AnonymousUpload";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_AnonymousUpload = KernelConfigurationView.Instance.ReplaceKeyValue(
                           KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_AnonymousUpload = KernelConfigurationView.Instance.ReplaceKeyValue(
                            this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_AnonymousUpload))
                    {
                        m_AnonymousUpload = "OFF";
                    }

                    m_AnonymousUpload = m_AnonymousUpload.ToUpper();
                }

                return m_AnonymousUpload;
            }
        }
        #endregion

        #region 属性:DistributedFileStorageMode
        private string m_DistributedFileStorageMode = string.Empty;

        /// <summary>分布式文件存储</summary>
        public string DistributedFileStorageMode
        {
            get
            {
                if (string.IsNullOrEmpty(m_DistributedFileStorageMode))
                {
                    // 属性名称
                    string propertyName = "DistributedFileStorageMode";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_DistributedFileStorageMode = KernelConfigurationView.Instance.ReplaceKeyValue(
                           KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_DistributedFileStorageMode = KernelConfigurationView.Instance.ReplaceKeyValue(
                            this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_DistributedFileStorageMode))
                    {
                        m_DistributedFileStorageMode = "OFF";
                    }

                    m_DistributedFileStorageMode = m_DistributedFileStorageMode.ToUpper();
                }

                return m_DistributedFileStorageMode;
            }
        }
        #endregion

        #region 属性:DefaultThumbnails
        private string m_DefaultThumbnails = string.Empty;

        /// <summary>默认缩略图</summary>
        public string DefaultThumbnails
        {
            get
            {
                if (string.IsNullOrEmpty(m_DefaultThumbnails))
                {
                    // 属性名称
                    string propertyName = "DefaultThumbnails";
                    // 属性全局名称
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        m_DefaultThumbnails = KernelConfigurationView.Instance.ReplaceKeyValue(
                           KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        m_DefaultThumbnails = KernelConfigurationView.Instance.ReplaceKeyValue(
                            this.Configuration.Keys[propertyName].Value);
                    }

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_DefaultThumbnails))
                    {
                        m_DefaultThumbnails = string.Empty;
                    }
                }

                return m_DefaultThumbnails; 
            }
        }
        #endregion
    }
}
