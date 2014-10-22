namespace X3Platform.Tasks.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.CacheBuffer;
    using X3Platform.Membership;
    using X3Platform.Messages;
    using X3Platform.Util;
    #endregion

    /// <summary>任务信息</summary>
    [Serializable]
    public class TaskInfo : IMessageObject, ISerializedObject, ICacheable
    {
        #region 构造函数:TaskInfo()
        /// <summary>默认构造函数</summary>
        public TaskInfo()
        {
            this.m_CreateDate = DateTime.Now;
        }
        #endregion

        #region 构造函数:TaskInfo(string applicationId)
        /// <summary>默认构造函数</summary>
        public TaskInfo(string applicationId)
            : this()
        {
            this.m_ApplicationId = applicationId;
        }
        #endregion

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
        private IList<TaskReceiverInfo> m_ReceiverGroup = null;

        /// <summary>接收者</summary>
        public IList<TaskReceiverInfo> ReceiverGroup
        {
            get
            {
                if (m_ReceiverGroup == null)
                {
                    m_ReceiverGroup = new List<TaskReceiverInfo>();
                }

                return m_ReceiverGroup;
            }
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
        // 显式实现 ICacheable
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(6);

        /// <summary>过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion

        // -------------------------------------------------------
        // Xml 元素的导入和导出 
        // -------------------------------------------------------

        #region 函数:Deserialize(XmlElement element)
        /// <summary>根据Xml元素加载对象</summary>
        /// <param name="element">Xml元素</param>
        public void Deserialize(XmlElement element)
        {
            StringBuilder outString = new StringBuilder();

            this.ApplicationId = element.SelectSingleNode("applicationId").InnerText;
            this.TaskCode = element.SelectSingleNode("taskCode").InnerText;

            // 设置任务标识
            this.Id = TasksContext.Instance.TaskService.GetIdsByTaskCodes(this.ApplicationId, this.TaskCode);

            if (string.IsNullOrEmpty(this.Id))
            {
                this.Id = StringHelper.ToGuid();
            }

            this.Type = element.SelectSingleNode("type").InnerText;
            this.Title = element.SelectSingleNode("title").InnerText;
            this.Content = element.SelectSingleNode("content").InnerText;
            this.Tags = element.SelectSingleNode("tags").InnerText;

            // 设置发送人

            IAccountInfo account = null;

            account = MembershipManagement.Instance.AccountService[element.SelectSingleNode("sender").InnerText];

            this.SenderId = (account == null) ? element.SelectSingleNode("sender").InnerText + "(unkown)" : account.Id;

            // 设置接收人

            outString.Append("<receiver>");

            TaskReceiverInfo item = null;

            string[] keys = element.SelectSingleNode("receiver").InnerText.Split(',');

            string[] temp;

            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key) && key.Contains("#"))
                {
                    temp = key.Split('#');

                    item = new TaskReceiverInfo();
                    item.TaskId = this.Id;
                    item.Status = Convert.ToInt32(temp[0]);

                    account = MembershipManagement.Instance.AccountService[temp[1]];

                    item.ReceiverId = (account == null) ? temp[1] : account.Id;

                    // 1 代表任务完 , 插入任务完成时间.
                    if (item.Status == 1)
                    {
                        item.FinishTime = Convert.ToDateTime(temp[2]);
                    }

                    this.ReceiverGroup.Add(item);
                }
            }

            this.CreateDate = Convert.ToDateTime(element.SelectSingleNode("createDate").InnerText);
        }
        #endregion

        #region 函数:Serializable()
        /// <summary>根据对象导出Xml元素</summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>根据对象导出Xml元素</summary>
        /// <param name="displayComment">显示注释</param>
        /// <param name="displayFriendlyName">显示友好名称</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<task>");
            outString.AppendFormat("<applicationId><![CDATA[{0}]]></applicationId>", this.ApplicationId);
            outString.AppendFormat("<taskCode><![CDATA[{0}]]></taskCode>", this.TaskCode);
            outString.AppendFormat("<title><![CDATA[{0}]]></title>", this.Title);
            outString.AppendFormat("<type><![CDATA[{0}]]></type>", this.Type);
            outString.AppendFormat("<content><![CDATA[{0}]]></content>", this.Content);
            outString.AppendFormat("<tags><![CDATA[{0}]]></tags>", this.Tags);
            outString.AppendFormat("<sender><![CDATA[{0}]]></sender>", this.SenderId);

            outString.Append("<receiver>");

            foreach (TaskReceiverInfo item in this.ReceiverGroup)
            {
                if (item.Status == 1)
                {
                    outString.AppendFormat("{0}#{1}#{2},", item.Status, item.ReceiverId, item.FinishTime);
                }
                else
                {
                    outString.AppendFormat("{0}#{1},", item.Status, item.ReceiverId);
                }
            }

            outString.Append("</receiver>");

            outString.AppendFormat("<createDate><![CDATA[{0}]]></createDate>", this.CreateDate);

            outString.Append("</task>");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetTaskWorkItems()
        /// <summary>获取任务工作项信息列表</summary>
        /// <returns></returns>
        public IList<TaskWorkItemInfo> GetTaskWorkItems()
        {
            IList<TaskWorkItemInfo> list = new List<TaskWorkItemInfo>();

            foreach (TaskReceiverInfo receiver in this.ReceiverGroup)
            {
                TaskWorkItemInfo item = new TaskWorkItemInfo();

                item.Id = this.Id;
                item.ApplicationId = this.ApplicationId;
                item.TaskCode = this.TaskCode;
                item.Type = this.Type;
                item.Title = this.Title;
                item.Content = this.Content;
                item.Tags = this.Tags;
                item.SenderId = this.SenderId;

                item.ReceiverId = receiver.ReceiverId;
                item.Status = receiver.Status;
                item.IsRead = receiver.IsRead;
                item.FinishTime = receiver.FinishTime;
                
                item.CreateDate = this.CreateDate;

                list.Add(item);
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 增加和移除接收者信息
        // -------------------------------------------------------

        /// <summary>增加接收者信息</summary>
        /// <param name="receiverId">接收者标识</param>
        public void AddReceiver(string receiverId)
        {
            this.AddReceiver(receiverId, false, 0, new DateTime(2000, 1, 1));
        }

        /// <summary>增加接收者信息</summary>
        /// <param name="receiverId">接收者标识</param>
        /// <param name="isRead">是否已读</param>
        /// <param name="status">状态</param>
        /// <param name="finishTime">完成时间</param>
        public void AddReceiver(string receiverId, bool isRead, int status, DateTime finishTime)
        {
            TaskReceiverInfo receiver = new TaskReceiverInfo();

            receiver.ReceiverId = receiverId;
            receiver.Status = status;
            receiver.IsRead = isRead;
            receiver.FinishTime = finishTime;
           
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
