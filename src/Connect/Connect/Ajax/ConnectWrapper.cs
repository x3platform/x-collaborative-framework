namespace X3Platform.Connect.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Configuration;
    using X3Platform.Membership;
    using X3Platform.Util;

    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.Model;
    #endregion

    /// <summary></summary>
    public class ConnectWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IConnectService service = ConnectContext.Instance.ConnectService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            ConnectInfo param = new ConnectInfo();

            param = (ConnectInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

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

            ConnectInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ConnectInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Query(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<ConnectQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ConnectQueryInfo>(list) + ",");

            outString.Append("\"paging\":" + paging + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetMyConnectPages(XmlDocument doc)
        /// <summary>获取我的文档列表数据</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetMyConnectPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            IAccountInfo account = KernelContext.Current.User;

            // paging.WhereClause = paging.WhereClause + (string.IsNullOrEmpty(paging.WhereClause) ? string.Empty : " AND ") + " ( AccountId = ##" + account.Id + "## ) ";

            int rowCount = -1;

            IList<ConnectQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ConnectQueryInfo>(list) + ",");

            outString.Append("\"paging\":" + paging + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:ResetAppSecret(XmlDocument doc)
        /// <summary>获取我的文档列表数据</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string ResetAppSecret(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string appKey = XmlHelper.Fetch("appKey", doc);

            ConnectInfo param = this.service.FindOneByAppKey(appKey);

            if (param == null)
            {
                return "{\"message\":{\"returnCode\":0,\"value\":\"未找到相关【" + appKey + "】应用连接器信息。\"}}";
            }

            this.service.ResetAppSecret(appKey);

            param = this.service.FindOneByAppKey(appKey);

            return "{\"ajaxStorage\":{\"appSecret\":\"" + param.AppSecret + "\"},\"message\":{\"returnCode\":0,\"value\":\"重置 App Secret 成功。\"}}";
        }
        #endregion
    }
}