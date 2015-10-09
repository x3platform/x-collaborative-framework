namespace X3Platform.Membership
{
    /// <summary>标准通用角色与角色的关联信息</summary>
    public interface IStandardGeneralRoleMappingRelationInfo
    {
        #region 属性:StandardGeneralRoleId
        /// <summary>所属标准通用角色标识</summary>
        string StandardGeneralRoleId { get; set; }
        #endregion

        #region 属性:OrganizationUnitId
        /// <summary>所属组织标识</summary>
        string OrganizationUnitId { get; }
        #endregion

        #region 属性:RoleId
        /// <summary>所属角色标识</summary>
        string RoleId { get; set; }
        #endregion

        #region 属性:StandardRoleId
        /// <summary>所属标准角色标识</summary>
        string StandardRoleId { get; }
        #endregion

        #region 函数:GetStandardGeneralRole()
        /// <summary>获取所属标准角色信息</summary>
        IStandardGeneralRoleInfo GetStandardGeneralRole();
        #endregion

        #region 函数:GetOrganizationUnit()
        /// <summary>获取所属角色信息</summary>
        IOrganizationUnitInfo GetOrganizationUnit();
        #endregion

        #region 函数:GetGroup()
        /// <summary>获取所属角色信息</summary>
        IRoleInfo GetRole();
        #endregion

        #region 函数:GetStandardRole()
        /// <summary>获取所属标准角色信息</summary>
        IStandardRoleInfo GetStandardRole();
        #endregion
    }
}
