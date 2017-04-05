using System.Web;

namespace X3Platform
{
    /// <summary>上下文封装接口</summary>
    public interface IContextWrapper
    {
        #region 属性:ProcessRequest(HttpContext context)
        /// <summary>处理请求</summary>
        /// <param name="context">HTTP 上下文对象</param>
        void ProcessRequest(HttpContext context);
        #endregion
    }
}
