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
                if (string.IsNullOrEmpty(this.m_PhysicalUploadFolder))
                {
                    this.m_PhysicalUploadFolder = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PhysicalUploadFolder",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(m_PhysicalUploadFolder))
                    {
                        this.m_PhysicalUploadFolder = string.Format(@"{0}{1}\", KernelConfigurationView.Instance.ApplicationPathRoot, "uploads");
                    }
                }

                return this.m_PhysicalUploadFolder;
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
                    this.m_PhysicalUploadFolderRule = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PhysicalUploadFolderRule",
                        this.Configuration.Keys);

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
                    this.m_VirtualUploadFolder = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "VirtualUploadFolder",
                        this.Configuration.Keys);

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

        /// <summary>生成文件名称的格式</summary>
        public string IdentityFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_IdentityFormat))
                {
                    this.m_IdentityFormat = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "IdentityFormat",
                        this.Configuration.Keys);

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

        #region 属性:AllowMaxFileSize
        private int m_AllowMaxFileSize = -1;

        /// <summary>允许上传的最大文件大小 单位(MB)</summary>
        public int AllowMaxFileSize
        {
            get
            {
                if (this.m_AllowMaxFileSize == -1)
                {
                    this.m_AllowMaxFileSize = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AllowMaxFileSize",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_AllowMaxFileSize <= 0)
                    {
                        this.m_AllowMaxFileSize = int.MaxValue;
                    }
                }

                return this.m_AllowMaxFileSize;
            }
        }
        #endregion

        #region 属性:AllowMinFileSize
        private int m_AllowMinFileSize = -1;

        /// <summary>允许上传的最小文件大小 单位(MB)</summary>
        public int AllowMinFileSize
        {
            get
            {
                if (this.m_AllowMinFileSize == -1)
                {
                    this.m_AllowMinFileSize = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AllowMinFileSize",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_AllowMinFileSize < 0)
                    {
                        this.m_AllowMinFileSize = 0;
                    }
                }

                return this.m_AllowMinFileSize;
            }
        }
        #endregion

        #region 属性:AllowFileTypes
        private string m_AllowFileTypes = string.Empty;

        /// <summary>允许文件的类型</summary>
        public string AllowFileTypes
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AllowFileTypes))
                {
                    this.m_AllowFileTypes = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "AllowFileTypes",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(this.m_AllowFileTypes))
                    {
                        this.m_AllowFileTypes = "";
                    }
                }

                return this.m_AllowFileTypes;
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
                    this.m_DistributedFileStorageMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DistributedFileStorageMode",
                        this.Configuration.Keys);

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
        private string m_DefaultThumbnails = null;

        /// <summary>默认缩略图</summary>
        public string DefaultThumbnails
        {
            get
            {
                if (string.IsNullOrEmpty(m_DefaultThumbnails))
                {
                    this.m_DefaultThumbnails = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "DefaultThumbnails",
                        this.Configuration.Keys);

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
