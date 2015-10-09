namespace X3Platform.Membership.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;

    /// <summary>联系人类型</summary>
    [Flags]
    public enum ContactType
    {
        /// <summary>默认</summary>
        Default = Account | OrganizationUnit | Role | Group,

        /// <summary>所有</summary>
        All = Account | OrganizationUnit | Role | Group | StandardOrganizationUnit | StandardRole | GeneralRole | Contact,

        /// <summary>帐号</summary>
        Account = 1,

        /// <summary>组织单位</summary>
        OrganizationUnit = 2,

        /// <summary>角色</summary>
        Role = 4,

        /// <summary>群组</summary>
        Group = 8,

        /// <summary>标准组织</summary>
        StandardOrganizationUnit = 16,

        /// <summary>标准角色</summary>
        StandardRole = 32,

        /// <summary>通用角色</summary>
        GeneralRole = 64,

        /// <summary>职位信息</summary>
        Job = 10240,

        /// <summary>岗位信息</summary>
        AssignedJob = 20480,

        /// <summary>我的联系人</summary>
        Contact = 65536,
    }
}