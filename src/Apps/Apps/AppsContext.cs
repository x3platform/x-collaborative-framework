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

    /// <summary>Ӧ�������Ļ���</summary>
    public class AppsContext : CustomPlugin
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ��̬����:Instance
        private static volatile AppsContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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

        #region ����:Name
        /// <summary>����</summary>
        public override string Name
        {
            get { return "Ӧ�ù���"; }
        }
        #endregion

        #region ����:Configuration
        private AppsConfiguration configuration = null;

        /// <summary>����</summary>
        public AppsConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ����:ApplicationService
        private IApplicationService m_ApplicationService;

        /// <summary>Ӧ�÷���</summary>
        public IApplicationService ApplicationService
        {
            get { return m_ApplicationService; }
        }
        #endregion

        #region ����:ApplicationErrorService
        private IApplicationErrorService m_ApplicationErrorService;

        /// <summary>Ӧ�ô������</summary>
        public IApplicationErrorService ApplicationErrorService
        {
            get { return m_ApplicationErrorService; }
        }
        #endregion

        #region ����:ApplicationEventService
        private IApplicationEventService m_ApplicationEventService;

        /// <summary>Ӧ���¼�����</summary>
        public IApplicationEventService ApplicationEventService
        {
            get { return m_ApplicationEventService; }
        }
        #endregion

        #region ����:ApplicationFeatureService
        private IApplicationFeatureService m_ApplicationFeatureService;

        /// <summary>Ӧ�ù��ܷ���</summary>
        public IApplicationFeatureService ApplicationFeatureService
        {
            get { return m_ApplicationFeatureService; }
        }
        #endregion

        #region ����:ApplicationFeatureDateLimitService
        private IApplicationFeatureDateLimitService m_ApplicationFeatureDateLimitService;

        /// <summary>Ӧ�ù���ʱ�����Ʒ���</summary>
        public IApplicationFeatureDateLimitService ApplicationFeatureDateLimitService
        {
            get { return m_ApplicationFeatureDateLimitService; }
        }
        #endregion

        #region ����:ApplicationSettingGroupService
        private IApplicationSettingGroupService m_ApplicationSettingGroupService;

        /// <summary>Ӧ�����÷������</summary>
        public IApplicationSettingGroupService ApplicationSettingGroupService
        {
            get { return m_ApplicationSettingGroupService; }
        }
        #endregion

        #region ����:ApplicationSettingService
        private IApplicationSettingService m_ApplicationSettingService;

        /// <summary>Ӧ�����÷���</summary>
        public IApplicationSettingService ApplicationSettingService
        {
            get { return m_ApplicationSettingService; }
        }
        #endregion

        #region ����:ApplicationMenuService
        private IApplicationMenuService m_ApplicationMenuService;

        /// <summary>Ӧ�ò˵�ά�ȷ���</summary>
        public IApplicationMenuService ApplicationMenuService
        {
            get { return m_ApplicationMenuService; }
        }
        #endregion

        #region ����:ApplicationMethodService
        private IApplicationMethodService m_ApplicationMethodService;

        /// <summary>Ӧ�ò˵������</summary>
        public IApplicationMethodService ApplicationMethodService
        {
            get { return m_ApplicationMethodService; }
        }
        #endregion

        #region ����:ApplicationOptionService
        private IApplicationOptionService m_ApplicationOptionService;

        /// <summary>Ӧ��ѡ�����</summary>
        public IApplicationOptionService ApplicationOptionService
        {
            get { return m_ApplicationOptionService; }
        }
        #endregion

        #region ���캯��:AppsContext()
        /// <summary>���캯��</summary>
        private AppsContext()
        {
            this.Restart();
        }
        #endregion

        /// <summary>��������������</summary>
        private int restartCount = 0;

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>���ط���. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();

                // ������������������
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

        #region ����:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // ���¼���������Ϣ
                AppsConfigurationView.Instance.Reload();
            }

            this.configuration = AppsConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
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
