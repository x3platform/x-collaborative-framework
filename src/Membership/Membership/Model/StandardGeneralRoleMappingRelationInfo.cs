﻿#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardGeneralRoleMappingRelationInfo
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>帐户和角色的关联信息</summary>
    [Serializable]
    public class StandardGeneralRoleMappingRelationInfo : IStandardGeneralRoleMappingRelationInfo
    {
        /// <summary></summary>
        public StandardGeneralRoleMappingRelationInfo() { }

        /// <summary></summary>
        public StandardGeneralRoleMappingRelationInfo(string standardGeneralRoleId, string organizationIdId)
        {
            this.StandardGeneralRoleId = standardGeneralRoleId;
            this.OrganizationId = organizationIdId;
        }

        #region 属性:StandardGeneralRoleId
        private string m_StandardGeneralRoleId = string.Empty;

        /// <summary>标准通用角色标识</summary>
        public string StandardGeneralRoleId
        {
            get { return this. m_StandardGeneralRoleId; }
            set { this.m_StandardGeneralRoleId = value; }
        }
        #endregion

        #region 属性:StandardGeneralRoleName
        private string m_StandardGeneralRoleName = string.Empty;

        /// <summary>标准通用角色名称</summary>
        public string StandardGeneralRoleName
        {
            get { return this.m_StandardGeneralRoleName; }
            set { this.m_StandardGeneralRoleName = value; }
        }
        #endregion

        #region 属性:OrganizationId
        private string m_OrganizationId = string.Empty;

        /// <summary>组织标识</summary>
        public string OrganizationId
        {
            get { return this.m_OrganizationId; }
            set { this.m_OrganizationId = value; }
        }
        #endregion

        #region 属性:OrganizationName
        private string m_OrganizationName = string.Empty;

        /// <summary>组织标识</summary>
        public string OrganizationName
        {
            get { return this.m_OrganizationName; }
            set { this.m_OrganizationName = value; }
        }
        #endregion

        #region 属性:RoleId
        private string m_RoleId = string.Empty;

        /// <summary>角色标识</summary>
        public string RoleId
        {
            get { return this.m_RoleId; }
            set { this.m_RoleId = value; }
        }
        #endregion

        #region 属性:RoleName
        private string m_RoleName = string.Empty;

        /// <summary>角色名称</summary>
        public string RoleName
        {
            get { return this.m_RoleName; }
            set { this.m_RoleName = value; }
        }
        #endregion

        #region 属性:StandardRoleId
        private string m_StandardRoleId = string.Empty;

        /// <summary></summary>
        public string StandardRoleId
        {
            get { return this.m_StandardRoleId; }
            set { this.m_StandardRoleId = value; }
        }
        #endregion

        #region 属性:StandardRoleName
        private string m_StandardRoleName = string.Empty;

        /// <summary></summary>
        public string StandardRoleName
        {
            get { return this.m_StandardRoleName; }
            set { this.m_StandardRoleName = value; }
        }
        #endregion

        /// <summary></summary>
        public IStandardGeneralRoleInfo GetStandardGeneralRole()
        {
            if (!string.IsNullOrEmpty(this.StandardGeneralRoleId))
            {
                return MembershipManagement.Instance.StandardGeneralRoleService.FindOne(this.StandardGeneralRoleId);
            }

            return null;
        }

        /// <summary></summary>
        public IOrganizationInfo GetOrganization()
        {
            if (!string.IsNullOrEmpty(this.OrganizationId))
            {
                return MembershipManagement.Instance.OrganizationService.FindOne(this.OrganizationId);
            }

            return null;
        }

        /// <summary></summary>
        public IRoleInfo GetRole()
        {
            if (!string.IsNullOrEmpty(this.RoleId))
            {
                return MembershipManagement.Instance.RoleService.FindOne(this.RoleId);
            }

            return null;
        }

        /// <summary></summary>
        public IStandardRoleInfo GetStandardRole()
        {
            if (!string.IsNullOrEmpty(this.StandardRoleId))
            {
                return MembershipManagement.Instance.StandardRoleService.FindOne(this.StandardRoleId);
            }

            return null;
        }
    }
}
