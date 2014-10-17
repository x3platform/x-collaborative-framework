namespace X3Platform.Plugins
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>自定义插件接口</summary>
    public interface ICustomPlugin
    {
        #region 属性:Id
        /// <summary>标识</summary>
        string Id { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>名称</summary>
        string Name { get; }
        #endregion

        #region 属性:Version
        /// <summary>版本</summary>
        string Version { get; }
        #endregion

        #region 属性:Author
        /// <summary>作者</summary>
        string Author { get; }
        #endregion

        #region 属性:Copyright
        /// <summary>版权</summary>
        string Copyright { get; }
        #endregion

        #region 属性:Url
        /// <summary>插件获取地址</summary>
        string Url { get; }
        #endregion

        #region 属性:ThumbnailUrl
        /// <summary>缩略图</summary>
        string ThumbnailUrl { get; }
        #endregion

        #region 属性:Description
        /// <summary>描述信息</summary>
        string Description { get; }
        #endregion

        #region 属性:Status
        /// <summary>状态: 0 关闭 | 1 开启</summary>
        int Status { get; set; }
        #endregion

        #region 属性:SupportMenu
        /// <summary>菜单支持: 0 不支持 | 1 支持</summary>
        int SupportMenu { get; set; }
        #endregion

        #region 函数:Install()
        /// <summary>安装插件</summary>
        /// <returns>返回信息. =0代表安装成功, >0代表安装失败.</returns>
        int Install();
        #endregion

        #region 函数:Uninstall()
        /// <summary>卸载插件</summary>
        /// <returns>返回信息. =0代表卸载成功, >0代表卸载失败.</returns>
        int Uninstall();
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表卸载成功, >0代表卸载失败.</returns>
        int Restart();
        #endregion

        #region 函数:Command(Hashtable agrs)
        /// <summary>执行命令</summary>
        /// <returns>返回信息. =0代表执行成功, >0代表执行失败.</returns>
        int Command(Hashtable agrs);
        #endregion
    }
}
