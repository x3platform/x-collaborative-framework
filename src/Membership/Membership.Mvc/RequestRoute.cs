﻿namespace X3Platform.Membership.Mvc
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
        private const string prefixUrl = "/membership/";

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
                routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Membership.Mvc.Controllers" };

                if (string.IsNullOrEmpty(friendlyUrl))
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
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/list\?treeViewId=([\w+\-]+)$"))
                {
                    // 分组类别设置
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/list\?treeViewId=([\w+\-]+)$").Groups[1].Value));
                    routeData.Values.Add("action", "List");
                    routeData.Values.Add("options", "{\"treeViewId\":\"" + Regex.Match(friendlyUrl, @"^([\w+\-]+)/list\?treeViewId=([\w+\-]+)$").Groups[2].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/mapping$"))
                {
                    // 映射设置
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/mapping").Groups[1].Value));
                    routeData.Values.Add("action", "Mapping");
                    // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/report$"))
                {
                    // 报表
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/report").Groups[1].Value));
                    routeData.Values.Add("action", "Report");
                    // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else if (Regex.IsMatch(friendlyUrl, @"^([\w+\-]+)/validator$"))
                {
                    // 数据验证
                    routeData.Values.Add("controller", FriendlyControllerName(Regex.Match(friendlyUrl, @"^([\w+\-]+)/validator$").Groups[1].Value));
                    routeData.Values.Add("action", "Validator$");
                    // routeData.Values.Add("options", "{\"id\":\"" + Regex.Match(friendlyUrl, @"^article\/([\w+\-]+)$").Groups[1].Value + "\"}");
                }
                else
                {
                    return null;
                }

                return routeData;
            } 
            // 判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
            else if (virtualPath.IndexOf("/hr/") == 0)
            {
                return null;
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
            //判断请求是否来源于CategoryController.Showcategory(string id),不是则返回null,让匹配继续
            var categoryId = values["id"] as string;

            if (categoryId == null) // 路由信息中缺少参数id，不是我们要处理的请求，返回null
                return null;

            //请求不是CategoryController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是CategoryController.Showcategory(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("showcategory", StringComparison.OrdinalIgnoreCase))
                return null;

            //至此，我们可以确定请求是CategoryController.Showcategory(string id)发起的，生成相应的URL并返回
            // var category = CategoryManager.AllCategories.Find(c => c.CategoeyID == categoryId);

            // if (category == null)
            //    throw new ArgumentNullException("category");//找不到分类抛出异常

            //生成URL
            // var path = "ca-" + category.CategoeyName.Trim();
            var path = "projects-123";

            return new VirtualPathData(this, path.ToLowerInvariant());
        }

        private string FriendlyControllerName(string text)
        {
            return text.Replace("-", string.Empty);
        }
    }
}