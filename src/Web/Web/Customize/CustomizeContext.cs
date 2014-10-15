namespace X3Platform.Web.Customize
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;
    using X3Platform.Web.Customize.IBLL;
    using X3Platform.Web.Configuration;

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

        #region 属性:PageService
        private IPageService m_PageService = null;

        public IPageService PageService
        {
            get { return m_PageService; }
        }
        #endregion

        #region 属性:WidgetService
        private IWidgetService m_WidgetService = null;

        public IWidgetService WidgetService
        {
            get { return m_WidgetService; }
        }
        #endregion

        #region 属性:WidgetInstanceService
        private IWidgetInstanceService m_WidgetInstanceService = null;

        public IWidgetInstanceService WidgetInstanceService
        {
            get { return m_WidgetInstanceService; }
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
                Reload();
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
            configuration = WebConfigurationView.Instance.Configuration;

            m_PageService = SpringContext.Instance.GetObject<IPageService>(typeof(IPageService));
            m_WidgetService = SpringContext.Instance.GetObject<IWidgetService>(typeof(IWidgetService));
            m_WidgetInstanceService = SpringContext.Instance.GetObject<IWidgetInstanceService>(typeof(IWidgetInstanceService));
        }
        #endregion
    }
}