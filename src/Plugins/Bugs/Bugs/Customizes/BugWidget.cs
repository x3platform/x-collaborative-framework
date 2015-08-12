#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.Customizes
{
    #region Using Libraries
    using System.Collections.Generic;

    using X3Platform.Ajax.Json;
    using X3Platform.Util;
    using X3Platform.Velocity;
    using X3Platform.Web.Customizes;

    using X3Platform.Plugins.Bugs.Model;
    #endregion

    /// <summary>Bug Widget</summary>
    public sealed class BugWidget : IWidget
    {
        private Dictionary<string, string> options = new Dictionary<string, string>();

        private int m_Height = 0;

        /// <summary>高度</summary>
        public int Height
        {
            get { return this.m_Height; }
        }

        private int m_Width = 0;

        /// <summary>宽度</summary>
        public int Width
        {
            get { return this.m_Width; }
        }

        /// <summary></summary>
        public void Load(string options)
        {
            JsonObject optionObjects = JsonObjectConverter.Deserialize(options);

            foreach (string key in optionObjects.Keys)
            {
                this.options.Add(key, ((JsonPrimary)optionObjects[key]).Value.ToString());
            }

            try
            {
                // 设置部件的高度
                int.TryParse(this.options["height"], out  this.m_Height);
                // 设置部件的宽度
                int.TryParse(this.options["width"], out  this.m_Width);
            }
            catch
            {
                this.m_Height = this.m_Width = 0;
            }
        }

        public string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            string whereCaluse = " Status IN (0,1,3) ORDER BY UpdateDate DESC";

            int length = 8;

            IList<BugInfo> list = BugContext.Instance.BugService.FindAll(whereCaluse, length);

            context.Put("list", list);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/bugzilla.vm");
        }
    }
}
