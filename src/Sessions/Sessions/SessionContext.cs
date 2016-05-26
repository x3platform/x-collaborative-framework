namespace X3Platform.Sessions
{
    #region Using Libraries
    using System;
    using System.Timers;

    using X3Platform.Membership;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Sessions.Configuration;
    using X3Platform.Sessions.IBLL;
    using X3Platform.Globalization;
    #endregion

    /// <summary>会话上下文环境</summary>
    public sealed class SessionContext : CustomPlugin
    {
        #region 静态属性:Instance
        private static volatile SessionContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static SessionContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SessionContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        public override string Name
        {
            get { return "会话管理"; }
        }
        #endregion

        private static Timer timer = new Timer();

        #region 属性:AccountCacheService
        private IAccountCacheService m_AccountCacheService = null;

        /// <summary>帐号缓存服务</summary>
        public IAccountCacheService AccountCacheService
        {
            get { return this.m_AccountCacheService; }
        }
        #endregion

        #region 构造函数:SessionContext()
        /// <summary>构造函数</summary>
        private SessionContext()
        {
            this.Restart();
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();

                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
                KernelContext.Log.Error(ex.Message, ex);
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                KernelContext.Log.Info(string.Format(I18n.Strings["application_is_reloading"], SessionsConfiguration.ApplicationName));

                // 重新加载配置信息
                SessionsConfigurationView.Instance.Reload();
            }
            else
            {
                KernelContext.Log.Info(string.Format(I18n.Strings["application_is_loading"], SessionsConfiguration.ApplicationName));
            }

            // 创建对象构建器(Spring.NET)
            string springObjectFile = SessionsConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(SessionsConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_AccountCacheService = objectBuilder.GetObject<IAccountCacheService>(typeof(IAccountCacheService));

            // 初始化的时候清理缓存
            this.AccountCacheService.Clear(DateTime.Now.AddHours(-6));

            // -------------------------------------------------------
            // 设置定时器
            // -------------------------------------------------------

            // 由数据库来定时任务来清理过期会话 
            timer.Enabled = true;

            timer.Interval = SessionsConfigurationView.Instance.SessionTimerInterval * 60 * 1000;

            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                SessionContext.Instance.AccountCacheService.Clear(DateTime.Now.AddHours(-SessionsConfigurationView.Instance.SessionTimeLimit));
            };

            timer.Start();

            KernelContext.Log.Info(string.Format(I18n.Strings["application_is_successfully_loaded"], SessionsConfiguration.ApplicationName));
        }
        #endregion

        #region 函数:GetAuthAccount<T>(IAccountStorageStrategy strategy)
        /// <summary>获取当前验证的帐号信息</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strategy">存储策略</param>
        /// <param name="accountIdentity">存储策略</param>
        /// <returns></returns>
        public T GetAuthAccount<T>(IAccountStorageStrategy strategy, string accountIdentity) where T : IAccountInfo
        {
            return (T)this.AccountCacheService.GetAuthAccount(strategy, accountIdentity);
        }
        #endregion

        #region 函数:Contains(string accountIdentity)
        /// <summary>检测是否包含当前的键</summary>
        /// <param name="accountIdentity">键</param>
        /// <returns></returns>
        public bool Contains(string accountIdentity)
        {
            return this.m_AccountCacheService.IsExist(accountIdentity);
        }
        #endregion

        #region 函数:Read(string accountIdentity)
        /// <summary>读取帐号缓存信息</summary>
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        public AccountCacheInfo Read(string accountIdentity)
        {
            return this.m_AccountCacheService.Read(accountIdentity);
        }
        #endregion

        #region 函数:Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        /// <summary>写入帐号缓存信息</summary>
        /// <param name="strategy">存储策略</param>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="account">帐号信息</param>
        public void Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        {
            this.m_AccountCacheService.Write(strategy, string.Empty, accountIdentity, account);
        }
        #endregion

        #region 函数:Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        /// <summary>写入帐号缓存信息</summary>
        /// <param name="strategy">存储策略</param>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="account">帐号信息</param>
        public void Write(IAccountStorageStrategy strategy, string appKey, string accountIdentity, IAccountInfo account)
        {
            this.m_AccountCacheService.Write(strategy, appKey, accountIdentity, account);
        }
        #endregion
    }
}