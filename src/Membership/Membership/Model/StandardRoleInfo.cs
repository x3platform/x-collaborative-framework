#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardRoleInfo.cs
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
    using System.Xml;
    using System.Text;
    #endregion

    /// <summary>标准角色</summary>
    public class StandardRoleInfo : IStandardRoleInfo
    {
        #region 构造函数:StandardRoleInfo()
        /// <summary>默认构造函数</summary>
        public StandardRoleInfo() { }
        #endregion

        #region 构造函数:StandardRoleInfo(string id)
        /// <summary>默认构造函数</summary>
        public StandardRoleInfo(string id)
        {
            this.Id = id;
        }
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

        #region 属性:Type
        private int m_Type;

        /// <summary>类别</summary>
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 属性:TypeView
        private string m_TypeView;

        /// <summary>类别视图</summary>
        public string TypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_TypeView))
                {
                    m_TypeView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_标准角色管理_标准角色类别",
                        this.Type.ToString()
                        );
                }

                return m_TypeView;
            }
        }
        #endregion

        #region 属性:Priority
        private int m_Priority;

        /// <summary>权重</summary>
        public int Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion

        #region 属性:PriorityView
        private string m_PriorityView;

        /// <summary>权重视图</summary>
        public string PriorityView
        {
            get
            {
                if (string.IsNullOrEmpty(m_PriorityView))
                {
                    m_PriorityView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_标准角色管理_标准角色权重",
                        this.Priority.ToString()
                        );
                }

                return m_PriorityView;
            }
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
        private IStandardRoleInfo m_Parent = null;

        /// <summary>父级对象</summary>
        public IStandardRoleInfo Parent
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
                    m_Parent = MembershipManagement.Instance.StandardRoleService[this.ParentId];
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:StandardOrganizationUnitId
        private string m_StandardOrganizationUnitId;

        /// <summary></summary>
        public string StandardOrganizationUnitId
        {
            get { return m_StandardOrganizationUnitId; }
            set { m_StandardOrganizationUnitId = value; }
        }
        #endregion

        #region 属性:StandardOrganizationUnitName
        /// <summary>标准角色名称</summary>
        public string StandardOrganizationUnitName
        {
            get { return this.StandardOrganizationUnit == null ? string.Empty : this.StandardOrganizationUnit.Name; }
        }
        #endregion

        #region 属性:StandardOrganizationUnit
        private IStandardOrganizationUnitInfo m_StandardOrganizationUnit = null;

        /// <summary>所属的标准组织</summary>
        public IStandardOrganizationUnitInfo StandardOrganizationUnit
        {
            get
            {
                if (m_StandardOrganizationUnit == null && !string.IsNullOrEmpty(this.StandardOrganizationUnitId))
                {
                    m_StandardOrganizationUnit = MembershipManagement.Instance.StandardOrganizationUnitService[this.StandardOrganizationUnitId];
                }

                return m_StandardOrganizationUnit;
            }
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

        #region 属性:IsKey
        private bool m_IsKey;

        /// <summary>是否是关键角色</summary>
        public bool IsKey
        {
            get { return m_IsKey; }
            set { m_IsKey = value; }
        }
        #endregion

        #region 属性:IsDraft
        private bool m_IsDraft;

        /// <summary>是否是草稿对象</summary>
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

        /// <summary></summary>
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
            get { return "StandardRole"; }
        }
        #endregion

        // -------------------------------------------------------
        // 实现 ISerializedObject 序列化
        // -------------------------------------------------------

        #region 函数:Serializable()
        /// <summary></summary>
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
            if (displayComment)
                outString.Append("<!-- 标准角色对象 -->");
            outString.Append("<standardRole>");
            if (displayComment)
                outString.Append("<!-- 标准角色标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 父级对象标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 所属标准组织标识(字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<standardOrganizationUnitId><![CDATA[{0}]]></standardOrganizationUnitId>", this.StandardOrganizationUnitId);
            if (displayComment)
                outString.Append("<!-- 标准角色类型(整型) (int) -->");
            outString.AppendFormat("<type><![CDATA[{0}]]></type>", this.Type);
            if (displayComment)
                outString.Append("<!-- 权重值(兼容门户系统，可以忽略。) (整型) (int) -->");
            outString.AppendFormat("<priority><![CDATA[{0}]]></priority>", this.Priority);
           if (displayComment)
                outString.Append("<!-- 是否是关键角色(兼容门户系统，可以忽略。) (布尔) (bit) -->");
            outString.AppendFormat("<isKey><![CDATA[{0}]]></isKey>", this.IsKey);
            if (displayComment)
                outString.Append("<!-- 分类标识(兼容门户系统，可以忽略。) (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<groupTreeNodeId><![CDATA[{0}]]></groupTreeNodeId>", this.GroupTreeNodeId);
            if (displayComment)
                outString.Append("<!-- 排序编号(字符串) (nvarchar(20)) -->");
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
            outString.Append("</standardRole>");

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
            this.ParentId = element.SelectSingleNode("parentId").InnerText;
            this.StandardOrganizationUnitId = element.SelectSingleNode("standardOrganizationUnitId").InnerText;
            this.Type = Convert.ToInt32(element.SelectSingleNode("type").InnerText);
            this.Priority = Convert.ToInt32(element.SelectSingleNode("priority").InnerText);
            this.IsKey = Convert.ToBoolean(element.SelectSingleNode("isKey").InnerText);
            this.GroupTreeNodeId = element.SelectSingleNode("groupTreeNodeId").InnerText;
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;
            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
