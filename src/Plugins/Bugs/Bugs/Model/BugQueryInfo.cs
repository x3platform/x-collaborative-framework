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
    using System.Collections.Generic;

    using X3Platform.Apps;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary>�����ѯ��Ϣ</summary>
    [Serializable]
    public class BugQueryInfo
    {
        #region ���캯��:NewsCategoryQueryInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public BugQueryInfo()
        {
        }
        #endregion

        #region ����:Id
        private string m_Id = string.Empty;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region ����:Code
        private string m_Code = string.Empty;

        /// <summary>���</summary>
        public string Code
        {
            get { return this.m_Code; }
            set { this.m_Code = value; }
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
        private string m_AccountName;

        /// <summary>����������</summary>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        #endregion

        #region ����:CategoryIndex
        private string m_CategoryIndex;

        /// <summary></summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex.Replace(@"\", "-"); }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region ����:Title
        private string m_Title;

        /// <summary>����</summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        #endregion

        #region ����:Tags
        private string m_Tags = string.Empty;

        /// <summary>��ǩ</summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region ����:AssignToAccountId
        private string m_AssignToAccountId;

        /// <summary>ָ���ʺű�ʶ</summary>
        public string AssignToAccountId
        {
            get { return m_AssignToAccountId; }
            set { m_AssignToAccountId = value; }
        }
        #endregion

        #region ����:AssignToAccountName

        private string m_AssignToAccountName;
        /// <summary>ָ���ʺ�����</summary>
        public string AssignToAccountName
        {
            get { return m_AssignToAccountName; }
            set { m_AssignToAccountName = value; }
        }
        #endregion

        #region ����:Priority
        private int m_Priority;

        /// <summary>Ȩ��ֵ</summary>
        public int Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion

        #region ����:Status
        private int m_Status;

        /// <summary>״̬: 0.������ | 1.ȷ���� | 2.������ | 3.�ѽ�� | 4.�ѹر�</summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region ����:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>�޸�����</summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
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
    }
}
