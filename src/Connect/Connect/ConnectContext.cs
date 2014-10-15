#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Connect
{
    #region Using Libraries
    using System;
    using System.Reflection;

    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IBLL;
    #endregion

    /// <summary>Ӧ�����������������Ļ���</summary>
    public sealed class ConnectContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "Connect"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile ConnectContext instance = null;

        private static object lockObject = new object();

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

        #region 属性:Configuration
        private ConnectConfiguration configuration = null;

        /// <summary>����</summary>
        public ConnectConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:ConnectService
        private IConnectService m_ConnectService = null;

        /// <summary>��������Ϣ</summary>
        public IConnectService ConnectService
        {
            get { return this.m_ConnectService; }
        }
        #endregion

        #region 属性:ConnectAuthorizationCodeService
        private IConnectAuthorizationCodeService m_ConnectAuthorizationCodeService = null;

        /// <summary>��������Ȩ����Ϣ</summary>
        public IConnectAuthorizationCodeService ConnectAuthorizationCodeService
        {
            get { return this.m_ConnectAuthorizationCodeService; }
        }
        #endregion

        #region 属性:ConnectAccessTokenService
        private IConnectAccessTokenService m_ConnectAccessTokenService = null;

        /// <summary>����������������Ϣ</summary>
        public IConnectAccessTokenService ConnectAccessTokenService
        {
            get { return this.m_ConnectAccessTokenService; }
        }
        #endregion

        #region 属性:ConnectCallService
        private IConnectCallService m_ConnectCallService = null;

        /// <summary>���������ü�¼��Ϣ</summary>
        public IConnectCallService ConnectCallService
        {
            get { return this.m_ConnectCallService; }
        }
        #endregion

        #region ���캯��:ConnectContext()
        /// <summary><see cref="ConnectContext"/>�Ĺ��캯��</summary>
        private ConnectContext()
        {
            Restart();
        }
        #endregion

        /// <summary>��������������</summary>
        private int restartCount = 0;

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();

                // ������������������
                this.restartCount++;
            }
            catch
            {
                throw;
            }
            
            return 0;
        }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // ���¼���������Ϣ
                ConnectConfigurationView.Instance.Reload();
            }

            this.configuration = ConnectConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // �������ݷ�������
            this.m_ConnectService = objectBuilder.GetObject<IConnectService>(typeof(IConnectService));
            this.m_ConnectAccessTokenService = objectBuilder.GetObject<IConnectAccessTokenService>(typeof(IConnectAccessTokenService));
            this.m_ConnectAuthorizationCodeService = objectBuilder.GetObject<IConnectAuthorizationCodeService>(typeof(IConnectAuthorizationCodeService));
            this.m_ConnectCallService = objectBuilder.GetObject<IConnectCallService>(typeof(IConnectCallService));
        }
        #endregion
    }
}
