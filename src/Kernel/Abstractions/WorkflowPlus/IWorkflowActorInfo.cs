namespace X3Platform.WorkflowPlus
{
    /// <summary>工作流执行者</summary>
    public interface IWorkflowActorInfo
    {
        /// <summary>标识</summary>
        string Id { get; set; }

        /// <summary>名称</summary>
        string Name { get; set; }

        /// <summary>登录名</summary>
        string LoginName { get; set; }

        /// <summary>单位成本(单位:分钟)</summary>
        decimal UnitCost { get; set; }
    }
}