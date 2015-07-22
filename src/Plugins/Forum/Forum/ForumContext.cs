namespace X3Platform.Plugins.Forum
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Spring;
    using X3Platform.Plugins;

    using X3Platform.Plugins.Forum.Configuration;
    using X3Platform.Plugins.Forum.IBLL;
    #endregion

    /// <summary>论坛管理上下文环境</summary>
    public sealed class ForumContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "论坛管理"; }
        }
        #endregion

        #region 静态属性:Instance
        private static volatile ForumContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static ForumContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ForumContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:ForumCategoryService
        private IForumCategoryService m_ForumCategoryService = null;

        /// <summary>论坛版块服务</summary>
        public IForumCategoryService ForumCategoryService
        {
            get { return this.m_ForumCategoryService; }
        }
        #endregion

        #region 属性:ForumThreadService
        private IForumThreadService m_ForumThreadService = null;

        /// <summary>论坛主题服务</summary>
        public IForumThreadService ForumThreadService
        {
            get { return this.m_ForumThreadService; }
        }
        #endregion

        #region 属性:ForumCommentService
        private IForumCommentService m_ForumCommentService = null;

        /// <summary>论坛评论服务</summary>
        public IForumCommentService ForumCommentService
        {
            get { return this.m_ForumCommentService; }
        }
        #endregion

        #region 属性:ForumMemberService
        private IForumMemberService m_ForumMemberService = null;

        /// <summary>论坛成员服务</summary>
        public IForumMemberService ForumMemberService
        {
            get { return this.m_ForumMemberService; }
        }
        #endregion

        #region 属性:ForumFollowService
        private IForumFollowService m_ForumFollowService = null;

        /// <summary>论坛关注服务</summary>
        public IForumFollowService ForumFollowService
        {
            get { return this.m_ForumFollowService; }
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 构造函数:ForumContext()
        /// <summary>构造函数</summary>
        private ForumContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            ApplicationInfo application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

            ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            applicationEvent.ApplicationId = application.Id;
            applicationEvent.Tags = "信息";
            applicationEvent.Description = string.Format("【{0}】应用执行{1}事件。", application.ApplicationDisplayName, (this.restartCount == 0 ? "初始化" : "第" + this.restartCount + "次重启"));

            applicationEvent.Start();

            try
            {
                // 重新加载信息
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

        #region 私有函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // 重新加载配置信息
                ForumConfigurationView.Instance.Reload();
            }

            // 创建对象构建器(Spring.NET)
            string springObjectFile = ForumConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_ForumCategoryService = objectBuilder.GetObject<IForumCategoryService>(typeof(IForumCategoryService));
            this.m_ForumThreadService = objectBuilder.GetObject<IForumThreadService>(typeof(IForumThreadService));
            this.m_ForumCommentService = objectBuilder.GetObject<IForumCommentService>(typeof(IForumCommentService));
            this.m_ForumMemberService = objectBuilder.GetObject<IForumMemberService>(typeof(IForumMemberService));
            this.m_ForumFollowService = objectBuilder.GetObject<IForumFollowService>(typeof(IForumFollowService));
        }
        #endregion
    }
}
