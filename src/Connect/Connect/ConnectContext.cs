namespace X3Platform.Connect
{
    #region Using Libraries
    using System;
    using System.Reflection;

    using Common.Logging;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IBLL;
    #endregion

    /// <summary>应用连接器管理上下文环境</summary>
    public sealed class ConnectContext : CustomPlugin
    {
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        #region 属性:Name
        public override string Name
        {
            get { return "Connect"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile ConnectContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static ConnectContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:ConnectService
        private IConnectService m_ConnectService = null;

        /// <summary>连接器信息</summary>
        public IConnectService ConnectService
        {
            get { return this.m_ConnectService; }
        }
        #endregion

        #region 属性:ConnectAuthorizationCodeService
        private IConnectAuthorizationCodeService m_ConnectAuthorizationCodeService = null;

        /// <summary>连接器授权码信息</summary>
        public IConnectAuthorizationCodeService ConnectAuthorizationCodeService
        {
            get { return this.m_ConnectAuthorizationCodeService; }
        }
        #endregion

        #region 属性:ConnectAccessTokenService
        private IConnectAccessTokenService m_ConnectAccessTokenService = null;

        /// <summary>连接器访问令牌信息</summary>
        public IConnectAccessTokenService ConnectAccessTokenService
        {
            get { return this.m_ConnectAccessTokenService; }
        }
        #endregion

        #region 属性:ConnectCallService
        private IConnectCallService m_ConnectCallService = null;

        /// <summary>连接器调用记录信息</summary>
        public IConnectCallService ConnectCallService
        {
            get { return this.m_ConnectCallService; }
        }
        #endregion

        #region 构造函数:ConnectContext()
        /// <summary><see cref="ConnectContext"/>的构造函数</summary>
        private ConnectContext()
        {
            Restart();
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
                logger.Error(ex);

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
                // 重新加载配置信息
                ConnectConfigurationView.Instance.Reload();
            }

            // 创建对象构建器(Spring.NET)
            string springObjectFile = ConnectConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_ConnectService = objectBuilder.GetObject<IConnectService>(typeof(IConnectService));
            this.m_ConnectAccessTokenService = objectBuilder.GetObject<IConnectAccessTokenService>(typeof(IConnectAccessTokenService));
            this.m_ConnectAuthorizationCodeService = objectBuilder.GetObject<IConnectAuthorizationCodeService>(typeof(IConnectAuthorizationCodeService));
            this.m_ConnectCallService = objectBuilder.GetObject<IConnectCallService>(typeof(IConnectCallService));
        }
        #endregion
    }
}
