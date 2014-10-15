namespace X3Platform.Membership.HumanResources
{
    using System;

    using X3Platform.Apps;
    using X3Platform.Membership.HumanResources.IBLL;
    using X3Platform.Membership.HumanResources.Configuration;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    /// <summary>人力资源管理</summary>
    public class HumanResourceManagement : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "人力资源管理"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile HumanResourceManagement instance = null;

        private static object lockObject = new object();

        public static HumanResourceManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new HumanResourceManagement();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        HumanResourcesConfiguration configuration = null;

        /// <summary>配置</summary>
        public HumanResourcesConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:HumanResourceOfficerService
        private IHumanResourceOfficerService m_HumanResourceOfficerService = null;

        /// <summary>人力资源事务员服务</summary>
        public IHumanResourceOfficerService HumanResourceOfficerService
        {
            get { return m_HumanResourceOfficerService; }
        }
        #endregion

        #region 属性:GeneralAccountService
        private IGeneralAccountService m_GeneralAccountService = null;

        /// <summary>普通帐号服务</summary>
        public IGeneralAccountService GeneralAccountService
        {
            get { return m_GeneralAccountService; }
        }
        #endregion

        #region 构造函数:HumanResourceManagement()
        /// <summary>构造函数</summary>
        private HumanResourceManagement()
        {
            Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                Reload();
            }
            catch
            {
                throw;
            }

            return 0;
        }
        #endregion

        public void Reload()
        {
            this.configuration = HumanResourcesConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(HumanResourcesConfiguration.ApplicationName, springObjectFile);

            this.m_HumanResourceOfficerService = objectBuilder.GetObject<IHumanResourceOfficerService>(typeof(IHumanResourceOfficerService));
            this.m_GeneralAccountService = objectBuilder.GetObject<IGeneralAccountService>(typeof(IGeneralAccountService));
        }

        public static bool IsHumanResourceOfficer(IAccountInfo account)
        {
            if (AppsSecurity.IsAdministrator(KernelContext.Current.User, HumanResourcesConfiguration.ApplicationName))
            {
                return true;
            }

            return Instance.HumanResourceOfficerService.IsHumanResourceOfficer(account);
        }
    }
}
