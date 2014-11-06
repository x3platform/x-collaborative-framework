namespace X3Platform.Plugins.Contacts
{
    using System;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Spring;

    using X3Platform.Plugins.Contacts.IBLL;
    using X3Platform.Plugins.Contacts.Configuration;

    /// <summary>联系人</summary>
    public class ContactContext : CustomPlugin
    {
        #region 属性:Name
        public override string Name
        {
            get { return "收藏夹"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile ContactContext instance = null;

        private static object lockObject = new object();

        public static ContactContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ContactContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private ContactConfiguration configuration = null;

        /// <summary>配置</summary>
        public ContactConfiguration Configuration
        {
            get { return this.configuration; }
        }
        #endregion

        #region 属性:ContactService
        private IContactService m_ContactService = null;

        /// <summary>收藏夹服务</summary>
        public IContactService ContactService
        {
            get { return this.m_ContactService; }
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 构造函数:ContactContext()
        /// <summary>构造函数</summary>
        private ContactContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            //ApplicationInfo application = AppsContext.Instance.ApplicationService[ContactConfiguration.ApplicationName];

            //ApplicationEventInfo applicationEvent = new ApplicationEventInfo();

            //applicationEvent.Id = DigitalNumberContext.Generate("Key_Guid");

            //applicationEvent.ApplicationId = application.Id;
            //applicationEvent.Tags = "信息";
            //applicationEvent.Description = string.Format("【{0}】应用执行{1}事件。", application.ApplicationDisplayName, (this.restartCount == 0 ? "初始化" : "第" + this.restartCount + "次重启"));

            //applicationEvent.Start();

            try
            {
                // 重新加载信息
                this.Reload();

                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
               // applicationEvent.Tags = "错误";
               // applicationEvent.Description = string.Format("【错误】{0}错误信息:{1}", applicationEvent.Description, ex.Message);

                throw ex;
            }
            finally
            {
               // applicationEvent.Finish();

               // AppsContext.Instance.ApplicationEventService.Save(applicationEvent);
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
                ContactConfigurationView.Instance.Reload();
            }

            this.configuration = ContactConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ContactConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.m_ContactService = objectBuilder.GetObject<IContactService>(typeof(IContactService));
        }
        #endregion
    }
}
