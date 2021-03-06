﻿namespace X3Platform.Plugins.Forum.Mvc
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
    private const string prefixUrl = "/forum/";

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
        routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Plugins.Forum.Mvc.Controllers" };

        Match match = null;

        if (string.IsNullOrEmpty(friendlyUrl))
        {
          // 主页
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "List");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^form$"))
        {
          // 表单信息
          routeData.Values.Add("controller", "ForumThread");
          routeData.Values.Add("action", "Form");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^detail\/([\w+\-]+)$"))
        {
          // 根据唯一标识查询详情
          routeData.Values.Add("controller", "Home");
          routeData.Values.Add("action", "Detail");
          routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^detail\/([\w+\-]+)$").Groups[1].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/list$"))
        {
          // 列表信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/list").Groups[1].Value));
          routeData.Values.Add("action", "List");
          // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/list\?treeViewId=([\w+\-]+)$"))
        {
          // 分组类别设置
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/list\?treeViewId=([\w+\-]+)$").Groups[1].Value));
          routeData.Values.Add("action", "List");
          routeData.Values.Add("options", "{\"treeViewId\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/list\?treeViewId=([\w+\-]+)$").Groups[2].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form$"))
        {
          // 表单信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/form$").Groups[1].Value));
          routeData.Values.Add("action", "Form");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$"))
        {
          // 表单信息
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[1].Value));
          routeData.Values.Add("action", "Form");
          routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[2].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form\?menuType=([\w+\-]+)&applicationId=([\w+\-]+)&menuId=([\w+\-]+)$"))
        {
          // 表单信息-新建
          match = Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?menuType=([\w+\-]+)&applicationId=([\w+\-]+)&menuId=([\w+\-]+)$");
          routeData.Values.Add("controller", FriendlyControllerName(match.Groups[1].Value));
          routeData.Values.Add("action", "Form");
          routeData.Values.Add("options", "{\"menuType\":\"" + match.Groups[2].Value + "\",\"applicationId\":\"" + match.Groups[3].Value + "\",\"menuId\":\"" + match.Groups[4].Value + "\"}");
        }
        else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/setting$"))
        {
          // 映射设置
          routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/setting$").Groups[1].Value));
          routeData.Values.Add("action", "Setting");
          //routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
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