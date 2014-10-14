// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :JsonHelper.cs
//
// Description  :Json Helper
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2010-01-01
//
// =============================================================================

namespace X3Platform.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using System.Text;
    using System.Reflection;
    using System.Web.Script.Serialization;

    /*
     * How to use object to Json?
     *
     * X3Platform.Util.JsonHelper.ToJson(param, typeof(NewsInfo))
     *
     * X3Platform.Util.JsonHelper.ToJson(list, typeof(NewsInfo))
     *
     */

    /// <summary>����ͨʵ�����������л���Json��ʽ.</summary>
    public sealed class JsonHelper
    {
        // -------------------------------------------------------
        // ������ת��ΪJson��ʽ�ı�
        // -------------------------------------------------------

        #region ����:ToJson<T>(List<T> list, Type type)
        /// <summary>��һ��ʵ��������ת����Json��ʽ</summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToJson<T>(List<T> list, Type type)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":[{");

            foreach (T item in list)
            {
                outString.Append(Convert(item, type));
            }

            outString.Append("]}");

            return outString.ToString();
        }
        #endregion

        #region ����:ToJson(object param, Type type)
        /// <summary>��һ��ʵ��������ת����Json��ʽ</summary>
        /// <param name="param"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToJson(object param, Type type)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":{");

            outString.Append(Convert(param, type));

            outString.Append("}}");

            return outString.ToString();
        }
        #endregion

        #region ����:ToJosn(DataView dataView)
        /// <summary>��һ��DataView����ת����Json��ʽ�ı�</summary>
        /// <param name="dataView"></param>
        /// <returns></returns>
        public static string ToJosn(DataView dataView)
        {
            return ToJosn(dataView, string.Empty, string.Empty, false);
        }
        #endregion

        #region ����:ToJosn(DataView dataView, bool removeAjaxStorageRoot)
        /// <summary>��һ��DataView����ת����Json��ʽ�ı�</summary>
        /// <param name="dataView"></param>
        /// <param name="selectedColumnName"></param>
        /// <param name="selectedValue"></param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataView dataView, bool removeAjaxStorageRoot)
        {
            return ToJosn(dataView, string.Empty, string.Empty, removeAjaxStorageRoot);
        }
        #endregion

        #region ����:ToJosn(DataTable dataView, bool columnNameFirstCharLower, bool removeAjaxStorageRoot)
        /// <summary>��һ��DataView����ת����Json��ʽ�ı�</summary>
        /// <param name="dataTable"></param>
        /// <param name="columnNameFirstCharLower">�������ַ�Сд</param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataView dataView, bool columnNameFirstCharLower, bool removeAjaxStorageRoot)
        {
            if (columnNameFirstCharLower)
            {
                for (int i = 0; i < dataView.Table.Columns.Count; i++)
                {
                    dataView.Table.Columns[i].ColumnName = StringHelper.ToFirstLower(dataView.Table.Columns[i].ColumnName);
                }
            }

            return ToJosn(dataView, string.Empty, string.Empty, removeAjaxStorageRoot);
        }
        #endregion

        #region ����:ToJosn(DataView dataView, string selectedColumnName, string selectedValue, bool removeAjaxStorageRoot)
        /// <summary>��һ��DataView����ת����Json��ʽ�ı�</summary>
        /// <param name="dataView"></param>
        /// <param name="selectedColumnName"></param>
        /// <param name="selectedValue"></param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataView dataView, string selectedColumnName, string selectedValue, bool removeAjaxStorageRoot)
        {
            StringBuilder outString = new StringBuilder();

            string jsonText = string.Empty;

            string dataColumnValue = string.Empty;

            outString.Append(removeAjaxStorageRoot ? "[" : "{\"ajaxStorage\":[");

            if (dataView.Table.Rows.Count > 0)
            {
                foreach (DataRowView dataRowView in dataView)
                {
                    jsonText = "{";

                    foreach (DataColumn dataColumn in dataView.Table.Columns)
                    {
                        // ���ڸ�ʽ����
                        if (dataColumn.DataType == typeof(DateTime))
                        {
                            dataColumnValue = StringHelper.ToSafeJson(System.Convert.ToDateTime(dataRowView[dataColumn.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            dataColumnValue = StringHelper.ToSafeJson(dataRowView[dataColumn.ColumnName].ToString());
                        }

                        // �������ݲ���˫����
                        if (dataColumnValue.Length > 2 && (dataColumnValue.Substring(0, 1) == "[" && dataColumnValue.Substring(0, 1) == "]"))
                        {
                            jsonText += "\"" + dataColumn.ColumnName + "\":" + dataColumnValue.Substring(1, dataColumnValue.Length - 2) + ",";
                        }
                        else
                        {
                            jsonText += "\"" + dataColumn.ColumnName + "\":\"" + dataColumnValue + "\",";
                        }
                    }

                    if (!string.IsNullOrEmpty(selectedColumnName))
                    {
                        jsonText += ",\"selected\":\"" + (selectedValue == StringHelper.ToSafeJson(dataRowView[selectedColumnName].ToString())) + "\"";
                    }

                    jsonText = jsonText.TrimEnd(',') + "},";

                    outString.Append(jsonText);
                }

                if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                {
                    outString = outString.Remove(outString.Length - 1, 1);
                }
            }

            outString.Append(removeAjaxStorageRoot ? "]" : "]}");

            return outString.ToString();
        }
        #endregion

        #region ����:ToJosn(DataTable dataTable)
        /// <summary>��һ��DataTable����ת����Json��ʽ�ı�</summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string ToJosn(DataTable dataTable)
        {
            return ToJosn(dataTable, string.Empty, string.Empty, false);
        }
        #endregion

        #region ����:ToJosn(DataTable dataTable, bool removeAjaxStorageRoot)
        /// <summary>��һ��DataView����ת����Json��ʽ�ı�</summary>
        /// <param name="dataTable"></param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataTable dataTable, bool removeAjaxStorageRoot)
        {
            return ToJosn(dataTable, string.Empty, string.Empty, removeAjaxStorageRoot);
        }
        #endregion

        #region ����:ToJosn(DataTable dataTable, bool columnNameFirstCharLower, bool removeAjaxStorageRoot)
        /// <summary>��һ��DataTable����ת����Json��ʽ�ı�</summary>
        /// <param name="dataTable"></param>
        /// <param name="columnNameFirstCharLower">�������ַ�Сд</param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataTable dataTable, bool columnNameFirstCharLower, bool removeAjaxStorageRoot)
        {
            if (columnNameFirstCharLower)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    dataTable.Columns[i].ColumnName = StringHelper.ToFirstLower(dataTable.Columns[i].ColumnName);
                }
            }

            return ToJosn(dataTable, string.Empty, string.Empty, removeAjaxStorageRoot);
        }
        #endregion

        #region ����:ToJosn(DataTable dataTable, bool removeAjaxStorageRoot)
        /// <summary>�� DataTable ��ʽ������ת����Json����ʽ������</summary>
        /// <param name="dataTable">����Դ</param>
        /// <param name="selectedColumnName">������ƥ��������</param>
        /// <param name="selectedValue">������ѡ������ֵ</param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataTable dataTable, string selectedColumnName, string selectedValue, bool removeAjaxStorageRoot)
        {
            StringBuilder outString = new StringBuilder();

            string jsonText = string.Empty;

            string dataColumnValue = string.Empty;

            List<string> colNameList = new List<string>();

            outString.Append(removeAjaxStorageRoot ? "[" : "{\"ajaxStorage\":[");

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    jsonText = "{";

                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        // ���ڸ�ʽ����
                        if (dataColumn.DataType == typeof(DateTime))
                        {
                            dataColumnValue = StringHelper.ToSafeJson(System.Convert.ToDateTime(dataRow[dataColumn.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            dataColumnValue = StringHelper.ToSafeJson(dataRow[dataColumn.ColumnName].ToString());
                        }

                        // �������ݲ���˫����
                        if (dataColumnValue.Length > 2 && (dataColumnValue.Substring(0, 1) == "[" && dataColumnValue.Substring(0, 1) == "]"))
                        {
                            jsonText += "\"" + dataColumn.ColumnName + "\":" + dataColumnValue.Substring(1, dataColumnValue.Length - 2) + ",";
                        }
                        else
                        {
                            jsonText += "\"" + dataColumn.ColumnName + "\":\"" + dataColumnValue + "\",";
                        }
                    }

                    if (!string.IsNullOrEmpty(selectedColumnName))
                    {
                        jsonText += ",\"selected\":\"" + (selectedValue == StringHelper.ToSafeJson(dataRow[selectedColumnName].ToString())) + "\"";
                    }

                    jsonText = jsonText.TrimEnd(',') + "},";

                    outString.Append(jsonText);
                }

                if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                {
                    outString = outString.Remove(outString.Length - 1, 1);
                }
            }

            outString.Append(removeAjaxStorageRoot ? "]" : "]}");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:Convert(object param, Type type)
        /// <summary>ת��</summary>
        /// <param name="param"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string Convert(object param, Type type)
        {
            StringBuilder outString = new StringBuilder();

            string propertyName = string.Empty;

            object result = null;

            // ���������� ����Ϊset_MethodName����get_MethodName

            MethodInfo[] methods = type.GetMethods();

            foreach (MethodInfo method in methods)
            {
                if (method.Name.Contains("get_"))
                {
                    propertyName = method.Name.Substring(4, method.Name.Length - 4);

                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        result = type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, param, new object[] { });

                        outString.Append(string.Format("\"{0}\":\"{1}\",", StringHelper.ToFirstLower(propertyName), StringHelper.ToSafeJson(result.ToString())));
                    }
                }
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 2.��Json�ַ���ת��ΪXml�ַ���.
        // -------------------------------------------------------

        #region ����:ToXmlDocument(string json)
        /// <summary>��Json��ʽתΪXML Document����</summary>
        /// <param name="json">Json��ʽ ����.</param>
        /// <returns>XML Document ����.</returns>
        public static XmlDocument ToXmlDocument(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new XmlDocument();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{0}", ToXml(json)));

            return doc;
        }
        #endregion

        #region ����:ToXml(string json)
        /// <summary>��һ��Json�ַ�����ʽ��ΪXml��Ϣ.</summary>
        /// <param name="json">Json�ַ���</param>
        /// <returns>XML��ʽ ����.</returns>
        public static string ToXml(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            /*
             * Test case :
             *
             * { "ajaxStorage":"hello world."}
             *
             * { "ajaxStorage":{"name": "Max","msn": "ruanyu1983@msn.com"}}
             *
             * { "ajaxStorage":[{"name": "Max","msn": "ruanyu1983@msn.com"},{"name": "Max","msn":"ruanyu1983@msn.com"}]}
             *
             *  hidden key
             */

            StringBuilder outString = new StringBuilder();

            json = json.Replace("\r", "");
            json = json.Replace("\n", "");
            json = json.Replace("\t", "");
            json = json.Replace("\"\"", "\"{NULL}\"");
            json = json.Replace("<", "{&lt;}");
            json = json.Replace(">", "{&gt;}");

            ConvertToXml(outString, json);

            outString = outString.Replace("{NULL}", "");

            //outString = outString.Replace("{&lt;}", "<");
            //outString = outString.Replace("{&gt;}", ">");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:ConvertToXml(StringBuilder outString, string node)
        private static void ConvertToXml(StringBuilder outString, string node)
        {
            if (node.IndexOf(":") == -1)
                return;

            node = node.Trim();

            string key = null;

            string value = null;

            string[] nodes = null;

            string[] childNodes = null;

            string temp = null;

            if (node[0] == '{')
                node = node.Substring(node.IndexOf("{") + 1, node.LastIndexOf("}") - node.IndexOf("{") - 1);

            if (node[node.Length - 1] == '}')
                nodes = new string[] { node };
            else if (node[0] != '{' && node[node.Length - 1] != '}' && node[node.Length - 1] != ']')
                nodes = node.Split(',');
            else
                nodes = new string[] { node };

            foreach (string text in nodes)
            {
                temp = text;

                key = temp.Substring(0, temp.IndexOf(":")).Replace("\"", "").Trim();

                value = temp.Substring(temp.IndexOf(":") + 1, temp.Length - temp.IndexOf(":") - 1).Replace("\"", "").Trim();

                outString.Append("<" + key + ">");

                if (value[0] == '{')
                {
                    temp = value.Substring(value.IndexOf("{") + 1, value.LastIndexOf("}") - value.IndexOf("{") - 1);

                    childNodes = temp.Split(',');

                    foreach (string childNode in childNodes)
                    {
                        ConvertToXml(outString, childNode);
                    }
                }
                else if (value[0] == '[')
                {
                    temp = value.Substring(value.IndexOf("[") + 1, value.LastIndexOf("]") - value.IndexOf("[") - 1).Replace("\"", "").Trim();

                    temp = temp.Replace("},", "}&");

                    childNodes = temp.Split('&');

                    foreach (string childNode in childNodes)
                    {
                        ConvertToXml(outString, childNode);
                    }
                }
                else
                {
                    outString.Append(value);
                }

                outString.Append("</" + key + ">");
            }
        }
        #endregion

        // -------------------------------------------------------
        // 3.��Json�ַ���ת��ΪHashTable.
        // -------------------------------------------------------

        #region ����:ToHashtable(string json)
        /// <summary>��һ��Json�ַ�����ʽ��ΪXml��Ϣ.</summary>
        /// <param name="json">Json�ַ���</param>
        /// <returns>XML��ʽ ����.</returns>
        public static Hashtable ToHashtable(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new Hashtable();

            /*
             * Test case :
             *
             * { "ajaxStorage":"hello world."}
             *
             * { "ajaxStorage":{"name": "Max","msn": "ruanyu1983@msn.com"}}
             *
             * { "ajaxStorage":[{"name": "Max","msn": "ruanyu1983@msn.com"},{"name": "Max","msn":"ruanyu1983@msn.com"}]}
             *
             *  hidden key
             */

            Hashtable hashtable = new Hashtable();

            Dictionary<string, object> dictionary = ToDictionary(json);

            foreach (KeyValuePair<string, object> item in dictionary)
            {
                hashtable[item.Key] = item.Value;
            }

            return hashtable;
        }
        #endregion

        #region ����:ToDictionary(string json)
        /// <summary>��һ��Json�ַ�����ʽ��ΪXml��Ϣ.</summary>
        /// <param name="json">Json�ַ���</param>
        /// <returns>XML��ʽ ����.</returns>
        public static Dictionary<string, object> ToDictionary(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new Dictionary<string, object>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Dictionary<string, object> dictionary = serializer.Deserialize<Dictionary<string, object>>(json);

            if (dictionary["ajaxStorage"] != null)
            {
                return (Dictionary<string, object>)dictionary["ajaxStorage"];
            }
            else
            {
                return dictionary;
            }
        }
        #endregion
    }
}