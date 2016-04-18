namespace X3Platform.Web.APIs.Configuration
{
    using System;
    using System.Xml.Serialization;

    /// <summary>API方法类型</summary>
    public class APIMethodType
    {
        #region 属性:Name
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region 属性:ClassName
        private string m_ClassName;

        /// <summary>类名称</summary>
        public string ClassName
        {
            get { return this.m_ClassName; }
            set { this.m_ClassName = value; }
        }
        #endregion
    }
}
