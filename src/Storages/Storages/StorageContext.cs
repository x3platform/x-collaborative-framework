namespace X3Platform.Storages
{
    #region Using Libraries
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Storages.Configuration;
    using X3Platform.Storages.IBLL;
    using X3Platform.IBatis.DataMapper;
    using System.Collections.Generic;
    using X3Platform.Configuration;
    using Common.Logging;
    using X3Platform.Globalization;
    #endregion

    /// <summary>存储应用上下文环境</summary>
    public class StorageContext : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 静态属性:Instance
        private static volatile StorageContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static StorageContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new StorageContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        public override string Name
        {
            get { return "应用管理"; }
        }
        #endregion

        #region 属性:StorageSchemaService
        private IStorageSchemaService m_StorageSchemaService;

        /// <summary>应用存储架构服务</summary>
        public IStorageSchemaService StorageSchemaService
        {
            get { return this.m_StorageSchemaService; }
        }
        #endregion

        #region 属性:StorageNodeService
        private IStorageNodeService m_StorageNodeService;

        /// <summary>应用存储节点服务</summary>
        public IStorageNodeService StorageNodeService
        {
            get { return m_StorageNodeService; }
        }
        #endregion

        #region 构造函数:StoragesContext()
        /// <summary>构造函数</summary>
        private StorageContext()
        {
            Restart();
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回服务. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();

                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
                KernelContext.Log.Error(ex.Message, ex);
                throw ex;
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
                KernelContext.Log.Info(string.Format(I18n.Strings["application_is_reloading"], StoragesConfiguration.ApplicationName));

                // 重新加载配置信息
                StoragesConfigurationView.Instance.Reload();
            }
            else
            {
                KernelContext.Log.Info(string.Format(I18n.Strings["application_is_loading"], StoragesConfiguration.ApplicationName));
            }

            // 创建对象构建器(Spring.NET)
            string springObjectFile = StoragesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(StoragesConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_StorageSchemaService = objectBuilder.GetObject<IStorageSchemaService>(typeof(IStorageSchemaService));
            this.m_StorageNodeService = objectBuilder.GetObject<IStorageNodeService>(typeof(IStorageNodeService));

            KernelContext.Log.Info(string.Format(I18n.Strings["application_is_successfully_loaded"], StoragesConfiguration.ApplicationName));
        }
        #endregion

        #region 函数:CreateSqlMappers(string storageSchemaId, string ibatisMapping)
        /// <summary>重新加载</summary>
        /// <param name="storageSchemaId">数据存储架构标识</param>
        /// <param name="ibatisMapping">IBatis映射文件</param>
        public Dictionary<string, ISqlMapper> CreateSqlMappers(string storageSchemaId, string ibatisMapping)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Storages.StorageContext.Instance.CreateSqlMappers(storageSchemaId:\"" + storageSchemaId + "\", ibatisMapping:\"" + ibatisMapping + "\") begin");
            }

            // 一个存储节点对应一个IBatisMapper对象, 每个节点都需要重复创建对象
            Dictionary<string, ISqlMapper> ibatisMappers = new Dictionary<string, ISqlMapper>();

            IList<IStorageNode> storageNodes = StorageContext.Instance.StorageNodeService.FindAllBySchemaId(storageSchemaId);

            logger.Debug("X3Platform.Storages.StorageContext.Instance.CreateSqlMappers() A");

            foreach (IStorageNode storageNode in storageNodes)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("X3Platform.Storages.StorageContext.Instance.CreateSqlMappers() creating " + storageNode.Name);
                }

                var ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

                ibatisMapper.DataSource.ConnectionString = KernelConfigurationView.Instance.ReplaceKeyValue(storageNode.ConnectionString);

                ibatisMappers.Add(storageNode.Name, ibatisMapper);
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Storages.StorageContext.Instance.CreateSqlMappers(storageSchemaId:\"" + storageSchemaId + "\", ibatisMapping:\"" + ibatisMapping + "\") end");
            }

            return ibatisMappers;
        }
        #endregion
    }
}
