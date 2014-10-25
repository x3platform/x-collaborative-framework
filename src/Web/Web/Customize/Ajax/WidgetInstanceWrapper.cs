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

namespace X3Platform.Web.Customize.Ajax
{
    #region Using Libraries
    using System.Xml;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Ajax.Json;
    using X3Platform.Util;

    using X3Platform.Web.Customize.Model;
    using X3Platform.Web.Customize.IBLL;
    #endregion

    /// <summary>����ʵ��</summary>
    public sealed class WidgetInstanceWrapper : ContextWrapper
    {
        private IWidgetInstanceService service = CustomizeContext.Instance.WidgetInstanceService; // ���ݷ���

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            WidgetInstanceInfo param = new WidgetInstanceInfo();

            param = (WidgetInstanceInfo)AjaxUtil.Deserialize(param, doc);

            service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            service.Delete(ids);

            return "{\"message\":{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            WidgetInstanceInfo param = service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<WidgetInstanceInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<WidgetInstanceInfo> list = service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<WidgetInstanceInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:Create(XmlDocument doc)
        /// <summary>��������ʵ��</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("create")]
        public string Create(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            string authorizationObjectType = XmlHelper.Fetch("authorizationObjectType", doc);

            string authorizationObjectId = XmlHelper.Fetch("authorizationObjectId", doc);

            string pageName = XmlHelper.Fetch("pageName", doc);

            string widgetName = XmlHelper.Fetch("widgetName", doc);

            WidgetInstanceInfo param = service.FindOne(id);

            if (param == null)
            {
                param = new WidgetInstanceInfo();

                param = (WidgetInstanceInfo)AjaxUtil.Deserialize(param, doc);

                this.service.SetPageAndWidget(param, authorizationObjectType, authorizationObjectId, pageName, widgetName);

                // ���ò���Ĭ��ѡ��
                WidgetInfo widget = CustomizeContext.Instance.WidgetService.FindOneByName(widgetName);

                param.Height = widget.Height;
                param.Width = widget.Width;
                param.Options = widget.Options;

                this.service.Save(param);
            }

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<WidgetInstanceInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:SetOptions(XmlDocument doc)
        /// <summary>����ѡ����Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("setOptions")]
        public string SetOptions(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string options = XmlHelper.Fetch("options", doc);

            string id = XmlHelper.Fetch("id", doc);

            WidgetInstanceInfo param = this.service.FindOne(id);

            if (param == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ����ز�����" + id + "��ʵ����\"}}";
            }

            param.Options = options;

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"���óɹ���\"}}";
        }
        #endregion

        #region 属性:GetOptionHtml(XmlDocument doc)
        /// <summary>��ȡ���Ա༭��HTML����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("getOptionHtml")]
        public string GetOptionHtml(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            WidgetInstanceInfo param = this.service.FindOne(id);

            if (param == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ����ز�����" + id + "��ʵ����\"}}";
            }

            string optionHtml = this.service.GetOptionHtml(id).Replace("${Id}", id);

            optionHtml = ParseHtml(optionHtml, param.Options);

            outString.Append("{\"ajaxStorage\":\"" + StringHelper.ToSafeJson(optionHtml) + "\",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        private string ParseHtml(string optionHtml, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return optionHtml;
            }

            JsonObject options = JsonObjectConverter.Deserialize(json);

            foreach (string key in options.Keys)
            {
                optionHtml = optionHtml.Replace("${" + key + "}", ((JsonPrimary)options[key]).Value.ToString());
            }

            return optionHtml;
        }
    }
}
