#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IStandardOrganizationInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership
{
    /// <summary>标准组织信息</summary>
    public interface IStandardOrganizationInfo : IAuthorizationObject
    {
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

        #region 属性:ParentId
        /// <summary>所属父级标准组织标识</summary>
        string ParentId { get; set; }
        #endregion

        #region 属性:ParentGlobalName
        /// <summary>父节点全局名称</summary>
        string ParentGlobalName { get; }
        #endregion
    }
}
