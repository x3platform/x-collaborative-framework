namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Membership;
    using X3Platform.Entities;
    #endregion

    /// <summary>实体类点击数信息</summary>
    public class EntityClickInfo : IEntityClickInfo
    {
        #region 构造函数:EntityClickInfo()
        /// <summary>默认构造函数</summary>
        public EntityClickInfo()
        {
        }
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

        #region 属性:Click
        private int m_Click;

        /// <summary>点击率</summary>
        public int Click
        {
            get { return m_Click; }
            set { m_Click = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>更新时间</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建时间</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
