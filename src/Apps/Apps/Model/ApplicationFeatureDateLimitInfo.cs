namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用功能时间限制</summary>
    public class ApplicationFeatureDateLimitInfo
    {
        #region 构造函数:ApplicationFeatureDateLimitInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationFeatureDateLimitInfo()
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

        #region 属性:BeginDate
        private string m_BeginDate;

        /// <summary></summary>
        public string BeginDate
        {
            get { return this.m_BeginDate; }
            set { this.m_BeginDate = value; }
        }
        #endregion

        #region 属性:EndDate
        private string m_EndDate;

        /// <summary></summary>
        public string EndDate
        {
            get { return this.m_EndDate; }
            set { this.m_EndDate = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return this.m_Status; }
            set { this.m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return this.m_ModifiedDate; }
            set { this.m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return this.m_CreatedDate; }
            set { this.m_CreatedDate = value; }
        }
        #endregion
    }
}
