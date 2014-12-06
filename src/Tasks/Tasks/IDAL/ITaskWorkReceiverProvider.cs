namespace X3Platform.Tasks.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IDAL.ITaskWorkReceiverProvider")]
    public interface ITaskWorkReceiverProvider
    {
        #region 函数:FindOne(string taskId, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TaskWorkItemInfo 实例的详细信息</returns>
        TaskWorkItemInfo FindOne(string taskId, string receiverId);
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TaskWorkReceiverInfo 实例的详细信息</returns>
        TaskWorkItemInfo FindOneByTaskCode(string applicationId, string taskCode, string receiverId);
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TaskWorkItemInfo 实例的详细信息</returns>
        IList<TaskWorkItemInfo> FindAllByReceiverId(string receiverId, string whereClause);
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause, string length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>  
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskWorkItemInfo 实例的详细信息</returns>
        IList<TaskWorkItemInfo> FindAllByReceiverId(string receiverId, string whereClause, int length);
        #endregion

        #region 函数:GetPaging(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="TaskWorkItemInfo"/></returns>
        IList<TaskWorkItemInfo> GetPaging(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string taskId, string receiverId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string taskId, string receiverId);
        #endregion

        #region 函数:Copy(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        /// <summary>复制待办信息</summary>
        /// <param name="fromReceiverId">待办来源接收者标识</param>
        /// <param name="toReceiverId">待办目标接收者标识</param>
        /// <param name="beginDate">复制待办的开始时间</param>
        /// <param name="endDate">复制待办结束时间</param>
        /// <returns></returns>
        int Copy(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:Copy(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        /// <summary>复制待办信息</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <param name="fromReceiverId">待办来源接收者标识</param>
        /// <param name="toReceiverId">待办目标接收者标识</param>
        /// <param name="beginDate">复制待办的开始时间</param>
        /// <param name="endDate">复制待办结束时间</param>
        /// <returns></returns>
        int Copy(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:Cut(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        /// <summary>剪切待办信息</summary>
        /// <param name="fromReceiverId">待办来源接收者标识</param>
        /// <param name="toReceiverId">待办目标接收者标识</param>
        /// <param name="beginDate">复制待办的开始时间</param>
        /// <param name="endDate">复制待办结束时间</param>
        /// <returns></returns>
        int Cut(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:Cut(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        /// <summary>剪切待办信息</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <param name="fromReceiverId">待办来源接收者标识</param>
        /// <param name="toReceiverId">待办目标接收者标识</param>
        /// <param name="beginDate">复制待办的开始时间</param>
        /// <param name="endDate">复制待办结束时间</param>
        /// <returns></returns>
        int Cut(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:SetRead(string taskId, string receiverId, bool isRead)
        /// <summary>设置任务阅读状态</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者的用户名</param>
        /// <param name="isRead">状态: true 已读 | false 未读 </param>
        void SetRead(string taskId, string receiverId, bool isRead);
        #endregion

        #region 函数:SetStatus(string taskId, string receiverId, int status)
        /// <summary>强制设置任务状态</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者的用户名</param>
        /// <param name="status">状态</param>
        void SetStatus(string taskId, string receiverId, int status);
        #endregion

        #region 函数:SetFinished(string receiverId)
        /// <summary>设置某个用户的任务全部完成</summary>
        /// <param name="receiverId">接收者</param>
        void SetFinished(string receiverId);
        #endregion

        #region 函数:SetFinished(string receiverId, string taskIds)
        /// <summary>
        /// 设置任务完成
        /// </summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        void SetFinished(string receiverId, string taskIds);
        #endregion

        #region 函数:SetFinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        void SetFinishedByTaskCode(string applicationId, string taskCodes, string receiverId);
        #endregion

        #region 函数:SetUnfinished(string receiverId, string taskIds)
        /// <summary>
        /// 设置任务未完成
        /// </summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        void SetUnfinished(string receiverId, string taskIds);
        #endregion

        #region 函数:SetUnfinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>设置任务未完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        void SetUnfinishedByTaskCode(string applicationId, string taskCodes, string receiverId);
        #endregion

        #region 函数:GetUnfinishedQuantities(string receiverId)
        /// <summary>获取未完成任务的数量</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个包含每个类型的统计数的字典 </returns>
        Dictionary<int, int> GetUnfinishedQuantities(string receiverId);
        #endregion
    }
}