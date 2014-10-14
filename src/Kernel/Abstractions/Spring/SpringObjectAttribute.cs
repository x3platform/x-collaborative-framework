namespace X3Platform.Spring
{
    using System;

    /// <summary>Srping对象属性标签</summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SpringObjectAttribute : Attribute
    {
        private string objectName;

        /// <summary>构造函数</summary>
        /// <param name="name">接口名称</param>
        public SpringObjectAttribute(string name)
        {
            this.objectName = name;
        }

        /// <summary>接口名称</summary>
        public string Name
        {
            get { return this.objectName; }
        }
    }
}
