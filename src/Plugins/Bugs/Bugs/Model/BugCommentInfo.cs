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

    /// <summary>�������/����</summary>
    public class BugCommentInfo : EntityClass
    {
        public BugCommentInfo() { }

        #region ����:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
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
            get { return this.m_BugId; }
            set { this.m_BugId = value; }
        }
        #endregion

        #region ����:Account
        private IAccountInfo m_Account = null;

        /// <summary>������</summary>
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

        #region ����:AccountId
        private string m_AccountId;

        /// <summary>�ʺű�ʶ</summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region ����:AccountName
        /// <summary>�ʺ�����</summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region ����:Title
        private string m_Title;

        /// <summary>����</summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region ����:Content
        private string m_Content;

        /// <summary>����</summary>
        public string Content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
        }
        #endregion

        #region ����:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>�޸�����</summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region ����:CreateDate
        private DateTime m_CreateDate;

        /// <summary>��������</summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ���� EntityClass ��ʶ
        // -------------------------------------------------------

        #region ����:EntityId
        /// <summary>ʵ������ʶ</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion
    }
}
