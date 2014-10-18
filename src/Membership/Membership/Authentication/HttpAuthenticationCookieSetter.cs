namespace X3Platform.Membership.Authentication
{
    #region Using Libraries
    using System;
    using System.Net;
    using System.Web;

    using X3Platform.Configuration;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>验证的HttpCookie设置器</summary>
    public sealed class HttpAuthenticationCookieSetter
    {
        /// <summary>设置当前用户信息</summary>
        public static void SetUserCookies(string accountIdentity)
        {
            string identityName = KernelContext.Current.AuthenticationManagement.IdentityName;

            // 单点登录功能启用状态下, 查找相关的根域名信息
            if (MembershipConfigurationView.Instance.SingleSignOn == "ON")
            {
                string domain = ParseDomain(HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == null ? KernelConfigurationView.Instance.Domain : HttpContext.Current.Request.ServerVariables["SERVER_NAME"]);

                if (domain != "localhost")
                {
                    HttpContext.Current.Response.Cookies[identityName].Domain = domain;
                }
            }

            HttpContext.Current.Response.Cookies[identityName].Value = accountIdentity;
            HttpContext.Current.Response.Cookies[identityName].HttpOnly = true;
        }

        /// <summary>清除当前用户信息</summary>
        public static void ClearUserCookies()
        {
            string identityName = KernelContext.Current.AuthenticationManagement.IdentityName;

            // 单点登录功能启用状态下, 查找相关的根域名信息
            if (MembershipConfigurationView.Instance.SingleSignOn == "ON")
            {
                string domain = ParseDomain(HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == null ? KernelConfigurationView.Instance.Domain : HttpContext.Current.Request.ServerVariables["SERVER_NAME"]);

                if (domain != "localhost")
                {
                    HttpContext.Current.Response.Cookies[identityName].Domain = domain;
                }
            }

            HttpContext.Current.Response.Cookies[identityName].Expires = DateTime.Now.AddDays(-1);
        }

        #region 函数:ParseDomain()
        /// <summary>分析域信息</summary>
        /// <returns></returns>
        public static string ParseDomain()
        {
            string serverNameView = HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == null ? KernelConfigurationView.Instance.Domain : HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            return ParseDomain(serverNameView);
        }
        #endregion

        #region 函数:ParseDomain(string serverNameView)
        /// <summary>分析域信息</summary>
        /// <param name="serverNameView"></param>
        /// <returns></returns>
        public static string ParseDomain(string serverNameView)
        {
            string domain = serverNameView;

            IPAddress address = IPAddress.None;

            if (IPAddress.TryParse(serverNameView, out address))
            {
                return domain;
            }

            int point = 0;

            if (serverNameView.LastIndexOf(".") == -1)
            {
                // 没有点的情况
                // => 不做处理, 设置为默认路径
            }
            else
            {
                point = serverNameView.LastIndexOf(".");

                if (serverNameView.Substring(0, point).LastIndexOf(".") == -1)
                {
                    // 只有一个点的情况
                    // => 例如 workspace.com 直接取 workspace.com  

                    domain = serverNameView;
                }
                else
                {
                    // 有两个或两个以上的点的情况
                    // => my.workspace.com 取 workspace.com

                    point = serverNameView.Substring(0, point).LastIndexOf(".") + 1;

                    domain = serverNameView.Substring(point, serverNameView.Length - point);
                }
            }

            return domain;
        }
        #endregion
    }
}