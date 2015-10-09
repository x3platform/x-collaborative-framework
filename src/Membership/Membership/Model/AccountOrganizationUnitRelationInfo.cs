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

namespace X3Platform.Membership.Model
{
    using System;

    /// <summary>帐户和组织的关联信息</summary>
    [Serializable]
    public class AccountOrganizationUnitRelationInfo : IAccountOrganizationUnitRelationInfo
    {
        /// <summary></summary>
        public AccountOrganizationUnitRelationInfo() { }
        
        /// <summary></summary>
        public AccountOrganizationUnitRelationInfo(string accountId, string organizationId)
        {
            this.AccountId = accountId;
            this.OrganizationUnitId = organizationId;
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

        /// <summary>帐号名称</summary>
        public string AccountGlobalName
        {
            get { return m_AccountGlobalName; }
            set { m_AccountGlobalName = value; }
        }
        #endregion

        #region 属性:OrganizationUnitId
        private string m_OrganizationUnitId = string.Empty;

        /// <summary>组织标识</summary>
        public string OrganizationUnitId
        {
            get { return m_OrganizationUnitId; }
            set { m_OrganizationUnitId = value; }
        }
        #endregion

        #region 属性:OrganizationUnitGlobalName
        private string m_OrganizationUnitGlobalName = string.Empty;

        /// <summary>组织全局名称</summary>
        public string OrganizationUnitGlobalName
        {
            get { return m_OrganizationUnitGlobalName; }
            set { m_OrganizationUnitGlobalName = value; }
        }
        #endregion

        #region 属性:IsDefault
        private int m_IsDefault = 0;

        /// <summary>是否默认组织</summary>
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

        #region 函数:GetAccount()
        /// <summary>获取相关组织信息</summary>
        public IOrganizationUnitInfo GetOrganizationUnit()
        {
            return MembershipManagement.Instance.OrganizationUnitService[this.OrganizationUnitId];
        }
        #endregion
    }
}
