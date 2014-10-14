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

        #region ����:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<TaskHistoryItemInfo> list = this.service.GetPages(KernelContext.Current.User.Id, pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<TaskHistoryItemInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:Redirect(XmlDocument doc)
        /// <summary>��ȡ������Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param> 
        public string Redirect(XmlDocument doc)
        {
            string html = this.Read(doc);

            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();

            return string.Empty;
        }
        #endregion

        #region ˽�к���:Read(XmlDocument doc)
        /// <summary>��ȡ������Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param> 
        private string Read(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string taskId = AjaxStorageConvertor.Fetch("taskId", doc);

            string receiverId = AjaxStorageConvertor.Fetch("receiverId", doc);

            TaskHistoryItemInfo param = service.FindOne(taskId, receiverId);

            if (param != null)
            {
                // -------------------------------------------------------
                // ������ת��ҳ��
                // -------------------------------------------------------

                string url = param.Content;

                outString.AppendFormat("<span style=\"font-size:12px;padding:10px;\" ><a href=\"{0}\">������ת����������ҳ�棬���Ժ�...</a></span>", url);
                outString.AppendFormat("<script type=\"text/javascript\" >location.href='{0}';</script>", url);
            }

            return outString.ToString();
        }
        #endregion
    }
}