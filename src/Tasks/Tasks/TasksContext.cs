namespace X3Platform.Tasks
{
    #region Using Libraries
    using System;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.IBLL;
    #endregion

    /// <summary></summary>
    public class TasksContext : CustomPlugin
    {
        #region ��̬����:Instance
        private static volatile TasksContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static TasksContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TasksContext();
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
            get { return "�������������"; }
        }
        #endregion

        #region ����:Configuration
        private TasksConfiguration configuration = null;

        /// <summary>����</summary>
        public TasksConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ����:TaskCategoryService
        private ITaskCategoryService m_TaskCategoryService;

        /// <summary>�������</summary>
        public ITaskCategoryService TaskCategoryService
        {
            get { return m_TaskCategoryService; }
        }
        #endregion

        #region ����:TaskWaitingService
        private ITaskWaitingService m_TaskWaitingService;

        /// <summary>��ʱ����</summary>
        public ITaskWaitingService TaskWaitingService
        {
            get { return m_TaskWaitingService; }
        }
        #endregion

        #region ����:TaskWaitingReceiverService
        private ITaskWaitingReceiverService m_TaskWaitingReceiverService;

        /// <summary>��ʱ���������</summary>
        public ITaskWaitingReceiverService TaskWaitingReceiverService
        {
            get { return m_TaskWaitingReceiverService; }
        }
        #endregion

        #region ����:TaskWorkService
        private ITaskWorkService m_TaskWorkService;

        /// <summary>����</summary>
        public ITaskWorkService TaskWorkService
        {
            get { return m_TaskWorkService; }
        }
        #endregion

        #region ����:TaskWorkReceiverService
        private ITaskWorkReceiverService m_TaskWorkReceiverService;

        /// <summary>���������</summary>
        public ITaskWorkReceiverService TaskWorkReceiverService
        {
            get { return m_TaskWorkReceiverService; }
        }
        #endregion

        #region ����:TaskHistoryService
        private ITaskHistoryService m_TaskHistoryService;

        /// <summary>������ʷ��¼</summary>
        public ITaskHistoryService TaskHistoryService
        {
            get { return m_TaskHistoryService; }
        }
        #endregion

        #region ���캯��:TasksContext()
        /// <summary>���캯��</summary>
        private TasksContext()
        {
            this.Restart();
        }
        #endregion

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();
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
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
            this.m_TaskWorkService = objectBuilder.GetObject<ITaskWorkService>(typeof(ITaskWorkService));
            this.m_TaskWorkReceiverService = objectBuilder.GetObject<ITaskWorkReceiverService>(typeof(ITaskWorkReceiverService));
            this.m_TaskCategoryService = objectBuilder.GetObject<ITaskCategoryService>(typeof(ITaskCategoryService));
            this.m_TaskHistoryService = objectBuilder.GetObject<ITaskHistoryService>(typeof(ITaskHistoryService));
            this.m_TaskWaitingService = objectBuilder.GetObject<ITaskWaitingService>(typeof(ITaskWaitingService));
            this.m_TaskWaitingReceiverService = objectBuilder.GetObject<ITaskWaitingReceiverService>(typeof(ITaskWaitingReceiverService));
        }
        #endregion
    }
}
