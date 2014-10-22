namespace X3Platform.Tasks.DAL.SQLServer
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class TimingTaskProvider : ITimingTaskProvider
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_TimingTask";

        /// <summary></summary>
        public TimingTaskProvider()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除 
        // -------------------------------------------------------

        #region 函数:Save(TimingTaskInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">TimingTaskInfo 实例详细信息</param>
        /// <returns>TimingTaskInfo 实例详细信息</returns>
        public TimingTaskInfo Save(TimingTaskInfo param)
        {
            if (!IsExistTaskCode(param.ApplicationId, param.TaskCode))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (TimingTaskInfo)param;
        }
        #endregion

        #region 函数:Insert(TimingTimingTaskInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">TimingTaskInfo 实例的详细信息</param>
        public void Insert(TimingTaskInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            try
            {
                this.ibatisMapper.BeginTransaction();

                this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);

                foreach (TimingTaskReceiverInfo item in param.ReceiverGroup)
                {
                    item.TimingTaskId = param.Id;

                    this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Receiver_Insert", tableName)), item);
                }

                this.ibatisMapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        #region 函数:Update(TimingTaskInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">TimingTaskInfo 实例的详细信息</param>
        public void Update(TimingTaskInfo param)
        {
            //
            // [主键] ApplicationId 和 TaskCode 
            //

            Dictionary<string, object> args = new Dictionary<string, object>();

            TimingTaskInfo temp = FindOneByTaskCode(param.ApplicationId, param.TaskCode);

            args.Add("Id", temp.Id);

            args.Add("ApplicationId", param.ApplicationId);
            args.Add("TaskCode", param.TaskCode);

            try
            {
                this.ibatisMapper.BeginTransaction();

                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);

                // 接收者

                // 删除旧的分配人员数据
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Receiver_DeleteByTaskCode", tableName)), args);

                // 添加新的分配人员数据
                foreach (TimingTaskReceiverInfo item in param.ReceiverGroup)
                {
                    item.TimingTaskId = temp.Id;

                    this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Receiver_Insert", tableName)), item);
                }

                this.ibatisMapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">任务的标识信息,多个以逗号隔开</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            try
            {
                this.ibatisMapper.BeginTransaction();

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Receiver_Delete", tableName)), args);

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode)
        /// <summary>删除记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        public void DeleteByTaskCode(string applicationId, string taskCode)
        {
            if (string.IsNullOrEmpty(applicationId) || string.IsNullOrEmpty(taskCode)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", StringHelper.ToSafeSQL(applicationId));
            args.Add("TaskCode", StringHelper.ToSafeSQL(taskCode));

            try
            {
                this.ibatisMapper.BeginTransaction();

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Receiver_DeleteByTaskCode", tableName)), args);

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_DeleteByTaskCode", tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">TimingTaskInfo Id号</param>
        /// <returns>返回一个 TimingTaskInfo 实例的详细信息</returns>
        public TimingTaskInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            TimingTaskInfo param = this.ibatisMapper.QueryForObject<TimingTaskInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            if (param != null)
            {
                // 绑定接收者信息
                BindReceiverGroup(param);
            }

            return param;
        }
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>返回一个 TimingTaskInfo 实例的详细信息</returns>
        public TimingTaskInfo FindOneByTaskCode(string applicationId, string taskCode)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", applicationId);

            args.Add("TaskCode", taskCode);

            TimingTaskInfo param = this.ibatisMapper.QueryForObject<TimingTaskInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByTaskCode", tableName)), args);

            if (param != null)
            {
                // 绑定接收者信息
                BindReceiverGroup(param);
            }

            return param;
        }
        #endregion

        private void BindReceiverGroup(TimingTaskInfo task)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" TimingTaskId = ##{0}## ", task.Id);

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", 0);

            IList<TimingTaskReceiverInfo> list = this.ibatisMapper.QueryForList<TimingTaskReceiverInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_Receiver_FindAll", tableName)), args);

            foreach (TimingTaskReceiverInfo item in list)
            {
                TimingTaskReceiverInfo receiver = new TimingTaskReceiverInfo();

                receiver.TimingTaskId = item.TimingTaskId;
                receiver.ReceiverId = item.ReceiverId;
                receiver.IsSend = item.IsSend;
                receiver.SendTime = item.SendTime;

                task.ReceiverGroup.Add(receiver);
            }
        }

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TimingTaskInfo 实例的详细信息</returns>
        public IList<TaskWaitingItemInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<TaskWaitingItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAllUnsentItems(int length)
        /// <summary>查询所有待发送的记录</summary>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskWaitingItemInfo 实例的详细信息</returns>
        public IList<TaskWaitingItemInfo> FindAllUnsentItems(int length)
        {
            // 查询所有未发送的待办且触发时间小于当前时间
            string whereClause = " IsSend = 0 AND TriggerTime < GETDATE() ";

            return this.FindAll(whereClause, length);
        }
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
        public IList<TaskWaitingItemInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", orderBy);

            IList<TaskWaitingItemInfo> list = this.ibatisMapper.QueryForList<TaskWaitingItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistTaskCode(string applicationId, string taskCode)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>布尔值</returns>
        public bool IsExistTaskCode(string applicationId, string taskCode)
        {
            if (string.IsNullOrEmpty(applicationId) || string.IsNullOrEmpty(taskCode))
                throw new Exception("应用标识和任务编号不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" ApplicationId='{0}' and TaskCode='{1}'", StringHelper.ToSafeSQL(applicationId), StringHelper.ToSafeSQL(taskCode)));

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:SetSent(string applicationId, string taskCode)
        /// <summary>设置任务已发送</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        public void SetSent(string applicationId, string taskCode)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", StringHelper.ToSafeSQL(applicationId));
            args.Add("TaskCode", StringHelper.ToSafeSQL(taskCode));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetSent", tableName)), args);
        }
        #endregion

        #region 函数:GetTaskTags(string key)
        /// <summary>获取任务标签列表</summary>
        /// <param name="key">匹配标签的关键字, 空值则全部匹配</param>
        public IList<string> GetTaskTags(string key)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Key", StringHelper.ToSafeSQL(key));

            IList<string> list = this.ibatisMapper.QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetTaskTags", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:GetIdsByTaskCodes(string applicationId,string taskCodes)
        /// <summary>将任务编号转换为标识信息</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCodes">任务编号,多个以逗号分开</param>
        public string GetIdsByTaskCodes(string applicationId, string taskCodes)
        {
            string ids = string.Empty;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", applicationId);

            args.Add("TaskCodes", taskCodes);

            IList<string> list = this.ibatisMapper.QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetIdsByTaskCodes", tableName)), args);

            foreach (string item in list)
            {
                ids += item + ",";
            }

            ids = ids.Trim(',');

            return ids;
        }
        #endregion
    }
}
