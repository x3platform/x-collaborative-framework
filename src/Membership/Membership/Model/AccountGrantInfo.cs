namespace X3Platform.Membership.Model
{
    using System;
    using System.Xml;
    using System.Text;

    /// <summary>帐户委托信息</summary>
    [Serializable]
    public class AccountGrantInfo : IAccountGrantInfo
    {
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
        private string m_Code;

        /// <summary>编码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Grantor
        private IAccountInfo m_Grantor = null;

        /// <summary>委托人</summary>
        public IAccountInfo Grantor
        {
            get
            {
                if (m_Grantor == null && !string.IsNullOrEmpty(this.GrantorId))
                {
                    m_Grantor = MembershipManagement.Instance.AccountService[this.GrantorId];
                }
                return m_Grantor;
            }
        }
        #endregion

        #region 属性:GrantorId
        private string m_GrantorId;

        /// <summary>委托人</summary>
        public string GrantorId
        {
            get { return m_GrantorId; }
            set { m_GrantorId = value; }
        }
        #endregion

        #region 属性:GrantorName
        /// <summary>委托人姓名</summary>
        public string GrantorName
        {
            get { return this.Grantor == null ? string.Empty : this.Grantor.Name; }
        }
        #endregion

        #region 属性:Grantor
        private IAccountInfo m_Grantee = null;

        /// <summary>委托人</summary>
        public IAccountInfo Grantee
        {
            get
            {
                if (m_Grantee == null && !string.IsNullOrEmpty(this.GranteeId))
                {
                    m_Grantee = MembershipManagement.Instance.AccountService[this.GranteeId];
                }
                return m_Grantee;
            }
        }
        #endregion

        #region 属性:GranteeId
        private string m_GranteeId;

        /// <summary>被委托人</summary>
        public string GranteeId
        {
            get { return m_GranteeId; }
            set { m_GranteeId = value; }
        }
        #endregion

        #region 属性:GranteeName
        /// <summary>被委托人姓名</summary>
        public string GranteeName
        {
            get { return this.Grantee == null ? string.Empty : this.Grantee.Name; }
        }
        #endregion

        #region 属性:GrantedTimeFrom
        private DateTime m_GrantedTimeFrom;

        /// <summary></summary>
        public DateTime GrantedTimeFrom
        {
            get { return m_GrantedTimeFrom; }
            set { m_GrantedTimeFrom = value; }
        }
        #endregion

        #region 属性:GrantedTimeTo
        private DateTime m_GrantedTimeTo;

        /// <summary></summary>
        public DateTime GrantedTimeTo
        {
            get { return m_GrantedTimeTo; }
            set { m_GrantedTimeTo = value; }
        }
        #endregion

        #region 属性:WorkflowGrantMode
        private int m_WorkflowGrantMode = 1;

        /// <summary>流程审批委托类别：1 未激活的流程审批只发待办给被委托的人 | 2 未激活的流程审批同时发送待办给委托人和被委托人 | 4 已激活的流程审批移交给被委托人 | 8 已激活的流程审批不移交给被委托人</summary>
        public int WorkflowGrantMode
        {
            get { return m_WorkflowGrantMode; }
            set { m_WorkflowGrantMode = value; }
        }
        #endregion

        #region 属性:WorkflowGrantModeView
        private string m_WorkflowGrantModeView;

        /// <summary>流程审批委托类别视图</summary>
        public string WorkflowGrantModeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_WorkflowGrantModeView))
                {
                    m_WorkflowGrantModeView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_帐号委托_流程审批委托类别",
                        this.WorkflowGrantMode.ToString()
                        );
                }

                return m_WorkflowGrantModeView;
            }
        }
        #endregion

        #region 属性:DataQueryGrantMode
        private int m_DataQueryGrantMode;

        /// <summary>数据查询的模式：0 不委托 | 1 委托</summary>
        public int DataQueryGrantMode
        {
            get { return m_DataQueryGrantMode; }
            set { m_DataQueryGrantMode = value; }
        }
        #endregion

        #region 属性:DataQueryGrantModeView
        private string m_DataQueryGrantModeView;

        /// <summary>数据授权委托类别视图</summary>
        public string DataQueryGrantModeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_DataQueryGrantModeView))
                {
                    m_DataQueryGrantModeView = MembershipManagement.Instance.SettingService.GetText(
                        "应用管理_协同平台_人员及权限管理_帐号委托_数据授权委托类别",
                        this.DataQueryGrantMode.ToString()
                        );
                }

                return m_DataQueryGrantModeView;
            }
        }
        #endregion

        #region 属性:IsAborted
        private bool m_IsAborted;

        /// <summary>是否中止</summary>
        public bool IsAborted
        {
            get { return m_IsAborted; }
            set { m_IsAborted = value; }
        }
        #endregion

        #region 属性:ActualFinishedTime
        private DateTime m_ActualFinishedTime = new DateTime(2000, 1, 1);

        /// <summary>实际结束时间</summary>
        public DateTime ActualFinishedTime
        {
            get
            {
                // 实际结束时间大于预计结束时间按，或者当前时间大于预计结束时间，则将实际结束时间设置为预计结束时间。
                if (m_ActualFinishedTime > this.GrantedTimeTo || DateTime.Now > this.GrantedTimeTo)
                {
                    m_ActualFinishedTime = this.GrantedTimeTo;
                }
                return m_ActualFinishedTime;
            }
            set { m_ActualFinishedTime = value; }
        }
        #endregion

        #region 属性:ApprovedUrl
        private string m_ApprovedUrl = string.Empty;

        /// <summary>相关审批地址</summary>
        public string ApprovedUrl
        {
            get { return m_ApprovedUrl; }
            set { m_ApprovedUrl = value; }
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

        /// <summary>相关审批地址</summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
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
        // Xml 元素的导入和导出 
        //

        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>序列化对象</summary>
        /// <param name="displayComment">显示注释</param>
        /// <param name="displayFriendlyName">显示友好名称</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            if (displayComment)
                outString.Append("<!-- 角色对象 -->");
            outString.Append("<grant>");
            if (displayComment)
                outString.Append("<!-- 角色标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 委托人标识 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<grantorId><![CDATA[{0}]]></grantorId>", this.GrantorId);
            if (displayComment)
                outString.Append("<!-- 被委托人标识 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<granteeId><![CDATA[{0}]]></granteeId>", this.GranteeId);
            if (displayComment)
                outString.Append("<!-- 委托开始时间(时间) (datetime) -->");
            outString.AppendFormat("<grantedTimeFrom><![CDATA[{0}]]></grantedTimeFrom>", this.GrantedTimeFrom);
            if (displayComment)
                outString.Append("<!-- 委托结束时间(时间) (datetime) -->");
            outString.AppendFormat("<grantedTimeTo><![CDATA[{0}]]></grantedTimeTo>", this.GrantedTimeTo);
            if (displayComment)
                outString.Append("<!-- 工作流委托模式(整型) (int) -->");
            outString.AppendFormat("<workflowGrantMode><![CDATA[{0}]]></workflowGrantMode>", this.WorkflowGrantMode);
            if (displayComment)
                outString.Append("<!-- 数据查询委托模式(整型) (int) -->");
            outString.AppendFormat("<dataQueryGrantMode><![CDATA[{0}]]></dataQueryGrantMode>", this.DataQueryGrantMode);
            if (displayComment)
                outString.Append("<!-- 手工中止(布尔值) (bit) -->");
            outString.AppendFormat("<isAborted><![CDATA[{0}]]></isAborted>", this.IsAborted);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</grant>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml元素</param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.SelectSingleNode("id").InnerText;
            this.Code = element.SelectSingleNode("code").InnerText;
            this.GrantorId = element.SelectSingleNode("grantorId").InnerText;
            this.GranteeId = element.SelectSingleNode("granteeId").InnerText;
            this.GrantedTimeFrom = Convert.ToDateTime(element.SelectSingleNode("grantedTimeFrom").InnerText);
            this.GrantedTimeTo = Convert.ToDateTime(element.SelectSingleNode("grantedTimeTo").InnerText);
            this.WorkflowGrantMode = Convert.ToInt32(element.SelectSingleNode("workflowGrantMode").InnerText);
            this.DataQueryGrantMode = Convert.ToInt32(element.SelectSingleNode("dataQueryGrantMode").InnerText);
            this.IsAborted = Convert.ToBoolean(element.SelectSingleNode("isAborted").InnerText);
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;
            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion

    }
}
