// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.AttachmentStorage
{
    using System;
    using System.IO;

    using X3Platform.DigitalNumber;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.IBLL;
    using X3Platform.AttachmentStorage.Util;

    /// <summary>���������Ļ���</summary>
    public sealed class AttachmentStorageContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "�����洢"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile AttachmentStorageContext instance = null;

        private static object lockObject = new object();

        public static AttachmentStorageContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AttachmentStorageContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private AttachmentStorageConfiguration configuration = null;

        /// <summary>����</summary>
        public AttachmentStorageConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:AttachmentStorageService
        private IAttachmentStorageService m_AttachmentStorageService = null;

        public IAttachmentStorageService AttachmentStorageService
        {
            get { return m_AttachmentStorageService; }
        }
        #endregion

        #region 属性:AttachmentDistributedFileService
        private IAttachmentDistributedFileService m_AttachmentDistributedFileService = null;

        public IAttachmentDistributedFileService AttachmentDistributedFileService
        {
            get { return m_AttachmentDistributedFileService; }
        }
        #endregion

        #region 属性:AttachmentWarnService
        private IAttachmentWarnService m_AttachmentWarnService = null;

        public IAttachmentWarnService AttachmentWarnService
        {
            get { return this.m_AttachmentWarnService; }
        }
        #endregion

        #region ���캯��:AttachmentStorageContext()
        /// <summary>���캯��</summary>
        private AttachmentStorageContext()
        {
            this.Restart();
        }
        #endregion

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();
            }
            catch
            {
                throw;
            }

            return 0;
        }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            this.configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // �������ݷ�������
            this.m_AttachmentStorageService = objectBuilder.GetObject<IAttachmentStorageService>(typeof(IAttachmentStorageService));
            this.m_AttachmentDistributedFileService = objectBuilder.GetObject<IAttachmentDistributedFileService>(typeof(IAttachmentDistributedFileService));
            this.m_AttachmentWarnService = objectBuilder.GetObject<IAttachmentWarnService>(typeof(IAttachmentWarnService));
        }
        #endregion

        #region 属性:CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        /// <summary></summary>
        /// <param name="entityId"></param>
        /// <param name="entityClassName"></param>
        /// <param name="attachmentEntityClassName"></param>
        /// <param name="attachmentFolder"></param>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public IAttachmentFileInfo CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        {
            IAttachmentParentObject parent = new AttachmentParentObject(entityId, entityClassName, attachmentEntityClassName, attachmentFolder);

            IAttachmentFileInfo attachment = UploadFileHelper.CreateAttachmentFile(parent, fileName, Path.GetExtension(fileName), fileData.Length, fileData);

            attachment.Save();

            return attachment;
        }
        #endregion
    }
}
