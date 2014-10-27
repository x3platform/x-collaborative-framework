namespace X3Platform.Apps.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;
    using System.Linq;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    using X3Platform.DigitalNumber;
    #endregion

    /// <summary></summary>
    public class ApplicationMethodWrapper : ContextWrapper
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
        [AjaxMethod("save")]
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

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            this.service.Delete(ids);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            ApplicationMethodInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationMethodInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<ApplicationMethodInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationMethodInfo>(list) + ",");

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
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<ApplicationMethodInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationMethodInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:IsExist(XmlDocument doc)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("isExist")]
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

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string applicationSettingGroupId = XmlHelper.Fetch("applicationSettingGroupId", doc);

            ApplicationMethodInfo param = new ApplicationMethodInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ApplicationId = applicationId;
            param.Type = "generic";
            param.Version = 1;

            param.Status = 1;

            param.CreateDate = param.UpdateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationMethodInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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
        [AjaxMethod("hi")]
        public string Hi(XmlDocument doc)
        {
            return "{\"message\":{\"returnCode\":0,\"value\":\"【时间 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】Hi()方法调用成功。\"}}";
        }
        #endregion

        #region 函数:Throw(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("throw")]
        public string Throw(XmlDocument doc)
        {
            throw new Exception("这是一个测试抛出异常的方法。");
        }
        #endregion
    }
}
