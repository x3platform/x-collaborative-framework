namespace X3Platform.Spring.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    #endregion

    /// <summary>配置视图</summary>
    public class SpringConfigurationView : XmlConfigurationView<SpringConfiguration>
    {
        /// <summary>配置文件的默认路径.</summary>
        private const string configFile = "config\\X3Platform.Spring.config";

        #region 静态属性:Instance
        private static volatile SpringConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static SpringConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SpringConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:SpringConfigurationView()
        /// <summary>构造函数</summary>
        private SpringConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 基类初始化后会默认执行 Reload() 函数
        }
        #endregion

        #region 构造函数:SpringConfigurationView(string path)
        /// <summary>构造函数</summary>
        private SpringConfigurationView(string path)
            : base(path)
        {
            // 基类初始化后会默认执行 Reload() 函数
        }
        #endregion

        #region 构造函数:LoadInstance(string fullConfigPath)
        /// <summary>通过配置文件载入实例</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new SpringConfigurationView(fullConfigPath);
        }
        #endregion
    }
}
