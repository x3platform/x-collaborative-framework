namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.DigitalNumber;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    public class EntityOperationLogService : IEntityOperationLogService
    {
        /// <summary>数据提供器</summary>
        private IEntityOperationLogProvider provider = null;

        #region 构造函数:EntityOperationLogService()
        /// <summary>构造函数</summary>
        public EntityOperationLogService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IEntityOperationLogProvider>(typeof(IEntityOperationLogProvider));
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(EntityOperationLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityOperationLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityOperationLogInfo"/>详细信息</returns>
        public EntityOperationLogInfo Save(EntityOperationLogInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Save(string customTableName, EntityOperationLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityOperationLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityOperationLogInfo"/>详细信息</returns>
        public EntityOperationLogInfo Save(string customTableName, EntityOperationLogInfo param)
        {
            return this.provider.Save(customTableName, param);
        }
        #endregion

        #region 函数:Copy(string customTableName, string oldEntityId, string newEntityId, string entityClassName)
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="oldEntityId">旧的实体类标识</param>
        /// <param name="newEntityId">新的实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        public void Copy(string customTableName, string oldEntityId, string newEntityId, string entityClassName)
        {
            IList<EntityOperationLogInfo> list = EntitiesManagement.Instance.EntityOperationLogService.FindAllByEntityId(customTableName, oldEntityId, entityClassName);

            foreach (EntityOperationLogInfo item in list)
            {
                item.Id = DigitalNumberContext.Generate("Key_Guid");

                item.EntityId = newEntityId;

                EntitiesManagement.Instance.EntityOperationLogService.Save(customTableName, item);
            }
        }
        #endregion

        #region 函数:Copy(string customTableName, string oldEntityId, string newEntityId, string entityClassName, int operationType)
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="oldEntityId">旧的实体类标识</param>
        /// <param name="newEntityId">新的实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="operationType">操作类型</param>
        public void Copy(string customTableName, string oldEntityId, string newEntityId, string entityClassName, int operationType)
        {
            IList<EntityOperationLogInfo> list = EntitiesManagement.Instance.EntityOperationLogService.FindAllByEntityId(customTableName, oldEntityId, entityClassName, operationType);

            foreach (EntityOperationLogInfo item in list)
            {
                item.Id = DigitalNumberContext.Generate("Key_Guid");

                item.EntityId = newEntityId;

                EntitiesManagement.Instance.EntityOperationLogService.Save(customTableName, item);
            }
        }
        #endregion

        #region 函数:Delete(string entityId, string entityClassName)
        /// <summary>删除所有相关记录</summary>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>0 表示成功 | 1 表示失败</returns>
        public int Delete(string entityId, string entityClassName)
        {
            return this.provider.Delete(entityId, entityClassName);
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
            return this.provider.Delete(entityId, entityClassName, operationType);
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
            return this.provider.Delete(customTableName, entityId, entityClassName);
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
            return this.provider.Delete(customTableName, entityId, entityClassName, operationType);
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
            return this.provider.FindAll(whereClause, length);
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
            return this.provider.FindAll(whereClause, length);
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
            return this.provider.FindAllByEntityId(entityId, entityClassName);
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
            return this.provider.FindAllByEntityId(entityId, entityClassName, operationType);
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
            return this.provider.FindAllByEntityId(customTableName, entityId, entityClassName);
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
            return this.provider.FindAllByEntityId(customTableName, entityId, entityClassName, operationType);
        }
        #endregion
    }
}
