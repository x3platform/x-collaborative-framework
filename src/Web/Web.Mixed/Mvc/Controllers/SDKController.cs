namespace X3Platform.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using X3Platform.Util;

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

        #region 函数:Uuid()
        /// <summary>UUID生成工具</summary>
        /// <returns></returns>
        public ActionResult Uuid()
        {
            ViewBag.UUID = Guid.NewGuid();

            return View("/views/main/sdk/uuid.cshtml");
        }
        #endregion

        #region 函数:Password()
        /// <summary>密码生成工具</summary>
        /// <returns></returns>
        public ActionResult Password()
        {
            ViewBag.StrongPassword = GeneratePassword();

            return View("/views/main/sdk/password.cshtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Password(object args)
        {
            ViewBag.StrongPassword = GeneratePassword();

            return View("/views/main/sdk/password.cshtml");
        }
        #endregion

        private string GeneratePassword()
        {
            // 生成一个八位长度的字符串
            string password = Guid.NewGuid().ToString().Substring(0, 5);

            Random random = new Random();

            password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("!@#$", 1));

            password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("abcdefghijkmnpqrstuvwxyz", 1));

            password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("ABCDEFGHJKLMNPQRSTUVWXYZ", 1));

            return password;
        }
    }
}
