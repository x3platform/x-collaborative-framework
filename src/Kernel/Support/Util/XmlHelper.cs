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

        //-------------------------------------------------------
        // 抓取 Xml 文档中节点的内容
        //-------------------------------------------------------

        #region 函数:Fetch(string nodeName, string xml)
        /// <summary>抓取关键字的值</summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="xml">Xml 字符串</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Fetch(nodeName, doc);
        }
        #endregion

        #region 函数:Fetch(string nodeName, XmlDocument doc)
        /// <summary>抓取关键字的值</summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlDocument doc)
        {
            return Fetch(nodeName, doc, "text");
        }
        #endregion

        #region 函数:Fetch(string nodeName, XmlDocument doc, string resultType)
        /// <summary>抓取关键字的值</summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="doc">Xml 文档对象</param>
        /// <param name="resultType">返回类型 text | xml</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlDocument doc, string resultType)
        {
            return Fetch(nodeName, doc.DocumentElement, resultType);
        }
        #endregion

        #region 函数:Fetch(string nodeName, XmlElement element)
        /// <summary>抓取关键字的值</summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="element">Xml 元素</param>
        /// <returns></returns>
        public static string Fetch(string nodeName, XmlElement element)
        {
            return Fetch(nodeName, element, "text");
        }
        #endregion

        #region 函数:Fetch(string nodeName, XmlElement element, string resultType)
        /// <summary>抓取关键字的值</summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="element">Xml 元素</param>
        /// <param name="resultType">返回类型 text | xml</param>
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

        #region 函数:Fetch(string parentNodeName, string nodeName, string xml)
        /// <summary>抓取关键字的值</summary>
        /// <param name="parentNodeName">父节点名称</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Fetch(parentNodeName, nodeName, doc);
        }
        #endregion

        #region 函数:Fetch(string parentNodeName, string nodeName, XmlDocument doc)
        /// <summary>抓取关键字的值</summary>
        /// <param name="parentNodeName">父节点名称</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlDocument doc)
        {
            return Fetch(parentNodeName, nodeName, doc.DocumentElement, "text");
        }
        #endregion

        #region 函数:Fetch(string parentNodeName, string nodeName, XmlDocument doc, string resultType)
        /// <summary>抓取关键字的值</summary>
        /// <param name="parentNodeName">父节点名称</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="doc">Xml 文档对象</param>
        /// <param name="resultType">返回类型 text | xml</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlDocument doc, string resultType)
        {
            return Fetch(parentNodeName, nodeName, doc.DocumentElement, resultType);
        }
        #endregion

        #region 函数:Fetch(string parentNodeName, string nodeName, XmlElement element)
        /// <summary>抓取关键字的值</summary>
        /// <param name="parentNodeName">父节点名称</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="element">Xml 元素</param>
        /// <returns></returns>
        public static string Fetch(string parentNodeName, string nodeName, XmlElement element)
        {
            return Fetch(parentNodeName, nodeName, element, "text");
        }
        #endregion

        #region 函数:Fetch(string parentNodeName, string nodeName, XmlElement element, string resultType)
        /// <summary>抓取关键字的值</summary>
        /// <param name="parentNodeName">父节点名称</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="element">Xml 元素</param>
        /// <param name="resultType">返回类型 text | xml</param>
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

        //-------------------------------------------------------
        // 抓取 Xml 节点的属性信息
        //-------------------------------------------------------

        #region 函数:FetchNodeAttribute(XmlNode node, string attributeName)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string FetchNodeAttribute(XmlNode node, string attributeName)
        {
            return FetchNodeAttribute(node, attributeName, string.Empty);
        }
        #endregion

        #region 函数:FetchNodeAttribute(XmlNode node, string attributeName, string defaultValue)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string FetchNodeAttribute(XmlNode node, string attributeName, string defaultValue)
        {
            if (node == null) { return defaultValue; }

            XmlElement element = (XmlElement)node;

            return FetchNodeAttribute(element, attributeName, defaultValue);
        }
        #endregion

        #region 函数:FetchNodeAttribute(XmlElement element, string attributeName)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string FetchNodeAttribute(XmlElement element, string attributeName)
        {
            return FetchNodeAttribute(element, attributeName, string.Empty);
        }
        #endregion

        #region 函数:FetchNodeAttribute(XmlElement element, string attributeName, string defaultValue)
        /// <summary>尝试获取节点标签属性</summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string FetchNodeAttribute(XmlElement element, string attributeName, string defaultValue)
        {
            string result = null;

            if (element != null)
            {
                result = element.GetAttribute(attributeName);
            }

            if (result == "undefined") { result = null; }

            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }
        #endregion

        //-------------------------------------------------------
        // 抓取 Xml 子节点相关信息
        //-------------------------------------------------------

        #region 函数:FetchChildNode(XmlElement element, string childNodeName)
        public static XmlElement FetchChildNode(XmlElement element, string childNodeName)
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
        #endregion

        #region 函数:FetchChildNodeAttribute(XmlElement element, string childNodeName, string attributeName)
        /// <summary>抓取子节点属性</summary>
        /// <param name="element">Xml 元素</param>
        /// <param name="childNodeName">子节点名称</param>
        /// <param name="attributeName">属性名称</param>
        /// <returns></returns>
        public static string FetchChildNodeAttribute(XmlElement element, string childNodeName, string attributeName)
        {
            XmlElement node = FetchChildNode(element, childNodeName);

            return node == null ? string.Empty : FetchNodeAttribute(node, attributeName);
        }
        #endregion
    }

    // -------------------------------------------------------
    // 扩展方法
    // -------------------------------------------------------

}
