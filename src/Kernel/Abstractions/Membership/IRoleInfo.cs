namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Security.Authority;

    /// <summary>角色</summary>
    public interface IRoleInfo : IAuthorizationObject
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
        /// <summary>名称</summary>
        new string Name { get; set; }
        #endregion

        #region 属性:GlobalName
        /// <summary>全局名称</summary>
        string GlobalName { get; }
        #endregion

        #region 属性:PinYin
        /// <summary>拼音</summary>
        string PinYin { get; set; }
        #endregion

        #region 属性:Parent
        /// <summary>父级信息</summary>
        IRoleInfo Parent { get; }
        #endregion

        #region 属性:OrganizationUnitId
        /// <summary>所属组织标识</summary>
        string OrganizationUnitId { get; set; }
        #endregion

        #region 属性:OrganizationUnit
        /// <summary>所属组织信息</summary>
        IOrganizationUnitInfo OrganizationUnit { get; }
        #endregion

        #region 属性:StandardRoleId
        /// <summary>所属标准角色标识</summary>
        string StandardRoleId { get; set; }
        #endregion

        #region 属性:StandardRole
        /// <summary>所属标准角色信息</summary>
        IStandardRoleInfo StandardRole { get; }
        #endregion

        #region 属性:EnableExchangeEmail
        /// <summary>启用企业邮箱</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region 属性:FullPath
        /// <summary>所属组织架构全路径</summary>
        string FullPath { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>唯一名称</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region 属性:Members
        /// <summary>成员列表</summary>
        IList<IAccountInfo> Members { get; }
        #endregion

        #region 属性:ExtensionInformation
        /// <summary>角色的扩展信息</summary>
        IExtensionInformation ExtensionInformation { get; }
        #endregion

        // -------------------------------------------------------
        // 重置成员关系
        // -------------------------------------------------------

        #region 函数:ResetMemberRelations(string relationText)
        /// <summary>重置成员关系</summary>
        /// <param name="relationText"></param>
        void ResetMemberRelations(string relationText);
        #endregion
    }
}
