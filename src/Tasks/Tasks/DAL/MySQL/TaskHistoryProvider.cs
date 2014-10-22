namespace X3Platform.Tasks.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership;
    using X3Platform.Util;

    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class TaskHistoryProvider : ITaskHistoryProvider
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Task_HistoryItem";

        #region 构造函数:TaskHistoryProvider()
        /// <summary>构造函数</summary>
        public TaskHistoryProvider()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(TaskHistoryItemInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例详细信息</param>
        /// <returns></returns>
        public TaskHistoryItemInfo Save(TaskHistoryItemInfo param)
        {
            if (!this.IsExist(param.Id, param.ReceiverId))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(TaskHistoryItemInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例的详细信息</param>
        public void Insert(TaskHistoryItemInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(TaskHistoryItemInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例的详细信息</param>
        public void Update(TaskHistoryItemInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id, string receiverId)
        /// <summary>删除记录</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        public void Delete(string id, string receiverId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(receiverId)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' AND ReceiverId = '{1}' ", StringHelper.ToSafeSQL(id), StringHelper.ToSafeSQL(receiverId)));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("ReceiverId", StringHelper.ToSafeSQL(receiverId));

            return this.ibatisMapper.QueryForObject<TaskHistoryItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
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
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        public IList<TaskHistoryItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(whereClause))
            {
                whereClause = string.Format(" ReceiverId = ##{0}## ", receiverId);
            }
            else
            {
                whereClause = string.Format(" ReceiverId = ##{0}## AND {1}", receiverId, whereClause);
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? " CreateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<TaskHistoryItemInfo> list = this.ibatisMapper.QueryForList<TaskHistoryItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id, string receiverId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id, string receiverId)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' AND ReceiverId = '{1}' ", StringHelper.ToSafeSQL(id), StringHelper.ToSafeSQL(receiverId)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;
        }
        #endregion
    }
}
