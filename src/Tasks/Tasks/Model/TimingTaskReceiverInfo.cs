namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>定时任务接收者</summary>
    [Serializable]
    public class TimingTaskReceiverInfo
    {
        /// <summary></summary>
        public TimingTaskReceiverInfo()
        {
            this.m_SendTime = new DateTime(2000, 1, 1);
        }

        /// <summary></summary>
        public TimingTaskReceiverInfo(string timingTaskId, string receiverId)
            : this()
        {
            this.m_TimingTaskId = timingTaskId;
            this.m_ReceiverId = receiverId;
        }

        #region 属性:TimingTaskId
        private string m_TimingTaskId;

        /// <summary></summary>
        public string TimingTaskId
        {
            get { return m_TimingTaskId; }
            set { m_TimingTaskId = value; }
        }
        #endregion

        private TimingTaskInfo m_TimingTask;

        /// <summary>任务</summary>
        public TimingTaskInfo TimingTask
        {
            get { return m_TimingTask; }
            set { m_TimingTask = value; }
        }

        #region 属性:ReceiverId
        private string m_ReceiverId;

        /// <summary></summary>
        public string ReceiverId
        {
            get { return m_ReceiverId; }
            set { m_ReceiverId = value; }
        }
        #endregion

        #region 属性:IsSend
        private bool m_IsSend;

        /// <summary></summary>
        public bool IsSend
        {
            get { return m_IsSend; }
            set { m_IsSend = value; }
        }
        #endregion

        #region 属性:SendTime
        private DateTime m_SendTime;

        /// <summary>发送时间</summary>
        public DateTime SendTime
        {
            get { return m_SendTime; }
            set { m_SendTime = value; }
        }
        #endregion

        //
        // 任务的信息
        //

        #region 属性:TaskCode
        private string m_TaskCode;

        /// <summary></summary>
        public string TaskCode
        {
            get { return m_TaskCode; }
            set { m_TaskCode = value; }
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

        #region 属性:Type
        private string m_Type;

        /// <summary>类型 "1"表示审批类待办,点击后不会马上消失. "4"表示通知类待办,点击后马上消失.</summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary></summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        #endregion

        #region 属性:Content
        private string m_Content;

        /// <summary></summary>
        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary></summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:SenderId
        private string m_SenderId;

        /// <summary></summary>
        public string SenderId
        {
            get { return m_SenderId; }
            set { m_SenderId = value; }
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
