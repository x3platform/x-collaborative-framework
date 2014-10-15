namespace X3Platform.Web.Builder.ILayouts
{
    #region Using Libraries
    using X3Platform.Membership;
    #endregion

    /// <summary>自定义页面构建器</summary>
    public interface ICustomizeBuilder
    {
        #region 函数:ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName)
        /// <summary>解析页面</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="pageName">页面名称</param>
        /// <returns></returns>
        string ParsePage(string authorizationObjectType, string authorizationObjectId, string pageName);
        #endregion

        #region 函数:ParsePage(IAccountInfo account, string pageName)
        /// <summary>解析页面</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="pageName">页面名称</param>
        /// <returns></returns>
        string ParsePage(IAccountInfo account, string pageName);
        #endregion

        #region 函数:ParseHomePage(string authorizationObjectType, string authorizationObjectId)
        /// <summary>解析主页</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns></returns>
        string ParseHomePage(string authorizationObjectType, string authorizationObjectId);
        #endregion

        #region 函数:ParseHomePage(IAccountInfo account)
        /// <summary>解析主页</summary>
        /// <param name="account">帐号信息</param>
        /// <returns></returns>
        string ParseHomePage(IAccountInfo account);
        #endregion
    }
}