namespace X3Platform.Web.Customizes
{
    #region Using Libraries
    using System.Collections.Generic;
    using X3Platform.Util;
    using X3Platform.Velocity;

    using X3Platform.Ajax.Json;
    #endregion

    /// <summary>部件抽象类</summary>
    public abstract class AbstractWidget : IWidget
    {
        /// <summary>部件配置选项信息</summary>
        protected Dictionary<string, string> options = new Dictionary<string, string>();

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

        /// <summary>加载选项信息</summary>
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

        public abstract string ParseHtml();
    }
}
