#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ApplicationSettingGroupInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using X3Platform.CacheBuffer;
    #endregion

    /// <summary></summary>
    public class ApplicationSettingGroupInfo : EntityClass, ICacheable
    {
        #region 构造函数:ApplicationSettingGroupInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationSettingGroupInfo() { }
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

        #region 属性:Parent
        private ApplicationSettingGroupInfo m_Parent;

        /// <summary>应用</summary>
        public ApplicationSettingGroupInfo Parent
        {
            get
            {
                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = AppsContext.Instance.ApplicationSettingGroupService.FindOne(this.ParentId);
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId;

        /// <summary></summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:ParentName
        /// <summary></summary>
        public string ParentName
        {
            get { return this.Parent == null ? this.ApplicationDisplayName : this.Parent.Name; }
        }
        #endregion

        #region 属性:ParentDisplayName
        /// <summary></summary>
        public string ParentDisplayName
        {
            get { return this.Parent == null ? string.Empty : this.Parent.DisplayName; }
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

        #region 属性:DisplayName
        private string m_DisplayName;

        /// <summary></summary>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DisplayName))
                {
                    this.m_DisplayName = this.Name;
                }

                return m_DisplayName;
            }
            set { m_DisplayName = value; }
        }
        #endregion

        #region 属性:ContentType
        private int m_ContentType;

        /// <summary></summary>
        public int ContentType
        {
            get { return m_ContentType; }
            set { m_ContentType = value; }
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
                outString.Append("<!-- 应用参数分组对象 -->");
            outString.Append("<settingGroup>");
            if (displayComment)
                outString.Append("<!-- 应用参数标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 所属应用标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<applicationId><![CDATA[{0}]]></applicationId>", this.ApplicationId);
            if (displayComment)
                outString.Append("<!-- 所属应用参数分组标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(100)) -->");
            outString.AppendFormat("<text><![CDATA[{0}]]></text>", this.Name);
            if (displayComment)
                outString.Append("<!-- 显示名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<displayName><![CDATA[{0}]]></displayName>", this.DisplayName);
            if (displayComment)
                outString.Append("<!-- 内容类型 (整型) (int) -->");
            outString.AppendFormat("<contentType><![CDATA[{0}]]></contentType>", this.ContentType);
            if (displayComment)
                outString.Append("<!-- 排序编号 (字符串) (nvarchar(20)) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.UpdateDate);
            outString.Append("</settingGroup>");

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
            this.ParentId = element.SelectSingleNode("parentId").InnerText;
            this.Code = element.SelectSingleNode("code").InnerText;
            this.Name = element.SelectSingleNode("text").InnerText;
            this.DisplayName = element.SelectSingleNode("displayName").InnerText;
            this.ContentType = Convert.ToInt32(element.SelectSingleNode("contentType").InnerText);
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;
            this.UpdateDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion
    }
}
