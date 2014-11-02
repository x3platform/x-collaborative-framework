namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary>抽象方法</summary>
    public abstract class AbstractMethod : IMethod
    {
        /// <summary>选项</summary>
        protected Dictionary<string, string> options;

        /// <summary>构造函数</summary>
        public AbstractMethod()
        { 
        }

        /// <summary>构造函数</summary>
        /// <param name="options">选项</param>
        /// <param name="doc">Xml文档</param>
        public AbstractMethod(Dictionary<string, string> options)
        {
            this.options = options;
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public abstract object Execute();
    }
}