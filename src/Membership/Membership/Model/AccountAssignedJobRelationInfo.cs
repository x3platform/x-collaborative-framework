namespace X3Platform.Membership.Model
{
    using System;

    /// <summary>帐户和岗位的关联信息</summary>
    [Serializable]
    public class AccountAssignedJobRelationInfo : IAccountAssignedJobRelationInfo
    {
        /// <summary></summary>
        public AccountAssignedJobRelationInfo() { }

        /// <summary></summary>
        public AccountAssignedJobRelationInfo(string accountId, string roleId)
        {
            this.AccountId = accountId;
            this.AssignedJobId = roleId;
        }

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>帐号标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountGlobalName
        private string m_AccountGlobalName = string.Empty;

        /// <summary>帐号全局名称</summary>
        public string AccountGlobalName
        {
            get { return m_AccountGlobalName; }
            set { m_AccountGlobalName = value; }
        }
        #endregion

        #region 属性:AssignedJobId
        private string m_AssignedJobId = string.Empty;

        /// <summary>岗位标识</summary>
        public string AssignedJobId
        {
            get { return m_AssignedJobId; }
            set { m_AssignedJobId = value; }
        }
        #endregion

        #region 属性:AssignedJobGlobalName
        private string m_AssignedJobGlobalName = string.Empty;

        /// <summary>岗位全局名称</summary>
        public string AssignedJobGlobalName
        {
            get { return m_AssignedJobGlobalName; }
            set { m_AssignedJobGlobalName = value; }
        }
        #endregion

        #region 属性:IsDefault
        private int m_IsDefault = 0;

        /// <summary>是否默认岗位</summary>
        public int IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
        }
        #endregion

        #region 属性:BeginDate
        private DateTime m_BeginDate = new DateTime(2000, 1, 1);

        /// <summary>生效时间</summary>
        public DateTime BeginDate
        {
            get { return m_BeginDate; }
            set { m_BeginDate = value; }
        }
        #endregion

        #region 属性:EndDate
        private DateTime m_EndDate = DateTime.MaxValue;

        /// <summary>失效时间</summary>
        public DateTime EndDate
        {
            get { return m_EndDate; }
            set { m_EndDate = value; }
        }
        #endregion

        #region 函数:GetAccount()
        /// <summary>获取相关帐号信息</summary>
        public IAccountInfo GetAccount()
        {
            return MembershipManagement.Instance.AccountService[this.AccountId];
        }
        #endregion

        #region 函数:GetAssignedJob()
        /// <summary>获取相关岗位信息</summary>
        public IAssignedJobInfo GetAssignedJob()
        {
            return MembershipManagement.Instance.AssignedJobService[this.AssignedJobId];
        }
        #endregion
    }
}
