#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :INavigationPortalTopMenuProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Navigation.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Navigation.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Navigation.IDAL.INavigationPortalTopMenuProvider")]
    public interface INavigationPortalTopMenuProvider
    {
        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region ����:BeginTransaction()
        /// <summary>��������</summary>
        void BeginTransaction();
        #endregion

        #region ����:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region ����:CommitTransaction()
        /// <summary>�ύ����</summary>
        void CommitTransaction();
        #endregion

        #region ����:RollBackTransaction()
        /// <summary>�ع�����</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(NavigationPortalTopMenuInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalTopMenuInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="NavigationPortalTopMenuInfo"/>��ϸ��Ϣ</returns>
        NavigationPortalTopMenuInfo Save(NavigationPortalTopMenuInfo param);
        #endregion

        #region ����:Insert(NavigationPortalTopMenuInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalTopMenuInfo"/>��ϸ��Ϣ</param>
        void Insert(NavigationPortalTopMenuInfo param);
        #endregion

        #region ����:Update(NavigationPortalTopMenuInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalTopMenuInfo"/>��ϸ��Ϣ</param>
        void Update(NavigationPortalTopMenuInfo param);
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="NavigationPortalTopMenuInfo"/>����ϸ��Ϣ</returns>
        NavigationPortalTopMenuInfo FindOne(string id);
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalTopMenuInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalTopMenuInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="NavigationPortalTopMenuInfo"/></returns>
        IList<NavigationPortalTopMenuInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
