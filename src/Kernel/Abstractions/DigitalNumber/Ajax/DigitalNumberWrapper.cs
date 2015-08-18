namespace X3Platform.DigitalNumber.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.DigitalNumber.Model;
    using X3Platform.DigitalNumber.IBLL;
    #endregion

    /// <summary></summary>
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

            string scopeText = XmlHelper.Fetch("scopeText", doc);

            param = (DigitalNumberInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string name = XmlHelper.Fetch("name", doc);

            this.service.Delete(name);

            return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取分页内容 / get paging.</summary>
        /// <param name="paging">paging helper.</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string name = XmlHelper.Fetch("name", doc);

            DigitalNumberInfo param = this.service.FindOne(name);

            outString.Append("{\"data\":" + AjaxUtil.Parse<DigitalNumberInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = 0;

            IList<DigitalNumberInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<DigitalNumberInfo>(list) + ",");

            outString.Append("\"paging\":" + paging + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            DigitalNumberInfo param = new DigitalNumberInfo();

            param.Name = string.Empty;

            param.CreateDate = param.UpdateDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<DigitalNumberInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"创建成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Generate(XmlDocument doc)
        /// <summary>生成流水号</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string Generate(XmlDocument doc)
        {
            string name = XmlHelper.Fetch("name", doc);

            string result = this.service.Generate(name);

            return "{\"data\":\"" + result + "\",\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}";
        }
        #endregion
    }
}