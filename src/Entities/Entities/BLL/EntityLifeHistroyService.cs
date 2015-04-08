namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    public class EntityLifeHistoryService : IEntityLifeHistoryService
    {
        /// <summary>数据提供器</summary>
        private IEntityLifeHistoryProvider provider = null;

        #region 构造函数:EntityLifeHistoryService()
        /// <summary>构造函数</summary>
        public EntityLifeHistoryService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = EntitiesConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(EntitiesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            provider = objectBuilder.GetObject<IEntityLifeHistoryProvider>(typeof(IEntityLifeHistoryProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityLifeHistoryInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(EntityLifeHistoryInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntityLifeHistoryInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityLifeHistoryInfo"/>详细信息</returns>
        public EntityLifeHistoryInfo Save(EntityLifeHistoryInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        ///<summary>删除记录</summary>
        ///<param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        ///<summary>查询某条记录</summary>
        ///<param name="id">标识</param>
        ///<returns>返回实例<see cref="EntityLifeHistoryInfo"/>的详细信息</returns>
        public EntityLifeHistoryInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        ///<summary>查询所有相关记录</summary>
        ///<returns>返回所有实例<see cref="EntityLifeHistoryInfo"/>的详细信息</returns>
        public IList<EntityLifeHistoryInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<returns>返回所有实例<see cref="EntityLifeHistoryInfo"/>的详细信息</returns>
        public IList<EntityLifeHistoryInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有实例<see cref="EntityLifeHistoryInfo"/>的详细信息</returns>
        public IList<EntityLifeHistoryInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
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
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="EntityLifeHistoryInfo"/></returns>
        public IList<EntityLifeHistoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        ///<summary>查询是否存在相关的记录.</summary>
        ///<param name="id">标识</param>
        ///<returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:Log(string methodName, EntityClass entity, string contextDiffLog)
        /// <summary>保存日志信息</summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="entity">实体类</param>
        /// <param name="contextDiffLog">上下文差异记录</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        public int Log(string methodName, EntityClass entity, string contextDiffLog)
        {
            return this.Log(methodName, entity.EntityId, entity.EntityClassName, contextDiffLog);
        }
        #endregion

        #region 函数:Log(string methodName, string entityId, string entityClassName, string contextDiffLog)
        /// <summary>保存日志信息</summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="contextDiffLog">上下文差异记录</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        public int Log(string methodName, string entityId, string entityClassName, string contextDiffLog)
        {
            IAccountInfo account = KernelContext.Current.User;

            // 保存实体数据操作记录
            EntityLifeHistoryInfo param = new EntityLifeHistoryInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");
            param.AccountId = account == null ? Guid.Empty.ToString() : account.Id;
            param.MethodName = methodName;
            param.EntityId = entityId;
            param.EntityClassName = entityClassName;
            param.ContextDiffLog = contextDiffLog;

            this.Save(param);

            return 0;
        }
        #endregion
    }
}
