﻿using System;
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

        #region 属性:CatalogItemId
        private string m_CatalogItemId;

        /// <summary></summary>
        public string CatalogItemId
        {
            get { return m_CatalogItemId; }
            set { m_CatalogItemId = value; }
        }
        #endregion

        #region 属性:CatalogItemName
        /// <summary>标准角色名称</summary>
        public string CatalogItemName
        {
            get { return this.CatalogItem == null ? string.Empty : this.CatalogItem.Name; }
        }
        #endregion

        #region 属性:CatalogItem
        private CatalogItemInfo m_CatalogItem = null;

        /// <summary>所属的标准组织</summary>
        public CatalogItemInfo CatalogItem
        {
            get
            {
                if (m_CatalogItem == null && !string.IsNullOrEmpty(this.CatalogItemId))
                {
                    m_CatalogItem = MembershipManagement.Instance.CatalogItemService[this.CatalogItemId];
                }

                return m_CatalogItem;
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

        #region 属性:Locking
        private int m_Locking = 1;

        /// <summary>防止意外删除 0 不锁定 | 1 锁定(默认)</summary>
        public int Locking
        {
            get { return m_Locking; }
            set { m_Locking = value; }
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

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
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
