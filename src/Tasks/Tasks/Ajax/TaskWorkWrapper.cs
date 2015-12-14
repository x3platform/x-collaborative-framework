namespace X3Platform.Tasks.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.Model;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class TaskWorkWrapper : ContextWrapper
    {
        private ITaskWorkService service = TasksContext.Instance.TaskWorkService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Send(XmlDocument doc)
        {
            TaskWorkInfo param = new TaskWorkInfo();

            param = (TaskWorkInfo)AjaxUtil.Deserialize(param, doc);

            XmlNodeList nodes = doc.SelectNodes("/request/receivers/receiver");

            IAccountInfo account = MembershipManagement.Instance.AccountService[param.SenderId];

            if (account == null)
            {
                // 如果默认状态下没有填写发送者标识(SenderId), 则根据填写的发送者的登录名信息查找相关标识信息.
                XmlNode senderNode = doc.SelectSingleNode("/request/sender/loginName");

                if (senderNode == null)
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"未找到发送人信息【id:" + param.SenderId + "】。\"}}";
                }
                else
                {
                    account = MembershipManagement.Instance.AccountService.FindOneByLoginName(senderNode.InnerText);

                    if (account == null)
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"未找到发送人信息【loginName:" + senderNode.InnerText + "】。\"}}";
                    }
                    else
                    {
                        param.SenderId = account.Id;
                    }
                }
            }

            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode("id") != null)
                {
                    account = MembershipManagement.Instance.AccountService[node.SelectSingleNode("id").InnerText];

                    if (account == null)
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"未找到接收人信息【id:" + node.SelectSingleNode("id").InnerText + "】。\"}}";
                    }
                }
                else if (node.SelectSingleNode("loginName") != null)
                {
                    account = MembershipManagement.Instance.AccountService.FindOneByLoginName(node.SelectSingleNode("loginName").InnerText);

                    if (account == null)
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"未找到接收人信息，【loginName:" + node.SelectSingleNode("loginName").InnerText + "】。\"}}";
                    }
                }
                else
                {
                    account = null;

                    return "{\"message\":{\"returnCode\":1,\"value\":\"未找到接收人标识的参数。\"}}";
                }

                if (node.SelectSingleNode("isFinished") == null)
                {
                    param.AddReceiver(account.Id);
                }
                else
                {
                    int isFinished = Convert.ToInt32(node.SelectSingleNode("isFinished").InnerText);

                    DateTime finishTime = new DateTime(2000, 1, 1);

                    if (isFinished == 1)
                    {
                        if (node.SelectSingleNode("finishTime") == null)
                        {
                            return "{\"message\":{\"returnCode\":1,\"value\":\"" + account.Name + "已完成的，但是未找到完成时间。\"}}";
                        }

                        finishTime = Convert.ToDateTime(node.SelectSingleNode("finishTime").InnerText);
                    }

                    param.AddReceiver(account.Id, false, isFinished, finishTime);
                }
            }

            if (param.ReceiverGroup.Count == 0)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写接收人信息。\"}}";
            }

            if (string.IsNullOrEmpty(param.Title))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写标题信息。\"}}";
            }

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"发送成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string taskCode = XmlHelper.Fetch("taskCode", doc);

            if (!string.IsNullOrEmpty(applicationId) && !string.IsNullOrEmpty(taskCode))
            {
                this.service.DeleteByTaskCode(applicationId, taskCode);
            }
            else
            {
                this.service.Delete(ids);
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回一个相关的实例列表.</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<TaskWorkItemInfo> list = this.service.GetPaging(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"data\":[");

            foreach (TaskWorkItemInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"applicationId\":\"" + item.ApplicationId + "\",");
                outString.Append("\"taskCode\":\"" + item.TaskCode + "\",");
                outString.Append("\"type\":\"" + item.Type + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Title) + "\",");
                outString.Append("\"content\":\"" + StringHelper.ToSafeJson(item.Content) + "\",");
                outString.Append("\"tags\":\"" + StringHelper.ToSafeJson(item.Tags) + "\",");
                outString.Append("\"senderId\":\"" + StringHelper.ToSafeJson(item.SenderId) + "\",");
                outString.Append("\"receiverId\":\"" + StringHelper.ToSafeJson(item.ReceiverId) + "\",");
                outString.Append("\"status\":\"" + item.Status + "\",");
                outString.Append("\"createDate\":\"" + item.CreateDate.ToString("yyyy,MM,dd,HH,mm,ss") + "\",");
                outString.Append("\"createDateView\":\"" + item.CreateDate.ToString("yyyy-MM-dd") + "\"");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SetFinished(XmlDocument doc)
        /// <summary>设置任务结束</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string SetFinished(XmlDocument doc)
        {
            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string taskCode = XmlHelper.Fetch("taskCode", doc);

            this.service.SetFinished(applicationId, taskCode);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:SetUsersFinished(XmlDocument doc)
        /// <summary>设置任务结束</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string SetUsersFinished(XmlDocument doc)
        {
            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string taskCode = XmlHelper.Fetch("taskCode", doc);

            XmlNodeList nodes = doc.SelectNodes("/ajaxStorage/receivers/receiver/loginName");

            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByLoginName(node.InnerText);

                    if (account != null)
                    {
                        TasksContext.Instance.TaskWorkReceiverService.SetFinishedByTaskCode(applicationId, taskCode, account.Id);
                    }
                }
            }
            else
            {
                nodes = doc.SelectNodes("/ajaxStorage/receivers/receiver/id");

                foreach (XmlNode node in nodes)
                {
                    TasksContext.Instance.TaskWorkReceiverService.SetFinishedByTaskCode(applicationId, taskCode, node.InnerText);
                }
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:GetTaskTags(XmlDocument doc)
        /// <summary>获取标签列表</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string GetTaskTags(XmlDocument doc)
        {
            IList<string> list = this.service.GetTaskTags();

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"data\":{\"list\":[");

            foreach (string item in list)
            {
                outString.Append("{");
                outString.Append("\"text\":\"" + item + "\",");
                outString.Append("\"value\":\"" + item + "\"");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("]},message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 归档、删除某一时段的待办记录
        // -------------------------------------------------------

        #region 函数:Archive(XmlDocument doc)
        /// <summary>将归档日期之前已完成的待办归档到历史数据表</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string Archive(XmlDocument doc)
        {
            DateTime archiveDate = Convert.ToDateTime(XmlHelper.Fetch("archiveDate", doc));

            this.service.Archive(archiveDate);

            return "{message:{\"returnCode\":0,\"value\":\"归档成功。\"}}";
        }
        #endregion

        #region 函数:RemoveUnfinishedWorkItems(XmlDocument doc)
        /// <summary>删除过期时间之前未完成的工作项记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string RemoveUnfinishedWorkItems(XmlDocument doc)
        {
            DateTime expireDate = Convert.ToDateTime(XmlHelper.Fetch("expireDate", doc));

            this.service.RemoveUnfinishedWorkItems(expireDate);

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion
    }
}