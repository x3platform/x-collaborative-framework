namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary>应用菜单查询类</summary>
    public class ApplicationMenuQueryInfo
    {
        #region 构造函数:ApplicationMenuQueryInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationMenuQueryInfo() { }
        #endregion
        
        #region 属性:Id
        private string m_Id = string.Empty;

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
        private string m_ApplicationId = string.Empty;

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
        private ApplicationMenuInfo m_Parent;

        /// <summary>应用</summary>
        public ApplicationMenuInfo Parent
        {
            get
            {
                if (this.ParentId == Guid.Empty.ToString())
                    return null;

                if (m_Parent == null && !string.IsNullOrEmpty(this.ParentId))
                {
                    m_Parent = AppsContext.Instance.ApplicationMenuService.FindOne(this.ParentId);
                }

                return m_Parent;
            }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId = Guid.Empty.ToString();

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
        private string m_Name = string.Empty;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary></summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url = string.Empty;

        /// <summary></summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        #endregion

        #region 属性:Target
        private string m_Target = "_self";

        /// <summary></summary>
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        #endregion

        #region 属性:TargetView
        private string m_TargetView = string.Empty;

        /// <summary></summary>
        public string TargetView
        {
            get
            {
                if (string.IsNullOrEmpty(m_TargetView) && !string.IsNullOrEmpty(this.Target))
                {
                    this.m_TargetView = AppsContext.Instance.ApplicationSettingService.GetText(
                       AppsContext.Instance.ApplicationService["ApplicationManagement"].Id,
                       "应用管理_应用链接打开方式",
                       this.Target);
                }

                return m_TargetView;
            }
        }
        #endregion

        #region 属性:MenuType
        private string m_MenuType = "ApplicationMenu";

        /// <summary></summary>
        public string MenuType
        {
            get { return m_MenuType; }
            set { m_MenuType = value; }
        }
        #endregion

        #region 属性:MenuTypeView
        private string m_MenuTypeView = null;

        /// <summary></summary>
        public string MenuTypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_MenuTypeView) && !string.IsNullOrEmpty(this.MenuType))
                {
                    this.m_MenuTypeView = AppsContext.Instance.ApplicationSettingService.GetText(
                       AppsContext.Instance.ApplicationService["ApplicationManagement"].Id,
                       "应用管理_应用菜单类别",
                       m_MenuType);
                }

                return m_MenuTypeView;
            }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath = string.Empty;

        /// <summary></summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:BigIconPath
        private string m_BigIconPath = string.Empty;

        /// <summary></summary>
        public string BigIconPath
        {
            get { return m_BigIconPath; }
            set { m_BigIconPath = value; }
        }
        #endregion

        #region 属性:DisplayType
        private string m_DisplayType = string.Empty;

        /// <summary>显示方式</summary>
        public string DisplayType
        {
            get { return m_DisplayType; }
            set { m_DisplayType = value; }
        }
        #endregion

        #region 属性:DisplayTypeView
        private string m_DisplayTypeView = string.Empty;

        /// <summary></summary>
        public string DisplayTypeView
        {
            get
            {
                if (string.IsNullOrEmpty(m_DisplayTypeView) && !string.IsNullOrEmpty(this.DisplayType))
                {
                    this.m_DisplayTypeView = AppsContext.Instance.ApplicationSettingService.GetText(
                       AppsContext.Instance.ApplicationService["ApplicationManagement"].Id,
                       "应用管理_应用菜单展现方式",
                       this.DisplayType);
                }

                return m_DisplayTypeView;
            }
        }
        #endregion

        #region 属性:HasChild
        private int m_HasChild;

        /// <summary></summary>
        public int HasChild
        {
            get { return m_HasChild; }
            set { m_HasChild = value; }
        }
        #endregion

        #region 属性:ContextObject
        private string m_ContextObject = string.Empty;

        /// <summary>上下文对象</summary>
        public string ContextObject
        {
            get { return m_ContextObject; }
            set { m_ContextObject = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

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
        private string m_Remark = string.Empty;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:FullPath
        private string m_FullPath = string.Empty;

        /// <summary></summary>
        public string FullPath
        {
            get { return m_FullPath; }
            set { m_FullPath = value; }
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
    }
}
