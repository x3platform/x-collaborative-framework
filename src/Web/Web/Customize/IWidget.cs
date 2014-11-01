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

namespace X3Platform.Web.Customize
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Web.Customize.Model;

    /// <summary></summary>
    public interface IWidget
    {
        #region 属性:Load(string configuration)
        /// <summary>����������Ϣ</summary>
        void Load(string configuration);
        #endregion

        #region 属性:ParseHtml()
        /// <summary>����Html����</summary>
        string ParseHtml();
        #endregion
	}
}
