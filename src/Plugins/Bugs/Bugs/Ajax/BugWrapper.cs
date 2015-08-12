#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :BugWrapper.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Ajax.Net;
    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Email.Client;
    using X3Platform.Membership;
    using X3Platform.Util;

    using X3Platform.Plugins.Bugs.IBLL;
    using X3Platform.Plugins.Bugs.Model;
    #endregion

    public class BugWrapper : ContextWrapper
    {
        private IBugService service = BugContext.Instance.BugService; // 数据服务

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            BugInfo param = new BugInfo();

            param = (BugInfo)AjaxUtil.Deserialize(param, doc);

            param.Properties["FromStatus"] = XmlHelper.Fetch("fromStatus", doc);
            param.Properties["ToStatus"] = XmlHelper.Fetch("toStatus", doc);

            bool isNewObject = !this.service.IsExist(param.Id);

            this.service.Save(param);

            if (isNewObject)
            {
                ApplicationInfo application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

                Uri actionUri = new Uri(KernelConfigurationView.Instance.HostName + "/api/timeline.save.aspx?client_id=" + application.Id + "&client_secret=" + application.ApplicationSecret);

                string taskCode = DigitalNumberContext.Generate("Key_Guid");

                string content = string.Format("报告了一个新的问题【{0}】。<a href=\"{1}/bugzilla/article/{2}.aspx\" target=\"_blank\" >网页链接</a>", param.Title, KernelConfigurationView.Instance.HostName, param.Id);

                string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<request>
    <id><![CDATA[{0}]]></id>
    <applicationId><![CDATA[{1}]]></applicationId>
    <accountId><![CDATA[{2}]]></accountId>
    <content><![CDATA[{3}]]></content>
</request>
", taskCode, application.Id, KernelContext.Current.User.Id, content);

                // 发送请求信息
                AjaxRequestData reqeustData = new AjaxRequestData();

                reqeustData.ActionUri = actionUri;
                reqeustData.Args.Add("xml", xml);

                AjaxRequest.RequestAsync(reqeustData, null);
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            this.service.Delete(id);

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

            BugInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<BugInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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

            // 设置当前用户权限
            if (XmlHelper.Fetch("su", doc) == "1" && AppsSecurity.IsAdministrator(KernelContext.Current.User, BugConfiguration.ApplicationName))
            {
              paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            IList<BugQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<BugQueryInfo>(list) + ",");

            outString.Append("\"paging\":" + paging + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetMyBugPages(XmlDocument doc)
        /// <summary>获取我的文档列表数据</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetMyListPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            // 设置当前用户权限
            if (XmlHelper.Fetch("su", doc) == "1" && AppsSecurity.IsAdministrator(KernelContext.Current.User, BugConfiguration.ApplicationName))
            {
              paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            IAccountInfo account = KernelContext.Current.User;

            // paging.WhereClause = paging.WhereClause + (string.IsNullOrEmpty(paging.WhereClause) ? string.Empty : " AND ") + " ( AccountId = ##" + account.Id + "## OR AssignToAccountId = ##" + account.Id + "## ) ";

            int rowCount = -1;

            IList<BugQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<BugQueryInfo>(list) + ",");

            outString.Append("\"paging\":" + paging + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}