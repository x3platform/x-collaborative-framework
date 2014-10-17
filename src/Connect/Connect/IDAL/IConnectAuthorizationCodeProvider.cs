namespace X3Platform.Connect.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership.Scope;
    using X3Platform.Spring;

    using X3Platform.Connect.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IDAL.IConnectAuthorizationCodeProvider")]
    public interface IConnectAuthorizationCodeProvider
    {
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">ConnectAuthorizationCodeInfo Id号</param>
        /// <returns>返回一个<see cref="ConnectAuthorizationCodeInfo" />实例的详细信息</returns>
        ConnectAuthorizationCodeInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByAccountId(string appKey, string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个实例<see cref="ConnectAuthorizationCodeInfo"/>的详细信息</returns>
        ConnectAuthorizationCodeInfo FindOneByAccountId(string appKey, string accountId);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="ConnectAuthorizationCodeInfo" />实例的详细信息</returns>
        IList<ConnectAuthorizationCodeInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectAuthorizationCodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectAuthorizationCodeInfo" />实例详细信息</param>
        /// <returns><see cref="ConnectAuthorizationCodeInfo" />实例详细信息</returns>
        ConnectAuthorizationCodeInfo Save(ConnectAuthorizationCodeInfo param);
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
        IList<ConnectAuthorizationCodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id"><see cref="ConnectAuthorizationCodeInfo" />实例详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExist(string appKey, string accountId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string appKey, string accountId);
        #endregion
    }
}
