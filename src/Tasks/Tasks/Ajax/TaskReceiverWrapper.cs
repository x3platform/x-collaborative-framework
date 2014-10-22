namespace X3Platform.Tasks.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;
    using System.Collections;

    using X3Platform.Ajax;
    using X3Platform.Util;
    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class TaskReceiverWrapper : ContextWrapper
    {
        ITaskReceiverService service = TasksContext.Instance.TaskReceiverService;

        #region 函数:GetPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<TaskWorkItemInfo> list = this.service.GetPages(KernelContext.Current.User.Id, pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":[");

            foreach (TaskWorkItemInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"applicationId\":\"" + item.ApplicationId + "\",");
                outString.Append("\"taskCode\":\"" + item.TaskCode + "\",");
                outString.Append("\"type\":\"" + item.Type + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Title) + "\",");
                outString.Append("\"tags\":\"" + item.Tags + "\",");
                outString.Append("\"content\":\"" + StringHelper.ToSafeJson(item.Content) + "\",");
                outString.Append("\"senderId\":\"" + StringHelper.ToSafeJson(item.SenderId) + "\",");
                outString.Append("\"receiverId\":\"" + item.ReceiverId + "\",");
                outString.Append("\"status\":\"" + item.Status + "\",");
                outString.Append("\"isRead\":\"" + item.IsRead + "\",");
                outString.Append("\"finishTime\":\"" + StringHelper.ToDate(item.FinishTime) + "\", ");
                outString.Append("\"createDate\":\"" + StringHelper.ToDate(item.CreateDate) + "\" ");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功.\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByReceiverId(XmlDocument doc)
        /// <summary>获取列表</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string FindAllByReceiverId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string receiverId = AjaxStorageConvertor.Fetch("receiverId", doc);

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            // 如果接收人为空, 则默认显示当前用户
            if (string.IsNullOrEmpty(receiverId))
            {
                receiverId = KernelContext.Current.User.Id;
            }

            IList<TaskWorkItemInfo> list = this.service.FindAllByReceiverId(receiverId, whereClause, length);

            int rowCount = list.Count;

            outString.Append("{'ajaxStorage':[");

            foreach (TaskWorkItemInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"type\":\"" + item.Type + "\",");
                outString.Append("\"taskCode\":\"" + StringHelper.ToSafeJson(item.TaskCode) + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(StringHelper.RemoveEnterTag(item.Title)) + "\",");
                outString.Append("\"tags\":\"" + StringHelper.ToSafeJson(item.Tags) + "\",");
                outString.Append("\"content\":\"" + StringHelper.ToSafeJson(item.Content) + "\",");
                outString.Append("\"senderId\":\"" + item.SenderId + "\",");
                outString.Append("\"receiverId\":\"" + item.ReceiverId + "\",");
                outString.Append("\"status\":\"" + item.Status + "\",");
                outString.Append("\"isRead\":\"" + item.IsRead + "\",");
                outString.Append("\"finishTime\":\"" + StringHelper.ToDate(item.FinishTime) + "\",");
                outString.Append("\"createDate\":\"" + StringHelper.ToDate(item.CreateDate) + "\" ");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],taskCount:\"" + rowCount + "\", message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Copy(XmlDocument doc)
        /// <summary>复制工作项信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string Copy(XmlDocument doc)
        {
            string applicationId = AjaxStorageConvertor.Fetch("applicationId", doc);

            string fromReceiverId = AjaxStorageConvertor.Fetch("fromReceiverId", doc);
            string toReceiverId = AjaxStorageConvertor.Fetch("toReceiverId", doc);

            DateTime beginDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("beginDate", doc));
            DateTime endDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("endDate", doc));

            // 格式结束时间为 23:59:59，避免当天没有收到待办信息
            endDate = Convert.ToDateTime(endDate.ToString("yyyy-MM-dd 23:59:59"));

            this.service.Copy(fromReceiverId, toReceiverId, beginDate, endDate);

            return "{message:{\"returnCode\":0,\"value\":\"复制成功。\"}}";
        }
        #endregion

        #region 函数:Cut(XmlDocument doc)
        /// <summary>复制工作项信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string Cut(XmlDocument doc)
        {
            string applicationId = AjaxStorageConvertor.Fetch("applicationId", doc);

            string fromReceiverId = AjaxStorageConvertor.Fetch("fromReceiverId", doc);
            string toReceiverId = AjaxStorageConvertor.Fetch("toReceiverId", doc);

            DateTime beginDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("beginDate", doc));
            DateTime endDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("endDate", doc));

            // 格式结束时间为 23:59:59，避免当天没有收到待办信息
            endDate = Convert.ToDateTime(endDate.ToString("yyyy-MM-dd 23:59:59"));

            this.service.Cut(fromReceiverId, toReceiverId, beginDate, endDate);

            return "{message:{\"returnCode\":0,\"value\":\"复制成功。\"}}";
        }
        #endregion

        #region 函数:SetStatus(XmlDocument doc)
        /// <summary>设置任务状态</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string SetStatus(XmlDocument doc)
        {
            string taskId = AjaxStorageConvertor.Fetch("taskId", doc);

            int status = Convert.ToInt32(AjaxStorageConvertor.Fetch("status", doc));

            this.service.SetStatus(taskId, KernelContext.Current.User.Id, status);

            return "{message:{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:SetFinished(XmlDocument doc)
        /// <summary>设置任务结束</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string SetFinished(XmlDocument doc)
        {
            string taskIds = AjaxStorageConvertor.Fetch("taskIds", doc);

            IAccountInfo account = KernelContext.Current.User;

            if (string.IsNullOrEmpty(taskIds))
            {
                this.service.SetFinished(account.Id);
            }
            else
            {
                taskIds = taskIds.Trim(',');

                this.service.SetFinished(account.Id, taskIds);
            }

            return "{message:{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:GetUnfinishedQuantities(XmlDocument doc)
        /// <summary>获取列表</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string GetUnfinishedQuantities(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string receiverId = AjaxStorageConvertor.Fetch("receiverId", doc);

            // 如果接收人为空, 则默认显示当前用户
            if (string.IsNullOrEmpty(receiverId))
            {
                receiverId = KernelContext.Current.User.Id;
            }

            Dictionary<int, int> unfinishedQuantities = this.service.GetUnfinishedQuantities(receiverId);

            outString.Append("{'ajaxStorage':[");

            foreach (KeyValuePair<int, int> entry in unfinishedQuantities)
            {
                outString.Append("{");
                outString.Append("\"key\":\"" + entry.Key + "\",");
                outString.Append("\"value\":\"" + entry.Value + "\" ");
                outString.Append("},");
            }

            outString = StringHelper.TrimEnd(outString, ",");

            outString.Append("],receiverId:\"" + receiverId + "\",message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}