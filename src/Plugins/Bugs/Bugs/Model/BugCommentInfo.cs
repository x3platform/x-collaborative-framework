#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.Model
{
    #region Using Libraries
    using System;

    using X3Platform.Membership;
    #endregion

    /// <summary>问题意见/评论</summary>
    public class BugCommentInfo : EntityClass
    {
        public BugCommentInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:Bug
        /// <summary>问题</summary>
        public BugInfo Bug
        {
            get { return BugContext.Instance.BugService[this.BugId]; }
        }
        #endregion

        #region 属性:BugId
        private string m_BugId;

        /// <summary>问题标识</summary>
        public string BugId
        {
            get { return this.m_BugId; }
            set { this.m_BugId = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>报告人</summary>
        public IAccountInfo Account
        {
            get
            {
                if (this.m_Account == null)
                {
                    this.m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return this.m_Account;
            }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary>帐号标识</summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary>帐号名称</summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary>标题</summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Content
        private string m_Content;

        /// <summary>内容</summary>
        public string Content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>修改日期</summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建日期</summary>
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
