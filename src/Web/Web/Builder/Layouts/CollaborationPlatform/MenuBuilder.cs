#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :MenuBuilder.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Web.Builder.Layouts.CollaborationPlatform
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Apps;
    using X3Platform.Membership;
    using X3Platform.Web.Builder.ILayouts;
    using X3Platform.Velocity;
    #endregion

    /// <summary>Ĭ�ϲ˵�������</summary>
    public class MenuBuilder : CommonBuilder, IMenuBuilder
    {
        /// <summary></summary>
        public MenuBuilder() { }

        #region 属性:GetMenu(IAccountInfo account, string applicationName)
        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public string GetMenu(IAccountInfo account, string applicationName)
        {
            return BuilderContext.Instance.NavigationManagement.GetApplicationMenu(account, applicationName);
        }
        #endregion

        #region 属性:GetMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        public string GetMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        {
            return BuilderContext.Instance.NavigationManagement.GetApplicationMenu(account, applicationName, parentMenuFullPath);
        }
        #endregion

        #region 属性:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        public string GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        {
            return BuilderContext.Instance.NavigationManagement.GetApplicationMenu(account, applicationName, options);
        }
        #endregion

        #region 属性:ParseMenu(IAccountInfo account, string menuName)
        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName)
        {
            return ParseMenu(account, menuName, AppsSecurity.IsAdministrator(account, menuName));
        }
        #endregion

        #region 属性:ParseMenu(IAccountInfo account, string menuName, bool isAdminToken)
        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="menuName"></param>
        /// <param name="isAdminToken"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken)
        {
            return ParseMenu(account, menuName, isAdminToken, new VelocityContext());
        }
        #endregion

        #region 属性:ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        /// <summary></summary>
        /// <param name="account"></param>
        /// <param name="menuName"></param>
        /// <param name="isAdminToken"></param>
        /// <returns></returns>
        public string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        {
            string templatePath = string.Format("web/builder/menu/collaboration/{0}.vm", menuName);

            switch (menuName.ToLower())
            {
                default:
                    context.Put("hostName", HostName);
                    context.Put("account", account);

                    // ��֤����Ա����
                    context.Put("isAdminToken", isAdminToken);

                    return VelocityManager.Instance.Merge(context, templatePath);
            }
        }
        #endregion
    }
}
