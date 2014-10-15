namespace X3Platform.Web.Builder
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Plugins;
    using X3Platform.Web.Builder.Layouts;
    using X3Platform.Web.Builder.Management;
    using X3Platform.Web.Builder.ILayouts;
    using X3Platform.Web.Configuration;
    #endregion

    /// <summary>页面构建器上下文环境</summary>
    public sealed class BuilderContext : CustomPlugin
    {
        #region 属性:Instance
        private static BuilderContext instance = new BuilderContext();

        private static object lockObject = new object();

        public static BuilderContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new BuilderContext();
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

        #region 属性:CustomizeManagement
        private CustomizeManagement m_CustomizeManagement;

        public CustomizeManagement CustomizeManagement
        {
            get { return m_CustomizeManagement; }
        }
        #endregion

        #region 属性:NavigationManagement
        private NavigationManagement m_NavigationManagement;

        public NavigationManagement NavigationManagement
        {
            get { return m_NavigationManagement; }
        }
        #endregion

        #region 属性:MenuManagement
        private MenuManagement m_MenuManagement;

        public MenuManagement MenuManagement
        {
            get { return m_MenuManagement; }
        }
        #endregion

        #region 属性:CustomizeBuilder
        private ICustomizeBuilder m_CustomizeBuilder;

        internal ICustomizeBuilder CustomizeBuilder
        {
            get { return m_CustomizeBuilder; }
        }
        #endregion

        #region 属性:NavigationBuilder
        private INavigationBuilder m_NavigationBuilder;

        internal INavigationBuilder NavigationBuilder
        {
            get { return m_NavigationBuilder; }
        }
        #endregion

        #region 属性:MenuBuilder
        private IMenuBuilder m_MenuBuilder;

        internal IMenuBuilder MenuBuilder
        {
            get { return m_MenuBuilder; }
        }
        #endregion

        #region 构造函数:BuilderContext()
        /// <summary><see cref="BuilderContext"/>的构造函数</summary>
        private BuilderContext()
        {
            Restart();
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
            this.configuration = WebConfigurationView.Instance.Configuration;

            this.m_CustomizeManagement = new CustomizeManagement();
            this.m_NavigationManagement = new NavigationManagement();
            this.m_MenuManagement = new MenuManagement();

            if (WebConfigurationView.Instance.Layout == "EnterprisePortalPlatform")
            {
                // 自定义页面
                m_CustomizeBuilder = new Layouts.EnterprisePortalPlatform.CustomizeBuilder();
                // 导航
                m_NavigationBuilder = new Layouts.EnterprisePortalPlatform.NavigationBuilder();
                // 菜单
                m_MenuBuilder = new Layouts.EnterprisePortalPlatform.MenuBuilder();
            }
            else
            {
                // 自定义页面
                m_CustomizeBuilder = new Layouts.CollaborationPlatform.CustomizeBuilder();
                // 导航
                m_NavigationBuilder = new Layouts.CollaborationPlatform.NavigationBuilder();
                // 菜单
                m_MenuBuilder = new Layouts.CollaborationPlatform.MenuBuilder();
            }
        }
        #endregion
    }
}
