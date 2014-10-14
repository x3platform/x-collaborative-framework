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
            get { return "����"; }
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

        #region ����:TaskService
        private ITaskService m_TaskService;

        /// <summary>����</summary>
        public ITaskService TaskService
        {
            get { return m_TaskService; }
        }
        #endregion

        #region ����:TaskReceiverService
        private ITaskReceiverService m_TaskReceiverService;

        /// <summary>���������</summary>
        public ITaskReceiverService TaskReceiverService
        {
            get { return m_TaskReceiverService; }
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

        #region ����:TaskHistoryService
        private ITaskHistoryService m_TaskHistoryService;

        /// <summary>������ʷ��¼</summary>
        public ITaskHistoryService TaskHistoryService
        {
            get { return m_TaskHistoryService; }
        }
        #endregion

        #region ����:TimingTaskService
        private ITimingTaskService m_TimingTaskService;

        /// <summary>��ʱ����</summary>
        public ITimingTaskService TimingTaskService
        {
            get { return m_TimingTaskService; }
        }
        #endregion

        #region ����:TimingTaskReceiverService
        private ITimingTaskReceiverService m_TimingTaskReceiverService;

        /// <summary>��ʱ���������</summary>
        public ITimingTaskReceiverService TimingTaskReceiverService
        {
            get { return m_TimingTaskReceiverService; }
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
            this.m_TaskService = objectBuilder.GetObject<ITaskService>(typeof(ITaskService));
            this.m_TaskReceiverService = objectBuilder.GetObject<ITaskReceiverService>(typeof(ITaskReceiverService));
            this.m_TaskCategoryService = objectBuilder.GetObject<ITaskCategoryService>(typeof(ITaskCategoryService));
            this.m_TaskHistoryService = objectBuilder.GetObject<ITaskHistoryService>(typeof(ITaskHistoryService));
            this.m_TimingTaskService = objectBuilder.GetObject<ITimingTaskService>(typeof(ITimingTaskService));
            this.m_TimingTaskReceiverService = objectBuilder.GetObject<ITimingTaskReceiverService>(typeof(ITimingTaskReceiverService));
        }
        #endregion
    }
}
