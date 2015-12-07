#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumFollowInfo.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class ForumFollowInfo : ForumInfo
    {
        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>帐号信息</summary>
        public IAccountInfo Account
        {
            get
            {
                if (m_Account == null)
                {
                    m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return m_Account;
            }

            set { m_Account = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>帐户标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:Member
        private ForumMemberInfo m_Member;

        /// <summary>个人论坛信息</summary>
        public ForumMemberInfo Member
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.AccountId) || string.IsNullOrEmpty(this.ApplicationTag)))
                {
                    m_Member = ForumContext.Instance.ForumMemberService.FindOneByAccountId(this.AccountId);
                }
                else
                {
                    m_Member = null;
                }

                return m_Member;
            }
        }
        #endregion

        #region 属性:MemberDisplayName

        public string MemberDisplayName
        {
            get { return this.Member == null ? string.Empty : this.Member.AccountName; }
        }

        #endregion

        #region 属性:MemberIconPath

        public string MemberIconPath
        {
            get { return this.Member == null ? string.Empty : this.Member.IconPath; }
        }
        #endregion

        #region 属性:ThreadId
        private string m_ThreadId = string.Empty;

        /// <summary></summary>
        public string ThreadId
        {
            get { return this.m_ThreadId; }
            set { this.m_ThreadId = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title = string.Empty;

        /// <summary></summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Content
        private string m_Content = string.Empty;

        /// <summary></summary>
        public string Content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
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

        #region 属性:Sign
        private int m_Sign;

        /// <summary></summary>
        public int Sign
        {
            get { return this.m_Sign; }
            set { this.m_Sign = value; }
        }
        #endregion
    }
}
