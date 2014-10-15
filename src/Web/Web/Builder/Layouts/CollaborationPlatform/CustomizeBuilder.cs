namespace X3Platform.Web.Builder.Layouts.CollaborationPlatform
{
    using System;

    using X3Platform.Membership;
    using X3Platform.Web.Customize;
    using X3Platform.Web.Builder.ILayouts;

    public class CustomizeBuilder : CommonBuilder, ICustomizeBuilder
    {
        #region 函数:ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        /// <summary>解析页面</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="pageName">页面名称</param>
        /// <returns></returns>
        public string ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        {
            try
            {
                return CustomizeContext.Instance.PageService.TryParseHtml(authorizationObjectType, authorizationObjectId, pageName);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region 函数:ParsePage(IAccountInfo account, string pageName)
        /// <summary>解析页面</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="pageName">页面名称</param>
        /// <returns></returns>
        public string ParsePage(IAccountInfo account, string pageName)
        {
            if (account == null)
            {
                return string.Empty;
            }

            return this.ParsePage("Account", account.Id, pageName);
        }
        #endregion

        #region 函数:ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        /// <summary>解析主页</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns></returns>
        public string ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        {
            return this.ParsePage(authorizationObjectType, authorizationObjectId, "HomePage");
        }
        #endregion

        #region 函数:ParseHomePage(IAccountInfo account)
        /// <summary>解析主页</summary>
        /// <param name="account">帐号信息</param>
        /// <returns></returns>
        public string ParseHomePage(IAccountInfo account)
        {
            return this.ParsePage(account, "HomePage");
        }
        #endregion
    }
}
