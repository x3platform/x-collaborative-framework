#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :INavigationPortalShortcutService.cs
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
    [SpringObject("X3Platform.Navigation.IBLL.INavigationPortalShortcutService")]
    public interface INavigationPortalShortcutService
    {
        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NavigationPortalShortcutInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(NavigationPortalShortcutInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalShortcutInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="NavigationPortalShortcutInfo"/>��ϸ��Ϣ</returns>
        NavigationPortalShortcutInfo Save(NavigationPortalShortcutInfo param);
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
        /// <returns>����ʵ��<see cref="NavigationPortalShortcutInfo"/>����ϸ��Ϣ</returns>
        NavigationPortalShortcutInfo FindOne(string id);
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllByPortalId(string portalId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="portalId">�����Ż���ʶ</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutInfo> FindAllByPortalId(string portalId);
        #endregion

        #region 属性:FindAllByShortcutGroupId(string shortcutGroupId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="shortcutGroupId">�������ݷ�ʽ������ʶ</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalShortcutInfo"/>����ϸ��Ϣ</returns>
        IList<NavigationPortalShortcutInfo> FindAllByShortcutGroupId(string shortcutGroupId);
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
        /// <returns>����һ���б�ʵ��<see cref="NavigationPortalShortcutInfo"/></returns>
        IList<NavigationPortalShortcutInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
