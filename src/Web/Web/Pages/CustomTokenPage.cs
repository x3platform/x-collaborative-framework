namespace X3Platform.Web.Pages
{
    using System;
    using System.Web;

    using X3Platform.Configuration;
    using X3Platform.Web.Configuration;

    /// <summary>自定义页面</summary>
    public class CustomTokenPage : CustomPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (AccountCard == null)
            {
                if (KernelConfigurationView.Instance.AuthenticationManagementType == "X3Platform.Membership.Authentication.NLMAuthenticationManagement,X3Platform.Membership")
                {
                    Response.StatusCode = 401;
                    Response.Write("Unauthorized");
                    Response.AddHeader("WWW-Authenticate", "NTLM");
                    Response.End();
                }
                else
                {
                    Response.Redirect(string.Format(WebConfigurationView.Instance.SignInUrl, HttpUtility.UrlEncode(Request.Url.ToString())));
                }
            }
        }
    }
}