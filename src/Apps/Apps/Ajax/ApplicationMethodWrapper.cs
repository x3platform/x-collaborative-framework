namespace X3Platform.Apps.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;
    using System.Linq;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Globalization;
    using X3Platform.Messages;
    using X3Platform.Util;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationMethodWrapper
    {
        /// <summary>数据服务</summary>
        private IApplicationMethodService service = AppsContext.Instance.ApplicationMethodService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            ApplicationMethodInfo param = new ApplicationMethodInfo();

            string originalCode = XmlHelper.Fetch("originalCode", doc);

            string originalName = XmlHelper.Fetch("originalName", doc);

            param = (ApplicationMethodInfo)AjaxUtil.Deserialize(param, doc);

            if (originalCode != param.Code)
            {
                if (this.service.IsExistCode(param.Code))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"已存在相同的代码。\"}}";
                }
            }

            if (originalName != param.Name)
            {
                if (this.service.IsExistName(param.Name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"已存在相同的名称。\"}}";
                }
            }

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

            ApplicationMethodInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationMethodInfo>(param) + ",");

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

            int rowCount = -1;

            IList<ApplicationMethodInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationMethodInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
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

            return MessageObject.Stringify("0", result.ToString().ToLower());
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

            ApplicationMethodInfo param = new ApplicationMethodInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ApplicationId = applicationId;
            param.Type = "generic";
            param.Version = 1;

            param.Status = 1;

            param.CreatedDate = param.ModifiedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationMethodInfo>(param) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 默认测试函数
        // -------------------------------------------------------

        #region 函数:Hi(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string Hi(XmlDocument doc)
        {
            return MessageObject.Stringify("0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " hi.");
        }
        #endregion

        #region 函数:Throw(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string Throw(XmlDocument doc)
        {
            throw new Exception("这是一个测试抛出异常的方法。");
        }
        #endregion
    }
}
