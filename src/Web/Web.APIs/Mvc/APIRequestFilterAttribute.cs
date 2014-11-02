namespace X3Platform.Web.Mvc.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    public class APIRequestFilterAttribute : ActionFilterAttribute
    {
        /// <summary>计时器</summary>
        private Stopwatch sw = new Stopwatch();
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction) {   return; }
           
            // 重置计时器
            sw.Restart();
            
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)   {return;  }
           
            //计时器停止
            sw.Stop();
          
            base.OnActionExecuted(filterContext);
        }
    }
}
