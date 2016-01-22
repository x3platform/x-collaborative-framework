namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class EntitySchemaService : IEntitySchemaService
    {
        /// <summary>数据提供器</summary>
        private IEntitySchemaProvider provider = null;

        #region 构造函数:EntitySchemaService()
        /// <summary>构造函数</summary>
        public EntitySchemaService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            provider = objectBuilder.GetObject<IEntitySchemaProvider>(typeof(IEntitySchemaProvider));
        }
        #endregion

        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EntitySchemaInfo this[string name]
        {
            get { return this.FindOneByName(name); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(EntitySchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntitySchemaInfo"/>详细信息</returns>
        public EntitySchemaInfo Save(EntitySchemaInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 函数:FindOneByEntityClassName(string entityClassName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOneByEntityClassName(string entityClassName)
        {
            return this.provider.FindOneByEntityClassName(entityClassName);
        }
        #endregion

        #region 函数:FindOneByEntityClassName(string entityClassName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOneByEntityClassName(string entityClassName)
        {
            return this.provider.FindOneByEntityClassName(entityClassName);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public IList<EntitySchemaInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public IList<EntitySchemaInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public IList<EntitySchemaInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByIds(string ids)
        /// <summary>查询所有相关记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public IList<EntitySchemaInfo> FindAllByIds(string ids)
        {
            return this.provider.FindAllByIds(ids);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="EntitySchemaInfo"/></returns> 
        public IList<EntitySchemaInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:GetEntityClassName(Type type)
        /// <summary>获取实体类名称</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetEntityClassName(Type type)
        {
            string objectType = KernelContext.ParseObjectType(type);

            EntitySchemaInfo param = this.FindOneByEntityClassFullName(objectType);

            return param == null ? objectType : param.EntityClassName;
        }
        #endregion
    }
}
