namespace X3Platform.Storages.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Storages.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Storages.IBLL.IStorageSchemaService")]
    public interface IStorageSchemaService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StorageSchemaInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(StorageSchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="StorageSchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="StorageSchemaInfo"/>详细信息</returns>
        StorageSchemaInfo Save(StorageSchemaInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        StorageSchemaInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByApplicationId(string applicationId)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        StorageSchemaInfo FindOneByApplicationId(string applicationId);
        #endregion

        #region 函数:FindOneByApplicationName(string applicationName)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationName">所属应用名称</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        StorageSchemaInfo FindOneByApplicationName(string applicationName);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        IList<StorageSchemaInfo> FindAll();
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        IList<StorageSchemaInfo> FindAll(DataQuery query);
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
        /// <returns>返回一个列表实例<see cref="StorageSchemaInfo"/></returns>
        IList<StorageSchemaInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
