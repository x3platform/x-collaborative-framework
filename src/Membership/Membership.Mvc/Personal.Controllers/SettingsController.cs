﻿namespace X3platform.Membership.Mvc.Personal.Controllers
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
    using X3Platform.Membership.Configuration;

    [LoginFilter]
    public class SettingsController : CustomController
    {
        private string APPLICATION_NAME = "PersonalSettings";

        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Personal Settings 个性化设置
            return Redirect("/account/settings/profile");
        }
        #endregion

        #region 函数:Admin()
        /// <summary>帐号管理</summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            // 修改密码
            // 修改帐号
            // 删除帐号
            return View("/views/main/account/settings/admin.cshtml");
        }
        #endregion

        #region 函数:Profile()
        /// <summary>帐号简介</summary>
        /// <returns></returns>
        public ActionResult Profile()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            IMemberInfo member = MembershipManagement.Instance.MemberService[this.Account.Id];

            if (member == null)
            {
                ApplicationError.Write(404);
            }

            ViewBag.member = member;

            IAccountInfo account = MembershipManagement.Instance.AccountService[member.Account.Id];

            string avatar_180x180 = string.Empty;

            if (string.IsNullOrEmpty(account.CertifiedAvatar))
            {
                avatar_180x180 = MembershipConfigurationView.Instance.AvatarVirtualFolder + "default_120x120.png";
            }
            else
            {
                avatar_180x180 = account.CertifiedAvatar.Replace("{avatar}", MembershipConfigurationView.Instance.AvatarVirtualFolder).Replace("//", "/");
            }

            ViewBag.avatar_180x180 = avatar_180x180;

            return View("/views/main/account/settings/profile.cshtml");
        }
        #endregion

        #region 函数:Avatar()
        /// <summary>联系信息</summary>
        /// <returns></returns>
        public ActionResult Avatar()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            IMemberInfo member = MembershipManagement.Instance.MemberService[this.Account.Id];

            if (member == null)
            {
                ApplicationError.Write(404);
            }

            ViewBag.member = member;

            IAccountInfo account = MembershipManagement.Instance.AccountService[member.Account.Id];

            string avatar_180x180 = string.Empty;

            if (string.IsNullOrEmpty(account.CertifiedAvatar))
            {
                avatar_180x180 = MembershipConfigurationView.Instance.AvatarVirtualFolder + "default_180x180.png";
            }
            else
            {
                avatar_180x180 = account.CertifiedAvatar.Replace("{avatar}", MembershipConfigurationView.Instance.AvatarVirtualFolder);
            }

            ViewBag.avatar_180x180 = avatar_180x180;

            return View("/views/main/account/settings/avatar.cshtml");
        }
        #endregion

        #region 函数:Emails()
        /// <summary>邮箱管理</summary>
        /// <returns></returns>
        public ActionResult Emails()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/settings/emails.cshtml");
        }
        #endregion

        #region 函数:Contact()
        /// <summary>联系信息</summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/settings/contact.cshtml");
        }
        #endregion

        #region 函数:Notifications()
        /// <summary>通知管理</summary>
        /// <returns></returns>
        public ActionResult Notifications()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/settings/notifications.cshtml");
        }
        #endregion

        #region 函数:Applications()
        /// <summary>应用管理</summary>
        /// <returns></returns>
        public ActionResult Applications()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            return View("/views/main/account/settings/applications.cshtml");
        }
        #endregion

        #region 函数:Security()
        /// <summary>安全管理</summary>
        /// <returns></returns>
        public ActionResult Security()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            ViewBag.sessions = SessionContext.Instance.AccountCacheService.Dump(this.Account.LoginName);

            ViewBag.logs = MembershipManagement.Instance.AccountLogService.FindAllByAccountId(this.Account.Id);

            return View("/views/main/account/settings/security.cshtml");
        }
        #endregion
    }
}
