namespace X3Platform.Tasks.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.Model;
    using X3Platform.Util;
    using X3Platform.DigitalNumber;
    #endregion

    /// <summary></summary>
    public class TaskCategoryWrapper : ContextWrapper
    {
        private ITaskCategoryService service = TasksContext.Instance.TaskCategoryService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            TaskCategoryInfo param = new TaskCategoryInfo();

            param = (TaskCategoryInfo)AjaxUtil.Deserialize(param, doc);

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

            if (this.service.CanDelete(id))
            {
                this.service.Delete(id);

                return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
            }
            else
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"此类别下还有相关业务数据，不能被删除。\"}}";
            }
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

            TaskCategoryInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAll(XmlDocument doc)
        {
            IList<TaskCategoryInfo> list = this.service.FindAll();

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(list) + ",");

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
            if (XmlHelper.Fetch("su", doc) == "1")
            {
                paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            // 设置特定的业务场景
            if (paging.Query.Where.ContainsKey("scence"))
            {
                // 场景
                // Query 根据关键字查询
                // QueryByOrganizationId 根据组织标识查询
                if (paging.Query.Where["scence"].ToString() == "Query" || paging.Query.Where["scence"].ToString() == "QueryByOrganizationId")
                {
                    paging.Query.Variables["scence"] = paging.Query.Where["scence"].ToString();
                }

                paging.Query.Where.Remove("scence");
            }

            int rowCount = -1;

            IList<TaskCategoryInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(list) + ",");
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

            TaskCategoryInfo param = new TaskCategoryInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<TaskCategoryInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SetStatus(XmlDocument doc)
        /// <summary>设置类别状态(停用/启用)</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SetStatus(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            int status = Convert.ToInt32(XmlHelper.Fetch("status", doc));

            if (this.service.SetStatus(id, status))
            {
                return "{message:{\"returnCode\":0,\"value\":\"操作成功。\"}}";
            }
            else
            {
                return "{message:{\"returnCode\":1,\"value\":\"操作失败。\"}}";
            }
        }
        #endregion
    }
}
