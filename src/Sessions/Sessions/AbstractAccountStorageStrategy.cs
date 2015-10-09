namespace X3Platform.Sessions
{
    #region Using Libraries
    using System;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Xml;

    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public abstract class AbstractAccountStorageStrategy : IAccountStorageStrategy
    {
        /// <summary></summary>
        public AbstractAccountStorageStrategy() { }

        #region 方法:Deserialize(AccountCacheInfo accountCache)
        /// <summary>反序列化缓存信息信息</summary>
        /// <returns></returns>
        public virtual IAccountInfo Deserialize(AccountCacheInfo accountCache)
        {
            if (accountCache == null) { return null; }

            return this.DeserializeObject(accountCache);
        }
        #endregion

        #region 方法:DeserializeObject(AccountCacheInfo accountCache)
        /// <summary>将缓存信息反序列化为帐号信息</summary>
        /// <returns></returns>
        protected abstract IAccountInfo DeserializeObject(AccountCacheInfo accountCache);
        #endregion

        #region 方法:Serialize(string sessionId, IAccountInfo account)
        /// <summary>序列化缓存信息</summary>
        /// <returns></returns>
        public virtual AccountCacheInfo Serialize(string sessionId, IAccountInfo account)
        {
            return Serialize(string.Empty, sessionId, account);
        }
        #endregion

        #region 方法:Serialize(string appKey, string sessionId, IAccountInfo account)
        /// <summary>序列化缓存信息</summary>
        /// <returns></returns>
        public virtual AccountCacheInfo Serialize(string appKey, string sessionId, IAccountInfo account)
        {
            AccountCacheInfo accountCache = new AccountCacheInfo();

            accountCache.AccountIdentity = sessionId;

            accountCache.AppKey = appKey;

            accountCache.AccountCacheValue = account.LoginName;

            accountCache.AccountObject = this.SerializeObject(account);

            accountCache.AccountObjectType = KernelContext.ParseObjectType(account.GetType());

            accountCache.IP = account.IP;

            accountCache.ValidFrom = DateTime.Now;

            accountCache.ValidTo = accountCache.ValidFrom.AddMonths(3);

            accountCache.Date = DateTime.Now;

            return accountCache;
        }
        #endregion

        #region 方法:SerializeObject(IAccountInfo account)
        /// <summary>将帐号信息序列化为字符串信息</summary>
        /// <returns></returns>
        protected virtual string SerializeObject(IAccountInfo account)
        {
            StringBuilder outString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");

            outString.Append("<accountObject>");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", account.Id);
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", account.Code);
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", account.Name);
            outString.AppendFormat("<loginName><![CDATA[{0}]]></loginName>", account.LoginName);
            outString.AppendFormat("<globalName><![CDATA[{0}]]></globalName>", account.GlobalName);
            outString.AppendFormat("<displayName><![CDATA[{0}]]></displayName>", account.DisplayName);
            outString.AppendFormat("<type><![CDATA[{0}]]></type>", account.Type);
            outString.AppendFormat("<certifiedAvatar><![CDATA[{0}]]></certifiedAvatar>", account.CertifiedAvatar);
            outString.AppendFormat("<certifiedEmail><![CDATA[{0}]]></certifiedEmail>", account.CertifiedEmail);
            outString.AppendFormat("<certifiedTelephone><![CDATA[{0}]]></certifiedTelephone>", account.CertifiedTelephone);
            outString.AppendFormat("<ip><![CDATA[{0}]]></ip>", account.IP);
            outString.Append("</accountObject>");

            return outString.ToString();
        }
        #endregion
    }
}
