#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Configuration;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary>定时任务服务</summary>
    public class TimingTaskService : ContextBoundObject, ITimingTaskService
    {
        private TasksConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private ITimingTaskProvider provider = null;

        #region 构造函数:TaskService()
        /// <summary>构造函数:TaskService()</summary>
        public TimingTaskService()
        {
            configuration = TasksConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<ITimingTaskProvider>(typeof(ITimingTaskProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        ///<param name="id">任务标识</param>
        /// <returns></returns>
        public TimingTaskInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        #region 索引:this[string applicationId, string taskCode]
        /// <summary>索引</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        /// <returns></returns>
        public TimingTaskInfo this[string applicationId, string taskCode]
        {
            get { return this.FindOneByTaskCode(applicationId, taskCode); }
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(TimingTaskInfo param)
        ///<summary>保存记录</summary>
        ///<param name="param"> 实例<see cref="TimingTaskInfo"/>详细信息</param>
        ///<returns>TimingTaskInfo 实例详细信息</returns>
        public TimingTaskInfo Save(TimingTaskInfo param)
        {
            if (string.IsNullOrEmpty(param.ApplicationId))
            {
                throw new ArgumentException("ApplicationId is null.");
            }

            if (string.IsNullOrEmpty(param.TaskCode))
            {
                throw new ArgumentException("TaskCode is null.");
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = StringHelper.ToGuid();
            }

            param = StringHelper.ToSafeXSS<TimingTaskInfo>(param);

            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode)
        ///<summary>删除记录</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        public void DeleteByTaskCode(string applicationId, string taskCode)
        {
            this.provider.DeleteByTaskCode(applicationId, taskCode);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        ///<summary>查询某条记录</summary>
        ///<param name="id">任务标识</param>
        ///<returns>返回一个 TimingTaskInfo 实例的详细信息</returns>
        public TimingTaskInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode)
        ///<summary>查询某条记录</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<returns>返回一个 TimingTaskInfo 实例的详细信息</returns>
        public TimingTaskInfo FindOneByTaskCode(string applicationId, string taskCode)
        {
            TimingTaskInfo param = this.provider.FindOneByTaskCode(applicationId, taskCode);

            if (param != null)
            {
                param = this[param.Id];
            }

            return param;
        }
        #endregion

        #region 函数:FindAll()
        ///<summary>查询所有相关记录</summary>
        ///<returns>返回所有 TimingTaskInfo 实例的详细信息</returns>
        public IList<TimingTaskInfo> FindAll()
        {
            return FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<returns>返回所有 TimingTaskInfo 实例的详细信息</returns>
        public IList<TimingTaskInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有 TimingTaskInfo 实例的详细信息</returns>
        public IList<TimingTaskInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="TimingTaskInfo"/></returns>
        public IList<TimingTaskInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        ///<summary>查询是否存在相关的记录</summary>
        ///<param name="id">标识</param>
        ///<returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistTaskCode(string applicationId, string taskCode)
        ///<summary>查询是否存在相关的记录</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<returns>布尔值</returns>
        public bool IsExistTaskCode(string applicationId, string taskCode)
        {
            return this.provider.IsExistTaskCode(applicationId, taskCode);
        }
        #endregion

        #region 函数:Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId, DateTime triggerTime)
        /// <summary>发送一对一的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细内容</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverId">接收者</param>
        /// <param name="triggerTime">触发时间</param>
        public void Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId, DateTime triggerTime)
        {
            TimingTaskInfo task = new TimingTaskInfo();

            task.Id = StringHelper.ToGuid();

            task.ApplicationId = applicationId;
            task.TaskCode = taskCode;
            task.Title = title;
            task.Content = content;
            task.Type = type;
            task.Tags = tags;
            task.SenderId = senderId;

            task.AddReceiver(receiverId);

            task.CreateDate = DateTime.Now;

            // 触发发送定时任务信息库到任务信息库的时间.
            task.TriggerTime = triggerTime;

            Save(task);
        }
        #endregion

        #region 函数:SetSent(string applicationId, string taskCode)
        /// <summary>设置任务已发送完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        public void SetSent(string applicationId, string taskCode)
        {
            this.provider.SetSent(applicationId, taskCode);
        }
        #endregion

        #region 函数:GetTaskTags()
        /// <summary></summary>
        public IList<string> GetTaskTags()
        {
            return this.provider.GetTaskTags(string.Empty);
        }
        #endregion

        #region 函数:GetTaskTags(string key)
        /// <summary></summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<string> GetTaskTags(string key)
        {
            return this.provider.GetTaskTags(key);
        }
        #endregion

        #region 函数:GetIdsByTaskCodes(string applicationId, string taskCodes)
        /// <summary>将任务编号转换为标识信息</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        public string GetIdsByTaskCodes(string applicationId, string taskCodes)
        {
            return this.provider.GetIdsByTaskCodes(applicationId, taskCodes);
        }
        #endregion
    }
}
