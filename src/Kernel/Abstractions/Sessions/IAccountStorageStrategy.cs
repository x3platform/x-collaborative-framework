namespace X3Platform.Sessions
{
    using X3Platform.Membership;

    /// <summary>帐号存储策略接口</summary>
    public interface IAccountStorageStrategy
    {
        #region 方法:Deserialize(AccountCacheInfo accountCache)
        /// <summary>反序列化缓存信息信息</summary>
        /// <returns></returns>
        IAccountInfo Deserialize(AccountCacheInfo accountCache);
        #endregion

        #region 方法:Serialize(string accountIdentity, IAccountInfo account)
        /// <summary>序列化缓存信息信息</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="account">帐号信息</param>
        /// <returns></returns>
        AccountCacheInfo Serialize(string accountIdentity, IAccountInfo account);
        #endregion
    }
}