namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    public class EntityClickService : IEntityClickService
    {
        /// <summary>数据提供器</summary>
        private IEntityClickProvider provider = null;

        #region 构造函数:EntityClickService()
        /// <summary>构造函数</summary>
        public EntityClickService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            provider = objectBuilder.GetObject<IEntityClickProvider>(typeof(IEntityClickProvider));
        }
        #endregion

        // -------------------------------------------------------
        // 保存
        // -------------------------------------------------------

        #region 函数:Save(IEntityClickInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityClickInfo"/>详细信息</returns>
        public IEntityClickInfo Save(IEntityClickInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Save(string customTableName, IEntityClickInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="IEntityClickInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityClickInfo"/>详细信息</returns>
        public IEntityClickInfo Save(string customTableName, IEntityClickInfo param)
        {
            return this.provider.Save(customTableName, param);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAll(string customTableName, string whereClause, int length)
        {
            return this.provider.FindAll(customTableName, whereClause, length);
        }
        #endregion

        #region 函数:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string entityId, string entityClassName)
        {
            return this.provider.FindAllByEntityId(entityId, entityClassName);
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
            return this.provider.FindAllByEntityId(customTableName, entityId, entityClassName);
        }
        #endregion

        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="mapper">数据结果映射器</param>
        /// <returns>返回所有实例<see cref="IEntityClickInfo"/>的详细信息</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
         {
             return this.provider.FindAllByEntityId(customTableName, entityId, entityClassName, mapper);
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
            return this.provider.IsExist(entityId, entityClassName, accountId);
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
            return this.provider.IsExist(customTableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 函数:Increment(string entityId, string entityClassName, string accountId)
        /// <summary>自增实体数据的点击数</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        public int Increment(string entityId, string entityClassName, string accountId)
        {
            return this.provider.Increment(entityId, entityClassName, accountId);
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
            return this.provider.Increment(customTableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 函数:CalculateClickCount(string entityId, string entityClassName)
        /// <summary>计算实体数据的点击数</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>点击数</returns>
        public int CalculateClickCount(string entityId, string entityClassName)
        {
            return this.provider.CalculateClickCount(entityId, entityClassName);
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
            return this.provider.CalculateClickCount(customTableName, entityId, entityClassName);
        }
        #endregion
    }
}
