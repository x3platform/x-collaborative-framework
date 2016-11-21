namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Timers;

    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    using X3Platform.Configuration;
    using X3Platform.Membership;
    using X3Platform.Security.Authentication;

    using Common.Logging;

    using X3Platform.Util;
    #endregion

    /// <summary>核心环境</summary>
    public sealed class KernelContext : IContext
    {
        /// <summary>日志记录器</summary>
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>日志记录</summary>
        public static ILog Log
        {
            get { return Current.logger; }
        }

        private Timer timer = new Timer();

        #region 属性:Current
        private static volatile KernelContext instance = null;

        private static object lockObject = new object();

        /// <summary>当前信息</summary>
        public static KernelContext Current
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new KernelContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        public string Name
        {
            get { return "核心环境"; }
        }
        #endregion

        #region 属性:User
        /// <summary>用户信息</summary>
        public IAccountInfo User
        {
            get { return this.AuthenticationManagement.GetAuthUser(); }
        }
        #endregion

        #region 属性:Configuration
        private KernelConfiguration configuration = null;

        /// <summary>配置信息</summary>
        public KernelConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:AuthenticationManagement
        private IAuthenticationManagement authenticationManagement = null;

        /// <summary>验证管理</summary>
        public IAuthenticationManagement AuthenticationManagement
        {
            get { return authenticationManagement; }
        }
        #endregion

        #region 构造函数:KernelContext()
        private KernelContext()
        {
            this.Reload();
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        public void Reload()
        {
            this.configuration = KernelConfigurationView.Instance.Configuration;

            string authenticationManagementType = KernelConfigurationView.Instance.AuthenticationManagementType;

            this.authenticationManagement = (IAuthenticationManagement)CreateObject(authenticationManagementType);

            // -------------------------------------------------------
            // 设置定时器
            // -------------------------------------------------------

            // 定时清理临时目录下的临时文件 设置为 3 天
            if (!Directory.Exists(KernelConfigurationView.Instance.ApplicationTempPathRoot))
            {
                DirectoryHelper.Create(KernelConfigurationView.Instance.ApplicationTempPathRoot);
            }

            timer.Enabled = true;

            // 每天检测一次目录
            timer.Interval = 24 * 60 * 60 * 1000;

            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                try
                {
                    string[] files = Directory.GetFiles(KernelConfigurationView.Instance.ApplicationTempPathRoot, "*.*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        DateTime createdTime = File.GetCreationTime(file);

                        if (createdTime.AddDays(KernelConfigurationView.Instance.ApplicationTempFileRemoveTimerInterval) < DateTime.Now)
                        {
                            File.Delete(file);

                            logger.Info("Delete expired temporary file:" + file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            };

            timer.Start();
        }
        #endregion

        // -------------------------------------------------------
        // 工具函数
        // -------------------------------------------------------

        #region 函数:CreateObject(string type)
        /// <summary>创建对象</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateObject(string type)
        {
            return CreateObject(type, null);
        }
        #endregion

        #region 函数:CreateObject(string type, object[] args)
        /// <summary>创建对象</summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateObject(string type, params object[] args)
        {
            if (string.IsNullOrEmpty(type)) { return null; }

            Type objectType = Type.GetType(type);

            if (objectType == null) { return null; }

            return Activator.CreateInstance(objectType, args);
        }
        #endregion

        #region 函数:ParseObjectType(Type type)
        /// <summary>解析对象类型</summary>
        /// <param name="type"></param>
        /// <returns>格式:X3Platform.KernelContext,X3Platform</returns>
        public static string ParseObjectType(Type type)
        {
            if (type == null)
                return null;

            string assemblyQualifiedName = type.AssemblyQualifiedName;

            int length = assemblyQualifiedName.IndexOf(',', assemblyQualifiedName.IndexOf(',') + 1);

            return assemblyQualifiedName.Substring(0, length);
        }
        #endregion
    }
}
