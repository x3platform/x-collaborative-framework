namespace X3Platform.Location.Regions
{
    #region Using Libraries
    using System;
    using System.Reflection;

    using X3Platform.DigitalNumber;
    using X3Platform.Logging;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Location.Regions.Configuration;
    using X3Platform.Location.Regions.IBLL;
    #endregion

    /// <summary>区域管理上下文环境</summary>
    public sealed class RegionContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "BackendManagement"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile RegionContext instance = null;

        private static object lockObject = new object();

        public static RegionContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new RegionContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:RegionService
        private IRegionService m_RegionService = null;

        public IRegionService RegionService
        {
            get { return m_RegionService; }
        }
        #endregion

        #region 构造函数:RegionContext()
        /// <summary><see cref="RegionContext"/>的构造函数</summary>
        private RegionContext()
        {
            Restart();
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            this.Reload();

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
                RegionsConfigurationView.Instance.Reload();
            }

            // 创建对象构建器(Spring.NET)
            string springObjectFile = RegionsConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(RegionsConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_RegionService = objectBuilder.GetObject<IRegionService>(typeof(IRegionService));
        }
        #endregion
    }
}
