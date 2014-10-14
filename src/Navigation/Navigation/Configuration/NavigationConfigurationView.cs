
namespace X3Platform.Navigation.Configuration
{
    using System;
    using System.IO;
    using X3Platform.Configuration;

    /// <summary>������ͼ</summary>
    public class NavigationConfigurationView : XmlConfigurationView<NavigationConfiguration>
    {
        private const string configFile = "config\\X3Platform.Navigation.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = NavigationConfiguration.ApplicationName;

        #region ��̬����:Instance
        private static volatile NavigationConfigurationView instance = null;

        private static object lockObject = new object();

        public static NavigationConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new NavigationConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:NavigationConfigurationView()
        /// <summary>���캯��</summary>
        private NavigationConfigurationView()
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
