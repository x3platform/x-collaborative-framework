namespace X3Platform.WorkflowPlus
{
    /// <summary>工作流实体类接口</summary>
    public interface IWorkflowEntity
    {
        /// <summary>实体类的编号或标识</summary>
        string Id { get; }

        /// <summary>实体类的全称</summary>
        string ClassName { get; }

        /// <summary>查找实体类</summary>
        /// <param name="context">上下文环境接口</param>
        /// <returns></returns>
        void Load(IWorkflowClientContext context);

        /// <summary>保存</summary>
        void Save();

        /// <summary>删除</summary>
        void Delete();
    }
}