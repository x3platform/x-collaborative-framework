namespace X3Platform.Membership
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Xml;

    using X3Platform.Sessions;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    public sealed class GenericAccountStorageStrategy : AbstractAccountStorageStrategy
    {
        /// <summary></summary>
        public GenericAccountStorageStrategy() { }

        /// <summary>将缓存信息反序列化为帐号信息</summary>
        /// <param name="accountCache"></param>
        /// <returns></returns>
        protected override IAccountInfo DeserializeObject(AccountCacheInfo accountCache)
        {
            AccountInfo account = new AccountInfo();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(accountCache.AccountObject);

            account.Id = doc.SelectSingleNode("accountObject/id").InnerText;

            if (doc.SelectSingleNode("accountObject/code") != null)
                account.Code = doc.SelectSingleNode("accountObject/code").InnerText;
            if (doc.SelectSingleNode("accountObject/name") != null)
                account.Name = doc.SelectSingleNode("accountObject/name").InnerText;
            if (doc.SelectSingleNode("accountObject/loginName") != null)
                account.LoginName = doc.SelectSingleNode("accountObject/loginName").InnerText;
            if (doc.SelectSingleNode("accountObject/globalName") != null)
                account.GlobalName = doc.SelectSingleNode("accountObject/globalName").InnerText;
            if (doc.SelectSingleNode("accountObject/displayName") != null)
                account.DisplayName = doc.SelectSingleNode("accountObject/displayName").InnerText;
            if (doc.SelectSingleNode("accountObject/type") != null)
                account.Type = Convert.ToInt32(doc.SelectSingleNode("accountObject/type").InnerText);
            if (doc.SelectSingleNode("accountObject/certifiedAvatar") != null)
                account.CertifiedAvatar = doc.SelectSingleNode("accountObject/certifiedAvatar").InnerText;
            if (doc.SelectSingleNode("accountObject/certifiedEmail") != null)
                account.CertifiedEmail = doc.SelectSingleNode("accountObject/certifiedEmail").InnerText;
            if (doc.SelectSingleNode("accountObject/certifiedTelephone") != null)
                account.CertifiedTelephone = doc.SelectSingleNode("accountObject/certifiedTelephone").InnerText;
            if (doc.SelectSingleNode("accountObject/ip") != null)
                account.IP = doc.SelectSingleNode("accountObject/ip").InnerText;

            return account;
        }
    }
}
