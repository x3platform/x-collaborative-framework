namespace X3Platform.WorkflowPlus
{
    /// <summary>运行时的所有流程对象接口</summary>
    public interface IWorkflowObject
    {
        /// <summary>标识</summary>
        string Id { get; }

        /// <summary>名称</summary>
        string Name { get; }

        /// <summary>类型</summary>		
        string Type { get; }

        /// <summary>工作流实例</summary>
        IWorkflowInstanceInfo WorkflowInstance { get; }
    }
}