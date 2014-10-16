namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    #endregion

    /// <summary>XML 数据处理辅助类</summary>
    public class XmlHelper
    {
        #region 函数:ToXml(object value)
        /// <summary>将一个对象序列化为Xml信息.</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXml(object value)
        {
            XmlDocument doc = new XmlDocument();

            Type type = value.GetType();

            using (StringWriter stringWriter = new StringWriter())
            {
                XmlSerializer ser = new XmlSerializer(type);

                ser.Serialize(stringWriter, value);

                doc.LoadXml(stringWriter.GetStringBuilder().ToString());
            }

            return doc.DocumentElement.InnerXml;
        }
        #endregion

        #region 函数:ToXml(string json)
        /// <summary>将一个Json字符串格式化为Xml信息.</summary>
        /// <param name="json">Json字符串</param>
        /// <returns>XML格式 数据.</returns>
        public static string ToXml(string json)
        {
            /*
             * Test case :
             *
             * { "ajaxStorage":"hello world."}
             *
             * { "ajaxStorage":{"name": "Max","msn": "ruanyu1983@msn.com"}}
             *
             * { "ajaxStorage":[{"name": "Max","msn": "ruanyu1983@msn.com"},{"name": "Max","msn":"ruanyu1983@msn.com"}]}
             *
             * { "ajaxStorage":[{"name": "Max","msn": "ruanyu1983@msn.com"},{"name": "Max","msn":"ruanyu1983@msn.com"}]}
             *
             *  hidden key
             */

            StringBuilder outString = new StringBuilder();

            json = json.Replace("\r", "");
            json = json.Replace("\n", "");
            json = json.Replace("\t", "");
            json = json.Replace("\", \"", "\",\"");
            json = json.Replace("\"\"", "\"{空白}\"");
            json = json.Replace("\\\"", "{双引号}");
            json = json.Replace("<", "{左尖括号}");
            json = json.Replace(">", "{右尖括号}");

            ConvertJsonToXml(outString, json);

            outString = outString.Replace("{空白}", "");
            outString = outString.Replace("{双引号}", "\"");
            outString = outString.Replace("{左尖括号}", "<");
            outString = outString.Replace("{右尖括号}", ">");

            return outString.ToString();
        }
        #endregion

        #region 私有函数:ConvertJsonToXml(StringBuilder outString, string node)
        private static void ConvertJsonToXml(StringBuilder outString, string node)
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
                nodes = node.Split(new string[] { "\",\"" }, StringSplitOptions.None);
            else
                nodes = new string[] { node };

            foreach (string text in nodes)
            {
                temp = text;

                key = temp.Substring(0, temp.IndexOf(":"));

                value = temp.Substring(temp.IndexOf(":") + 1, temp.Length - temp.IndexOf(":") - 1);

                outString.Append("<" + key.Replace("\"", "").Replace("\'", "").Trim() + ">");

                if (value[0] == '{')
                {
                    temp = value.Substring(value.IndexOf("{") + 1, value.LastIndexOf("}") - value.IndexOf("{") - 1);

                    childNodes = temp.Split(',');

                    foreach (string childNode in childNodes)
                    {
                        ConvertJsonToXml(outString, childNode);
                    }
                }
                else if (value[0] == '[')
                {
                    temp = value.Substring(value.IndexOf("[") + 1, value.LastIndexOf("]") - value.IndexOf("[") - 1);

                    temp = temp.Replace("},", "}&");

                    childNodes = temp.Split('&');


                    foreach (string childNode in childNodes)
                    {
                        outString.Append("<node>");

                        ConvertJsonToXml(outString, childNode);

                        outString.Append("</node>");
                    }
                }
                else
                {
                    outString.Append("<![CDATA[" + value.Replace("\"", "").Replace("\'", "").Trim() + "]]>");
                }

                outString.Append("</" + key.Replace("\"", "").Replace("\'", "").Trim() + ">");
            }

        }
        #endregion

        #region 函数:ToXmlDocument(string json)
        /// <summary>
        /// 将Json格式转为XML Document对象.
        /// </summary>
        /// <param name="text">Json格式 数据.</param>
        /// <returns>XML Document 对象.</returns>
        public static XmlDocument ToXmlDocument(string json)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n{0}", ToXml(json)));

            return doc;
        }
        #endregion

        #region 函数:ToXmlTable(DataTable table)
        /// <summary>将一个DataTable转化为Xml信息.</summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string ToXmlTable(DataTable table)
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendFormat("<{0}>", table.TableName);

            for (int x = 0; x < table.Rows.Count; x++)
            {
                for (int y = 0; y < table.Columns.Count; y++)
                {
                    outString.AppendFormat("<{0}>{1}</{0}>", table.Columns[y].ColumnName, table.Rows[x][y]);
                }
            }
            outString.AppendFormat("</{0}>", table.TableName);

            return outString.ToString();
        }
        #endregion

        #region 函数:IsNullOrEmpty(string value)
        /// <summary>判断是否是空的Xml信息.</summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string value)
        {
            return (string.IsNullOrEmpty(value) || value == "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\" ?>");
        }
        #endregion

        #region 函数:TryFetchNodeAttribute(XmlNode node, string attributeName)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string TryFetchNodeAttribute(XmlNode node, string attributeName)
        {
            return TryFetchNodeAttribute(node, attributeName, string.Empty);
        }
        #endregion

        #region 函数:TryFetchNodeAttribute(XmlNode node, string attributeName, string defaultValue)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string TryFetchNodeAttribute(XmlNode node, string attributeName, string defaultValue)
        {
            if (node == null)
                return defaultValue;

            XmlElement element = (XmlElement)node;

            return TryFetchNodeAttribute(element, attributeName, string.Empty);
        }
        #endregion

        #region 函数:TryFetchNodeAttribute(XmlElement element, string attributeName)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string TryFetchNodeAttribute(XmlElement element, string attributeName)
        {
            return TryFetchNodeAttribute(element, attributeName, string.Empty);
        }
        #endregion

        #region 函数:TryFetchNodeAttribute(XmlElement element, string attributeName, string defaultValue)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string TryFetchNodeAttribute(XmlElement element, string attributeName, string defaultValue)
        {
            string result = null;

            if (element != null)
            {
                result = element.GetAttribute(attributeName);
            }

            if (result == "undefined")
                result = null;

            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }
        #endregion

        public static XmlElement TryFetchChildNode(XmlElement element, string childNodeName)
        {
            XmlNodeList nodes = element.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.Name == childNodeName)
                {
                    return (XmlElement)node;
                }
            }

            return null;
        }

        public static string TryFetchChildNodeAttribute(XmlElement element, string childNodeName, string attributeName)
        {
            XmlElement node = TryFetchChildNode(element, childNodeName);

            return node == null ? string.Empty : node.GetAttribute(attributeName);
        }
    }
}
