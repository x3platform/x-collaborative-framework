using System.Web.Mvc;
using X3Platform.Configuration;
using X3Platform.Web.Configuration;
using System.Web;
using Common.Logging;
using System.Reflection;

namespace X3Platform.Web.Mvc.Attributes
{
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (KernelContext.Current.User == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult()
                    {
                        Content = "{\"message\":{\"returnCode\":401,\"value\":\"Unauthorized\"}}"
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
