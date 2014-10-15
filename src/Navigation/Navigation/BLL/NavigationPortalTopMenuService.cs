#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalTopMenuService.cs
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
    public class NavigationPortalTopMenuService : INavigationPortalTopMenuService
    {
        /// <summary>����</summary>
        private NavigationConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private INavigationPortalTopMenuProvider provider = null;

        #region ���캯��:NavigationPortalTopMenuService()
        /// <summary>���캯��</summary>
        public NavigationPortalTopMenuService()
        {
            this.configuration = NavigationConfigurationView.Instance.Configuration;

            this.provider = SpringContext.Instance.GetObject<INavigationPortalTopMenuProvider>(typeof(INavigationPortalTopMenuProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NavigationPortalTopMenuInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(NavigationPortalTopMenuInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="NavigationPortalTopMenuInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="NavigationPortalTopMenuInfo"/>��ϸ��Ϣ</returns>
        public NavigationPortalTopMenuInfo Save(NavigationPortalTopMenuInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
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

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="NavigationPortalTopMenuInfo"/>����ϸ��Ϣ</returns>
        public NavigationPortalTopMenuInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="NavigationPortalTopMenuInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalTopMenuInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalTopMenuInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalTopMenuInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="NavigationPortalTopMenuInfo"/>����ϸ��Ϣ</returns>
        public IList<NavigationPortalTopMenuInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
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
        /// <returns>����һ���б�ʵ��<see cref="NavigationPortalTopMenuInfo"/></returns>
        public IList<NavigationPortalTopMenuInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
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
