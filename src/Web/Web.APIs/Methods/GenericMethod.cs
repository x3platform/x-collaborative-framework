namespace X3Platform.Web.APIs.Methods
{
    using Globalization;
    using Json;
    using Messages;
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Xml;
    #endregion

    /// <summary></summary>
    public class GenericMethod : AbstractMethod
    {
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
        public override object Execute()
        {
            this.target = KernelContext.CreateObject(this.className);

            Type type = this.target.GetType();

            // 设置映射参数
            Mapping();

            // 验证必填参数
            Validate();

            // 执行方法
            return type.InvokeMember(this.methodName, BindingFlags.InvokeMethod, null, target, new object[] { doc });
        }
    }
}