namespace X3Platform.Sessions.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Sessions.IBLL.IAccountCacheService")]
    public interface IAccountCacheService
    {
        #region 索引:this[string accountIdentity]
        /// <summary>缓存的索引</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns></returns>
        AccountCacheInfo this[string accountIdentity] { get; }
        #endregion

        #region 属性:GetAuthAccount(IAccountStorageStrategy strategy, string accountIdentity)
        /// <summary>获取当前帐号缓存信息</summary>
        /// <param name="strategy">帐号存储策略</param>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        IAccountInfo GetAuthAccount(IAccountStorageStrategy strategy, string accountIdentity);
        #endregion

        #region 函数:Authorize(string accountIdentity)
        /// <summary>查找授权信息</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns></returns>
        bool Authorize(string accountIdentity);
        #endregion

        #region 函数:Authorize(string accountIdentity, string appKey)
        /// <summary>查找授权信息</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="appKey">App Key</param>
        /// <returns></returns>
        bool Authorize(string accountIdentity, string appKey);
        #endregion

        #region 函数:Read(string accountIdentity)
        /// <summary>查找缓存记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        AccountCacheInfo Read(string accountIdentity);
        #endregion

        #region 函数:Read(string accountIdentity)
        /// <summary>查找缓存记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="appKey">App Key</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        // AccountCacheInfo Read(string accountIdentity, string appKey);
        #endregion

        #region 函数:ReadWithAccountCacheValue(string accountCacheValue)
        /// <summary>查找缓存记录</summary>
        /// <param name="accountCacheValue">缓存的值</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        AccountCacheInfo ReadWithAccountCacheValue(string accountCacheValue);
        #endregion

        //#region 函数:ReadWithAccountCacheValue(string accountCacheValue, string ip)
        ///// <summary>查找缓存记录</summary>
        ///// <param name="accountCacheValue">缓存的值</param>
        ///// <param name="ip">访问者IP</param>
        ///// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        //AccountCacheInfo ReadWithAccountCacheValue(string accountCacheValue, string ip);
        //#endregion

        #region 函数:Write(IAccountStorageStrategy strategy)
        /// <summary>写入信息</summary>
        /// <param name="strategy">策略</param>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="account">帐号信息</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        void Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account);
        #endregion

        #region 函数:FindByAccountIdentity(string accountIdentity)
        /// <summary>根据查找某条记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        AccountCacheInfo FindByAccountIdentity(string accountIdentity);
        #endregion

        #region 函数:FindByAccountCacheValue(string accountCacheValue)
        /// <summary>查找某条记录</summary>
        /// <param name="value">缓存的值</param>
        /// <returns>返回一个 实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        AccountCacheInfo FindByAccountCacheValue(string accountCacheValue);
        #endregion

        #region 函数:Dump()
        /// <summary>转储所有记录信息</summary>
        /// <returns>返回一个<see cref="AccountCacheInfo"/>列表</returns>
        IList<AccountCacheInfo> Dump();
        #endregion

        #region 函数:Insert(AccountCacheInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="AccountCacheInfo"/>的详细信息</param>
        void Insert(AccountCacheInfo param);
        #endregion

        #region 函数:Update(AccountCacheInfo param)
        /// <summary>更新记录</summary>
        /// <param name="param">实例<see cref="AccountCacheInfo"/>的详细信息</param>
        void Update(AccountCacheInfo param);
        #endregion

        #region 函数:Delete(string accountIdentity)
        /// <summary>删除记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        int Delete(string accountIdentity);
        #endregion

        #region 函数:IsExist(string accountIdentity)
        /// <summary>检测记录是否存在</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        bool IsExist(string accountIdentity);
        #endregion

        #region 函数:IsExistAccountCacheValue(string accountCacheValue)
        /// <summary>检测记录是否存在</summary>
        /// <param name="accountCacheValue">帐号缓存值</param>
        bool IsExistAccountCacheValue(string accountCacheValue);
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
