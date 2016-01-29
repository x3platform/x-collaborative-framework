namespace X3Platform.Plugins.Forum.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;
    using System.Web;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Plugins.Forum.IBLL;
    using X3Platform.Plugins.Forum.Model;
    using X3Platform.Location.IPQuery;
    using X3Platform.Globalization;
    #endregion
    public class ForumFollowWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IForumFollowService service = ForumContext.Instance.ForumFollowService;

        // -------------------------------------------------------
        // 添加 删除
        // -------------------------------------------------------

        #region 函数:Insert(XmlDocument doc)
        /// <summary>
        /// 添加关注用户
        /// </summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("insert")]
        public string Insert(XmlDocument doc)
        {
            string followAccountId = XmlHelper.Fetch("followAccountId", doc);
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            string value = "";
            if (KernelContext.Current.User.Id == followAccountId)
            {
                value = "您不能关注自己！";
            }
            else
            {
                if (this.service.IsExist(KernelContext.Current.User.Id, followAccountId))
                {
                    value = "您已关注过了，请不要重复添加关注！";
                }
                else
                {
                    this.service.Insert(KernelContext.Current.User.Id, followAccountId);
                    ForumContext.Instance.ForumMemberService.SetFollowCount(followAccountId, 1);
                    value = "已成功添加到关注列表！";
                }
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + value + "\"}}";
        }
        #endregion

        #region 函数:InsertMore(XmlDocument doc)
        /// <summary>
        /// 添加关注用户
        /// </summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("insertMore")]
        public string InsertMore(XmlDocument doc)
        {
            string authorizationObjectText = XmlHelper.Fetch("authorizationObjectText", doc);
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);
            string accountId = KernelContext.Current.User.Id;

            // 操作结果
            string value = string.Empty;
            // 总选择人数
            int amount = 0;
            // 成功导入人数
            int count = 0;

            if (!(string.IsNullOrEmpty(authorizationObjectText) || string.IsNullOrEmpty(applicationTag)))
            {
                string[] followObject = authorizationObjectText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                amount = followObject.Length;

                foreach (var item in followObject)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] followInfo = item.Split('#');
                        string followAccountId = followInfo[1];

                        if (accountId != followAccountId)
                        {
                            if (!this.service.IsExist(accountId, followAccountId))
                            {
                                count++;
                                this.service.Insert(accountId, followAccountId);
                            }
                        }
                    }
                }
            }

            value = string.Format("您选择了{0}位人员，成功关注了{1}为人员！", amount, count);
            if (amount != count)
            {
                value += "\\n选择人员中可能包含已关注的或者自己。";
            }
            return "{\"message\":{\"returnCode\":0,\"value\":\"" + value + "\"}}";
        }
        #endregion

        #region 函数:Delete(string,accountId,string followAccountId)
        /// <summary>
        /// 删除关注用户
        /// </summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string followAccountId = XmlHelper.Fetch("followAccountId", doc);
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            if (this.service.IsExist(KernelContext.Current.User.Id, followAccountId))
            {
                this.service.Delete(KernelContext.Current.User.Id, followAccountId);
                ForumContext.Instance.ForumMemberService.SetFollowCount(followAccountId, -1);
            }

            return GenericException.Serialize(0, I18n.Strings["msg_delete_success"]);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("getPages")]
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));
            // string applicationTag = XmlHelper.Fetch("applicationTag", doc);
            int rowCount = -1;

            IList<ForumFollowQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ForumFollowQueryInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion
    }
}
