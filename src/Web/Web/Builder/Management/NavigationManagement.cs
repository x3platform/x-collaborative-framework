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

        #region ����:GetStartMenu(IAccountInfo account)
        public string GetStartMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetStartMenu(account);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetStartMenu(account);
        }
        #endregion

        #region ����:GetTopMenu(IAccountInfo account)
        /// <summary>��ȡ�����˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetTopMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetTopMenu(account);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetTopMenu(account);
        }
        #endregion

        #region ����:GetBottomMenu(IAccountInfo account)
        public string GetBottomMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetBottomMenu(account);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetBottomMenu(account);
        }
        #endregion

        #region ����:GetShortcutMenu(IAccountInfo account)
        /// <summary>��ȡ�����˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetShortcutMenu(IAccountInfo account)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetShortcutMenu(account);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetShortcutMenu(account);
        }
        #endregion

        #region ����:GetApplicationMenu(IAccountInfo account, string applicationName)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationMenu(IAccountInfo account, string applicationName)
        {
            return GetApplicationMenu(account, applicationName, string.Empty);
        }
        #endregion

        #region ����:GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetApplicationMenu(account, applicationName, parentMenuFullPath);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetApplicationMenu(account, applicationName, parentMenuFullPath);
        }
        #endregion

        #region ����:GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>��ȡӦ�ò˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetApplicationMenu(account, applicationName, options);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetApplicationMenu(account, applicationName, options);
        }
        #endregion

        #region ����:GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        /// <summary>��ȡӦ�ù��ܲ˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        {
            foreach (INavigationBuilder builder in list)
            {
                if (((CommonBuilder)builder).IsActiveDomain)
                {
                    // �Զ��幹����Ϣ
                    return builder.GetApplicationFunctionMenu(account, applicationFunctionName);
                }
            }

            // Ĭ�Ϲ�����Ϣ
            return BuilderContext.Instance.NavigationBuilder.GetApplicationFunctionMenu(account, applicationFunctionName);
        }
        #endregion
    }
}