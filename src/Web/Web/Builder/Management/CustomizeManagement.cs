namespace X3Platform.Web.Builder.Management
{
    using System.Collections.Generic;
    using X3Platform.Configuration;

    using X3Platform.Membership;
    using X3Platform.Web.Builder.ILayouts;
    using X3Platform.Web.Configuration;

    /// <summary></summary>
    public sealed class CustomizeManagement
    {
        private WebConfiguration configuration;
        
        private IList<ICustomizeBuilder> list = new List<ICustomizeBuilder>();

        public CustomizeManagement()
        {
            Load();
        }
        
        public void Load()
        {
            configuration = WebConfigurationView.Instance.Configuration;

            NameValueConfigurationCollection collection = configuration.Customize;

            ICustomizeBuilder builder = null;

            foreach (NameValueConfigurationElement element in collection)
            {
                builder = (ICustomizeBuilder)KernelContext.CreateObject(element.Value);

                list.Add(builder);
            }
        }

        #region ����:ParsePage(IAccountInfo account)
        /// <summary>��ȡ�����˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.ParsePage(authorizationObjectType, authorizationObjectId, pageName);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.CustomizeBuilder.ParsePage(authorizationObjectType, authorizationObjectId, pageName);
        }
        #endregion

        #region ����:ParsePage(IAccountInfo account)
        /// <summary>��ȡ�����˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParsePage(IAccountInfo account, string pageName)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.ParsePage(account, pageName);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.CustomizeBuilder.ParsePage(account, pageName);
        }
        #endregion

        #region ����:ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        public string ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.ParseHomePage(authorizationObjectType, authorizationObjectId);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.CustomizeBuilder.ParseHomePage(authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region ����:ParseHomePage(IAccountInfo account)
        public string ParseHomePage(IAccountInfo account)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.ParseHomePage(account);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.CustomizeBuilder.ParseHomePage(account);
        }
        #endregion
    }
}