namespace X3Platform.Membership.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>帐户和群组的关联信息</summary>
    [Serializable]
    public class AccountGroupRelationInfo : IAccountGroupRelationInfo
    {
        /// <summary></summary>
        public AccountGroupRelationInfo() { }

        /// <summary></summary>
        public AccountGroupRelationInfo(string accountId, string groupId)
        {
            this.AccountId = accountId;
            this.GroupId = groupId;
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

        #region 属性:GroupId
        private string m_GroupId = string.Empty;

        /// <summary>群组标识</summary>
        public string GroupId
        {
            get { return m_GroupId; }
            set { m_GroupId = value; }
        }
        #endregion

        #region 属性:GroupGlobalName
        private string m_GroupGlobalName = string.Empty;

        /// <summary>群组全局名称</summary>
        public string GroupGlobalName
        {
            get { return m_GroupGlobalName; }
            set { m_GroupGlobalName = value; }
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

        #region 函数:GetGroup()
        /// <summary>获取相关组织信息</summary>
        public IGroupInfo GetGroup()
        {
            return MembershipManagement.Instance.GroupService[this.GroupId];
        }
        #endregion
    }
}
