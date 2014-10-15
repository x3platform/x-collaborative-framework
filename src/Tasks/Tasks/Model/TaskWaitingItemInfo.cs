#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :TaskHistoryInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>��������������Ϣ</summary>
    public class TaskWaitingItemInfo
    {
        #region ���캯��:TaskWaitingItemInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public TaskWaitingItemInfo()
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

        #region 属性:IsSend
        private bool m_IsSend;

        /// <summary></summary>
        public bool IsSend
        {
            get { return this.m_IsSend; }
            set { this.m_IsSend = value; }
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

        #region 属性:SendTime
        private DateTime m_SendTime;

        /// <summary>ʵ�ʷ���ʱ��</summary>
        public DateTime SendTime
        {
            get { return this.m_SendTime; }
            set { this.m_SendTime = value; }
        }
        #endregion

        #region 属性:TriggerTime
        private DateTime m_TriggerTime;

        /// <summary>����ʱ��</summary>
        public DateTime TriggerTime
        {
            get { return m_TriggerTime; }
            set { m_TriggerTime = value; }
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
