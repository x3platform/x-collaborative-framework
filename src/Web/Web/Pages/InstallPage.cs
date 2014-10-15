// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :InstallPage.cs
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
    using System.Web.UI;

    using X3Platform.Velocity;

    /// <summary>系统安装页面</summary>
    public class InstallPage : Page
    {
        #region 函数:Render(HtmlTextWriter writer)
        protected override void Render(HtmlTextWriter writer)
        {
            string templatePath = "/";

            VelocityContext velocityContext = new VelocityContext();

            // header

            velocityContext.Put("head", "I'm head.");

            // body

            velocityContext.Put("body", "I'm body.");

            Response.Write(VelocityManager.Instance.Merge(velocityContext, templatePath));
        }
        #endregion
    }
}