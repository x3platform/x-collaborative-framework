namespace X3Platform.Location.Regions
{
    #region Using Libraries
    using System;
    using System.Reflection;

    using X3Platform.DigitalNumber;
    using X3Platform.Logging;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Location.Regions.Configuration;
    using X3Platform.Location.Regions.IBLL;
    #endregion

    /// <summary>������������Ļ���</summary>
    public sealed class RegionContext : CustomPlugin
    {
        #region ����:Name
        public override string Name
        {
            get { return "BackendManagement"; }
        }
        #endregion

        #region ����:Instance
        private static volatile RegionContext instance = null;

        private static object lockObject = new object();

        public static RegionContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new RegionContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ����:RegionService
        private IRegionService m_RegionService = null;

        public IRegionService RegionService
        {
            get { return m_RegionService; }
        }
        #endregion

        #region ���캯��:RegionContext()
        /// <summary><see cref="RegionContext"/>�Ĺ��캯��</summary>
        private RegionContext()
        {
            Restart();
        }
        #endregion

        /// <summary>��������������</summary>
        private int restartCount = 0;

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            this.Reload();

            return 0;
        }
        #endregion

        #region ����:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // ���¼���������Ϣ
                RegionsConfigurationView.Instance.Reload();
            }

            // �������󹹽���(Spring.NET)
            string springObjectFile = RegionsConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(RegionsConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
            this.m_RegionService = objectBuilder.GetObject<IRegionService>(typeof(IRegionService));
        }
        #endregion
    }
}
