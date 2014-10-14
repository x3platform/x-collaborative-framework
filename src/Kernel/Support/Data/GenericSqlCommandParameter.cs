#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Data
{
    #region Using Libraries
    using System;
    using System.Data;
    #endregion

    /// <summary>ͨ�õ�SQL��������</summary>
    public struct GenericSqlCommandParameter
    {
        public string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        public object m_Value;

        /// <summary>ֵ</summary>
        public object Value
        {
            get { return this.m_Value; }
            set { this.m_Value = value; }
        }

        public ParameterDirection m_Direction;

        /// <summary>����</summary>
        public ParameterDirection Direction
        {
            get { return this.m_Direction; }
            set { this.m_Direction = value; }
        }

        /// <summary>���캯��</summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="direction">����</param>
        public GenericSqlCommandParameter(string name, object value, ParameterDirection direction)
        {
            this.m_Name = name;
            this.m_Value = value;
            this.m_Direction = direction;
        }

        /// <summary>���캯��</summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        public GenericSqlCommandParameter(string name, object value)
            : this(name, value, ParameterDirection.Input)
        {
        }
    }
}


