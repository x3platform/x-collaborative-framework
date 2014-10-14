// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxMethodAttribute.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;

namespace X3Platform.Ajax
{
    /// <summary>Ajax方法特性</summary>
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
