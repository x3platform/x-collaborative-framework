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

    /// <summary>应用功能</summary>
    public class ApplicationFeatureInfo : EntityClass
    {
        #region 构造函数:ApplicationFeatureInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationFeatureInfo() { }
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
        private string m_ApplicationId;

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
        private ApplicationFeatureInfo m_Parent;

        /// <summary>应用</summary>
        public ApplicationFeatureInfo Parent
        {
            get
            {
                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = AppsContext.Instance.ApplicationFeatureService.FindOne(this.ParentId);
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId;

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

        #region 属性:ParentDisplayName
        /// <summary></summary>
        public string ParentDisplayName
        {
            get { return this.Parent == null ? string.Empty : this.Parent.DisplayName; }
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

        #region 属性:DisplayName
        private string m_DisplayName;

        /// <summary></summary>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DisplayName))
                {
                    this.m_DisplayName = this.Name;
                }

                return m_DisplayName;
            }
            set { m_DisplayName = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type;

        /// <summary></summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url;

        /// <summary>功能地址</summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        #endregion

        #region 属性:Target
        private string m_Target;

        /// <summary>目标</summary>
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath;

        /// <summary>图标文件</summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:BigIconPath
        private string m_BigIconPath;

        /// <summary>大图标文件</summary>
        public string BigIconPath
        {
            get { return m_BigIconPath; }
            set { m_BigIconPath = value; }
        }
        #endregion

        #region 属性:HelpUrl
        private string m_HelpUrl;

        /// <summary>功能帮助文件</summary>
        public string HelpUrl
        {
            get { return m_HelpUrl; }
            set { m_HelpUrl = value; }
        }
        #endregion

        #region 属性:Hidden
        private bool m_Hidden;

        /// <summary>显示为菜单列表时是否隐藏。</summary>
        public bool Hidden
        {
            get { return m_Hidden; }
            set { m_Hidden = value; }
        }
        #endregion

        #region 属性:EffectScope
        private int m_EffectScope;

        /// <summary>作用范围 0:local | 1:deep</summary>
        public int EffectScope
        {
            get { return m_EffectScope; }
            set { m_EffectScope = value; }
        }
        #endregion

        #region 属性:Locking
        private int m_Locking;

        /// <summary>是否锁定 0:允许 1:锁定</summary>
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
        // 设置权限范围
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
                    m_AuthorizationReadScopeObjects = AppsContext.Instance.ApplicationFeatureService.GetAuthorizationScopeObjects(
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
        //
        // 提高运算速度, 预先增加
        // UPDATE tb_Application_Feature SET AuthorizationReadScopeObjectText =
        // dbo.func_GetAuthorizationScopeObjectText(Id,'X3Platform.Apps.Model.ApplicationFeatureInfo, X3Platform.Apps', '00000000-0000-0000-0000-000000000001')

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
                outString.Append("<!-- 应用功能对象 -->");
            outString.Append("<feature>");
            if (displayComment)
                outString.Append("<!-- 应用功能标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 所属应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<applicationId><![CDATA[{0}]]></applicationId>", this.ApplicationId);
            if (displayComment)
                outString.Append("<!-- 所属父级功能标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 应用功能编码 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 应用功能名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 应用功能显示名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<displayName><![CDATA[{0}]]></displayName>", this.DisplayName);
            if (displayComment)
                outString.Append("<!-- 应用功能类型 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<type><![CDATA[{0}]]></type>", this.Type);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 授权对象列表 -->");
            outString.Append("<authorizationObjects>");
            if (this.m_AuthorizationReadScopeObjects != null)
            {
                foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.m_AuthorizationReadScopeObjects)
                {
                    outString.AppendFormat("<authorizationObject id=\"{0}\" type=\"{1}\" />",
                        authorizationScopeObject.AuthorizationObjectId,
                        authorizationScopeObject.AuthorizationObjectType);
                }
            }
            outString.Append("</authorizationObjects>");
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            outString.Append("</feature>");

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
            this.DisplayName = element.SelectSingleNode("displayName").InnerText;
            this.Type = element.SelectSingleNode("type").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
