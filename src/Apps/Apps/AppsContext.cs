namespace X3Platform.Apps
{
    #region Using Libraries
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using Common.Logging;
    #endregion

    /// <summary>应用上下文环境</summary>
    public class AppsContext : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 静态属性:Instance
        private static volatile AppsContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static AppsContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AppsContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        public override string Name
        {
            get { return "应用管理"; }
        }
        #endregion

        #region 属性:Configuration
        private AppsConfiguration configuration = null;

        /// <summary>配置</summary>
        public AppsConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:ApplicationService
        private IApplicationService m_ApplicationService;

        /// <summary>应用服务</summary>
        public IApplicationService ApplicationService
        {
            get { return m_ApplicationService; }
        }
        #endregion

        #region 属性:ApplicationErrorService
        private IApplicationErrorService m_ApplicationErrorService;

        /// <summary>应用错误服务</summary>
        public IApplicationErrorService ApplicationErrorService
        {
            get { return m_ApplicationErrorService; }
        }
        #endregion

        #region 属性:ApplicationEventService
        private IApplicationEventService m_ApplicationEventService;

        /// <summary>应用事件服务</summary>
        public IApplicationEventService ApplicationEventService
        {
            get { return m_ApplicationEventService; }
        }
        #endregion

        #region 属性:ApplicationFeatureService
        private IApplicationFeatureService m_ApplicationFeatureService;

        /// <summary>应用功能服务</summary>
        public IApplicationFeatureService ApplicationFeatureService
        {
            get { return m_ApplicationFeatureService; }
        }
        #endregion

        #region 属性:ApplicationFeatureDateLimitService
        private IApplicationFeatureDateLimitService m_ApplicationFeatureDateLimitService;

        /// <summary>应用功能时间限制服务</summary>
        public IApplicationFeatureDateLimitService ApplicationFeatureDateLimitService
        {
            get { return m_ApplicationFeatureDateLimitService; }
        }
        #endregion

        #region 属性:ApplicationSettingGroupService
        private IApplicationSettingGroupService m_ApplicationSettingGroupService;

        /// <summary>应用配置服务分组</summary>
        public IApplicationSettingGroupService ApplicationSettingGroupService
        {
            get { return m_ApplicationSettingGroupService; }
        }
        #endregion

        #region 属性:ApplicationSettingService
        private IApplicationSettingService m_ApplicationSettingService;

        /// <summary>应用配置服务</summary>
        public IApplicationSettingService ApplicationSettingService
        {
            get { return m_ApplicationSettingService; }
        }
        #endregion

        #region 属性:ApplicationMenuService
        private IApplicationMenuService m_ApplicationMenuService;

        /// <summary>应用菜单维度服务</summary>
        public IApplicationMenuService ApplicationMenuService
        {
            get { return m_ApplicationMenuService; }
        }
        #endregion

        #region 属性:ApplicationMethodService
        private IApplicationMethodService m_ApplicationMethodService;

        /// <summary>应用菜单项服务</summary>
        public IApplicationMethodService ApplicationMethodService
        {
            get { return m_ApplicationMethodService; }
        }
        #endregion

        #region 属性:ApplicationOptionService
        private IApplicationOptionService m_ApplicationOptionService;

        /// <summary>应用选项服务</summary>
        public IApplicationOptionService ApplicationOptionService
        {
            get { return m_ApplicationOptionService; }
        }
        #endregion

        #region 构造函数:AppsContext()
        /// <summary>构造函数</summary>
        private AppsContext()
        {
            this.Restart();
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回服务. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();

                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
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
                AppsConfigurationView.Instance.Reload();
            }

            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_ApplicationService = objectBuilder.GetObject<IApplicationService>(typeof(IApplicationService));
            this.m_ApplicationOptionService = objectBuilder.GetObject<IApplicationOptionService>(typeof(IApplicationOptionService));
            this.m_ApplicationErrorService = objectBuilder.GetObject<IApplicationErrorService>(typeof(IApplicationErrorService));
            this.m_ApplicationEventService = objectBuilder.GetObject<IApplicationEventService>(typeof(IApplicationEventService));
            this.m_ApplicationFeatureService = objectBuilder.GetObject<IApplicationFeatureService>(typeof(IApplicationFeatureService));
            this.m_ApplicationFeatureDateLimitService = objectBuilder.GetObject<IApplicationFeatureDateLimitService>(typeof(IApplicationFeatureDateLimitService));
            this.m_ApplicationSettingService = objectBuilder.GetObject<IApplicationSettingService>(typeof(IApplicationSettingService));
            this.m_ApplicationSettingGroupService = objectBuilder.GetObject<IApplicationSettingGroupService>(typeof(IApplicationSettingGroupService));
            this.m_ApplicationMenuService = objectBuilder.GetObject<IApplicationMenuService>(typeof(IApplicationMenuService));
            this.m_ApplicationMethodService = objectBuilder.GetObject<IApplicationMethodService>(typeof(IApplicationMethodService));
        }
        #endregion
    }
}
