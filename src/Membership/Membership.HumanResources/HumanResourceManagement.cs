namespace X3Platform.Membership.HumanResources
{
    using System;

    using X3Platform.Apps;
    using X3Platform.Membership.HumanResources.IBLL;
    using X3Platform.Membership.HumanResources.Configuration;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    /// <summary>������Դ����</summary>
    public class HumanResourceManagement : CustomPlugin
    {
        #region ����:Name
        public override string Name
        {
            get { return "������Դ����"; }
        }
        #endregion

        #region ����:Instance
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

        #region ����:Configuration
        HumanResourcesConfiguration configuration = null;

        /// <summary>����</summary>
        public HumanResourcesConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ����:HumanResourceOfficerService
        private IHumanResourceOfficerService m_HumanResourceOfficerService = null;

        /// <summary>������Դ����Ա����</summary>
        public IHumanResourceOfficerService HumanResourceOfficerService
        {
            get { return m_HumanResourceOfficerService; }
        }
        #endregion

        #region ����:GeneralAccountService
        private IGeneralAccountService m_GeneralAccountService = null;

        /// <summary>��ͨ�ʺŷ���</summary>
        public IGeneralAccountService GeneralAccountService
        {
            get { return m_GeneralAccountService; }
        }
        #endregion

        #region ���캯��:HumanResourceManagement()
        /// <summary>���캯��</summary>
        private HumanResourceManagement()
        {
            Restart();
        }
        #endregion

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
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

            // �������󹹽���(Spring.NET)
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
