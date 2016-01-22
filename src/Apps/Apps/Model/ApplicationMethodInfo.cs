namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Text;
    using System.Xml;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>应用方法</summary>
    public class ApplicationMethodInfo : EntityClass, ICacheable
    {
        #region 构造函数:ApplicationMethodInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationMethodInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:Application
        private ApplicationInfo m_Application;

        /// <summary>应用</summary>
        public ApplicationInfo Application
        {
            get
            {
                if (this.m_Application == null && !string.IsNullOrEmpty(this.ApplicationId))
                {
                    this.m_Application = AppsContext.Instance.ApplicationService.FindOne(this.ApplicationId);
                }

                return this.m_Application;
            }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId = "00000000-0000-0000-0000-000000000001";

        /// <summary></summary>
        public string ApplicationId
        {
            get { return this.m_ApplicationId; }
            set { this.m_ApplicationId = value; }
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

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary></summary>
        public string Code
        {
            get { return this.m_Code; }
            set { this.m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary></summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region 属性:DisplayName
        private string m_DisplayName = string.Empty;

        /// <summary></summary>
        public string DisplayName
        {
            get { return this.m_DisplayName; }
            set { this.m_DisplayName = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary></summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region 属性:Detail
        private string m_Detail = string.Empty;

        /// <summary></summary>
        public string Detail
        {
            get { return this.m_Detail; }
            set { this.m_Detail = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type = string.Empty;

        /// <summary>类型: generic | ajax | wsdl</summary>
        public string Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }
        #endregion

        #region 属性:Options
        private string m_Options = string.Empty;

        /// <summary>选项</summary>
        public string Options
        {
            get { return this.m_Options; }
            set { this.m_Options = value; }
        }
        #endregion

        #region 属性:EffectScope
        private int m_EffectScope = 0;

        /// <summary>作用范围 1 匿名用户 2 登录用户 4 应用可访问成员 8 应用审查员 16 应用管理员</summary>
        public int EffectScope
        {
            get { return this.m_EffectScope; }
            set { this.m_EffectScope = value; }
        }
        #endregion

        #region 属性:Version
        private int m_Version;

        /// <summary>版本</summary>
        public int Version
        {
            get { return this.m_Version; }
            set { this.m_Version = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

        /// <summary></summary>
        public string OrderId
        {
            get { return this.m_OrderId; }
            set { this.m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return this.m_Status; }
            set { this.m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark = string.Empty;

        /// <summary></summary>
        public string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return this.m_ModifiedDate; }
            set { this.m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return this.m_CreatedDate; }
            set { this.m_CreatedDate = value; }
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
            get { return this.m_Expires; }
            set { this.m_Expires = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 实现 EntityClass 序列化
        // -------------------------------------------------------

        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
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
                outString.Append("<!-- 应用功能对象 -->");
            outString.Append("<feature>");
            if (displayComment)
                outString.Append("<!-- 应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);

            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);

            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            outString.Append("</feature>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml元素</param>
        public override void Deserialize(XmlElement element)
        {
            this.Id = element.GetElementsByTagName("id")[0].InnerText;
            this.Code = element.GetElementsByTagName("code")[0].InnerText;
            // this.Name = element.GetElementsByTagName("name")[0].InnerText;
            this.Status = Convert.ToInt32(element.GetElementsByTagName("status")[0].InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.GetElementsByTagName("updateDate")[0].InnerText);
        }
        #endregion
    }
}
