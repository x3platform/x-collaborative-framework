#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityImplementationInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class EntityImplementationInfo
    {
        #region 构造函数:EntityImplementationInfo()
        /// <summary>默认构造函数</summary>
        public EntityImplementationInfo() { }
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

        #region 属性:EntityId
        private string m_EntityId;

        /// <summary></summary>
        public string EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary></summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>帐户</summary>
        public IAccountInfo Account
        {
            get
            {
                if (m_Account == null && !string.IsNullOrEmpty(this.AccountId))
                {
                    m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return m_Account;
            }
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

        #region 属性:AccountName
        /// <summary></summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region 属性:CompletionStatus
        private string m_CompletionStatus;

        /// <summary>完成状况</summary>
        public string CompletionStatus
        {
            get { return m_CompletionStatus; }
            set { m_CompletionStatus = value; }
        }
        #endregion

        #region 属性:WorkContent
        private string m_WorkContent;

        /// <summary>工作内容</summary>
        public string WorkContent
        {
            get { return m_WorkContent; }
            set { m_WorkContent = value; }
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

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

    }
}
