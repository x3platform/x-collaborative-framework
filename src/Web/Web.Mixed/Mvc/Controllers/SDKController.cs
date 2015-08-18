namespace X3Platform.Web.Mvc.Controllers
{
  using System.Web.Mvc;

  /// <summary>在线开发辅助工具包</summary>
  public sealed partial class SDKController : Controller
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      return View("/views/main/sdk/default.cshtml");
    }
    #endregion
  }
}
