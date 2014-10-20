namespace X3Platform.Sessions.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Sessions.IDAL.IAccountCacheProvider")]
    public interface IAccountCacheProvider
    {
        #region 函数:FindByAccountIdentity(string accountIdentity)
        /// <summary>根据查找某条记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        AccountCacheInfo FindByAccountIdentity(string accountIdentity);
        #endregion

        #region 函数:FindByAccountCacheValue(string accountCacheValue)
        /// <summary>查找某条记录</summary>
        /// <param name="accountCacheValue">帐号缓存的值</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
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
        /// <summary>修改记录</summary>
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
    }
}
