namespace X3Platform.Tasks.DAL.SQLServer
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
    public class TaskCategoryProvider : ITaskCategoryProvider
    {
        /// <summary>配置</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Task_Category";

        #region 构造函数:TaskCategoryProvider()
        /// <summary>构造函数</summary>
        public TaskCategoryProvider()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(TaskCategoryInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例详细信息</param>
        /// <returns></returns>
        public TaskCategoryInfo Save(TaskCategoryInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(TaskCategoryInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例的详细信息</param>
        public void Insert(TaskCategoryInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(TaskCategoryInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例的详细信息</param>
        public void Update(TaskCategoryInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 函数:CanDelete(string id)
        /// <summary>判断新闻类别是否能够被删除</summary>
        /// <param name="id">新闻类别标识</param>
        /// <returns>true：可删除；false：不能删除。</returns>
        public bool CanDelete(string id)
        {
            bool canDelete = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CategoryId", StringHelper.ToSafeSQL(id));

            canDelete = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CanDelete", this.tableName)), args) == 0) ? true : false;

            return canDelete;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">新闻类别标识</param>
        public void Delete(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            string scopeTableName = string.Format("{0}_Scope", this.tableName);

            string entityId = StringHelper.ToSafeSQL(id);
            string entityClassName = KernelContext.ParseObjectType(typeof(TaskCategoryInfo));

            try
            {
                this.BeginTransaction();

                MembershipManagement.Instance.AuthorizationObjectService.RemoveAuthorizationScopeObjects(
                    this.ibatisMapper.CreateGenericSqlCommand(),
                    scopeTableName,
                    entityId,
                    entityClassName,
                    "应用_通用_添加权限");

                MembershipManagement.Instance.AuthorizationObjectService.RemoveAuthorizationScopeObjects(
                    this.ibatisMapper.CreateGenericSqlCommand(),
                    scopeTableName,
                    entityId,
                    entityClassName,
                    "应用_通用_查看权限");

                MembershipManagement.Instance.AuthorizationObjectService.RemoveAuthorizationScopeObjects(
                    this.ibatisMapper.CreateGenericSqlCommand(),
                    scopeTableName,
                    entityId,
                    entityClassName,
                    "应用_通用_修改权限");

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

                this.CommitTransaction();
            }
            catch
            {
                this.RollBackTransaction();

                throw;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例的详细信息</returns>
        public TaskCategoryInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            TaskCategoryInfo param = this.ibatisMapper.QueryForObject<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">类别索引</param>
        /// <returns>返回实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CategoryIndex", categoryIndex);

            TaskCategoryInfo param = this.ibatisMapper.QueryForObject<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCategoryIndex", this.tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns></returns>
        public IList<TaskCategoryInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<TaskCategoryInfo> list = this.ibatisMapper.QueryForList<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllQueryObject(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public IList<TaskCategoryInfo> FindAllQueryObject(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<TaskCategoryInfo> list = this.ibatisMapper.QueryForList<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllQueryObject", this.tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计.</param>
        /// <param name="pageSize">每页显示的数据行数</param>
        /// <param name="whereClause">WHERE 查询条件.</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <param name="rowCount">符合条件的数据总行数</param>
        /// <returns></returns>
        public IList<TaskCategoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<TaskCategoryInfo> list = this.ibatisMapper.QueryForList<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>
        /// 查询是否存在相关的记录
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:SetStatus(string id, int status)
        /// <summary>设置类别状态(停用/启用)</summary>
        /// <param name="id">新闻类别标识</param>
        /// <param name="status">1将停用的类别启用，0将在用的类别停用</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);
            args.Add("Status", status);

            return (this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", this.tableName)), args) == 1 ? true : false);
        }
        #endregion
    }
}
