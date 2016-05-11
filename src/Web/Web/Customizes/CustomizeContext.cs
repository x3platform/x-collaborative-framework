namespace X3Platform.Web.Customizes
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;
    using X3Platform.Web.Customizes.IBLL;
    using X3Platform.Web.Configuration;
    using X3Platform.Globalization;

    /// <summary>页面自定义</summary>
    public sealed class CustomizeContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "页面自定义"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile CustomizeContext instance = null;

        private static object lockObject = new object();

        public static CustomizeContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new CustomizeContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private WebConfiguration configuration = null;

        /// <summary>配置</summary>
        public WebConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:CustomizePageService
        private ICustomizePageService m_CustomizePageService = null;

        public ICustomizePageService CustomizePageService
        {
            get { return m_CustomizePageService; }
        }
        #endregion

        #region 属性:CustomizeContentService
        private ICustomizeContentService m_CustomizeContentService = null;

        public ICustomizeContentService CustomizeContentService
        {
            get { return m_CustomizeContentService; }
        }
        #endregion

        #region 属性:CustomizeLayoutService
        private ICustomizeLayoutService m_CustomizeLayoutService = null;

        public ICustomizeLayoutService CustomizeLayoutService
        {
            get { return this.m_CustomizeLayoutService; }
        }
        #endregion

        #region 属性:CustomizeWidgetService
        private ICustomizeWidgetService m_CustomizeWidgetService = null;

        public ICustomizeWidgetService CustomizeWidgetService
        {
            get { return m_CustomizeWidgetService; }
        }
        #endregion

        #region 属性:CustomizeWidgetInstanceService
        private ICustomizeWidgetInstanceService m_CustomizeWidgetInstanceService = null;

        public ICustomizeWidgetInstanceService CustomizeWidgetInstanceService
        {
            get { return m_CustomizeWidgetInstanceService; }
        }
        #endregion

        #region 构造函数:CustomizeContext()
        /// <summary>构造函数</summary>
        private CustomizeContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

            this.m_CustomizePageService = objectBuilder.GetObject<ICustomizePageService>(typeof(ICustomizePageService));
            this.m_CustomizeContentService = objectBuilder.GetObject<ICustomizeContentService>(typeof(ICustomizeContentService));
            this.m_CustomizeLayoutService = objectBuilder.GetObject<ICustomizeLayoutService>(typeof(ICustomizeLayoutService));
            this.m_CustomizeWidgetService = objectBuilder.GetObject<ICustomizeWidgetService>(typeof(ICustomizeWidgetService));
            this.m_CustomizeWidgetInstanceService = objectBuilder.GetObject<ICustomizeWidgetInstanceService>(typeof(ICustomizeWidgetInstanceService));

            KernelContext.Log.Info(string.Format(I18n.Strings["application_is_successfully_loaded"], WebConfiguration.APP_NAME_CUSTOMIZES));
        }
        #endregion
    }
}