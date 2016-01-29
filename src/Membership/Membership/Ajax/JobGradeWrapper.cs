namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    using X3Platform.Globalization;
    #endregion

    /// <summary></summary>
    public class JobGradeWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IJobGradeService service = MembershipManagement.Instance.JobGradeService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            IJobGradeInfo param = new JobGradeInfo();

            param = (IJobGradeInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            return GenericException.Serialize(0, I18n.Strings["msg_save_success"]);
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

            return GenericException.Serialize(0, I18n.Strings["msg_delete_success"]);
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

            IJobGradeInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobGradeInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

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

            IList<IJobGradeInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobGradeInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

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

            IList<IJobGradeInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobGradeInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            // 兼容 ExtJS 设置
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
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            JobGradeInfo param = new JobGradeInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.ModifiedDate = param.CreatedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobGradeInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion    
        
        #region 函数:GetCombobox(XmlDocument doc)
        /// <summary>查询类别数据以供形成类别下拉框数据源</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("getCombobox")]
        public string GetCombobox(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string combobox = XmlHelper.Fetch("combobox", doc);

            string selectedValue = XmlHelper.Fetch("selectedValue", doc);

            string emptyItemText = XmlHelper.Fetch("emptyItemText", doc);

            string whereClause = " Status = 1 ORDER BY OrderId ";

            IList<IJobGradeInfo> list = this.service.FindAll(whereClause);

            outString.Append("{\"data\":[");

            if (!string.IsNullOrEmpty(emptyItemText))
            {
                outString.Append("{text:\"" + emptyItemText + "\",value:\"" + string.Empty + "\"},");
            }

            foreach (IJobGradeInfo item in list)
            {
                outString.Append("{text:\"" + item.Name + "\",value:\"" + item.Id + "\"},");
            }

            outString.Append("],");

            outString.Append("\"combobox\":\"" + combobox + "\",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion
    }
}
