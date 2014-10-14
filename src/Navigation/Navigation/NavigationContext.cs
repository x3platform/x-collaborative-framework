namespace X3Platform.Navigation
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Navigation.Configuration;
    using X3Platform.Navigation.IBLL;

    public sealed class NavigationContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "导航"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile NavigationContext instance = null;

        private static object lockObject = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static NavigationContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new NavigationContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private NavigationConfiguration configuration = null;

        /// <summary>配置</summary>
        public NavigationConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:NavigationPortalService
        private INavigationPortalService m_NavigationPortalService;

        /// <summary>门户</summary>
        public INavigationPortalService NavigationPortalService
        {
            get { return m_NavigationPortalService; }
        }
        #endregion

        #region 属性:NavigationPortalTopMenuService
        private INavigationPortalTopMenuService m_NavigationPortalTopMenuService;

        /// <summary>顶部菜单</summary>
        public INavigationPortalTopMenuService NavigationPortalTopMenuService
        {
            get { return m_NavigationPortalTopMenuService; }
        }
        #endregion

        #region 属性:NavigationPortalShortcutGroupService
        private INavigationPortalShortcutGroupService m_NavigationPortalShortcutGroupService;

        /// <summary>快捷方式分组</summary>
        public INavigationPortalShortcutGroupService NavigationPortalShortcutGroupService
        {
            get { return m_NavigationPortalShortcutGroupService; }
        }
        #endregion

        #region 属性:NavigationPortalShortcutService
        private INavigationPortalShortcutService m_NavigationPortalShortcutService;

        /// <summary>快捷方式</summary>
        public INavigationPortalShortcutService NavigationPortalShortcutService
        {
            get { return m_NavigationPortalShortcutService; }
        }
        #endregion

        #region 属性:NavigationPortalSidebarItemGroupService
        private INavigationPortalSidebarItemGroupService m_NavigationPortalSidebarItemGroupService;

        /// <summary>侧栏菜单项分组</summary>
        public INavigationPortalSidebarItemGroupService NavigationPortalSidebarItemGroupService
        {
            get { return m_NavigationPortalSidebarItemGroupService; }
        }
        #endregion

        #region 属性:NavigationPortalSidebarItemService
        private INavigationPortalSidebarItemService m_NavigationPortalSidebarItemService;

        /// <summary>侧栏菜单项</summary>
        public INavigationPortalSidebarItemService NavigationPortalSidebarItemService
        {
            get { return m_NavigationPortalSidebarItemService; }
        }
        #endregion

        #region 构造函数:NavigationContext()
        /// <summary>构造函数</summary>
        private NavigationContext()
        {
            Reload();
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
            catch
            {
                throw;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            configuration = NavigationConfigurationView.Instance.Configuration;

            this.m_NavigationPortalService = SpringContext.Instance.GetObject<INavigationPortalService>(typeof(INavigationPortalService));
            this.m_NavigationPortalTopMenuService = SpringContext.Instance.GetObject<INavigationPortalTopMenuService>(typeof(INavigationPortalTopMenuService));
            this.m_NavigationPortalShortcutGroupService = SpringContext.Instance.GetObject<INavigationPortalShortcutGroupService>(typeof(INavigationPortalShortcutGroupService));
            this.m_NavigationPortalShortcutService = SpringContext.Instance.GetObject<INavigationPortalShortcutService>(typeof(INavigationPortalShortcutService));
            this.m_NavigationPortalSidebarItemGroupService = SpringContext.Instance.GetObject<INavigationPortalSidebarItemGroupService>(typeof(INavigationPortalSidebarItemGroupService));
            this.m_NavigationPortalSidebarItemService = SpringContext.Instance.GetObject<INavigationPortalSidebarItemService>(typeof(INavigationPortalSidebarItemService));
        }
        #endregion
    }
}
