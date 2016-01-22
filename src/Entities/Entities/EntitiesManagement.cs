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

    /// <summary>实体类管理上下文环境</summary>
    public sealed class EntitiesManagement : CustomPlugin
    {
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

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

        #region 属性:EntityFeedbackService
        private IEntityFeedbackService m_EntityFeedbackService = null;

        public IEntityFeedbackService EntityFeedbackService
        {
            get { return m_EntityFeedbackService; }
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

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            ApplicationInfo application = AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName];

            ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            applicationEvent.ApplicationId = application.Id;
            applicationEvent.Tags = "信息";
            applicationEvent.Description = string.Format("【{0}】应用执行{1}事件。", application.ApplicationDisplayName, (this.restartCount == 0 ? "初始化" : "第" + this.restartCount + "次重启"));

            applicationEvent.Start();

            try
            {
                // 重新加载信息
                Reload();

                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

                applicationEvent.Tags = "错误";
                applicationEvent.Description = string.Format("【错误】{0}错误信息:{1}", applicationEvent.Description, ex.Message);

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

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // 重新加载配置信息
                EntitiesConfigurationView.Instance.Reload();
            }

            // 创建对象构建器(Spring.NET)
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

        #region 函数:GetEntityClassName(Type type)
        /// <summary>获取实体类名称</summary>
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
