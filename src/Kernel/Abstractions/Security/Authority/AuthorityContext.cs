// =============================================================================
//
// Copyright (c) x3platfrom.com
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

using System;
using System.Reflection;

using Common.Logging;

using X3Platform.Plugins;
using X3Platform.Spring;
using X3Platform.Security.Authority.Configuration;
using X3Platform.Security.Authority.IBLL;

namespace X3Platform.Security.Authority
{
    /// <summary>Ȩ������</summary>
    public sealed class AuthorityContext : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ��̬属性:Instance
        private static volatile AuthorityContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static AuthorityContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AuthorityContext();
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
            get { return "Ȩ��"; }
        }
        #endregion

        #region 属性:Configuration
        private AuthorityConfiguration configuration = null;

        /// <summary>����</summary>
        public AuthorityConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:AuthorityService
        private IAuthorityService m_AuthorityService = null;

        /// <summary>Ȩ�޷���</summary>
        public IAuthorityService AuthorityService
        {
            get { return m_AuthorityService; }
        }
        #endregion

        #region ���캯��:AuthorityContext()
        /// <summary>���캯��</summary>
        private AuthorityContext()
        {
            Reload();
        }
        #endregion

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                Reload();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        public void Reload()
        {
            configuration = AuthorityConfigurationView.Instance.Configuration;

            this.m_AuthorityService = SpringContext.Instance.GetObject<IAuthorityService>(typeof(IAuthorityService));
        }
        #endregion
    }
}
