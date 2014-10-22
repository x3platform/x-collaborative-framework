namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Spring;
    using X3Platform.Configuration;

    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    public class TimingTaskReceiverService : ITimingTaskReceiverService
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private ITimingTaskReceiverProvider provider = null;

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:TaskReceiverService()
        /// <summary>构造函数:TaskReceiverService()</summary>
        public TimingTaskReceiverService()
        {
            configuration = TasksConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ITimingTaskReceiverProvider>(typeof(ITimingTaskReceiverProvider));
        }
        #endregion

        #region 函数:FindOne(string taskId, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TimingTaskReceiverInfo 实例的详细信息</returns>
        public TaskWaitingItemInfo FindOne(string taskId, string receiverId)
        {
            return provider.FindOne(taskId, receiverId);
        }
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">延迟的接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TimingTaskReceiverInfo 实例的详细信息</returns>
        public IList<TaskWaitingItemInfo> FindAllByReceiverId(string receiverId, string whereClause)
        {
            return provider.FindAllByReceiverId(receiverId, whereClause);
        }
        #endregion

        #region 函数:GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="TimingTaskReceiverInfo"/></returns>
        public IList<TaskWaitingItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(receiverId, startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string taskId, string receiverId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string taskId, string receiverId)
        {
            return provider.IsExist(taskId, receiverId);
        }
        #endregion

        #region 函数:SetSent(string receiverId, string taskIds)
        /// <summary>
        /// 设置任务完成
        /// </summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        public void SetSent(string receiverId, string taskIds)
        {
            provider.SetSent(receiverId, taskIds);
        }
        #endregion

        #region 函数:SetSentByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>
        /// 设置任务完成
        /// </summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        public void SetSentByTaskCode(string applicationId, string taskCodes, string receiverId)
        {
            provider.SetSentByTaskCode(applicationId, taskCodes, receiverId);
        }
        #endregion

        #region 函数:SetUnsent(string receiverId, string taskIds)
        /// <summary>
        /// 设置任务未完成
        /// </summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        public void SetUnsent(string receiverId, string taskIds)
        {
            provider.SetUnsent(receiverId, taskIds);
        }
        #endregion

        #region 函数:SetUnsentByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>
        /// 设置任务未完成
        /// </summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        public void SetUnsentByTaskCode(string applicationId, string taskCodes, string receiverId)
        {
            provider.SetUnsentByTaskCode(applicationId, taskCodes, receiverId);
        }
        #endregion
    }
}
