namespace X3Platform.Entities.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using X3Platform.Entities;
    #endregion

    [DataObject]
    public class EntityClickProvider : IEntityClickProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Entity_Click";

        #region 构造函数:EntityClickProvider()
        /// <summary>构造函数</summary>
        public EntityClickProvider()
        {
            this.ibatisMapping = EntitiesConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
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
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(IEntityClickInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityClickInfo"/>详细信息</returns>
        public IEntityClickInfo Save(IEntityClickInfo param)
        {
            return this.Save(this.tableName, param);
        }
        #endregion

        #region 函数:Save(string customTableName, IEntityClickInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityClickInfo"/>详细信息</returns>
        public IEntityClickInfo Save(string customTableName, IEntityClickInfo param)
        {
            if (!this.IsExist(customTableName, param.EntityId, param.EntityClassName, param.AccountId))
            {
                this.Insert(customTableName, param);
            }
            else
            {
                this.Update(customTableName, param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(IEntityClickInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        public void Insert(IEntityClickInfo param)
        {
            this.Insert(this.tableName, param);
        }
        #endregion

        #region 函数:Insert(string customTableName, IEntityClickInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        public void Insert(string customTableName, IEntityClickInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("EntityId", StringHelper.ToSafeSQL(param.EntityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(param.EntityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(param.AccountId));
            args.Add("Click", param.Click);

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), args);
        }
        #endregion

        #region 函数:Update(IEntityClickInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        public void Update(IEntityClickInfo param)
        {
            this.Update(this.tableName, param);
        }
        #endregion

        #region 函数:Update(string customTableName, IEntityClickInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        public void Update(string customTableName, IEntityClickInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("EntityId", StringHelper.ToSafeSQL(param.EntityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(param.EntityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(param.AccountId));
            args.Add("Click", param.Click);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityDraftInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAll(string whereClause, int length)
        {
            return this.FindAll(this.tableName, whereClause, length);
        }
        #endregion

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityDraftInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAll(string customTableName, string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<IEntityClickInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string entityId, string entityClassName)
        {
            return this.FindAllByEntityId(this.tableName, entityId, entityClassName);
        }
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        {
            string whereClause = string.Format(" EntityId = ##{0}## AND EntityClassName = ##{1}## ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName));

            return this.FindAll(customTableName, whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        {
            string whereClause = string.Format(" {0} = ##{1}## AND {2} = ##{3}## ORDER BY {4} DESC ", mapper["EntityId"].DataColumnName, entityId, mapper["EntityClassName"].DataColumnName, entityClassName, mapper["UpdateDate"].DataColumnName);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("DataColumnSql", StringHelper.ToSafeSQL(mapper.GetDataColumnSql()));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", 0);

            return this.ibatisMapper.QueryForList<IEntityClickInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string entityId, string entityClassName, string accountId)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string entityId, string entityClassName, string accountId)
        {
            return this.IsExist(this.tableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 函数:IsExist(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string customTableName, string entityId, string entityClassName, string accountId)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                throw new Exception("实例标识不能为空.");
            }

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", string.Format(" EntityId = '{0}' AND EntityClassName = '{1}' AND AccountId = '{2}' ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName), StringHelper.ToSafeSQL(accountId)));

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:Increment(string entityId, string entityClassName, string accountId)
        /// <summary>自增实体数据的点击数</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns></returns>
        public int Increment(string entityId, string entityClassName, string accountId)
        {
            return this.Increment(this.tableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 函数:Increment(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>自增实体数据的点击数</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        public int Increment(string customTableName, string entityId, string entityClassName, string accountId)
        {
            if (string.IsNullOrEmpty(customTableName))
            {
                throw new Exception("数据表明此不能为空.");
            }

            if (string.IsNullOrEmpty(entityId))
            {
                throw new Exception("实例标识不能为空.");
            }

            if (string.IsNullOrEmpty(entityClassName))
            {
                throw new Exception("实例类的名称不能为空.");
            }

            if (string.IsNullOrEmpty(accountId))
            {
                throw new Exception("帐号标识不能为空.");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("EntityId", StringHelper.ToSafeSQL(entityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(entityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Increment", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:CalculateClickCount(string entityId, string entityClassName)
        /// <summary>计算实体数据的点击数</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>点击数</returns>
        public int CalculateClickCount(string entityId, string entityClassName)
        {
            return this.CalculateClickCount(this.tableName, entityId, entityClassName);
        }
        #endregion

        #region 函数:CalculateClickCount(string tableName, string entityId, string entityClassName)
        /// <summary>计算实体数据的点击数</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>点击数</returns>
        public int CalculateClickCount(string customTableName, string entityId, string entityClassName)
        {
            if (string.IsNullOrEmpty(customTableName))
            {
                throw new Exception("数据表明此不能为空.");
            }

            if (string.IsNullOrEmpty(entityId))
            {
                throw new Exception("实例标识不能为空.");
            }

            if (string.IsNullOrEmpty(entityClassName))
            {
                throw new Exception("实例类的名称不能为空.");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("EntityId", StringHelper.ToSafeSQL(entityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(entityClassName));

            return (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CalculateClickCount", tableName)), args);
        }
        #endregion

    }
}
