﻿namespace X3Platform.Membership.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using X3Platform.Apps;
    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.DigitalNumber;
    using X3Platform.Configuration;
    using X3Platform.Web.Mvc.Controllers;
    using X3Platform.Apps.Model;
    using X3Platform.Web.Mvc.Attributes;

    [LoginFilter]
    public class GroupTreeNodeController : CustomController
    {
        private string APPLICATION_NAME = "Membership";

        #region 函数:List()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult List(string options)
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

            JsonData request = JsonMapper.ToObject(options);

            string treeViewId = !request.Keys.Contains("treeViewId") ? string.Empty : request["treeViewId"].ToString();

            ViewBag.tree = MembershipManagement.Instance.GroupTreeService.FindOne(treeViewId);

            // 角色
            return View("/views/main/membership/group-tree-node-list.cshtml");
        }
        #endregion
    }
}
