namespace X3Platform.Apps.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    using X3Platform.Apps.Configuration;
    #endregion

    /// <summary></summary>
    public class ApplicationMenuWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IApplicationMenuService service = AppsContext.Instance.ApplicationMenuService;

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
            ApplicationMenuInfo param = new ApplicationMenuInfo();

            string authorizationReadScopeObjectText = XmlHelper.Fetch("authorizationReadScopeObjectText", doc);

            param = (ApplicationMenuInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            this.service.BindAuthorizationScopeObjects(param.Id, "应用_通用_查看权限", authorizationReadScopeObjectText);

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

            ApplicationMenuInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationMenuInfo>(param) + ",");

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

            IList<ApplicationMenuInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationMenuInfo>(list) + ",");

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

            IList<ApplicationMenuInfo> list = this.service.GetPaging(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ApplicationMenuInfo>(list) + ",");

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
            // string applicationId#fff#menuId#000000000000000;

            string url = XmlHelper.Fetch("url", doc);

            // 树形控件默认根节点标识为0, 需要特殊处理.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"data\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            // 特殊类型
            if (parentId == "menu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000")
            {
                if (AppsConfigurationView.Instance.HiddenStartMenu == "ON")
                {
                    outString.Append("{");
                    outString.Append("\"id\":\"startMenu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000\",");
                    outString.Append("\"parentId\":\"0\",");
                    outString.Append("\"name\":\"开始菜单\",");
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", "startMenu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000").Replace("{treeNodeName}", "开始菜单")) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                }

                if (AppsConfigurationView.Instance.HiddenTopMenu == "ON")
                {
                    outString.Append("{");
                    outString.Append("\"id\":\"topMenu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000\",");
                    outString.Append("\"parentId\":\"0\",");
                    outString.Append("\"name\":\"顶部菜单\",");
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", "topMenu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000").Replace("{treeNodeName}", "顶部菜单")) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                }

                if (AppsConfigurationView.Instance.HiddenShortcutMenu == "ON")
                {
                    outString.Append("{");
                    outString.Append("\"id\":\"shortcutMenu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000\",");
                    outString.Append("\"parentId\":\"0\",");
                    outString.Append("\"name\":\"快捷菜单\",");
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", "shortcutMenu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000").Replace("{treeNodeName}", "快捷菜单")) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                }
            }

            string whereClause = null;

            string[] keys = parentId.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            keys[0] = (keys[0] == "menu") ? "ApplicationMenu" : StringHelper.ToFirstUpper(keys[0]);

            //
            // ApplicationMenu
            // 
            if (keys[0] == "ApplicationMenu" && keys[4] == "00000000-0000-0000-0000-000000000000")
            {
                // 应用系统
                whereClause = string.Format(" ParentId = ##{0}## AND Status = 1 ORDER BY OrderId, Code ", keys[2]);

                AppsContext.Instance.ApplicationService.FindAll(whereClause).ToList().ForEach(item =>
                {
                    outString.Append("{");
                    outString.Append("\"id\":\"" + string.Format("applicationMenu#applicationId#{0}#menuId#00000000-0000-0000-0000-000000000000", item.Id) + "\",");
                    outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == "00000000-0000-0000-0000-000000000001" ? "0" : string.Format("applicationMenu#applicationId#{0}#menuId#00000000-0000-0000-0000-000000000000", item.ParentId)) + "\",");
                    outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.ApplicationDisplayName) + "\",");
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", string.Format("applicationMenu#applicationId#{0}#menuId#00000000-0000-0000-0000-000000000000", item.Id)).Replace("{treeNodeName}", item.ApplicationDisplayName)) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                });
            }

            if (parentId != "menu#applicationId#00000000-0000-0000-0000-000000000001#menuId#00000000-0000-0000-0000-000000000000")
            {
                // 菜单项
                if (keys[4] == "00000000-0000-0000-0000-000000000000")
                {
                    whereClause = string.Format(" MenuType = ##{0}## AND ApplicationId = ##{1}## AND ParentId = ##{2}## AND Status = 1 ORDER BY OrderId ", keys[0], keys[2], keys[4]);
                }
                else
                {
                    whereClause = string.Format(" MenuType = ##{0}## AND ParentId = ##{1}## AND Status = 1 ORDER BY OrderId ", keys[0], keys[4]);
                }

                AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
                {
                    if (item.DisplayType == "MenuSplitLine")
                    {
                        // 分割线不显示。
                    }
                    else
                    {
                        outString.Append("{");
                        outString.Append("\"id\":\"" + string.Format("{0}#applicationId#{1}#menuId#{2}", StringHelper.ToFirstLower(item.MenuType), item.ApplicationId, item.Id) + "\",");
                        outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson((item.MenuType == "ApplicationMenu" && item.ApplicationId == "00000000-0000-0000-0000-000000000001" && item.ParentId == "00000000-0000-0000-0000-000000000000") ? "0" : string.Format("{0}#applicationId#{1}#menuId#{2}", StringHelper.ToFirstLower(item.MenuType), item.ApplicationId, item.ParentId)) + "\",");
                        outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                        outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", string.Format("{0}#applicationId#{1}#menuId#{2}", StringHelper.ToFirstLower(item.MenuType), item.ApplicationId, item.Id)).Replace("{treeNodeName}", item.Name)) + "\",");
                        outString.Append("\"target\":\"_self\"");
                        outString.Append("},");
                    }
                });
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
