#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :INavigationPortalSidebarItemService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Navigation.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Navigation.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Navigation.IBLL.INavigationPortalSidebarItemService")]
    public interface INavigationPortalSidebarItemService
    {
        #region ����:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NavigationPortalSidebarItemInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(NavigationPortalSidebarItemInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalSidebarItemInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="NavigationPortalSidebarItemInfo"/>��ϸ��Ϣ</returns>
        NavigationPortalSidebarItemInfo Save(NavigationPortalSidebarItemInfo param);
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
        /// <returns>����ʵ��<see cref="NavigationPortalSidebarItemInfo"/>����ϸ��Ϣ</returns>
        NavigationPortalSidebarItemInfo FindOne(string id);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalSidebarItemInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalSidebarItemInfo> FindAll(string whereClause);
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalSidebarItemInfo> FindAll(string whereClause, int length);
        #endregion

        #region ����:FindAllByPortalId(string portalId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="portalId">�����Ż���ʶ</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalSidebarItemInfo> FindAllByPortalId(string portalId);
        #endregion

        #region ����:FindAllBySidebarItemGroupId(string sidebarItemGroupId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="sidebarItemGroupId">���������˵���������ʶ</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalSidebarItemInfo> FindAllBySidebarItemGroupId(string sidebarItemGroupId);
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
        /// <returns>����һ���б�ʵ��<see cref="NavigationPortalSidebarItemInfo"/></returns>
        IList<NavigationPortalSidebarItemInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
