// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxSqlExpression.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace X3Platform.Ajax
{
    /// <summary>Ajax Sql ����ʽ</summary>
    [Serializable()]
    public class AjaxSqlExpression : AjaxSqlParameter
    {
        #region ����:Parent
        private AjaxSqlExpression m_Parent = null;

        /// <summary>��������ʽ</summary>
        public AjaxSqlExpression Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
        #endregion

        #region ����:SubExpressionList
        private IList<AjaxSqlExpression> list = new List<AjaxSqlExpression>();

        /// <summary>�ӱ���ʽ�б�</summary>
        public IList<AjaxSqlExpression> SubExpressionList
        {
            get { return list; }
            set { list = value; }
        }
        #endregion

        #region ����:Prefix
        private string m_Prefix = null;

        /// <summary>ֵ������</summary>
        [XmlAttribute("prefix")]
        public string Prefix
        {
            get { return m_Prefix; }
            set { m_Prefix = value; }
        }
        #endregion

        #region ���캯��:AjaxSqlExpression()
        /// <summary></summary>
        public AjaxSqlExpression()
        {
        }
        #endregion

        #region ���캯��:AjaxSqlExpression(string key, string value, string type, string prefix)
        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        public AjaxSqlExpression(string key, string value, string type, string prefix)
        {
            Key = key;
            Value = value;
            Type = type;
            Prefix = prefix;
        }
        #endregion

        #region ����:LoadXml(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public void LoadXml(XmlElement element)
        {
            Key = element.GetAttribute("key");
            Operation = string.IsNullOrEmpty(element.GetAttribute("operation")) ? "=" : element.GetAttribute("operation").Replace("!=", "<>");
            Value = element.GetAttribute("value");
            Type = element.GetAttribute("type");

            Prefix = element.GetAttribute("prefix").ToUpper();

            XmlNodeList nodes = element.ChildNodes;

            AjaxSqlExpression subexpression = null;

            foreach (XmlNode node in nodes)
            {
                subexpression = new AjaxSqlExpression();

                subexpression.Parent = this;

                subexpression.LoadXml((XmlElement)node);

                Add(subexpression);
            }
        }
        #endregion

        #region ����:Add(AjaxSqlExpression item)
        /// <summary></summary>
        /// <param name="item"></param>
        public void Add(AjaxSqlExpression item)
        {
            list.Add(item);
        }
        #endregion

        #region ����:Remove(AjaxSqlExpression item)
        /// <summary></summary>
        /// <param name="item"></param>
        public void Remove(AjaxSqlExpression item)
        {
            list.Remove(item);
        }
        #endregion

        #region ����:ToString()
        /// <summary>תΪ�ַ���</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();

            if (!string.IsNullOrEmpty(Key))
            {
                switch (this.Type.ToLower())
                {
                    case "string":
                        outString.AppendFormat(" {0} {1} ##{2}## ", Key, Operation, Value);
                        break;

                    case "datetime":
                        outString.AppendFormat(" {0} {1} ##{2}## ", Key, Operation, Convert.ToDateTime(Value));
                        break;

                    case "guid":
                        outString.AppendFormat(" {0} {1} ##{2}## ", Key, Operation, new Guid(Value));
                        break;

                    case "int":
                        outString.AppendFormat(" {0} {1} {2} ", Key, Operation, Value);
                        break;

                    default:
                        outString.AppendFormat(" {0} {1} {2} ", Key, Operation, Value);
                        break;
                }
            }

            foreach (AjaxSqlExpression item in list)
            {
                outString.Append(item.ToString());
            }

            outString = outString.Replace("  ", " ");

            if (this.Parent != null && (this.Prefix == "AND" || this.Prefix == "OR"))
            {
                return string.Format(" {0} ({1}) ", Prefix, outString.ToString().Trim());
            }
            else
            {
                return string.Format(" ({0}) ", outString.ToString().Trim());
            }
        }
        #endregion
    }
}
