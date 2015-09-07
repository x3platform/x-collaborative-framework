namespace X3Platform.Entities.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IBLL.IEntitySchemaService")]
    public interface IEntitySchemaService
    {
        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        EntitySchemaInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(EntitySchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntitySchemaInfo"/>详细信息</returns>
        EntitySchemaInfo Save(EntitySchemaInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
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

        #region 函数:FindOneByName(string entityClassName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        EntitySchemaInfo FindOneByEntityClassName(string entityClassName);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        IList<EntitySchemaInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        IList<EntitySchemaInfo> FindAll(string whereClause);
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

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="EntitySchemaInfo"/></returns>
        IList<EntitySchemaInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
