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

    /// <summary>�洢Ӧ�������Ļ���</summary>
    public class StorageContext : CustomPlugin
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ��̬����:Instance
        private static volatile StorageContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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

        #region ����:Name
        /// <summary>����</summary>
        public override string Name
        {
            get { return "Ӧ�ù���"; }
        }
        #endregion

        #region ����:StorageSchemaService
        private IStorageSchemaService m_StorageSchemaService;

        /// <summary>Ӧ�ô洢�ܹ�����</summary>
        public IStorageSchemaService StorageSchemaService
        {
            get { return this.m_StorageSchemaService; }
        }
        #endregion

        #region ����:StorageNodeService
        private IStorageNodeService m_StorageNodeService;

        /// <summary>Ӧ�ô洢�ڵ����</summary>
        public IStorageNodeService StorageNodeService
        {
            get { return m_StorageNodeService; }
        }
        #endregion

        #region ���캯��:StoragesContext()
        /// <summary>���캯��</summary>
        private StorageContext()
        {
            Restart();
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
                KernelContext.Log.Error(ex.Message, ex);
                throw ex;
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
                KernelContext.Log.Info(string.Format(I18n.Strings["application_is_reloading"], StoragesConfiguration.ApplicationName));

                // ���¼���������Ϣ
                StoragesConfigurationView.Instance.Reload();
            }
            else
            {
                KernelContext.Log.Info(string.Format(I18n.Strings["application_is_loading"], StoragesConfiguration.ApplicationName));
            }

            // �������󹹽���(Spring.NET)
            string springObjectFile = StoragesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(StoragesConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
            this.m_StorageSchemaService = objectBuilder.GetObject<IStorageSchemaService>(typeof(IStorageSchemaService));
            this.m_StorageNodeService = objectBuilder.GetObject<IStorageNodeService>(typeof(IStorageNodeService));

            KernelContext.Log.Info(string.Format(I18n.Strings["application_is_successfully_loaded"], StoragesConfiguration.ApplicationName));
        }
        #endregion

        #region ����:CreateSqlMappers(string storageSchemaId, string ibatisMapping)
        /// <summary>���¼���</summary>
        /// <param name="storageSchemaId">���ݴ洢�ܹ���ʶ</param>
        /// <param name="ibatisMapping">IBatisӳ���ļ�</param>
        public Dictionary<string, ISqlMapper> CreateSqlMappers(string storageSchemaId, string ibatisMapping)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Storages.StorageContext.Instance.CreateSqlMappers(storageSchemaId:\"" + storageSchemaId + "\", ibatisMapping:\"" + ibatisMapping + "\") begin");
            }

            // һ���洢�ڵ��Ӧһ��IBatisMapper����, ÿ���ڵ㶼��Ҫ�ظ���������
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
