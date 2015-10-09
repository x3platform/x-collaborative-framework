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

    using X3Platform.Apps;
    using X3Platform.Membership;

    using X3Platform.Plugins.Bugs.Configuration;
    #endregion

    /// <summary>问题的状态修改记录</summary>
    public class BugHistoryInfo : EntityClass
    {
        public BugHistoryInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
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
            get { return m_BugId; }
            set { m_BugId = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>报告人</summary>
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
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary>帐号标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary>帐号名称</summary>
        public string AccountName
        {
            get { return (this.Account == null ? string.Empty : Account.Name); }
        }
        #endregion

        #region 属性:FromStatus
        private int m_FromStatus;

        /// <summary>修改前状态</summary>
        public int FromStatus
        {
            get { return m_FromStatus; }
            set { m_FromStatus = value; }
        }
        #endregion

        #region 属性:FromStatusView
        private string m_FromStatusView = null;

        /// <summary>修改前状态</summary>
        public string FromStatusView
        {
            get
            {
                if (string.IsNullOrEmpty(m_FromStatusView))
                {
                    m_FromStatusView = AppsContext.Instance.ApplicationSettingService.GetText(
                          AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
                          "应用管理_协同平台_问题跟踪_问题状态",
                          this.FromStatus.ToString());
                }

                return m_FromStatusView;
            }
        }
        #endregion

        #region 属性:ToStatus
        private int m_ToStatus;

        /// <summary>修改后状态</summary>
        public int ToStatus
        {
            get { return m_ToStatus; }
            set { m_ToStatus = value; }
        }
        #endregion

        #region 属性:ToStatusView
        private string m_ToStatusView = null;

        /// <summary>修改后状态</summary>
        public string ToStatusView
        {
            get
            {
                if (string.IsNullOrEmpty(m_ToStatusView))
                {
                    m_ToStatusView = AppsContext.Instance.ApplicationSettingService.GetText(
                          AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
                          "应用管理_协同平台_问题跟踪_问题状态",
                          this.ToStatus.ToString());
                }

                return m_ToStatusView;
            }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建日期</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        //
        // 设置 EntityClass 标识
        // 

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion
    }
}
