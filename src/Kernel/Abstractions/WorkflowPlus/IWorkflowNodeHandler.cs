namespace X3Platform.WorkflowPlus
{
    /// <summary>工作流节点处理器</summary>
    public interface IWorkflowNodeHandler
    {
        /// <summary>执行</summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数</param>
        void Execute(string methodName, IWorkflowClientContext context);
    }
}