#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :SessionContext.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

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
    #endregion

    /// <summary>�Ự�����Ļ���</summary>
    public sealed class SessionContext : CustomPlugin
    {
        #region ��̬属性:Instance
        private static volatile SessionContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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
        /// <summary>����</summary>
        public override string Name
        {
            get { return "�Ự����"; }
        }
        #endregion

        private static Timer timer = new Timer();

        #region 属性:Configuration
        private SessionsConfiguration configuration = null;

        /// <summary>����</summary>
        public SessionsConfiguration Configuration
        {
            get { return this.configuration; }
        }
        #endregion

        #region 属性:AccountCacheService
        private IAccountCacheService m_AccountCacheService = null;

        /// <summary>�ʺŻ�������</summary>
        public IAccountCacheService AccountCacheService
        {
            get { return this.m_AccountCacheService; }
        }
        #endregion

        #region ���캯��:SessionContext()
        /// <summary>���캯��</summary>
        private SessionContext()
        {
            this.Restart();
        }
        #endregion

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
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

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            this.configuration = SessionsConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(SessionsConfiguration.ApplicationName, springObjectFile);

            // �������ݷ�������
            this.m_AccountCacheService = objectBuilder.GetObject<IAccountCacheService>(typeof(IAccountCacheService));

            // ��ʼ����ʱ��������
            this.AccountCacheService.Clear(DateTime.Now.AddHours(-6));

            // -------------------------------------------------------
            // ���ö�ʱ��
            // -------------------------------------------------------

            timer.Enabled = true;

            timer.Interval = SessionsConfigurationView.Instance.SessionTimerInterval * 60 * 1000;

            timer.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                SessionContext.Instance.AccountCacheService.Clear(DateTime.Now.AddHours(-SessionsConfigurationView.Instance.SessionTimeLimit));
            };

            timer.Start();
        }
        #endregion

        #region 属性:GetAuthAccount<T>(IAccountStorageStrategy strategy)
        /// <summary>��ȡ��ǰ��֤���ʺ���Ϣ</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strategy">�洢����</param>
        /// <param name="accountIdentity">�洢����</param>
        /// <returns></returns>
        public T GetAuthAccount<T>(IAccountStorageStrategy strategy, string accountIdentity) where T : IAccountInfo
        {
            return (T)this.AccountCacheService.GetAuthAccount(strategy, accountIdentity);
        }
        #endregion

        #region 属性:Contains(string accountIdentity)
        /// <summary>�����Ƿ�������ǰ�ļ�</summary>
        /// <param name="accountIdentity">��</param>
        /// <returns></returns>
        public bool Contains(string accountIdentity)
        {
            return this.m_AccountCacheService.IsExist(accountIdentity);
        }
        #endregion

        #region 属性:Read(string accountIdentity)
        /// <summary>��ȡ�ʺŻ�����Ϣ</summary>
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        public AccountCacheInfo Read(string accountIdentity)
        {
            return this.m_AccountCacheService.Read(accountIdentity);
        }
        #endregion

        #region 属性:Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        /// <summary>д���ʺŻ�����Ϣ</summary>
        /// <param name="strategy">�洢����</param>
        /// <param name="accountIdentity">�ʺŻỰΨһ��ʶ</param>
        /// <param name="account">�ʺ���Ϣ</param>
        public void Write(IAccountStorageStrategy strategy, string accountIdentity, IAccountInfo account)
        {
            this.m_AccountCacheService.Write(strategy, accountIdentity, account);
        }
        #endregion
    }
}