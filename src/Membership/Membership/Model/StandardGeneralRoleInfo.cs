#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardGeneralRoleInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    #endregion

    /// <summary></summary>
    public class StandardGeneralRoleInfo : IStandardGeneralRoleInfo
    {
        #region 构造函数:StandardGeneralRoleInfo()
        /// <summary>默认构造函数</summary>
        public StandardGeneralRoleInfo() { }
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

        /// <summary></summary>
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

        #region 属性:Suffix
        private string m_Suffix = string.Empty;

        /// <summary>新建角色时的后缀信息</summary>
        public string Suffix
        {
            get { return this.m_Suffix; }
            set { this.m_Suffix = value; }
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

        #region 属性:Locking
        private int m_Locking;

        /// <summary></summary>
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

        // -------------------------------------------------------
        // 显式实现 IAuthorizationObject Type
        // -------------------------------------------------------

        #region 属性:IAuthorizationObject.Type
        /// <summary>类型</summary>
        string IAuthorizationObject.Type
        {
            get { return "StandardGeneralRole"; }
        }
        #endregion

        // -------------------------------------------------------
        // 实现 ISerializedObject 序列化
        // -------------------------------------------------------

        #region 函数:Serializable()
        /// <summary>根据对象导出Xml元素</summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>根据对象导出Xml元素</summary>
        /// <param name="displayComment">显示注释</param>
        /// <param name="displayFriendlyName">显示友好名称</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<!-- 标准通用角色对象 -->");
            outString.Append("<standardGeneralRole>");
            outString.Append("<!-- 标准组织标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            outString.Append("<!-- 排序编号(字符串) nvarchar(20) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);

            if (displayComment)
                outString.Append("<!-- 关联对象-->");
            outString.Append("<relationObjects>");
            //if (this.m_AuthorizationReadScopeObjects != null)
            //{
            //    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.m_AuthorizationReadScopeObjects)
            //    {
            //        outString.AppendFormat("<authorizationObject organizationId=\"{0}\" standardRoleId=\"{1}\" roleId=\"{2}\" />",
            //            authorizationScopeObject.AuthorizationObjectId,
            //            authorizationScopeObject.AuthorizationObjectType);
            //    }
            //}
            outString.Append("</relationObjects>");
            outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</standardGeneralRole>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.GetElementsByTagName("id")[0].InnerText;
            this.Name = element.GetElementsByTagName("name")[0].InnerText;
            // this.NickName = element.GetElementsByTagName("name")[0].InnerText;
            // <fullName>组织架构</fullName>
            // this.Type = Convert.ToInt32(element.GetElementsByTagName("type")[0].InnerText);
            // this.ParentId = element.GetElementsByTagName("parentId")[0].InnerText;
            this.OrderId = element.GetElementsByTagName("orderId")[0].InnerText;
            this.Status = Convert.ToInt32(element.GetElementsByTagName("status")[0].InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.GetElementsByTagName("updateDate")[0].InnerText);
        }
        #endregion
    }
}
