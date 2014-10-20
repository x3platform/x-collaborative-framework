namespace X3Platform.Storages.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    #endregion

    /// <summary>应用存储配置视图</summary>
    public class StoragesConfigurationView : XmlConfigurationView<StoragesConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Storages.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = StoragesConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile StoragesConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static StoragesConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new StoragesConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:StoragesConfigurationView()
        /// <summary>���캯��</summary>
        private StoragesConfigurationView()
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
        // 自定义属性
        // -------------------------------------------------------

    }
}
