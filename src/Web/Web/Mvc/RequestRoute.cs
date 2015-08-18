namespace X3Platform.Web.Mvc
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Web.Mvc;
  using System.Web.Routing;
  using System.Web;
  using System.Xml;
  using System.Text.RegularExpressions;
  #endregion

  /// <summary>请求路由信息</summary>
  public class RequestRoute : RouteBase
  {
    /// <summary></summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public override RouteData GetRouteData(HttpContextBase httpContext)
    {
      // 获取相对路径
      var virtualPath = httpContext.Request.RawUrl;

      // 请求地址的前缀
      string prefixUrl = "/customizes/";

      // 判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
      if (virtualPath.IndexOf(prefixUrl) == 0)
      {
        // 请求地址的前缀长度
        int prefixUrlLength = prefixUrl.Length;

        // 符合规定的地址规则 {prefixUrl}{friendlyUrl}，截取后面的friendlyUrl
        string friendlyUrl = virtualPath.Substring(prefixUrlLength).Trim('/');

        if (friendlyUrl.LastIndexOf(".aspx") == (friendlyUrl.Length - prefixUrlLength))
        {
          friendlyUrl = friendlyUrl.Substring(0, friendlyUrl.Length - prefixUrlLength);
        }

        // 声明一个RouteData，添加相应的路由值
        var routeData = new RouteData(this, new MvcRouteHandler());

        // 限制名称空间
        routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Web.Mvc.Controllers" };

        Match match = null;

        if (string.IsNullOrEmpty(friendlyUrl) || friendlyUrl == "home")
        {
          // 主页
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "Index");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/list$"))
        {
          // 列表信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/list").Groups[1].Value));
          routeData.Values.Add("action", "List");
          // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form$"))
        {
          // 表单信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/form$").Groups[1].Value));
          routeData.Values.Add("action", "Form");
          // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[2].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$"))
        {
          // 表单信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[1].Value));
          routeData.Values.Add("action", "Form");
          routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[2].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/detail\?id=([\w+\-]+)$"))
        {
          // 详细信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/detail\?id=([\w+\-]+)$").Groups[1].Value));
          routeData.Values.Add("action", "Detail");
          routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/detail\?id=([\w+\-]+)$").Groups[2].Value + "\"}");
        }
        else
        {
          return null;
        }

        return routeData;
      }

      // 请求地址的前缀
      prefixUrl = "/account/";

      // 判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
      if (virtualPath.IndexOf(prefixUrl) == 0)
      {
        // 请求地址的前缀长度
        int prefixUrlLength = prefixUrl.Length;

        // 符合规定的地址规则 {prefixUrl}{friendlyUrl}，截取后面的friendlyUrl
        string friendlyUrl = virtualPath.Substring(prefixUrlLength).Trim('/');

        if (friendlyUrl.LastIndexOf(".aspx") == (friendlyUrl.Length - prefixUrlLength))
        {
          friendlyUrl = friendlyUrl.Substring(0, friendlyUrl.Length - prefixUrlLength);
        }

        // 声明一个RouteData，添加相应的路由值
        var routeData = new RouteData(this, new MvcRouteHandler());

        // 限制名称空间
        routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Web.Mvc.Controllers" };

        if (Regex.IsMatch(friendlyUrl, @"^sign-up$"))
        {
          // 注册
          routeData.Values.Add("controller", "Account");
          routeData.Values.Add("action", "SignUp");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^sign-in$"))
        {
          // 登录
          routeData.Values.Add("controller", "Account");
          routeData.Values.Add("action", "SignIn");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^sign-in\?returnUrl=([\w+\-\.\%]+)$"))
        {
          // 登录 ?returnUrl=http%3a%2f%2flocal.kernel.x3platform.com%2f
          routeData.Values.Add("controller", "Account");
          routeData.Values.Add("action", "SignIn");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^sign-out$"))
        {
          // 注销
          routeData.Values.Add("controller", "Account");
          routeData.Values.Add("action", "SignOut");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^forgot-password$"))
        {
          // 忘记密码
          routeData.Values.Add("controller", "Account");
          routeData.Values.Add("action", "ForgotPassword");
        }
        else
        {
          return null;
        }

        return routeData;
      }

      // 请求地址的前缀
      prefixUrl = "/sys/";

      // 判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
      if (virtualPath.IndexOf(prefixUrl) == 0)
      {
        // 请求地址的前缀长度
        int prefixUrlLength = prefixUrl.Length;

        // 符合规定的地址规则 {prefixUrl}{friendlyUrl}，截取后面的friendlyUrl
        string friendlyUrl = virtualPath.Substring(prefixUrlLength).Trim('/');

        if (friendlyUrl.LastIndexOf(".aspx") == (friendlyUrl.Length - prefixUrlLength))
        {
          friendlyUrl = friendlyUrl.Substring(0, friendlyUrl.Length - prefixUrlLength);
        }

        // 声明一个RouteData，添加相应的路由值
        var routeData = new RouteData(this, new MvcRouteHandler());

        // 限制名称空间
        routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Web.Mvc.Controllers" };

        if (string.IsNullOrEmpty(friendlyUrl))
        {
          // 系统环境
          routeData.Values.Add("controller", "Sys");
          routeData.Values.Add("action", "Environment");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^digital-number$"))
        {
          // 流水号设置
          routeData.Values.Add("controller", "Sys");
          routeData.Values.Add("action", "DigitalNumber");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^email-client$"))
        {
          // 邮箱设置
          routeData.Values.Add("controller", "Sys");
          routeData.Values.Add("action", "EmailClient");
        }
        else
        {
          return null;
        }

        return routeData;
      }

      return null;
    }

    /// <summary></summary>
    /// <param name="requestContext"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
    {
      return null;
    }

    /// <summary></summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string FriendlyControllerName(string text)
    {
      return text.Replace("-", string.Empty);
    }
  }
}