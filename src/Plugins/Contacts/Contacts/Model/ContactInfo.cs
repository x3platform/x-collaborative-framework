#region Copyright & Author
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
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Contacts.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary></summary>
    public class ContactInfo : EntityClass
    {
        #region 构造函数:ContactInfo()
        /// <summary>默认构造函数</summary>
        public ContactInfo()
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

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region 属性:Mobile
        private string m_Mobile;

        /// <summary>手机</summary>
        public string Mobile
        {
            get { return this.m_Mobile; }
            set { this.m_Mobile = value; }
        }
        #endregion

        #region 属性:Telephone
        private string m_Telephone;

        /// <summary>座机</summary>
        public string Telephone
        {
            get { return this.m_Telephone; }
            set { this.m_Telephone = value; }
        }
        #endregion

        #region 属性:Email
        private string m_Email;

        /// <summary>邮件</summary>
        public string Email
        {
            get { return this.m_Email; }
            set { this.m_Email = value; }
        }
        #endregion

        #region 属性:QQ
        private string m_QQ;

        /// <summary>QQ</summary>
        public string QQ
        {
            get { return this.m_QQ; }
            set { this.m_QQ = value; }
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

        // -------------------------------------------------------
        // 设置 EntityClass 标识
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion
    }
}
