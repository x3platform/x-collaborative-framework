#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AccountCacheService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Sessions.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    using X3Platform.Membership;
    using X3Platform.Spring;
    using X3Platform.Security;
    using X3Platform.Security.Authority;

    using X3Platform.Sessions.Configuration;
    using X3Platform.Sessions.IBLL;
    using X3Platform.Sessions.IDAL;
    using Common.Logging;
    #endregion

    /// <summary></summary>
    public class AccountCacheService : IAccountCacheService
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SessionsConfiguration configuration = null;

        private IAccountCacheProvider provider = null;

        private IDictionary<string, AccountCacheInfo> cacheStorage = null;

        #region ���캯��:AccountCacheService()
        /// <summary>���캯��</summary>
        public AccountCacheService()
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Sessions.BLL.AccountCacheService constructor() begin");
            }

            this.configuration = SessionsConfigurationView.Instance.Configuration;

            this.cacheStorage = new SyncDictionary<string, AccountCacheInfo>();

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(SessionsConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<IAccountCacheProvider>(typeof(IAccountCacheProvider));

            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Sessions.BLL.AccountCacheService constructor() end");
            }
        }
        #endregion

        #region 属性:this[string accountIdentity]
        /// <summary>����������</summary>
        /// <param name="accountIdentity">������ʶ</param>
        /// <returns></returns>
        public AccountCacheInfo this[string accountIdentity]
        {
            get { return this.Read(accountIdentity); }
        }
        #endregion

        #region 属性:GetAuthAccount(IAccountStorageStrategy strategy)
        /// <summary>��ȡ��ǰ�ʺŻ�����Ϣ</summary>
        /// <param name="strategy">�ʺŴ洢����</param>
        public IAccountInfo GetAuthAccount(IAccountStorageStrategy strategy, string accountIdentity)
        {
            AccountCacheInfo param = this.Read(accountIdentity);

            return strategy.Deserialize(param);
        }
        #endregion

        #region 属性:Authorize(string accountIdentity)
        /// <summary>������Ȩ��Ϣ</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns></returns>
        public bool Authorize(string accountIdentity) { return false; }
        #endregion

        #region 属性:Authorize(string accountIdentity, string appKey)
        /// <summary>������Ȩ��Ϣ</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="appKey">App Key</param>
        /// <returns></returns>
        public bool Authorize(string accountIdentity, string appKey) { return false; }
        #endregion

        /// <summary></summary>
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        public AccountCacheInfo Read(string accountIdentity)
        {
            // ���˿�ֵ
            if (string.IsNullOrEmpty(accountIdentity)) { return null; }

            if (this.cacheStorage.ContainsKey(accountIdentity))
            {
                return this.cacheStorage[accountIdentity];
            }
            else
            {
                AccountCacheInfo accountCacheInfo = this.FindByAccountIdentity(accountIdentity);

                if (accountCacheInfo != null && !this.cacheStorage.ContainsKey(accountIdentity))
                {
                    this.cacheStorage.Add(accountIdentity, accountCacheInfo);
                }

                return accountCacheInfo;
            }
        }

        /// <summary></summary>
        /// <param name="accountCacheValue"></param>
        /// <returns></returns>
        public AccountCacheInfo ReadWithAccountCacheValue(string accountCacheValue)
        {
            return this.FindByAccountCacheValue(accountCacheValue);
        }

        #region 属性:Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        ///<summary>д����Ϣ</summary>
        ///<param name="strategy">����</param>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="account">�ʺ���Ϣ</param>
        ///<returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        public void Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        {
            // ���˿�ֵ
            if (string.IsNullOrEmpty(accountIdentity)) { return; }

            AccountCacheInfo param = strategy.Serialize(accountIdentity, account);

            param.UpdateDate = DateTime.Now;

            // �����ֵ���Ϣ
            if (cacheStorage.ContainsKey(param.AccountIdentity))
            {
                cacheStorage[param.AccountIdentity] = param;
            }
            else
            {
                cacheStorage.Add(param.AccountIdentity, param);
            }

            // �������ݿ���Ϣ
            if (this.IsExist(param.AccountIdentity))
            {
                this.Update(param);
            }
            else
            {
                param.BeginDate = DateTime.Now;
                param.EndDate = DateTime.Now.AddHours(SessionsConfigurationView.Instance.SessionTimeLimit);

                this.Insert(param);
            }
        }
        #endregion

        #region 属性:FindByAccountIdentity(string accountIdentity)
        /// <summary>���ݲ���ĳ����¼</summary>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        public AccountCacheInfo FindByAccountIdentity(string accountIdentity)
        {
            return this.provider.FindByAccountIdentity(accountIdentity);
        }
        #endregion

        #region 属性:FindByAccountCacheValue(string cacheValue)
        ///<summary>���ݻ�����ֵ����ĳ����¼</summary>
        ///<param name="cacheValue">������ֵ</param>
        ///<returns>����һ�� AccountCacheInfo ʵ������ϸ��Ϣ</returns>
        public AccountCacheInfo FindByAccountCacheValue(string cacheValue)
        {
            return this.provider.FindByAccountCacheValue(cacheValue);
        }
        #endregion

        #region 属性:Dump()
        /// <summary>ת�����м�¼��Ϣ</summary>
        ///<returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        public IList<AccountCacheInfo> Dump()
        {
            return this.provider.Dump();
        }
        #endregion

        #region 属性:Insert(AccountCacheInfo param)
        ///<summary>���Ӽ�¼</summary>
        ///<param name="param">ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</param>
        public void Insert(AccountCacheInfo param)
        {
            this.provider.Insert(param);
        }
        #endregion

        #region 属性:Update(AccountCacheInfo param)
        ///<summary>�޸ļ�¼</summary>
        ///<param name="param">ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</param>
        public void Update(AccountCacheInfo param)
        {
            this.provider.Update(param);
        }
        #endregion

        #region 属性:Delete(string accountIdentity)
        ///<summary>ɾ����¼</summary>
        ///<param name="accountIdentity">�ʺű�ʶ</param>
        public int Delete(string accountIdentity)
        {
            return this.provider.Delete(accountIdentity);
        }
        #endregion

        #region 属性:IsExist(string accountIdentity)
        ///<summary>������¼�Ƿ�����</summary>
        public bool IsExist(string accountIdentity)
        {
            return this.provider.IsExist(accountIdentity);
        }
        #endregion

        #region 属性:IsExistAccountCacheValue(string accountIdentity)
        ///<summary>������¼�Ƿ�����</summary>
        public bool IsExistAccountCacheValue(string accountIdentity)
        {
            return this.provider.IsExistAccountCacheValue(accountIdentity);
        }
        #endregion

        #region 属性:Clear(DateTime expiryTime)
        /// <summary>��������ʱ��֮ǰ�Ļ�����¼</summary>
        /// <param name="expiryTime">����ʱ��</param>
        public int Clear(DateTime expiryTime)
        {
            return this.provider.Clear(expiryTime);
        }
        #endregion

        #region 属性:Clear()
        ///<summary>���ջ�����¼</summary>
        public int Clear()
        {
            return this.provider.Clear(DateTime.Now);
        }
        #endregion
    }
}
