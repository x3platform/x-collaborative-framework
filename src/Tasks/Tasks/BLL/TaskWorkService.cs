namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Configuration;
    using X3Platform.Messages;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.MSMQ;
    using X3Platform.Json;
    #endregion

    /// <summary>任务服务</summary>
    public class TaskWorkService : ITaskWorkService
    {
        /// <summary>数据提供器</summary>
        private ITaskWorkProvider provider = null;

        /// <summary>提醒程序</summary>
        private Dictionary<string, INotificationProvider> notifications = new Dictionary<string, INotificationProvider>();

        /// <summary></summary>
        private Dictionary<string, TaskWorkInfo> dictionary = new Dictionary<string, TaskWorkInfo>();

        /// <summary>任务队列</summary>
        private TaskQueue queue = new TaskQueue();

        #region 构造函数:TaskWorkService()
        /// <summary>构造函数:TaskWorkService()</summary>
        public TaskWorkService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = TasksConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ITaskWorkProvider>(typeof(ITaskWorkProvider));

            // 创建提醒程序
            var list = TasksConfigurationView.Instance.Configuration.Notifications;

            for (var i = 0; i < list.AllKeys.Length; i++)
            {
                var type = list[list.AllKeys[i]].Value;

                if (!string.IsNullOrEmpty(type))
                {
                    var obj = KernelContext.CreateObject(type);

                    if (obj != null && obj is INotificationProvider)
                    {
                        this.notifications.Add(list.AllKeys[i], (INotificationProvider)obj);
                    }
                }
            }
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">任务标识</param>
        /// <returns></returns>
        public TaskWorkInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        #region 索引:this[string applicationId, string taskCode]
        /// <summary>索引</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns></returns>
        public TaskWorkInfo this[string applicationId, string taskCode]
        {
            get { return this.FindOneByTaskCode(applicationId, taskCode); }
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(TaskWorkInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="TaskWorkInfo"/>详细信息</param>
        /// <returns>TaskWorkInfo 实例详细信息</returns>
        public TaskWorkInfo Save(TaskWorkInfo param)
        {
            if (string.IsNullOrEmpty(param.ApplicationId))
            {
                throw new ArgumentException("没有应用标识信息，请填写此任务所属的应用标识。");
            }

            if (string.IsNullOrEmpty(param.TaskCode))
            {
                throw new ArgumentException("没有任务编码信息，请填写此任务的编码。");
            }

            if (string.IsNullOrEmpty(param.Title))
            {
                throw new ArgumentException("没有任务标题信息，请填写此任务的标题。");
            }

            if (param.ReceiverGroup.Count == 0)
            {
                throw new ArgumentException("没有任何接收人数据，请确认此任务信息的接收人数据。");
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = StringHelper.ToGuid();
            }

            // 设置待办信息内容 
            param.Content = param.Content.Trim();

            if (param.Content.IndexOf("http://") == 0 || param.Content.IndexOf("https://") == 0)
            {
                // 内容为完整的Url地址的数据不处理。
            }
            else
            {
                string prefixTargetUrl = TasksConfigurationView.Instance.PrefixTargetUrl;

                if (!string.IsNullOrEmpty(prefixTargetUrl))
                {
                    prefixTargetUrl = prefixTargetUrl.TrimEnd(new char[] { '/' });

                    // 第一个字符不为 / 。
                    if (param.Content.IndexOf("/") != 0)
                    {
                        param.Content = "/" + param.Content;
                    }

                    param.Content = prefixTargetUrl + param.Content;
                }
            }

            param = StringHelper.ToSafeXSS<TaskWorkInfo>(param);

            // 如果消息队列可用，则将数据发送到队列。
            if (TasksConfigurationView.Instance.MessageQueueMode == "ON" && queue.Enabled)
            {
                queue.Send(param);

                return param;
            }
            else
            {
                return this.provider.Save(param);
            }
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识信息,多个以逗号分开.</param>
        public void Delete(string ids)
        {
            string[] keys = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in keys)
            {
                if (dictionary.ContainsKey(key))
                {
                    dictionary.Remove(key);
                }
            }

            provider.Delete(ids);
        }
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode)
        /// <summary>删除记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        public void DeleteByTaskCode(string applicationId, string taskCode)
        {
            TaskWorkInfo param = FindOneByTaskCode(applicationId, taskCode);

            if (param != null)
            {
                if (dictionary.ContainsKey(param.Id))
                {
                    dictionary.Remove(param.Id);
                }

                provider.DeleteByTaskCode(applicationId, taskCode);
            }
        }
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode, string receiverIds)
        /// <summary>删除记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <param name="receiverIds">任务接收人标识</param>
        public void DeleteByTaskCode(string applicationId, string taskCode, string receiverIds)
        {
            TaskWorkInfo param = this.FindOneByTaskCode(applicationId, taskCode);

            if (param != null)
            {
                if (dictionary.ContainsKey(param.Id))
                {
                    dictionary.Remove(param.Id);
                }

                this.provider.DeleteByTaskCode(applicationId, taskCode, receiverIds);
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">任务标识</param>
        /// <returns>返回一个 TaskWorkInfo 实例的详细信息</returns>
        public TaskWorkInfo FindOne(string id)
        {
            TaskWorkInfo param = null;

            if (dictionary.ContainsKey(id))
            {
                param = dictionary[id];
            }
            else
            {
                param = provider.FindOne(id);

                if (param != null)
                {
                    dictionary.Add(param.Id, param);
                }
            }

            return param;
        }
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>返回一个 TaskWorkInfo 实例的详细信息</returns>
        public TaskWorkInfo FindOneByTaskCode(string applicationId, string taskCode)
        {
            TaskWorkInfo param = this.provider.FindOneByTaskCode(applicationId, taskCode);

            if (param != null)
            {
                param = this[param.Id];
            }

            return param;
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 TaskWorkInfo 实例的详细信息</returns>
        public IList<TaskWorkItemInfo> FindAll()
        {
            return FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TaskWorkInfo 实例的详细信息</returns>
        public IList<TaskWorkItemInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskWorkInfo 实例的详细信息</returns>
        public IList<TaskWorkItemInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="TaskWorkInfo"/></returns>
        public IList<TaskWorkItemInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistTaskCode(string applicationId, string taskCode)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>布尔值</returns>
        public bool IsExistTaskCode(string applicationId, string taskCode)
        {
            return this.provider.IsExistTaskCode(applicationId, taskCode);
        }
        #endregion

        #region 函数:Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId)
        /// <summary>发送一对一的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细信息地址</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverId">接收者</param>
        public void Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId)
        {
            this.Send(applicationId, taskCode, type, title, content, tags, senderId, receiverId, TasksConfigurationView.Instance.NotificationOptions);
        }
        #endregion

        #region 函数:Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId, string notificationOptions)
        /// <summary>发送一对一的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细信息地址</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverId">接收者</param>
        /// <param name="notificationOptions">通知选项</param>
        public void Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId, string notificationOptions)
        {
            TaskWorkInfo task = new TaskWorkInfo();

            task.Id = StringHelper.ToGuid();

            task.ApplicationId = applicationId;
            task.TaskCode = taskCode;
            task.Title = title;
            task.Content = content;
            task.Type = type;
            task.Tags = tags;
            task.SenderId = senderId;

            if (!string.IsNullOrEmpty(receiverId))
            {
                task.AddReceiver(receiverId);
            }

            task.CreateDate = DateTime.Now;

            this.Save(task);

            // 发送通知
            this.Notification(task, receiverId, notificationOptions);
        }
        #endregion

        #region 函数:SendRange(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverIds)
        /// <summary>发送一对多的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细信息地址</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverIds">接收者</param>
        public void SendRange(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverIds)
        {
            this.SendRange(applicationId, taskCode, type, title, content, tags, senderId, receiverIds, TasksConfigurationView.Instance.NotificationOptions);
        }
        #endregion

        #region 函数:SendRange(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverIds, string notificationOptions)
        /// <summary>发送一对多的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细信息地址</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="notificationOptions">通知选项</param>
        public void SendRange(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverIds, string notificationOptions)
        {
            TaskWorkInfo task = new TaskWorkInfo();

            task.Id = StringHelper.ToGuid();

            task.ApplicationId = applicationId;
            task.TaskCode = taskCode;

            task.Title = title;
            task.Content = content;
            task.Type = type;
            task.Tags = tags;
            task.SenderId = senderId;

            string[] keys = receiverIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    task.AddReceiver(key);
                }
            }

            task.CreateDate = DateTime.Now;

            this.Save(task);

            // 发送通知
            this.Notification(task, receiverIds, notificationOptions);
        }
        #endregion

        #region 函数:SendAppendRange(string applicationId, string taskCode, string receiverIds)
        /// <summary>附加待办信息新的接收人</summary>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="taskCode">任务编号</param>
        /// <param name="receiverIds">接收者</param>
        public void SendAppendRange(string applicationId, string taskCode, string receiverIds)
        {
            this.SendAppendRange(applicationId, taskCode, receiverIds, TasksConfigurationView.Instance.NotificationOptions);
        }
        #endregion

        #region 函数:SendAppendRange(string applicationId, string taskCode, string receiverIds, string notificationOptions)
        /// <summary>附加待办信息新的接收人</summary>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="taskCode">任务编号</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="notificationOptions">通知选项</param>
        public void SendAppendRange(string applicationId, string taskCode, string receiverIds, string notificationOptions)
        {
            TaskWorkInfo task = this.FindOneByTaskCode(applicationId, taskCode);

            if (task == null) { throw new Exception("【ApplicationId " + applicationId + " - TaskCode " + taskCode + "】任务不存在。"); }

            string[] keys = receiverIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    task.AddReceiver(key);
                }
            }

            task.CreateDate = DateTime.Now;

            this.provider.Insert(task);

            // 发送通知
            this.Notification(task, receiverIds, notificationOptions);
        }
        #endregion

        #region 函数:Notification(TaskWorkInfo task, string receiverIds, string notificationOptions)
        /// <summary>发送通知</summary>
        /// <param name="task">任务信息</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="notificationOptions">通知选项</param>
        public void Notification(TaskWorkInfo task, string receiverIds, string notificationOptions)
        {
            JsonData data = JsonMapper.ToObject(notificationOptions);

            foreach (string key in data.Keys)
            {
                if (this.notifications.ContainsKey(key))
                {
                    this.notifications[key].Send(task, receiverIds, data[key].ToJson());
                }
            }
        }
        #endregion

        #region 函数:AsyncReceive()
        /// <summary>异步接收待办信息</summary>
        public void AsyncReceive()
        {
            IMessageObject message = this.queue.Receive();

            while (message != null)
            {
                if (message is TaskWorkInfo)
                {
                    try
                    {
                        this.provider.Save((TaskWorkInfo)message);
                    }
                    catch (Exception ex)
                    {
                        this.queue.Send(message);

                        throw ex;
                    }
                }

                message = this.queue.Receive();
            }
        }
        #endregion

        #region 函数:SetTitle(string applicationId, string taskCode, string title)
        /// <summary>设置任务标题</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        /// <param name="title">任务标题</param>
        public void SetTitle(string applicationId, string taskCode, string title)
        {
            this.provider.SetTitle(applicationId, taskCode, title);
        }
        #endregion

        #region 函数:SetFinished(string applicationId, string taskCode)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        public void SetFinished(string applicationId, string taskCode)
        {
            this.provider.SetFinished(applicationId, taskCode);
        }
        #endregion

        #region 函数:GetTaskTags()
        /// <summary></summary>
        /// <returns></returns>
        public IList<string> GetTaskTags()
        {
            return this.provider.GetTaskTags(string.Empty);
        }
        #endregion

        #region 函数:GetTaskTags(string key)
        /// <summary></summary>
        public IList<string> GetTaskTags(string key)
        {
            return this.provider.GetTaskTags(key);
        }
        #endregion

        #region 函数:GetIdsByTaskCodes(string applicationId,string taskCodes)
        /// <summary>将任务编号转换为标识信息</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        public string GetIdsByTaskCodes(string applicationId, string taskCodes)
        {
            return this.provider.GetIdsByTaskCodes(applicationId, taskCodes);
        }
        #endregion

        // -------------------------------------------------------
        // 归档、删除某一时段的待办记录
        // -------------------------------------------------------

        #region 函数:Archive()
        /// <summary>将已完成的待办归档到历史数据表</summary>
        public int Archive()
        {
            return this.Archive(DateTime.Now);
        }
        #endregion

        #region 函数:Archive(DateTime archiveDate)
        /// <summary>将归档日期之前已完成的待办归档到历史数据表</summary>
        /// <param name="archiveDate">归档日期</param>
        public int Archive(DateTime archiveDate)
        {
            return this.provider.Archive(archiveDate);
        }
        #endregion

        #region 函数:RemoveUnfinishedWorkItems(DateTime expireDate)
        /// <summary>删除过期时间之前未完成的工作项记录</summary>
        /// <param name="expireDate">过期时间</param>
        public void RemoveUnfinishedWorkItems(DateTime expireDate)
        {
            this.provider.RemoveUnfinishedWorkItems(expireDate);
        }
        #endregion

        #region 函数:RemoveWorkItems(DateTime expireDate)
        /// <summary>删除过期时间之前的工作项记录</summary>
        /// <param name="expireDate">过期时间</param>
        public void RemoveWorkItems(DateTime expireDate)
        {
            this.provider.RemoveWorkItems(expireDate);
        }
        #endregion

        #region 函数:RemoveHistoryItems(DateTime expireDate)
        /// <summary>删除过期时间之前的历史记录</summary>
        /// <param name="expireDate">过期时间</param>
        public void RemoveHistoryItems(DateTime expireDate)
        {
            this.provider.RemoveHistoryItems(expireDate);
        }
        #endregion
    }
}
