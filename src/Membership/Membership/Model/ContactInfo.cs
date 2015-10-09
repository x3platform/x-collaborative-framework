// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ContactInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>联系人信息</summary>
    public class ContactInfo
    {
        #region 构造函数:ContactInfo()
        /// <summary>默认构造函数</summary>
        public ContactInfo() { }
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

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:Token
        private string m_Token;

        /// <summary></summary>
        public string Token
        {
            get { return m_Token; }
            set { m_Token = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary></summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:Telephone
        private string m_Telephone;

        /// <summary></summary>
        public string Telephone
        {
            get { return m_Telephone; }
            set { m_Telephone = value; }
        }
        #endregion

        #region 属性:Address
        private string m_Address;

        /// <summary></summary>
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }
        #endregion

        #region 属性:Email
        private string m_Email;

        /// <summary></summary>
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
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
