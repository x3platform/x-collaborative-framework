namespace X3Platform.Ajax
{
    using System;

    /// <summary>Ajax 方法特性</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AjaxMethodAttribute : Attribute
    {
        private string methodName;

        /// <summary>构造函数</summary>
        /// <param name="name"></param>
        public AjaxMethodAttribute(string name)
        {
            this.methodName = name;
        }

        /// <summary>名称</summary>
        public string Name
        {
            get { return this.methodName; }
        }
    }
}
