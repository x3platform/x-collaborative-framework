namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.DigitalNumber;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.IBLL;
    using X3Platform.AttachmentStorage.Util;
    #endregion

    /// <summary>附件上下文环境</summary>
    public sealed class AttachmentStorageContext : CustomPlugin
    {
        #region 静态属性:Instance
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

        #region 属性:Name
        public override string Name
        {
            get { return "附件存储"; }
        }
        #endregion

        #region 属性:AttachmentFileService
        private IAttachmentFileService m_AttachmentFileService = null;

        public IAttachmentFileService AttachmentFileService
        {
            get { return this.m_AttachmentFileService; }
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

        #region 构造函数:AttachmentStorageContext()
        /// <summary>构造函数</summary>
        private AttachmentStorageContext()
        {
            this.Restart();
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
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

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // 重新加载配置信息
                AttachmentStorageConfigurationView.Instance.Reload();
            }

            // 创建对象构建器(Spring.NET)
            string springObjectFile = AttachmentStorageConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_AttachmentFileService = objectBuilder.GetObject<IAttachmentFileService>(typeof(IAttachmentFileService));
            this.m_AttachmentDistributedFileService = objectBuilder.GetObject<IAttachmentDistributedFileService>(typeof(IAttachmentDistributedFileService));
            this.m_AttachmentWarnService = objectBuilder.GetObject<IAttachmentWarnService>(typeof(IAttachmentWarnService));
        }
        #endregion
    }
}
