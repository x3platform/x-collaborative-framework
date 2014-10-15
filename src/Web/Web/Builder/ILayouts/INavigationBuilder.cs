namespace X3Platform.Web.Builder.ILayouts
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;

    /// <summary>导航构建器</summary>
    public interface INavigationBuilder
    {
        #region 函数:GetStartMenu(IAccountInfo account)
        /// <summary>获取开始菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetStartMenu(IAccountInfo account);
        #endregion

        #region 函数:GetTopMenu(IAccountInfo account)
        /// <summary>获取顶部菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetTopMenu(IAccountInfo account);
        #endregion

        #region 函数:GetBottomMenu(IAccountInfo account)
        /// <summary>获取底部菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetBottomMenu(IAccountInfo account);
        #endregion

        #region 函数:GetShortcutMenu(IAccountInfo account)
        /// <summary>获取快捷方式菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetShortcutMenu(IAccountInfo account);
        #endregion

        #region 函数:GetApplicationMenu(IAccountInfo account, string applicationName)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="applicationName">应用名称</param>
        /// <returns></returns>
        string GetApplicationMenu(IAccountInfo account, string applicationName);
        #endregion

        #region 函数:GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="applicationName">应用名称</param>
        /// <param name="parentMenuFullPath">父级菜单的完整路径</param>
        /// <returns></returns>
        string GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath);
        #endregion

        #region 函数:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>获取应用菜单信息</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="applicationName">应用名称</param>
        /// <param name="options">菜单选项</param>
        /// <returns></returns>
        string GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options);
        #endregion

        #region 函数:GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        /// <summary>获取应用功能点菜单信息</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName);
        #endregion
    }
}