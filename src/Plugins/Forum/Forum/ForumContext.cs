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

    /// <summary>��̳���������Ļ���</summary>
    public sealed class ForumContext : CustomPlugin
    {
        #region ����:Name
        public override string Name
        {
            get { return "��̳����"; }
        }
        #endregion

        #region ��̬����:Instance
        private static volatile ForumContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
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

        #region ����:ForumCategoryService
        private IForumCategoryService m_ForumCategoryService = null;

        /// <summary>��̳������</summary>
        public IForumCategoryService ForumCategoryService
        {
            get { return this.m_ForumCategoryService; }
        }
        #endregion

        #region ����:ForumThreadService
        private IForumThreadService m_ForumThreadService = null;

        /// <summary>��̳�������</summary>
        public IForumThreadService ForumThreadService
        {
            get { return this.m_ForumThreadService; }
        }
        #endregion

        #region ����:ForumCommentService
        private IForumCommentService m_ForumCommentService = null;

        /// <summary>��̳���۷���</summary>
        public IForumCommentService ForumCommentService
        {
            get { return this.m_ForumCommentService; }
        }
        #endregion

        #region ����:ForumMemberService
        private IForumMemberService m_ForumMemberService = null;

        /// <summary>��̳��Ա����</summary>
        public IForumMemberService ForumMemberService
        {
            get { return this.m_ForumMemberService; }
        }
        #endregion

        #region ����:ForumFollowService
        private IForumFollowService m_ForumFollowService = null;

        /// <summary>��̳��ע����</summary>
        public IForumFollowService ForumFollowService
        {
            get { return this.m_ForumFollowService; }
        }
        #endregion

        /// <summary>��������������</summary>
        private int restartCount = 0;

        #region ���캯��:ForumContext()
        /// <summary>���캯��</summary>
        private ForumContext()
        {
            this.Restart();
        }
        #endregion

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            ApplicationInfo application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

            ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            applicationEvent.ApplicationId = application.Id;
            applicationEvent.Tags = "��Ϣ";
            applicationEvent.Description = string.Format("��{0}��Ӧ��ִ��{1}�¼���", application.ApplicationDisplayName, (this.restartCount == 0 ? "��ʼ��" : "��" + this.restartCount + "������"));

            applicationEvent.Start();

            try
            {
                // ���¼�����Ϣ
                this.Reload();

                // ������������������
                this.restartCount++;
            }
            catch (Exception ex)
            {
                applicationEvent.Tags = "����";
                applicationEvent.Description = string.Format("������{0}������Ϣ:{1}", applicationEvent.Description, ex.Message);

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

        #region ˽�к���:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // ���¼���������Ϣ
                ForumConfigurationView.Instance.Reload();
            }

            // �������󹹽���(Spring.NET)
            string springObjectFile = ForumConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
            this.m_ForumCategoryService = objectBuilder.GetObject<IForumCategoryService>(typeof(IForumCategoryService));
            this.m_ForumThreadService = objectBuilder.GetObject<IForumThreadService>(typeof(IForumThreadService));
            this.m_ForumCommentService = objectBuilder.GetObject<IForumCommentService>(typeof(IForumCommentService));
            this.m_ForumMemberService = objectBuilder.GetObject<IForumMemberService>(typeof(IForumMemberService));
            this.m_ForumFollowService = objectBuilder.GetObject<IForumFollowService>(typeof(IForumFollowService));
        }
        #endregion
    }
}
