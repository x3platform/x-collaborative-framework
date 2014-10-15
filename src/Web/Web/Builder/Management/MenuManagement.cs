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

        #region 函数:GetMenu(IAccountInfo account, string applicationName)
        /// <summary>获取菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetMenu(account, applicationName);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.MenuBuilder.GetMenu(account, applicationName);
        }
        #endregion

        #region 函数:GetMenu(IAccountInfo account, string applicationName, string applicationMenuPathName)
        /// <summary>获取菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName, string applicationMenuPathName)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetMenu(account, applicationName, applicationMenuPathName);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.MenuBuilder.GetMenu(account, applicationName, applicationMenuPathName);
        }
        #endregion

        #region 函数:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>获取菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetMenu(account, applicationName, options);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.MenuBuilder.GetMenu(account, applicationName, options);
        }
        #endregion

        #region 函数:ParsePage(IAccountInfo account, string menuName)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName)
        {
            return ParseMenu(account, menuName, AppsSecurity.IsAdministrator(account, menuName));
        }
        #endregion

        #region 函数:ParsePage(IAccountInfo account, string menuName, bool isAdminToken)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken)
        {
            return ParseMenu(account, menuName, isAdminToken, new VelocityContext());
        }
        #endregion

        #region 函数:ParsePage(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        {
            foreach (IMenuBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.ParseMenu(account, menuName, isAdminToken, context);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.MenuBuilder.ParseMenu(account, menuName, isAdminToken, context);
        }
        #endregion
    }
}