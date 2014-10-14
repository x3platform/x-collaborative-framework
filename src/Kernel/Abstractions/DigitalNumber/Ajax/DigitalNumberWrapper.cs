// =============================================================================
//
// Copyright (c) x3platfrom.com
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

namespace X3Platform.DigitalNumber.Ajax
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Util;
    using X3Platform.DigitalNumber.Model;
    using X3Platform.DigitalNumber.IBLL;

    public class DigitalNumberWrapper : ContextWrapper
    {
        private IDigitalNumberService service = DigitalNumberContext.Instance.DigitalNumberService; // 数据提供器

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            DigitalNumberInfo param = new DigitalNumberInfo();

            string scopeText = AjaxStorageConvertor.Fetch("scopeText", doc);

            param = (DigitalNumberInfo)AjaxStorageConvertor.Deserialize(param, doc);

            service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

            service.Delete(ids);

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取分页内容 / get pages.</summary>
        /// <param name="pages">pages helper.</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            DigitalNumberInfo param = service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<DigitalNumberInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = 0;

            IList<DigitalNumberInfo> list = service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<DigitalNumberInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}