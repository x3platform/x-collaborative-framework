using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace X3Platform.Web.Component
{
    /// <summary></summary>
    [ToolboxData(@"<{0}:ScriptWidget runat='server' /></{0}:ScriptWidget>")]
    public class ScriptWidget : Control
    {
        private string m_Src = "";

        [Browsable(true)]
        public virtual string Src
        {
            get { return this.m_Src; }
            set { this.m_Src = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptInclude("xx", Src);
        }
    }
}