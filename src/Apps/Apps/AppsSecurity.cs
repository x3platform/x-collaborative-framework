namespace X3Platform.Apps
{
    #region Using Libraries
    using System.Collections.Generic;

    using X3Platform.Apps.Model;
    using X3Platform.Membership;
    #endregion

    /// <summary>Ӧ�ð�ȫ����</summary>
    public sealed class AppsSecurity
    {
        private static IDictionary<string, ApplicationInfo> applicationCache = null;

        private static object lockObject = new object();

        #region ����:FindApplication(string applicationName)
        /// <summary>����Ӧ�õ���Ϣ</summary>
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

        #region ����:ResetApplicationAdministrators(string applicationName)
        /// <summary>����Ӧ�ù���Ա</summary>
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

        #region ����:IsAdministrator(IAccountInfo account, string applicationName)
        /// <summary>�ж��û��Ƿ���Ӧ�õ�Ĭ�Ϲ���Ա</summary>
        public static bool IsAdministrator(IAccountInfo account, string applicationName)
        {
            ApplicationInfo application = FindApplication(applicationName);

            return application == null ? false : AppsContext.Instance.ApplicationService.IsAdministrator(account, application.Id);
        }
        #endregion

        #region ����:IsReviewer(IAccountInfo account, string applicationName)
        /// <summary>�ж��û��Ƿ���Ӧ�õ�Ĭ�����Ա</summary>
        public static bool IsReviewer(IAccountInfo account, string applicationName)
        {
            ApplicationInfo application = FindApplication(applicationName);

            return application == null ? false : AppsContext.Instance.ApplicationService.IsReviewer(account, application.Id);
        }
        #endregion

        #region ����:IsMember(IAccountInfo account, string applicationName)
        /// <summary>�ж��û��Ƿ���Ӧ�õ�Ĭ�Ͽɷ��ʳ�Ա</summary>
        public static bool IsMember(IAccountInfo account, string applicationName)
        {
            ApplicationInfo application = FindApplication(applicationName);

            if (application == null)
                return false;

            // ����Ա Ĭ����Ӧ�õĳ�Ա
            if (AppsContext.Instance.ApplicationService.IsAdministrator(account, application.Id))
                return true;

            // ���Ա Ĭ����Ӧ�õĳ�Ա
            if (AppsContext.Instance.ApplicationService.IsReviewer(account, application.Id))
                return true;

            // Ĭ�ϵ���ͨ��Ա���
            return AppsContext.Instance.ApplicationService.IsMember(account, application.Id);
        }
        #endregion

        #region ����:HasAuthorizedFeature(IAccountInfo account, string applicationFeatureName)
        /// <summary>�ж��û��Ƿ���ӵ����Ȩ�Ĺ��ܵ�</summary>
        public static bool HasAuthorizedFeature(IAccountInfo account, string applicationFeatureName)
        {
            ApplicationInfo application = FindApplication(applicationFeatureName);

            if (application == null)
                return false;

            // ����Ա Ĭ����Ӧ�õĳ�Ա
            if (AppsContext.Instance.ApplicationService.IsAdministrator(account, application.Id))
                return true;

            // ���Ա Ĭ����Ӧ�õĳ�Ա
            if (AppsContext.Instance.ApplicationService.IsReviewer(account, application.Id))
                return true;

            // Ĭ�ϵ���ͨ��Ա���
            return AppsContext.Instance.ApplicationService.IsMember(account, application.Id);
        }
        #endregion
    }
}
