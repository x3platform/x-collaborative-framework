//=============================================================================
//
// Copyright (c) 2011 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2011-01-01
//
//=============================================================================

namespace X3Platform.Web.Customize.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Messages;
    using X3Platform.Spring;
    using X3Platform.Web.Customize.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customize.IDAL.IWidgetProvider")]
    public interface IWidgetProvider
    {
        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region 属性:BeginTransaction()
        /// <summary>��������</summary>
        void BeginTransaction();
        #endregion

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 属性:CommitTransaction()
        /// <summary>�ύ����</summary>
        void CommitTransaction();
        #endregion

        #region 属性:RollBackTransaction()
        /// <summary>�ع�����</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">WidgetInfo ʵ����ϸ��Ϣ</param>
        ///<returns>WidgetInfo ʵ����ϸ��Ϣ</returns>
        WidgetInfo Save(WidgetInfo param);
        #endregion

        #region 属性:Insert(WidgetInfo param)
        ///<summary>���Ӽ�¼</summary>
        ///<param name="param">WidgetInfo ʵ������ϸ��Ϣ</param>
        void Insert(WidgetInfo param);
        #endregion

        #region 属性:Update(WidgetInfo param)
        ///<summary>�޸ļ�¼</summary>
        ///<param name="param">WidgetInfo ʵ������ϸ��Ϣ</param>
        void Update(WidgetInfo param);
        #endregion

        #region 属性:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="ids">��ʶ,�����Զ��Ÿ���</param>
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
        ///<param name="param">WidgetInfo ʵ����ϸ��Ϣ</param>
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
