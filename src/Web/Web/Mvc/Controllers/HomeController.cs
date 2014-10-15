using System.Web.Mvc;
using X3Platform.Apps;
using X3Platform.Json;
using X3Platform.Configuration;
using Common.Logging;
using System.Reflection;

namespace X3Platform.Web.Mvc.Controllers
{
    public sealed class HomeController : CustomController
    {
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public ActionResult Index()
        {
            if (KernelConfigurationView.Instance.ApplicationHomePage != "/")
            {
                return Redirect(KernelConfigurationView.Instance.ApplicationHomePage);
            }

            return View("/views/main/default.cshtml");
        }
    }
}
