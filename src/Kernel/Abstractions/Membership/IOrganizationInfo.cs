#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IOrganizationInfo.cs
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
    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CacheBuffer;

    /// <summary>组织单位</summary>
    public interface IOrganizationInfo : IAuthorizationObject, ICacheable
    {
        /// <summary>标识</summary>
        new string Id { get; set; }

        /// <summary>编号</summary>
        string Code { get; set; }

        /// <summary>名称</summary>
        new string Name { get; set; }

        #region 属性:GlobalName
        /// <summary>全局名称</summary>
        string GlobalName { get; }
        #endregion

        #region 属性:FullName
        /// <summary>全称</summary>
        string FullName { get; }
        #endregion

        /// <summary>拼音</summary>
        string PinYin { get; set; }

        /// <summary>所属父级组织标识</summary>
        string ParentId { get; set; }

        #region 属性:ParentGlobalName
        /// <summary>父节点全局名称</summary>
        string ParentGlobalName { get; }
        #endregion

        /// <summary>父级组织信息</summary>
        IOrganizationInfo Parent { get; }

        #region 属性:ChindNodes
        /// <summary>子节点</summary>
        IList<IAuthorizationObject> ChindNodes { get; }
        #endregion

        /// <summary>角色集合</summary>
        IList<IRoleInfo> Roles { get; }

        /// <summary>所属标准组织标识</summary>
        string StandardOrganizationId { get; set; }

        #region 属性:Type
        /// <summary>类型</summary>
        new int Type { get; set; }
        #endregion

        #region 属性:EnableExchangeEmail
        /// <summary>启用企业邮箱</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region 属性:OrderId
        /// <summary>排序标识</summary>
        string OrderId { get; set; }
        #endregion

        #region 属性:FullPath
        /// <summary>所属组织架构全路径</summary>
        string FullPath { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>唯一名称</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region 属性:ExtensionInformation
        /// <summary>组织单位扩展信息</summary>
        IExtensionInformation ExtensionInformation { get; }
        #endregion
    }
}
