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

        #region 函数:ParsePage(IAccountInfo account)
        /// <summary>获取顶部菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.ParsePage(authorizationObjectType, authorizationObjectId, pageName);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.CustomizeBuilder.ParsePage(authorizationObjectType, authorizationObjectId, pageName);
        }
        #endregion

        #region 函数:ParsePage(IAccountInfo account)
        /// <summary>获取顶部菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParsePage(IAccountInfo account, string pageName)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.ParsePage(account, pageName);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.CustomizeBuilder.ParsePage(account, pageName);
        }
        #endregion

        #region 函数:ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        public string ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.ParseHomePage(authorizationObjectType, authorizationObjectId);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.CustomizeBuilder.ParseHomePage(authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 函数:ParseHomePage(IAccountInfo account)
        public string ParseHomePage(IAccountInfo account)
        {
            foreach (ICustomizeBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.ParseHomePage(account);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.CustomizeBuilder.ParseHomePage(account);
        }
        #endregion
    }
}