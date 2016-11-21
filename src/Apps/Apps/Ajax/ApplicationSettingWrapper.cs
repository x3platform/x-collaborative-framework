namespace X3Platform.Apps.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Globalization;
    using X3Platform.Messages;
    using X3Platform.Util;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationSettingWrapper
    {
        /// <summary>数据服务</summary>
        private IApplicationSettingService service = AppsContext.Instance.ApplicationSettingService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            ApplicationSettingInfo param = new ApplicationSettingInfo();

            param = (ApplicationSettingInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            return MessageObject.Stringify("0", I18n.Strings["msg_save_success"]);
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

            return MessageObject.Stringify("0", I18n.Strings["msg_delete_success"]);
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

            ApplicationSettingInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationSettingInfo>(param) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<ApplicationSettingInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationSettingInfo>(list) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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
            if (XmlHelper.Fetch("su", doc) == "1")
            {
                paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            IList<ApplicationSettingInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationSettingInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        #region 函数:IsExist(XmlDocument doc)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string IsExist(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            bool result = this.service.IsExist(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string applicationSettingGroupId = XmlHelper.Fetch("applicationSettingGroupId", doc);

            ApplicationSettingInfo param = new ApplicationSettingInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ApplicationId = applicationId;

            param.ApplicationSettingGroupId = "00000000-0000-0000-0000-000000000000";

            param.Status = 1;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationSettingInfo>(param) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetCombobox(XmlDocument doc)
        /// <summary>获取权重值列表</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string GetCombobox(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string combobox = XmlHelper.Fetch("combobox", doc);

            string selectedValue = XmlHelper.Fetch("selectedValue", doc);

            string emptyItemText = XmlHelper.Fetch("emptyItemText", doc);

            // 系统将逐渐取消 直接写 Where 条件的查询

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            // 应用参数分组名称
            string applicationSettingGroupName = XmlHelper.Fetch("applicationSettingGroupName", doc);

            // 容错处理
            if (string.IsNullOrEmpty(selectedValue))
            {
                selectedValue = "-1";
            }

            whereClause = " ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##" + applicationSettingGroupName + "## ) ";

            if (whereClause.ToUpper().IndexOf(" Status ") == -1)
            {
                // 只读取启用状态的数据
                whereClause = " Status = 1 AND " + whereClause;
            }

            if (whereClause.ToUpper().IndexOf("ORDER BY") == -1)
            {
                whereClause = whereClause + " ORDER BY OrderId ";
            }

            IList<ApplicationSettingInfo> list = this.service.FindAll(whereClause, 0);

            outString.Append("{\"data\":[");

            if (!string.IsNullOrEmpty(emptyItemText))
            {
                outString.Append("{\"text\":\"" + emptyItemText + "\",\"value\":\"\"}" + ",");
            }

            foreach (ApplicationSettingInfo item in list)
            {
                outString.Append("{\"text\":\"" + item.Text + "\",\"value\":\"" + item.Value + "\",\"selected\":\"" + ((selectedValue == item.Value) ? true : false) + "\"}" + ",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"combobox\":\"" + combobox + "\",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion
    }
}
