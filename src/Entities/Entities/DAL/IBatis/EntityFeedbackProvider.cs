namespace X3Platform.Entities.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class EntityFeedbackProvider : IEntityFeedbackProvider
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Entity_Feedback";

        #region 构造函数:EntityFeedbackProvider()
        /// <summary>构造函数</summary>
        public EntityFeedbackProvider()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
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

        //-------------------------------------------------------
        // 添加 删除 修改
        //-------------------------------------------------------

        #region 函数:Save(string customTableName, EntityFeedbackInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityFeedbackInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityFeedbackInfo"/>详细信息</returns>
        public EntityFeedbackInfo Save(string customTableName, EntityFeedbackInfo param)
        {
            if (!IsExist(customTableName, param.Id))
            {
                Insert(customTableName, param);
            }
            else
            {
                Update(customTableName, param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(string customTableName, EntityFeedbackInfo param)
        /// <summary>添加记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityFeedbackInfo"/>详细信息</param>
        public void Insert(string customTableName, EntityFeedbackInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(string customTableName, EntityFeedbackInfo param)
        /// <summary>修改记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityFeedbackInfo"/>详细信息</param>
        public void Update(string customTableName, EntityFeedbackInfo param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string customTableName, string id)
        /// <summary>删除记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">记录标识</param>
        public void Delete(string customTableName, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
        
            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName)
        /// <summary>删除记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        public void Delete(string customTableName, string entityId, string entityClassName)
        {
            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityId))
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("WhereClause", string.Format(" EntityId = '{0}' AND EntityClassName = '{1}' ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName)));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityFeedbackInfo"/>的详细信息</returns>
        public IList<EntityFeedbackInfo> FindAll(string customTableName, string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<EntityFeedbackInfo> list = ibatisMapper.QueryForList<EntityFeedbackInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityFeedbackInfo"/>的详细信息</returns>
        public IList<EntityFeedbackInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        {
            string whereClause = string.Format(" EntityId = ##{0}## AND EntityClassName = ##{1}## ORDER BY CreateDate ", entityId, entityClassName);

            return FindAll(customTableName, whereClause, 0);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:IsExist(string customTableName, string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string customTableName, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion
    }
}
