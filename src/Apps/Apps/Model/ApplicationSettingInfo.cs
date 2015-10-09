namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Text;
    using System.Xml;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary></summary>
    public class ApplicationSettingInfo : EntityClass, ICacheable
    {
        #region 构造函数:ApplicationSettingInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationSettingInfo() { }
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

        #region 属性:Application
        private ApplicationInfo m_Application;

        /// <summary>应用</summary>
        public ApplicationInfo Application
        {
            get
            {
                if (m_Application == null && !string.IsNullOrEmpty(this.ApplicationId))
                {
                    m_Application = AppsContext.Instance.ApplicationService.FindOne(this.ApplicationId);
                }

                return m_Application;
            }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return m_ApplicationId; }
            set { m_ApplicationId = value; }
        }
        #endregion

        #region 属性:ApplicationName
        /// <summary></summary>
        public string ApplicationName
        {
            get { return this.Application == null ? string.Empty : this.Application.ApplicationName; }
        }
        #endregion

        #region 属性:ApplicationDisplayName
        /// <summary></summary>
        public string ApplicationDisplayName
        {
            get { return this.Application == null ? string.Empty : this.Application.ApplicationDisplayName; }
        }
        #endregion

        #region 属性:ApplicationSettingGroup
        private ApplicationSettingGroupInfo m_ApplicationSettingGroup;

        /// <summary>应用</summary>
        public ApplicationSettingGroupInfo ApplicationSettingGroup
        {
            get
            {
                if (m_ApplicationSettingGroup == null && !string.IsNullOrEmpty(this.ApplicationSettingGroupId))
                {
                    m_ApplicationSettingGroup = AppsContext.Instance.ApplicationSettingGroupService.FindOne(this.ApplicationSettingGroupId);
                }

                return m_ApplicationSettingGroup;
            }
        }
        #endregion

        #region 属性:ApplicationSettingGroupId
        private string m_ApplicationSettingGroupId;

        /// <summary></summary>
        public string ApplicationSettingGroupId
        {
            get { return m_ApplicationSettingGroupId; }
            set { m_ApplicationSettingGroupId = value; }
        }
        #endregion

        #region 属性:ApplicationSettingGroupName
        /// <summary></summary>
        public string ApplicationSettingGroupName
        {
            get { return this.ApplicationSettingGroup == null ? string.Empty : this.ApplicationSettingGroup.Name; }
        }
        #endregion

        #region 属性:ApplicationSettingGroupDisplayName
        /// <summary></summary>
        public string ApplicationSettingGroupDisplayName
        {
            get { return this.ApplicationSettingGroup == null ? string.Empty : this.ApplicationSettingGroup.DisplayName; }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary>代码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Text
        private string m_Text;

        /// <summary>文本</summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }
        #endregion

        #region 属性:Value
        private string m_Value;

        /// <summary>值</summary>
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
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

        /// <summary></summary>
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
        // 设置 EntityClass 标识
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 ICacheable
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.MaxValue;

        /// <summary>过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 实现 EntityClass 序列化
        // -------------------------------------------------------

        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
        /// <returns></returns>
        public override string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>序列化对象</summary>
        /// <param name="displayComment">显示注释</param>
        /// <param name="displayFriendlyName">显示友好名称</param>
        /// <returns></returns>
        public override string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            if (displayComment)
                outString.Append("<!-- 应用参数对象 -->");
            outString.Append("<setting>");
            if (displayComment)
                outString.Append("<!-- 应用参数标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 所属应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<applicationId><![CDATA[{0}]]></applicationId>", this.ApplicationId);
            if (displayComment)
                outString.Append("<!-- 所属应用参数分组标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<applicationSettingGroupId><![CDATA[{0}]]></applicationSettingGroupId>", this.ApplicationSettingGroupId);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 参数文本信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<text><![CDATA[{0}]]></text>", this.Text);
            if (displayComment)
                outString.Append("<!-- 参数值信息 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<value><![CDATA[{0}]]></value>", this.Value);
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
            outString.Append("</setting>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml元素</param>
        public override void Deserialize(XmlElement element)
        {
            this.Id = element.SelectSingleNode("id").InnerText;
            this.ApplicationId = element.SelectSingleNode("applicationId").InnerText;
            this.ApplicationSettingGroupId = element.SelectSingleNode("applicationSettingGroupId").InnerText;
            this.Code = element.SelectSingleNode("code").InnerText;
            this.Text = element.SelectSingleNode("text").InnerText;
            this.Value = element.SelectSingleNode("value").InnerText;
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;

            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
