// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :GeneralRoleInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace X3Platform.Membership.Model
{
    /// <summary></summary>
    public class GeneralRoleInfo : IAuthorizationObject
    {
        #region 构造函数:GeneralRoleInfo()
        /// <summary>默认构造函数</summary>
        public GeneralRoleInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary>编码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:GlobalName
        private string m_GlobalName;

        /// <summary>全局名称</summary>
        public string GlobalName
        {
            get { return string.IsNullOrEmpty(m_GlobalName) ? this.Name : this.m_GlobalName; }
            set { m_GlobalName = value; }
        }
        #endregion

        #region 属性:PinYin
        private string m_PinYin;

        /// <summary></summary>
        public string PinYin
        {
            get { return m_PinYin; }
            set { m_PinYin = value; }
        }
        #endregion

        #region 属性:GroupTreeNodeId
        private string m_GroupTreeNodeId;

        /// <summary></summary>
        public string GroupTreeNodeId
        {
            get { return m_GroupTreeNodeId; }
            set { m_GroupTreeNodeId = value; }
        }
        #endregion

        #region 属性:GroupTreeNodeName
        /// <summary>标准角色名称</summary>
        public string GroupTreeNodeName
        {
            get { return this.GroupTreeNode == null ? string.Empty : this.GroupTreeNode.Name; }
        }
        #endregion

        #region 属性:GroupTreeNode
        private GroupTreeNodeInfo m_GroupTreeNode = null;

        /// <summary>所属的标准组织</summary>
        public GroupTreeNodeInfo GroupTreeNode
        {
            get
            {
                if (m_GroupTreeNode == null && !string.IsNullOrEmpty(this.GroupTreeNodeId))
                {
                    m_GroupTreeNode = MembershipManagement.Instance.GroupTreeNodeService[this.GroupTreeNodeId];
                }

                return m_GroupTreeNode;
            }
        }
        #endregion

        #region 属性:EnableExchangeEmail
        private int m_EnableExchangeEmail;

        /// <summary>启用企业邮箱</summary>
        public int EnableExchangeEmail
        {
            get { return m_EnableExchangeEmail; }
            set { m_EnableExchangeEmail = value; }
        }
        #endregion

        #region 属性:Lock
        private int m_Lock = 1;

        /// <summary>防止意外删除 0 不锁定 | 1 锁定(默认)</summary>
        public int Lock
        {
            get { return m_Lock; }
            set { m_Lock = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:Roles
        private IList<IRoleInfo> m_Roles = null;

        /// <summary>角色接口集合</summary>
        public IList<IRoleInfo> Roles
        {
            get
            {
                if (m_Roles == null)
                {
                    m_Roles = MembershipManagement.Instance.RoleService.FindAllByGeneralRoleId(this.Id);
                }

                return m_Roles;
            }
        }
        #endregion

        #region 属性:Scopes
        private IList<IAuthorizationScope> m_Scopes = new List<IAuthorizationScope>();

        /// <summary>范围接口集合</summary>
        public IList<IAuthorizationScope> Scopes
        {
            get { return m_Scopes; }
        }
        #endregion

        #region 属性:FullPath
        private string m_FullPath = null;

        /// <summary>所属组织架构全路径</summary>
        public string FullPath
        {
            get { return m_FullPath; }
            set { m_FullPath = value; }
        }
        #endregion

        #region 属性:DistinguishedName
        private string m_DistinguishedName = null;

        /// <summary>唯一名称</summary>
        public string DistinguishedName
        {
            get { return m_DistinguishedName; }
            set { m_DistinguishedName = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        //
        // 显式实现 IAuthorizationObject Type
        // 

        #region 属性:IAuthorizationObject.Type
        /// <summary>类型</summary>
        string IAuthorizationObject.Type
        {
            get { return "GeneralRole"; }
        }
        #endregion

        /// <summary></summary>
        /// <param name="displayComment"></param>
        /// <param name="displayFriendlyName"></param>
        /// <returns></returns>
        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public string Serializable()
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public void Deserialize(XmlElement element)
        {
            throw new NotImplementedException();
        }
    }
}
