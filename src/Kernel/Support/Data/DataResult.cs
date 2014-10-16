namespace X3Platform.Data
{
    #region Using Libraries
    using System;

    using X3Platform.Util;
    #endregion

    /// <summary>数据结果</summary>
    public class DataResultMap
    {
        /// <summary></summary>
        public DataResultMap() { }

        /// <summary></summary>
        public DataResultMap(string propertyName, string dataColumnName)
        {
            this.PropertyName = propertyName;

            if (dataColumnName.IndexOf("#") ==-1)
            {
                this.DataColumnName = dataColumnName;
            }
            else
            {
                string[] keys = dataColumnName.Split('#');
                
                if (keys.Length == 2 && !string.IsNullOrEmpty(keys[0]) && !string.IsNullOrEmpty(keys[1]))
                {
                    this.DataColumnName = keys[0];
                    this.DataColumnCastType = keys[1];
                }
            }
        }

        private string m_PropertyName;

        /// <summary>对象属性的名称</summary>
        public string PropertyName
        {
            get { return this.m_PropertyName; }
            set { this.m_PropertyName = StringHelper.ToFirstUpper(value); }
        }

        private string m_DataColumnName;

        /// <summary>数据列的名称</summary>
        public string DataColumnName
        {
            get { return this.m_DataColumnName; }
            set { this.m_DataColumnName = StringHelper.ToFirstUpper(value); }
        }

        private string m_DataColumnCastType;

        /// <summary>数据列转换后的类型</summary>
        public string DataColumnCastType
        {
            get { return this.m_DataColumnCastType; }
            set { this.m_DataColumnCastType = value.ToLower(); }
        }
    }
}
