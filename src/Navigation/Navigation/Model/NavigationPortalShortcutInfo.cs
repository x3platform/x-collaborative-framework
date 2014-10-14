#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalShortcutInfo.cs
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
    public class NavigationPortalShortcutInfo : ICacheable
    {
        #region ���캯��:NavigationPortalShortcutInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public NavigationPortalShortcutInfo()
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

        #region ����:ShortcutGroupId
        private string m_ShortcutGroupId;

        /// <summary></summary>
        public string ShortcutGroupId
        {
            get { return this.m_ShortcutGroupId; }
            set { this.m_ShortcutGroupId = value; }
        }
        #endregion

        #region ����:ShortcutGroupName
        private string m_ShortcutGroupName;

        /// <summary></summary>
        public string ShortcutGroupName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ShortcutGroupId))
                {
                    NavigationPortalShortcutGroupInfo group = NavigationContext.Instance.NavigationPortalShortcutGroupService.FindOne(this.ShortcutGroupId);
                    if (group != null)
                    {
                        this.m_ShortcutGroupName = group.Text;
                    }
                }
                return this.m_ShortcutGroupName;
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
