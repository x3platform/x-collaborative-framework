namespace X3Platform.WorkflowPlus
{
    /// <summary>运行时的流程实例</summary>
    public interface IWorkflowInstanceInfo
    {
        #region 属性:Id
        /// <summary>标识</summary>
        string Id { get; }
        #endregion

        #region 属性:WorkflowEntity
        /// <summary>工作流相关实体信息</summary>
        IWorkflowEntity WorkflowEntity { get; }
        #endregion
    }
}