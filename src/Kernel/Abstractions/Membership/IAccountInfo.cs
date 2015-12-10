namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>帐号</summary>
    public interface IAccountInfo : IAuthorizationObject, ICacheable, IIdentity
    {
        #region 属性:Id
        /// <summary>标识</summary>
        new string Id { get; set; }
        #endregion

        #region 属性:Code
        /// <summary>编号</summary>
        string Code { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>姓名</summary>
        new string Name { get; set; }
        #endregion

        #region 属性:GlobalName
        /// <summary>全局名称</summary>
        string GlobalName { get; }
        #endregion

        #region 属性:DisplayName
        /// <summary>显示名称</summary>
        string DisplayName { get; set; }
        #endregion

        #region 属性:PinYin
        /// <summary>拼音</summary>
        string PinYin { get; set; }
        #endregion

        #region 属性:LoginName
        /// <summary>登录名</summary>
        string LoginName { get; set; }
        #endregion

        #region 属性:PasswordChangedDate
        /// <summary>密码更新时间</summary>
        DateTime PasswordChangedDate { get; set; }
        #endregion

        #region 属性:IdentityCard
        /// <summary>身份证</summary>
        string IdentityCard { get; set; }
        #endregion

        #region 属性:Type
        /// <summary>帐号类型 0:普通帐号 1:邮箱帐号 2:Rtx帐号 3:CRM帐号 1000:供应商帐号 2000:客户帐号</summary>
        new int Type { get; set; }
        #endregion

        #region 属性:CertifiedMobile
        /// <summary>已验证的手机号码</summary>
        string CertifiedMobile { get; set; }
        #endregion

        #region 属性:CertifiedEmail
        /// <summary>已验证的邮箱</summary>
        string CertifiedEmail { get; set; }
        #endregion

        #region 属性:CertifiedAvatar
        /// <summary>已验证的头像</summary>
        string CertifiedAvatar { get; set; }
        #endregion

        #region 属性:EnableExchangeEmail
        /// <summary>启用企业邮箱</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region 属性:Type
        /// <summary>排序标识</summary>
        string OrderId { get; set; }
        #endregion

        #region 属性:IP
        /// <summary>IP地址</summary>
        string IP { get; set; }
        #endregion

        #region 属性:OrganizationUnitRelations
        /// <summary>组织信息</summary>
        IList<IAccountOrganizationUnitRelationInfo> OrganizationUnitRelations { get; }
        #endregion

        #region 属性:RoleRelations
        /// <summary>角色集合</summary>
        IList<IAccountRoleRelationInfo> RoleRelations { get; }
        #endregion

        #region 属性:GroupRelations
        /// <summary>群组集合</summary>
        IList<IAccountGroupRelationInfo> GroupRelations { get; }
        #endregion

        #region 属性:LoginDate
        /// <summary>登录时间</summary>
        DateTime LoginDate { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>唯一名称</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region 属性:CreatedDate
        /// <summary>创建时间</summary>
        DateTime CreatedDate { get; set; }
        #endregion
    }
}