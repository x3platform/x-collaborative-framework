namespace X3Platform.Web.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;
  using System.Web.Security;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Configuration;
  using X3Platform.Json;
  using X3Platform.Location.IPQuery;
  using X3Platform.Membership;
  using X3Platform.Membership.Authentication;
  using X3Platform.Web.Mvc.Attributes;

  /// <summary>帐号基本信息</summary>
  public sealed class AccountController : CustomController
  {
    #region 函数:SignUp()
    /// <summary>注册</summary>
    public ActionResult SignUp()
    {
      return View("/views/" + LocateFolder("main") + "/account/sign-up.cshtml");
    }
    #endregion

    #region 函数:SignIn()
    /// <summary>登录</summary>
    public ActionResult SignIn()
    {
      return View("/views/" + LocateFolder("main") + "/account/sign-in.cshtml");
    }
    #endregion

    #region 函数:SignOut()
    /// <summary>注销</summary>
    public ActionResult SignOut()
    {
      string accountIdentityName = KernelContext.Current.AuthenticationManagement.IdentityName;

      // 获取当前用户信息
      IAccountInfo account = KernelContext.Current.User;

      KernelContext.Current.AuthenticationManagement.Logout();

      // -------------------------------------------------------
      // Session
      // -------------------------------------------------------

      Session.Abandon();

      HttpAuthenticationCookieSetter.ClearUserCookies();

      Response.Cookies[accountIdentityName].Value = null;
      Response.Cookies[accountIdentityName].Expires = DateTime.Now.AddDays(-1);

      // -------------------------------------------------------
      // IIdentity
      // -------------------------------------------------------

      if (this.User != null && this.User.Identity.IsAuthenticated)
      {
        FormsAuthentication.SignOut();
      }

      // 记录帐号操作日志
      MembershipManagement.Instance.AccountLogService.Log(account.Id, "退出", string.Format("【{0}】在 {1} 登录了系统。【IP:{2}】", account.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), IPQueryContext.GetClientIP()));

      return View("/views/" + LocateFolder("main") + "/account/sign-out.cshtml");
    }
    #endregion

    #region 函数:ForgotPassword()
    /// <summary>忘记密码</summary>
    public ActionResult ForgotPassword()
    {
      return View("/views/" + LocateFolder("main") + "/account/forgot-password.cshtml");
    }
    #endregion
  }
}
