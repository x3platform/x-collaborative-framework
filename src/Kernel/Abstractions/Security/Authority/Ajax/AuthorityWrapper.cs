namespace X3Platform.Security.Authority.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Data;
    using X3Platform.Util;

    using X3Platform.Security.Authority.IBLL;
    #endregion

    /// <summary>权限</summary>
    public class AuthorityWrapper : ContextWrapper
    {
        IAuthorityService service = AuthorityContext.Instance.AuthorityService;

        //-------------------------------------------------------
        // 保存 删除
        //-------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            AuthorityInfo param = new AuthorityInfo();

            param = AjaxUtil.Deserialize<AuthorityInfo>(param, doc);

            service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除数据</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            service.Delete(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            AuthorityInfo param = service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<AuthorityInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindOneByName(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string FindOneByName(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string name = XmlHelper.Fetch("name", doc);

            AuthorityInfo param = service.FindOneByName(name);

            outString.Append("{\"data\":" + AjaxUtil.Parse<AuthorityInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // string whereClause = XmlHelper.Fetch("whereClause", doc);

            IList<AuthorityInfo> list = service.FindAll(new DataQuery());

            outString.Append("{\"data\":" + AjaxUtil.Parse<AuthorityInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 属性:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<AuthorityInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<AuthorityInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\",");
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

            AuthorityInfo param = new AuthorityInfo();

            param.Id = StringHelper.ToGuid();

            param.CreateDate = param.UpdateDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<AuthorityInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"创建成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
