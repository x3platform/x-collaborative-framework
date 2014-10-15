namespace X3Platform.Apps
{
    #region Using Libraries
    using System.Collections.Generic;

    using X3Platform.Apps.Model;
    using X3Platform.Membership;
    #endregion

    /// <summary>应用安全管理</summary>
    public sealed class AppsSecurity
    {
        private static IDictionary<string, ApplicationInfo> applicationCache = null;

        private static object lockObject = new object();

        #region 函数:FindApplication(string applicationName)
        /// <summary>查找应用的信息</summary>
        public static ApplicationInfo FindApplication(string applicationName)
        {
            ApplicationInfo application = null;

            lock (lockObject)
            {
                if (applicationCache == null)
                {
                    applicationCache = new Dictionary<string, ApplicationInfo>();
                }

                if (applicationCache.ContainsKey(applicationName))
                {
                    application = applicationCache[applicationName];
                }
                else
                {
                    application = AppsContext.Instance.ApplicationService.FindOneByApplicationName(applicationName);

                    if (application != null)
                    {
                        applicationCache.Add(applicationName, application);
                    }
                }
            }

            return application;
        }
        #endregion

        #region 函数:ResetApplicationAdministrators(string applicationName)
        /// <summary>重置应用管理员</summary>
        public static void ResetApplicationAdministrators(string applicationName)
        {
            lock (lockObject)
            {
                if (applicationCache != null && applicationCache.ContainsKey(applicationName))
                {
                    applicationCache.Remove(applicationName);
                }
            }
        }
        #endregion

        #region 函数:IsAdministrator(IAccountInfo account, string applicationName)
        /// <summary>判断用户是否是应用的默认管理员</summary>
        public static bool IsAdministrator(IAccountInfo account, string applicationName)
        {
            ApplicationInfo application = FindApplication(applicationName);

            return application == null ? false : AppsContext.Instance.ApplicationService.IsAdministrator(account, application.Id);
        }
        #endregion

        #region 函数:IsReviewer(IAccountInfo account, string applicationName)
        /// <summary>判断用户是否是应用的默认审查员</summary>
        public static bool IsReviewer(IAccountInfo account, string applicationName)
        {
            ApplicationInfo application = FindApplication(applicationName);

            return application == null ? false : AppsContext.Instance.ApplicationService.IsReviewer(account, application.Id);
        }
        #endregion

        #region 函数:IsMember(IAccountInfo account, string applicationName)
        /// <summary>判断用户是否是应用的默认可访问成员</summary>
        public static bool IsMember(IAccountInfo account, string applicationName)
        {
            ApplicationInfo application = FindApplication(applicationName);

            if (application == null)
                return false;

            // 管理员 默认是应用的成员
            if (AppsContext.Instance.ApplicationService.IsAdministrator(account, application.Id))
                return true;

            // 审查员 默认是应用的成员
            if (AppsContext.Instance.ApplicationService.IsReviewer(account, application.Id))
                return true;

            // 默认的普通成员情况
            return AppsContext.Instance.ApplicationService.IsMember(account, application.Id);
        }
        #endregion

        #region 函数:HasAuthorizedFeature(IAccountInfo account, string applicationFeatureName)
        /// <summary>判断用户是否是拥有授权的功能的</summary>
        public static bool HasAuthorizedFeature(IAccountInfo account, string applicationFeatureName)
        {
            ApplicationInfo application = FindApplication(applicationFeatureName);

            if (application == null)
                return false;

            // 管理员 默认是应用的成员
            if (AppsContext.Instance.ApplicationService.IsAdministrator(account, application.Id))
                return true;

            // 审查员 默认是应用的成员
            if (AppsContext.Instance.ApplicationService.IsReviewer(account, application.Id))
                return true;

            // 默认的普通成员情况
            return AppsContext.Instance.ApplicationService.IsMember(account, application.Id);
        }
        #endregion
    }
}
