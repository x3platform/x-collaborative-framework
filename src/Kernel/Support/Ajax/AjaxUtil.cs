namespace X3Platform.Ajax
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml;

    using X3Platform.Util;

    using X3Platform.Ajax.Configuration;

    /// <summary></summary>
    public class AjaxUtil
    {
        //-------------------------------------------------------
        // C# 对象 => JOSN 对象
        //-------------------------------------------------------

        #region 函数:Parse<T>(IList<T> list)
        /// <summary>对象解析为JOSN字符串</summary>
        public static string Parse<T>(IList<T> list)
        {
            return Parse<T>(list, null);
        }
        #endregion

        #region 函数:Parse<T>(IList<T> list, List<string> banKeys)
        /// <summary>对象解析为JOSN字符串</summary>
        public static string Parse<T>(IList<T> list, List<string> banKeys)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("[");

            foreach (T item in list)
            {
                outString.AppendFormat("{0},", Parse<T>(item, banKeys));
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]");

            return outString.ToString();
        }
        #endregion

        #region 函数:Parse<T>(T targetObject)
        /// <summary>对象解析为JOSN字符串</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static string Parse<T>(T targetObject)
        {
            return Parse<T>(targetObject, null);
        }
        #endregion

        #region 函数:Parse<T>(T targetObject, List<string> banKeys)
        /// <summary>对象解析为JOSN字符串</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <param name="banKeys">禁止转化的键</param>
        /// <returns></returns>
        public static string Parse<T>(T targetObject, List<string> banKeys)
        {
            return Parse<T>(targetObject, banKeys, AjaxConfigurationView.Instance.NamingRule);
        }
        #endregion

        #region 函数:Parse<T>(T targetObject, List<string> banKeys, string namingRule)
        /// <summary>对象解析为JOSN字符串</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <param name="banKeys">禁止转化的键</param>
        /// <returns></returns>
        public static string Parse<T>(T targetObject, List<string> banKeys, string namingRule)
        {
            StringBuilder outString = new StringBuilder();

            if (targetObject == null) return "{}";

            // string propertyName = string.Empty;

            object result = null;

            Type type = targetObject.GetType();

            //
            // 对象的属性 反射为set_MethodName或者get_MethodName
            //

            MethodInfo[] methods = type.GetMethods();

            string key;

            string value;

            outString.Append("{");

            foreach (MethodInfo method in methods)
            {
                if (method.Name.Contains("get_"))
                {
                    key = method.Name.Substring(4, method.Name.Length - 4);

                    if (banKeys != null && banKeys.Contains(key))
                        continue;

                    if (!string.IsNullOrEmpty(key))
                    {
                        result = type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { });

                        if (namingRule == "camel")
                        {
                            if (AjaxConfigurationView.Instance.Configuration.SpecialWords[key] == null)
                            {
                                key = StringHelper.ToFirstLower(key);
                            }
                            else
                            {
                                key = AjaxConfigurationView.Instance.Configuration.SpecialWords[key].Value;
                            }
                        }
                        else if (namingRule == "underline")
                        {
                            if (AjaxConfigurationView.Instance.Configuration.SpecialWords[key] == null)
                            {
                                key = StringHelper.CamelToUnderline(key);
                            }
                            else
                            {
                                key = AjaxConfigurationView.Instance.Configuration.SpecialWords[key].Value;
                            }
                        }

                        if (result == null)
                        {
                            outString.Append(string.Format(" \"{0}\" : \"{1}\",", key, string.Empty));
                        }
                        else
                        {
                            value = result.ToString();

                            switch (result.GetType().FullName)
                            {
                                case "System.Int16":
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Double":
                                case "System.Decimal":
                                case "System.Guid":
                                    outString.Append(string.Format(" \"{0}\" : \"{1}\",", key, value));
                                    break;

                                case "System.Boolean":
                                    outString.Append(string.Format(" \"{0}\" : \"{1}\",", key, Convert.ToBoolean(value) ? 1 : 0));
                                    break;

                                case "System.DateTime":
                                    AjaxUtil.SerializeDate(outString, namingRule, key, (DateTime)result);
                                    break;

                                case "System.String":
                                    outString.Append(string.Format(" \"{0}\" : \"{1}\",", key, StringHelper.ToSafeJson(value)));
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("}");

            return StringHelper.RemoveEnterTag(outString.ToString());
        }
        #endregion

        //-------------------------------------------------------
        // Xml 字符串 => C# 对象
        //-------------------------------------------------------

        #region 函数:Deserialize<T>(T targetObject, string xml)
        /// <summary>Xml信息反序列化为实体对象</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Deserialize<T>(targetObject, doc);
        }
        #endregion

        #region 函数:Deserialize<T>(T targetObject, XmlDocument doc)
        /// <summary>Xml信息反序列化为实体对象</summary>
        /// <param name="targetObject">需要反序列化的对象</param>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, XmlDocument doc)
        {
            return Deserialize<T>(targetObject, doc, null);
        }
        #endregion

        #region 函数:Deserialize<T>(T targetObject, string xml, List<string> banKeys)
        /// <summary>Xml信息反序列化为实体对象</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, string xml, List<string> banKeys)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Deserialize<T>(targetObject, doc, banKeys);
        }
        #endregion

        #region 函数:Deserialize<T>(T targetObject, XmlDocument doc, List<string> banKeys)
        /// <summary>Xml信息反序列化为实体对象</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, XmlDocument doc, List<string> banKeys)
        {
            return Deserialize<T>(targetObject, doc, banKeys, AjaxConfigurationView.Instance.NamingRule);
        }
        #endregion

        #region 函数:Deserialize<T>(T targetObject, XmlDocument doc, List<string> banKeys)
        /// <summary>Xml信息反序列化为实体对象</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, XmlDocument doc, List<string> banKeys, string namingRule)
        {
            object result = null;

            Type type = targetObject.GetType();

            //
            // 对象的属性 反射为set_MethodName或者get_MethodName
            //

            MethodInfo[] methods = type.GetMethods();

            List<string> methodNames = new List<string>();

            string methodSetName;

            string key;

            string value;

            // 获取所有方法的名称

            foreach (MethodInfo method in methods)
            {
                methodNames.Add(method.Name);
            }

            foreach (MethodInfo method in methods)
            {
                if (method.Name.Contains("get_"))
                {
                    key = method.Name.Substring(4, method.Name.Length - 4);

                    if (banKeys != null && banKeys.Contains(key))
                        continue;

                    if (!string.IsNullOrEmpty(key))
                    {
                        result = type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { });

                        if (namingRule == "camel")
                        {
                            if (AjaxConfigurationView.Instance.Configuration.SpecialWords[key] == null)
                            {
                                key = StringHelper.ToFirstLower(key);
                            }
                            else
                            {
                                key = AjaxConfigurationView.Instance.Configuration.SpecialWords[key].Value;
                            }
                        }
                        else if (namingRule == "underline")
                        {
                            if (AjaxConfigurationView.Instance.Configuration.SpecialWords[key] == null)
                            {
                                key = StringHelper.CamelToUnderline(key);
                            }
                            else
                            {
                                key = AjaxConfigurationView.Instance.Configuration.SpecialWords[key].Value;
                            }
                        }

                        value = XmlHelper.Fetch(key, doc);

                        if (value == null)
                            continue;

                        if (result == null)
                        {
                            result = value;
                        }
                        else
                        {
                            switch (result.GetType().FullName)
                            {
                                case "System.Int16":
                                    result = (string.IsNullOrEmpty(value)) ? (short)0 : Convert.ToInt16(value);
                                    break;
                                case "System.Int32":
                                    result = (string.IsNullOrEmpty(value)) ? 0 : Convert.ToInt32(value);
                                    break;
                                case "System.Int64":
                                    result = (string.IsNullOrEmpty(value)) ? 0 : Convert.ToInt64(value);
                                    break;
                                case "System.Double":
                                    result = (string.IsNullOrEmpty(value)) ? 0 : Convert.ToDouble(value);
                                    break;
                                case "System.Decimal":
                                    result = (string.IsNullOrEmpty(value)) ? 0 : Convert.ToDecimal(value);
                                    break;
                                case "System.Boolean":
                                    if (value == "1" || value == "true" || value == "Yes")
                                    {
                                        result = true;
                                    }
                                    else if (value == "0" || value == "false" || value == "No")
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = (string.IsNullOrEmpty(value)) ? false : Convert.ToBoolean(value);
                                    }
                                    break;
                                case "System.Guid":
                                    result = (string.IsNullOrEmpty(value)) ? Guid.Empty : new Guid(value);
                                    break;
                                case "System.DateTime":
                                    result = (string.IsNullOrEmpty(value)) ? DateHelper.DefaultTime : Convert.ToDateTime(value);
                                    break;

                                case "System.String":
                                default:
                                    result = value;
                                    break;
                            }
                        }

                        methodSetName = method.Name.Replace("get_", "set_");

                        if (methodNames.Contains(methodSetName))
                        {
                            type.InvokeMember(methodSetName, BindingFlags.InvokeMethod, null, targetObject, new object[] { result });
                        }
                    }
                }
            }

            return targetObject;
        }
        #endregion

        private static IDateTimeSerializer serializer = null;

        /// <summary>
        /// 将日期格式的属性序列化为相关的日期的字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static void SerializeDate(StringBuilder outString, string namingRule, string key, DateTime date)
        {
            if (serializer == null)
            {
                serializer = (IDateTimeSerializer)Activator.CreateInstance(Type.GetType(AjaxConfigurationView.Instance.DateTimeSerializer));
            }

            serializer.Serialize(outString, namingRule, key, date);
        }
    }
}
