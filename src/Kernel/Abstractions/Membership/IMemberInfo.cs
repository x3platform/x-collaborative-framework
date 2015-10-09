namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>成员</summary>
    public interface IMemberInfo : ISerializedObject
    {
        /// <summary>帐号</summary>
        IAccountInfo Account { get; set; }

        /// <summary>标识</summary>
        string Id { get; set; }

        /// <summary>姓名</summary>
        string FullName { get; set; }

        /// <summary>默认公司标识</summary>
        string CorporationId { get; set; }

        /// <summary>默认公司信息</summary>
        IOrganizationUnitInfo Corporation { get; }

        /// <summary>默认一级部门标识</summary>
        string DepartmentId { get; set; }

        /// <summary>默认一级部门信息</summary>
        IOrganizationUnitInfo Department { get; }

        /// <summary>默认二级部门标识</summary>
        string Department2Id { get; set; }

        /// <summary>默认二级部门信息</summary>
        IOrganizationUnitInfo Department2 { get; }

        /// <summary>默认三级部门标识</summary>
        string Department3Id { get; set; }

        /// <summary>默认三级部门信息</summary>
        IOrganizationUnitInfo Department3 { get; }

        /// <summary>默认所属组织标识</summary>
        string OrganizationUnitId { get; set; }

        /// <summary>默认所属组织信息</summary>
        IOrganizationUnitInfo OrganizationUnit { get; }

        /// <summary>默认所属组织路径</summary>
        string OrganizationPath { get; }

        /// <summary>默认角色标识</summary>
        string RoleId { get; set; }
        
        /// <summary>默认角色信息</summary>
        IRoleInfo Role { get; }

        /// <summary>岗位头衔</summary>
        string Headship { get; set; }

        /// <summary>性别</summary>
        string Sex { get; set; }

        /// <summary>默认职位标识</summary>
        string JobId { get; set; }

        /// <summary>默认职位信息</summary>
        IJobInfo Job { get; }
        
        /// <summary>默认职级标识</summary>
        string JobGradeId { get; set; }

        /// <summary>默认职级信息</summary>
        IJobGradeInfo JobGrade { get; }

        /// <summary>默认主职岗位标识</summary>
        string AssignedJobId { get; set; }

        /// <summary>默认主职岗位</summary>
        IAssignedJobInfo AssignedJob { get; }

        /// <summary>兼职信息</summary>
        IList<IAssignedJobInfo> PartTimeJobs { get; }

        /// <summary>手机号码</summary>
        string Mobile { get; set; }

        /// <summary>座机号码</summary>
        string Telephone { get; set; }

        /// <summary>邮箱帐号</summary>
        string Email { get; set; }

        /// <summary>QQ帐号</summary>
        string QQ { get; set; }

        /// <summary>MSN帐号</summary>
        string MSN { get; set; }

        /// <summary>Rtx帐号</summary>
        string Rtx { get; set; }

        /// <summary>所属组织架构全路径</summary>
        string FullPath { get; set; }

        #region 属性:ExtensionInformation
        /// <summary>成员扩展信息</summary>
        IExtensionInformation ExtensionInformation { get; }
        #endregion
    }
}
