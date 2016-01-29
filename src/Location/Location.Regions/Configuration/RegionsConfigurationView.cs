namespace X3Platform.Location.Regions.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>������ͼ</summary>
    public class RegionsConfigurationView : XmlConfigurationView<RegionsConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Location.Regions.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = RegionsConfiguration.ApplicationName;

        #region ��̬����:Instance
        private static volatile RegionsConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static RegionsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new RegionsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:RegionsConfigurationView()
        /// <summary>���캯��</summary>
        private RegionsConfigurationView()
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

        // -------------------------------------------------------
        // �Զ�������
        // -------------------------------------------------------

        #region ����:DB_CONFIG
        private string m_DB_CONFIG = string.Empty;

        /// <summary>�Ѻõĵ�ַ��д</summary>
        public string DB_CONFIG
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DB_CONFIG))
                {
                    // ��ȡ����ȫ����Ϣ
                    this.m_DB_CONFIG = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DB_CONFIG", this.Configuration.Keys);

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_DB_CONFIG))
                    {
                        this.m_DB_CONFIG = "#";
                    }

                    this.m_DB_CONFIG = this.m_DB_CONFIG.ToUpper();
                }

                return this.m_DB_CONFIG;
            }
        }
        #endregion
    }
}
