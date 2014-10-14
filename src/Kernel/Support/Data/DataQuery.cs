// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// Filename     :DataQuery.cs
//
// Description  :���ݲ�ѯ������
//
// Author       :ruanyu@x3platfrom.com
//
// Date			:2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Ajax;
using System.Xml;
using X3Platform.Util;

namespace X3Platform.Data
{
    /// <summary>���ݲ�ѯ��������������</summary>
    public class DataQuery : ISerializedObject
    {
        #region ����:Variables
        private Dictionary<string, string> m_Variables = new Dictionary<string, string>() {
           { "ElevatedPrivileges", "0" }
        };

        /// <summary>���������ı�������</summary>
        public Dictionary<string, string> Variables
        {
            get { return this.m_Variables; }
        }
        #endregion

        #region ����:Table
        private string m_Table = string.Empty;

        /// <summary>���ݱ�����</summary>
        public string Table
        {
            get { return this.m_Table; }
            set { this.m_Table = value; }
        }
        #endregion

        #region ����:Fields
        private IList<string> m_Fields = new List<string>();

        /// <summary>SQL ��ѯ�ֶμ���</summary>
        public IList<string> Fields
        {
            get { return this.m_Fields; }
        }
        #endregion

        #region ����:Where
        private Dictionary<string, object> m_Where = new Dictionary<string, object>();

        /// <summary>SQL ��ѯ�����Ĳ�������</summary>
        public Dictionary<string, object> Where
        {
            get { return this.m_Where; }
        }
        #endregion

        #region ����:Orders
        private IList<string> m_Orders = new List<string>();

        /// <summary>SQL ���������Ĳ�������</summary>
        public IList<string> Orders
        {
            get { return this.m_Orders; }
        }
        #endregion

        #region ����:Limit
        private int m_Limit = 1000;

        /// <summary>�������� Ĭ��ֵ(1000)</summary>
        public int Limit
        {
            get { return this.m_Limit; }
            set { this.m_Limit = value; }
        }
        #endregion

        #region ����:GetWhereSql()
        /// <summary>��ȡ SQL ��ѯ����������</summary>
        /// <returns></returns>
        public string GetWhereSql()
        {
            return this.GetWhereSql(new Dictionary<string, string>());
        }
        #endregion

        #region ����:GetWhereSql(Dictionary<string, string> operators)
        /// <summary>��ȡ SQL ��ѯ����������</summary>
        /// <param name="defaults">Ĭ�������ֶ�</param>
        /// <returns></returns>
        public string GetWhereSql(Dictionary<string, string> operators)
        {
            StringBuilder outString = new StringBuilder();

            foreach (KeyValuePair<string, object> item in this.Where)
            {
                if (item.Value == null) { continue; }

                string typeName = item.Value.GetType().FullName;

                string op = "=";

                foreach (KeyValuePair<string, string> childoperator in operators)
                {
                    if (item.Key.ToUpper() == childoperator.Key.ToUpper())
                    {
                        if (childoperator.Value.ToUpper() == "LIKE" && typeName == "System.String")
                        {
                            op = childoperator.Value.ToUpper();
                        }
                        else
                        {
                            op = childoperator.Value;
                        }
                    }
                }

                switch (typeName)
                {
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Double":
                    case "System.Decimal":
                        outString.AppendFormat("{0} {1} {2}", item.Key, op, item.Value);
                        break;
                    case "System.Boolean":
                        outString.AppendFormat("{0} {1} {2}", item.Key, op, Convert.ToBoolean(item.Value) ? 1 : 0);
                        break;
                    case "System.Guid":
                        outString.AppendFormat("{0} {1} '{2}'", item.Key, op, item.Value);
                        break;
                    case "System.DateTime":
                        outString.AppendFormat("{0} {1} '{2}'", item.Key, op, Convert.ToDateTime(item.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                    case "System.String":
                        if (op == "LIKE")
                        {
                            // Ϊ����Ч�� LIKE ���ݲ���Ϊ��
                            if (!string.IsNullOrEmpty(item.Value.ToString()))
                            {
                                outString.AppendFormat("{0} LIKE '%{1}%'", item.Key, StringHelper.ToSafeSQL(item.Value.ToString()));
                            }
                        }
                        else
                        {
                            outString.AppendFormat("{0} {1} '{2}'", item.Key, op, StringHelper.ToSafeSQL(item.Value.ToString()));
                        }
                        break;
                    case "System.Array":
                        break;
                    default:
                        outString.AppendFormat("{0} {1} '{2}'", item.Key, op, item.Value);
                        break;
                }

                outString.Append(" AND ");
            }

            // �Ƴ�����һ�� AND ������
            if (outString.Length >= 5 && outString.ToString().Substring(outString.Length - 5, 5) == " AND ")
            {
                outString = outString.Remove(outString.Length - 5, 5);
            }

            return outString.ToString();
        }
        #endregion

        #region ����:GetOrderBySql()
        /// <summary>��ȡ SQL ����������</summary>
        /// <returns></returns>
        public string GetOrderBySql()
        {
            return this.GetOrderBySql(string.Empty);
        }
        #endregion

        #region ����:GetOrderBySql(string defaults)
        /// <summary>��ȡ SQL ����������</summary>
        /// <param name="defaults">Ĭ�������ֶ�</param>
        /// <returns></returns>
        public string GetOrderBySql(string defaults)
        {
            if (this.Orders.Count == 0) { return defaults; }

            string orderBy = string.Empty;

            foreach (string item in this.Orders)
            {
                orderBy += StringHelper.ToSafeSQL(item, true) + ",";
            }

            orderBy = orderBy.TrimEnd(',');

            return orderBy;
        }
        #endregion

        // -------------------------------------------------------
        // ʵ�� ISerializedObject ���л�
        // -------------------------------------------------------

        #region ����:Serializable()
        /// <summary>���ݶ��󵼳�XmlԪ��</summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region ����:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>���ݶ��󵼳�XmlԪ��</summary>
        /// <param name="displayComment">��ʾע��</param>
        /// <param name="displayFriendlyName">��ʾ�Ѻ�����</param>
        /// <returns></returns>
        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            string innerText = null;

            outString.Append("<query>");
            outString.AppendFormat("<table><![CDATA[{0}]]></table>", this.Table);

            // �����ֶ���Ϣ
            innerText = string.Empty;

            foreach (string item in this.Fields)
            {
                innerText += item + ",";
            }

            innerText = innerText.TrimEnd(',');

            outString.AppendFormat("<fields><![CDATA[{0}]]></fields>", innerText);

            // �����ֶ���Ϣ
            innerText = string.Empty;

            foreach (KeyValuePair<string, object> item in this.Where)
            {
                innerText += "<key name=\"" + item.Key + "\" type=\"" + item.Value.GetType() + "\" ><![CDATA[" + item.Value + "]]></key>";
            }

            outString.AppendFormat("<where><![CDATA[{0}]]></where>", innerText);

            // ����������Ϣ
            innerText = string.Empty;

            foreach (string item in this.Orders)
            {
                innerText += item + ",";
            }

            innerText = innerText.TrimEnd(',');

            outString.AppendFormat("<orders><![CDATA[{0}]]></orders>", innerText);

            outString.Append("</query>");

            return outString.ToString();
        }
        #endregion

        #region ����:Deserialize(XmlElement element)
        /// <summary>����XmlԪ�ؼ��ض���</summary>
        /// <param name="element">XmlԪ��</param>
        public void Deserialize(System.Xml.XmlElement element)
        {
            XmlNode node = null;

            // Table
            node = element.SelectSingleNode("table");

            this.Table = (node == null) ? string.Empty : node.InnerText;

            // Fields
            node = element.SelectSingleNode("fileds");

            this.Fields.Clear();

            if (node != null)
            {
                string[] fields = node.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string field in fields)
                {
                    this.Fields.Add(field);
                }
            }

            // Where
            XmlNodeList nodes = element.SelectNodes("where/key");

            this.Where.Clear();

            foreach (XmlNode item in nodes)
            {
                if (item.Attributes["type"] == null || item.Attributes["type"].Value == "string")
                {
                    this.Where.Add(item.Attributes["name"].Value, item.InnerText);
                }
                else if (item.Attributes["type"].Value == "int")
                {
                    this.Where.Add(item.Attributes["name"].Value, Convert.ToInt32(item.InnerText));
                }
                else if (item.Attributes["type"].Value == "long")
                {
                    this.Where.Add(item.Attributes["name"].Value, Convert.ToInt64(item.InnerText));
                }
                else if (item.Attributes["type"].Value == "decimal")
                {
                    this.Where.Add(item.Attributes["name"].Value, Convert.ToDecimal(item.InnerText));
                }
                else if (item.Attributes["type"].Value == "date")
                {
                    this.Where.Add(item.Attributes["name"].Value, Convert.ToDateTime(item.InnerText));
                }
            }

            // Orders
            node = element.SelectSingleNode("orders");

            this.Orders.Clear();

            if (node != null)
            {
                string[] orders = node.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string order in orders)
                {
                    this.Fields.Add(order);
                }
            }
        }
        #endregion
    }
}