namespace X3platform.Membership.Mvc.Personal.Controllers
{
    using System;
    using System.Web.Mvc;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.Web.Mvc.Controllers;
    using X3Platform.Web.Mvc.Attributes;
    using X3Platform.Web.Builder;
    using X3Platform.Sessions;
    using X3Platform.Util;

    [LoginFilter]
    public class FriendsController : CustomController
    {
        private string APPLICATION_NAME = "PersonalSettings";

        #region 函数:Index()
        /// <summary>好友信息</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/friends/account-friend-list.cshtml");
        }
        #endregion

        #region 函数:Detail()
        /// <summary>好友信息</summary>
        /// <returns></returns>
        public ActionResult Detail(string options)
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            // -------------------------------------------------------
            // 数据加载
            // -------------------------------------------------------

            JsonData request = JsonMapper.ToObject(options);

            string id = JsonHelper.GetDataValue(request, "id");

            IMemberInfo member = MembershipManagement.Instance.MemberService[id];

            if (member == null)
            {
                ApplicationError.Write(404);
            }

            ViewBag.member = member;

            IAccountInfo account = MembershipManagement.Instance.AccountService[id];

            string avatar_120x120 = string.Empty;

            if (string.IsNullOrEmpty(account.CertifiedAvatar))
            {
                avatar_120x120 = AttachmentStorageConfigurationView.Instance.VirtualUploadFolder + "avatar/default_120x120.png";
            }
            else
            {
                avatar_120x120 = account.CertifiedAvatar.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.VirtualUploadFolder);
            }

            ViewBag.avatar_120x120 = avatar_120x120;

            return View("/views/main/account/friends/account-friend-detail.cshtml");
        }
        #endregion

        #region 函数:Invite()
        /// <summary>邀请好友</summary>
        /// <returns></returns>
        public ActionResult Invite()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/friends/account-friend-invite.cshtml");
        }
        #endregion

        #region 函数:Accept()
        /// <summary>同意好友邀请的列表</summary>
        /// <returns></returns>
        public ActionResult Accept()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/friends/account-friend-accept.cshtml");
        }
        #endregion
    }
}
