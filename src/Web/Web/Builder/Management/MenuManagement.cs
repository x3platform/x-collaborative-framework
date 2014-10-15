namespace X3Platform.Web.Builder.Management
{
    using System.Collections.Generic;
    using X3Platform.Configuration;

    using X3Platform.Apps;
    using X3Platform.Membership;
    using X3Platform.Web.Builder.ILayouts;
    using X3Platform.Web.Configuration;
    using X3Platform.Velocity;

    /// <summary></summary>
    public sealed class MenuManagement
    {
        private WebConfiguration configuration;

        private IList<IMenuBuilder> list = new List<IMenuBuilder>();

        /// <summary></summary>
        public MenuManagement()
        {
            Load();
        }

        /// <summary></summary>
        public void Load()
        {
            configuration = WebConfigurationView.Instance.Configuration;

            NameValueConfigurationCollection collection = configuration.Menu;

            IMenuBuilder builder = null;

            foreach (NameValueConfigurationElement element in collection)
            {
                builder = (IMenuBuilder)KernelContext.CreateObject(element.Value);

                list.Add(builder);
            }
        }

        #region ����:GetMenu(IAccountInfo account, string applicationName)
        /// <summary>��ȡ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetMenu(account, applicationName);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.MenuBuilder.GetMenu(account, applicationName);
        }
        #endregion

        #region ����:GetMenu(IAccountInfo account, string applicationName, string applicationMenuPathName)
        /// <summary>��ȡ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName, string applicationMenuPathName)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetMenu(account, applicationName, applicationMenuPathName);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.MenuBuilder.GetMenu(account, applicationName, applicationMenuPathName);
        }
        #endregion

        #region ����:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>��ȡ�˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetMenu(account, applicationName, options);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.MenuBuilder.GetMenu(account, applicationName, options);
        }
        #endregion

        #region ����:ParsePage(IAccountInfo account, string menuName)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName)
        {
            return ParseMenu(account, menuName, AppsSecurity.IsAdministrator(account, menuName));
        }
        #endregion

        #region ����:ParsePage(IAccountInfo account, string menuName, bool isAdminToken)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken)
        {
            return ParseMenu(account, menuName, isAdminToken, new VelocityContext());
        }
        #endregion

        #region ����:ParsePage(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.ParseMenu(account, menuName, isAdminToken, context);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.MenuBuilder.ParseMenu(account, menuName, isAdminToken, context);
        }
        #endregion
    }
}