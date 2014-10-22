namespace X3Platform.Tasks.DAL.SQLServer
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class TimingTaskReceiverProvider : ITimingTaskReceiverProvider
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_TimingTask_Receiver";

        /// <summary></summary>
        public TimingTaskReceiverProvider()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        #region 函数:FindOne(string timingTaskId, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="timingTaskId">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回一个 TimingTaskReceiverInfo 实例的详细信息</returns>
        public TaskWaitingItemInfo FindOne(string timingTaskId, string receiverId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("TimingTaskId", timingTaskId);
            args.Add("ReceiverId", receiverId);

            return this.ibatisMapper.QueryForObject<TaskWaitingItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindAllByReceiverId(string receiverId, string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="receiverId">延迟的接收者帐号标识</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 TimingTaskReceiverInfo 实例的详细信息</returns>
        public IList<TaskWaitingItemInfo> FindAllByReceiverId(string receiverId, string whereClause)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ReceiverId", receiverId);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            return this.ibatisMapper.QueryForList<TaskWaitingItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllByReceiverId", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", orderBy);

            IList<TaskWaitingItemInfo> list = this.ibatisMapper.QueryForList<TaskWaitingItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(TimingTaskReceiverInfo param)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="timingTaskId">任务标识</param>
        /// <param name="receiverId">接收者标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string timingTaskId, string receiverId)
        {
            if (string.IsNullOrEmpty(timingTaskId) || string.IsNullOrEmpty(receiverId))
                throw new Exception("定时任务标识和接收人标识不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" TimingTaskId='{0}' and ReceiverId='{1}'",
                StringHelper.ToSafeSQL(timingTaskId),
                StringHelper.ToSafeSQL(receiverId)));

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:SetSent(string receiverId, string taskIds)
        /// <summary>设置任务完成</summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        public void SetSent(string receiverId, string taskIds)
        {
            if (string.IsNullOrEmpty(taskIds))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" ReceiverId='{0}' AND TimingTaskId IN ('{1}') ", receiverId, taskIds.Replace(",", "','"));

            args.Add("WhereClause", whereClause);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetSent", tableName)), args);
        }
        #endregion

        #region 函数:SetUnsent(string receiverId, string taskIds)
        /// <summary>设置任务未完成</summary>
        /// <param name="receiverId">接收者</param>
        /// <param name="taskIds">任务编号,多个以逗号分开</param>
        public void SetUnsent(string receiverId, string taskIds)
        {
            if (string.IsNullOrEmpty(taskIds))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" ReceiverId = '{0}' AND TimingTaskId IN ('{1}') ", receiverId, taskIds.Replace(",", "','"));

            args.Add("WhereClause", whereClause);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetUnsent", tableName)), args);
        }
        #endregion

        #region 函数:SetSentByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        public void SetSentByTaskCode(string applicationId, string taskCodes, string receiverId)
        {
            if (string.IsNullOrEmpty(taskCodes))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" ReceiverId='{0}' AND TaskId IN ( SELECT Id FROM tb_Task WHERE ApplicationId = '{1}' AND TaskCode IN ('{2}') ) ", receiverId, applicationId, taskCodes.Replace(",", "','"));

            args.Add("WhereClause", whereClause);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetSent", tableName)), args);
        }
        #endregion

        #region 函数:SetUnsentByTaskCode(string applicationId, string taskCodes, string receiverId)
        /// <summary>设置任务未完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        /// <param name="receiverId">接收者</param>
        public void SetUnsentByTaskCode(string applicationId, string taskCodes, string receiverId)
        {
            if (string.IsNullOrEmpty(taskCodes))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" ReceiverId='{0}' AND TaskId IN ( SELECT Id FROM tb_Task WHERE ApplicationId = '{1}' AND TaskCode IN ('{2}') ) ", receiverId, applicationId, taskCodes.Replace(",", "','"));

            args.Add("WhereClause", whereClause);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetUnsent", tableName)), args);
        }
        #endregion
    }
}
