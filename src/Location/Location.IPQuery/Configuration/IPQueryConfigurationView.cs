namespace X3Platform.Location.IPQuery.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>配置视图</summary>
    public class IPQueryConfigurationView : XmlConfigurationView<IPQueryConfiguration>
    {
        private const string configFile = "config\\X3Platform.Location.IPQuery.config";

        #region 静态属性:Instance
        private static volatile IPQueryConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
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

        #region 构造函数:IPQueryConfigurationView()
        /// <summary>构造函数</summary>
        private IPQueryConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion
    }
}
