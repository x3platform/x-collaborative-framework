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
    using System.Web;
    using System.IO;
    using X3Platform.Apps;
    using X3Platform.Globalization;
    #endregion

    /// <summary>附件存储信息</summary>
    public class AttachmentFileWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IAttachmentFileService service = AttachmentStorageContext.Instance.AttachmentFileService; // 数据服务

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            if (KernelContext.Current.User == null) { return "{\"message\":{\"returnCode\":1,\"value\":\"必须登陆后才能操作。\"}}"; }

            string id = XmlHelper.Fetch("id", doc);

            this.service.Delete(id);

            return GenericException.Serialize(0, I18n.Strings["msg_delete_success"]);
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

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

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
            string entityId = XmlHelper.Fetch("entityId", doc);
            string entityClassName = XmlHelper.Fetch("entityClassName", doc);
            // 排序条件
            string orderBy = XmlHelper.Fetch("orderBy", doc);
            // 文件列表最大长度
            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityClassName))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"请填写附件所属实体对象信息【entityId】和【entityClassName】。\"}}";
            }

            // 这里需要修改
            DataQuery query = new DataQuery();

            query.Where.Add("entityId", entityId);
            query.Where.Add("entityClassName", entityClassName);

            if (!string.IsNullOrEmpty(orderBy))
            {
                query.Orders.Add(orderBy);
            }

            query.Length = length;

            IList<IAttachmentFileInfo> list = this.service.FindAll(query);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAttachmentFileInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

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

            // 设置查询方案
            paging.Query.Variables["scence"] = "Query";

            int rowCount = -1;

            IList<IAttachmentFileInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAttachmentFileInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Download(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string Download(XmlDocument doc)
        {
            this.ProcessRequest(HttpContext.Current);

            string id = HttpContext.Current.Request.QueryString["id"];

            IAttachmentFileInfo param = AttachmentStorageContext.Instance.AttachmentFileService[id];

            if (param == null)
            {
                ApplicationError.Write(404);
            }
            else
            {
                if (param != null && param.FileData == null)
                {
                    // 下载分布式文件数据
                    param.FileData = DistributedFileStorage.Download(param);
                }

                // [容错] 由于人为原因将服务器上的文件删除。
                if (param.FileData == null)
                {
                    ApplicationError.Write(404);
                }

                HttpContext.Current.Response.ContentType = "application/octet-stream";

                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + EncodeFileName(param.AttachmentName));
                HttpContext.Current.Response.BinaryWrite(param.FileData);
            }

            HttpContext.Current.Response.End();

            return string.Empty;
        }

        /// <summary></summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string EncodeFileName(string fileName)
        {
            string agent = HttpContext.Current.Request.Headers["USER-AGENT"];

            if (!string.IsNullOrEmpty(agent) && agent.IndexOf("MSIE 6.0") != -1)
            {
                string prefix = Path.GetFileNameWithoutExtension(fileName);

                string extension = Path.GetExtension(fileName);

                //
                // 在IE6访问此页面是, 输出文件名不能长于17个中文字符.
                // Bug of IE (http://support.microsoft.com/?kbid=816868)  
                //

                //UTF8 URL encoding only works in IE  
                fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);

                // Encoded fileName cannot be more than 150  
                int limit = 150 - extension.Length;

                if (fileName.Length > limit)
                {
                    // because the UTF-8 encoding scheme uses 9 bytes to represent a single CJK character  
                    fileName = HttpUtility.UrlEncode(prefix.Substring(0, Math.Min(prefix.Length, limit / 9)), Encoding.UTF8);

                    fileName = fileName + extension;
                }

                return fileName;
            }
            else if (!string.IsNullOrEmpty(agent) && agent.IndexOf("MSIE") != -1)
            {
                return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            }
            else if (!string.IsNullOrEmpty(agent) && agent.IndexOf("Mozilla") != -1)
            {
                // return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                return "=?UTF-8?B?" + StringHelper.ToBase64(fileName, "UTF-8") + "?=";
            }
            else
            {
                return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            }
        }

        #endregion
    }
}