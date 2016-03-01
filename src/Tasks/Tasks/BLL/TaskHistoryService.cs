namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary></summary>
    public class TaskHistoryService : ITaskHistoryService
    {
        private TasksConfiguration configuration = null;

        private ITaskHistoryProvider provider = null;

        /// <summary></summary>
        public TaskHistoryService()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ITaskHistoryProvider>(typeof(ITaskHistoryProvider));
        }

        #region 索引:this[string id, string receiverId]
        /// <summary>索引</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns></returns>
        public TaskHistoryItemInfo this[string id, string receiverId]
        {
            get { return this.FindOne(id, receiverId); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(TaskHistoryItemInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="TaskHistoryItemInfo"/>详细信息</param>
        /// <returns>实例<see cref="TaskHistoryItemInfo"/>详细信息</returns>
        public TaskHistoryItemInfo Save(TaskHistoryItemInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new Exception("实例标识不能为空。");
            }

            // 处理XSS特殊字符 
            param = StringHelper.ToSafeXSS<TaskHistoryItemInfo>(param);

            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id, string receiverId)
        /// <summary>删除记录</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        public void Delete(string id, string receiverId)
        {
            this.provider.Delete(id, receiverId);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回实例<see cref="TaskHistoryItemInfo"/></returns>
        public TaskHistoryItemInfo FindOne(string id, string receiverId)
        {
            return this.provider.FindOne(id, receiverId);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        public IList<TaskHistoryItemInfo> GetPaging(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPaging(receiverId, startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id, string receiverId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id, string receiverId)
        {
            return this.provider.IsExist(id, receiverId);
        }
        #endregion
    }
}
