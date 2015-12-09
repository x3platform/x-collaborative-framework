namespace X3Platform.TemplateContent.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.TemplateContent.Model;
    using X3Platform.TemplateContent.IBLL;
    #endregion

    /// <summary>模板内容</summary>
    public sealed class TemplateContentWrapper : ContextWrapper
    {
        private ITemplateContentService service = TemplateContentContext.Instance.TemplateContentService; // 数据服务

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOneByName(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindOneByName(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string name = XmlHelper.Fetch("name", doc);

            TemplateContentInfo param = service.FindOneByName(name);

            outString.Append("{\"data\":" + AjaxUtil.Parse<TemplateContentInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
