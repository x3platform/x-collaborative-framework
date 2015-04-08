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
    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    public class EntityDocObjectService : IEntityDocObjectService
    {
        /// <summary>数据提供器</summary>
        private IEntityDocObjectProvider provider = null;

        #region 构造函数:EntityDocObjectService()
        /// <summary>构造函数</summary>
        public EntityDocObjectService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            provider = objectBuilder.GetObject<IEntityDocObjectProvider>(typeof(IEntityDocObjectProvider));
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IEntityDocObjectInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="IEntityDocObjectInfo"/>详细信息</param>
        /// <returns>实例<see cref="IEntityDocObjectInfo"/>详细信息</returns>
        public IEntityDocObjectInfo Save(string customTableName, IEntityDocObjectInfo param)
        {
            return this.provider.Save(customTableName, param);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        public IList<IEntityDocObjectInfo> FindAll(string customTableName, string whereClause, int length)
        {
            return this.provider.FindAll(customTableName, whereClause, length);
        }
        #endregion

        #region 函数:FindAllByDocToken(string customTableName, string docToken)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="docToken">文档全局标识</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        public IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken)
        {
            return this.provider.FindAllByDocToken(customTableName, docToken);
        }
        #endregion

        #region 函数:FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="docToken">文档全局标识</param>
        /// <param name="mapper">数据结果映射器</param>
        /// <returns>返回所有实例<see cref="IEntityDocObjectInfo"/>的详细信息</returns>
        public IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper)
        {
            return this.provider.FindAllByDocToken(customTableName, docToken, mapper);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string customTableName, string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string customTableName, string id)
        {
            return this.provider.IsExist(customTableName, id);
        }
        #endregion
    }
}
