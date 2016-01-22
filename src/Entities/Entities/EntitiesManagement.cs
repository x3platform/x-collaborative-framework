namespace X3Platform.Entities
{
    #region Using Libraries
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Apps;
    using Common.Logging;
    using System.Reflection;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary>ʵ������������Ļ���</summary>
    public sealed class EntitiesManagement : CustomPlugin
    {
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        #region ����:Name
        public override string Name
        {
            get { return "ʵ�������"; }
        }
        #endregion

        #region ����:Instance
        private static volatile EntitiesManagement instance = null;

        private static object lockObject = new object();

        public static EntitiesManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new EntitiesManagement();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ����:EntityClickService
        private IEntityClickService m_EntityClickService = null;

        public IEntityClickService EntityClickService
        {
            get { return m_EntityClickService; }
        }
        #endregion

        #region ����:EntityDocObjectService
        private IEntityDocObjectService m_EntityDocObjectService = null;

        public IEntityDocObjectService EntityDocObjectService
        {
            get { return m_EntityDocObjectService; }
        }
        #endregion

        #region ����:EntityDraftService
        private IEntityDraftService m_EntityDraftService = null;

        public IEntityDraftService EntityDraftService
        {
            get { return m_EntityDraftService; }
        }
        #endregion

        #region ����:EntityFeedbackService
        private IEntityFeedbackService m_EntityFeedbackService = null;

        public IEntityFeedbackService EntityFeedbackService
        {
            get { return m_EntityFeedbackService; }
        }
        #endregion

        #region ����:EntityLifeHistoryService
        private IEntityLifeHistoryService m_EntityLifeHistoryService = null;

        public IEntityLifeHistoryService EntityLifeHistoryService
        {
            get { return m_EntityLifeHistoryService; }
        }
        #endregion

        #region ����:EntityMetaDataService
        private IEntityMetaDataService m_EntityMetaDataService = null;

        public IEntityMetaDataService EntityMetaDataService
        {
            get { return m_EntityMetaDataService; }
        }
        #endregion

        #region ����:EntityOperationLogService
        private IEntityOperationLogService m_EntityOperationLogService = null;

        public IEntityOperationLogService EntityOperationLogService
        {
            get { return m_EntityOperationLogService; }
        }
        #endregion

        #region ����:EntitySchemaService
        private IEntitySchemaService m_EntitySchemaService = null;

        public IEntitySchemaService EntitySchemaService
        {
            get { return m_EntitySchemaService; }
        }
        #endregion

        #region ����:EntitySnapshotService
        private IEntitySnapshotService m_EntitySnapshotService = null;

        public IEntitySnapshotService EntitySnapshotService
        {
            get { return m_EntitySnapshotService; }
        }
        #endregion

        #region ���캯��:EntitiesManagement()
        /// <summary>���캯��</summary>
        private EntitiesManagement()
        {
            Restart();
        }
        #endregion

        /// <summary>��������������</summary>
        private int restartCount = 0;

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            ApplicationInfo application = AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName];

            ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            applicationEvent.ApplicationId = application.Id;
            applicationEvent.Tags = "��Ϣ";
            applicationEvent.Description = string.Format("��{0}��Ӧ��ִ��{1}�¼���", application.ApplicationDisplayName, (this.restartCount == 0 ? "��ʼ��" : "��" + this.restartCount + "������"));

            applicationEvent.Start();

            try
            {
                // ���¼�����Ϣ
                Reload();

                // ������������������
                this.restartCount++;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

                applicationEvent.Tags = "����";
                applicationEvent.Description = string.Format("������{0}������Ϣ:{1}", applicationEvent.Description, ex.Message);

                throw ex;
            }
            finally
            {
                applicationEvent.Finish();

                AppsContext.Instance.ApplicationEventService.Save(applicationEvent);
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
                EntitiesConfigurationView.Instance.Reload();
            }

            // �������󹹽���(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            this.m_EntityClickService = objectBuilder.GetObject<IEntityClickService>(typeof(IEntityClickService));
            this.m_EntityDocObjectService = objectBuilder.GetObject<IEntityDocObjectService>(typeof(IEntityDocObjectService));
            this.m_EntityDraftService = objectBuilder.GetObject<IEntityDraftService>(typeof(IEntityDraftService));
            this.m_EntityFeedbackService = objectBuilder.GetObject<IEntityFeedbackService>(typeof(IEntityFeedbackService));
            this.m_EntityLifeHistoryService = objectBuilder.GetObject<IEntityLifeHistoryService>(typeof(IEntityLifeHistoryService));
            this.m_EntityMetaDataService = objectBuilder.GetObject<IEntityMetaDataService>(typeof(IEntityMetaDataService));
            this.m_EntityOperationLogService = objectBuilder.GetObject<IEntityOperationLogService>(typeof(IEntityOperationLogService));
            this.m_EntitySchemaService = objectBuilder.GetObject<IEntitySchemaService>(typeof(IEntitySchemaService));
            this.m_EntitySnapshotService = objectBuilder.GetObject<IEntitySnapshotService>(typeof(IEntitySnapshotService));
        }
        #endregion

        #region ����:GetEntityClassName(Type type)
        /// <summary>��ȡʵ��������</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetEntityClassName(Type type)
        {
            string objectType = KernelContext.ParseObjectType(type);

            EntitySchemaInfo param = Instance.EntitySchemaService.FindOneByEntityClassFullName(objectType);

            return param == null ? objectType : param.EntityClassName;
        }
        #endregion
    }
}
