namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>定时任务信息</summary>
    [Serializable]
    public class TaskWaitingInfo
    {
        /// <summary></summary>
        public TaskWaitingInfo()
        {
            this.m_TriggerTime = new DateTime(2000, 1, 1);
            this.m_CreateDate = new DateTime(2000, 1, 1);
        }

        /// <summary></summary>
        public TaskWaitingInfo(string applicationId)
            : this()
        {
            this.m_ApplicationId = applicationId;
        }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
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

        #region 属性:TaskCode
        private string m_TaskCode;

        /// <summary>任务编号</summary>
        public string TaskCode
        {
            get { return m_TaskCode; }
            set { m_TaskCode = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type;

        /// <summary>
        /// 类型 "1"表示审批类待办,点击后不会马上消失. "4"表示通知类待办,点击后马上消失.
        /// </summary>
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

        // 接收者信息

        #region 属性:ReceiverGroup
        private IList<TaskWaitingReceiverInfo> m_ReceiverGroup = null;

        /// <summary>接收者</summary>
        public IList<TaskWaitingReceiverInfo> ReceiverGroup
        {
            get
            {
                if (m_ReceiverGroup == null)
                {
                    m_ReceiverGroup = new List<TaskWaitingReceiverInfo>();
                }

                return m_ReceiverGroup;
            }
        }
        #endregion

        #region 属性:TriggerTime
        private DateTime m_TriggerTime;

        /// <summary>触发时间</summary>
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
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 获取任务工作项信息
        // -------------------------------------------------------

        /// <summary></summary>
        /// <returns></returns>
        public IList<TaskWaitingItemInfo> GetTaskWaitingItems()
        {
            IList<TaskWaitingItemInfo> list = new List<TaskWaitingItemInfo>();

            foreach (TaskWaitingReceiverInfo receiver in this.ReceiverGroup)
            {
                TaskWaitingItemInfo item = new TaskWaitingItemInfo();

                item.Id = this.Id;
                item.ApplicationId = this.ApplicationId;
                item.TaskCode = this.TaskCode;
                item.Type = this.Type;
                item.Title = this.Title;
                item.Content = this.Content;
                item.Tags = this.Tags;
                item.SenderId = this.SenderId;

                item.ReceiverId = receiver.ReceiverId;
                item.Status = 0;
                item.IsSend = false;
                item.SendTime = new DateTime(2000, 1, 1);

                item.TriggerTime = this.TriggerTime;
                item.CreateDate = this.CreateDate;

                list.Add(item);
            }

            return list;
        }

        // -------------------------------------------------------
        // 增加和移除接收者信息
        // -------------------------------------------------------

        /// <summary>增加接收者信息</summary>
        /// <param name="receiverId">接收者标识</param>
        public void AddReceiver(string receiverId)
        {
            this.AddReceiver(receiverId, false, new DateTime(2000, 1, 1));
        }

        /// <summary>增加接收者信息</summary>
        /// <param name="receiverId">接收者标识</param>
        /// <param name="isSend">是否已发送</param>
        /// <param name="sendTime">发送时间</param>
        public void AddReceiver(string receiverId, bool isSend, DateTime sendTime)
        {
            TaskWaitingReceiverInfo receiver = new TaskWaitingReceiverInfo();

            receiver.ReceiverId = receiverId;
            receiver.IsSend = isSend;
            receiver.SendTime = sendTime;

            this.ReceiverGroup.Add(receiver);
        }

        /// <summary>移除接收者信息</summary>
        /// <param name="receiverId">接收者标识</param>
        public void RemoveReceiver(string receiverId)
        {
            for (int i = 0; i < this.ReceiverGroup.Count; i++)
            {
                if (this.ReceiverGroup[i].ReceiverId == receiverId)
                {
                    this.ReceiverGroup.RemoveAt(i);
                }
            }
        }
    }
}
