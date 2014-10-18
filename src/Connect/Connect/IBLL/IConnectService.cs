namespace X3Platform.Connect.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Connect.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IBLL.IConnectService")]
    public interface IConnectService
    {
        #region 索引:this[string appKey]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ConnectInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">ConnectInfo Id号</param>
        /// <returns>返回一个<see cref="ConnectInfo"/>实例的详细信息</returns>
        ConnectInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByAppKey(string appKey)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>返回一个实例<see cref="ConnectInfo"/>的详细信息</returns>
        ConnectInfo FindOneByAppKey(string appKey);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有<see cref="ConnectInfo"/>实例的详细信息</returns>
        IList<ConnectInfo> FindAll();
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="ConnectInfo"/>实例的详细信息</returns>
        IList<ConnectInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectInfo"/>实例详细信息</param>
        /// <returns><see cref="ConnectInfo"/>实例详细信息</returns>
        ConnectInfo Save(ConnectInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        int Delete(string id);
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
        /// <returns>返回一个列表实例</returns> 
        IList<ConnectInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="CostInfo"/></returns>
        IList<ConnectQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistAppKey(string appKey)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>布尔值</returns>
        bool IsExistAppKey(string appKey);
        #endregion

        #region 函数:ResetAppSecret(string appKey)
        /// <summary>重置应用链接器的密钥</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns></returns>
        int ResetAppSecret(string appKey);
        #endregion

        #region 函数:ResetAppSecret(string appKey, string appSecret)
        /// <summary>重置应用链接器的密钥</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <returns></returns>
        int ResetAppSecret(string appKey, string appSecret);
        #endregion
    }
}
