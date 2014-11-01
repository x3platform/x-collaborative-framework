namespace X3Platform.Web.APIs.Configuration
{
    using System;
    using System.Xml.Serialization;

    /// <summary>API方法</summary>
    public class APIMethod
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

        #region 属性:Description
        private string m_Description;

        /// <summary>描述</summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
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

        #region 属性:MethodName
        private string m_MethodName;

        /// <summary>方法名称</summary>
        public string MethodName
        {
            get { return this.m_MethodName; }
            set { this.m_MethodName = value; }
        }
        #endregion
    }
}
