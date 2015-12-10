using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

using Common.Logging;

namespace X3Platform.Web.Mvc.Attributes
{
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            KernelContext.Log.Error(filterContext.Exception);

            string message = filterContext.Exception.Message;

            ActionResult result = null;

            if (!filterContext.IsChildAction && !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = null;
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "{\"message\":{\"returnCode\":500,\"value\":\"Unauthorized\"}}"
                };
                // filterContext.Result = new JsonResult() { Data = new { Result = false, Message = message } };
            }
            else if (filterContext.IsChildAction)
            {
                filterContext.Result = new ContentResult() { Content = message };
            }
            else
            {
                filterContext.Result = result;
            }

            filterContext.ExceptionHandled = true;

            base.OnException(filterContext);
        }
    }
}