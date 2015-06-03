using System.Web.Mvc;
using System.Reflection;

using Common.Logging;

using X3Platform.Json;
using X3Platform.Configuration;
using X3Platform.Web.Mvc.Attributes;

namespace X3Platform.Web.Mvc.Controllers
{
  [LoginFilter]
  public sealed class HomeController : CustomController
  {
    public ActionResult Index()
    {
      if (KernelConfigurationView.Instance.ApplicationHomePage != "/")
      {
        return Redirect(KernelConfigurationView.Instance.ApplicationHomePage);
      }

      return View("~/views/main/default.cshtml");
    }
  }
}
