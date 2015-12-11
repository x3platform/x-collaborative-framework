using System.Web.Mvc;
using X3Platform.Configuration;
using X3Platform.Web.Configuration;
using System.Web;
using Common.Logging;
using System.Reflection;

namespace X3Platform.Web.Mvc.Attributes
{
    /// <summary>
    /// 验证码验证
    /// </summary>
    public class CaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 验证码
            // string captcha = XmlHelper.Fetch("captcha", doc);
            string captcha = HttpContext.Current.Request.QueryString["captcha"];

            if (HttpContext.Current.Session["captcha"] == null || captcha != HttpContext.Current.Session["captcha"].ToString())
            {
                // return "{\"message\":{\"returnCode\":1,\"value\":\"验证码错误。\"}}";

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult()
                    {
                        Content = "{\"message\":{\"returnCode\":401,\"value\":\"验证码错误。\"}}"
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult(string.Format(
                        WebConfigurationView.Instance.SignInUrl,
                        HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.Url.ToString())));
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
