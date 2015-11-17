namespace X3Platform.Web.Customizes.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using X3Platform.Data;
    using X3Platform.Membership;
    using X3Platform.Membership.Model;
    using X3Platform.Security;
    using X3Platform.Spring;
    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customizes.IBLL;
    using X3Platform.Web.Customizes.IDAL;
    using X3Platform.Web.Customizes.Model;

    /// <summary>页面</summary>
    [SecurityClass]
    public class CustomizePageService : SecurityObject, ICustomizePageService
    {
        private ICustomizePageProvider provider = null;

        #region 构造函数:CustomizePageService()
        /// <summary>构造函数</summary>
        public CustomizePageService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ICustomizePageProvider>(typeof(ICustomizePageProvider));
        }
        #endregion

        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CustomizePageInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizePageInfo param, IAccountInfo account, string effectScope)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizePageInfo 实例详细信息</param>
        /// <returns>CustomizePageInfo 实例详细信息</returns>
        public CustomizePageInfo Save(CustomizePageInfo param, IAccountInfo account, string effectScope)
        {
            switch (effectScope)
            {
                case "Corporation":
                    param.AuthorizationObjectType = "Organization";
                    param.AuthorizationObjectId = MembershipManagement.Instance.MemberService[account.Id].CorporationId;
                    break;
                case "Department":
                    param.AuthorizationObjectType = "Organization";
                    param.AuthorizationObjectId = MembershipManagement.Instance.MemberService[account.Id].DepartmentId;
                    break;
                case "Department2":
                    param.AuthorizationObjectType = "Organization";
                    param.AuthorizationObjectId = MembershipManagement.Instance.MemberService[account.Id].Department2Id;
                    break;
                case "Organization":
                    param.AuthorizationObjectType = "Organization";
                    param.AuthorizationObjectId = MembershipManagement.Instance.MemberService[account.Id].OrganizationUnitId;
                    break;
                default:
                    param.AuthorizationObjectType = "Account";
                    param.AuthorizationObjectId = account.Id;
                    break;
            }

            return this.Save(param);
        }
        #endregion

        #region 函数:Save(CustomizePageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizePageInfo 实例详细信息</param>
        /// <returns>CustomizePageInfo 实例详细信息</returns>
        public CustomizePageInfo Save(CustomizePageInfo param)
        {
            this.provider.Save(param);

            // 当用户编辑页面，但是没有保存页面，在下一次保存页面内容时需要检查正确的部件实例，然后删除多余的部件。
            // <ul id="column1" class="x-ui-pkg-customize-column" ><li id="aa9eef91-ae84-8025-2d1b-73baf86d0ce5" class="x-ui-pkg-customize-widget" widget="news" ><div class="x-ui-pkg-customize-widget-head"><h3>重庆新闻</h3></div><div class="x-ui-pkg-customize-widget-content" ></div></li><li id="cfa9fa79-2466-1f00-bd35-373573916537" class="x-ui-pkg-customize-widget" widget="weather" ><div class="x-ui-pkg-customize-widget-head"><h3>天气预报</h3></div><div class="x-ui-pkg-customize-widget-content" ></div></li></ul><ul id="column2" class="x-ui-pkg-customize-column" ><li id="a79651a7-7f22-adaf-8d11-101631cf987b" class="x-ui-pkg-customize-widget" widget="news" ><div class="x-ui-pkg-customize-widget-head"><h3>北京新闻</h3></div><div class="x-ui-pkg-customize-widget-content" ></div></li></ul><ul id="column3" class="x-ui-pkg-customize-column" ><li id="f69dc940-7bc2-8a42-cb8f-6eabefec266d" class="x-ui-pkg-customize-widget" widget="tasks" ><div class="x-ui-pkg-customize-widget-head"><h3>123</h3></div><div class="x-ui-pkg-customize-widget-content" ></div></li></ul>

            // 由于每次保存页面是根据授权对象的标识和类型来判断页面唯一性，所以页面标识可能会被修改，
            // 所以获取数据库中的自定义页面标识需要用以下方法。
            param = this.FindOneByName(param.AuthorizationObjectType, param.AuthorizationObjectId, param.Name);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(string.Format("<root>{0}</root>", param.Html));

            XmlNodeList nodes = doc.GetElementsByTagName("li");

            // XmlNodeList nodes = doc.SelectNodes("ajaxStorage/ul/li");

            string bindingWidgetInstanceIds = string.Empty;

            foreach (XmlNode node in nodes)
            {
                if (((XmlElement)node).GetAttribute("class").IndexOf("x-ui-pkg-customize-widget") > -1)
                {
                    bindingWidgetInstanceIds += node.Attributes["id"].Value + ",";
                }
            }

            bindingWidgetInstanceIds = bindingWidgetInstanceIds.TrimEnd(',');

            CustomizeContext.Instance.CustomizeWidgetInstanceService.RemoveUnbound(param.Id, bindingWidgetInstanceIds);

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">CustomizePageInfo Id号</param>
        /// <returns>返回一个 CustomizePageInfo 实例的详细信息</returns>
        public CustomizePageInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询某条记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
        public CustomizePageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            return this.provider.FindOneByName(authorizationObjectType, authorizationObjectId, name);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
        public IList<CustomizePageInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
        public IList<CustomizePageInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
        public IList<CustomizePageInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个 WorkflowCollectorInfo 列表实例</returns>
        public IList<CustomizePageInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out  rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">登录名信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            return this.provider.IsExistName(authorizationObjectType, authorizationObjectId, name);
        }
        #endregion

        #region 函数:GetHtml(string name)
        /// <summary>获取Html文本</summary>
        /// <param name="name">页面名称</param>
        /// <returns>Html文本</returns>
        public string GetHtml(string name)
        {
            string xml = this.provider.GetHtml(name);

            if (string.IsNullOrEmpty(xml)) { return string.Empty; }

            return RenderHtml(xml);
        }
        #endregion

        #region 函数:GetHtml(string name, string authorizationObjectType, string authorizationObjectId)
        /// <summary>获取Html文本</summary>
        /// <param name="name">页面名称</param>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
        public string GetHtml(string name, string authorizationObjectType, string authorizationObjectId)
        {
            string xml = this.provider.GetHtml(name, authorizationObjectType, authorizationObjectId);

            if (string.IsNullOrEmpty(xml))
            {
                xml = CustomizeContext.Instance.CustomizeLayoutService.GetHtml(name);

                CustomizePageInfo param = new CustomizePageInfo();

                param.AuthorizationObjectType = authorizationObjectType;
                param.AuthorizationObjectId = authorizationObjectId;

                param.Name = name;
                param.Title = name;
                param.Html = xml;

                this.provider.Save(param);

                return string.Empty;
            }
            else
            {
                return RenderHtml(xml);
            }
        }
        #endregion

        #region 私有函数:RenderHtml(string xml)
        private string RenderHtml(string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            XmlNodeList nodes = doc.GetElementsByTagName("li");

            XmlElement element = null;

            XmlNodeList childNodes = null;

            XmlElement childElement = null;

            CustomizeWidgetInstanceInfo widgetInstance = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                element = (XmlElement)nodes[i];

                if (element.GetAttribute("class").IndexOf("x-ui-pkg-customize-widget") > -1)
                {
                    widgetInstance = CustomizeContext.Instance.CustomizeWidgetInstanceService[element.GetAttribute("id")];

                    if (widgetInstance != null)
                    {
                        childNodes = element.ChildNodes;

                        foreach (XmlNode childNode in childNodes)
                        {
                            childElement = (XmlElement)childNode;

                            if (childElement.GetAttribute("class").IndexOf("x-ui-pkg-customize-widget-head") > -1)
                            {
                                if (widgetInstance.Widget != null && !string.IsNullOrEmpty(widgetInstance.Widget.Url))
                                {
                                    childElement.SetAttribute("style", "cursor:pointer;");
                                }
                            }

                            if (childElement.GetAttribute("class").IndexOf("x-ui-pkg-customize-widget-content") > -1)
                            {
                                childNode.InnerXml = widgetInstance.Html;
                            }
                        }
                    }
                }
            }

            return doc.InnerXml;
        }
        #endregion
    }
}
