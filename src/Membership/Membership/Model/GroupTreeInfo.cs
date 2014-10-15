// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :GroupTreeInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Membership.Model
{
    /// <summary></summary>
    public class GroupTreeInfo
    {
        #region 构造函数:GroupTreeInfo()
        /// <summary>默认构造函数</summary>
        public GroupTreeInfo() { }
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

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:DisplayType
        private string m_DisplayType;

        /// <summary></summary>
        public string DisplayType
        {
            get { return m_DisplayType; }
            set { m_DisplayType = value; }
        }
        #endregion

        #region 属性:RootTreeNodeId
        private string m_RootTreeNodeId;

        /// <summary></summary>
        public string RootTreeNodeId
        {
            get { return m_RootTreeNodeId; }
            set { m_RootTreeNodeId = value; }
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

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
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
    }
}
