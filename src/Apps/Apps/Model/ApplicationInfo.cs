namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.CacheBuffer;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security;

    using X3Platform.Apps.Configuration;
    #endregion

    /// <summary>应用</summary>
    public class ApplicationInfo : EntityClass, ICacheable
    {
        #region 构造函数:ApplicationInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account;

        /// <summary>帐号</summary>
        public IAccountInfo Account
        {
            get
            {
                if (m_Account == null && !string.IsNullOrEmpty(this.AccountId))
                {
                    m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return m_Account;
            }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>帐号标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary>
        /// 账号姓名
        /// </summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region 属性:Parent
        private ApplicationInfo m_Parent;

        /// <summary>应用</summary>
        public ApplicationInfo Parent
        {
            get
            {
                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = AppsContext.Instance.ApplicationService.FindOne(this.ParentId);
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId = string.Empty;

        /// <summary>父级应用标识</summary>
        public string ParentId
        {
            get
            {
                if (string.IsNullOrEmpty(m_ParentId))
                {
                    m_ParentId = "00000000-0000-0000-0000-000000000001";
                }

                return m_ParentId;
            }

            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:ParentName
        /// <summary></summary>
        public string ParentName
        {
            get { return this.Parent == null ? this.ApplicationDisplayName : this.Parent.ApplicationName; }
        }
        #endregion

        #region 属性:ParentDisplayName
        /// <summary></summary>
        public string ParentDisplayName
        {
            get { return this.Parent == null ? string.Empty : this.Parent.ApplicationDisplayName; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary>应用编码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:ApplicationName
        private string m_ApplicationName = string.Empty;

        /// <summary></summary>
        public string ApplicationName
        {
            get { return m_ApplicationName; }
            set { m_ApplicationName = value; }
        }
        #endregion

        #region 属性:ApplicationDisplayName
        private string m_ApplicationDisplayName = string.Empty;

        /// <summary></summary>
        public string ApplicationDisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ApplicationDisplayName))
                {
                    this.m_ApplicationDisplayName = this.m_ApplicationName;
                }

                return m_ApplicationDisplayName;
            }

            set
            {
                m_ApplicationDisplayName = value;
            }
        }
        #endregion

        #region 属性:ApplicationKey
        private string m_ApplicationKey = string.Empty;

        /// <summary>应用许可号</summary>
        public string ApplicationKey
        {
            get
            {
                if (string.IsNullOrEmpty(m_ApplicationKey))
                {
                    m_ApplicationKey = Guid.NewGuid().ToString("N");
                }

                if (m_ApplicationKey.Length > 32)
                {
                    m_ApplicationKey = m_ApplicationKey.Substring(0, 32);
                }

                if (m_ApplicationKey.Length < 32)
                {
                    int paddingTextCount = 32 - m_ApplicationKey.Length;

                    for (int i = 0; i < paddingTextCount; i++)
                    {
                        m_ApplicationKey += "0";
                    }
                }

                return m_ApplicationKey;
            }

            set
            {
                m_ApplicationKey = value;
            }
        }
        #endregion

        #region 属性:ApplicationSecret
        private string m_ApplicationSecret = string.Empty;

        /// <summary>应用密钥</summary>
        public string ApplicationSecret
        {
            get { return m_ApplicationSecret; }
            set { m_ApplicationSecret = value; }
        }
        #endregion

        #region 属性:PinYin
        private string m_PinYin = string.Empty;

        /// <summary>拼音简写</summary>
        public string PinYin
        {
            get { return m_PinYin; }
            set { m_PinYin = value; }
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

        #region 属性:HasChildren
        private int m_HasChildren = 0;

        /// <summary>是否有叶子节点</summary>
        public int HasChildren
        {
            get { return m_HasChildren; }
            set { m_HasChildren = value; }
        }
        #endregion

        #region 属性:AdministratorEmail
        private string m_AdministratorEmail = string.Empty;

        /// <summary></summary>
        public string AdministratorEmail
        {
            get { return m_AdministratorEmail; }
            set { m_AdministratorEmail = value; }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath = string.Empty;

        /// <summary>图标文件</summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:BigIconPath
        private string m_BigIconPath = string.Empty;

        /// <summary>大图标文件</summary>
        public string BigIconPath
        {
            get { return m_BigIconPath; }
            set { m_BigIconPath = value; }
        }
        #endregion

        #region 属性:HelpUrl
        private string m_HelpUrl = string.Empty;

        /// <summary>功能帮助文件</summary>
        public string HelpUrl
        {
            get { return m_HelpUrl; }
            set { m_HelpUrl = value; }
        }
        #endregion

        #region 属性:LicenseStatus
        private string m_LicenseStatus = string.Empty;

        /// <summary>授权状态</summary>
        public string LicenseStatus
        {
            get { return m_LicenseStatus; }
            set { m_LicenseStatus = value; }
        }
        #endregion

        #region 属性:Hidden
        private int m_Hidden;

        /// <summary>显示为菜单列表时是否隐藏。</summary>
        public int Hidden
        {
            get { return m_Hidden; }
            set { m_Hidden = value; }
        }
        #endregion

        #region 属性:Locking
        private int m_Locking;

        /// <summary>是否锁定 0:允许 1:锁定</summary>
        public int Locking
        {
            get { return this.m_Locking; }
            set { this.m_Locking = value; }
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

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate = DateTime.Now;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate = DateTime.Now;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 管理员信息
        // -------------------------------------------------------

        #region 属性:Administrators
        private IList<MembershipAuthorizationScopeObject> m_Administrators = null;

        /// <summary></summary>
        public IList<MembershipAuthorizationScopeObject> Administrators
        {
            get
            {
                if (m_Administrators == null)
                {
                    m_Administrators = AppsContext.Instance.ApplicationService.GetAuthorizationScopeObjects(this.Id, "应用_默认_管理员");
                }

                return m_Administrators;
            }
        }
        #endregion

        #region 属性:AdministratorScopeView
        private string m_AdministratorScopeView = string.Empty;

        /// <summary></summary>
        public string AdministratorScopeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AdministratorScopeView) && this.Administrators.Count > 0)
                {
                    IAuthorizationObject authorizationObject = null;

                    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.Administrators)
                    {
                        authorizationObject = authorizationScopeObject.GetAuthorizationObject();

                        if (authorizationObject != null)
                        {
                            m_AdministratorScopeView += authorizationObject.Name + ";";
                        }
                    }
                }

                return m_AdministratorScopeView;
            }
        }
        #endregion

        #region 属性:AdministratorScopeText
        private string m_AdministratorScopeText = string.Empty;

        /// <summary></summary>
        public string AdministratorScopeText
        {
            get
            {
                if (string.IsNullOrEmpty(m_AdministratorScopeText) && this.Administrators.Count > 0)
                {
                    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.Administrators)
                    {
                        m_AdministratorScopeText += authorizationScopeObject.ToString();
                    }
                }

                return m_AdministratorScopeText;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 审查员信息
        // -------------------------------------------------------

        #region 属性:Reviewers
        private IList<MembershipAuthorizationScopeObject> m_Reviewers = null;

        /// <summary></summary>
        public IList<MembershipAuthorizationScopeObject> Reviewers
        {
            get
            {
                if (m_Reviewers == null)
                {
                    m_Reviewers = AppsContext.Instance.ApplicationService.GetAuthorizationScopeObjects(this.Id, "应用_默认_审查员");
                }

                return m_Reviewers;
            }
        }
        #endregion

        #region 属性:ReviewerScopeView
        private string m_ReviewerScopeView = string.Empty;

        /// <summary></summary>
        public string ReviewerScopeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_ReviewerScopeView) && this.Reviewers.Count > 0)
                {
                    IAuthorizationObject authorizationObject = null;

                    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.Reviewers)
                    {
                        authorizationObject = authorizationScopeObject.GetAuthorizationObject();

                        if (authorizationObject != null)
                        {
                            m_ReviewerScopeView += authorizationObject.Name + ";";
                        }
                    }
                }

                return m_ReviewerScopeView;
            }
        }
        #endregion

        #region 属性:ReviewerScopeText
        private string m_ReviewerScopeText = string.Empty;

        /// <summary></summary>
        public string ReviewerScopeText
        {
            get
            {
                if (string.IsNullOrEmpty(m_ReviewerScopeText) && this.Reviewers.Count > 0)
                {
                    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.Reviewers)
                    {
                        m_ReviewerScopeText += authorizationScopeObject.ToString();
                    }
                }

                return m_ReviewerScopeText;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 可访问成员信息
        // -------------------------------------------------------

        #region 属性:Members
        private IList<MembershipAuthorizationScopeObject> m_Members = null;

        /// <summary></summary>
        public IList<MembershipAuthorizationScopeObject> Members
        {
            get
            {
                if (m_Members == null)
                {
                    m_Members = AppsContext.Instance.ApplicationService.GetAuthorizationScopeObjects(this.Id, "应用_默认_可访问成员");

                    if (m_Members.Count == 0)
                    {
                        m_Members.Add(new MembershipAuthorizationScopeObject("Role", "00000000-0000-0000-0000-000000000000", "所有人"));
                    }
                }

                return m_Members;
            }
        }
        #endregion

        #region 属性:MemberScopeView
        private string m_MemberScopeView = string.Empty;

        /// <summary></summary>
        public string MemberScopeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_MemberScopeView) && this.Members.Count > 0)
                {
                    IAuthorizationObject authorizationObject = null;

                    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.Members)
                    {
                        authorizationObject = authorizationScopeObject.GetAuthorizationObject();

                        if (authorizationObject != null)
                        {
                            m_MemberScopeView += authorizationObject.Name + ";";
                        }
                    }
                }

                return m_MemberScopeView;
            }
        }
        #endregion

        #region 属性:MemberScopeText
        private string m_MemberScopeText = string.Empty;

        /// <summary></summary>
        public string MemberScopeText
        {
            get
            {
                if (string.IsNullOrEmpty(m_MemberScopeText) && this.Members.Count > 0)
                {
                    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.Members)
                    {
                        m_MemberScopeText += authorizationScopeObject.ToString();
                    }
                }

                return m_MemberScopeText;
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
        // 显式实现 ICacheable
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.MaxValue;

        /// <summary>过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
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
                outString.Append("<!-- 应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 父级应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<applicationId><![CDATA[{0}]]></applicationId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 应用编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 应用名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.ApplicationName);
            if (displayComment)
                outString.Append("<!-- 应用显示名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<type><![CDATA[{0}]]></type>", this.ApplicationDisplayName);
            if (displayComment)
                outString.Append("<!-- 所属父级功能标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 授权对象列表 -->");
            outString.Append("<authorizationObjects>");
            //if (this.m_AuthorizationReadScopeObjects != null)
            //{
            //    foreach (MembershipAuthorizationScopeObject authorizationScopeObject in this.m_AuthorizationReadScopeObjects)
            //    {
            //        outString.AppendFormat("<authorizationObject id=\"{0}\" type=\"{1}\" />",
            //            authorizationScopeObject.AuthorizationObjectId,
            //            authorizationScopeObject.AuthorizationObjectType);
            //    }
            //}
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
            this.Id = element.GetElementsByTagName("id")[0].InnerText;
            this.Code = element.GetElementsByTagName("code")[0].InnerText;
            // this.Name = element.GetElementsByTagName("name")[0].InnerText;
            this.Status = Convert.ToInt32(element.GetElementsByTagName("status")[0].InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.GetElementsByTagName("updateDate")[0].InnerText);
        }
        #endregion
    }
}
