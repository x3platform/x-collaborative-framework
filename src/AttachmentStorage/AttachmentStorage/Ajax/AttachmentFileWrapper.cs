namespace X3Platform.AttachmentStorage.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Data;
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.IBLL;
    #endregion

    /// <summary>附件存储信息</summary>
    public class AttachmentFileWrapper : ContextWrapper
    {
        private IAttachmentFileService service = AttachmentStorageContext.Instance.AttachmentFileService; // 数据服务

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            AttachmentFileInfo param = new AttachmentFileInfo();

            string sendEmail = XmlHelper.Fetch("sendEmail", doc);

            param = (AttachmentFileInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            this.service.Delete(ids);

            return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            IAttachmentFileInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAttachmentFileInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查找成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取列表内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // SQL过滤条件
            string whereClause = XmlHelper.Fetch("whereClause", doc);
            // 排序条件
            string orderBy = XmlHelper.Fetch("orderBy", doc);
            // 文件列表最大长度
            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            if (string.IsNullOrEmpty(whereClause))
            {
                outString.Append("\"message\":{\"returnCode\":1,\"value\":\"请填写过滤条件【WhereClause】。\"}}");
            }
            else
            {
                if (!string.IsNullOrEmpty(orderBy) && whereClause.ToUpper().IndexOf(" ORDER BY ") == -1)
                {
                    whereClause = whereClause + " ORDER BY " + orderBy;
                }
            }

            // 这里需要修改
            DataQuery query = new DataQuery();

            if (!string.IsNullOrEmpty(orderBy))
            {
                query.Orders.Add(orderBy);
            }

            IList<IAttachmentFileInfo> list = this.service.FindAll(query);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAttachmentFileInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:Query(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string Query(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<IAttachmentFileInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAttachmentFileInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查找成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}