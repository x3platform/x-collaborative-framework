namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using System.Text;
    using System.Reflection;
    using System.Web.Script.Serialization;
    #endregion

    /// <summary>JSON 数据处理辅助类</summary>
    public sealed class JsonHelper
    {
        // -------------------------------------------------------
        // 将对象转换为Json格式文本
        // -------------------------------------------------------

        #region 函数:ToJson<T>(List<T> list, Type type)
        /// <summary>将一个实体类对象转换到Json格式</summary>
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

        #region 函数:ToJson(object param, Type type)
        /// <summary>将一个实体类对象转换到Json格式</summary>
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

        #region 函数:ToJosn(DataView dataView)
        /// <summary>将一个DataView对象转换到Json格式文本</summary>
        /// <param name="dataView"></param>
        /// <returns></returns>
        public static string ToJosn(DataView dataView)
        {
            return ToJosn(dataView, string.Empty, string.Empty, false);
        }
        #endregion

        #region 函数:ToJosn(DataView dataView, bool removeAjaxStorageRoot)
        /// <summary>将一个DataView对象转换到Json格式文本</summary>
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

        #region 函数:ToJosn(DataTable dataView, bool columnNameFirstCharLower, bool removeAjaxStorageRoot)
        /// <summary>将一个DataView对象转换到Json格式文本</summary>
        /// <param name="dataTable"></param>
        /// <param name="columnNameFirstCharLower">列名首字符小写</param>
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

        #region 函数:ToJosn(DataView dataView, string selectedColumnName, string selectedValue, bool removeAjaxStorageRoot)
        /// <summary>将一个DataView对象转换到Json格式文本</summary>
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
                        // 日期格式数据
                        if (dataColumn.DataType == typeof(DateTime))
                        {
                            dataColumnValue = StringHelper.ToSafeJson(System.Convert.ToDateTime(dataRowView[dataColumn.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            dataColumnValue = StringHelper.ToSafeJson(dataRowView[dataColumn.ColumnName].ToString());
                        }

                        // 数组数据不加双引号
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

        #region 函数:ToJosn(DataTable dataTable)
        /// <summary>将一个DataTable对象转换到Json格式文本</summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string ToJosn(DataTable dataTable)
        {
            return ToJosn(dataTable, string.Empty, string.Empty, false);
        }
        #endregion

        #region 函数:ToJosn(DataTable dataTable, bool removeAjaxStorageRoot)
        /// <summary>将一个DataView对象转换到Json格式文本</summary>
        /// <param name="dataTable"></param>
        /// <param name="removeAjaxStorageRoot"></param>
        /// <returns></returns>
        public static string ToJosn(DataTable dataTable, bool removeAjaxStorageRoot)
        {
            return ToJosn(dataTable, string.Empty, string.Empty, removeAjaxStorageRoot);
        }
        #endregion

        #region 函数:ToJosn(DataTable dataTable, bool columnNameFirstCharLower, bool removeAjaxStorageRoot)
        /// <summary>将一个DataTable对象转换到Json格式文本</summary>
        /// <param name="dataTable"></param>
        /// <param name="columnNameFirstCharLower">列名首字符小写</param>
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

        #region 函数:ToJosn(DataTable dataTable, bool removeAjaxStorageRoot)
        /// <summary>将 DataTable 形式的数据转换成Json的形式的数据</summary>
        /// <param name="dataTable">数据源</param>
        /// <param name="selectedColumnName">下拉框匹配的列名</param>
        /// <param name="selectedValue">下拉框选中项的值</param>
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
                        // 日期格式数据
                        if (dataColumn.DataType == typeof(DateTime))
                        {
                            dataColumnValue = StringHelper.ToSafeJson(System.Convert.ToDateTime(dataRow[dataColumn.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            dataColumnValue = StringHelper.ToSafeJson(dataRow[dataColumn.ColumnName].ToString());
                        }

                        // 数组数据不加双引号
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

        #region 私有函数:Convert(object param, Type type)
        /// <summary>转换</summary>
        /// <param name="param"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string Convert(object param, Type type)
        {
            StringBuilder outString = new StringBuilder();

            string propertyName = string.Empty;

            object result = null;

            // 对象的属性 反射为set_MethodName或者get_MethodName

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
        // 2.将Json字符串转换为Xml字符串.
        // -------------------------------------------------------

        #region 函数:ToXmlDocument(string json)
        /// <summary>将Json格式转为XML Document对象</summary>
        /// <param name="json">Json格式 数据.</param>
        /// <returns>XML Document 对象.</returns>
        public static XmlDocument ToXmlDocument(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new XmlDocument();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{0}", ToXml(json)));

            return doc;
        }
        #endregion

        #region 函数:ToXml(string json)
        /// <summary>将一个Json字符串格式化为Xml信息.</summary>
        /// <param name="json">Json字符串</param>
        /// <returns>XML格式 数据.</returns>
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

        #region 私有函数:ConvertToXml(StringBuilder outString, string node)
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
        // 3.将Json字符串转换为HashTable.
        // -------------------------------------------------------

        #region 函数:ToHashtable(string json)
        /// <summary>将一个Json字符串格式化为Xml信息.</summary>
        /// <param name="json">Json字符串</param>
        /// <returns>XML格式 数据.</returns>
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

        #region 函数:ToDictionary(string json)
        /// <summary>将一个Json字符串格式化为Xml信息.</summary>
        /// <param name="json">Json字符串</param>
        /// <returns>XML格式 数据.</returns>
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