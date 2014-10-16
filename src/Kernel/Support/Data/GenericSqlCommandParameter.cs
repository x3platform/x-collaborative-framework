namespace X3Platform.Data
{
    #region Using Libraries
    using System;
    using System.Data;
    #endregion

    /// <summary>通用的SQL命令参数</summary>
    public struct GenericSqlCommandParameter
    {
        public string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        public object m_Value;

        /// <summary>值</summary>
        public object Value
        {
            get { return this.m_Value; }
            set { this.m_Value = value; }
        }

        public ParameterDirection m_Direction;

        /// <summary>类型</summary>
        public ParameterDirection Direction
        {
            get { return this.m_Direction; }
            set { this.m_Direction = value; }
        }

        /// <summary>构造函数</summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="direction">类型</param>
        public GenericSqlCommandParameter(string name, object value, ParameterDirection direction)
        {
            this.m_Name = name;
            this.m_Value = value;
            this.m_Direction = direction;
        }

        /// <summary>构造函数</summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public GenericSqlCommandParameter(string name, object value)
            : this(name, value, ParameterDirection.Input)
        {
        }
    }
}


