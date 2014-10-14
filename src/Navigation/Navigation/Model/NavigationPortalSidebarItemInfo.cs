#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalSidebarItemInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Navigation.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Apps;
    using X3Platform.CacheBuffer;
    #endregion

    /// <summary></summary>
    public class NavigationPortalSidebarItemInfo : ICacheable
    {
        #region ���캯��:NavigationPortalSidebarItemInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public NavigationPortalSidebarItemInfo()
        {
        }
        #endregion

        #region ����:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region ����:SidebarItemGroupId
        private string m_SidebarItemGroupId;

        /// <summary></summary>
        public string SidebarItemGroupId
        {
            get { return this.m_SidebarItemGroupId; }
            set { this.m_SidebarItemGroupId = value; }
        }
        #endregion

        #region ����:SidebarItemGroupName
        private string m_SidebarItemGroupName;

        /// <summary></summary>
        public string SidebarItemGroupName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.SidebarItemGroupId))
                {
                    NavigationPortalSidebarItemGroupInfo group = NavigationContext.Instance.NavigationPortalSidebarItemGroupService.FindOne(this.SidebarItemGroupId);
                    if (group != null)
                    {
                        this.m_SidebarItemGroupName = group.Text;
                    }
                }
                return this.m_SidebarItemGroupName;
            }
        }
        #endregion

        #region ����:PortalId
        private string m_PortalId;

        /// <summary></summary>
        public string PortalId
        {
            get { return this.m_PortalId; }
            set { this.m_PortalId = value; }
        }
        #endregion

        #region ����:PortalName
        private string m_PortalName;

        /// <summary></summary>
        public string PortalName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PortalId))
                {
                    NavigationPortalInfo portal = NavigationContext.Instance.NavigationPortalService.FindOne(this.PortalId);
                    if (portal != null)
                    {
                        this.m_PortalName = portal.Text;
                    }
                }
                return this.m_PortalName;
            }
        }
        #endregion

        #region ����:ParentId
        private string m_ParentId;

        /// <summary></summary>
        public string ParentId
        {
            get { return this.m_ParentId; }
            set { this.m_ParentId = value; }
        }
        #endregion

        #region ����:ParentName
        private string m_ParentName;

        /// <summary></summary>
        public string ParentName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ParentId) && this.ParentId != "00000000-0000-0000-0000-000000000000")
                {
                    NavigationPortalSidebarItemInfo item = NavigationContext.Instance.NavigationPortalSidebarItemService.FindOne(this.ParentId);
                    if (item != null)
                        this.m_ParentName = item.Text;
                }
                return this.m_ParentName;
            }
        }
        #endregion

        #region ����:Text
        private string m_Text;

        /// <summary></summary>
        public string Text
        {
            get { return this.m_Text; }
            set { this.m_Text = value; }
        }
        #endregion

        #region ����:Description
        private string m_Description;

        /// <summary></summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region ����:Url
        private string m_Url;

        /// <summary></summary>
        public string Url
        {
            get { return this.m_Url; }
            set { this.m_Url = value; }
        }
        #endregion

        #region ����:Target
        private string m_Target;

        /// <summary></summary>
        public string Target
        {
            get { return this.m_Target; }
            set { this.m_Target = value; }
        }
        #endregion

        #region ����:TargetView
        private string m_TargetView;
        public string TargetView
        {
            get
            {
                if (!(!string.IsNullOrEmpty(this.m_TargetView) || string.IsNullOrEmpty(this.Target)))
                {
                    this.m_TargetView = AppsContext.Instance.ApplicationSettingService.GetText(AppsContext.Instance.ApplicationService["ApplicationManagement"].Id, "Ӧ�ù���_Ӧ�����Ӵ򿪷�ʽ", this.Target);
                }
                return this.m_TargetView;
            }
        }
        #endregion

        #region ����:IconPath
        private string m_IconPath = string.Empty;

        /// <summary></summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region ����:BigIconPath
        private string m_BigIconPath = string.Empty;

        /// <summary></summary>
        public string BigIconPath
        {
            get { return m_BigIconPath; }
            set { m_BigIconPath = value; }
        }
        #endregion

        #region ����:InheritTemplate
        private bool m_InheritTemplate;

        /// <summary></summary>
        public bool InheritTemplate
        {
            get { return this.m_InheritTemplate; }
            set { this.m_InheritTemplate = value; }
        }
        #endregion

        #region ����:SidebarItemGroupTemplateId
        private string m_SidebarItemGroupTemplateId;

        /// <summary></summary>
        public string SidebarItemGroupTemplateId
        {
            get { return this.m_SidebarItemGroupTemplateId; }
            set { this.m_SidebarItemGroupTemplateId = value; }
        }
        #endregion

        #region ����:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return this.m_OrderId; }
            set { this.m_OrderId = value; }
        }
        #endregion

        #region ����:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return this.m_Status; }
            set { this.m_Status = value; }
        }
        #endregion

        #region ����:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion

        #region ����:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region ����:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ��ʽʵ�� ICacheable
        // -------------------------------------------------------

        #region ����:Expires
        private DateTime m_Expires = DateTime.MaxValue;

        /// <summary>����ʱ��</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}
