using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace X3Platform.Data
{
    /// <summary>数据查询表达式</summary>
    [Serializable()]
    public class DataQueryExpression : DataQueryParameter
    {
        #region 属性:Parent
        private DataQueryExpression m_Parent = null;

        /// <summary>父级表达式</summary>
        public DataQueryExpression Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
        #endregion

        #region 属性:SubExpressionList
        private IList<DataQueryExpression> list = new List<DataQueryExpression>();

        /// <summary>子表达式列表</summary>
        public IList<DataQueryExpression> SubExpressionList
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

        #region 构造函数:DataQueryExpression()
        /// <summary></summary>
        public DataQueryExpression()
        {
        }
        #endregion

        #region 构造函数:DataQueryExpression(string key, string value, string type, string prefix)
        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        public DataQueryExpression(string key, string value, string type, string prefix)
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

            DataQueryExpression subexpression = null;

            foreach (XmlNode node in nodes)
            {
                subexpression = new DataQueryExpression();

                subexpression.Parent = this;

                subexpression.LoadXml((XmlElement)node);

                Add(subexpression);
            }
        }
        #endregion

        #region 函数:Add(DataQueryExpression item)
        /// <summary></summary>
        /// <param name="item"></param>
        public void Add(DataQueryExpression item)
        {
            list.Add(item);
        }
        #endregion

        #region 函数:Remove(DataQueryExpression item)
        /// <summary></summary>
        /// <param name="item"></param>
        public void Remove(DataQueryExpression item)
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

            foreach (DataQueryExpression item in list)
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
