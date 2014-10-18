namespace X3Platform.Membership
{
    using System;

    /// <summary>帐户和角色的关联信息</summary>
    public interface IAccountRoleRelationInfo
    {
        #region 属性:AccountId
        /// <summary>帐号标识</summary>
        string AccountId { get; set; }
        #endregion

        #region 属性:AccountGlobalName
        /// <summary>帐号全局名称</summary>
        string AccountGlobalName { get; set; }
        #endregion

        #region 属性:RoleId
        /// <summary>角色标识</summary>
        string RoleId { get; set; }
        #endregion

        #region 属性:RoleGlobalName
        /// <summary>角色全局名称</summary>
        string RoleGlobalName { get; set; }
        #endregion

        #region 属性:IsDefault
        /// <summary>是否默认角色</summary>
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

        #region 函数:GetRole()
        IRoleInfo GetRole();
        #endregion
    }
}
