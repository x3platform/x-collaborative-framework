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
    [SpringObject("X3Platform.Web.Customize.IBLL.IWidgetService")]
    public interface IWidgetService
    {
        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        WidgetInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">WidgetInfo ʵ����ϸ��Ϣ</param>
        ///<returns>WidgetInfo ʵ����ϸ��Ϣ</returns>
        WidgetInfo Save(WidgetInfo param);
        #endregion

        #region 属性:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="keys">��ʶ,�����Զ��Ÿ���</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">WidgetInfo Id��</param>
        ///<returns>����һ�� WidgetInfo ʵ������ϸ��Ϣ</returns>
        WidgetInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByName(string name)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="name">ҳ������</param>
        ///<returns>����һ�� WidgetInfo ʵ������ϸ��Ϣ</returns>
        WidgetInfo FindOneByName(string name);
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>�������� WidgetInfo ʵ������ϸ��Ϣ</returns>
        IList<WidgetInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>�������� WidgetInfo ʵ������ϸ��Ϣ</returns>
        IList<WidgetInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� WidgetInfo ʵ������ϸ��Ϣ</returns>
        IList<WidgetInfo> FindAll(string whereClause, int length);
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
        IList<WidgetInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="id">WidgetInfo ʵ����ϸ��Ϣ</param>
        ///<returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistName(string name)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="name">ҳ������</param>
        ///<returns>����ֵ</returns>
        bool IsExistName(string name);
        #endregion

        #region 属性:GetOptionHtml(string id)
        /// <summary>��ȡ���Ա༭��Html�ı�</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>Html�ı�</returns>
        string GetOptionHtml(string id);
        #endregion 
    }
}
