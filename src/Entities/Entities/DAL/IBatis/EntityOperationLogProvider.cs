// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityOperationLogProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date	        :2010-01-01
//
// =============================================================================

namespace X3Platform.Entities.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;

    /// <summary></summary>
    [DataObject]
    public class EntityOperationLogProvider : IEntityOperationLogProvider
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Entity_OperationLog";

        #region 构造函数:EntityOperationLogProvider()
        /// <summary>构造函数</summary>
        public EntityOperationLogProvider()
        {
            this.configuration = EntitiesConfigurationView.Instance.Configuration;

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
        // 保存
        // -------------------------------------------------------

        #region 函数:Save(EntityOperationLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityOperationLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityOperationLogInfo"/>详细信息</returns>
        public EntityOperationLogInfo Save(EntityOperationLogInfo param)
        {
            return this.Save(this.tableName, param);
        }
        #endregion

        #region 函数:Save(string customTableName, EntityOperationLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityOperationLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityOperationLogInfo"/>详细信息</returns>
        public EntityOperationLogInfo Save(string customTableName, EntityOperationLogInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("EntityId", StringHelper.ToSafeSQL(param.EntityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(param.EntityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(param.AccountId));
            args.Add("OperationType", param.OperationType);
            args.Add("ToAuthorizationObjectId", StringHelper.ToSafeSQL(param.ToAuthorizationObjectId));
            args.Add("ToAuthorizationObjectType", StringHelper.ToSafeSQL(param.ToAuthorizationObjectType));
            args.Add("Reason", StringHelper.ToSafeSQL(param.Reason));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Save", this.tableName)), args);

            return param;
        }
        #endregion

        #region 函数:Delete(string entityId, string entityClassName)
        /// <summary>删除所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        public int Delete(string entityId, string entityClassName)
        {
            return this.Delete(this.tableName, entityId, entityClassName);
        }
        #endregion

        #region 函数:Delete(string entityId, string entityClassName, int operationType)
        /// <summary>删除所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        public int Delete(string entityId, string entityClassName, int operationType)
        {
            return this.Delete(this.tableName, entityId, entityClassName, operationType);
        }
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName)
        /// <summary>删除所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        public int Delete(string customTableName, string entityId, string entityClassName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", string.Format(" EntityId = '{0}' AND EntityClassName = '{1}' ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName)));

            this.ibatisMapper.QueryForList<EntityOperationLogInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName, int operationType)
        /// <summary>删除所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        public int Delete(string customTableName, string entityId, string entityClassName, int operationType)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", string.Format(" EntityId = '{0}' AND EntityClassName = '{1}' AND OperationType = '{2}' ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName), operationType));

            this.ibatisMapper.QueryForList<EntityOperationLogInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        public IList<EntityOperationLogInfo> FindAll(string whereClause, int length)
        {
            return this.FindAll(this.tableName, whereClause, length);
        }
        #endregion

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        public IList<EntityOperationLogInfo> FindAll(string customTableName, string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<EntityOperationLogInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        public IList<EntityOperationLogInfo> FindAllByEntityId(string entityId, string entityClassName)
        {
            return this.FindAllByEntityId(this.tableName, entityId, entityClassName);
        }
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName, int operationType)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        public IList<EntityOperationLogInfo> FindAllByEntityId(string entityId, string entityClassName, int operationType)
        {
            return this.FindAllByEntityId(this.tableName, entityId, entityClassName, operationType);
        }
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        public IList<EntityOperationLogInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        {
            string whereClause = string.Format(
                " EntityId = ##{0}## AND EntityClassName = ##{1}## ORDER BY CreateDate ",
                entityId,
                entityClassName);

            return this.FindAll(customTableName, whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName, int operationType)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回所有实例<see cref="EntityOperationLogInfo"/>的详细信息</returns>
        public IList<EntityOperationLogInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, int operationType)
        {
            string whereClause = string.Format(
                " EntityId = ##{0}## AND EntityClassName = ##{1}## AND OperationType = {2} ORDER BY CreateDate ",
                entityId,
                entityClassName,
                operationType);

            return this.FindAll(customTableName, whereClause, 0);
        }
        #endregion
    }
}
