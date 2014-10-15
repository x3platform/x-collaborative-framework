namespace X3Platform.Location.IPQuery.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>������ͼ</summary>
    public class IPQueryConfigurationView : XmlConfigurationView<IPQueryConfiguration>
    {
        private const string configFile = "config\\X3Platform.Location.IPQuery.config";

        #region ��̬����:Instance
        private static volatile IPQueryConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static IPQueryConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                    instance = new IPQueryConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:IPQueryConfigurationView()
        /// <summary>���캯��</summary>
        private IPQueryConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion
    }
}
