namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.CacheBuffer;
    using X3Platform.Spring;
    using X3Platform.Configuration;

    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    public class TaskReceiverService : ITaskReceiverService
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private ITaskReceiverProvider provider = null;

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:TaskReceiverService()
        /// <summary>构造函数:TaskReceiverService()</summary>
        public TaskReceiverService()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ITaskReceiverProvider>(typeof(ITaskReceiverProvider));
        }
        #endregion

        #region 函数:FindOne(string taskId, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TaskReceiverInfo 实例的详细信息</returns>
        public TaskWorkItemInfo FindOne(string taskId, string receiverId)
        {
            return this.provider.FindOne(taskId, receiverId);
        }
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TaskReceiverInfo 实例的详细信息</returns>
        public TaskWorkItemInfo FindOneByTaskCode(string applicationId, string taskCode, string receiverId)
        {
            return this.provider.FindOneByTaskCode(applicationId, taskCode, receiverId);
        }
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TaskReceiverInfo 实例的详细信息</returns>
        public IList<TaskWorkItemInfo> FindAllByReceiverId(string receiverId, string whereClause)
        {
            return this.FindAllByReceiverId(receiverId, whereClause, 50);
        }
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">长度</param>
        /// <returns>返回所有 TaskReceiverInfo 实例的详细信息</returns>
        public IList<TaskWorkItemInfo> FindAllByReceiverId(string receiverId, string whereClause, int length)
        {
            return this.provider.FindAllByReceiverId(receiverId, whereClause, length);
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
        /// <returns>返回一个列表实例<see cref="TaskWorkItemInfo"/></returns>
        public IList<TaskWorkItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            // 我的待办 过滤接收者
            //
            // [兼容]
            // 龙湖的待办系统接收者标识取的是登录名, 系统默认取帐号标识.
            //
            string bindReceiverSQL = string.Format(" T.ReceiverId = ##{0}## ", receiverId);

            // if (KernelContext.ParseObjectType(this.provider.GetType()) == "X3Platform.Plugins.Longfor.Tasks.DAL.IBatis.TaskReceiverProvider, X3Platform.Plugins.Longfor")
            // {
            //    bindReceiverSQL = string.Format(" T.ReceiverId = ##{0}## ", KernelContext.Current.User.LoginName);
            // }

            if (string.IsNullOrEmpty(whereClause))
            {
                whereClause = string.Format(" {0} AND T.Status = 0 ", bindReceiverSQL);
            }
            else if (whereClause.IndexOf(bindReceiverSQL) == -1)
            {
                whereClause += string.Format(" AND {0} ", bindReceiverSQL);
            }

            return this.provider.GetPages(receiverId, startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string taskId, string receiverId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string taskId, string receiverId)
        {
            return this.provider.IsExist(taskId, receiverId);
        }
        #endregion

        #region 函数:Copy(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        /// <summary>复制待办信息</summary>
        /// <param name="fromReceiverId">待办来源接收者标识</param>
        /// <param name="toReceiverId">待办目标接收者标识</param>
        /// <param name="beginDate">复制待办的开始时间</param>
        /// <param name="endDate">复制待办结束时间</param>
        /// <returns></returns>
        public int Copy(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        {
            return this.provider.Copy(fromReceiverId, toReceiverId, beginDate, endDate);
        }
        #endregion

        #region 函数:Cut(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        /// <summary>剪切待办信息</summary>
        /// <param name="fromReceiverId">待办来源接收者标识</param>
        /// <param name="toReceiverId">待办目标接收者标识</param>
        /// <param name="beginDate">复制待办的开始时间</param>
        /// <param name="endDate">复制待办结束时间</param>
        /// <returns></returns>
        public int Cut(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
        {
            return this.provider.Cut(fromReceiverId, toReceiverId, beginDate, endDate);
        }
        #endregion

        #region 函数:SetRead(string taskId, string receiverId, bool isRead)
        /// <summary>设置任务阅读状态</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者的用户名</param>
        /// <param name="isRead">状态: true 已读 | false 未读 </param>
        public void SetRead(string taskId, string receiverId, bool isRead)
        {
            this.provider.SetRead(taskId, receiverId, isRead);
        }
        #endregion

        #region 函数:SetStatus(Guid taskId, string receiverId, int status)
        /// <summary>强制设置任务状态</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者的用户名</param>
        /// <param name="status">状态</param>
        public void SetStatus(string taskId, string receiverId, int status)
        {
            this.provider.SetStatus(taskId, receiverId, status);
        }
        #endregion

        #region 函数:SetFinished(string receiverId)
        /// <summary>设置某个用户的任务全部完成</summary>
        /// <param name="receiverId">接收者</param>
        public void SetFinished(string receiverId)
        {
            this.provider.SetFinished(receiverId);
        }
        #endregion

        #region 函数:SetFinished(string receiverId, string taskIds)
        /// <summary>设置任务完成</summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        public void SetFinished(string receiverId, string taskIds)
        {
            this.provider.SetFinished(receiverId, taskIds);
        }
        #endregion

        #region 函数:SetUnfinished(string receiverId, string taskIds)
        /// <summary>设置任务未完成</summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        public void SetUnfinished(string receiverId, string taskIds)
        {
            this.provider.SetUnfinished(receiverId, taskIds);
        }
        #endregion

        #region 函数:SetFinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>
        /// 设置任务完成
        /// </summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        public void SetFinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
        {
            this.provider.SetFinishedByTaskCode(applicationId, taskCodes, receiverId);
        }
        #endregion

        #region 函数:SetUnfinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>
        /// 设置任务未完成
        /// </summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        public void SetUnfinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
        {
            this.provider.SetUnfinishedByTaskCode(applicationId, taskCodes, receiverId);
        }
        #endregion

        #region 函数:GetUnfinishedQuantities(string receiverId)
        /// <summary>获取未完成任务的数量</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个包含每个类型的统计数的 DataTable </returns>
        public Dictionary<int, int> GetUnfinishedQuantities(string receiverId)
        {
            return this.provider.GetUnfinishedQuantities(receiverId);
        }
        #endregion

        #region 函数:GetWidgetData(string receiverId, int length)
        /// <summary>获取任务部件的数据</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <param name="length">显示的最大条数</param>
        /// <returns>返回所有 TaskWorkItemInfo 实例的列表信息</returns>
        public IList<TaskWorkItemInfo> GetWidgetData(string receiverId, int length)
        {
            IList<TaskWorkItemInfo> list = this.provider.FindAllByReceiverId(receiverId, " Type IN (1, 8, 266) AND Status = 0 ORDER BY CreateDate DESC ", length);

            if (list.Count == 0)
            {
                list = this.provider.FindAllByReceiverId(receiverId, " Type IN (2, 4, 258, 260) AND Status = 0 ORDER BY CreateDate DESC ", length);
            }

            return list;
        }
        #endregion
    }
}
