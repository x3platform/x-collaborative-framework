namespace X3Platform.Web.Builder.ILayouts
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Velocity;

    /// <summary>自定义菜单构建器接口</summary>
    public interface IMenuBuilder
    {
        #region 函数:GetMenu(IAccountInfo account, string applicationName)
        /// <summary>获取菜单信息</summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        string GetMenu(IAccountInfo account, string applicationName);
        #endregion

        #region 函数:GetMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
        /// <summary>获取菜单信息</summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <param name="parentMenuFullPath">父级菜单的完整路径</param>
        /// <returns></returns>
        string GetMenu(IAccountInfo account, string applicationName, string parentMenuFullPath);
        #endregion

        #region 函数:GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
        /// <summary>获取菜单信息</summary>
        /// <param name="account"></param>
        /// <param name="applicationName"></param>
        /// <param name="options">菜单选项</param>
        /// <returns></returns>
        string GetMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options);
        #endregion
          
        #region 函数:ParseMenu(IAccountInfo account, string menuName)
        /// <summary>解析菜单</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="menuName">菜单名称</param>
        /// <returns></returns>
        [Obsolete]
        string ParseMenu(IAccountInfo account, string menuName);
        #endregion

        #region 函数:ParseMenu(IAccountInfo account, string menuName, bool isAdminToken)
        /// <summary>解析菜单</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="menuName">菜单名称</param>
        /// <param name="isAdminToken">是否是管理员</param>
        /// <returns></returns>
        [Obsolete]
        string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken);
        #endregion

        #region 函数:ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context)
        /// <summary>解析菜单</summary>
        /// <param name="account">帐号信息</param>
        /// <param name="menuName">菜单名称</param>
        /// <param name="isAdminToken">是否是管理员</param>
        /// <returns></returns>
        string ParseMenu(IAccountInfo account, string menuName, bool isAdminToken, VelocityContext context);
        #endregion
    }
}