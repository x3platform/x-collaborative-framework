#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>自定义插件</summary>
    public abstract class CustomPlugin : ICustomPlugin
    {
        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>标识</summary>
        public virtual string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary>名称</summary>
        public virtual string Name
        {
            get { return m_Name; }
        }
        #endregion

        #region 属性:Version
        private string m_Version = "1.0.0.0";

        /// <summary>版本</summary>
        public string Version
        {
            get { return m_Version; }
        }
        #endregion

        #region 属性:Author
        private string m_Author = "ruanyu83@gmail.com";

        /// <summary>作者</summary>
        public virtual string Author
        {
            get { return m_Author; }
        }
        #endregion

        #region 属性:Copyright
        private string m_Copyright = "MIT";

        /// <summary>版权</summary>
        public virtual string Copyright
        {
            get { return m_Copyright; }
        }
        #endregion

        #region 属性:Url
        private string m_Url = string.Empty;

        /// <summary>插件获取地址</summary>
        public virtual string Url
        {
            get { return m_Url; }
        }
        #endregion

        #region 属性:ThumbnailUrl
        private string m_ThumbnailUrl = string.Empty;

        /// <summary>缩略图</summary>
        public virtual string ThumbnailUrl
        {
            get { return m_ThumbnailUrl; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>描述信息</summary>
        public virtual string Description
        {
            get { return m_Description; }
        }
        #endregion

        #region 属性:Status
        private int m_Status = 0;

        /// <summary>状态, 0 未激活, 1 已激活.</summary>
        public virtual int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:SupportMenu
        private int m_SupportMenu = 1;

        /// <summary>菜单支持</summary>
        public virtual int SupportMenu
        {
            get { return m_SupportMenu; }
            set { m_SupportMenu = value; }
        }
        #endregion

        #region 函数:Install()
        /// <summary>安装插件</summary>
        /// <returns>返回信息. =0代表安装成功, >0代表安装失败.</returns>
        public virtual int Install()
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

        #region 函数:Uninstall()
        /// <summary>卸载插件</summary>
        /// <returns>返回信息. =0代表卸载成功, >0代表卸载失败.</returns>
        public virtual int Uninstall()
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public virtual int Restart()
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

        #region 函数:Command(Hashtable agrs)
        /// <summary>执行命令</summary>
        /// <returns>返回信息. =0代表执行成功, >0代表执行失败.</returns>
        public virtual int Command(Hashtable agrs)
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion
    }
}
