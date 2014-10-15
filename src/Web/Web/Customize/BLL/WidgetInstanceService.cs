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

    using X3Platform.Security;
    using X3Platform.Spring;

    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customize.IBLL;
    using X3Platform.Web.Customize.IDAL;
    using X3Platform.Web.Customize.Model;

    /// <summary>ҳ��</summary>
    [SecurityClass]
    public class WidgetInstanceService : SecurityObject, IWidgetInstanceService
    {
        private IWidgetInstanceProvider provider = null;

        private WebConfiguration configuration = null;

        #region ���캯��:WidgetInstanceService()
        /// <summary>���캯��</summary>
        public WidgetInstanceService()
        {
            configuration = WebConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IWidgetInstanceProvider>(typeof(IWidgetInstanceProvider));
        }
        #endregion

        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WidgetInstanceInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInstanceInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        ///<returns>WidgetInstanceInfo ʵ����ϸ��Ϣ</returns>
        public WidgetInstanceInfo Save(WidgetInstanceInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="ids">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">WidgetInstanceInfo Id��</param>
        ///<returns>����һ�� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        public WidgetInstanceInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInstanceInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInstanceInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInstanceInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        ///<summary>��ҳ����</summary>
        ///<param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        ///<param name="pageSize">ҳ����С</param>
        ///<param name="whereClause">WHERE ��ѯ����</param>
        ///<param name="orderBy">ORDER BY ��������</param>
        ///<param name="rowCount">��¼����</param>
        ///<returns>����һ�� WorkflowCollectorInfo �б�ʵ��</returns>
        public IList<WidgetInstanceInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out  rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>�����Ƿ��������صļ�¼</summary>
        ///<param name="id">��¼����Ϣ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:GetOptionHtml(string id)
        /// <summary>��ȡ���Ա༭��Html�ı�</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>Html�ı�</returns>
        public string GetOptionHtml(string id)
        {
            WidgetInstanceInfo param = this.FindOne(id);

            return CustomizeContext.Instance.WidgetService.GetOptionHtml(param.WidgetId);
        }
        #endregion

        #region 属性:SetPageAndWidget(WidgetInstanceInfo param, string authorizationObjectType, string authorizationObjectId, string pageName, string widgetName)
        ///<summary>����ʵ�����ڵ�ҳ���Ͳ�������</summary>
        ///<param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        ///<param name="authorizationObjectType">��Ȩ��������</param>
        ///<param name="authorizationObjectId">��Ȩ������ʶ</param>
        ///<param name="pageName">ҳ������</param>
        ///<param name="widgetName">��������</param>
        ///<returns>����ֵ</returns>
        public WidgetInstanceInfo SetPageAndWidget(WidgetInstanceInfo param, string authorizationObjectType, string authorizationObjectId, string pageName, string widgetName)
        {
            PageInfo page = CustomizeContext.Instance.PageService.FindOneByName(authorizationObjectType, authorizationObjectId, pageName);

            WidgetInfo widget = CustomizeContext.Instance.WidgetService.FindOneByName(widgetName);

            param.PageId = (page == null) ? string.Empty : page.Id;
            param.PageName = pageName;

            param.WidgetId = (widget == null) ? string.Empty : widget.Id;
            param.WidgetName = widgetName;

            return param;
        }
        #endregion

        #region 属性:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        ///<summary>ɾ��δ�󶨵Ĳ���ʵ��</summary>
        ///<param name="pageId">ҳ������</param>
        ///<param name="bindingWidgetInstanceIds">�󶨵Ĳ�����ʶ</param>
        ///<returns>����ֵ</returns>
        public int RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        {
            return this.provider.RemoveUnbound(pageId, bindingWidgetInstanceIds);
        }
        #endregion
    }
}
