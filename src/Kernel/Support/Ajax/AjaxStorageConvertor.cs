// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxStorageConvertor.cs
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
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml;

    using X3Platform.Util;
    using X3Platform.Ajax.Configuration;

    // using X3Platform.Ajax.Configuration;

    /// <summary></summary>
    public class AjaxStorageConvertor
    {

        //-------------------------------------------------------
        // C# ���� => JOSN ����
        //-------------------------------------------------------

        #region ����:Parse<T>(IList<T> list)
        /// <summary>��������ΪJOSN�ַ���</summary>
        public static string Parse<T>(IList<T> list)
        {
            return Parse<T>(list, null);
        }
        #endregion

        #region ����:Parse<T>(IList<T> list, List<string> banKeys)
        /// <summary>��������ΪJOSN�ַ���</summary>
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

        #region ����:Parse<T>(T targetObject)
        /// <summary>��������ΪJOSN�ַ���</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static string Parse<T>(T targetObject)
        {
            return Parse<T>(targetObject, null);
        }
        #endregion

        #region ����:Parse<T>(T targetObject, List<string> banKeys)
        /// <summary>��������ΪJOSN�ַ���</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <param name="banKeys">��ֹת���ļ�</param>
        /// <returns></returns>
        public static string Parse<T>(T targetObject, List<string> banKeys)
        {
            StringBuilder outString = new StringBuilder();

            if (targetObject == null)
                return "{\"id\":\"0\", \"value\":\"the object is null.\"}";

            // string propertyName = string.Empty;

            object result = null;

            Type type = targetObject.GetType();

            //
            // ���������� ����Ϊset_MethodName����get_MethodName
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

                        if (AjaxConfigurationView.Instance.CamelStyle == "ON")
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

                                    value = ((DateTime)result).ToString("yyyy-MM-dd HH:mm:ss.fff");

                                    outString.Append(string.Format(" \"{0}\" : \"{1}\",", key, (string.IsNullOrEmpty(value) ? value : ((DateTime)result).ToString("yyyy,MM,dd,HH,mm,ss"))));
                                    outString.Append(string.Format(" \"{0}View\" : \"{1}\",", key, StringHelper.ToDate(value)));
                                    outString.Append(string.Format(" \"{0}TimestampView\" : \"{1}\",", key, value));
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
        // Xml �ַ��� => C# ����
        //-------------------------------------------------------

        #region ����:Deserialize<T>(T targetObject, string xml)
        /// <summary>Xml��Ϣ�����л�Ϊʵ������</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Deserialize<T>(targetObject, doc);
        }
        #endregion

        #region ����:Deserialize<T>(T targetObject, XmlDocument doc)
        /// <summary>Xml��Ϣ�����л�Ϊʵ������</summary>
        /// <param name="targetObject">��Ҫ�����л��Ķ���</param>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, XmlDocument doc)
        {
            return Deserialize<T>(targetObject, doc, null);
        }
        #endregion

        #region ����:Deserialize<T>(T targetObject, string xml, List<string> banKeys)
        /// <summary>Xml��Ϣ�����л�Ϊʵ������</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, string xml, List<string> banKeys)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Deserialize<T>(targetObject, doc, banKeys);
        }
        #endregion

        #region ����:Deserialize<T>(T targetObject, XmlDocument doc, List<string> banKeys)
        /// <summary>Xml��Ϣ�����л�Ϊʵ������</summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T targetObject, XmlDocument doc, List<string> banKeys)
        {
            object result = null;

            Type type = targetObject.GetType();

            //
            // ���������� ����Ϊset_MethodName����get_MethodName
            //

            MethodInfo[] methods = type.GetMethods();

            List<string> methodNames = new List<string>();

            string methodSetName;

            string key;

            string value;

            // ��ȡ���з���������

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

                        if (AjaxConfigurationView.Instance.CamelStyle == "ON")
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

                        value = Fetch(key, doc);

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
                                    result = (string.IsNullOrEmpty(value)) ? false : Convert.ToBoolean(value);
                                    break;
                                case "System.Guid":
                                    result = (string.IsNullOrEmpty(value)) ? Guid.Empty : new Guid(value);
                                    break;
                                case "System.DateTime":
                                    result = (string.IsNullOrEmpty(value)) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(value);
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

        //-------------------------------------------------------
        // ץȡ Xml �ĵ��нڵ�������
        //-------------------------------------------------------

        #region ����:Fetch(string nodeName, string xml)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="xml">Xml �ַ���</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Fetch(nodeName, doc);
        }
        #endregion

        #region ����:Fetch(string nodeName, XmlDocument doc)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlDocument doc)
        {
            return Fetch(nodeName, doc, "text");
        }
        #endregion

        #region ����:Fetch(string nodeName, XmlDocument doc, string resultType)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <param name="resultType">�������� text | xml</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlDocument doc, string resultType)
        {
            return Fetch(nodeName, doc.DocumentElement, resultType);
        }
        #endregion

        #region ����:Fetch(string nodeName, XmlElement element)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="element">Xml Ԫ��</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlElement element)
        {
            return Fetch(nodeName, element, "text");
        }
        #endregion

        #region ����:Fetch(string nodeName, XmlElement element, string resultType)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="element">Xml Ԫ��</param>
        /// <param name="resultType">�������� text | xml</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlElement element, string resultType)
        {
            XmlNodeList nodes = element.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.Name == nodeName)
                {
                    if (resultType == "xml")
                    {
                        return node.InnerXml;
                    }
                    else
                    {
                        return node.InnerText;
                    }
                }
            }

            return null;
        }
        #endregion

        #region ����:Fetch(string parentNodeName, string nodeName, string xml)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="parentNodeName">���ڵ�����</param>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Fetch(parentNodeName, nodeName, doc);
        }
        #endregion

        #region ����:Fetch(string parentNodeName, string nodeName, XmlDocument doc)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="parentNodeName">���ڵ�����</param>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlDocument doc)
        {
            return Fetch(parentNodeName, nodeName, doc.DocumentElement, "text");
        }
        #endregion

        #region ����:Fetch(string parentNodeName, string nodeName, XmlDocument doc, string resultType)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="parentNodeName">���ڵ�����</param>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <param name="resultType">�������� text | xml</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlDocument doc, string resultType)
        {
            return Fetch(parentNodeName, nodeName, doc.DocumentElement, resultType);
        }
        #endregion

        #region ����:Fetch(string parentNodeName, string nodeName, XmlElement element)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="parentNodeName">���ڵ�����</param>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="element">Xml Ԫ��</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlElement element)
        {
            return Fetch(parentNodeName, nodeName, element, "text");
        }
        #endregion

        #region ����:Fetch(string parentNodeName, string nodeName, XmlElement element, string resultType)
        /// <summary>ץȡ�ؼ��ֵ�ֵ</summary>
        /// <param name="parentNodeName">���ڵ�����</param>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="element">Xml Ԫ��</param>
        /// <param name="resultType">�������� text | xml</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlElement element, string resultType)
        {
            XmlNodeList nodes = element.GetElementsByTagName(parentNodeName)[0].ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.Name == nodeName)
                {
                    if (resultType == "xml")
                    {
                        return node.InnerXml;
                    }
                    else
                    {
                        return node.InnerText;
                    }
                }
            }

            return null;
        }
        #endregion
    }
}
