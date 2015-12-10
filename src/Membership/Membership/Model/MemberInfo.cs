namespace X3Platform.Membership.Model
{
    #region Copyright & Author
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Configuration;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>成员</summary>
    [Serializable]
    public class MemberInfo : IMemberInfo
    {
        /// <summary></summary>
        public MemberInfo()
        {
        }

        /// <summary></summary>
        public MemberInfo(string id, string accountId)
            : this()
        {
            this.Id = id;
            this.AccountId = accountId;
        }

        // -------------------------------------------------------
        // 扩展
        // -------------------------------------------------------

        #region 属性:ExtensionInformation
        private IExtensionInformation m_ExtensionInformation = null;

        /// <summary>扩展</summary>
        public IExtensionInformation ExtensionInformation
        {
            get
            {
                if (this.m_ExtensionInformation == null && !string.IsNullOrEmpty(this.AccountId))
                {
                    // 创建对象构建器(Spring.NET)
                    string springObjectFile = MembershipConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

                    SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

                    Dictionary<string, object> args = new Dictionary<string, object>();

                    args.Add("AccountId", this.AccountId);

                    string extensionInformationName = "X3Platform.Membership.Model.MemberInfo.ExtensionInformation";

                    this.m_ExtensionInformation = objectBuilder.GetObject<IExtensionInformation>(extensionInformationName);

                    this.m_ExtensionInformation.Load(args);
                }

                return this.m_ExtensionInformation;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 属性, 可存放扩展属性, 临时属性
        //
        // 可添加的属性 例如: AccountId, DeaultOrganizationId, DeaultRoleId, CorporationId, DepartmentId
        // -------------------------------------------------------

        #region 属性:Properties
        private Dictionary<string, object> m_Properties = new Dictionary<string, object>();

        /// <summary>属性</summary>
        public Dictionary<string, object> Properties
        {
            get { return m_Properties; }
        }
        #endregion

        // -------------------------------------------------------
        // 具体属性
        // -------------------------------------------------------

        #region 属性:Id
        private string m_Id;

        /// <summary>用户标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:FullName
        private string m_FullName = string.Empty;

        /// <summary>姓名</summary>
        public string FullName
        {
            get { return this.Account == null ? m_FullName : this.Account.Name; }

            set
            {
                if (this.Account == null)
                {
                    m_FullName = value;
                }
                else
                {
                    this.Account.DisplayName = value;
                }
            }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>帐号</summary>
        public IAccountInfo Account
        {
            get
            {
                if (m_Account == null)
                {
                    m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return m_Account;
            }

            set { m_Account = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>帐户标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary>帐号名称</summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region 属性:Corporation
        private IOrganizationUnitInfo m_Corporation = null;

        /// <summary>公司</summary>
        public IOrganizationUnitInfo Corporation
        {
            get
            {
                if (m_Corporation == null && !string.IsNullOrEmpty(m_CorporationId))
                {
                    m_Corporation = MembershipManagement.Instance.OrganizationUnitService[m_CorporationId];
                }

                return m_Corporation;
            }
        }
        #endregion

        #region 属性:CorporationId
        private string m_CorporationId = null;

        /// <summary>公司标识</summary>
        public string CorporationId
        {
            get { return m_CorporationId; }
            set { m_CorporationId = value; }
        }
        #endregion

        #region 属性:CorporationName
        /// <summary>公司名称</summary>
        public string CorporationName
        {
            get { return this.Corporation == null ? string.Empty : this.Corporation.Name; }
        }
        #endregion

        #region 属性:DepartmentId
        private string m_DepartmentId = null;

        /// <summary>部门标识</summary>
        public string DepartmentId
        {
            get { return m_DepartmentId; }
            set { m_DepartmentId = value; }
        }
        #endregion

        #region 属性:DepartmentName
        /// <summary>一级部门名称</summary>
        public string DepartmentName
        {
            get { return this.Department == null ? string.Empty : this.Department.Name; }
        }
        #endregion

        #region 属性:Department
        private IOrganizationUnitInfo m_Department = null;

        /// <summary>一级部门</summary>
        public IOrganizationUnitInfo Department
        {
            get
            {
                if (m_Department == null && !string.IsNullOrEmpty(m_DepartmentId))
                {
                    m_Department = MembershipManagement.Instance.OrganizationUnitService[m_DepartmentId];
                }

                return m_Department;
            }
        }
        #endregion

        #region 属性:Department2Id
        private string m_Department2Id = null;

        /// <summary>二级部门标识</summary>
        public string Department2Id
        {
            get { return m_Department2Id; }
            set { m_Department2Id = value; }
        }
        #endregion

        #region 属性:Department2Name
        /// <summary>二级部门名称</summary>
        public string Department2Name
        {
            get { return this.Department2 == null ? string.Empty : this.Department2.Name; }
        }
        #endregion

        #region 属性:Department2
        private IOrganizationUnitInfo m_Department2 = null;

        /// <summary>二级部门</summary>
        public IOrganizationUnitInfo Department2
        {
            get
            {
                if (m_Department2 == null && !string.IsNullOrEmpty(this.Department2Id))
                {
                    m_Department2 = MembershipManagement.Instance.OrganizationUnitService[this.Department2Id];
                }

                return m_Department2;
            }
        }
        #endregion

        #region 属性:Department3Id
        private string m_Department3Id = null;

        /// <summary>三级部门标识</summary>
        public string Department3Id
        {
            get { return m_Department3Id; }
            set { m_Department3Id = value; }
        }
        #endregion

        #region 属性:Department3Name
        /// <summary>三级部门名称</summary>
        public string Department3Name
        {
            get { return this.Department3 == null ? string.Empty : this.Department3.Name; }
        }
        #endregion

        #region 属性:Department3
        private IOrganizationUnitInfo m_Department3 = null;

        /// <summary>三级部门</summary>
        public IOrganizationUnitInfo Department3
        {
            get
            {
                if (m_Department3 == null && !string.IsNullOrEmpty(this.Department3Id))
                {
                    m_Department3 = MembershipManagement.Instance.OrganizationUnitService[this.Department3Id];
                }

                return m_Department3;
            }
        }
        #endregion

        #region 属性:OrganizationUnitId
        private string m_OrganizationUnitId = null;

        /// <summary>默认的组织单位标识</summary>
        public string OrganizationUnitId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_OrganizationUnitId))
                {
                    this.m_OrganizationUnitId = MembershipConfigurationView.Instance.DefaultOrganizationUnitId;
                }

                return m_OrganizationUnitId;
            }
            set { m_OrganizationUnitId = value; }
        }
        #endregion

        #region 属性:OrganizationUnit
        private IOrganizationUnitInfo m_OrganizationUnit = null;

        /// <summary>默认的组织单位</summary>
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

        #region 属性:OrganizationPath
        private string m_OrganizationPath = string.Empty;

        /// <summary>默认的组织路径</summary>
        public string OrganizationPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_OrganizationPath) && !string.IsNullOrEmpty(this.CorporationId))
                {
                    this.m_OrganizationPath = this.CorporationName;

                    if (this.Department != null)
                    {
                        this.m_OrganizationPath += "\\" + this.Department.Name;
                    }

                    if (this.Department2 != null)
                    {
                        this.m_OrganizationPath += "\\" + this.Department2.Name;
                    }

                    if (this.Department3 != null)
                    {
                        this.m_OrganizationPath += "\\" + this.Department3.Name;
                    }
                }

                return m_OrganizationPath;
            }
        }
        #endregion

        #region 属性:RoleId
        private string m_RoleId = null;

        /// <summary>默认的角色标识</summary>
        public string RoleId
        {
            get { return m_RoleId; }
            set { m_RoleId = value; }
        }
        #endregion

        #region 属性:RoleName
        /// <summary>默认角色名称</summary>
        public string RoleName
        {
            get { return this.Role == null ? string.Empty : this.Role.Name; }
        }
        #endregion

        #region 属性:Role
        private IRoleInfo m_Role = null;

        /// <summary>默认的角色</summary>
        public IRoleInfo Role
        {
            get
            {
                if (m_Role == null && !string.IsNullOrEmpty(this.RoleId))
                {
                    m_Role = MembershipManagement.Instance.RoleService[this.RoleId];
                }

                return m_Role;
            }
        }
        #endregion

        #region 属性:Headship
        private string m_Headship = string.Empty;

        /// <summary>职务 | 头衔</summary>
        public string Headship
        {
            get { return m_Headship; }
            set { m_Headship = value; }
        }
        #endregion

        #region 属性:Sex
        private string m_Sex;

        /// <summary>性别</summary>
        public string Sex
        {
            get
            {
                if (string.IsNullOrEmpty(m_Sex))
                {
                    m_Sex = "男";
                }

                return m_Sex;
            }
            set { m_Sex = value; }
        }
        #endregion

        #region 属性:Birthday
        private DateTime m_Birthday;

        /// <summary>生日</summary>
        public DateTime Birthday
        {
            get
            {
                if (m_Birthday == DateTime.MinValue)
                {
                    m_Birthday = new DateTime(1900, 1, 1);
                }

                return m_Birthday;
            }
            set { m_Birthday = value; }
        }
        #endregion

        #region 属性:GraduationDate
        private DateTime m_GraduationDate;

        /// <summary>毕业时间</summary>
        public DateTime GraduationDate
        {
            get
            {
                if (this.m_GraduationDate == DateTime.MinValue)
                {
                    this.m_GraduationDate = new DateTime(1900, 1, 1);
                }

                return this.m_GraduationDate;
            }
            set { this.m_GraduationDate = value; }
        }
        #endregion

        #region 属性:EntryDate
        private DateTime m_EntryDate;

        /// <summary>入职时间</summary>
        public DateTime EntryDate
        {
            get
            {
                if (m_EntryDate == DateTime.MinValue)
                {
                    m_EntryDate = new DateTime(1900, 1, 1);
                }

                return m_EntryDate;
            }
            set { m_EntryDate = value; }
        }
        #endregion

        #region 属性:PromotionDate
        private DateTime m_PromotionDate;

        /// <summary>最近一次晋升时间，如果刚入职则等于入职时间</summary>
        public DateTime PromotionDate
        {
            get
            {
                if (this.m_PromotionDate == DateTime.MinValue)
                {
                    this.m_PromotionDate = this.EntryDate;
                }

                return this.m_PromotionDate;
            }
            set { this.m_PromotionDate = value; }
        }
        #endregion

        #region 属性:Hometown
        private string m_Hometown = string.Empty;

        /// <summary>出生地</summary>
        public string Hometown
        {
            get { return m_Hometown; }
            set { m_Hometown = value; }
        }
        #endregion

        #region 属性:City
        private string m_City;

        /// <summary>居住城市</summary>
        public string City
        {
            get { return m_City; }
            set { m_City = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 职位
        // -------------------------------------------------------

        #region 属性:JobId
        private string m_JobId = string.Empty;

        /// <summary>职位</summary>
        public string JobId
        {
            get { return m_JobId; }
            set { m_JobId = value; }
        }
        #endregion

        #region 属性:JobName
        /// <summary>职位名称</summary>
        public string JobName
        {
            get { return this.Job == null ? string.Empty : this.Job.Name; }
        }
        #endregion

        #region 属性:Job
        private IJobInfo m_Job = null;

        /// <summary>职位</summary>
        public IJobInfo Job
        {
            get
            {
                if (m_Job == null && !string.IsNullOrEmpty(this.JobId))
                {
                    m_Job = MembershipManagement.Instance.JobService[this.JobId];
                }

                return m_Job;
            }
        }
        #endregion

        #region 属性:AssignedJobId
        private string m_AssignedJobId = string.Empty;

        /// <summary>岗位</summary>
        public string AssignedJobId
        {
            get { return m_AssignedJobId; }
            set { m_AssignedJobId = value; }
        }
        #endregion

        #region 属性:AssignedJobName
        /// <summary>岗位名称</summary>
        public string AssignedJobName
        {
            get { return this.AssignedJob == null ? string.Empty : this.AssignedJob.Name; }
        }
        #endregion

        #region 属性:AssignedJob
        private IAssignedJobInfo m_AssignedJob = null;

        /// <summary>岗位</summary>
        public IAssignedJobInfo AssignedJob
        {
            get
            {
                if (m_AssignedJob == null && !string.IsNullOrEmpty(this.AssignedJobId))
                {
                    m_AssignedJob = MembershipManagement.Instance.AssignedJobService[this.AssignedJobId];
                }

                return m_AssignedJob;
            }
        }
        #endregion

        #region 属性:JobGradeDisplayName
        private string m_JobGradeDisplayName = string.Empty;

        /// <summary>职级显示名称</summary>
        public string JobGradeDisplayName
        {
            get { return m_JobGradeDisplayName; }
            set { m_JobGradeDisplayName = value; }
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
                    this.m_JobGrade = MembershipManagement.Instance.JobGradeService[this.JobGradeId];
                }

                return m_JobGrade;
            }
        }
        #endregion

        #region 属性:PartTimeJobs
        private IList<IAssignedJobInfo> m_PartTimeJobs = null;

        /// <summary>兼职</summary>
        public IList<IAssignedJobInfo> PartTimeJobs
        {
            get
            {
                if (this.m_PartTimeJobs == null && !string.IsNullOrEmpty(this.AccountId))
                {
                    this.m_PartTimeJobs = MembershipManagement.Instance.AssignedJobService.FindAllPartTimeJobsByAccountId(this.Id);
                }

                return this.m_PartTimeJobs;
            }
        }
        #endregion

        #region 属性:PartTimeJobsView
        /// <summary>兼职视图</summary>
        public string PartTimeJobsView
        {
            get
            {
                string outString = string.Empty;

                foreach (IAssignedJobInfo partTimeJob in this.PartTimeJobs)
                {
                    outString += partTimeJob.Name + ";";
                }

                return outString.TrimEnd(new char[] { ';' });
            }
        }
        #endregion

        #region 属性:PartTimeJobsText
        private string m_PartTimeJobsText = string.Empty;

        /// <summary>兼职文本数据</summary>
        public string PartTimeJobsText
        {
            get
            {
                if (this.PartTimeJobs != null && string.IsNullOrEmpty(this.m_PartTimeJobsText) && this.PartTimeJobs.Count > 0)
                {
                    foreach (IAssignedJobInfo item in this.PartTimeJobs)
                    {
                        this.m_PartTimeJobsText += string.Format("assignedJob#{0}#{1},", item.Id, item.Name);
                    }

                    this.m_PartTimeJobsText = this.m_PartTimeJobsText.TrimEnd(',');
                }

                return this.m_PartTimeJobsText;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 职级
        // -------------------------------------------------------

        #region 属性:JobGradeGroup
        private IGroupInfo m_JobGradeGroup = null;

        /// <summary>职级群组</summary>
        public IGroupInfo JobGradeGroup
        {
            get
            {
                if (m_JobGradeGroup == null && !string.IsNullOrEmpty(m_JobGradeGroupId))
                {
                    m_JobGradeGroup = MembershipManagement.Instance.GroupService[m_JobGradeGroupId];
                }

                return m_JobGradeGroup;
            }
        }
        #endregion

        #region 属性:JobGradeGroupId
        private string m_JobGradeGroupId;

        /// <summary>职级群组的值</summary>
        public string JobGradeGroupId
        {
            get { return m_JobGradeGroupId; }
            set { m_JobGradeGroupId = value; }
        }
        #endregion

        #region 属性:JobGradeGroupName
        /// <summary>职级群组的名称</summary>
        public string JobGradeGroupName
        {
            get { return this.JobGradeGroup == null ? string.Empty : this.JobGradeGroup.Name; }
        }
        #endregion

        #region 属性:JobGradeValue
        private int m_JobGradeValue = -1;

        /// <summary>职级值</summary>
        public int JobGradeValue
        {
            get
            {
                if (this.m_JobGradeValue == -1)
                {
                    this.m_JobGradeValue = (this.JobGrade == null) ? 0 : this.JobGrade.Value;
                }

                return this.m_JobGradeValue;
            }
        }
        #endregion

        #region 私有函数:ParseJobGradeValue(string groupId)
        /// <summary>解析职级群组的值</summary>
        /// <returns></returns>
        private int ParseJobGradeValue(string groupId)
        {
            string key = string.Format("G{0}", groupId);

            MembershipConfiguration configuration = MembershipConfigurationView.Instance.Configuration;

            if (configuration.Keys["JobGradeMapping"] == null)
                return 0;

            Hashtable table = JsonHelper.ToHashtable(configuration.Keys["JobGradeMapping"].Value);

            foreach (DictionaryEntry entry in table)
            {
                if (entry.Key.ToString() == key)
                {
                    try
                    {
                        return Convert.ToInt32(entry.Value.ToString());
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 联系方式
        // -------------------------------------------------------

        #region 属性:Mobile
        private string m_Mobile = string.Empty;

        /// <summary>手机号码</summary>
        public string Mobile
        {
            get { return m_Mobile; }
            set { m_Mobile = value; }
        }
        #endregion

        #region 属性:Telephone
        private string m_Telephone = string.Empty;

        /// <summary>办公号码</summary>
        public string Telephone
        {
            get { return m_Telephone; }
            set { m_Telephone = value; }
        }
        #endregion

        #region 属性:Email
        private string m_Email = string.Empty;

        /// <summary>邮箱</summary>
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
        #endregion

        #region 属性:QQ
        private string m_QQ = string.Empty;

        /// <summary></summary>
        public string QQ
        {
            get { return m_QQ; }
            set { m_QQ = value; }
        }
        #endregion

        #region 属性:MSN
        private string m_MSN = string.Empty;

        /// <summary></summary>
        public string MSN
        {
            get { return m_MSN; }
            set { m_MSN = value; }
        }
        #endregion

        #region 属性:Rtx
        private string m_Rtx = string.Empty;

        /// <summary>RTX</summary>
        public string Rtx
        {
            get { return m_Rtx; }
            set { m_Rtx = value; }
        }
        #endregion

        #region 属性:Office
        private string m_Office;

        /// <summary>办公地址</summary>
        public string Office
        {
            get { return m_Office; }
            set { m_Office = value; }
        }
        #endregion

        #region 属性:PostCode
        private string m_PostCode = string.Empty;

        /// <summary></summary>
        public string PostCode
        {
            get { return m_PostCode; }
            set { m_PostCode = value; }
        }
        #endregion

        #region 属性:Address
        private string m_Address = string.Empty;

        /// <summary></summary>
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url = string.Empty;

        /// <summary>个人主页</summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
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
                outString.Append("<!-- 用户对象 -->");
            outString.Append("<user>");
            if (displayComment)
                outString.Append("<!-- 用户标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 用户编号 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Account == null ? string.Empty : this.Account.Code);
            if (displayComment)
                outString.Append("<!-- 登录名 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<loginName><![CDATA[{0}]]></loginName>", this.Account == null ? string.Empty : this.Account.LoginName);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Account == null ? string.Empty : this.Account.Name);
            if (displayComment)
                outString.Append("<!-- 全局名称 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<globalName><![CDATA[{0}]]></globalName>", this.Account == null ? string.Empty : this.Account.GlobalName);
            if (displayComment)
                outString.Append("<!-- 显示名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<alias><![CDATA[{0}]]></alias>", this.Account == null ? string.Empty : this.Account.DisplayName);
            if (displayComment)
                outString.Append("<!-- 拼音 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<pinyin><![CDATA[{0}]]></pinyin>", this.Account == null ? string.Empty : this.Account.PinYin);
            if (displayComment)
                outString.Append("<!-- 默认所属公司标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<corporationId><![CDATA[{0}]]></corporationId>", this.CorporationId);
            if (displayComment)
                outString.Append("<!-- 默认所属部门标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<departmentId><![CDATA[{0}]]></departmentId>", this.DepartmentId);
            if (displayComment)
                outString.Append("<!-- 默认所属二级部门标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<department2Id><![CDATA[{0}]]></department2Id>", this.Department2Id);
            if (displayComment)
                outString.Append("<!-- 默认所属三级部门标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<department3Id><![CDATA[{0}]]></department3Id>", this.Department3Id);
            if (displayComment)
                outString.Append("<!-- 默认所属最末级组织标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<organizationUnitId><![CDATA[{0}]]></organizationUnitId>", this.OrganizationUnitId);
            if (displayComment)
                outString.Append("<!-- 默认所属角色 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<roleId><![CDATA[{0}]]></roleId>", this.RoleId);
            if (displayComment)
                outString.Append("<!-- 岗位头衔/职务 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<headship><![CDATA[{0}]]></headship>", this.Headship);
            if (displayComment)
                outString.Append("<!-- 性别 (字符串) (nvarchar(4)) -->");
            outString.AppendFormat("<sex><![CDATA[{0}]]></sex>", this.Sex);
            if (displayComment)
                outString.Append("<!-- 生日 (时间) (datetime) -->");
            outString.AppendFormat("<birthday><![CDATA[{0}]]></birthday>", this.Birthday);
            if (displayComment)
                outString.Append("<!-- 毕业时间 (时间) (datetime) -->");
            outString.AppendFormat("<graduateDate><![CDATA[{0}]]></graduateDate>", this.GraduationDate);
            if (displayComment)
                outString.Append("<!-- 入职时间 (时间) (datetime) -->");
            outString.AppendFormat("<entryDate><![CDATA[{0}]]></entryDate>", this.EntryDate);
            if (displayComment)
                outString.Append("<!-- 晋升时间 (时间) (datetime) -->");
            outString.AppendFormat("<promotionDate><![CDATA[{0}]]></promotionDate>", this.PromotionDate);
            if (displayComment)
                outString.Append("<!-- 移动电话 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<mobile><![CDATA[{0}]]></mobile>", this.Mobile);
            if (displayComment)
                outString.Append("<!-- 座机 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<telephone><![CDATA[{0}]]></telephone>", this.Telephone);
            if (displayComment)
                outString.Append("<!-- 电子邮件 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<email><![CDATA[{0}]]></email>", this.Email);
            if (displayComment)
                outString.Append("<!-- RTX (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<rtx><![CDATA[{0}]]></rtx>", this.Rtx);
            if (displayComment)
                outString.Append("<!-- 所属职位标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<jobId><![CDATA[{0}]]></jobId>", this.JobId);
            if (displayComment)
                outString.Append("<!-- 所属职级标识 (已作废，可以忽略。) (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<jobGradeId><![CDATA[{0}]]></jobGradeId>", this.JobGradeId);
            if (displayComment)
                outString.Append("<!-- 职级显示信息 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<jobGrade><![CDATA[{0}]]></jobGrade>", this.JobGradeDisplayName);
            if (displayComment)
                outString.Append("<!-- 所属岗位标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<assignedJobId><![CDATA[{0}]]></assignedJobId>", this.AssignedJobId);
            if (displayComment)
                outString.Append("<!-- 兼职信息 -->");
            outString.Append("<partTimeJobs>");
            foreach (IAssignedJobInfo partTimeJob in this.PartTimeJobs)
            {
                if (displayComment)
                    outString.Append("<!-- 所属兼职标识 (字符串) (nvarchar(36)) -->");
                outString.AppendFormat("<partTimeJobId><![CDATA[{0}]]></partTimeJobId>", partTimeJob.Id);
            }
            outString.Append("</partTimeJobs>");

            if (displayComment)
                outString.Append("<!-- 标准角色 -->");
            outString.Append("<standardRoles>");
            if (this.Account != null)
            {
                IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByAccountId(this.Account.Id);

                foreach (IRoleInfo role in roles)
                {
                    if (!string.IsNullOrEmpty(role.StandardRoleId) && !string.IsNullOrEmpty(role.StandardRoleId) && role.StandardRole != null)
                    {
                        if (displayComment)
                            outString.Append("<!-- 所属标准角色标识 (字符串) (nvarchar(36)) -->");

                        outString.AppendFormat("<standardRole organizationId=\"{0}\" standardRoleType=\"{1}\" standardRoleId=\"{2}\" />",
                            role.OrganizationUnitId,
                            role.StandardRole.Type,
                            role.StandardRole.Id);
                    }
                }
            }
            outString.Append("</standardRoles>");

            if (displayComment)
                outString.Append("<!-- 关联对象-->");
            outString.Append("<relationObjects>");
            if (this.Account != null)
            { //foreach (IOrganizationUnitInfo organization in this.Account.OrganizationUnits)
                //{
                //    outString.AppendFormat("<relationObject id=\"{0}\" type=\"OrganizationUnit\" />", organization.Id);
                //}
                foreach (IAccountRoleRelationInfo relation in this.Account.RoleRelations)
                {
                    if (displayComment)
                        outString.Append("<!-- 所属角色标识 (字符串) (nvarchar(36)) -->");

                    outString.AppendFormat("<relationObject id=\"{0}\" type=\"Role\" />", relation.RoleId);
                }
                foreach (IAccountGroupRelationInfo relation in this.Account.GroupRelations)
                {
                    if (displayComment)
                        outString.Append("<!-- 所属群组标识(兼容门户系统，可以忽略。) (字符串) (nvarchar(36)) -->");

                    outString.AppendFormat("<relationObject id=\"{0}\" type=\"Group\" />", relation.GroupId);
                }
            }
            outString.Append("</relationObjects>");
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Account == null ? 0 : this.Account.Status);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<modifiedDate><![CDATA[{0}]]></modifiedDate>", this.ModifiedDate);
            outString.Append("</user>");

            return outString.ToString();
        }

        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>根据Xml元素加载对象</summary>
        /// <param name="element">Xml元素</param>
        public virtual void Deserialize(XmlElement element)
        {
            if (this.Account == null)
            {
                m_Account = new AccountInfo();
            }

            this.Account.Deserialize(element);

            this.Id = this.AccountId = this.Account.Id = element.SelectSingleNode("id").InnerText;

            this.FullName = element.SelectSingleNode("name").InnerText;
            this.CorporationId = element.SelectSingleNode("corporationId").InnerText;
            this.DepartmentId = element.SelectSingleNode("departmentId").InnerText;

            if (element.SelectSingleNode("department2Id") != null)
            {
                this.Department2Id = element.SelectSingleNode("department2Id").InnerText;
            }

            if (element.SelectSingleNode("department3Id") != null)
            {
                this.Department3Id = element.SelectSingleNode("department3Id").InnerText;
            }

            this.OrganizationUnitId = element.SelectSingleNode("organizationId").InnerText;

            if (element.SelectSingleNode("roleId") != null)
            {
                this.RoleId = element.SelectSingleNode("roleId").InnerText;
            }

            if (element.SelectSingleNode("headship") != null)
            {
                this.Headship = element.SelectSingleNode("headship").InnerText;
            }

            if (element.SelectSingleNode("sex") != null)
            {
                this.Sex = element.SelectSingleNode("sex").InnerText;
            }

            if (element.SelectSingleNode("birthday") != null)
            {
                this.Birthday = Convert.ToDateTime(element.SelectSingleNode("birthday").InnerText);
            }

            if (element.SelectSingleNode("graduateDate") != null)
            {
                this.GraduationDate = Convert.ToDateTime(element.SelectSingleNode("graduateDate").InnerText);
            }

            if (element.SelectSingleNode("entryDate") != null)
            {
                this.EntryDate = Convert.ToDateTime(element.SelectSingleNode("entryDate").InnerText);
            }

            if (element.SelectSingleNode("promotionDate") != null)
            {
                this.PromotionDate = Convert.ToDateTime(element.SelectSingleNode("promotionDate").InnerText);
            }

            this.Mobile = element.SelectSingleNode("mobile").InnerText;

            this.Telephone = element.SelectSingleNode("telephone").InnerText;

            if (element.SelectSingleNode("email") != null)
            {
                this.Email = element.SelectSingleNode("email").InnerText;
            }

            if (element.SelectSingleNode("rtx") != null)
            {
                this.Rtx = element.SelectSingleNode("rtx").InnerText;
            }

            this.AssignedJobId = element.SelectSingleNode("assignedJobId").InnerText;

            XmlNodeList nodes = null;

            nodes = element.SelectNodes("partTimeJobs/partTimeJobId");

            foreach (XmlNode node in nodes)
            {
                IAssignedJobInfo partTimeJob = MembershipManagement.Instance.AssignedJobService[node.InnerText];

                if (partTimeJob != null)
                {
                    this.PartTimeJobs.Add(partTimeJob);
                }
            }

            if (element.SelectSingleNode("jobId") != null)
            {
                this.JobId = element.SelectSingleNode("jobId").InnerText;
            }

            if (element.SelectSingleNode("jobGrade") != null)
            {
                this.JobGradeDisplayName = element.SelectSingleNode("jobGrade").InnerText;
            }

            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
