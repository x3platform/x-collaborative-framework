// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Web.Customize.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using X3Platform.Membership;
    using X3Platform.Membership.Model;
    using X3Platform.Security;
    using X3Platform.Spring;

    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customize.IBLL;
    using X3Platform.Web.Customize.IDAL;
    using X3Platform.Web.Customize.Model;

    /// <summary>ҳ��</summary>
    [SecurityClass]
    public class PageService : SecurityObject, IPageService
    {
        private IPageProvider provider = null;

        private WebConfiguration configuration = null;

        #region ���캯��:PageService()
        /// <summary>���캯��</summary>
        public PageService()
        {
            configuration = WebConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IPageProvider>(typeof(IPageProvider));
        }
        #endregion

        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PageInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(PageInfo param, IAccountInfo account, string effectScope)
        /// <summary>������¼</summary>
        /// <param name="param">PageInfo ʵ����ϸ��Ϣ</param>
        /// <returns>PageInfo ʵ����ϸ��Ϣ</returns>
        public PageInfo Save(PageInfo param, IAccountInfo account, string effectScope)
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
                    param.AuthorizationObjectId = MembershipManagement.Instance.MemberService[account.Id].OrganizationId;
                    break;
                default:
                    param.AuthorizationObjectType = "Account";
                    param.AuthorizationObjectId = account.Id;
                    break;
            }

            return this.Save(param);
        }
        #endregion

        #region 属性:Save(PageInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">PageInfo ʵ����ϸ��Ϣ</param>
        /// <returns>PageInfo ʵ����ϸ��Ϣ</returns>
        public PageInfo Save(PageInfo param)
        {
            this.provider.Save(param);

            // ���û��༭ҳ�棬����û�б���ҳ�棬����һ�α���ҳ������ʱ��Ҫ������ȷ�Ĳ���ʵ����Ȼ��ɾ�������Ĳ�����
            // <ul id="column1" class="ajax-customize-column" ><li id="aa9eef91-ae84-8025-2d1b-73baf86d0ce5" class="ajax-customize-widget" widget="news" ><div class="ajax-customize-widget-head"><h3>��������</h3></div><div class="ajax-customize-widget-content" ></div></li><li id="cfa9fa79-2466-1f00-bd35-373573916537" class="ajax-customize-widget" widget="weather" ><div class="ajax-customize-widget-head"><h3>����Ԥ��</h3></div><div class="ajax-customize-widget-content" ></div></li></ul><ul id="column2" class="ajax-customize-column" ><li id="a79651a7-7f22-adaf-8d11-101631cf987b" class="ajax-customize-widget" widget="news" ><div class="ajax-customize-widget-head"><h3>��������</h3></div><div class="ajax-customize-widget-content" ></div></li></ul><ul id="column3" class="ajax-customize-column" ><li id="f69dc940-7bc2-8a42-cb8f-6eabefec266d" class="ajax-customize-widget" widget="tasks" ><div class="ajax-customize-widget-head"><h3>123</h3></div><div class="ajax-customize-widget-content" ></div></li></ul>

            // ����ÿ�α���ҳ���Ǹ�����Ȩ�����ı�ʶ���������ж�ҳ��Ψһ�ԣ�����ҳ����ʶ���ܻᱻ�޸ģ�
            // ���Ի�ȡ���ݿ��е��Զ���ҳ����ʶ��Ҫ�����·�����
            param = this.FindOneByName(param.AuthorizationObjectType, param.AuthorizationObjectId, param.Name);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(string.Format("<root>{0}</root>", param.Html));

            XmlNodeList nodes = doc.SelectNodes("ajaxStorage/ul/li");

            string bindingWidgetInstanceIds = string.Empty;

            foreach (XmlNode node in nodes)
            {
                bindingWidgetInstanceIds += node.Attributes["id"].Value + ",";
            }

            bindingWidgetInstanceIds = bindingWidgetInstanceIds.TrimEnd(',');

            CustomizeContext.Instance.WidgetInstanceService.RemoveUnbound(param.Id, bindingWidgetInstanceIds);

            return param;
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">PageInfo Id��</param>
        /// <returns>����һ�� PageInfo ʵ������ϸ��Ϣ</returns>
        public PageInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ��ʵ��<see cref="PageInfo"/>����ϸ��Ϣ</returns>
        public PageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            return this.provider.FindOneByName(authorizationObjectType, authorizationObjectId, name);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� PageInfo ʵ������ϸ��Ϣ</returns>
        public IList<PageInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� PageInfo ʵ������ϸ��Ϣ</returns>
        public IList<PageInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� PageInfo ʵ������ϸ��Ϣ</returns>
        public IList<PageInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ�� WorkflowCollectorInfo �б�ʵ��</returns>
        public IList<PageInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out  rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��¼����Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            return this.provider.IsExistName(authorizationObjectType, authorizationObjectId, name);
        }
        #endregion

        #region 属性:TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ��ʵ��<see cref="PageInfo"/>����ϸ��Ϣ</returns>
        public string TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        {
            string xml = this.provider.TryParseHtml(authorizationObjectType, authorizationObjectId, name);

            if (string.IsNullOrEmpty(xml))
            {
                xml = "<ul id=\"column1\" class=\"ajax-customize-column\" ></ul><ul id=\"column2\" class=\"ajax-customize-column\" ></ul><ul id=\"column3\" class=\"ajax-customize-column\" ></ul>";

                // <div class="ajax-customize-wrapper">
                // <div class="ajax-customize-design-template">
                // <ul id="column1" class="ajax-customize-column">
                //   <li id="a135b93e-e31d-d65a-397d-91d11cc70e13" class="ajax-customize-widget" widget="weather">
                //   </li></ul>
                //  <ul id="column2" class="ajax-customize-column">
                //    <li id="b9821c7e-d77c-0000-0154-646d2caa6868" class="ajax-customize-widget" widget="bug">
                // <div class="ajax-customize-widget-head" style="cursor:pointer;" onclick="x.customize.widget.redirct('/bugzilla/default.aspx');"><h3>��������</h3></div><div class="ajax-customize-widget-content"><div id="17a3b7de-eb92-40e2-84a4-f68fb4925f26" class="ajax-customize-widget-whitebox"><div class="ajax-tabs-view-wrapper"><div class="ajax-tabs-view-links"><a href="/bugzilla/default.aspx">����</a></div><div class="ajax-tabs-view-item-over"><strong>δ����</strong></div><div class="ajax-tabs-view-item"><a href="/bugzilla/default.aspx">ȫ��</a></div></div><div style="padding:0 10px 10px 10px;"><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000027.aspx" target="_blank">C_06 ���򲦿</a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000026.aspx" target="_blank">B_12 �ճ��䶯������</a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000023.aspx" target="_blank">B_10 ��Ŀ�޸� </a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000022.aspx" target="_blank">B_09 Ԥ������,Ԥ��׷��(��)</a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000025.aspx" target="_blank">A_04 ���ҹ���</a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000020.aspx" target="_blank">D_01 ��λ��ˮ�˱���</a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000021.aspx" target="_blank">B_08 �ճ��䶯������ </a></div><div style="padding:10px 0 0 0 ;"><a href="/bugzilla/archive/2010051900000024.aspx" target="_blank">A_03 ͳ�ƿھ�����</a></div></div></div></div></li></ul>
                // <ul id="column3" class="ajax-customize-column">
                //   <li id="dc2414aa-8700-cd0a-dbde-68af13e78290" class="ajax-customize-widget" widget="tasks"><div class="ajax-customize-widget-head" style="cursor:pointer;" onclick="x.customize.widget.redirct('/tasks/default.aspx');"><h3>����</h3></div><div class="ajax-customize-widget-content">
                // <div id="af6f651a-23e1-44fd-be79-c2f4cfc7066d" class="ajax-customize-widget-whitebox">
                // <div style="background-color:#ffffe0;border-bottom:1px solid #ccc;padding:6px 10px 4px 10px;"><div class="float-right"><a href="/tasks/default.aspx">ȫ����Ϣ</a></div><div>����<strong>29</strong>��δ���ɵ���Ϣ</div></div><div style="padding:0px 10px 10px 10px; border:0px solid #ccc;"><div style="padding:6px 0px 0px 0px;"><a href="/tasks/archive/0c44c315-72e5-43e7-bc2d-111b30546be9.aspx" target="_blank">��������-����4ab0a77e-1f2c-4...</a></div><div style="padding:6px 0px 0px 0px;"><a href="/tasks/archive/0da5dbec-8960-4275-8697-78fb75e7b684.aspx" target="_blank">��������-����ef1ae016-3ae4-4...</a></div><div style="padding:6px 0px 0px 0px;"><a href="/tasks/archive/1.aspx" target="_blank">��������1</a></div><div style="padding:6px 0px 0px 0px;"><a href="/tasks/archive/10.aspx" target="_blank">��������1</a></div><div style="padding:6px 0px 0px 0px;"><a href="/tasks/archive/1a28f295-a7eb-4438-b2ab-02ed2f588e15.aspx" target="_blank">��������-����3410e03d-a242-4...</a></div></div></div></div>
                // </li></ul></div>

                // </div>

                PageInfo param = new PageInfo();

                param.AuthorizationObjectType = authorizationObjectType;
                param.AuthorizationObjectId = authorizationObjectId;
                param.AuthorizationObjectName = MembershipManagement.Instance.AuthorizationObjectService.FindOne(authorizationObjectType, authorizationObjectId).Name;

                param.Name = name;
                param.Title = name;
                param.Html = xml;

                this.provider.Save(param);
            }

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(string.Format("<div class=\"ajax-customize-design-template\">{0}</div>", xml));

            XmlNodeList nodes = doc.GetElementsByTagName("li");

            XmlElement element = null;

            XmlNodeList childNodes = null;

            XmlElement childElement = null;

            WidgetInstanceInfo widgetInstance = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                element = (XmlElement)nodes[i];

                if (element.GetAttribute("class") == "ajax-customize-widget")
                {
                    widgetInstance = CustomizeContext.Instance.WidgetInstanceService[element.GetAttribute("id")];

                    if (widgetInstance != null)
                    {
                        childNodes = element.ChildNodes;

                        foreach (XmlNode childNode in childNodes)
                        {
                            childElement = (XmlElement)childNode;

                            if (childElement.GetAttribute("class") == "ajax-customize-widget-head")
                            {
                                if (widgetInstance.Widget != null && !string.IsNullOrEmpty(widgetInstance.Widget.Url))
                                {
                                    childElement.SetAttribute("style", "cursor:pointer;");
                                    // childElement.SetAttribute("onclick", "x.customize.widget.redirct('" + widgetInstance.Widget.RedirctUrl + "');");
                                }
                            }

                            if (childElement.GetAttribute("class") == "ajax-customize-widget-content")
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
