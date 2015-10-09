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

    /// <summary>�����״̬�޸ļ�¼</summary>
    public class BugHistoryInfo : EntityClass
    {
        public BugHistoryInfo() { }

        #region ����:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region ����:Bug
        /// <summary>����</summary>
        public BugInfo Bug
        {
            get { return BugContext.Instance.BugService[this.BugId]; }
        }
        #endregion

        #region ����:BugId
        private string m_BugId;

        /// <summary>�����ʶ</summary>
        public string BugId
        {
            get { return m_BugId; }
            set { m_BugId = value; }
        }
        #endregion

        #region ����:Account
        private IAccountInfo m_Account = null;

        /// <summary>������</summary>
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

        #region ����:AccountId
        private string m_AccountId;

        /// <summary>�ʺű�ʶ</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region ����:AccountName
        /// <summary>�ʺ�����</summary>
        public string AccountName
        {
            get { return (this.Account == null ? string.Empty : Account.Name); }
        }
        #endregion

        #region ����:FromStatus
        private int m_FromStatus;

        /// <summary>�޸�ǰ״̬</summary>
        public int FromStatus
        {
            get { return m_FromStatus; }
            set { m_FromStatus = value; }
        }
        #endregion

        #region ����:FromStatusView
        private string m_FromStatusView = null;

        /// <summary>�޸�ǰ״̬</summary>
        public string FromStatusView
        {
            get
            {
                if (string.IsNullOrEmpty(m_FromStatusView))
                {
                    m_FromStatusView = AppsContext.Instance.ApplicationSettingService.GetText(
                          AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
                          "Ӧ�ù���_Эͬƽ̨_�������_����״̬",
                          this.FromStatus.ToString());
                }

                return m_FromStatusView;
            }
        }
        #endregion

        #region ����:ToStatus
        private int m_ToStatus;

        /// <summary>�޸ĺ�״̬</summary>
        public int ToStatus
        {
            get { return m_ToStatus; }
            set { m_ToStatus = value; }
        }
        #endregion

        #region ����:ToStatusView
        private string m_ToStatusView = null;

        /// <summary>�޸ĺ�״̬</summary>
        public string ToStatusView
        {
            get
            {
                if (string.IsNullOrEmpty(m_ToStatusView))
                {
                    m_ToStatusView = AppsContext.Instance.ApplicationSettingService.GetText(
                          AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
                          "Ӧ�ù���_Эͬƽ̨_�������_����״̬",
                          this.ToStatus.ToString());
                }

                return m_ToStatusView;
            }
        }
        #endregion

        #region ����:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>��������</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        //
        // ���� EntityClass ��ʶ
        // 

        #region ����:EntityId
        /// <summary>ʵ������ʶ</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion
    }
}
