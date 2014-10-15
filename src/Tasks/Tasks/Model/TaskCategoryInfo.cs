#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;

    using X3Platform.Membership;
    #endregion

    /// <summary>����������Ϣ</summary>
    [Serializable]
    public class TaskCategoryInfo
    {
        #region ���캯��:TaskCategoryInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public TaskCategoryInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>
        /// ��ʶ
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>
        /// ������Id
        /// </summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>
        /// ����������
        /// </summary>
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(m_AccountName) && !string.IsNullOrEmpty(this.AccountId))
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(this.AccountId);

                    if (account != null)
                    {
                        m_AccountName = account.Name;
                    }
                }

                return m_AccountName;
            }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex = string.Empty;

        /// <summary>��������</summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex; }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>
        /// ����˵��
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags = string.Empty;

        /// <summary>��ǩ</summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

        /// <summary>��������</summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status = 1;

        /// <summary>
        /// ״̬ 1:���� | 0:����
        /// </summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>
        /// �����޸�ʱ��
        /// </summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
