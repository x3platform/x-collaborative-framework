#region Copyright & Author
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
#endregion

namespace X3Platform.Web.Customize.IBLL
{
    #region Using Libraries
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Web.Customize.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customize.IBLL.IWidgetInstanceService")]
    public interface IWidgetInstanceService
    {
        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        WidgetInstanceInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInstanceInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <returns>WidgetInstanceInfo ʵ����ϸ��Ϣ</returns>
        WidgetInstanceInfo Save(WidgetInstanceInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="keys">��ʶ,�����Զ��Ÿ���</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">WidgetInstanceInfo Id��</param>
        /// <returns>����һ�� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        WidgetInstanceInfo FindOne(string id);
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        IList<WidgetInstanceInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        IList<WidgetInstanceInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� WidgetInstanceInfo ʵ������ϸ��Ϣ</returns>
        IList<WidgetInstanceInfo> FindAll(string whereClause, int length);
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
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<WidgetInstanceInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:GetOptionHtml(string id)
        /// <summary>��ȡ���Ա༭��Html�ı�</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>Html�ı�</returns>
        string GetOptionHtml(string id);
        #endregion

        #region 属性:SetPageAndWidget(WidgetInstanceInfo param, string authorizationObjectType, string authorizationObjectId, string pageName, string widgetName)
        /// <summary>����ʵ�����ڵ�ҳ���Ͳ�������</summary>
        /// <param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="pageName">ҳ������</param>
        /// <param name="widgetName">��������</param>
        /// <returns>����ֵ</returns>
        WidgetInstanceInfo SetPageAndWidget(WidgetInstanceInfo param, string authorizationObjectType, string authorizationObjectId, string pageName, string widgetName);
        #endregion

        #region 属性:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        /// <summary>ɾ��δ�󶨵Ĳ���ʵ��</summary>
        /// <param name="pageId">ҳ������</param>
        /// <param name="bindingWidgetInstanceIds">�󶨵Ĳ�����ʶ</param>
        /// <returns>����ֵ</returns>
        int RemoveUnbound(string pageId, string bindingWidgetInstanceIds);
        #endregion
    }
}
