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

        #region 属性:OptionAccountId
        private string m_OptionAccountId;

        /// <summary></summary>
        public string OptionAccountId
        {
            get { return this.m_OptionAccountId; }
            set { this.m_OptionAccountId = value; }
        }
        #endregion

        #region 属性:OptionName
        private string m_OptionName;

        /// <summary></summary>
        public string OptionName
        {
            get { return this.m_OptionName; }
            set { this.m_OptionName = value; }
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

        #region 属性:Date
        private DateTime m_Date;

        /// <summary></summary>
        public DateTime Date
        {
            get { return this.m_Date; }
            set { this.m_Date = value; }
        }
        #endregion
    }
}
