﻿namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion
    
    /// <summary>任务工作项信息</summary>
    public class TaskWorkItemInfo
    {
        #region 构造函数:TaskWorkItemInfo()
        /// <summary>默认构造函数</summary>
        public TaskWorkItemInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:TrackingToken
        private string m_TrackingToken;

        /// <summary>跟踪标记</summary>
        public string TrackingToken
        {
            get { return this.m_TrackingToken; }
            set { this.m_TrackingToken = value; }
        }
        #endregion
        
        #region 属性:TaskCode
        private string m_TaskCode;

        /// <summary></summary>
        public string TaskCode
        {
            get { return this.m_TaskCode; }
            set { this.m_TaskCode = value; }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return this.m_ApplicationId; }
            set { this.m_ApplicationId = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type;

        /// <summary></summary>
        public string Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary></summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Content
        private string m_Content;

        /// <summary></summary>
        public string Content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary></summary>
        public string Tags
        {
            get { return this.m_Tags; }
            set { this.m_Tags = value; }
        }
        #endregion

        #region 属性:SenderId
        private string m_SenderId;

        /// <summary></summary>
        public string SenderId
        {
            get { return this.m_SenderId; }
            set { this.m_SenderId = value; }
        }
        #endregion

        #region 属性:ReceiverId
        private string m_ReceiverId;

        /// <summary></summary>
        public string ReceiverId
        {
            get { return this.m_ReceiverId; }
            set { this.m_ReceiverId = value; }
        }
        #endregion

        #region 属性:IsRead
        private bool m_IsRead;

        /// <summary></summary>
        public bool IsRead
        {
            get { return this.m_IsRead; }
            set { this.m_IsRead = value; }
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

        #region 属性:FinishTime
        private DateTime m_FinishTime;

        /// <summary></summary>
        public DateTime FinishTime
        {
            get { return this.m_FinishTime; }
            set { this.m_FinishTime = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
