using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using X3Platform.Util;

namespace X3Platform.Data
{
    /// <summary>Ajax Sql 条件参数</summary>
    [Serializable()]
    public class DataQueryParameter
    {
        #region 属性:Key
        private string m_Key = null;

        /// <summary>键</summary>
        [XmlAttribute("key")]
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        #endregion

        #region 属性:Operation
        private string m_Operation = null;

        /// <summary>操作符</summary>
        [XmlAttribute("operation")]
        public string Operation
        {
            get { return m_Operation; }
            set { m_Operation = value; }
        }
        #endregion

        #region 属性:Value
        private string m_Value = null;

        /// <summary>值</summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type = null;

        /// <summary>值的类型</summary>
        [XmlAttribute("type")]
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 构造函数:DataQueryParameter()
        /// <summary></summary>
        public DataQueryParameter()
        {
        }
        #endregion

        #region 构造函数:DataQueryParameter(string key, string value, string type)
        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public DataQueryParameter(string key, string value, string type)
        {
            m_Key = key;
            m_Value = value;
            m_Type = type;
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

            return outString.ToString();
        }
        #endregion
    }
}
