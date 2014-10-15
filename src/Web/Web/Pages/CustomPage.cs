// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :CustomPage.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Web.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    using X3Platform.Configuration;
    using X3Platform.Membership;
    using X3Platform.Velocity;
    using X3Platform.Web.Configuration;

    /// <summary>自定义页面</summary>
    public abstract class CustomPage : Page
    {
        /// <summary>初始化时间，主要用于计算页面加载速度</summary>
        protected DateTime initializedTime;

        /// <summary>头部信息</summary>
        protected StringBuilder renderHeader = new StringBuilder();

        /// <summary>底部信息</summary>
        protected StringBuilder renderFooter = new StringBuilder();

        /// <summary>内容缓冲器</summary>
        protected StringBuilder renderBuffer = new StringBuilder();

        /// <summary>模板上下文</summary>
        protected VelocityContext velocityContext;

        /// <summary>帐户信息</summary>
        public IAccountInfo AccountCard = null;

        /// <summary>重启次数计数器</summary>
        private int reloadCount = 0;

        public CustomPage()
        {
            // 记录页面初始化时间
            this.initializedTime = DateTime.Now;

            this.velocityContext = new VelocityContext();

            this.velocityContext.Put("year", this.initializedTime.Year);

            this.velocityContext.Put("enableToolbar", HttpContext.Current.Request.Cookies["enableToolbar"] == null ? "false" : HttpContext.Current.Request.Cookies["enableToolbar"].Value);

            this.velocityContext.Put("enableSildeMenu", HttpContext.Current.Request.Cookies["enableSildeMenu"] == null ? "false" : HttpContext.Current.Request.Cookies["enableSildeMenu"].Value);

            this.AccountCard = KernelContext.Current.User;
        }

        #region 函数:ProcessRequest(HttpContext context)
        /// <summary>处理请求</summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            // Process Count
            this.reloadCount++;
            // 清空内容
            this.renderBuffer.Remove(0, renderBuffer.Length);

            this.ParseRenderTemplatePath();

            base.ProcessRequest(context);
        }
        #endregion

        #region 函数:ParseRenderTemplatePath()
        /// <summary>处理请求</summary>
        /// <param name="context"></param>
        public void ParseRenderTemplatePath()
        {
            // -------------------------------------------------------
            // 自动解析模板路径
            // -------------------------------------------------------

            string serverNameView = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string path = string.Empty;

            int point = 0;

            if (serverNameView.LastIndexOf(".") == -1)
            {
                // 没有点的情况
                // => 不做处理, 设置为默认路径
            }
            else if (WebConfigurationView.Instance.ServerName.IndexOf(serverNameView + ";") > -1)
            {
                // 当访问的服务器没有设置相关的域名时,并且符合设置的服务器名称(ServerName)信息.
                // 设置域为相关的IP或者机器名
                KernelConfigurationView.Instance.Configuration.Keys["Domain"].Value = serverNameView;
            }
            else if (WebConfigurationView.Instance.SiteThemeName == "dynamic")
            {
                // 如果将页面的主题设置为 dynamic, 则根据域名信息自动匹配相关的模板

                point = serverNameView.LastIndexOf(".");

                if (serverNameView.Substring(0, point).LastIndexOf(".") == -1)
                {
                    // 只有一个点的情况
                    // => 例如 workspace.com 直接取 workspace.com  

                    path = serverNameView;
                }
                else
                {
                    // 有两个或两个以上的点的情况
                    // => my.workspace.com 取 workspace.com

                    point = serverNameView.Substring(0, point).LastIndexOf(".") + 1;

                    path = serverNameView.Substring(point, serverNameView.Length - point);
                }

                RenderHeadTemplatePath = string.Format("sites/{0}/head.vm", path);
                RenderBodyTemplatePath = string.Format("sites/{0}/body.vm", path);
                RenderTemplatePath = string.Format("sites/{0}/render.vm", path);
            }
        }
        #endregion

        #region 属性:ServerNameView
        /// <summary>客户访问时看到的服务器名称</summary>
        public string ServerNameView
        {
            get { return Request.ServerVariables["SERVER_NAME"]; }
        }
        #endregion

        #region 属性:HostName
        /// <summary>服务器地址</summary>
        public string HostName
        {
            get { return KernelConfigurationView.Instance.HostName; }
        }
        #endregion

        #region 属性:StaticFileHostName
        /// <summary>文件服务器地址</summary>
        public string StaticFileHostName
        {
            get { return KernelConfigurationView.Instance.StaticFileHostName; }
        }
        #endregion

        #region 属性:SiteName
        /// <summary>站点名称</summary>
        public string SiteName
        {
            get { return KernelConfigurationView.Instance.SystemName; }
        }
        #endregion

        #region 属性:Title
        private string m_Title = string.Empty;

        /// <summary>页面标题</summary>
        public new string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        #endregion

        #region 属性:Keywords
        private string m_Keywords = string.Empty;

        /// <summary>页面关键字</summary>
        public string Keywords
        {
            get { return m_Keywords; }
            set { m_Keywords = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>页面描述</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Scripts
        private IList<string> m_Scripts = null;

        /// <summary>外部脚本链接集合</summary>
        public IList<string> Scripts
        {
            get
            {
                if (m_Scripts == null)
                {
                    m_Scripts = new List<string>();
                }
                return m_Scripts;
            }
        }
        #endregion

        #region 属性:Styles
        private IList<string> m_Styles = null;

        /// <summary>外部样式链接集合</summary>
        public IList<string> Styles
        {
            get
            {
                if (m_Styles == null)
                {
                    m_Styles = new List<string>();
                }

                return m_Styles;
            }
        }
        #endregion

        #region 属性:RenderHeadTemplatePath
        private string m_RenderHeadTemplatePath = WebConfigurationView.Instance.RenderHeadTemplatePath;

        public string RenderHeadTemplatePath
        {
            get { return m_RenderHeadTemplatePath; }
            set { m_RenderHeadTemplatePath = value; }
        }
        #endregion

        #region 属性:RenderBodyTemplatePath
        private string m_RenderBodyTemplatePath = WebConfigurationView.Instance.RenderBodyTemplatePath;

        public string RenderBodyTemplatePath
        {
            get { return m_RenderBodyTemplatePath; }
            set { m_RenderBodyTemplatePath = value; }
        }
        #endregion

        #region 属性:RenderTemplatePath
        private string m_RenderTemplatePath = WebConfigurationView.Instance.RenderTemplatePath;

        public string RenderTemplatePath
        {
            get { return m_RenderTemplatePath; }
            set { m_RenderTemplatePath = value; }
        }
        #endregion

        #region 函数:RenderHead(string title, string keywords, string description)
        public virtual string RenderHead(string title, string keywords, string description)
        {
            velocityContext.Put("title", title);
            velocityContext.Put("keywords", keywords);
            velocityContext.Put("description", description);

            velocityContext.Put("scripts", Scripts);
            velocityContext.Put("styles", Styles);

            //velocityContext.Put("googleAnalyticsMode", GoogleAnalyticsMode);
            //velocityContext.Put("googleMapKey", GoogleMapKey);

            return VelocityManager.Instance.Merge(velocityContext, RenderHeadTemplatePath);
        }
        #endregion

        #region 函数:RenderBody()
        public virtual string RenderBody()
        {
            velocityContext.Put("account", AccountCard);

            velocityContext.Put("header", renderHeader.ToString());

            velocityContext.Put("buffer", renderBuffer.ToString());

            velocityContext.Put("footer", renderFooter.ToString());
            // 默认域名
            velocityContext.Put("domain", KernelConfigurationView.Instance.Domain);
            // 签名
            velocityContext.Put("signature", KernelConfigurationView.Instance.ApplicationClientSignature);
            // 页面加载次数
            velocityContext.Put("reloadCount", this.reloadCount);
            // 给Session对象赋值，固定取得SessionID
            HttpContext.Current.Session["__session__ticket__"] = this.initializedTime;
            // 会话标识
            velocityContext.Put("session", HttpContext.Current.Session.SessionID);
            // 时间间隔
            velocityContext.Put("timeSpan", (new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(this.initializedTime.Ticks))));

            return VelocityManager.Instance.Merge(velocityContext, RenderBodyTemplatePath);
        }
        #endregion

        #region 函数:Render(HtmlTextWriter writer)
        protected override void Render(HtmlTextWriter writer)
        {
            // header

            velocityContext.Put("head", RenderHead(this.Title, this.Keywords, this.Description));

            // body

            velocityContext.Put("body", RenderBody());

            // output html = header + body;

            Response.Write(VelocityManager.Instance.Merge(velocityContext, RenderTemplatePath));
        }
        #endregion
    }
}