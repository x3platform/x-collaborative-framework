// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :Form.cs
//
// Description  :��չĬ�ϵ�HtmlForm��һЩ����, ʹ��ʧЧ, �����ϵ�ַ��д.
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace X3Platform.Web.UrlRewriter
{
    /// <summary>��ַ��д����ʹ�õ�Form����</summary>
    public class Form : HtmlForm
    {
        #region 属性:RenderAttributes(HtmlTextWriter writer)
        /// <summary> ����ҳ�汻�ض���, ��дHtmlForm��RenderAttributes�����Ƴ�Ĭ��action����, ��ֹҳ���ύ.</summary>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            // 1.��дForm����name��method����.

            writer.WriteAttribute("name", this.Name);
            base.Attributes.Remove("name");

            writer.WriteAttribute("method", this.Method);
            base.Attributes.Remove("method");

            // 2�Ƴ�action����

            base.Attributes.Remove("action");

            // 3.д��������ǩ.

            this.Attributes.Render(writer);

            if (base.ID != null)
            {
                writer.WriteAttribute("id", base.ClientID);
            }
        }
        #endregion
    }
}