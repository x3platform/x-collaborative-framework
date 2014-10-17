namespace X3Platform.WorkflowPlus
{
    /// <summary>工作流任务</summary>
    public interface IWorkflowTask
    {
        /// <summary>发送工作流任务信息</summary>
        /// <param name="args">参数</param>
        void Send(IWorkflowClientContext context);
    }
}