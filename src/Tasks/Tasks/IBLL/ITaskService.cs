namespace X3Platform.Tasks.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IBLL.ITaskService")]
    public interface ITaskService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">任务标识</param>
        /// <returns></returns>
        TaskInfo this[string id] { get; }
        #endregion

        #region 索引:this[string applicationId, string taskCode]
        /// <summary>索引</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns></returns>
        TaskInfo this[string applicationId, string taskCode] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(TaskInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="TaskInfo"/>详细信息</param>
        /// <returns>TaskInfo 实例详细信息</returns>
        TaskInfo Save(TaskInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识信息,多个以逗号分开.</param>
        void Delete(string ids);
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode)
        /// <summary>删除记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        void DeleteByTaskCode(string applicationId, string taskCode);
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode, string receiverIds)
        /// <summary>删除记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <param name="receiverIds">任务接收人标识</param>
        void DeleteByTaskCode(string applicationId, string taskCode, string receiverIds);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">TaskInfo Id号</param>
        /// <returns>返回一个 TaskInfo 实例的详细信息</returns>
        TaskInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>返回一个 TaskInfo 实例的详细信息</returns>
        TaskInfo FindOneByTaskCode(string applicationId, string taskCode);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 TaskInfo 实例的详细信息</returns>
        IList<TaskWorkItemInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TaskInfo 实例的详细信息</returns>
        IList<TaskWorkItemInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskInfo 实例的详细信息</returns>
        IList<TaskWorkItemInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        IList<TaskWorkItemInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistTaskCode(string applicationId, string taskCode)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>布尔值</returns>
        bool IsExistTaskCode(string applicationId, string taskCode);
        #endregion

        #region 函数:Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId)
        /// <summary>发送一对一的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细信息地址</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者帐号标识</param>
        /// <param name="receiverId">接收者帐号标识</param>
        void Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId);
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
        void SendRange(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverIds);
        #endregion

        #region 函数:SendAppendRange(string applicationId, string taskCode, string receiverIds)
        /// <summary>附加待办信息新的接收人</summary>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="taskCode">任务编号</param>
        /// <param name="receiverIds">接收者</param>
        void SendAppendRange(string applicationId, string taskCode, string receiverIds);
        #endregion

        #region 函数:AsyncReceive()
        /// <summary>异步接收待办信息</summary>
        void AsyncReceive();
        #endregion

        #region 函数:SetTitle(string applicationId, string taskCode, string title)
        /// <summary>设置任务标题</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        /// <param name="title">任务标题</param>
        void SetTitle(string applicationId, string taskCode, string title);
        #endregion

        #region 函数:SetFinished(string applicationId, string taskCode)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        void SetFinished(string applicationId, string taskCode);
        #endregion

        #region 函数:GetTaskTags()
        /// <summary>获取任务标签列表</summary>
        IList<string> GetTaskTags();
        #endregion

        #region 函数:GetTaskTags(string key)
        /// <summary>获取任务标签列表</summary>
        /// <param name="key">匹配标签的关键字, 空值则全部匹配</param>
        IList<string> GetTaskTags(string key);
        #endregion

        #region 函数:GetIdsByTaskCodes(string applicationId,string taskCodes)
        /// <summary>将任务编号转换为标识信息</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        string GetIdsByTaskCodes(string applicationId, string taskCodes);
        #endregion

        // -------------------------------------------------------
        // 归档、删除某一时段的待办记录
        // -------------------------------------------------------

        #region 函数:Archive()
        /// <summary>将已完成的待办归档到历史数据表</summary>
        int Archive();
        #endregion

        #region 函数:Archive(DateTime archiveDate)
        /// <summary>将归档日期之前已完成的待办归档到历史数据表</summary>
        /// <param name="archiveDate">归档日期</param>
        int Archive(DateTime archiveDate);
        #endregion

        #region 函数:RemoveUnfinishedWorkItems(DateTime expireDate)
        /// <summary>删除过期时间之前未完成的工作项记录</summary>
        /// <param name="expireDate">过期时间</param>
        void RemoveUnfinishedWorkItems(DateTime expireDate);
        #endregion

        #region 函数:RemoveWorkItems(DateTime expireDate)
        /// <summary>删除过期时间之前的工作项记录</summary>
        /// <param name="expireDate">过期时间</param>
        void RemoveWorkItems(DateTime expireDate);
        #endregion

        #region 函数:RemoveHistoryItems(DateTime expireDate)
        /// <summary>删除过期时间之前的历史记录</summary>
        /// <param name="expireDate">过期时间</param>
        void RemoveHistoryItems(DateTime expireDate);
        #endregion
    }
}