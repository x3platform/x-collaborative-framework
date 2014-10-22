namespace X3Platform.Tasks.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Storages;
    using X3Platform.Util;

    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class TaskProvider : ITaskProvider
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象集合</summary>
        private Dictionary<string, ISqlMapper> ibatisMappers = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Task";

        /// <summary>数据存储架构标识</summary>
        private string storageSchemaId = "01-09-Tasks";

        /// <summary>数据存储策略</summary>
        private IStorageStrategy storageStrategy = null;

        /// <summary></summary>
        public TaskProvider()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMappers = StorageContext.Instance.CreateSqlMappers(this.storageSchemaId, this.ibatisMapping);

            this.storageStrategy = StorageContext.Instance.StorageSchemaService[this.storageSchemaId].GetStrategyClass();
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除 
        // -------------------------------------------------------

        #region 函数:Save(TaskInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">TaskInfo 实例详细信息</param>
        /// <returns>TaskInfo 实例详细信息</returns>
        public TaskInfo Save(TaskInfo param)
        {
            if (!this.IsExistTaskCode(param.ApplicationId, param.TaskCode))
            {
                this.Insert(param);
            }
            else
            {
                param.Id = this.GetIdsByTaskCodes(param.ApplicationId, param.TaskCode);

                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(TaskInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">TaskInfo 实例的详细信息</param>
        public void Insert(TaskInfo param)
        {
            IList<TaskWorkItemInfo> list = param.GetTaskWorkItems();

            // 接收者信息
            foreach (TaskWorkItemInfo item in list)
            {
                IStorageNode storageNode = storageStrategy.GetStorageNode("Node", item.ReceiverId);

                this.ibatisMappers[storageNode.Name].Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), item);
            }
        }
        #endregion

        #region 函数:Update(TaskInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">TaskInfo 实例的详细信息</param>
        public void Update(TaskInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = this.GetIdsByTaskCodes(param.ApplicationId, param.TaskCode);
            }

            // 删除原始
            this.Delete(param.Id);

            // 插入新的数据
            this.Insert(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">任务的标识信息,多个以逗号隔开</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) { return; }

            ids = StringHelper.ToSafeSQL(ids, true);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", ids.Replace(",", "','")));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
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

            args.Add("WhereClause", string.Format(" ApplicationId = '{0}' AND TaskCode = '{1}' ", StringHelper.ToSafeSQL(applicationId, true), StringHelper.ToSafeSQL(taskCode, true)));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
            }
        }
        #endregion

        #region 函数:DeleteByTaskCode(string applicationId, string taskCode, string receiverIds)
        /// <summary>删除记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <param name="receiverIds">任务接收人标识</param>
        public void DeleteByTaskCode(string applicationId, string taskCode, string receiverIds)
        {
            if (string.IsNullOrEmpty(applicationId) || string.IsNullOrEmpty(taskCode)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" ApplicationId = '{0}' AND TaskCode = '{1}' AND ReceiverId IN ('{2}') ",
                StringHelper.ToSafeSQL(applicationId, true),
                StringHelper.ToSafeSQL(taskCode, true),
                StringHelper.ToSafeSQL(receiverIds).Replace(",", "','")));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">TaskInfo Id号</param>
        /// <returns>返回一个 TaskInfo 实例的详细信息</returns>
        public TaskInfo FindOne(string id)
        {
            TaskInfo param = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                param = ibatisMapper.QueryForObject<TaskInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);

                if (param != null) { return param; }
            }

            return null;
        }
        #endregion

        #region 函数:FindOneByTaskCode(string applicationId, string taskCode)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>返回一个 TaskInfo 实例的详细信息</returns>
        public TaskInfo FindOneByTaskCode(string applicationId, string taskCode)
        {
            TaskInfo param = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", applicationId);
            args.Add("TaskCode", taskCode);

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                param = ibatisMapper.QueryForObject<TaskInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByTaskCode", this.tableName)), args);

                if (param != null) { return param; }
            }

            return null;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskInfo 实例的详细信息</returns>
        public IList<TaskWorkItemInfo> FindAll(string whereClause, int length)
        {
            IList<TaskWorkItemInfo> list = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                list = ibatisMapper.QueryForList<TaskWorkItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
            }

            return list;
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
        public IList<TaskWorkItemInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            orderBy = string.IsNullOrEmpty(orderBy) ? " CreateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", orderBy);

            args.Add("RowCount", 0);

            IList<TaskWorkItemInfo> list = this.ibatisMappers[storageNode.Name].QueryForList<TaskWorkItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = (int)this.ibatisMappers[storageNode.Name].QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

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

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                if (Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) > 0)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 函数:IsExistTaskCode(string applicationId, string taskCode)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>布尔值</returns>
        public bool IsExistTaskCode(string applicationId, string taskCode)
        {
            if (string.IsNullOrEmpty(applicationId) || string.IsNullOrEmpty(taskCode)) { throw new Exception("应用标识和任务编号不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" ApplicationId = '{0}' AND TaskCode = '{1}' ", StringHelper.ToSafeSQL(applicationId), StringHelper.ToSafeSQL(taskCode)));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                if (Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) > 0)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 函数:SetTitle(string applicationId, string taskCode, string title)
        /// <summary>设置任务标题</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        /// <param name="title">任务标题</param>
        public void SetTitle(string applicationId, string taskCode, string title)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", applicationId);
            args.Add("TaskCode", taskCode);
            args.Add("Title", StringHelper.ToSafeSQL(title));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetTitle", this.tableName)), args);
            }
        }
        #endregion

        #region 函数:SetFinished(string applicationId, string taskCode)
        /// <summary>设置任务完成</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <param name="taskCode">任务编号</param>
        public void SetFinished(string applicationId, string taskCode)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", applicationId);
            args.Add("TaskCode", taskCode);

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetFinished", this.tableName)), args);
            }
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

            args.Add("WhereClause", string.Format(" ApplicationId = '{0}' AND TaskCode IN ('{1}') ",
                StringHelper.ToSafeSQL(applicationId, true),
                StringHelper.ToSafeSQL(taskCodes).Replace(",", "','")));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                IList<string> list = ibatisMapper.QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetIdsByTaskCodes", this.tableName)), args);

                foreach (string id in list)
                {
                    ids += id + ",";
                }
            }

            ids = ids.Trim(',');

            return ids;
        }
        #endregion

        #region 函数:GetTaskTags(string key)
        /// <summary>获取任务标签列表</summary>
        /// <param name="key">匹配标签的关键字, 空值则全部匹配</param>
        public IList<string> GetTaskTags(string key)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.IsNullOrEmpty(key) ? string.Empty : string.Format(" Category LIKE '%{0}%' ", StringHelper.ToSafeSQL(key)));

            return this.ibatisMappers[storageNode.Name].QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetTaskTags", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 归档、删除某一时段的待办记录
        // -------------------------------------------------------

        #region 函数:Archive()
        /// <summary>将归档日期之前已完成的待办归档到历史数据表</summary>
        /// <param name="archiveDate">归档日期</param>
        public int Archive(DateTime archiveDate)
        {
            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                Dictionary<string, object> args = new Dictionary<string, object>();

                args.Add("WhereClause", string.Format(" Status = 1 AND CreateDate < '{0}' ", archiveDate));

                ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Archive", this.tableName)), args);
            }

            this.RemoveWorkItems(archiveDate);

            return 0;
        }
        #endregion

        #region 函数:RemoveUnfinishedWorkItems(DateTime expireDate)
        /// <summary>删除过期时间之前未完成的工作项记录</summary>
        /// <param name="expireDate">过期时间</param>
        public void RemoveUnfinishedWorkItems(DateTime expireDate)
        {
            Dictionary<string, object> args1 = new Dictionary<string, object>();

            args1.Add("WhereClause", string.Format(" Status = 0 AND CreateDate < '{0}' ", expireDate));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args1);
            }
        }
        #endregion

        #region 函数:RemoveWorkItems(DateTime expireDate)
        /// <summary>删除过期时间之前的工作项记录</summary>
        /// <param name="expireDate">过期时间</param>
        public void RemoveWorkItems(DateTime expireDate)
        {
            // DELETE FROM tb_Task WHERE CreateDate < DATEADD(month, 3,(SELECT  MIN(CreateDate) FROM tb_Task))
            Dictionary<string, object> args1 = new Dictionary<string, object>();

            args1.Add("WhereClause", string.Format(" Status = 1 AND CreateDate < '{0}' ", expireDate));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args1);
            }
        }
        #endregion

        #region 函数:RemoveHistoryItems(DateTime expireDate)
        /// <summary>删除过期时间之前的历史记录</summary>
        /// <param name="expireDate">过期时间</param>
        public void RemoveHistoryItems(DateTime expireDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" CreateDate < '{0}' ", expireDate));

            foreach (KeyValuePair<string, ISqlMapper> entity in this.ibatisMappers)
            {
                ISqlMapper ibatisMapper = entity.Value;

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveHistoryItems", this.tableName)), args);
            }
        }
        #endregion
    }
}
