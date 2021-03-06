﻿namespace X3Platform.Connect.Mvc
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
        private const string prefixUrl = "/connect/";

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
                routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Connect.Mvc.Controllers" };

                Match match = null;

                if (string.IsNullOrEmpty(friendlyUrl))
                {
                    // 主页
                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Index");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^form$"))
                {
                    // 表单信息
                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Form");
                    routeData.Values.Add("options", "{}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^form\?id=([\w+\-]+)$"))
                {
                    // 表单信息
                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Form");
                    routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^form\?id=([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^overview/([\w+\-]+)$"))
                {
                    // 概述信息
                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Overview");
                    routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^overview/([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^overview/([\w+\-]+)\?applicationName=([\w+\-]+)$"))
                {
                    // 概述信息
                    match = Regex.Match(friendlyUrl, @"^overview/([\w+\-]+)\?applicationName=([\w+\-]+)$");

                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Overview");
                    routeData.Values.Add("options", "{\"id\":\"" + match.Groups[1].Value + "\",\"applicationName\":\"" + match.Groups[2].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^form\?applicationName=([\w+\-]+)$"))
                {
                    // 表单信息
                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Form");
                    routeData.Values.Add("options", "{\"applicationName\":\"" + Regex.Match(friendlyUrl, @"^form\?applicationName=([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^form\?id=([\w+\-]+)&applicationName=([\w+\-]+)$"))
                {
                    // 表单信息
                    match = Regex.Match(friendlyUrl, @"^form\?id=([\w+\-]+)&applicationName=([\w+\-]+)$");
                    routeData.Values.Add("controller", "Home");
                    routeData.Values.Add("action", "Form");
                    routeData.Values.Add("options", "{\"id\":\"" + match.Groups[1].Value + "\",\"applicationName\":\"" + match.Groups[2].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/list$"))
                {
                    // 列表信息
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/list").Groups[1].Value));
                    routeData.Values.Add("action", "List");
                    // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$"))
                {
                    // 表单信息
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[1].Value));
                    routeData.Values.Add("action", "Form");
                    routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?id=([\w+\-]+)$").Groups[2].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/form\?applicationName=([\w+\-]+)$"))
                {
                    // 表单信息
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?applicationName=([\w+\-]+)$").Groups[1].Value));
                    routeData.Values.Add("action", "Form");
                    routeData.Values.Add("options", "{\"applicationName\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/form\?applicationName=([\w+\-]+)$").Groups[2].Value + "\"}");
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