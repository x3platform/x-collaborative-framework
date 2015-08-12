﻿namespace X3Platform.AttachmentStorage.Mvc
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

  /// <summary></summary>
  public class RequestRoute : RouteBase
  {
    /// <summary>请求地址的前缀</summary>
    private const string prefixUrl = "/attachment/";

    /// <summary></summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public override RouteData GetRouteData(HttpContextBase httpContext)
    {
      // 获取相对路径
      var virtualPath = httpContext.Request.RawUrl;

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
        routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.AttachmentStorage.Mvc.Controllers" };

        Match match = null;

        if (string.IsNullOrEmpty(friendlyUrl))
        {
          // 主页
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "Index");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^archive/([\w+\-]+)$"))
        {
          // 附件下载信息
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "Download");
          routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^archive/([\w+\-]+)$").Groups[1].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^warn$"))
        {
          // 列表信息
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "Warn");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^options$"))
        {
          // 映射设置
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "Options");
        }
        else
        {
          return null;
        }

        return routeData;
      }
      else
      {
        return null;
      }
    }

    /// <summary></summary>
    /// <param name="requestContext"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
    {
      return null;
    }

    private string FriendlyControllerName(string text)
    {
      return text.Replace("-", string.Empty);
    }
  }
}