namespace X3Platform.Membership
{
    /// <summary>岗位信息</summary>
    public interface IAssignedJobInfo : IAuthorizationObject
    {
        #region 属性:OrganizationUnitId
        /// <summary>所属组织标识</summary>
        string OrganizationUnitId { get; set; }
        #endregion

        #region 属性:OrganizationUnit
        /// <summary>所属组织信息</summary>
        IOrganizationUnitInfo OrganizationUnit { get; }
        #endregion

        #region 属性:JobId
        /// <summary>所属职位标识</summary>
        string JobId { get; set; }
        #endregion

        #region 属性:Job
        /// <summary>所属职位信息</summary>
        IJobInfo Job { get; }
        #endregion

        #region 属性:JobGradeId
        /// <summary>所属职级标识</summary>
        string JobGradeId { get; set; }
        #endregion

        #region 属性:Job
        /// <summary>所属职级信息</summary>
        IJobGradeInfo JobGrade { get; }
        #endregion
    }
}
