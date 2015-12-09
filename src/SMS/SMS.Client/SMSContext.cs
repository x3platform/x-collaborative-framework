namespace X3Platform.SMS.Client
{
    #region Using Libraries
    using System;
    using System.Timers;

    using X3Platform.Membership;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.SMS.Client.Configuration;
    using X3Platform.SMS.Client.IBLL;
    #endregion

    /// <summary>会话上下文环境</summary>
    public sealed class SMSContext : CustomPlugin
    {
        #region 静态属性:Instance
        private static volatile SMSContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static SMSContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SMSContext();
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

        #region 属性:SMSService
        private ISMSService m_SMSService = null;

        /// <summary>帐号缓存服务</summary>
        public ISMSService SMSService
        {
            get { return this.m_SMSService; }
        }
        #endregion

        #region 构造函数:SMSContext()
        /// <summary>构造函数</summary>
        private SMSContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();
            }
            catch
            {
                throw;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = SMSConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(SMSConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_SMSService = objectBuilder.GetObject<ISMSService>(typeof(ISMSService));
        }
        #endregion
    }
}