//=============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
//=============================================================================

using System;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;

using X3Platform.Logging;
using X3Platform.CacheBuffer;
using System.Xml;
using X3Platform.Ajax;

namespace X3Platform.Membership.HumanResources.Model
{
    /// <summary>用户扩展属性信息</summary>
    [Serializable]
    public class MemberExtensionInformation : IExtensionInformation
    {
        #region 构造函数:MemberExtensionInformation()
        /// <summary>默认构造函数</summary>
        public MemberExtensionInformation() { }
        #endregion

        #region 属性:Id
        /// <summary></summary>
        public string Id
        {
            set { m_AccountId = value; }
            get { return m_AccountId; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            set { m_AccountId = value; }
            get { return m_AccountId; }
        }
        #endregion

        #region 属性:Country
        private string m_Country = string.Empty;

        /// <summary></summary>
        public string Country
        {
            set { m_Country = value; }
            get { return m_Country; }
        }
        #endregion

        #region 属性:National
        private string m_National = string.Empty;

        /// <summary></summary>
        public string National
        {
            set { m_National = value; }
            get { return m_National; }
        }
        #endregion

        #region 属性:Passport
        private string m_Passport = string.Empty;

        /// <summary></summary>
        public string Passport
        {
            set { m_Passport = value; }
            get { return m_Passport; }
        }
        #endregion

        #region 属性:MaritalStatus
        private string m_MaritalStatus = string.Empty;

        /// <summary></summary>
        public string MaritalStatus
        {
            set { m_MaritalStatus = value; }
            get { return m_MaritalStatus; }
        }
        #endregion

        #region 属性:Professional
        private string m_Professional = string.Empty;

        /// <summary>专长</summary>
        public string Professional
        {
            set { m_Professional = value; }
            get { return m_Professional; }
        }
        #endregion

        #region 属性:Hobby
        private string m_Hobby = string.Empty;

        /// <summary></summary>
        public string Hobby
        {
            set { m_Hobby = value; }
            get { return m_Hobby; }
        }
        #endregion

        #region 属性:Profile
        private string m_Profile = string.Empty;

        /// <summary></summary>
        public string Profile
        {
            set { m_Profile = value; }
            get { return m_Profile; }
        }
        #endregion

        #region 属性:HighestEducation
        private string m_HighestEducation = string.Empty;

        /// <summary></summary>
        public string HighestEducation
        {
            set { m_HighestEducation = value; }
            get { return m_HighestEducation; }
        }
        #endregion

        #region 属性:HighestDegree
        private string m_HighestDegree = string.Empty;

        /// <summary></summary>
        public string HighestDegree
        {
            set { m_HighestDegree = value; }
            get { return m_HighestDegree; }
        }
        #endregion

        #region 属性:ForeignLanguage
        private string m_ForeignLanguage = string.Empty;

        /// <summary></summary>
        public string ForeignLanguage
        {
            set { m_ForeignLanguage = value; }
            get { return m_ForeignLanguage; }
        }
        #endregion

        #region 属性:ForeignLanguageLevel
        private string m_ForeignLanguageLevel = string.Empty;

        /// <summary></summary>
        public string ForeignLanguageLevel
        {
            set { m_ForeignLanguageLevel = value; }
            get { return m_ForeignLanguageLevel; }
        }
        #endregion

        #region 属性:GraduationSchool
        private string m_GraduationSchool = string.Empty;

        /// <summary></summary>
        public string GraduationSchool
        {
            set { m_GraduationSchool = value; }
            get { return m_GraduationSchool; }
        }
        #endregion

        #region 属性:GraduationCertificateId
        private string m_GraduationCertificateId = string.Empty;

        /// <summary></summary>
        public string GraduationCertificateId
        {
            set { m_GraduationCertificateId = value; }
            get { return m_GraduationCertificateId; }
        }
        #endregion

        #region 属性:Major
        private string m_Major = string.Empty;

        /// <summary></summary>
        public string Major
        {
            set { m_Major = value; }
            get { return m_Major; }
        }
        #endregion

        #region 属性:EmployeeId
        private string m_EmployeeId = string.Empty;

        /// <summary></summary>
        public string EmployeeId
        {
            set { m_EmployeeId = value; }
            get { return m_EmployeeId; }
        }
        #endregion

        #region 属性:AttendanceCardId
        private string m_AttendanceCardId = string.Empty;

        /// <summary></summary>
        public string AttendanceCardId
        {
            set { m_AttendanceCardId = value; }
            get { return m_AttendanceCardId; }
        }
        #endregion

        #region 属性:JobGrade
        private string m_JobGrade = string.Empty;

        /// <summary></summary>
        public string JobGrade
        {
            set { m_JobGrade = value; }
            get { return m_JobGrade; }
        }
        #endregion

        #region 属性:JobBegindate
        private DateTime m_JobBegindate;

        /// <summary></summary>
        public DateTime JobBegindate
        {
            get
            {
                if (m_JobBegindate == DateTime.MinValue)
                {
                    m_JobBegindate = new DateTime(2000, 1, 1);
                }

                return m_JobBegindate;
            }
            set { m_JobBegindate = value; }
        }
        #endregion

        #region 属性:JobOfficialDate
        private DateTime m_JobOfficialDate;

        /// <summary></summary>
        public DateTime JobOfficialDate
        {
            get
            {
                if (m_JobOfficialDate == DateTime.MinValue)
                {
                    m_JobOfficialDate = new DateTime(2000, 1, 1);
                }

                return m_JobOfficialDate;
            }
            set { m_JobOfficialDate = value; }
        }
        #endregion

        #region 属性:JobEndDate
        private DateTime m_JobEndDate;

        /// <summary></summary>
        public DateTime JobEndDate
        {
            get
            {
                if (m_JobEndDate == DateTime.MinValue)
                {
                    m_JobEndDate = new DateTime(2000, 1, 1);
                }

                return m_JobEndDate;
            }
            set { m_JobEndDate = value; }
        }
        #endregion

        #region 属性:JobStatus
        private int m_JobStatus = 0;

        /// <summary></summary>
        public int JobStatus
        {
            set { m_JobStatus = value; }
            get { return m_JobStatus; }
        }
        #endregion

        #region 属性:ContractType
        private string m_ContractType = string.Empty;

        /// <summary></summary>
        public string ContractType
        {
            set { m_ContractType = value; }
            get { return m_ContractType; }
        }
        #endregion

        #region 属性:ContractExpireDate
        private DateTime m_ContractExpireDate;

        /// <summary></summary>
        public DateTime ContractExpireDate
        {
            set { m_ContractExpireDate = value; }
            get
            {
                if (m_ContractExpireDate == DateTime.MinValue)
                {
                    m_ContractExpireDate = new DateTime(2000, 1, 1);
                }

                return m_ContractExpireDate;
            }
        }
        #endregion

        #region 属性:PaidHoliday
        private int m_PaidHoliday = 5;

        /// <summary></summary>
        public int PaidHoliday
        {
            set { m_PaidHoliday = value; }
            get { return m_PaidHoliday; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark = string.Empty;

        /// <summary></summary>
        public string Remark
        {
            set { m_Remark = value; }
            get { return m_Remark; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            set { m_UpdateDate = value; }
            get { return m_UpdateDate; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            set { m_CreateDate = value; }
            get { return m_CreateDate; }
        }
        #endregion

        private Dictionary<string, object> properties = new Dictionary<string, object>();

        #region 索引:this[string name]
        /// <summary>属性索引信息</summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        public object this[string name]
        {
            get { return this.properties[name]; }
            set
            {
                if (this.properties.ContainsKey(name))
                {
                    this.properties[name] = value;
                }
                else
                {
                    this.properties.Add(name, value);
                }
            }
        }
        #endregion

        #region IExtensionInformation 成员

        public void Load(XmlDocument doc)
        {
            // 常用信息
            this.AccountId = AjaxStorageConvertor.Fetch("accountId", doc);

            this.Country = AjaxStorageConvertor.Fetch("country", doc);
            this.National = AjaxStorageConvertor.Fetch("national", doc);
            this.Passport = AjaxStorageConvertor.Fetch("passport", doc);
            this.MaritalStatus = AjaxStorageConvertor.Fetch("maritalStatus", doc);
            this.Professional = AjaxStorageConvertor.Fetch("professional", doc);
            this.Hobby = AjaxStorageConvertor.Fetch("hobby", doc);
            this.Profile = AjaxStorageConvertor.Fetch("profile", doc);

            // 学历信息
            this.HighestEducation = AjaxStorageConvertor.Fetch("highestEducation", doc);
            this.HighestDegree = AjaxStorageConvertor.Fetch("highestDegree", doc);
            this.ForeignLanguage = AjaxStorageConvertor.Fetch("foreignLanguage", doc);
            this.ForeignLanguageLevel = AjaxStorageConvertor.Fetch("foreignLanguageLevel", doc);
            this.GraduationSchool = AjaxStorageConvertor.Fetch("graduationSchool", doc);
            this.GraduationCertificateId = AjaxStorageConvertor.Fetch("graduationCertificateId", doc);
            this.Major = AjaxStorageConvertor.Fetch("major", doc);

            // 用工信息
            this.EmployeeId = AjaxStorageConvertor.Fetch("employeeId", doc);
            this.AttendanceCardId = AjaxStorageConvertor.Fetch("attendanceCardId", doc);
            this.JobBegindate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("jobBegindate", doc));
            this.JobOfficialDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("jobOfficialDate", doc));
            this.JobEndDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("jobEndDate", doc));
            this.JobStatus = Convert.ToInt32(AjaxStorageConvertor.Fetch("jobStatus", doc));
            this.ContractType = AjaxStorageConvertor.Fetch("contractType", doc);
            this.ContractExpireDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("contractExpireDate", doc));
            this.PaidHoliday = Convert.ToInt32(AjaxStorageConvertor.Fetch("paidHoliday", doc));
            this.Remark = AjaxStorageConvertor.Fetch("remark", doc);
        }

        public void Load(Dictionary<string, object> args)
        {
            this.AccountId = args["AccountId"].ToString();

            MemberExtensionInformation temp = null;

            try
            {
                temp = HumanResourceManagement.Instance.HumanResourceOfficerService.MemberExtensionInformationProvider.FindOneByAccountId(AccountId);
            }
            catch
            {
                throw;
            }

            if (temp == null) { return; }

            // 常用信息
            this.Country = temp.Country;
            this.National = temp.National;
            this.Passport = temp.Passport;
            this.MaritalStatus = temp.MaritalStatus;
            this.Professional = temp.Professional;
            this.Hobby = temp.Hobby;
            this.Profile = temp.Profile;

            // 学历信息
            this.HighestEducation = temp.HighestEducation;
            this.HighestDegree = temp.HighestDegree;
            this.ForeignLanguage = temp.ForeignLanguage;
            this.ForeignLanguageLevel = temp.ForeignLanguageLevel;
            this.GraduationSchool = temp.GraduationSchool;
            this.GraduationCertificateId = temp.GraduationCertificateId;
            this.Major = temp.Major;

            // 用工信息
            this.EmployeeId = temp.Remark;
            this.AttendanceCardId = temp.Remark;
            this.JobBegindate = temp.JobBegindate;
            this.JobOfficialDate = temp.JobOfficialDate;
            this.JobEndDate = temp.JobEndDate;
            this.JobStatus = temp.JobStatus;
            this.ContractType = temp.ContractType;
            this.ContractExpireDate = temp.ContractExpireDate;
            this.PaidHoliday = temp.PaidHoliday;
            this.Remark = temp.Remark;
        }

        public void Save()
        {
            HumanResourceManagement.Instance.HumanResourceOfficerService.MemberExtensionInformationProvider.Save(this);
        }

        public void Delete()
        {
            HumanResourceManagement.Instance.HumanResourceOfficerService.MemberExtensionInformationProvider.Delete(this.AccountId);
        }
        #endregion
    }
}
