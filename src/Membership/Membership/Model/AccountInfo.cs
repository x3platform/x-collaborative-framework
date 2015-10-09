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
    using System.Text;
    using System.Collections.Generic;
    using System.Xml;

    using X3Platform.CacheBuffer;
    using X3Platform.Util;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>帐户信息</summary>
    [Serializable]
    public class AccountInfo : IAccountInfo
    {
        /// <summary></summary>
        public AccountInfo() { }

        //
        // 属性, 可存放扩展属性, 临时属性
        //
        // 可添加的属性 例如: OriginalPassword, OriginalNickName 
        //

        #region 属性:Properties
        private Dictionary<string, object> m_Properties = new Dictionary<string, object>();

        /// <summary>属性</summary>
        public Dictionary<string, object> Properties
        {
            get { return m_Properties; }
        }
        #endregion

        //
        // 具体属性
        //

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>用户标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary>编码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
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

        #region 属性:DisplayName
        private string m_DisplayName = string.Empty;

        /// <summary>显示名称</summary>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(m_DisplayName))
                {
                    m_DisplayName = this.Name;
                }

                return m_DisplayName;
            }
            set { m_DisplayName = value; }
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

        #region 属性:LoginName
        private string m_LoginName = string.Empty;

        /// <summary>登录名</summary>
        public string LoginName
        {
            get { return m_LoginName; }
            set { m_LoginName = value; }
        }
        #endregion

        #region 属性:Password
        private string m_Password;

        /// <summary>密码</summary>
        public string Password
        {
            get { return this.m_Password; }
            set { this.m_Password = value; }
        }
        #endregion

        #region 属性:PasswordChangedDate
        private DateTime m_PasswordChangedDate;

        /// <summary>密码更新时间</summary>
        public DateTime PasswordChangedDate
        {
            get { return this.m_PasswordChangedDate; }
            set { this.m_PasswordChangedDate = value; }
        }
        #endregion

        #region 属性:IdentityCard
        private string m_IdentityCard = string.Empty;

        /// <summary>身份证</summary>
        public string IdentityCard
        {
            get { return m_IdentityCard; }
            set { m_IdentityCard = value; }
        }
        #endregion

        #region 属性:Type
        private int m_Type = 0;

        /// <summary>帐号类型 0:普通帐号 1:邮箱帐号 2:Rtx帐号 3:CRM帐号 1000:供应商帐号 2000:客户帐号</summary>
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 属性:TypeView
        private string m_TypeView;

        /// <summary>帐号类别视图 0:普通帐号 1:邮箱帐号 2:CRM帐号 3:RTX帐号 1000:供应商帐号 2000:客户帐号</summary>
        public string TypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_TypeView))
                {
                    m_TypeView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_帐号管理_帐号类别",
                        this.Type.ToString()
                        );
                }

                return m_TypeView;
            }
        }
        #endregion

        #region 属性:CertifiedTelephone
        private string m_CertifiedTelephone;

        /// <summary>已验证的电话</summary>
        public string CertifiedTelephone
        {
            get { return m_CertifiedTelephone; }
            set { m_CertifiedTelephone = value; }
        }
        #endregion

        #region 属性:CertifiedEmail
        private string m_CertifiedEmail;

        /// <summary>已验证的邮箱</summary>
        public string CertifiedEmail
        {
            get { return m_CertifiedEmail; }
            set { m_CertifiedEmail = value; }
        }
        #endregion

        #region 属性:CertifiedAvatar
        private string m_CertifiedAvatar = string.Empty;

        /// <summary>已验证的头像</summary>
        public string CertifiedAvatar
        {
            get { return m_CertifiedAvatar; }
            set { m_CertifiedAvatar = value; }
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

        #region 属性:Parent
        private IAuthorizationObject m_Parent;

        /// <summary>父级权限对象接口集合</summary>
        public IAuthorizationObject Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
        #endregion

        #region 属性:OrganizationUnitRelations
        private IList<IAccountOrganizationUnitRelationInfo> m_OrganizationUnitRelations = null;

        /// <summary>帐号和角色接口集合</summary>
        public IList<IAccountOrganizationUnitRelationInfo> OrganizationUnitRelations
        {
            get
            {
                if (m_OrganizationUnitRelations == null && !string.IsNullOrEmpty(this.Id))
                {
                    m_OrganizationUnitRelations = MembershipManagement.Instance.OrganizationUnitService.FindAllRelationByAccountId(this.Id);
                }

                return m_OrganizationUnitRelations;
            }
        }
        #endregion

        #region 属性:OrganizationUnitText
        private string m_OrganizationUnitText = string.Empty;

        /// <summary>所属组织视图</summary>
        public string OrganizationUnitText
        {
            get
            {
                if (string.IsNullOrEmpty(m_OrganizationUnitText) && this.OrganizationUnitRelations.Count > 0)
                {
                    foreach (IAccountOrganizationUnitRelationInfo relation in this.OrganizationUnitRelations)
                    {
                        m_OrganizationUnitText += string.Format("organization#{0}#{1}#{2}#{3},", relation.OrganizationUnitId, relation.OrganizationUnitGlobalName, relation.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), relation.IsDefault);
                    }
                }

                return m_OrganizationUnitText;
            }
        }
        #endregion

        #region 属性:OrganizationUnitView
        private string m_OrganizationUnitView = string.Empty;

        /// <summary>所属组织视图</summary>
        public string OrganizationUnitView
        {
            get
            {
                if (string.IsNullOrEmpty(m_OrganizationUnitView) && this.OrganizationUnitRelations.Count > 0)
                {
                    foreach (IAccountOrganizationUnitRelationInfo relation in this.OrganizationUnitRelations)
                    {
                        m_OrganizationUnitView += relation.OrganizationUnitGlobalName
                            + ((relation.EndDate.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss")) ? string.Empty : ("(" + StringHelper.ToDate(relation.EndDate) + ")"))
                            + ";";
                    }
                }

                return m_OrganizationUnitView;
            }
        }
        #endregion

        #region 属性:RoleRelations
        private IList<IAccountRoleRelationInfo> m_RoleRelations = null;

        /// <summary>帐号和角色接口集合</summary>
        public IList<IAccountRoleRelationInfo> RoleRelations
        {
            get
            {
                if (m_RoleRelations == null && !string.IsNullOrEmpty(this.Id))
                {
                    m_RoleRelations = MembershipManagement.Instance.RoleService.FindAllRelationByAccountId(this.Id);
                }

                return m_RoleRelations;
            }
        }
        #endregion

        #region 属性:RoleText
        private string m_RoleText = string.Empty;

        /// <summary>所属角色视图</summary>
        public string RoleText
        {
            get
            {
                if (string.IsNullOrEmpty(m_RoleText) && this.RoleRelations.Count > 0)
                {
                    foreach (IAccountRoleRelationInfo relation in this.RoleRelations)
                    {
                        m_RoleText += string.Format("role#{0}#{1}#{2}#{3},", relation.RoleId, relation.RoleGlobalName, relation.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), relation.IsDefault);
                    }
                }

                return m_RoleText;
            }
        }
        #endregion

        #region 属性:RoleView
        private string m_RoleView = string.Empty;

        /// <summary>所属角色视图</summary>
        public string RoleView
        {
            get
            {
                if (string.IsNullOrEmpty(m_RoleView) && this.RoleRelations.Count > 0)
                {
                    foreach (IAccountRoleRelationInfo relation in this.RoleRelations)
                    {
                        m_RoleView += relation.RoleGlobalName
                            + ((relation.EndDate.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss")) ? string.Empty : ("(" + StringHelper.ToDate(relation.EndDate) + ")"))
                            + ";";
                    }
                }

                return m_RoleView;
            }
        }
        #endregion

        #region 属性:GroupRelations
        private IList<IAccountGroupRelationInfo> m_GroupRelations = null;

        /// <summary>帐号和角色接口集合</summary>
        public IList<IAccountGroupRelationInfo> GroupRelations
        {
            get
            {
                if (m_GroupRelations == null && !string.IsNullOrEmpty(this.Id))
                {
                    m_GroupRelations = MembershipManagement.Instance.GroupService.FindAllRelationByAccountId(this.Id);
                }

                return m_GroupRelations;
            }
        }
        #endregion

        #region 属性:GroupText
        private string m_GroupText;

        /// <summary>所属群组文本信息</summary>
        public string GroupText
        {
            get
            {
                if (string.IsNullOrEmpty(m_GroupText) && this.GroupRelations.Count > 0)
                {
                    foreach (IAccountGroupRelationInfo relation in this.GroupRelations)
                    {
                        m_GroupText += string.Format("group#{0}#{1}#{2},", relation.GroupId, relation.GroupGlobalName, relation.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }

                return m_GroupText;
            }
        }
        #endregion

        #region 属性:GroupView
        private string m_GroupView;

        /// <summary>所属群组视图</summary>
        public string GroupView
        {
            get
            {
                if (string.IsNullOrEmpty(m_GroupView) && this.GroupRelations.Count > 0)
                {
                    foreach (IAccountGroupRelationInfo relation in this.GroupRelations)
                    {
                        m_GroupView += relation.GroupGlobalName
                            + ((relation.EndDate.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss")) ? string.Empty : ("(" + StringHelper.ToDate(relation.EndDate) + ")"))
                            + ";";
                    }
                }

                return m_GroupView;
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

        #region 属性:IsDraft
        private bool m_IsDraft;

        /// <summary>是否是草稿</summary>
        public bool IsDraft
        {
            get { return m_IsDraft; }
            set { m_IsDraft = value; }
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

        #region 属性:IP
        private string m_IP;

        /// <summary>IP</summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        #endregion

        #region 属性:LoginDate
        private DateTime m_LoginDate;

        /// <summary>最近一次的登录时间</summary>
        public DateTime LoginDate
        {
            get { return m_LoginDate; }
            set { m_LoginDate = value; }
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

        //
        // 重置关系
        //

        /// <summary></summary>
        /// <param name="relationType"></param>
        /// <param name="relationText"></param>
        public void ResetRelations(string relationType, string relationText)
        {
            string[] list = relationText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (relationType == "organization")
                this.OrganizationUnitRelations.Clear();

            if (relationType == "role")
                this.RoleRelations.Clear();

            if (relationType == "group")
                this.GroupRelations.Clear();

            // 设置组织关系

            foreach (string item in list)
            {
                string[] keys = item.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                if (keys.Length > 2)
                {
                    switch (keys[0])
                    {
                        case "organization":
                            if (relationType == "organization")
                                this.OrganizationUnitRelations.Add(new AccountOrganizationUnitRelationInfo(this.Id, keys[1]));
                            break;
                        case "role":
                            if (relationType == "role")
                                this.RoleRelations.Add(new AccountRoleRelationInfo(this.Id, keys[1]));
                            break;
                        case "group":
                            if (relationType == "group")
                                this.GroupRelations.Add(new AccountGroupRelationInfo(this.Id, keys[1]));
                            break;
                    }
                }
            }
        }

        // -------------------------------------------------------
        // 实现 IIdentity 接口
        // -------------------------------------------------------

        #region 属性:AuthenticationType
        /// <summary></summary>
        public string AuthenticationType
        {
            get { return "MembershipAuthentication"; }
        }
        #endregion

        #region 属性:IsAuthenticated
        /// <summary></summary>
        public bool IsAuthenticated
        {
            get { return this.Id == "00000000-0000-0000-0000-000000000000" ? false : true; }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 ICacheable 接口
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(6);

        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 IAuthorizationObject Type
        // -------------------------------------------------------

        #region 属性:IAuthorizationObject.Name
        /// <summary>名称</summary>
        string IAuthorizationObject.Name
        {
            get { return string.Format("{0}", this.Name, this.LoginName); }
            set
            {
                int index = value.IndexOf("(");
                if (index > -1)
                {
                    this.Name = value.Substring(0, index);
                    this.LoginName = value.Substring((index + 1), (value.Length - index - 2));
                }
            }
        }
        #endregion

        #region 属性:IAuthorizationObject.Type
        /// <summary>类型</summary>
        string IAuthorizationObject.Type
        {
            get { return "Account"; }
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
        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<account>");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            outString.AppendFormat("<loginName><![CDATA[{0}]]></loginName>", this.LoginName);
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            outString.AppendFormat("<globalName><![CDATA[{0}]]></globalName>", this.GlobalName);
            outString.AppendFormat("<alias><![CDATA[{0}]]></alias>", this.DisplayName);
            outString.AppendFormat("<pinyin><![CDATA[{0}]]></pinyin>", this.PinYin);
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</account>");

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

            if (element.SelectSingleNode("loginName") != null)
            {
                this.LoginName = element.SelectSingleNode("loginName").InnerText;
            }

            this.Name = element.SelectSingleNode("name").InnerText;

            if (element.SelectSingleNode("globalName") != null)
            {
                this.GlobalName = element.SelectSingleNode("globalName").InnerText;
            }

            if (element.SelectSingleNode("alias") != null)
            {
                this.DisplayName = element.SelectSingleNode("alias").InnerText;
            }

            if (element.SelectSingleNode("pinyin") != null)
            {
                this.PinYin = element.SelectSingleNode("pinyin").InnerText;
            }

            // 关系列表
            XmlNodeList list = element.SelectNodes("relationObjects/relationObject");

            // 由于设置了Id
            this.m_OrganizationUnitRelations = new List<IAccountOrganizationUnitRelationInfo>();
            this.m_RoleRelations = new List<IAccountRoleRelationInfo>();
            this.m_GroupRelations = new List<IAccountGroupRelationInfo>();

            foreach (XmlNode node in list)
            {
                switch (node.Attributes["type"].Value)
                {
                    case "OrganizationUnit":
                        this.OrganizationUnitRelations.Add(new AccountOrganizationUnitRelationInfo(this.Id, node.Attributes["id"].Value));
                        break;

                    case "Role":
                        this.RoleRelations.Add(new AccountRoleRelationInfo(this.Id, node.Attributes["id"].Value));
                        break;

                    case "Group":
                        this.GroupRelations.Add(new AccountGroupRelationInfo(this.Id, node.Attributes["id"].Value));
                        break;
                }
            }

            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
        }
        #endregion
    }
}
