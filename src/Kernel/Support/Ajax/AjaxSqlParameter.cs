// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxSqlParameter.cs
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
using System.Xml.Serialization;
using X3Platform.Util;

namespace X3Platform.Ajax
{
    /// <summary>Ajax Sql ��������</summary>
    [Serializable()]
    public class AjaxSqlParameter
    {
        #region ����:Key
        private string m_Key = null;

        /// <summary>��</summary>
        [XmlAttribute("key")]
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        #endregion

        #region ����:Operation
        private string m_Operation = null;

        /// <summary>������</summary>
        [XmlAttribute("operation")]
        public string Operation
        {
            get { return m_Operation; }
            set { m_Operation = value; }
        }
        #endregion

        #region ����:Value
        private string m_Value = null;

        /// <summary>ֵ</summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        #endregion

        #region ����:Type
        private string m_Type = null;

        /// <summary>ֵ������</summary>
        [XmlAttribute("type")]
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region ���캯��:AjaxSqlParameter()
        /// <summary></summary>
        public AjaxSqlParameter()
        {
        }
        #endregion

        #region ���캯��:AjaxSqlParameter(string key, string value, string type)
        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public AjaxSqlParameter(string key, string value, string type)
        {
            m_Key = key;
            m_Value = value;
            m_Type = type;
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

            return outString.ToString();
        }
        #endregion
    }
}
