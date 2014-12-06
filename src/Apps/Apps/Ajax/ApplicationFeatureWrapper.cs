namespace X3Platform.Apps.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationFeatureWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IApplicationFeatureService service = AppsContext.Instance.ApplicationFeatureService;

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
            ApplicationFeatureInfo param = new ApplicationFeatureInfo();

            string originalName = XmlHelper.Fetch("originalName", doc);

            param = (ApplicationFeatureInfo)AjaxUtil.Deserialize(param, doc);

            if (originalName != param.Name)
            {
                if (this.service.IsExistName(param.Name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ�����ͬ�����ơ�\"}}";
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

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
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

            ApplicationFeatureInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationFeatureInfo>(param) + ",");

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

            IList<ApplicationFeatureInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationFeatureInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByApplicationId(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findAllByApplicationId")]
        public string FindAllByApplicationId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            IList<ApplicationFeatureInfo> list = this.service.FindAllByApplicationId(applicationId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationFeatureInfo>(list) + ",");

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

            IList<ApplicationFeatureInfo> list = this.service.GetPaging(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationFeatureInfo>(list) + ",");

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

            ApplicationFeatureInfo param = new ApplicationFeatureInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ApplicationId = applicationId;

            param.ParentId = "00000000-0000-0000-0000-000000000000";

            param.Status = 1;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationFeatureInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
        public string GetDynamicTreeView(XmlDocument doc)
        {
            // 必填字段
            string tree = XmlHelper.Fetch("tree", doc);
            string parentId = XmlHelper.Fetch("parentId", doc);

            // 附加属性
            string treeViewId = XmlHelper.Fetch("treeViewId", doc);
            string treeViewName = XmlHelper.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

            string url = XmlHelper.Fetch("url", doc);

            // 树形控件默认根节点标识为0, 需要特殊处理.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"data\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            //查找树的子节点
            string whereClause = string.Format(" [ParentId] = ##{0}## AND [Type] = ##function## AND [Status] = 1 ORDER BY OrderId DESC ", parentId);

            if (treeViewRootTreeNodeId == parentId)
            {
                whereClause = string.Format(" [ApplicationId] = ##{0}## AND ParentId = ##00000000-0000-0000-0000-000000000000## AND [Type] = ##function## AND [Status] = 1 ORDER BY OrderId DESC ", treeViewRootTreeNodeId);
            }

            IList<ApplicationFeatureInfo> list = this.service.FindAll(whereClause);

            foreach (ApplicationFeatureInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson((item.ParentId == treeViewRootTreeNodeId || item.ParentId == "00000000-0000-0000-0000-000000000000") ? "0" : item.ParentId) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.DisplayName) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.DisplayName)) + "\",");
                outString.Append("\"target\":\"_self\"");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 应用功能授权
        // -------------------------------------------------------

        #region 函数:GetTreeTableView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getTreeTableView")]
        public string GetTreeTableView(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string authorizationObjectType = XmlHelper.Fetch("authorizationObjectType", doc);
            string authorizationObjectId = XmlHelper.Fetch("authorizationObjectId", doc);

            DataTable table = this.service.GetApplicationFeatureScope(applicationId, authorizationObjectType, authorizationObjectId);

            outString.Append("{\"data\":[");

            foreach (DataRow row in table.Rows)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + row["Id"] + "\",");
                outString.Append("\"applicationId\":\"" + row["ApplicationId"] + "\",");
                outString.Append("\"parentId\":\"" + row["ParentId"] + "\",");
                outString.Append("\"code\":\"" + row["Code"] + "\",");
                outString.Append("\"name\":\"" + row["Name"] + "\",");
                outString.Append("\"displayName\":\"" + row["DisplayName"] + "\",");
                outString.Append("\"type\":\"" + row["Type"] + "\",");
                outString.Append("\"authorizationObjectType\":\"" + row["AuthorizationObjectType"] + "\",");
                outString.Append("\"authorizationObjectId\":\"" + row["AuthorizationObjectId"] + "\",");
                outString.Append("\"authorized\":\"" + row["Authorized"] + "\" ");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"应用功能授权查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SetTreeTableView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("setTreeTableView")]
        public string SetTreeTableView(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            string authorizationObjectType = XmlHelper.Fetch("authorizationObjectType", doc);
            string authorizationObjectId = XmlHelper.Fetch("authorizationObjectId", doc);

            string applicationFeatureIds = XmlHelper.Fetch("applicationFeatureIds", doc);

            this.service.SetApplicationFeatureScope(applicationId, authorizationObjectType, authorizationObjectId, applicationFeatureIds);

            return "{\"message\":{\"returnCode\":0,\"value\":\"应用功能授权设置成功。\"}}";
        }
        #endregion
    }
}
