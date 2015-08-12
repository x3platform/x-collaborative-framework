#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs
{
    #region Using Libraries
    using System;
    using System.Reflection;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Logging;
    using X3Platform.Plugins;
    using X3Platform.Spring;

    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Plugins.Bugs.IBLL;
    #endregion

    /// <summary>问题跟踪上下文环境</summary>
    public sealed class BugContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "Bug"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile BugContext instance = null;

        private static object lockObject = new object();

        public static BugContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new BugContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private BugConfiguration configuration = null;

        /// <summary>配置</summary>
        public BugConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:BugService
        private IBugService m_BugService = null;

        /// <summary>问题信息</summary>
        public IBugService BugService
        {
            get { return m_BugService; }
        }
        #endregion

        #region 属性:BugCategoryService
        private IBugCategoryService m_BugCategoryService = null;

        /// <summary>问题类别信息</summary>
        public IBugCategoryService BugCategoryService
        {
            get { return m_BugCategoryService; }
        }
        #endregion

        #region 属性:BugHistoryService
        private IBugHistoryService m_BugHistoryService = null;

        public IBugHistoryService BugHistoryService
        {
            get { return m_BugHistoryService; }
        }
        #endregion

        #region 属性:BugCommentService
        private IBugCommentService m_BugCommentService = null;

        public IBugCommentService BugCommentService
        {
            get { return m_BugCommentService; }
        }
        #endregion

        #region 构造函数:BugContext()
        /// <summary><see cref="BugContext"/>的构造函数</summary>
        private BugContext()
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
            ApplicationInfo application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

            ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            applicationEvent.ApplicationId = application.Id;
            applicationEvent.Tags = "信息";
            applicationEvent.Description = string.Format("【{0}】应用执行{1}事件。", application.ApplicationDisplayName, (this.restartCount == 0 ? "初始化" : "第" + this.restartCount + "次重启"));

            applicationEvent.Start();

            try
            {
                this.Reload();
            
                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
                applicationEvent.Tags = "错误";
                applicationEvent.Description = string.Format("【错误】{0}错误信息:{1}", applicationEvent.Description, ex.Message);

                throw ex;
            }
            finally
            {
                applicationEvent.Finish();

                AppsContext.Instance.ApplicationEventService.Save(applicationEvent);
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
                BugConfigurationView.Instance.Reload();
            }

            this.configuration = BugConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_BugService = objectBuilder.GetObject<IBugService>(typeof(IBugService));
            this.m_BugCategoryService = objectBuilder.GetObject<IBugCategoryService>(typeof(IBugCategoryService));
            this.m_BugCommentService = objectBuilder.GetObject<IBugCommentService>(typeof(IBugCommentService));
            this.m_BugHistoryService = objectBuilder.GetObject<IBugHistoryService>(typeof(IBugHistoryService));
        }
        #endregion
    }
}
