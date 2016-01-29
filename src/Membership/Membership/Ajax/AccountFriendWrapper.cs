namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using System.Text;

    using X3Platform;
    using X3Platform.Ajax;
    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Globalization;
    using X3Platform.Util;

    using X3Platform.Membership;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public class AccountFriendWrapper
    {
        /// <summary>数据服务</summary>
        private IAccountFriendService service = MembershipManagement.Instance.AccountFriendService;

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

            string friendAccountId = XmlHelper.Fetch("friendAccountId", doc);

            IAccountInfo friendAccount = MembershipManagement.Instance.AccountService[friendAccountId];

            if (friendAccount == null)
            {
                return GenericException.Serialize(1, "用户信息不存在。");
            }

            AccountFriendInfo param = this.service.FindOne(KernelContext.Current.User.Id, friendAccountId);

            outString.Append("{\"data\":{");
            outString.Append("\"accountId\":\"" + friendAccount.Id + "\",");
            outString.Append("\"name\":\"" + friendAccount.Name + "\",");
            outString.Append("\"displayName\":\"" + ((param == null) ? string.Empty : param.FriendDisplayName) + "\",");
            outString.Append("\"certifiedAvatarView\":\"" + friendAccount.CertifiedAvatarView + "\",");
            outString.Append("\"isFriend\":" + ((param == null) ? 0 : 1));
            outString.Append("},");

            outString.Append(GenericException.Serialize(0, "查询成功。", true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string searchText = XmlHelper.Fetch("searchText", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            DataQuery query = DataQuery.Create(XmlHelper.Fetch("query", doc, "xml"));

            query.Length = length;

            IList<AccountFriendInfo> list = this.service.FindAll(query);

            outString.Append("{\"data\":" + AjaxUtil.Parse<AccountFriendInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取带分页的列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetMyListPaging(XmlDocument doc)
        /// <summary>获取我的签约商家信息</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetMyListPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            paging.Query.Variables["scence"] = "QueryMyList";
            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            IList<AccountFriendInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<AccountFriendInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetAcceptListPaging(XmlDocument doc)
        /// <summary>获取加我好友请求信息</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetAcceptListPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            paging.Query.Variables["scence"] = "QueryAcceptList";
            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            DataTable table = this.service.GetAcceptListPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            // outString.Append("{\"data\":" + AjaxUtil.Parse<AccountFriendInfo>(list) + ",");
            outString.Append("{\"data\":[");

            foreach (DataRow row in table.Rows)
            {
                IAccountInfo friend = MembershipUtil.GetAccount(row["FriendAccountId"].ToString());

                outString.Append("{");
                outString.Append("\"accountId\":\"" + row["AccountId"].ToString() + "\",");
                outString.Append("\"friendAccountId\":\"" + row["FriendAccountId"].ToString() + "\",");
                outString.Append("\"friendAccountName\":\"" + friend.Name + "\",");
                outString.Append("\"friendCertifiedAvatar\":\"" + friend.CertifiedAvatar + "\",");
                outString.Append("\"friendCertifiedAvatarView\":\"" + friend.CertifiedAvatarView + "\",");
                outString.Append("\"reason\":\"" + StringHelper.ToSafeJson(row["Reason"].ToString()) + "\",");
                outString.Append("\"status\":" + row["Status"].ToString() + ",");
                outString.Append("\"createdDateView\":\"" + ((DateTime)row["CreatedDate"]).ToString("yyyy-MM-dd") + "\"");
                outString.Append("},");
            }

            outString = StringHelper.TrimEnd(outString, ",");

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

        #region 函数:Invite(XmlDocument doc)
        /// <summary>邀请好友</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Invite(XmlDocument doc)
        {
            string accountId = KernelContext.Current.User.Id;
            string friendAccountId = XmlHelper.Fetch("friendAccountId", doc);
            string reason = XmlHelper.Fetch("reason", doc);

            this.service.Invite(accountId, friendAccountId, reason);

            return "{\"message\":{\"returnCode\":0,\"value\":\"邀请成功，请等待对方同意。\"}}";
        }
        #endregion

        #region 函数:Accept(XmlDocument doc)
        /// <summary>同意好友邀请</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Accept(XmlDocument doc)
        {
            string accountId = KernelContext.Current.User.Id;
            string friendAccountId = XmlHelper.Fetch("friendAccountId", doc);

            this.service.Accept(accountId, friendAccountId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"已同意好友邀请。\"}}";
        }
        #endregion

        #region 函数:Unfriend(XmlDocument doc)
        /// <summary>解除好友关系</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Unfriend(XmlDocument doc)
        {
            string accountId = KernelContext.Current.User.Id;
            string friendAccountId = XmlHelper.Fetch("friendAccountId", doc);

            this.service.Unfriend(accountId, friendAccountId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"已成功解除好友关系。\"}}";
        }
        #endregion

        #region 函数:SetDisplayName(XmlDocument doc)
        /// <summary>设置好友的显示名称</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SetDisplayName(XmlDocument doc)
        {
            string accountId = KernelContext.Current.User.Id;
            string friendAccountId = XmlHelper.Fetch("friendAccountId", doc);
            string friendDisplayName = XmlHelper.Fetch("friendDisplayName", doc);

            this.service.SetDisplayName(accountId, friendAccountId, friendDisplayName);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion
    }
}
