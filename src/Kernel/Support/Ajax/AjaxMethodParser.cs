// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxMethodParser.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Ajax
{
    using System;
    using System.Reflection;
    using System.Xml;

    /// <summary>Ajax 方法解析器</summary>
    public sealed class AjaxMethodParser
    {
        #region 函数:Parse(object target, XmlDocument doc)
        /// <summary>解析</summary>
        /// <param name="target">目标对象</param>
        /// <param name="doc">Xml文档</param>
        /// <returns></returns>
        public static string Parse(object target, XmlDocument doc)
        {
            string action = AjaxStorageConvertor.Fetch("action", doc);

            return Parse(target, action, doc);
        }
        #endregion

        #region 函数:Parse(object target, string action, XmlDocument doc)
        /// <summary>解析</summary>
        /// <param name="target">目标对象</param>
        /// <param name="action">操作信息</param>
        /// <param name="doc">Xml文档</param>
        /// <returns></returns>
        public static string Parse(object target, string action, XmlDocument doc)
        {
            if (string.IsNullOrEmpty(action))
            {
                return "{message:{\"returnCode\":1,\"value\":\"Oops, I can't find action argument.\"}}";
            }

            string attributeName = null;

            Type type = target.GetType();

            MethodInfo[] methods = type.GetMethods();

            foreach (MethodInfo method in methods)
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(method);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is AjaxMethodAttribute)
                    {
                        attributeName = ((AjaxMethodAttribute)attribute).Name;

                        if (attributeName == action)
                        {
                            return type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, target, new object[] { doc }).ToString();
                        }
                    }
                }
            }

            return null;
        }
        #endregion
    }
}
