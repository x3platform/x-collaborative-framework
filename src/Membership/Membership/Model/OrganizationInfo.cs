#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
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
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using X3Platform.CacheBuffer;
    using X3Platform.Membership.Configuration;
    using X3Platform.Spring;
    #endregion

    /// <summary>组织单位信息</summary>
    [Serializable]
    public class OrganizationInfo : IOrganizationInfo
    {
        /// <summary></summary>
        public OrganizationInfo()
        {
        }

        /// <summary></summary>
        public OrganizationInfo(string id)
        {
            this.Id = id;
        }

        //
        // 属性, 可存放扩展属性, 临时属性
        //
        // 可添加的属性 例如: ParentId , ParentName
        //

        #region 属性:ExtensionInformation
        private IExtensionInformation m_ExtensionInformation = null;

        /// <summary>属性</summary>
        public IExtensionInformation ExtensionInformation
        {
            get
            {
                if (m_ExtensionInformation == null)
                {
                    string springObjectFile = MembershipConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

                    SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

                    string extensionInformationName = "X3Platform.Membership.Model.OrganizationInfo.ExtensionInformation";

                    m_ExtensionInformation = objectBuilder.GetObject<IExtensionInformation>(extensionInformationName);
                }

                return m_ExtensionInformation;
            }
        }
        #endregion

        //
        // 具体属性
        //

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:StandardOrganizationId
        private string m_StandardOrganizationId;

        /// <summary>标准组织标识</summary>
        public string StandardOrganizationId
        {
            get { return m_StandardOrganizationId; }
            set { m_StandardOrganizationId = value; }
        }
        #endregion

        #region 属性:StandardOrganizationName
        /// <summary>父节点名称</summary>
        public string StandardOrganizationName
        {
            get { return this.StandardOrganization == null ? string.Empty : this.StandardOrganization.Name; }
        }
        #endregion

        #region 属性:StandardOrganization
        private IStandardOrganizationInfo m_StandardOrganization = null;

        /// <summary>所属标准组织</summary>
        public IStandardOrganizationInfo StandardOrganization
        {
            get
            {
                //
                // StandardOrganizationId = "00000000-0000-0000-0000-000000000000" 表示所属标准组织为空
                // 
                if (this.StandardOrganizationId == Guid.Empty.ToString())
                {
                    return null;
                }

                if (m_StandardOrganization == null && !string.IsNullOrEmpty(this.StandardOrganizationId))
                {
                    m_StandardOrganization = MembershipManagement.Instance.StandardOrganizationService[this.StandardOrganizationId];
                }

                return m_StandardOrganization;
            }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary>代码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:FullName
        private string m_FullName = string.Empty;

        /// <summary>全称</summary>
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(m_FullName))
                {
                    m_FullName = this.Name;
                }

                return m_FullName;
            }
            set { m_FullName = value; }
        }
        #endregion

        #region 属性:GlobalName
        private string m_GlobalName = string.Empty;

        /// <summary>全局名称</summary>
        public string GlobalName
        {
            get { return m_GlobalName; }
            set { m_GlobalName = value; }
        }
        #endregion

        #region 属性:PinYin
        private string m_PinYin = string.Empty;

        /// <summary>拼音</summary>
        public string PinYin
        {
            get { return m_PinYin; }
            set { m_PinYin = value; }
        }
        #endregion

        #region 属性:Type
        private int m_Type = 0;

        /// <summary>类型 -1:区域 0:公司 1:部门 2:项目团队 3:项目 4:项目分期</summary>
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 属性:TypeView
        private string m_TypeView;

        /// <summary>类型视图</summary>
        public string TypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_TypeView) && this.Type != -1)
                {
                    m_TypeView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_组织管理_组织类别",
                        this.Type.ToString());
                }

                return m_TypeView;
            }
        }
        #endregion

        #region 属性:Level
        private int m_Level = 0;

        /// <summary>层级</summary>
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId;

        /// <summary>父节点标识</summary>
        public string ParentId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ParentId))
                {
                    this.m_ParentId = Guid.Empty.ToString();
                }

                return m_ParentId;
            }
            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:ParentGlobalName
        /// <summary>父节点全局名称</summary>
        public string ParentGlobalName
        {
            get { return this.Parent == null ? string.Empty : this.Parent.GlobalName; }
        }
        #endregion

        #region 属性:Parent
        private IOrganizationInfo m_Parent = null;

        /// <summary>父级对象</summary>
        public IOrganizationInfo Parent
        {
            get
            {
                //
                // ParentId = "00000000-0000-0000-0000-000000000000" 表示父节点为空
                // 系统中的特殊角色[所有人]的Id为"00000000-0000-0000-0000-000000000000".
                // 所以为避免错误, 当ParentId = "00000000-0000-0000-0000-000000000000", 直接返回null.
                // 
                if (this.ParentId == Guid.Empty.ToString())
                {
                    return null;
                }

                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = MembershipManagement.Instance.OrganizationService[this.ParentId];
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:CorporationId
        /// <summary>公司标识</summary>
        public string CorporationId
        {
            get { return this.Corporation == null ? string.Empty : this.Corporation.Id; }
        }
        #endregion

        #region 属性:CorporationName
        /// <summary>公司名称</summary>
        public string CorporationName
        {
            get { return this.Corporation == null ? string.Empty : this.Corporation.Name; }
        }
        #endregion

        #region 属性:Corporation
        private IOrganizationInfo m_Corporation = null;

        /// <summary>公司</summary>
        public IOrganizationInfo Corporation
        {
            get
            {
                if (m_Corporation == null && !string.IsNullOrEmpty(this.Id))
                {
                    m_Corporation = MembershipManagement.Instance.OrganizationService.FindCorporationByOrganizationId(this.Id);
                }

                return m_Corporation;
            }
        }
        #endregion

        #region 属性:ChindNodes
        private IList<IAuthorizationObject> m_ChindNodes = null;

        /// <summary>子节点</summary>
        public IList<IAuthorizationObject> ChindNodes
        {
            get
            {
                if (m_ChindNodes == null)
                {
                    m_ChindNodes = MembershipManagement.Instance.OrganizationService.GetChildNodes(this.Id);
                }

                return m_ChindNodes;
            }
        }

        #endregion

        #region 属性:Roles
        private IList<IRoleInfo> m_Roles = null;

        /// <summary>所属角色信息</summary>
        public IList<IRoleInfo> Roles
        {
            get
            {
                if (m_Roles == null)
                {
                    this.m_Roles = MembershipManagement.Instance.RoleService.FindAllByOrganizationId(this.Id);
                }

                return this.m_Roles;
            }
        }
        #endregion

        #region 属性:RoleText
        private string m_RoleText;

        /// <summary>所属角色视图</summary>
        public string RoleText
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RoleText) && this.Roles.Count > 0)
                {
                    foreach (IRoleInfo role in this.Roles)
                    {
                        this.m_RoleText += string.Format("role#{0}#{1},", role.Id, role.GlobalName);
                    }

                    this.m_RoleText = this.m_RoleText.TrimEnd(',');
                }

                return this.m_RoleText;
            }
        }
        #endregion

        #region 属性:RoleView
        private string m_RoleView;

        /// <summary>所属角色视图</summary>
        public string RoleView
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RoleView) && this.Roles.Count > 0)
                {
                    foreach (IRoleInfo role in this.Roles)
                    {
                        this.m_RoleView += role.GlobalName + ";";
                    }

                    this.m_RoleView = this.m_RoleView.TrimEnd(';');
                }

                return this.m_RoleView;
            }
        }
        #endregion

        #region 属性:RoleMemberView
        private string m_RoleMemberView = string.Empty;

        /// <summary>所属角色的成员视图</summary>
        public string RoleMemberView
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_RoleMemberView) && this.Roles.Count > 0)
                {
                    IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAllByOrganizationId(this.Id);

                    foreach (IAccountInfo item in list)
                    {
                        this.m_RoleMemberView += (string.IsNullOrEmpty(item.GlobalName) ? item.Name : item.GlobalName) + ";";
                    }

                    this.m_RoleMemberView = this.m_RoleMemberView.TrimEnd(';');
                }

                return this.m_RoleMemberView;
            }
        }
        #endregion

        #region 属性:EnableExchangeEmail
        private int m_EnableExchangeEmail;

        /// <summary>是否启用邮件</summary>
        public int EnableExchangeEmail
        {
            get { return m_EnableExchangeEmail; }
            set { m_EnableExchangeEmail = value; }
        }
        #endregion

        #region 属性:EffectScope
        private int m_EffectScope = -1;

        /// <summary>作用范围 0:local | 1:deep</summary>
        public int EffectScope
        {
            get { return m_EffectScope; }
            set { m_EffectScope = value; }
        }
        #endregion

        #region 属性:EffectScopeView
        private string m_EffectScopeView;

        /// <summary>作用范围视图</summary>
        public string EffectScopeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_EffectScopeView) && this.EffectScope != -1)
                {
                    m_EffectScopeView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_权限作用范围",
                        this.EffectScope.ToString());
                }

                return m_EffectScopeView;
            }
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

        /// <summary>排序</summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary>状态</summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary>备注</summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
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

        /// <summary>修改时间</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建时间</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        //
        // 显示实现 ICacheable
        // 

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(6);

        /// <summary>过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion

        //
        // 显式实现 IAuthorizationObject Type
        // 

        #region 属性:IAuthorizationObject.Type
        /// <summary>类型</summary>
        string IAuthorizationObject.Type
        {
            get { return "Organization"; }
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

            if (displayComment)
                outString.Append("<!-- 组织对象 -->");
            outString.Append("<organization>");
            if (displayComment)
                outString.Append("<!-- 组织标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 全称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<fullName><![CDATA[{0}]]></fullName>", this.FullName);
            if (displayComment)
                outString.Append("<!-- 全局名称 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<globalName><![CDATA[{0}]]></globalName>", this.GlobalName);
            if (displayComment)
                outString.Append("<!-- 拼音 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<pinyin><![CDATA[{0}]]></pinyin>", this.PinYin);
            if (displayComment)
                outString.Append("<!-- 类型 (整型) (int) -->");
            outString.AppendFormat("<type><![CDATA[{0}]]></type>", this.Type);
            if (displayComment)
                outString.Append("<!-- 父级对象标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayFriendlyName)
            {
                if (displayComment)
                    outString.Append("<!-- 父级对象名称 (字符串) (nvarchar(50)) -->");
                outString.AppendFormat("<parentName><![CDATA[{0}]]></parentName>", this.ParentGlobalName);
            }
            if (displayComment)
                outString.Append("<!-- 排序编号 (字符串) nvarchar(20) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.UpdateDate);
            outString.Append("</organization>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>根据Xml元素加载对象</summary>
        /// <param name="element">Xml元素</param>
        public void Deserialize(XmlElement element)
        {
            // 以下数据由人力资源输入.
            this.Id = element.SelectSingleNode("id").InnerText;

            if (element.SelectSingleNode("standardOrganizationId") != null)
            {
                this.StandardOrganizationId = element.SelectSingleNode("standardOrganizationId").InnerText;
            }

            this.Code = element.SelectSingleNode("code").InnerText;
            this.Name = element.SelectSingleNode("name").InnerText;

            if (element.SelectSingleNode("fullName") != null)
            {
                this.FullName = element.SelectSingleNode("fullName").InnerText;
            }
            if (element.SelectSingleNode("globalName") != null)
            {
                this.GlobalName = element.SelectSingleNode("globalName").InnerText;
            }
            if (element.SelectSingleNode("pinyin") != null)
            {
                this.PinYin = element.SelectSingleNode("pinyin").InnerText;
            }
            this.Type = Convert.ToInt32(element.SelectSingleNode("type").InnerText);
            this.ParentId = element.SelectSingleNode("parentId").InnerText;
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            if (element.SelectSingleNode("remark") != null)
            {
                this.Remark = element.SelectSingleNode("remark").InnerText;
            }
            this.UpdateDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
