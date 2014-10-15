namespace X3Platform.Entities.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>������ͼ</summary>
    public class EntitiesConfigurationView : XmlConfigurationView<EntitiesConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��.</summary>
        private const string configFile = "config\\X3Platform.Entities.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = EntitiesConfiguration.ApplicationName;

        #region ��̬����:Instance
        private static volatile EntitiesConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static EntitiesConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new EntitiesConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:EntitiesConfigurationView()
        /// <summary>���캯��</summary>
        private EntitiesConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region ����:Reload()
        /// <summary>���¼���������Ϣ</summary>
        public override void Reload()
        {
            base.Reload();

            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion
    }
}
