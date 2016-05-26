#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountLogInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary></summary>
    public class AccountLogInfo
    {
        #region 构造函数:AccountLogInfo()
        /// <summary>默认构造函数</summary>
        public AccountLogInfo()
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

        #region 属性:OriginalObjectValue
        private string m_OriginalObjectValue;

        /// <summary></summary>
        public string OriginalObjectValue
        {
            get { return this.m_OriginalObjectValue; }
            set { this.m_OriginalObjectValue = value; }
        }
        #endregion

        #region 属性:OperatedBy
        private string m_OperatedBy;

        /// <summary></summary>
        public string OperatedBy
        {
            get { return this.m_OperatedBy; }
            set { this.m_OperatedBy = value; }
        }
        #endregion

        #region 属性:OperationName
        private string m_OperationName;

        /// <summary></summary>
        public string OperationName
        {
            get { return this.m_OperationName; }
            set { this.m_OperationName = value; }
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
