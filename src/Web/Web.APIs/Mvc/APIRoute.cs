namespace X3Platform.Web.APIs.Mvc
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    
    /// <summary></summary>
    public class APIRoute : RouteBase
    {
        /// <summary></summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            //获取相对路径
            var virtualPath = httpContext.Request.Path;

            // 判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
            if (virtualPath.IndexOf("/api/") == 0)
            {
                // 符合规定的地址规则 /api/methodName，截取后面的methodName
                string methodName = virtualPath.Substring(5).Trim('/');

                if (methodName.LastIndexOf(".aspx") == (methodName.Length - 5))
                {
                    methodName = methodName.Substring(0, methodName.Length - 5);
                }

                // 声明一个RouteData，添加相应的路由值
                var routeData = new RouteData(this, new MvcRouteHandler());

                // 限制名称空间
                routeData.DataTokens["Namespaces"] = new string[] { "X3Platform.Web.APIs" };

                routeData.Values.Add("controller", "API");
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("methodName", methodName);

                if (httpContext.Request.ContentType == "application/xml" || httpContext.Request.ContentType == "application/json")
                {
                    using (StreamReader reader = new StreamReader(httpContext.Request.InputStream))
                    {
                        routeData.Values.Add("rawInput", reader.ReadToEnd());

                        reader.Close();
                    }
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
            return new VirtualPathData(this, string.Empty);
        }
    }
}