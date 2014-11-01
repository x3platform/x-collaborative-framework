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
        /// <summary>���캯��</summary>
        /// <param name="options">ѡ��</param>
        /// <param name="doc">Xml�ĵ�</param>
        public RESTMethod(Dictionary<string, string> options, XmlDocument doc)
            : base(options, doc)
        {
            this.className = this.options["className"];

            this.methodName = this.options["methodName"];
        }

        /// <summary>ִ��</summary>
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