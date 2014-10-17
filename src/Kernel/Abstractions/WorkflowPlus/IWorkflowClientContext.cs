namespace X3Platform.WorkflowPlus
{
    /// <summary>工作流客户端上下文环境接口</summary>
    public interface IWorkflowClientContext
    {
        /// <summary>索引</summary>
        /// <param name="key">对象在客户端上下文环境中的键</param>
        /// <returns></returns>
        object this[string key] { set; get; }

        /// <summary>设置对象</summary>
        /// <param name="key">对象在客户端上下文环境中的键</param>
        /// <param name="value"></param>
        void Set(string key, object value);

        /// <summary>获取对象</summary>
        /// <param name="key">对象的键</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>尝试获取对象</summary>
        /// <param name="key">对象在客户端上下文环境中的键</param>
        /// <param name="defaultValue">如果对象的值不存在时, 返回defaultValue.</param>
        /// <returns></returns>
        object TryGet(string key, object defaultValue);

        /// <summary>是否存在键</summary>
        /// <param name="key">键</param>
        bool Exists(string key);
    }
}
