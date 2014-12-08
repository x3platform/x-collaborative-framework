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
        #region 静态属性:Instance
        private static volatile TasksContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
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

        #region 属性:Name
        /// <summary>名称</summary>
        public override string Name
        {
            get { return "任务管理上下文"; }
        }
        #endregion

        #region 属性:Configuration
        private TasksConfiguration configuration = null;

        /// <summary>配置</summary>
        public TasksConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:TaskCategoryService
        private ITaskCategoryService m_TaskCategoryService;

        /// <summary>任务类别</summary>
        public ITaskCategoryService TaskCategoryService
        {
            get { return m_TaskCategoryService; }
        }
        #endregion

        #region 属性:TaskWaitingService
        private ITaskWaitingService m_TaskWaitingService;

        /// <summary>定时任务</summary>
        public ITaskWaitingService TaskWaitingService
        {
            get { return m_TaskWaitingService; }
        }
        #endregion

        #region 属性:TaskWaitingReceiverService
        private ITaskWaitingReceiverService m_TaskWaitingReceiverService;

        /// <summary>定时任务接收者</summary>
        public ITaskWaitingReceiverService TaskWaitingReceiverService
        {
            get { return m_TaskWaitingReceiverService; }
        }
        #endregion

        #region 属性:TaskWorkService
        private ITaskWorkService m_TaskWorkService;

        /// <summary>任务</summary>
        public ITaskWorkService TaskWorkService
        {
            get { return m_TaskWorkService; }
        }
        #endregion

        #region 属性:TaskWorkReceiverService
        private ITaskWorkReceiverService m_TaskWorkReceiverService;

        /// <summary>任务接收者</summary>
        public ITaskWorkReceiverService TaskWorkReceiverService
        {
            get { return m_TaskWorkReceiverService; }
        }
        #endregion

        #region 属性:TaskHistoryService
        private ITaskHistoryService m_TaskHistoryService;

        /// <summary>任务历史记录</summary>
        public ITaskHistoryService TaskHistoryService
        {
            get { return m_TaskHistoryService; }
        }
        #endregion

        #region 构造函数:TasksContext()
        /// <summary>构造函数</summary>
        private TasksContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
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

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
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
