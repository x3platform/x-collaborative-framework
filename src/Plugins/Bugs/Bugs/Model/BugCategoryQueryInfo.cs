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
    #endregion

    /// <summary>��������ѯ��Ϣ</summary>
    [Serializable]
    public class BugCategoryQueryInfo
    {
        #region ���캯��:NewsCategoryQueryInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public BugCategoryQueryInfo()
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

        #region ����:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>�����˱�ʶ</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region ����:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>����������</summary>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        #endregion

        #region ����:CategoryIndex
        private string m_CategoryIndex = string.Empty;

        /// <summary>�������</summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex; }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region ����:Description
        private string m_Description = string.Empty;

        /// <summary>����˵��</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region ����:OrderId
        private string m_OrderId = string.Empty;

        /// <summary>�����</summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region ����:Status
        private int m_Status;

        /// <summary>״̬: 1 ��Ч | 0��Ч</summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region ����:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>����޸�ʱ��</summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region ����:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>����ʱ��</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion
    }
}
