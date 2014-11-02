namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary></summary>
    public class GenericMethod : IMethod
    {
        /// <summary>选项</summary>
        protected Dictionary<string, string> options;
        /// <summary>Xml文档</summary>
        protected XmlDocument doc;
        /// <summary>目标对象</summary>
        protected object target;
        /// <summary>类名称</summary>
        protected string className;
        /// <summary>方法名称</summary>
        protected string methodName;

        /// <summary>构造函数</summary>
        public GenericMethod()
        { 
        }

        /// <summary>构造函数</summary>
        /// <param name="options">选项</param>
        /// <param name="doc">Xml文档</param>
        public GenericMethod(Dictionary<string, string> options, XmlDocument doc)
        {
            this.options = options;

            this.doc = doc;

            this.className = this.options["className"];

            this.methodName = this.options["methodName"];
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public virtual object Execute()
        {
            // entityClassName
            this.target = KernelContext.CreateObject(this.className);

            Type type = this.target.GetType();

            return type.InvokeMember(this.methodName, BindingFlags.InvokeMethod, null, target, new object[] { doc });
        }
    }
}