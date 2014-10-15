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
    public class WidgetService : SecurityObject, IWidgetService
    {
        private IWidgetProvider provider = null;

        private WebConfiguration configuration = null;

        #region ���캯��:WidgetService()
        /// <summary>���캯��</summary>
        public WidgetService()
        {
            configuration = WebConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IWidgetProvider>(typeof(IWidgetProvider));
        }
        #endregion

        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WidgetInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">WidgetInfo ʵ����ϸ��Ϣ</param>
        ///<returns>WidgetInfo ʵ����ϸ��Ϣ</returns>
        public WidgetInfo Save(WidgetInfo param)
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
        ///<param name="id">WidgetInfo Id��</param>
        ///<returns>����һ�� WidgetInfo ʵ������ϸ��Ϣ</returns>
        public WidgetInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByName(string name)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="name">ҳ������</param>
        ///<returns>����һ�� WidgetInfo ʵ������ϸ��Ϣ</returns>
        public WidgetInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>�������� WidgetInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>�������� WidgetInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� WidgetInfo ʵ������ϸ��Ϣ</returns>
        public IList<WidgetInfo> FindAll(string whereClause, int length)
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
        public IList<WidgetInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out  rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>�����Ƿ��������صļ�¼</summary>
        ///<param name="id">������ʶ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string name)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="name">ҳ������</param>
        ///<returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 属性:GetOptionHtml(string id)
        /// <summary>��ȡ���Ա༭��Html�ı�</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>Html�ı�</returns>
        public string GetOptionHtml(string id)
        {
            return this.provider.GetOptionHtml(id);
        }
        #endregion
    }
}
