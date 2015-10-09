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

    using X3Platform.Membership.Configuration;
    using X3Platform.Security.Authority;
    using X3Platform.Spring;
    #endregion

    /// <summary>普通角色信息</summary>
    [Serializable]
    public class RoleInfo : IRoleInfo
    {
        /// <summary></summary>
        public RoleInfo() { }

        /// <summary></summary>
        public RoleInfo(string id)
        {
            this.Id = id;
        }

        //
        // 属性, 可存放扩展属性, 临时属性
        //
        // 可添加的属性 例如: ParentId , ParentName
        //

        #region 属性:Properties
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

                    string extensionInformationName = "X3Platform.Membership.Model.RoleInfo.ExtensionInformation";

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

        /// <summary>名称</summary>
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

        /// <summary>拼音</summary>
        public string PinYin
        {
            get { return m_PinYin; }
            set { m_PinYin = value; }
        }
        #endregion

        #region 属性:Type
        private int m_Type;

        /// <summary>类型</summary>
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
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

        #region 属性:ParentName
        /// <summary>父节点名称</summary>
        public string ParentName
        {
            get { return this.Parent == null ? string.Empty : this.Parent.Name; }
        }
        #endregion

        #region 属性:Parent
        private IRoleInfo m_Parent = null;

        /// <summary>父级对象</summary>
        public IRoleInfo Parent
        {
            get
            {
                //
                // ParentId = "00000000-0000-0000-0000-000000000000" 表示父节点为空
                // 系统中的特殊角色[所有人]的Id为"00000000-0000-0000-0000-000000000000".
                // 所以为避免错误, 当ParentId = "00000000-0000-0000-0000-000000000000", 直接返回null.
                // 
                if (this.ParentId == "00000000-0000-0000-0000-000000000000")
                {
                    return null;
                }

                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = MembershipManagement.Instance.RoleService[this.ParentId];
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
        private IOrganizationUnitInfo m_Corporation = null;

        /// <summary>公司</summary>
        public IOrganizationUnitInfo Corporation
        {
            get
            {
                if (m_Corporation == null && !string.IsNullOrEmpty(this.OrganizationUnitId))
                {
                    m_Corporation = MembershipManagement.Instance.OrganizationUnitService.FindCorporationByOrganizationUnitId(this.OrganizationUnitId);
                }

                return m_Corporation;
            }
        }
        #endregion

        #region 属性:GeneralRoleId
        private string m_GeneralRoleId;

        /// <summary>所属通用角色标识</summary>
        public string GeneralRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_GeneralRoleId))
                {
                    this.m_GeneralRoleId = MembershipConfigurationView.Instance.DefaultGeneralRoleId;
                }

                return m_GeneralRoleId;
            }
            set { m_GeneralRoleId = value; }
        }
        #endregion

        #region 属性:GeneralRoleName
        /// <summary>所属通用角色名称</summary>
        public string GeneralRoleName
        {
            get { return this.GeneralRole == null ? string.Empty : this.GeneralRole.Name; }
        }
        #endregion

        #region 属性:GeneralRole
        private GeneralRoleInfo m_GeneralRole = null;

        /// <summary>所属通用角色</summary>
        public GeneralRoleInfo GeneralRole
        {
            get
            {
                if (m_GeneralRole == null && !string.IsNullOrEmpty(this.GeneralRoleId))
                {
                    m_GeneralRole = MembershipManagement.Instance.GeneralRoleService[this.GeneralRoleId];
                }

                return m_GeneralRole;
            }
        }
        #endregion

        #region 属性:StandardRoleId
        private string m_StandardRoleId;

        /// <summary>标准角色标识</summary>
        public string StandardRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_StandardRoleId))
                {
                    this.m_StandardRoleId = MembershipConfigurationView.Instance.DefaultStandardRoleId;
                }

                return m_StandardRoleId;
            }
            set { m_StandardRoleId = value; }
        }
        #endregion

        #region 属性:StandardRoleName
        /// <summary>标准角色名称</summary>
        public string StandardRoleName
        {
            get { return this.StandardRole == null ? string.Empty : this.StandardRole.Name; }
        }
        #endregion

        #region 属性:StandardRole
        private IStandardRoleInfo m_StandardRole = null;

        /// <summary>父级对象</summary>
        public IStandardRoleInfo StandardRole
        {
            get
            {
                if (m_StandardRole == null && !string.IsNullOrEmpty(this.StandardRoleId))
                {
                    m_StandardRole = MembershipManagement.Instance.StandardRoleService[this.StandardRoleId];
                }

                return m_StandardRole;
            }
        }
        #endregion

        #region 属性:OrganizationUnitId
        private string m_OrganizationUnitId;

        /// <summary>所属组织标识</summary>
        public string OrganizationUnitId
        {
            get { return m_OrganizationUnitId; }
            set { m_OrganizationUnitId = value; }
        }
        #endregion

        #region 属性:OrganizationUnitGlobalName
        /// <summary>所属组织全局名称</summary>
        public string OrganizationUnitGlobalName
        {
            get { return this.OrganizationUnit == null ? string.Empty : this.OrganizationUnit.GlobalName; }
        }
        #endregion

        #region 属性:OrganizationUnit
        private IOrganizationUnitInfo m_OrganizationUnit = null;

        /// <summary>父级对象</summary>
        public IOrganizationUnitInfo OrganizationUnit
        {
            get
            {
                if (m_OrganizationUnit == null && !string.IsNullOrEmpty(this.OrganizationUnitId))
                {
                    m_OrganizationUnit = MembershipManagement.Instance.OrganizationUnitService[this.OrganizationUnitId];
                }

                return m_OrganizationUnit;
            }
        }
        #endregion

        #region 属性:AssignedJob
        private IList<IAssignedJobInfo> m_AssignedJobs = null;

        /// <summary>相关岗位信息</summary>
        public IList<IAssignedJobInfo> AssignedJobs
        {
            get
            {
                if (m_AssignedJobs == null && !string.IsNullOrEmpty(this.StandardRoleId))
                {
                    m_AssignedJobs = MembershipManagement.Instance.AssignedJobService.FindAllByRoleId(this.Id);
                }

                return m_AssignedJobs;
            }
        }
        #endregion

        #region 属性:AssignedJobName
        /// <summary>所属岗位名称</summary>
        public string AssignedJobNames
        {
            get
            {
                if (this.AssignedJobs == null)
                    return string.Empty;

                string outString = null;

                foreach (IAssignedJobInfo item in this.AssignedJobs)
                {
                    outString += item.Name + ";";
                }

                return outString;
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
        private int m_EffectScope;

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

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>修改时间</summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建时间</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 重置成员关系
        // -------------------------------------------------------

        #region 函数:ResetMemberRelations(string relationText)
        /// <summary>重置成员关系</summary>
        /// <param name="relationText"></param>
        public void ResetMemberRelations(string relationText)
        {
            string[] list = relationText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            // 清空旧的成员关系
            this.Members.Clear();

            string[] keys = null;

            // 设置新的成员关系
            foreach (string item in list)
            {
                keys = item.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                if (keys.Length > 2 && keys[0] == "account")
                {
                    this.Members.Add(MembershipManagement.Instance.AccountService[keys[1]]);
                }
            }
        }
        #endregion

        // -------------------------------------------------------
        // 角色所拥有的成员
        // -------------------------------------------------------

        #region 属性:Members
        private IList<IAccountInfo> m_Members = null;

        /// <summary>权限</summary>
        public IList<IAccountInfo> Members
        {
            get
            {
                if (m_Members == null && !string.IsNullOrEmpty(this.Id))
                {
                    m_Members = MembershipManagement.Instance.AccountService.FindAllByRoleId(this.Id);
                }

                return m_Members;
            }
        }
        #endregion

        #region 属性:MemberView
        private string m_MemberView;

        /// <summary>权限视图</summary>
        public string MemberView
        {
            get
            {
                if (this.Members != null && string.IsNullOrEmpty(this.m_MemberView) && this.Members.Count > 0)
                {
                    foreach (IAccountInfo item in Members)
                    {
                        this.m_MemberView += item.GlobalName + ";";
                    }

                    this.m_MemberView = this.m_MemberView.TrimEnd(';');
                }

                return this.m_MemberView;
            }
        }
        #endregion

        #region 属性:MemberText
        private string m_MemberText;

        /// <summary>成员视图</summary>
        public string MemberText
        {
            get
            {
                if (this.Members != null && string.IsNullOrEmpty(this.m_MemberText) && this.Members.Count > 0)
                {
                    foreach (IAccountInfo item in Members)
                    {
                        this.m_MemberText += string.Format("account#{0}#{1},", item.Id, item.GlobalName);
                    }

                    this.m_MemberText = this.m_MemberText.TrimEnd(',');
                }

                return this.m_MemberText;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 IAuthorizationObject Type
        // -------------------------------------------------------
        
        #region 属性:IAuthorizationObject.Type
        /// <summary>类型</summary>
        string IAuthorizationObject.Type
        {
            get { return "Role"; }
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
                outString.Append("<!-- 角色对象 -->");
            outString.Append("<role>");
            if (displayComment)
                outString.Append("<!-- 角色标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 拼音 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<pinyin><![CDATA[{0}]]></pinyin>", this.PinYin);
            if (displayComment)
                outString.Append("<!-- 所属标准角色标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<standardRoleId><![CDATA[{0}]]></standardRoleId>", this.StandardRoleId);
            if (displayComment)
                outString.Append("<!-- 所属组织标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<organizationId><![CDATA[{0}]]></organizationId>", this.OrganizationUnitId);
            if (displayComment)
                outString.Append("<!-- 所属旧版通用角色标识(兼容门户系统，可以忽略。) (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<generalRoleId><![CDATA[{0}]]></generalRoleId>", this.GeneralRoleId);
            if (displayComment)
                outString.Append("<!-- 上级角色标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 排序编号(字符串) nvarchar(20) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</role>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>根据Xml元素加载对象</summary>
        /// <param name="element">Xml元素</param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.SelectSingleNode("id").InnerText;
            this.Code = element.SelectSingleNode("code").InnerText;
            this.Name = element.SelectSingleNode("name").InnerText;
            this.PinYin = element.SelectSingleNode("pinyin").InnerText;
            this.OrganizationUnitId = element.SelectSingleNode("organizationId").InnerText;
            this.StandardRoleId = element.SelectSingleNode("standardRoleId").InnerText;
            this.ParentId = element.SelectSingleNode("parentId").InnerText;
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;
            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
