namespace X3Platform.Web.Customizes
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Web.Customizes.Model;

    /// <summary></summary>
    public interface IWidget
    {
        #region 函数:Load(string configuration)
        /// <summary>加载配置信息</summary>
        void Load(string configuration);
        #endregion

        #region 函数:ParseHtml()
        /// <summary>解析Html内容</summary>
        string ParseHtml();
        #endregion
	}
}
