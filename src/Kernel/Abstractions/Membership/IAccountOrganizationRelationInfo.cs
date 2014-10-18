namespace X3Platform.Membership
{
    using System;

    /// <summary>帐户和组织的关联信息</summary>
    public interface IAccountOrganizationRelationInfo
    {
        #region 属性:AccountId
        /// <summary>帐号标识</summary>
        string AccountId { get; set; }
        #endregion

        #region 属性:AccountGlobalName
        /// <summary>帐号全局名称</summary>
        string AccountGlobalName { get; set; }
        #endregion

        #region 属性:OrganizationId
        /// <summary>组织标识</summary>
        string OrganizationId { get; set; }
        #endregion

        #region 属性:OrganizationGlobalName
        /// <summary>组织全局名称</summary>
        string OrganizationGlobalName { get; set; }
        #endregion

        #region 属性:IsDefault
        /// <summary>是否默认组织</summary>
        int IsDefault { get; set; }
        #endregion

        #region 属性:BeginDate
        /// <summary>生效时间</summary>
        DateTime BeginDate { get; set; }
        #endregion

        #region 属性:EndDate
        /// <summary>失效时间</summary>
        DateTime EndDate { get; set; }
        #endregion

        #region 函数:GetAccount()
        IAccountInfo GetAccount();
        #endregion

        #region 函数:GetOrganization()
        IOrganizationInfo GetOrganization();
        #endregion
    }
}
