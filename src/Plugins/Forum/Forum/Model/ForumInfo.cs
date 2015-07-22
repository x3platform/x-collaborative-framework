using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X3Platform.Plugins.Forum.Model
{
    public class ForumInfo
    {
        #region 属性:ApplicationTag
        private string m_ApplicationTag = string.Empty;

        public string ApplicationTag
        {
            get { return this.m_ApplicationTag; }
            set { this.m_ApplicationTag = value; }
        }
        #endregion

        #region 属性:ApplicationStore
        private string m_ApplicationStore = string.Empty;

        public string ApplicationStore
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ApplicationTag))
                {
                    if (this.ApplicationTag.IndexOf("tb_Forum_$") == -1)
                    {
                        m_ApplicationStore = "tb_Forum_$" + this.ApplicationTag;
                    }
                    else
                    {
                        m_ApplicationStore = this.ApplicationTag;
                    }
                }
                return m_ApplicationStore;
            }
        }
        #endregion
    }
}
