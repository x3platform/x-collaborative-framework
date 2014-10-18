namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Security.Authority;
    #endregion

    /// <summary>标准角色信息</summary>
    public interface IStandardRoleInfo : IAuthorizationObject
    {
        /// <summary>编号</summary>
        string Code { get; set; }

        /// <summary>类型 0:集团总部 1:地区地产公司 2:地区地产项目团队 11:地区商业公司 12:地区地产项目团队 21:地区物业公司 22:地区物业项目团队 65535:其他</summary>
        int Type { get; set; }

        /// <summary>优先级</summary>
        int Priority { get; set; }

        /// <summary>父节点标识</summary>
        string ParentId { get; set; }

        /// <summary>标准组织标识</summary>
        string StandardOrganizationId { get; set; }

        /// <summary>是否是关键角色</summary>
        bool IsKey { get; set; }

        /// <summary>排序标识</summary>
        string OrderId { get; set; }
    }
}
