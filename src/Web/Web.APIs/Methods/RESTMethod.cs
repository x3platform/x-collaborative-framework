namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary></summary>
    public class RESTMethod : AjaxMethod
    {
        /// <summary>构造函数</summary>
        /// <param name="options">选项</param>
        /// <param name="doc">Xml文档</param>
        public RESTMethod(Dictionary<string, string> options, XmlDocument doc)
            : base(options, doc)
        {
            this.className = this.options["className"];

            this.methodName = this.options["methodName"];
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public override object Execute()
        {
            this.className = this.options["className"];

            this.methodName = this.options["methodName"];

            // entityClassName
            this.target = KernelContext.CreateObject(this.className);

            Type type = this.target.GetType();

            return type.InvokeMember(this.methodName, BindingFlags.InvokeMethod, null, target, new object[] { doc });
        }
    }
}