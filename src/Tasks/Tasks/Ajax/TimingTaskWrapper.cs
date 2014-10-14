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
    #endregion

    /// <summary></summary>
    public class TimingTaskWrapper : ContextWrapper
    {
        private ITimingTaskService service = TasksContext.Instance.TimingTaskService;

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Save(XmlDocument doc)
        {
            TimingTaskInfo param = new TimingTaskInfo();

            param = (TimingTaskInfo)AjaxStorageConvertor.Deserialize(param, doc);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region ����:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Delete(XmlDocument doc)
        {
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

            this.service.Delete(ids);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<TimingTaskInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":[");

            foreach (TimingTaskInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"applicationId\":\"" + item.ApplicationId + "\",");
                outString.Append("\"taskCode\":\"" + item.TaskCode + "\",");
                outString.Append("\"type\":\"" + item.Type + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(StringHelper.RemoveEnterTag(item.Title)) + "\",");
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
    }
}