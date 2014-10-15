#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ApplicationMenuInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary>应用菜单</summary>
    public class ApplicationMenuInfo : EntityClass
    {
        #region 构造函数:ApplicationMenuInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationMenuInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Application
        private ApplicationInfo m_Application;

        /// <summary>应用</summary>
        public ApplicationInfo Application
        {
            get
            {
                if (m_Application == null && !string.IsNullOrEmpty(this.ApplicationId))
                {
                    m_Application = AppsContext.Instance.ApplicationService.FindOne(this.ApplicationId);
                }

                return m_Application;
            }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId = string.Empty;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return m_ApplicationId; }
            set { m_ApplicationId = value; }
        }
        #endregion

        #region 属性:ApplicationName
        /// <summary></summary>
        public string ApplicationName
        {
            get { return this.Application == null ? string.Empty : this.Application.ApplicationName; }
        }
        #endregion

        #region 属性:ApplicationDisplayName
        /// <summary></summary>
        public string ApplicationDisplayName
        {
            get { return this.Application == null ? string.Empty : this.Application.ApplicationDisplayName; }
        }
        #endregion

        #region 属性:Parent
        private ApplicationMenuInfo m_Parent;

        /// <summary>应用</summary>
        public ApplicationMenuInfo Parent
        {
            get
            {
                if (this.ParentId == Guid.Empty.ToString())
                    return null;

                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = AppsContext.Instance.ApplicationMenuService.FindOne(this.ParentId);
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId = Guid.Empty.ToString();

        /// <summary></summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:ParentName
        /// <summary></summary>
        public string ParentName
        {
            get { return this.Parent == null ? this.ApplicationDisplayName : this.Parent.Name; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary></summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary></summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url = string.Empty;

        /// <summary></summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        #endregion

        #region 属性:Target
        private string m_Target = "_self";

        /// <summary></summary>
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        #endregion

        #region 属性:TargetView
        private string m_TargetView = string.Empty;

        /// <summary></summary>
        public string TargetView
        {
            get
            {
                if (string.IsNullOrEmpty(m_TargetView) && !string.IsNullOrEmpty(this.Target))
                {
                    this.m_TargetView = AppsContext.Instance.ApplicationSettingService.GetText(
                       AppsContext.Instance.ApplicationService["ApplicationManagement"].Id,
                       "应用管理_应用链接打开方式",
                       this.Target);
                }

                return m_TargetView;
            }
        }
        #endregion

        #region 属性:MenuType
        private string m_MenuType = "ApplicationMenu";

        /// <summary></summary>
        public string MenuType
        {
            get { return m_MenuType; }
            set { m_MenuType = value; }
        }
        #endregion

        #region 属性:MenuTypeView
        private string m_MenuTypeView = null;

        /// <summary></summary>
        public string MenuTypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_MenuTypeView) && !string.IsNullOrEmpty(this.MenuType))
                {
                    this.m_MenuTypeView = AppsContext.Instance.ApplicationSettingService.GetText(
                       AppsContext.Instance.ApplicationService["ApplicationManagement"].Id,
                       "应用管理_应用菜单类别",
                       m_MenuType);
                }

                return m_MenuTypeView;
            }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath = string.Empty;

        /// <summary></summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:BigIconPath
        private string m_BigIconPath = string.Empty;

        /// <summary></summary>
        public string BigIconPath
        {
            get { return m_BigIconPath; }
            set { m_BigIconPath = value; }
        }
        #endregion

        #region 属性:DisplayType
        private string m_DisplayType = string.Empty;

        /// <summary>显示方式</summary>
        public string DisplayType
        {
            get { return m_DisplayType; }
            set { m_DisplayType = value; }
        }
        #endregion

        #region 属性:DisplayTypeView
        private string m_DisplayTypeView = string.Empty;

        /// <summary></summary>
        public string DisplayTypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_DisplayTypeView) && !string.IsNullOrEmpty(this.DisplayType))
                {
                    this.m_DisplayTypeView = AppsContext.Instance.ApplicationSettingService.GetText(
                       AppsContext.Instance.ApplicationService["ApplicationManagement"].Id,
                       "应用管理_应用菜单展现方式",
                       this.DisplayType);
                }

                return m_DisplayTypeView;
            }
        }
        #endregion

        #region 属性:HasChild
        private int m_HasChild;

        /// <summary></summary>
        public int HasChild
        {
            get { return m_HasChild; }
            set { m_HasChild = value; }
        }
        #endregion

        #region 属性:ContextObject
        private string m_ContextObject = string.Empty;

        /// <summary>上下文对象</summary>
        public string ContextObject
        {
            get { return m_ContextObject; }
            set { m_ContextObject = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

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
        private string m_Remark = string.Empty;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:FullPath
        private string m_FullPath = string.Empty;

        /// <summary></summary>
        public string FullPath
        {
            get { return m_FullPath; }
            set { m_FullPath = value; }
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

        // -------------------------------------------------------
        // 可访问成员信息
        // -------------------------------------------------------

        /// <summary>权限：应用_通用_查看权限</summary>
        private AuthorityInfo authorizationRead = AuthorityContext.Instance.AuthorityService["应用_通用_查看权限"];

        #region 函数:BindAuthorizationReadScope(string scopeText)
        /// <summary>绑定查看权限</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationReadScope(string scopeText)
        {
            // 清空缓存数据
            this.m_AuthorizationReadScopeObjectText = null;
            this.m_AuthorizationReadScopeObjectView = null;

            if (this.m_AuthorizationReadScopeObjects == null)
            {
                this.m_AuthorizationReadScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationReadScopeObjects, scopeText);
        }
        #endregion

        #region 属性:AuthorizationReadScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationReadScopeObjects = null;

        /// <summary>权限：应用_通用_查看权限范围</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationReadScopeObjects
        {
            get
            {
                if (m_AuthorizationReadScopeObjects == null)
                {
                    m_AuthorizationReadScopeObjects = AppsContext.Instance.ApplicationMenuService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationRead.Name);

                    // 设置默认权限是所有人
                    if (m_AuthorizationReadScopeObjects.Count == 0)
                    {
                        IAuthorizationObject authorizationObject = MembershipManagement.Instance.RoleService.GetEveryoneObject();

                        m_AuthorizationReadScopeObjects.Add(new MembershipAuthorizationScopeObject(authorizationObject.Type, authorizationObject.Id, authorizationObject.Name));
                    }
                }

                return m_AuthorizationReadScopeObjects;
            }
        }
        #endregion

        #region 属性:AuthorizationReadScopeObjectText
        private string m_AuthorizationReadScopeObjectText = null;

        /// <summary>权限：应用_通用_查看权限范围文本</summary>
        public string AuthorizationReadScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectText))
                {
                    m_AuthorizationReadScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationReadScopeObjects);
                }

                return m_AuthorizationReadScopeObjectText;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BindAuthorizationReadScope(value);
                }
            }
        }
        #endregion

        #region 属性:AuthorizationReadScopeObjectView
        private string m_AuthorizationReadScopeObjectView = null;

        /// <summary>权限：应用_通用_查看权限范围视图</summary>
        public string AuthorizationReadScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectView))
                {
                    m_AuthorizationReadScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationReadScopeObjects);
                }

                return m_AuthorizationReadScopeObjectView;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 设置 EntityClass 标识
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

        // -------------------------------------------------------
        // 实现 EntityClass 序列化
        // -------------------------------------------------------

        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
        public override string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>序列化对象</summary>
        /// <param name="displayComment">显示注释</param>
        /// <param name="displayFriendlyName">显示友好名称</param>
        /// <returns></returns>
        public override string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();
            if (displayComment)
                outString.Append("<!-- 应用菜单对象 -->");
            outString.Append("<menu>");
            if (displayComment)
                outString.Append("<!-- 应用菜单标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 所属应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<applicationId><![CDATA[{0}]]></applicationId>", this.ApplicationId);
            if (displayComment)
                outString.Append("<!-- 所属父级菜单标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 应用菜单编码 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 应用菜单名称 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 应用菜单描述 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<description><![CDATA[{0}]]></description>", this.Description);
            if (displayComment)
                outString.Append("<!-- 应用菜单地址 (字符串) (nvarchar(800)) -->");
            outString.AppendFormat("<url><![CDATA[{0}]]></url>", this.Url);
            if (displayComment)
                outString.Append("<!-- 应用菜单名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<target><![CDATA[{0}]]></target>", this.Target);
            if (displayComment)
                outString.Append("<!-- 应用菜单类型 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<menuType><![CDATA[{0}]]></menuType>", this.MenuType);
            if (displayComment)
                outString.Append("<!-- 图标路径 (字符串) (nvarchar(400)) -->");
            outString.AppendFormat("<iconPath><![CDATA[{0}]]></iconPath>", this.IconPath);
            if (displayComment)
                outString.Append("<!-- 大图标路径 (字符串) (nvarchar(400)) -->");
            outString.AppendFormat("<bigIconPath><![CDATA[{0}]]></bigIconPath>", this.BigIconPath);
            if (displayComment)
                outString.Append("<!-- 显示的类型 (字符串) (nvarchar(20)) -->");
            outString.AppendFormat("<displayType><![CDATA[{0}]]></displayType>", this.DisplayType);
            if (displayComment)
                outString.Append("<!-- 排序 (字符串) (nvarchar(20)) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 备注 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            if (displayComment)
                outString.Append("<!-- 授权对象列表 -->");
            outString.Append("<authorizationObjects>");
            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.AuthorizationReadScopeObjects)
            {
                outString.AppendFormat("<authorizationObject id=\"{0}\" type=\"{1}\" authority=\"应用_通用_查看权限\" />",
                    authorizationScopeObject.AuthorizationObjectId,
                    authorizationScopeObject.AuthorizationObjectType);
            }
            outString.Append("</authorizationObjects>");
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            outString.Append("</menu>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml元素</param>
        public override void Deserialize(XmlElement element)
        {
            this.Id = element.SelectSingleNode("id").InnerText;
            this.ApplicationId = element.SelectSingleNode("applicationId").InnerText;
            this.ParentId = element.SelectSingleNode("parentId").InnerText;
            this.Code = element.SelectSingleNode("code").InnerText;
            this.Name = element.SelectSingleNode("name").InnerText;
            this.Description = element.SelectSingleNode("description").InnerText;
            this.Url = element.SelectSingleNode("url").InnerText;
            this.Target = element.SelectSingleNode("target").InnerText;
            this.MenuType = element.SelectSingleNode("menuType").InnerText;
            this.IconPath = element.SelectSingleNode("iconPath").InnerText;
            this.BigIconPath = element.SelectSingleNode("bigIconPath").InnerText;
            this.DisplayType = element.SelectSingleNode("displayType").InnerText;
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;

            // 设置可访问成员信息
            XmlNodeList list = element.SelectNodes("authorizationObjects/authorizationObject[@authority='应用_通用_查看权限']");

            this.m_AuthorizationReadScopeObjects = new List<MembershipAuthorizationScopeObject>();

            foreach (XmlNode node in list)
            {
                this.AuthorizationReadScopeObjects.Add(new MembershipAuthorizationScopeObject(node.Attributes["type"].Value, node.Attributes["id"].Value));
            }

            this.UpdateDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
