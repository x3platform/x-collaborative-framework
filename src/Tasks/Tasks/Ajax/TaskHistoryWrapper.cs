namespace X3Platform.Tasks.Ajax
{
    #region Using Libraries
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;
    using System.Web;

    using X3Platform.Ajax;
    using X3Platform.Util;
    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.IBLL;
    #endregion

    /// <summary></summary>
    public class TaskHistoryWrapper : ContextWrapper
    {
        ITaskHistoryService service = TasksContext.Instance.TaskHistoryService;

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<TaskHistoryItemInfo> list = this.service.GetPaging(KernelContext.Current.User.Id, pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<TaskHistoryItemInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Redirect(XmlDocument doc)
        /// <summary>读取任务信息</summary>
        /// <param name="doc">Xml 文档对象</param> 
        public string Redirect(XmlDocument doc)
        {
            string html = this.Read(doc);

            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();

            return string.Empty;
        }
        #endregion

        #region 私有函数:Read(XmlDocument doc)
        /// <summary>读取任务信息</summary>
        /// <param name="doc">Xml 文档对象</param> 
        private string Read(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string taskId = XmlHelper.Fetch("taskId", doc);

            string receiverId = XmlHelper.Fetch("receiverId", doc);

            TaskHistoryItemInfo param = service.FindOne(taskId, receiverId);

            if (param != null)
            {
                // -------------------------------------------------------
                // 设置跳转的页面
                // -------------------------------------------------------

                string url = param.Content;

                outString.AppendFormat("<span style=\"font-size:12px;padding:10px;\" ><a href=\"{0}\">正在跳转到任务相关页面，请稍候...</a></span>", url);
                outString.AppendFormat("<script type=\"text/javascript\" >location.href='{0}';</script>", url);
            }

            return outString.ToString();
        }
        #endregion
    }
}