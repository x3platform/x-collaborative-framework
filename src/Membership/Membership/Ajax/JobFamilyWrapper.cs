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
    using X3Platform.Globalization; using X3Platform.Messages;
    #endregion

    /// <summary></summary>
    public class JobFamilyWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IJobFamilyService service = MembershipManagement.Instance.JobFamilyService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            IJobFamilyInfo param = new JobFamilyInfo();

            param = (IJobFamilyInfo)AjaxUtil.Deserialize(param, doc);

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

            IJobFamilyInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobFamilyInfo>(param) + ",");

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

            IList<IJobFamilyInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobFamilyInfo>(list) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IJobFamilyInfo> list = this.service.GetPaging(pages.RowIndex, pages.PageSize, pages.Query, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobFamilyInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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

            JobFamilyInfo param = new JobFamilyInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.ModifiedDate = param.CreatedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IJobFamilyInfo>(param) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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

            IList<IJobFamilyInfo> list = this.service.FindAll(whereClause);

            outString.Append("{\"data\":[");

            if (!string.IsNullOrEmpty(emptyItemText))
            {
                outString.Append("{text:\"" + emptyItemText + "\",value:\"" + string.Empty + "\"},");
            }

            foreach (IJobFamilyInfo item in list)
            {
                outString.Append("{text:\"" + item.Name + "\",value:\"" + item.Id + "\"},");
            }

            outString.Append("],");

            outString.Append("\"combobox\":\"" + combobox + "\",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion
    }
}
