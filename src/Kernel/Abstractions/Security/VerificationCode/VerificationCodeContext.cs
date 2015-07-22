using System;
using System.Reflection;

using Common.Logging;

using X3Platform.Plugins;
using X3Platform.Spring;
using X3Platform.Security.VerificationCode.Configuration;
using X3Platform.Security.VerificationCode.IBLL;

namespace X3Platform.Security.VerificationCode
{
    /// <summary>权限引擎</summary>
    public sealed class VerificationCodeContext : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 静态属性:Instance
        private static volatile VerificationCodeContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static VerificationCodeContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new VerificationCodeContext();
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
            get { return "权限"; }
        }
        #endregion

        #region 属性:VerificationCodeService
        private IVerificationCodeService m_VerificationCodeService = null;

        /// <summary>权限服务</summary>
        public IVerificationCodeService VerificationCodeService
        {
            get { return m_VerificationCodeService; }
        }
        #endregion

        #region 构造函数:VerificationCodeContext()
        /// <summary>构造函数</summary>
        private VerificationCodeContext()
        {
            Reload();
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
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = VerificationCodeConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(VerificationCodeConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_VerificationCodeService = objectBuilder.GetObject<IVerificationCodeService>(typeof(IVerificationCodeService));
        }
        #endregion
    }
}
