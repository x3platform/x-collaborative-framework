using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace X3Platform.Web.Component
{
    /// <summary></summary>
    [ToolboxData(@"<{0}:StyleWidget runat='server' /></{0}:StyleWidget>")]
    public class StyleWidget : Control
    {
        private string m_Href = "";

        [Browsable(true)]
        public virtual string Href
        {
            get { return this.m_Href; }
            set { this.m_Href = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            HtmlGenericControl control = new HtmlGenericControl("link");

            control.Attributes.Add("type", "text/css");
            control.Attributes.Add("rel", "stylesheet");
            control.Attributes.Add("href", "style.css");

            this.Page.Header.Controls.Add(control);
        }
    }
}