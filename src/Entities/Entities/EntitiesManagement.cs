namespace X3Platform.Entities
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;

    /// <summary>ʵ������������Ļ���</summary>
    public sealed class EntitiesManagement : CustomPlugin
    {
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

        #region ����:Configuration
        private EntitiesConfiguration configuration = null;

        /// <summary>����</summary>
        public EntitiesConfiguration Configuration
        {
            get { return configuration; }
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

        #region ����:EntityImplementationService
        private IEntityImplementationService m_EntityImplementationService = null;

        public IEntityImplementationService EntityImplementationService
        {
            get { return m_EntityImplementationService; }
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
            configuration = EntitiesConfigurationView.Instance.Configuration;

            this.m_EntityClickService = SpringContext.Instance.GetObject<IEntityClickService>(typeof(IEntityClickService));
            this.m_EntityDocObjectService = SpringContext.Instance.GetObject<IEntityDocObjectService>(typeof(IEntityDocObjectService));
            this.m_EntityDraftService = SpringContext.Instance.GetObject<IEntityDraftService>(typeof(IEntityDraftService));
            this.m_EntityImplementationService = SpringContext.Instance.GetObject<IEntityImplementationService>(typeof(IEntityImplementationService));
            this.m_EntityLifeHistoryService = SpringContext.Instance.GetObject<IEntityLifeHistoryService>(typeof(IEntityLifeHistoryService));
            this.m_EntityMetaDataService = SpringContext.Instance.GetObject<IEntityMetaDataService>(typeof(IEntityMetaDataService));
            this.m_EntityOperationLogService = SpringContext.Instance.GetObject<IEntityOperationLogService>(typeof(IEntityOperationLogService));
            this.m_EntitySchemaService = SpringContext.Instance.GetObject<IEntitySchemaService>(typeof(IEntitySchemaService));
            this.m_EntitySnapshotService = SpringContext.Instance.GetObject<IEntitySnapshotService>(typeof(IEntitySnapshotService));
        }
        #endregion
    }
}
