namespace X3Platform.Entities.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IDAL.IEntitySchemaProvider")]
    public interface IEntitySchemaProvider
    {
        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        void BeginTransaction();
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        void CommitTransaction();
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(EntitySchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntitySchemaInfo"/>详细信息</returns>
        EntitySchemaInfo Save(EntitySchemaInfo param);
        #endregion

        #region 函数:Insert(EntitySchemaInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        void Insert(EntitySchemaInfo param);
        #endregion

        #region 函数:Update(EntitySchemaInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        void Update(EntitySchemaInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        EntitySchemaInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        EntitySchemaInfo FindOneByName(string name);
        #endregion

        #region 函数:FindOneByEntityClassName(string entityClassName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        EntitySchemaInfo FindOneByEntityClassName(string entityClassName);
        #endregion

        #region 函数:FindOneByEntityClassFullName(string entityClassFullName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        EntitySchemaInfo FindOneByEntityClassFullName(string entityClassFullName);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        IList<EntitySchemaInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByIds(string ids)
        /// <summary>查询所有相关记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        IList<EntitySchemaInfo> FindAllByIds(string ids);
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
        IList<EntitySchemaInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistCode(string code, string ignoreIds = null)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="code">代码</param>
        /// <param name="ignoreIds">忽略对象的标识，多个以逗号隔开</param>
        /// <returns>布尔值</returns>
        bool IsExistCode(string code, string ignoreIds = null);
        #endregion

        #region 函数:IsExistName(string name, string ignoreIds = null)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">名称</param>
        /// <param name="ignoreIds">忽略对象的标识，多个以逗号隔开</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name, string ignoreIds = null);
        #endregion

        #region 函数:IsExistEntityClassName(string entityClassName, string ignoreIds = null)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="ignoreIds">忽略对象的标识，多个以逗号隔开</param>
        /// <returns>布尔值</returns>
        bool IsExistEntityClassName(string entityClassName, string ignoreIds = null);
        #endregion

        #region 函数:SetCode(string id, string code)
        /// <summary>设置对象的代码</summary>
        /// <param name="id">标识</param>
        /// <param name="code">代码</param>
        /// <returns>布尔值</returns>
        int SetCode(string id, string code);
        #endregion
    }
}
