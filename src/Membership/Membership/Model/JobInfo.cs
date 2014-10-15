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

    /// <summary>ְλ��Ϣ</summary>
    public class JobInfo : IJobInfo
    {
        #region ���캯��:JobInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
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

        /// <summary>ְλ���б�ʶ</summary>
        public string JobFamilyId
        {
            get { return m_JobFamilyId; }
            set { m_JobFamilyId = value; }
        }
        #endregion

        #region 属性:JobFamilyName
        private string m_JobFamilyName = null;

        /// <summary>ְλ��������</summary>
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

        /// <summary>����</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Lock
        private int m_Lock = 1;

        /// <summary>��ֹ����ɾ�� 0 ������ | 1 ����(Ĭ��)</summary>
        public int Lock
        {
            get { return m_Lock; }
            set { m_Lock = value; }
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

        /// <summary>��ע</summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ��ʽʵ�� IAuthorizationObject Type
        // -------------------------------------------------------

        #region 属性:IAuthorizationObject.Type
        /// <summary>����</summary>
        string IAuthorizationObject.Type
        {
            get { return "Job"; }
        }
        #endregion

        // -------------------------------------------------------
        // ʵ�� ISerializedObject ���л�
        // -------------------------------------------------------

        #region 属性:Deserialize(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.GetElementsByTagName("id")[0].InnerText;
            this.Code = element.GetElementsByTagName("code")[0].InnerText;
            this.Name = element.GetElementsByTagName("name")[0].InnerText;
            this.Status = Convert.ToInt32(element.GetElementsByTagName("status")[0].InnerText);
            this.UpdateDate = Convert.ToDateTime(element.GetElementsByTagName("updateDate")[0].InnerText);
        }
        #endregion

        #region 属性:Serializable()
        /// <summary></summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 属性:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary></summary>
        /// <returns></returns>
        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();
            if (displayComment)
                outString.Append("<!-- ְλ���� -->");
            outString.Append("<job>");
            if (displayComment)
                outString.Append("<!-- ְλ��ʶ (�ַ���) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- ���� (�ַ���) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- ���� (�ַ���) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- ״̬ (����) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- ��������ʱ�� (ʱ��) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.UpdateDate);
            outString.Append("</job>");

            return outString.ToString();
        }
        #endregion
    }
}
