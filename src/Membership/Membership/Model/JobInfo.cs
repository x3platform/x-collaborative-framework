#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :JobInfo.cs
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
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    #endregion

    /// <summary>职位信息</summary>
    public class JobInfo : IJobInfo
    {
        #region 构造函数:JobInfo()
        /// <summary>默认构造函数</summary>
        public JobInfo() { }
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

        #region 属性:JobFamilyId
        private string m_JobFamilyId;

        /// <summary>职位序列标识</summary>
        public string JobFamilyId
        {
            get { return m_JobFamilyId; }
            set { m_JobFamilyId = value; }
        }
        #endregion

        #region 属性:JobFamilyName
        private string m_JobFamilyName = null;

        /// <summary>职位序列名称</summary>
        public string JobFamilyName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_JobFamilyName) && !string.IsNullOrEmpty(this.JobFamilyId))
                {
                    IJobFamilyInfo temp = MembershipManagement.Instance.JobFamilyService[this.JobFamilyId];

                    this.m_JobFamilyName = (temp == null) ? string.Empty : temp.Name;
                }
                return this.m_JobFamilyName;
            }
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

        #region 属性:Description
        private string m_Description;

        /// <summary>描述</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
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
            get { return "Job"; }
        }
        #endregion

        // -------------------------------------------------------
        // 实现 ISerializedObject 序列化
        // -------------------------------------------------------

        #region 函数:Deserialize(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.GetElementsByTagName("id")[0].InnerText;
            this.Code = element.GetElementsByTagName("code")[0].InnerText;
            this.Name = element.GetElementsByTagName("name")[0].InnerText;
            this.Status = Convert.ToInt32(element.GetElementsByTagName("status")[0].InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.GetElementsByTagName("updateDate")[0].InnerText);
        }
        #endregion

        #region 函数:Serializable()
        /// <summary></summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary></summary>
        /// <returns></returns>
        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();
            if (displayComment)
                outString.Append("<!-- 职位对象 -->");
            outString.Append("<job>");
            if (displayComment)
                outString.Append("<!-- 职位标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</job>");

            return outString.ToString();
        }
        #endregion
    }
}
