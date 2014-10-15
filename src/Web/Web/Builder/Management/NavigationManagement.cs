using System.Collections.Generic;

using X3Platform.Configuration;
using X3Platform.Membership;
using X3Platform.Web.Builder.ILayouts;
using X3Platform.Web.Configuration;

namespace X3Platform.Web.Builder.Management
{
    /// <summary></summary>
    public class NavigationManagement
    {
        private WebConfiguration configuration;

        private IList<INavigationBuilder> list = new List<INavigationBuilder>();

        public NavigationManagement()
        {
            Load();
        }

        public void Load()
        {
            configuration = WebConfigurationView.Instance.Configuration;

            NameValueConfigurationCollection collection = configuration.Navigation;

            INavigationBuilder builder = null;

            foreach (NameValueConfigurationElement element in collection)
            {
                builder = (INavigationBuilder)KernelContext.CreateObject(element.Value);

                list.Add(builder);
            }
        }

        #region 函数:GetStartMenu(IAccountInfo account)
        public string GetStartMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetStartMenu(account);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetStartMenu(account);
        }
        #endregion

        #region 函数:GetTopMenu(IAccountInfo account)
        /// <summary>获取顶部菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetTopMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetTopMenu(account);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetTopMenu(account);
        }
        #endregion

        #region 函数:GetBottomMenu(IAccountInfo account)
        public string GetBottomMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetBottomMenu(account);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetBottomMenu(account);
        }
        #endregion

        #region 函数:GetShortcutMenu(IAccountInfo account)
        /// <summary>获取顶部菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetShortcutMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetShortcutMenu(account);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetShortcutMenu(account);
        }
        #endregion

        #region 函数:GetApplicationMenu(IAccountInfo account, string applicationName)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationMenu(IAccountInfo account, string applicationName)
        {
            return GetApplicationMenu(account, applicationName, string.Empty);
        }
        #endregion

        #region 函数:GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetApplicationMenu(account, applicationName, parentMenuFullPath);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetApplicationMenu(account, applicationName, parentMenuFullPath);
        }
        #endregion

        #region 函数:GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetApplicationMenu(account, applicationName, options);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetApplicationMenu(account, applicationName, options);
        }
        #endregion

        #region 函数:GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        /// <summary>获取应用功能菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // 自定义构建信息
                    return builder.GetApplicationFunctionMenu(account, applicationFunctionName);
                }
            }

            // 默认构建信息
            return BuilderContext.Instance.NavigationBuilder.GetApplicationFunctionMenu(account, applicationFunctionName);
        }
        #endregion
    }
}