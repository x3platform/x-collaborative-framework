namespace X3Platform.Entities
{
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;

    /// <summary>实体类管理上下文环境</summary>
    public sealed class EntitiesManagement : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "实体类管理"; }
        }
        #endregion

        #region 属性:Instance
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

        #region 属性:Configuration
        private EntitiesConfiguration configuration = null;

        /// <summary>配置</summary>
        public EntitiesConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:EntityClickService
        private IEntityClickService m_EntityClickService = null;

        public IEntityClickService EntityClickService
        {
            get { return m_EntityClickService; }
        }
        #endregion

        #region 属性:EntityDocObjectService
        private IEntityDocObjectService m_EntityDocObjectService = null;

        public IEntityDocObjectService EntityDocObjectService
        {
            get { return m_EntityDocObjectService; }
        }
        #endregion

        #region 属性:EntityDraftService
        private IEntityDraftService m_EntityDraftService = null;

        public IEntityDraftService EntityDraftService
        {
            get { return m_EntityDraftService; }
        }
        #endregion

        #region 属性:EntityImplementationService
        private IEntityImplementationService m_EntityImplementationService = null;

        public IEntityImplementationService EntityImplementationService
        {
            get { return m_EntityImplementationService; }
        }
        #endregion

        #region 属性:EntityLifeHistoryService
        private IEntityLifeHistoryService m_EntityLifeHistoryService = null;

        public IEntityLifeHistoryService EntityLifeHistoryService
        {
            get { return m_EntityLifeHistoryService; }
        }
        #endregion

        #region 属性:EntityMetaDataService
        private IEntityMetaDataService m_EntityMetaDataService = null;

        public IEntityMetaDataService EntityMetaDataService
        {
            get { return m_EntityMetaDataService; }
        }
        #endregion

        #region 属性:EntityOperationLogService
        private IEntityOperationLogService m_EntityOperationLogService = null;

        public IEntityOperationLogService EntityOperationLogService
        {
            get { return m_EntityOperationLogService; }
        }
        #endregion

        #region 属性:EntitySchemaService
        private IEntitySchemaService m_EntitySchemaService = null;

        public IEntitySchemaService EntitySchemaService
        {
            get { return m_EntitySchemaService; }
        }
        #endregion

        #region 属性:EntitySnapshotService
        private IEntitySnapshotService m_EntitySnapshotService = null;

        public IEntitySnapshotService EntitySnapshotService
        {
            get { return m_EntitySnapshotService; }
        }
        #endregion

        #region 构造函数:EntitiesManagement()
        /// <summary>构造函数</summary>
        private EntitiesManagement()
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
