#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :TaskWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

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
    public class TaskWrapper : ContextWrapper
    {
        private ITaskService service = TasksContext.Instance.TaskService;

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Send(XmlDocument doc)
        {
            TaskInfo param = new TaskInfo();

            param = (TaskInfo)AjaxStorageConvertor.Deserialize(param, doc);

            XmlNodeList nodes = doc.SelectNodes("/ajaxStorage/receivers/receiver");

            IAccountInfo account = MembershipManagement.Instance.AccountService[param.SenderId];

            if (account == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ���������Ϣ��id:" + param.SenderId + "����\"}}";
            }

            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode("id") != null)
                {
                    account = MembershipManagement.Instance.AccountService[node.SelectSingleNode("id").InnerText];

                    if (account == null)
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ���������Ϣ��id:" + node.SelectSingleNode("id").InnerText + "����\"}}";
                    }
                }
                else if (node.SelectSingleNode("loginName") != null)
                {
                    account = MembershipManagement.Instance.AccountService.FindOneByLoginName(node.SelectSingleNode("loginName").InnerText);

                    if (account == null)
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ���������Ϣ����loginName:" + node.SelectSingleNode("loginName").InnerText + "����\"}}";
                    }
                }
                else
                {
                    account = null;

                    return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ������˱�ʶ�Ĳ�����\"}}";
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
                            return "{\"message\":{\"returnCode\":1,\"value\":\"" + account.Name + "�����ɵģ�����δ�ҵ�����ʱ�䡣\"}}";
                        }

                        finishTime = Convert.ToDateTime(node.SelectSingleNode("finishTime").InnerText);
                    }

                    param.AddReceiver(account.Id, false, isFinished, finishTime);
                }
            }

            if (param.ReceiverGroup.Count == 0)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"������д��������Ϣ��\"}}";
            }

            if (string.IsNullOrEmpty(param.Title))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"������д������Ϣ��\"}}";
            }

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"���ͳɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Delete(XmlDocument doc)
        {
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

            string applicationId = AjaxStorageConvertor.Fetch("applicationId", doc);
            
            string taskCode = AjaxStorageConvertor.Fetch("taskCode", doc);

            if (!string.IsNullOrEmpty(applicationId) && !string.IsNullOrEmpty(taskCode))
            {
                this.service.DeleteByTaskCode(applicationId, taskCode);
            }
            else
            {
                this.service.Delete(ids);
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<TaskInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":[");

            foreach (TaskInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"applicationId\":\"" + item.ApplicationId + "\",");
                outString.Append("\"taskCode\":\"" + item.TaskCode + "\",");
                outString.Append("\"type\":\"" + item.Type + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Title) + "\",");
                outString.Append("\"content\":\"" + StringHelper.ToSafeJson(item.Content) + "\",");
                outString.Append("\"tags\":\"" + item.Tags + "\",");
                outString.Append("\"senderId\":\"" + StringHelper.ToSafeJson(item.SenderId) + "\",");
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

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:SetFinished(XmlDocument doc)
        /// <summary>������������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        public string SetFinished(XmlDocument doc)
        {
            string applicationId = AjaxStorageConvertor.Fetch("applicationId", doc);

            string taskCode = AjaxStorageConvertor.Fetch("taskCode", doc);

            this.service.SetFinished(applicationId, taskCode);

            return "{message:{\"returnCode\":0,\"value\":\"���óɹ���\"}}";
        }
        #endregion

        #region 属性:SetUsersFinished(XmlDocument doc)
        /// <summary>������������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        public string SetUsersFinished(XmlDocument doc)
        {
            string applicationId = AjaxStorageConvertor.Fetch("applicationId", doc);

            string taskCode = AjaxStorageConvertor.Fetch("taskCode", doc);

            XmlNodeList nodes = doc.SelectNodes("/ajaxStorage/receivers/receiver/loginName");

            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByLoginName(node.InnerText);

                    if (account == null)
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"δ�ҵ���������Ϣ����loginName:" + node.SelectSingleNode("loginName").InnerText + "����\"}}";
                    }

                    TasksContext.Instance.TaskReceiverService.SetFinishedByTaskCode(applicationId, taskCode, account.Id);
                }
            }
            else
            {
                nodes = doc.SelectNodes("/ajaxStorage/receivers/receiver/id");

                foreach (XmlNode node in nodes)
                {
                    TasksContext.Instance.TaskReceiverService.SetFinishedByTaskCode(applicationId, taskCode, node.InnerText);
                }
            }

            return "{message:{\"returnCode\":0,\"value\":\"���óɹ���\"}}";
        }
        #endregion

        #region 属性:GetTaskTags(XmlDocument doc)
        /// <summary>��ȡ��ǩ�б�</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        public string GetTaskTags(XmlDocument doc)
        {
            IList<string> list = this.service.GetTaskTags();

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":{\"list\":[");

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

            outString.Append("]},message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �鵵��ɾ��ĳһʱ�εĴ�����¼
        // -------------------------------------------------------

        #region 属性:Archive(XmlDocument doc)
        /// <summary>���鵵����֮ǰ�����ɵĴ����鵵����ʷ���ݱ�</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        public string Archive(XmlDocument doc)
        {
            DateTime archiveDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("archiveDate", doc));

            this.service.Archive(archiveDate);

            return "{message:{\"returnCode\":0,\"value\":\"�鵵�ɹ���\"}}";
        }
        #endregion

        #region 属性:RemoveUnfinishedWorkItems(XmlDocument doc)
        ///<summary>ɾ������ʱ��֮ǰδ���ɵĹ�������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        public string RemoveUnfinishedWorkItems(XmlDocument doc)
        {
            DateTime expireDate = Convert.ToDateTime(AjaxStorageConvertor.Fetch("expireDate", doc));

            this.service.RemoveUnfinishedWorkItems(expireDate);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion
    }
}