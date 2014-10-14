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

namespace X3Platform.Tasks.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IBLL.ITimingTaskService")]
    public interface ITimingTaskService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        ///<param name="id">任务标识</param>
        /// <returns></returns>
        TimingTaskInfo this[string id] { get; }
        #endregion

        #region 索引:this[string applicationId, string taskCode]
        /// <summary>索引</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        /// <returns></returns>
        TimingTaskInfo this[string applicationId, string taskCode] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(TimingTaskInfo param)
        ///<summary>保存记录</summary>
        ///<param name="param"> 实例<see cref="TimingTaskInfo"/>详细信息</param>
        ///<returns>TimingTaskInfo 实例详细信息</returns>
        TimingTaskInfo Save(TimingTaskInfo param);
        #endregion

        #region 函数:Delete(string ids)
        ///<summary>删除记录</summary>
        ///<param name="ids">实例的标识信息,多个以逗号分开.</param>
        void Delete(string ids);
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode)
        ///<summary>删除记录</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        void DeleteByTaskCode(string applicationId, string taskCode);
        #endregion

        //#region 函数:DeleteEarliestTasks(int monthInterval)
        /////<summary>删除最早的任务记录</summary>
        /////<param name="monthInterval">月份范围</param>
        /////<remarks>若要删除最早三个月的任务, monthInterval = 3.</remarks>
        //void DeleteEarliestTasks(int monthInterval);
        //#endregion
        
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        ///<summary>查询某条记录</summary>
        ///<param name="id">TimingTaskInfo Id号</param>
        ///<returns>返回一个 TimingTaskInfo 实例的详细信息</returns>
        TimingTaskInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode)
        ///<summary>查询某条记录</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<returns>返回一个 TimingTaskInfo 实例的详细信息</returns>
        TimingTaskInfo FindOneByTaskCode(string applicationId, string taskCode);
        #endregion

        #region 函数:FindAll()
        ///<summary>查询所有相关记录</summary>
        ///<returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        IList<TimingTaskInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        IList<TimingTaskInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有 TimingTaskInfo 实例的详细信息</returns>
        IList<TimingTaskInfo> FindAll(string whereClause, int length);
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
        IList<TimingTaskInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        ///<summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        ///<returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistTaskCode(string applicationId, string taskCode)
        ///<summary>查询是否存在相关的记录</summary>
        ///<param name="applicationId">应用系统的标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<returns>布尔值</returns>
        bool IsExistTaskCode(string applicationId, string taskCode);
        #endregion

        #region 函数:Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId, DateTime triggerTime)
        /// <summary>发送一对一的待办信息</summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="applicationId">第三方系统帐号标识</param>
        /// <param name="title">标题</param>
        /// <param name="content">详细信息地址</param>
        /// <param name="tags">标签</param>
        /// <param name="type">类型</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverId">接收者</param>
        /// <param name="triggerTime">触发时间</param>
        void Send(string applicationId, string taskCode, string type, string title, string content, string tags, string senderId, string receiverId, DateTime triggerTime);
        #endregion

        #region 函数:SetSent(string applicationId, string taskCode)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        void SetSent(string applicationId, string taskCode);
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
        string GetIdsByTaskCodes(string applicationId,string taskCodes);
        #endregion
    }
}