#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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

namespace X3Platform.Web.Customize
{
    #region Using Libraries
    using System.Collections.Generic;
    using X3Platform.Util;
    using X3Platform.Velocity;

    using X3Platform.Ajax.Json;
    #endregion

    /// <summary>����������</summary>
    public abstract class AbstractWidget : IWidget
    {
        /// <summary>��������ѡ����Ϣ</summary>
        protected Dictionary<string, string> options = new Dictionary<string, string>();

        private int m_Height = 0;

        /// <summary>�߶�</summary>
        public int Height
        {
            get { return this.m_Height; }
        }

        private int m_Width = 0;

        /// <summary>����</summary>
        public int Width
        {
            get { return this.m_Width; }
        }

        /// <summary>����ѡ����Ϣ</summary>
        public void Load(string options)
        {
            JsonObject optionObjects = JsonObjectConverter.Deserialize(options);

            foreach (string key in optionObjects.Keys)
            {
                this.options.Add(key, ((JsonPrimary)optionObjects[key]).Value.ToString());
            }

            try
            {
                // ���ò����ĸ߶�
                int.TryParse(this.options["height"], out  this.m_Height);
                // ���ò����Ŀ���
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
