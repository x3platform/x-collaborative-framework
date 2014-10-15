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

    /// <summary>帐户和角色的关联信息</summary>
    [Serializable]
    public class AccountRoleRelationInfo : IAccountRoleRelationInfo
    {
        /// <summary></summary>
        public AccountRoleRelationInfo() { }

        /// <summary></summary>
        public AccountRoleRelationInfo(string accountId, string roleId)
        {
            this.AccountId = accountId;
            this.RoleId = roleId;
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

        #region 属性:RoleId
        private string m_RoleId = string.Empty;

        /// <summary>角色标识</summary>
        public string RoleId
        {
            get { return m_RoleId; }
            set { m_RoleId = value; }
        }
        #endregion

        #region 属性:RoleGlobalName
        private string m_RoleGlobalName = string.Empty;

        /// <summary>角色全局名称</summary>
        public string RoleGlobalName
        {
            get { return m_RoleGlobalName; }
            set { m_RoleGlobalName = value; }
        }
        #endregion

        #region 属性:IsDefault
        private int m_IsDefault = 0;

        /// <summary>是否默认角色</summary>
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

        #region 函数:GetRole()
        /// <summary>获取相关角色信息</summary>
        public IRoleInfo GetRole()
        {
            return MembershipManagement.Instance.RoleService[this.RoleId];
        }
        #endregion
    }
}
