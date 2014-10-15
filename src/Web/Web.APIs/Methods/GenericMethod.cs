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
    public class GenericMethod : IMethod
    {
        /// <summary>ѡ��</summary>
        protected Dictionary<string, string> options;
        /// <summary>Xml�ĵ�</summary>
        protected XmlDocument doc;
        /// <summary>Ŀ������</summary>
        protected object target;
        /// <summary>������</summary>
        protected string className;
        /// <summary>��������</summary>
        protected string methodName;

        /// <summary>���캯��</summary>
        public GenericMethod()
        { 
        }

        /// <summary>���캯��</summary>
        /// <param name="options">ѡ��</param>
        /// <param name="doc">Xml�ĵ�</param>
        public GenericMethod(Dictionary<string, string> options, XmlDocument doc)
        {
            this.options = options;

            this.doc = doc;

            this.className = this.options["className"];

            this.methodName = this.options["methodName"];
        }

        /// <summary>ִ��</summary>
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