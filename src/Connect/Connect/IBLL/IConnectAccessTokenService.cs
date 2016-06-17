namespace X3Platform.Connect.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Connect.Model;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IBLL.IConnectAccessTokenService")]
    public interface IConnectAccessTokenService
    {
        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ConnectAccessTokenInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">ConnectAccessTokenInfo Id号</param>
        /// <returns>返回一个 <see cref="ConnectAccessTokenInfo"/>实例的详细信息</returns>
        ConnectAccessTokenInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByAccountId(string appKey, string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        ConnectAccessTokenInfo FindOneByAccountId(string appKey, string accountId);
        #endregion

        #region 函数:FindOneByRefreshToken(string appKey, string refreshToken)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>返回一个实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        ConnectAccessTokenInfo FindOneByRefreshToken(string appKey, string refreshToken);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 <see cref="ConnectAccessTokenInfo"/>实例的详细信息</returns>
        IList<ConnectAccessTokenInfo> FindAll();
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 <see cref="ConnectAccessTokenInfo"/>实例的详细信息</returns>
        IList<ConnectAccessTokenInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectAccessTokenInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectAccessTokenInfo"/>实例详细信息</param>
        /// <returns><see cref="ConnectAccessTokenInfo"/>实例详细信息</returns>
        ConnectAccessTokenInfo Save(ConnectAccessTokenInfo param);
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
        IList<ConnectAccessTokenInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
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

        #region 函数:Write(string appKey, string accountId)
        /// <summary>写入的帐号的访问令牌信息</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns></returns>
        int Write(string appKey, string accountId);
        #endregion

        #region 函数:Refesh(string appKey, string refreshToken, DateTime expireDate)
        /// <summary>刷新帐号的访问令牌</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="refreshToken">刷新令牌</param>
        /// <param name="expireDate">过期时间</param>
        /// <returns></returns>
        int Refesh(string appKey, string refreshToken, DateTime expireDate);
        #endregion

        #region 函数:Clear(DateTime expiryTime)
        /// <summary>清理过期时间之前的缓存记录</summary>
        /// <param name="expiryTime">过期时间</param>
        int Clear(DateTime expiryTime);
        #endregion

        #region 函数:Clear()
        /// <summary>清空缓存记录</summary>
        int Clear();
        #endregion
    }
}
