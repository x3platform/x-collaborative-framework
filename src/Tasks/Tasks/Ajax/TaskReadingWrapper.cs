namespace X3Platform.Tasks.Ajax
{
    #region Using Libraries
    using System;
    using System.Text;
    using System.Web;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Configuration;

    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    public class TaskReadingWrapper : ContextWrapper
    {
        ITaskReceiverService service = TasksContext.Instance.TaskReceiverService;

        /// <summary></summary>
        public override void ProcessRequest(HttpContext context)
        {
            // 测试地址
            // /services/Elane/X/Tasks/Ajax.TaskReadingWrapper.aspx?taskId=877193cc-668d-4332-a4f2-d66798b6f6a0&receiverId=administrator

            string action = (context.Request["action"] == null) ? string.Empty : context.Request["action"];

            if (!string.IsNullOrEmpty(action) && action == "read")
            {
                string taskId = (context.Request["taskId"] == null) ? string.Empty : context.Request["taskId"];

                string receiverId = (context.Request["receiverId"] == null) ? string.Empty : context.Request["receiverId"];

                XmlDocument doc = new XmlDocument();

                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<ajaxStorage><taskId>" + taskId + "</taskId><receiverId>" + receiverId + "</receiverId></ajaxStorage>");

                string html = this.Read(doc);

                context.Response.ContentType = "text/html";
                context.Response.Write(html);
                context.Response.End();
            }
        }

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

            string taskId = AjaxStorageConvertor.Fetch("taskId", doc);

            string receiverId = AjaxStorageConvertor.Fetch("receiverId", doc);

            TaskWorkItemInfo param = service.FindOne(taskId, receiverId);

            if (param != null)
            {
                service.SetRead(taskId, receiverId, true);

                // 通知类的任务信息, 查看完后就消失
                if (param.Type == "4")
                {
                    service.SetFinished(receiverId, taskId);
                }

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