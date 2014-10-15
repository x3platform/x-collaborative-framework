#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary></summary>
    public class AjaxMethod : GenericMethod
    {
        /// <summary>���캯��</summary>
        /// <param name="options">ѡ��</param>
        /// <param name="doc">Xml�ĵ�</param>
        public AjaxMethod(Dictionary<string, string> options, XmlDocument doc)
            : base(options, doc)
        {
            this.className = this.options["className"];

            this.methodName = this.options["methodName"];
        }

        /// <summary>ִ��</summary>
        /// <returns></returns>
        public override object Execute()
        {
            // entityClassName
            this.target = KernelContext.CreateObject(this.className);

            Type type = this.target.GetType();

            return type.InvokeMember(this.methodName, BindingFlags.InvokeMethod, null, target, new object[] { doc });
        }
    }
}