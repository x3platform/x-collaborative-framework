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

    /// <summary>ҳ�湹���������Ļ���</summary>
    public sealed class BuilderContext : CustomPlugin
    {
        #region ����:Instance
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

        #region ����:Configuration
        private WebConfiguration configuration = null;

        /// <summary>����</summary>
        public WebConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ����:CustomizeManagement
        private CustomizeManagement m_CustomizeManagement;

        public CustomizeManagement CustomizeManagement
        {
            get { return m_CustomizeManagement; }
        }
        #endregion

        #region ����:NavigationManagement
        private NavigationManagement m_NavigationManagement;

        public NavigationManagement NavigationManagement
        {
            get { return m_NavigationManagement; }
        }
        #endregion

        #region ����:MenuManagement
        private MenuManagement m_MenuManagement;

        public MenuManagement MenuManagement
        {
            get { return m_MenuManagement; }
        }
        #endregion

        #region ����:CustomizeBuilder
        private ICustomizeBuilder m_CustomizeBuilder;

        internal ICustomizeBuilder CustomizeBuilder
        {
            get { return m_CustomizeBuilder; }
        }
        #endregion

        #region ����:NavigationBuilder
        private INavigationBuilder m_NavigationBuilder;

        internal INavigationBuilder NavigationBuilder
        {
            get { return m_NavigationBuilder; }
        }
        #endregion

        #region ����:MenuBuilder
        private IMenuBuilder m_MenuBuilder;

        internal IMenuBuilder MenuBuilder
        {
            get { return m_MenuBuilder; }
        }
        #endregion

        #region ���캯��:BuilderContext()
        /// <summary><see cref="BuilderContext"/>�Ĺ��캯��</summary>
        private BuilderContext()
        {
            Restart();
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
            this.configuration = WebConfigurationView.Instance.Configuration;

            this.m_CustomizeManagement = new CustomizeManagement();
            this.m_NavigationManagement = new NavigationManagement();
            this.m_MenuManagement = new MenuManagement();

            if (WebConfigurationView.Instance.Layout == "EnterprisePortalPlatform")
            {
                // �Զ���ҳ��
                m_CustomizeBuilder = new Layouts.EnterprisePortalPlatform.CustomizeBuilder();
                // ����
                m_NavigationBuilder = new Layouts.EnterprisePortalPlatform.NavigationBuilder();
                // �˵�
                m_MenuBuilder = new Layouts.EnterprisePortalPlatform.MenuBuilder();
            }
            else
            {
                // �Զ���ҳ��
                m_CustomizeBuilder = new Layouts.CollaborationPlatform.CustomizeBuilder();
                // ����
                m_NavigationBuilder = new Layouts.CollaborationPlatform.NavigationBuilder();
                // �˵�
                m_MenuBuilder = new Layouts.CollaborationPlatform.MenuBuilder();
            }
        }
        #endregion
    }
}
