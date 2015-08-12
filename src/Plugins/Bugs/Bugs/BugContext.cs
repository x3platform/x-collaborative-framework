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

    /// <summary>������������Ļ���</summary>
    public sealed class BugContext : CustomPlugin
    {
        #region ����:Name
        public override string Name
        {
            get { return "Bug"; }
        }
        #endregion

        #region ����:Instance
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

        #region ����:Configuration
        private BugConfiguration configuration = null;

        /// <summary>����</summary>
        public BugConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ����:BugService
        private IBugService m_BugService = null;

        /// <summary>������Ϣ</summary>
        public IBugService BugService
        {
            get { return m_BugService; }
        }
        #endregion

        #region ����:BugCategoryService
        private IBugCategoryService m_BugCategoryService = null;

        /// <summary>���������Ϣ</summary>
        public IBugCategoryService BugCategoryService
        {
            get { return m_BugCategoryService; }
        }
        #endregion

        #region ����:BugHistoryService
        private IBugHistoryService m_BugHistoryService = null;

        public IBugHistoryService BugHistoryService
        {
            get { return m_BugHistoryService; }
        }
        #endregion

        #region ����:BugCommentService
        private IBugCommentService m_BugCommentService = null;

        public IBugCommentService BugCommentService
        {
            get { return m_BugCommentService; }
        }
        #endregion

        #region ���캯��:BugContext()
        /// <summary><see cref="BugContext"/>�Ĺ��캯��</summary>
        private BugContext()
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
            ApplicationInfo application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

            ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            applicationEvent.ApplicationId = application.Id;
            applicationEvent.Tags = "��Ϣ";
            applicationEvent.Description = string.Format("��{0}��Ӧ��ִ��{1}�¼���", application.ApplicationDisplayName, (this.restartCount == 0 ? "��ʼ��" : "��" + this.restartCount + "������"));

            applicationEvent.Start();

            try
            {
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

        #region ����:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // ���¼���������Ϣ
                BugConfigurationView.Instance.Reload();
            }

            this.configuration = BugConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
            this.m_BugService = objectBuilder.GetObject<IBugService>(typeof(IBugService));
            this.m_BugCategoryService = objectBuilder.GetObject<IBugCategoryService>(typeof(IBugCategoryService));
            this.m_BugCommentService = objectBuilder.GetObject<IBugCommentService>(typeof(IBugCommentService));
            this.m_BugHistoryService = objectBuilder.GetObject<IBugHistoryService>(typeof(IBugHistoryService));
        }
        #endregion
    }
}
