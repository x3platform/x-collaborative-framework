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
    /// <summary>Ajax Sql 表达式</summary>
    [Serializable()]
    public class AjaxSqlExpression : AjaxSqlParameter
    {
        #region 属性:Parent
        private AjaxSqlExpression m_Parent = null;

        /// <summary>父级表达式</summary>
        public AjaxSqlExpression Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
        #endregion

        #region 属性:SubExpressionList
        private IList<AjaxSqlExpression> list = new List<AjaxSqlExpression>();

        /// <summary>子表达式列表</summary>
        public IList<AjaxSqlExpression> SubExpressionList
        {
            get { return list; }
            set { list = value; }
        }
        #endregion

        #region 属性:Prefix
        private string m_Prefix = null;

        /// <summary>值的类型</summary>
        [XmlAttribute("prefix")]
        public string Prefix
        {
            get { return m_Prefix; }
            set { m_Prefix = value; }
        }
        #endregion

        #region 构造函数:AjaxSqlExpression()
        /// <summary></summary>
        public AjaxSqlExpression()
        {
        }
        #endregion

        #region 构造函数:AjaxSqlExpression(string key, string value, string type, string prefix)
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

        #region 函数:LoadXml(XmlElement element)
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

        #region 函数:Add(AjaxSqlExpression item)
        /// <summary></summary>
        /// <param name="item"></param>
        public void Add(AjaxSqlExpression item)
        {
            list.Add(item);
        }
        #endregion

        #region 函数:Remove(AjaxSqlExpression item)
        /// <summary></summary>
        /// <param name="item"></param>
        public void Remove(AjaxSqlExpression item)
        {
            list.Remove(item);
        }
        #endregion

        #region 函数:ToString()
        /// <summary>转为字符串</summary>
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
