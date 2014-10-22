namespace X3Platform.Tasks.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IDAL.ITimingTaskReceiverProvider")]
    public interface ITimingTaskReceiverProvider
    {
        #region 函数:FindOne(string taskId, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TimingTaskReceiverInfo 实例的详细信息</returns>
        TaskWaitingItemInfo FindOne(string taskId, string receiverId);
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">延迟的接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TimingTaskReceiverInfo 实例的详细信息</returns>
        IList<TaskWaitingItemInfo> FindAllByReceiverId(string receiverId, string whereClause);
        #endregion

        #region 函数:GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        IList<TaskWaitingItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string timingTaskId, string receiverId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="timingTaskId">任务标识</param>
        /// <param name="receiverId">接收者标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string timingTaskId, string receiverId);
        #endregion

        #region 函数:SetSent(string receiverId, string taskIds)
        /// <summary>
        /// 设置任务完成
        /// </summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        void SetSent(string receiverId, string taskIds);
        #endregion

        #region 函数:SetSentByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        void SetSentByTaskCode(string applicationId, string taskCodes, string receiverId);
        #endregion

        #region 函数:SetUnsent(string receiverId, string taskIds)
        /// <summary>
        /// 设置任务未完成
        /// </summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        void SetUnsent(string receiverId, string taskIds);
        #endregion

        #region 函数:SetUnsentByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>设置任务未完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        void SetUnsentByTaskCode(string applicationId, string taskCodes, string receiverId);
        #endregion
    }
}