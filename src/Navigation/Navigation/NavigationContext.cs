namespace X3Platform.Navigation
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Navigation.Configuration;
    using X3Platform.Navigation.IBLL;

    public sealed class NavigationContext : CustomPlugin
    {
        #region ����:Name
        public override string Name
        {
            get { return "����"; }
        }
        #endregion

        #region ����:Instance
        private static volatile NavigationContext instance = null;

        private static object lockObject = new object();

        /// <summary>
        /// ʵ��
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

        #region ����:Configuration
        private NavigationConfiguration configuration = null;

        /// <summary>����</summary>
        public NavigationConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ����:NavigationPortalService
        private INavigationPortalService m_NavigationPortalService;

        /// <summary>�Ż�</summary>
        public INavigationPortalService NavigationPortalService
        {
            get { return m_NavigationPortalService; }
        }
        #endregion

        #region ����:NavigationPortalTopMenuService
        private INavigationPortalTopMenuService m_NavigationPortalTopMenuService;

        /// <summary>�����˵�</summary>
        public INavigationPortalTopMenuService NavigationPortalTopMenuService
        {
            get { return m_NavigationPortalTopMenuService; }
        }
        #endregion

        #region ����:NavigationPortalShortcutGroupService
        private INavigationPortalShortcutGroupService m_NavigationPortalShortcutGroupService;

        /// <summary>��ݷ�ʽ����</summary>
        public INavigationPortalShortcutGroupService NavigationPortalShortcutGroupService
        {
            get { return m_NavigationPortalShortcutGroupService; }
        }
        #endregion

        #region ����:NavigationPortalShortcutService
        private INavigationPortalShortcutService m_NavigationPortalShortcutService;

        /// <summary>��ݷ�ʽ</summary>
        public INavigationPortalShortcutService NavigationPortalShortcutService
        {
            get { return m_NavigationPortalShortcutService; }
        }
        #endregion

        #region ����:NavigationPortalSidebarItemGroupService
        private INavigationPortalSidebarItemGroupService m_NavigationPortalSidebarItemGroupService;

        /// <summary>�����˵������</summary>
        public INavigationPortalSidebarItemGroupService NavigationPortalSidebarItemGroupService
        {
            get { return m_NavigationPortalSidebarItemGroupService; }
        }
        #endregion

        #region ����:NavigationPortalSidebarItemService
        private INavigationPortalSidebarItemService m_NavigationPortalSidebarItemService;

        /// <summary>�����˵���</summary>
        public INavigationPortalSidebarItemService NavigationPortalSidebarItemService
        {
            get { return m_NavigationPortalSidebarItemService; }
        }
        #endregion

        #region ���캯��:NavigationContext()
        /// <summary>���캯��</summary>
        private NavigationContext()
        {
            Reload();
        }
        #endregion

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
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

        #region ����:Reload()
        /// <summary>���¼���</summary>
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
