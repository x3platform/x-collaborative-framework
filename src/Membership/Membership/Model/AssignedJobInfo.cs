namespace X3Platform.Membership.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    /// <summary>岗位信息</summary>
    public class AssignedJobInfo : IAssignedJobInfo
    {
        #region 构造函数:AssignedJobInfo()
        /// <summary>默认构造函数</summary>
        public AssignedJobInfo() { }
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

        #region 属性:Job
        private IJobInfo m_Job = null;

        /// <summary>职位信息</summary>
        public IJobInfo Job
        {
            get
            {
                if (this.m_Job == null && !string.IsNullOrEmpty(this.JobId))
                {
                    this.m_Job = MembershipManagement.Instance.JobService[this.JobId];
                }

                return m_Job;
            }
        }
        #endregion

        #region 属性:JobId
        private string m_JobId;

        /// <summary></summary>
        public string JobId
        {
            get { return m_JobId; }
            set { m_JobId = value; }
        }
        #endregion

        #region 属性:JobName
        /// <summary></summary>
        public string JobName
        {
            get { return this.Job == null ? string.Empty : this.Job.Name; }
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
                if (this.m_OrganizationUnit == null && !string.IsNullOrEmpty(this.OrganizationUnitId))
                {
                    this.m_OrganizationUnit = MembershipManagement.Instance.OrganizationUnitService[this.OrganizationUnitId];
                }

                return this.m_OrganizationUnit;
            }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId;

        /// <summary>父节点标识</summary>
        public string ParentId
        {
            get { return m_ParentId; }
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
        private IAssignedJobInfo m_Parent = null;

        /// <summary>父级对象</summary>
        public IAssignedJobInfo Parent
        {
            get
            {
                //
                // ParentId = "00000000-0000-0000-0000-000000000000" 表示父节点为空(系统保留)
                // 所以为避免错误, 当ParentId = "00000000-0000-0000-0000-000000000000", 直接返回null.
                //
                if (this.ParentId == "00000000-0000-0000-0000-000000000000")
                {
                    return null;
                }

                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = MembershipManagement.Instance.AssignedJobService[this.ParentId];
                }

                return m_Parent;
            }
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
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description;

        /// <summary>描述</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:JobGradeDisplayName
        /// <summary>职级显示名称</summary>
        public string JobGradeName
        {
            get { return this.JobGrade == null ? string.Empty : this.JobGrade.DisplayName; }
        }
        #endregion

        #region 属性:JobGradeId
        private string m_JobGradeId;

        /// <summary>职级标识</summary>
        public string JobGradeId
        {
            get { return m_JobGradeId; }
            set { m_JobGradeId = value; }
        }
        #endregion

        #region 属性:JobGrade
        private IJobGradeInfo m_JobGrade = null;

        /// <summary>职级</summary>
        public IJobGradeInfo JobGrade
        {
            get
            {
                if (m_JobGrade == null && !string.IsNullOrEmpty(this.JobGradeId))
                {
                    m_JobGrade = MembershipManagement.Instance.JobGradeService[this.JobGradeId];
                }

                return m_JobGrade;
            }
        }
        #endregion

        #region 属性:RoleId
        private string m_RoleId;

        /// <summary>所属角色标识</summary>
        public string RoleId
        {
            get { return this.m_RoleId; }
            set { this.m_RoleId = value; }
        }
        #endregion

        #region 属性:RoleGlobalName
        /// <summary>所属角色全局名称</summary>
        public string RoleGlobalName
        {
            get { return this.Role == null ? string.Empty : this.Role.GlobalName; }
        }
        #endregion

        #region 属性:Role
        private IRoleInfo m_Role = null;

        /// <summary>所属角色信息</summary>
        public IRoleInfo Role
        {
            get
            {
                if (this.m_Role == null && !string.IsNullOrEmpty(this.RoleId))
                {
                    this.m_Role = MembershipManagement.Instance.RoleService[this.RoleId];
                }

                return this.m_Role;
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
            get { return "AssignedJob"; }
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

            if (displayComment)
                outString.Append("<!-- 岗位对象 -->");
            outString.Append("<assignedJob>");
            if (displayComment)
                outString.Append("<!-- 岗位标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 所属职位标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<jobId><![CDATA[{0}]]></jobId>", this.JobId);
            if (displayComment)
                outString.Append("<!-- 所属组织标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<organizationId><![CDATA[{0}]]></organizationId>", this.OrganizationUnitId);
            if (displayComment)
                outString.Append("<!-- 岗位编号 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 岗位名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</assignedJob>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>根据Xml元素加载对象</summary>
        /// <param name="element">Xml元素</param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.SelectSingleNode("id").InnerText;
            this.JobId = element.SelectSingleNode("jobId").InnerText;
            this.OrganizationUnitId = element.SelectSingleNode("organizationId").InnerText;

            if (element.SelectSingleNode("code") != null)
            {
                this.Code = element.SelectSingleNode("code").InnerText;
            }

            this.Name = element.SelectSingleNode("name").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
