#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalSidebarItemGroupService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Navigation.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Navigation.Configuration;
    using X3Platform.Navigation.IBLL;
    using X3Platform.Navigation.IDAL;
    using X3Platform.Navigation.Model;
    #endregion

    /// <summary></summary>
    public class NavigationPortalSidebarItemGroupService : INavigationPortalSidebarItemGroupService
    {
        /// <summary>����</summary>
        private NavigationConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private INavigationPortalSidebarItemGroupProvider provider = null;

        #region ���캯��:NavigationPortalSidebarItemGroupService()
        /// <summary>���캯��</summary>
        public NavigationPortalSidebarItemGroupService()
        {
            this.configuration = NavigationConfigurationView.Instance.Configuration;

            this.provider = SpringContext.Instance.GetObject<INavigationPortalSidebarItemGroupProvider>(typeof(INavigationPortalSidebarItemGroupProvider));
        }
        #endregion

        #region ����:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NavigationPortalSidebarItemGroupInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(NavigationPortalSidebarItemGroupInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>��ϸ��Ϣ</returns>
        public NavigationPortalSidebarItemGroupInfo Save(NavigationPortalSidebarItemGroupInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>����ϸ��Ϣ</returns>
        public NavigationPortalSidebarItemGroupInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalSidebarItemGroupInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalSidebarItemGroupInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalSidebarItemGroupInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region ����:FindAllByPortalId(string portalId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="portalId">�����Ż���ʶ</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalSidebarItemGroupInfo> FindAllByPortalId(string portalId)
        {
            return this.provider.FindAllByPortalId(portalId);
        }
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
        /// <returns>����һ���б�ʵ��<see cref="NavigationPortalSidebarItemGroupInfo"/></returns>
        public IList<NavigationPortalSidebarItemGroupInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion
    }
}
