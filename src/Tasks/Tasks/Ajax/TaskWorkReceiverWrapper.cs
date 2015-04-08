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
    public class TaskWorkReceiverWrapper : ContextWrapper
    {
        ITaskWorkReceiverService service = TasksContext.Instance.TaskWorkReceiverService;

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            // 设置当前用户权限
            if (XmlHelper.Fetch("su", doc) == "1")
            {
                paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            // 设置特定的业务场景
            if (paging.Query.Where.ContainsKey("scence"))
            {
                // 场景
                // Query 根据关键字查询
                // QueryByOrganizationId 根据组织标识查询
                if (paging.Query.Where["scence"].ToString() == "Query" || paging.Query.Where["scence"].ToString() == "QueryByOrganizationId")
                {
                    paging.Query.Variables["scence"] = paging.Query.Where["scence"].ToString();
                }

                paging.Query.Where.Remove("scence");
            }

            int rowCount = -1;

            IList<TaskWorkItemInfo> list = this.service.GetPaging(KernelContext.Current.User.Id, paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":[");

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
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

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

            string receiverId = XmlHelper.Fetch("receiverId", doc);

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            // 如果接收人为空, 则默认显示当前用户
            if (string.IsNullOrEmpty(receiverId))
            {
                receiverId = KernelContext.Current.User.Id;
            }

            IList<TaskWorkItemInfo> list = this.service.FindAllByReceiverId(receiverId, whereClause, length);

            int rowCount = list.Count;

            outString.Append("{\"data\":[");

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

            outString.Append("],taskCount:\"" + rowCount + "\", \"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Copy(XmlDocument doc)
        /// <summary>复制工作项信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string Copy(XmlDocument doc)
        {
            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string fromReceiverId = XmlHelper.Fetch("fromReceiverId", doc);
            string toReceiverId = XmlHelper.Fetch("toReceiverId", doc);

            DateTime beginDate = Convert.ToDateTime(XmlHelper.Fetch("beginDate", doc));
            DateTime endDate = Convert.ToDateTime(XmlHelper.Fetch("endDate", doc));

            // 格式结束时间为 23:59:59，避免当天没有收到待办信息
            endDate = Convert.ToDateTime(endDate.ToString("yyyy-MM-dd 23:59:59"));

            this.service.Copy(fromReceiverId, toReceiverId, beginDate, endDate);

            return "{\"message\":{\"returnCode\":0,\"value\":\"复制成功。\"}}";
        }
        #endregion

        #region 函数:Cut(XmlDocument doc)
        /// <summary>复制工作项信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string Cut(XmlDocument doc)
        {
            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string fromReceiverId = XmlHelper.Fetch("fromReceiverId", doc);
            string toReceiverId = XmlHelper.Fetch("toReceiverId", doc);

            DateTime beginDate = Convert.ToDateTime(XmlHelper.Fetch("beginDate", doc));
            DateTime endDate = Convert.ToDateTime(XmlHelper.Fetch("endDate", doc));

            // 格式结束时间为 23:59:59，避免当天没有收到待办信息
            endDate = Convert.ToDateTime(endDate.ToString("yyyy-MM-dd 23:59:59"));

            this.service.Cut(fromReceiverId, toReceiverId, beginDate, endDate);

            return "{\"message\":{\"returnCode\":0,\"value\":\"复制成功。\"}}";
        }
        #endregion

        #region 函数:SetStatus(XmlDocument doc)
        /// <summary>设置任务状态</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string SetStatus(XmlDocument doc)
        {
            string taskId = XmlHelper.Fetch("taskId", doc);

            int status = Convert.ToInt32(XmlHelper.Fetch("status", doc));

            this.service.SetStatus(taskId, KernelContext.Current.User.Id, status);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:SetFinished(XmlDocument doc)
        /// <summary>设置任务结束</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string SetFinished(XmlDocument doc)
        {
            string taskIds = XmlHelper.Fetch("taskIds", doc);

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

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:GetUnfinishedQuantities(XmlDocument doc)
        /// <summary>获取列表</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string GetUnfinishedQuantities(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string receiverId = XmlHelper.Fetch("receiverId", doc);

            // 如果接收人为空, 则默认显示当前用户
            if (string.IsNullOrEmpty(receiverId))
            {
                receiverId = KernelContext.Current.User.Id;
            }

            Dictionary<int, int> unfinishedQuantities = this.service.GetUnfinishedQuantities(receiverId);

            outString.Append("{\"data\":[");

            foreach (KeyValuePair<int, int> entry in unfinishedQuantities)
            {
                outString.Append("{");
                outString.Append("\"key\":\"" + entry.Key + "\",");
                outString.Append("\"value\":\"" + entry.Value + "\" ");
                outString.Append("},");
            }

            outString = StringHelper.TrimEnd(outString, ",");

            outString.Append("],receiverId:\"" + receiverId + "\",\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}