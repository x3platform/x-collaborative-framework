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

    /// <summary>��Ա</summary>
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
        // ��չ
        // -------------------------------------------------------

        #region 属性:ExtensionInformation
        private IExtensionInformation m_ExtensionInformation = null;

        /// <summary>��չ</summary>
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
        // ����, �ɴ�����չ����, ��ʱ����
        //
        // �����ӵ����� 属性: AccountId, DeaultOrganizationId, DeaultRoleId, CorporationId, DepartmentId
        // -------------------------------------------------------

        #region 属性:Properties
        private Dictionary<string, object> m_Properties = new Dictionary<string, object>();

        /// <summary>����</summary>
        public Dictionary<string, object> Properties
        {
            get { return m_Properties; }
        }
        #endregion

        // -------------------------------------------------------
        // ��������
        // -------------------------------------------------------

        #region 属性:Id
        private string m_Id;

        /// <summary>�û���ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:FullName
        private string m_FullName = string.Empty;

        /// <summary>����</summary>
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

        /// <summary>�ʺ�</summary>
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

        /// <summary>�ʻ���ʶ</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary>�ʺ�����</summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region 属性:Corporation
        private IOrganizationInfo m_Corporation = null;

        /// <summary>��˾</summary>
        public IOrganizationInfo Corporation
        {
            get
            {
                if (m_Corporation == null && !string.IsNullOrEmpty(m_CorporationId))
                {
                    m_Corporation = MembershipManagement.Instance.OrganizationService[m_CorporationId];
                }

                return m_Corporation;
            }
        }
        #endregion

        #region 属性:CorporationId
        private string m_CorporationId = null;

        /// <summary>��˾��ʶ</summary>
        public string CorporationId
        {
            get { return m_CorporationId; }
            set { m_CorporationId = value; }
        }
        #endregion

        #region 属性:CorporationName
        /// <summary>��˾����</summary>
        public string CorporationName
        {
            get { return this.Corporation == null ? string.Empty : this.Corporation.Name; }
        }
        #endregion

        #region 属性:DepartmentId
        private string m_DepartmentId = null;

        /// <summary>���ű�ʶ</summary>
        public string DepartmentId
        {
            get { return m_DepartmentId; }
            set { m_DepartmentId = value; }
        }
        #endregion

        #region 属性:DepartmentName
        /// <summary>һ����������</summary>
        public string DepartmentName
        {
            get { return this.Department == null ? string.Empty : this.Department.Name; }
        }
        #endregion

        #region 属性:Department
        private IOrganizationInfo m_Department = null;

        /// <summary>һ������</summary>
        public IOrganizationInfo Department
        {
            get
            {
                if (m_Department == null && !string.IsNullOrEmpty(m_DepartmentId))
                {
                    m_Department = MembershipManagement.Instance.OrganizationService[m_DepartmentId];
                }

                return m_Department;
            }
        }
        #endregion

        #region 属性:Department2Id
        private string m_Department2Id = null;

        /// <summary>�������ű�ʶ</summary>
        public string Department2Id
        {
            get { return m_Department2Id; }
            set { m_Department2Id = value; }
        }
        #endregion

        #region 属性:Department2Name
        /// <summary>������������</summary>
        public string Department2Name
        {
            get { return this.Department2 == null ? string.Empty : this.Department2.Name; }
        }
        #endregion

        #region 属性:Department2
        private IOrganizationInfo m_Department2 = null;

        /// <summary>��������</summary>
        public IOrganizationInfo Department2
        {
            get
            {
                if (m_Department2 == null && !string.IsNullOrEmpty(this.Department2Id))
                {
                    m_Department2 = MembershipManagement.Instance.OrganizationService[this.Department2Id];
                }

                return m_Department2;
            }
        }
        #endregion

        #region 属性:Department3Id
        private string m_Department3Id = null;

        /// <summary>�������ű�ʶ</summary>
        public string Department3Id
        {
            get { return m_Department3Id; }
            set { m_Department3Id = value; }
        }
        #endregion

        #region 属性:Department3Name
        /// <summary>������������</summary>
        public string Department3Name
        {
            get { return this.Department3 == null ? string.Empty : this.Department3.Name; }
        }
        #endregion

        #region 属性:Department3
        private IOrganizationInfo m_Department3 = null;

        /// <summary>��������</summary>
        public IOrganizationInfo Department3
        {
            get
            {
                if (m_Department3 == null && !string.IsNullOrEmpty(this.Department3Id))
                {
                    m_Department3 = MembershipManagement.Instance.OrganizationService[this.Department3Id];
                }

                return m_Department3;
            }
        }
        #endregion

        #region 属性:OrganizationId
        private string m_OrganizationId = null;

        /// <summary>Ĭ�ϵ���֯��λ��ʶ</summary>
        public string OrganizationId
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_OrganizationId))
                {
                    this.m_OrganizationId = MembershipConfigurationView.Instance.DefaultOrganizationId;
                }

                return m_OrganizationId;
            }
            set { m_OrganizationId = value; }
        }
        #endregion

        #region 属性:Organization
        private IOrganizationInfo m_Organization = null;

        /// <summary>Ĭ�ϵ���֯��λ</summary>
        public IOrganizationInfo Organization
        {
            get
            {
                if (m_Organization == null && !string.IsNullOrEmpty(this.OrganizationId))
                {
                    m_Organization = MembershipManagement.Instance.OrganizationService[this.OrganizationId];
                }

                return m_Organization;
            }
        }
        #endregion

        #region 属性:OrganizationPath
        private string m_OrganizationPath = string.Empty;

        /// <summary>Ĭ�ϵ���֯·��</summary>
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

        /// <summary>Ĭ�ϵĽ�ɫ��ʶ</summary>
        public string RoleId
        {
            get { return m_RoleId; }
            set { m_RoleId = value; }
        }
        #endregion

        #region 属性:RoleName
        /// <summary>Ĭ�Ͻ�ɫ����</summary>
        public string RoleName
        {
            get { return this.Role == null ? string.Empty : this.Role.Name; }
        }
        #endregion

        #region 属性:Role
        private IRoleInfo m_Role = null;

        /// <summary>Ĭ�ϵĽ�ɫ</summary>
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

        /// <summary>ְ�� | ͷ��</summary>
        public string Headship
        {
            get { return m_Headship; }
            set { m_Headship = value; }
        }
        #endregion

        #region 属性:Sex
        private string m_Sex;

        /// <summary>�Ա�</summary>
        public string Sex
        {
            get
            {
                if (string.IsNullOrEmpty(m_Sex))
                {
                    m_Sex = "��";
                }

                return m_Sex;
            }
            set { m_Sex = value; }
        }
        #endregion

        #region 属性:Birthday
        private DateTime m_Birthday;

        /// <summary>����</summary>
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

        /// <summary>��ҵʱ��</summary>
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

        /// <summary>��ְʱ��</summary>
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

        /// <summary>����һ�ν���ʱ�䣬��������ְ��������ְʱ��</summary>
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

        /// <summary>������</summary>
        public string Hometown
        {
            get { return m_Hometown; }
            set { m_Hometown = value; }
        }
        #endregion

        #region 属性:City
        private string m_City;

        /// <summary>��ס����</summary>
        public string City
        {
            get { return m_City; }
            set { m_City = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ְλ
        // -------------------------------------------------------

        #region 属性:JobId
        private string m_JobId = string.Empty;

        /// <summary>ְλ</summary>
        public string JobId
        {
            get { return m_JobId; }
            set { m_JobId = value; }
        }
        #endregion

        #region 属性:JobName
        /// <summary>ְλ����</summary>
        public string JobName
        {
            get { return this.Job == null ? string.Empty : this.Job.Name; }
        }
        #endregion

        #region 属性:Job
        private IJobInfo m_Job = null;

        /// <summary>ְλ</summary>
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

        /// <summary>��λ</summary>
        public string AssignedJobId
        {
            get { return m_AssignedJobId; }
            set { m_AssignedJobId = value; }
        }
        #endregion

        #region 属性:AssignedJobName
        /// <summary>��λ����</summary>
        public string AssignedJobName
        {
            get { return this.AssignedJob == null ? string.Empty : this.AssignedJob.Name; }
        }
        #endregion

        #region 属性:AssignedJob
        private IAssignedJobInfo m_AssignedJob = null;

        /// <summary>��λ</summary>
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

        /// <summary>ְ����ʾ����</summary>
        public string JobGradeDisplayName
        {
            get { return m_JobGradeDisplayName; }
            set { m_JobGradeDisplayName = value; }
        }
        #endregion

        #region 属性:JobGradeId
        private string m_JobGradeId;

        /// <summary>ְ����ʶ</summary>
        public string JobGradeId
        {
            get { return m_JobGradeId; }
            set { m_JobGradeId = value; }
        }
        #endregion

        #region 属性:JobGrade
        private IJobGradeInfo m_JobGrade = null;

        /// <summary>ְ��</summary>
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

        /// <summary>��ְ</summary>
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
        /// <summary>��ְ��ͼ</summary>
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

        /// <summary>��ְ�ı�����</summary>
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
        // ְ��
        // -------------------------------------------------------

        #region 属性:JobGradeGroup
        private IGroupInfo m_JobGradeGroup = null;

        /// <summary>ְ��Ⱥ��</summary>
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

        /// <summary>ְ��Ⱥ����ֵ</summary>
        public string JobGradeGroupId
        {
            get { return m_JobGradeGroupId; }
            set { m_JobGradeGroupId = value; }
        }
        #endregion

        #region 属性:JobGradeGroupName
        /// <summary>ְ��Ⱥ��������</summary>
        public string JobGradeGroupName
        {
            get { return this.JobGradeGroup == null ? string.Empty : this.JobGradeGroup.Name; }
        }
        #endregion

        #region 属性:JobGradeValue
        private int m_JobGradeValue = -1;

        /// <summary>ְ��ֵ</summary>
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

        #region ˽�к���:ParseJobGradeValue(string groupId)
        /// <summary>����ְ��Ⱥ����ֵ</summary>
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
        // ��ϵ��ʽ
        // -------------------------------------------------------

        #region 属性:Mobile
        private string m_Mobile = string.Empty;

        /// <summary>�ֻ�����</summary>
        public string Mobile
        {
            get { return m_Mobile; }
            set { m_Mobile = value; }
        }
        #endregion

        #region 属性:Telephone
        private string m_Telephone = string.Empty;

        /// <summary>�칫����</summary>
        public string Telephone
        {
            get { return m_Telephone; }
            set { m_Telephone = value; }
        }
        #endregion

        #region 属性:Email
        private string m_Email = string.Empty;

        /// <summary>����</summary>
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

        /// <summary>�칫��ַ</summary>
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

        /// <summary>������ҳ</summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        #endregion

        #region 属性:FullPath
        private string m_FullPath = null;

        /// <summary>������֯�ܹ�ȫ·��</summary>
        public string FullPath
        {
            get { return m_FullPath; }
            set { m_FullPath = value; }
        }
        #endregion

        #region 属性:DistinguishedName
        private string m_DistinguishedName = null;

        /// <summary>Ψһ����</summary>
        public string DistinguishedName
        {
            get { return m_DistinguishedName; }
            set { m_DistinguishedName = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>�޸�ʱ��</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>����ʱ��</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ʵ�� ISerializedObject ���л�
        // -------------------------------------------------------

        #region 属性:Serializable()
        /// <summary>���ݶ��󵼳�XmlԪ��</summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 属性:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>���ݶ��󵼳�XmlԪ��</summary>
        /// <param name="displayComment">��ʾע��</param>
        /// <param name="displayFriendlyName">��ʾ�Ѻ�����</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            if (displayComment)
                outString.Append("<!-- �û����� -->");
            outString.Append("<user>");
            if (displayComment)
                outString.Append("<!-- �û���ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- �û����� (�ַ���) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Account == null ? string.Empty : this.Account.Code);
            if (displayComment)
                outString.Append("<!-- ��¼�� (�ַ���) (nvarchar(50)) -->");
            outString.AppendFormat("<loginName><![CDATA[{0}]]></loginName>", this.Account == null ? string.Empty : this.Account.LoginName);
            if (displayComment)
                outString.Append("<!-- ���� (�ַ���) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Account == null ? string.Empty : this.Account.Name);
            if (displayComment)
                outString.Append("<!-- ȫ������ (�ַ���) (nvarchar(100)) -->");
            outString.AppendFormat("<globalName><![CDATA[{0}]]></globalName>", this.Account == null ? string.Empty : this.Account.GlobalName);
            if (displayComment)
                outString.Append("<!-- ��ʾ���� (�ַ���) (nvarchar(50)) -->");
            outString.AppendFormat("<alias><![CDATA[{0}]]></alias>", this.Account == null ? string.Empty : this.Account.DisplayName);
            if (displayComment)
                outString.Append("<!-- ƴ�� (�ַ���) (nvarchar(100)) -->");
            outString.AppendFormat("<pinyin><![CDATA[{0}]]></pinyin>", this.Account == null ? string.Empty : this.Account.PinYin);
            if (displayComment)
                outString.Append("<!-- Ĭ��������˾��ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<corporationId><![CDATA[{0}]]></corporationId>", this.CorporationId);
            if (displayComment)
                outString.Append("<!-- Ĭ���������ű�ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<departmentId><![CDATA[{0}]]></departmentId>", this.DepartmentId);
            if (displayComment)
                outString.Append("<!-- Ĭ�������������ű�ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<department2Id><![CDATA[{0}]]></department2Id>", this.Department2Id);
            if (displayComment)
                outString.Append("<!-- Ĭ�������������ű�ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<department3Id><![CDATA[{0}]]></department3Id>", this.Department3Id);
            if (displayComment)
                outString.Append("<!-- Ĭ��������ĩ����֯��ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<organizationId><![CDATA[{0}]]></organizationId>", this.OrganizationId);
            if (displayComment)
                outString.Append("<!-- Ĭ��������ɫ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<roleId><![CDATA[{0}]]></roleId>", this.RoleId);
            if (displayComment)
                outString.Append("<!-- ��λͷ��/ְ�� (�ַ���) (nvarchar(50)) -->");
            outString.AppendFormat("<headship><![CDATA[{0}]]></headship>", this.Headship);
            if (displayComment)
                outString.Append("<!-- �Ա� (�ַ���) (nvarchar(4)) -->");
            outString.AppendFormat("<sex><![CDATA[{0}]]></sex>", this.Sex);
            if (displayComment)
                outString.Append("<!-- ���� (ʱ��) (datetime) -->");
            outString.AppendFormat("<birthday><![CDATA[{0}]]></birthday>", this.Birthday);
            if (displayComment)
                outString.Append("<!-- ��ҵʱ�� (ʱ��) (datetime) -->");
            outString.AppendFormat("<graduateDate><![CDATA[{0}]]></graduateDate>", this.GraduationDate);
            if (displayComment)
                outString.Append("<!-- ��ְʱ�� (ʱ��) (datetime) -->");
            outString.AppendFormat("<entryDate><![CDATA[{0}]]></entryDate>", this.EntryDate);
            if (displayComment)
                outString.Append("<!-- ����ʱ�� (ʱ��) (datetime) -->");
            outString.AppendFormat("<promotionDate><![CDATA[{0}]]></promotionDate>", this.PromotionDate);
            if (displayComment)
                outString.Append("<!-- �ƶ��绰 (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<mobile><![CDATA[{0}]]></mobile>", this.Mobile);
            if (displayComment)
                outString.Append("<!-- ���� (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<telephone><![CDATA[{0}]]></telephone>", this.Telephone);
            if (displayComment)
                outString.Append("<!-- �����ʼ� (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<email><![CDATA[{0}]]></email>", this.Email);
            if (displayComment)
                outString.Append("<!-- RTX (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<rtx><![CDATA[{0}]]></rtx>", this.Rtx);
            if (displayComment)
                outString.Append("<!-- ����ְλ��ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<jobId><![CDATA[{0}]]></jobId>", this.JobId);
            if (displayComment)
                outString.Append("<!-- ����ְ����ʶ (�����ϣ����Ժ��ԡ�) (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<jobGradeId><![CDATA[{0}]]></jobGradeId>", this.JobGradeId);
            if (displayComment)
                outString.Append("<!-- ְ����ʾ��Ϣ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<jobGrade><![CDATA[{0}]]></jobGrade>", this.JobGradeDisplayName);
            if (displayComment)
                outString.Append("<!-- ������λ��ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<assignedJobId><![CDATA[{0}]]></assignedJobId>", this.AssignedJobId);
            if (displayComment)
                outString.Append("<!-- ��ְ��Ϣ -->");
            outString.Append("<partTimeJobs>");
            foreach (IAssignedJobInfo partTimeJob in this.PartTimeJobs)
            {
                if (displayComment)
                    outString.Append("<!-- ������ְ��ʶ (�ַ���) (nvarchar(36)) -->");
                outString.AppendFormat("<partTimeJobId><![CDATA[{0}]]></partTimeJobId>", partTimeJob.Id);
            }
            outString.Append("</partTimeJobs>");

            if (displayComment)
                outString.Append("<!-- ��׼��ɫ -->");
            outString.Append("<standardRoles>");
            if (this.Account != null)
            {
                IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByAccountId(this.Account.Id);

                foreach (IRoleInfo role in roles)
                {
                    if (!string.IsNullOrEmpty(role.StandardRoleId) && !string.IsNullOrEmpty(role.StandardRoleId) && role.StandardRole != null)
                    {
                        if (displayComment)
                            outString.Append("<!-- ������׼��ɫ��ʶ (�ַ���) (nvarchar(36)) -->");

                        outString.AppendFormat("<standardRole organizationId=\"{0}\" standardRoleType=\"{1}\" standardRoleId=\"{2}\" />",
                            role.OrganizationId,
                            role.StandardRole.Type,
                            role.StandardRole.Id);
                    }
                }
            }
            outString.Append("</standardRoles>");

            if (displayComment)
                outString.Append("<!-- ��������-->");
            outString.Append("<relationObjects>");
            if (this.Account != null)
            { //foreach (IOrganizationInfo organization in this.Account.Organizations)
                //{
                //    outString.AppendFormat("<relationObject id=\"{0}\" type=\"Organization\" />", organization.Id);
                //}
                foreach (IAccountRoleRelationInfo relation in this.Account.RoleRelations)
                {
                    if (displayComment)
                        outString.Append("<!-- ������ɫ��ʶ (�ַ���) (nvarchar(36)) -->");

                    outString.AppendFormat("<relationObject id=\"{0}\" type=\"Role\" />", relation.RoleId);
                }
                foreach (IAccountGroupRelationInfo relation in this.Account.GroupRelations)
                {
                    if (displayComment)
                        outString.Append("<!-- ����Ⱥ����ʶ(�����Ż�ϵͳ�����Ժ��ԡ�) (�ַ���) (nvarchar(36)) -->");

                    outString.AppendFormat("<relationObject id=\"{0}\" type=\"Group\" />", relation.GroupId);
                }
            }
            outString.Append("</relationObjects>");
            if (displayComment)
                outString.Append("<!-- ״̬ (����) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Account == null ? 0 : this.Account.Status);
            if (displayComment)
                outString.Append("<!-- ��������ʱ�� (ʱ��) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.UpdateDate);
            outString.Append("</user>");

            return outString.ToString();
        }

        #endregion

        #region 属性:Deserialize(XmlElement element)
        /// <summary>����XmlԪ�ؼ��ض���</summary>
        /// <param name="element">XmlԪ��</param>
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

            this.OrganizationId = element.SelectSingleNode("organizationId").InnerText;

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

            this.UpdateDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
