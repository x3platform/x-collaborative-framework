namespace X3Platform.AttachmentStorage.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;
    
    /// <summary>������ͼ</summary>
    public class AttachmentStorageConfigurationView : XmlConfigurationView<AttachmentStorageConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��.</summary>
        private const string configFile = "config\\X3Platform.AttachmentStorage.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = AttachmentStorageConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile AttachmentStorageConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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

        #region ���캯��:AttachmentStorageConfigurationView()
        /// <summary>���캯��</summary>
        private AttachmentStorageConfigurationView()
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

        #region 属性:PhysicalUploadFolder
        private string m_PhysicalUploadFolder = string.Empty;

        /// <summary>�ϴ��ļ�������·��</summary>
        public string PhysicalUploadFolder
        {
            get
            {
                if (string.IsNullOrEmpty(m_PhysicalUploadFolder))
                {
                    // ��������
                    string propertyName = "PhysicalUploadFolder";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>�ϴ��ļ�������·������</summary>
        public string PhysicalUploadFolderRule
        {
            get
            {
                if (string.IsNullOrEmpty(m_PhysicalUploadFolderRule))
                {
                    // ��������
                    string propertyName = "PhysicalUploadFolderRule";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>�ϴ��ļ�������·��</summary>
        public string VirtualUploadFolder
        {
            get
            {
                if (string.IsNullOrEmpty(m_VirtualUploadFolder))
                {
                    // ��������
                    string propertyName = "VirtualUploadFolder";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>����ͼƬ���Ƶĸ�ʽ</summary>
        public string IdentityFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_IdentityFormat))
                {
                    // ��������
                    string propertyName = "IdentityFormat";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>�����ϴ��ļ�</summary>
        public string AnonymousUpload
        {
            get
            {
                if (string.IsNullOrEmpty(m_AnonymousUpload))
                {
                    // ��������
                    string propertyName = "AnonymousUpload";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>�ֲ�ʽ�ļ��洢</summary>
        public string DistributedFileStorageMode
        {
            get
            {
                if (string.IsNullOrEmpty(m_DistributedFileStorageMode))
                {
                    // ��������
                    string propertyName = "DistributedFileStorageMode";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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

        /// <summary>Ĭ������ͼ</summary>
        public string DefaultThumbnails
        {
            get
            {
                if (string.IsNullOrEmpty(m_DefaultThumbnails))
                {
                    // ��������
                    string propertyName = "DefaultThumbnails";
                    // ����ȫ������
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

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
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
