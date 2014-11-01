namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary>���󷽷�</summary>
    public abstract class AbstractMethod : IMethod
    {
        /// <summary>ѡ��</summary>
        protected Dictionary<string, string> options;

        /// <summary>���캯��</summary>
        public AbstractMethod()
        { 
        }

        /// <summary>���캯��</summary>
        /// <param name="options">ѡ��</param>
        /// <param name="doc">Xml�ĵ�</param>
        public AbstractMethod(Dictionary<string, string> options)
        {
            this.options = options;
        }

        /// <summary>ִ��</summary>
        /// <returns></returns>
        public abstract object Execute();
    }
}