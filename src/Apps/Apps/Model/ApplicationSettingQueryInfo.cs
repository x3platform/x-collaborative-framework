namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;

    using X3Platform.Security.Authority;
    #endregion

    /// <summary></summary>
    public class ApplicationSettingQueryInfo
    {
        #region 构造函数:ApplicationSettingQueryInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationSettingQueryInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return m_ApplicationId; }
            set { m_ApplicationId = value; }
        }
        #endregion

        #region 属性:ApplicationSettingGroupId
        private string m_ApplicationSettingGroupId;

        /// <summary></summary>
        public string ApplicationSettingGroupId
        {
            get { return m_ApplicationSettingGroupId; }
            set { m_ApplicationSettingGroupId = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary>代码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Text
        private string m_Text;

        /// <summary>文本</summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }
        #endregion

        #region 属性:Value
        private string m_Value;

        /// <summary>值</summary>
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion
    }
}
