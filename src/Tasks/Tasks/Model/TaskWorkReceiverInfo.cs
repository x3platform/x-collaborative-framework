namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>任务接收者</summary>
    [Serializable]
	public class TaskWorkReceiverInfo
    {
        /// <summary></summary>
		public TaskWorkReceiverInfo()
        {
            this.m_Status = 0;
            this.m_FinishTime = new DateTime(2000, 1, 1);
        }

        /// <summary></summary>
        public TaskWorkReceiverInfo(string taskId, string receiverId)
            : this()
        {
            this.m_TaskId = taskId;
            this.m_ReceiverId = receiverId;
        }

		#region 属性:TaskId
        private string m_TaskId;

		/// <summary></summary>
        public string TaskId
		{
			 get{ return m_TaskId ; }
			 set{ m_TaskId = value ; }
		}
		#endregion

		#region 属性:ReceiverId
		private string m_ReceiverId;

		/// <summary></summary>
		public string ReceiverId
		{
			 get{ return m_ReceiverId ; }
			 set{ m_ReceiverId = value ; }
		}
		#endregion

		#region 属性:Status
		private int m_Status;

		/// <summary>任务状态, 0表示未完成, 1表示完成.</summary>
		public int Status
		{
			 get{ return m_Status ; }
			 set{ m_Status = value ; }
		}
		#endregion

        #region 属性:IsRead
        private bool m_IsRead;

		/// <summary></summary>
        public bool IsRead
		{
            get { return m_IsRead; }
            set { m_IsRead = value; }
		}
		#endregion

		#region 属性:FinishTime
		private DateTime m_FinishTime;

		/// <summary></summary>
		public DateTime FinishTime
		{
			 get{ return m_FinishTime ; }
			 set{ m_FinishTime = value ; }
		}
		#endregion
	}
}
