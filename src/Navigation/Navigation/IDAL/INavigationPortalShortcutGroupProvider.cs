#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :INavigationPortalShortcutGroupProvider.cs
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
    [SpringObject("X3Platform.Navigation.IDAL.INavigationPortalShortcutGroupProvider")]
    public interface INavigationPortalShortcutGroupProvider
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

        #region 属性:Save(NavigationPortalShortcutGroupInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>��ϸ��Ϣ</returns>
        NavigationPortalShortcutGroupInfo Save(NavigationPortalShortcutGroupInfo param);
        #endregion

        #region 属性:Insert(NavigationPortalShortcutGroupInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>��ϸ��Ϣ</param>
        void Insert(NavigationPortalShortcutGroupInfo param);
        #endregion

        #region 属性:Update(NavigationPortalShortcutGroupInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>��ϸ��Ϣ</param>
        void Update(NavigationPortalShortcutGroupInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>����ϸ��Ϣ</returns>
        NavigationPortalShortcutGroupInfo FindOne(string id);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutGroupInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllByPortalId(string portalId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="portalId">�����Ż���ʶ</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutGroupInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutGroupInfo> FindAllByPortalId(string portalId);
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
        /// <returns>����һ���б�ʵ��<see cref="NavigationPortalShortcutGroupInfo"/></returns>
        IList<NavigationPortalShortcutGroupInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
