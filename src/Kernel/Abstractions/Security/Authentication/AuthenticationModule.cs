using System;
using System.Web;

namespace X3Platform.Security.Authentication
{
    /// <summary></summary>
    public class AuthenticationModule : IHttpModule
    {
        /// <summary>构造函数</summary>
        public AuthenticationModule()
        {
        }

        /// <summary>初始化函数</summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            //
            // 若用户已经通过认证
            //

            //IAccountInfo account = (IAccountInfo)KernelContext.Instance.User;

            //if (account != null)
            //{
            //    application.Context.User = new AuthenticationPrincipal((IMemberInfo)account);
            //}
        }

        #region 函数:Dispose()
        public void Dispose()
        {
        }
        #endregion
    }
}