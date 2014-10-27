namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary></summary>
    public class ApplicationErrorInfo
    {
        #region 构造函数:ApplicationErrorInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationErrorInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return this.m_ApplicationId; }
            set { this.m_ApplicationId = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary></summary>
        public string Code
        {
            get { return this.m_Code; }
            set { this.m_Code = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary></summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description;

        /// <summary></summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region 属性:StatusCode
        private int m_StatusCode;

        /// <summary>HTTP StatusCode</summary>
        public int StatusCode
        {
            get { return this.m_StatusCode; }
            set { this.m_StatusCode = value; }
        }
        #endregion

        #region 属性:Lock
        private int m_Lock;

        /// <summary></summary>
        public int Lock
        {
            get { return this.m_Lock; }
            set { this.m_Lock = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary></summary>
        public string Tags
        {
            get { return this.m_Tags; }
            set { this.m_Tags = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
