#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :IForumCategoryProvider.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Plugins.Forum.Model;
    using X3Platform.CategoryIndexes;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Forum.IDAL.IForumCategoryProvider")]
    public interface IForumCategoryProvider
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
        /// <param name="isolationLevel">������뼶��</param>
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
        // ���� ��� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(ForumCategoryInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</returns>
        ForumCategoryInfo Save(ForumCategoryInfo param);
        #endregion

        #region ����:Insert(ForumCategoryInfo param)
        /// <summary>��Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
        void Insert(ForumCategoryInfo param);
        #endregion

        #region ����:Update(ForumCategoryInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
        void Update(ForumCategoryInfo param);
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
        ForumCategoryInfo FindOne(string id);
        #endregion

        #region ����:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">��������</param>
        /// <returns>����ʵ��<see cref="DengBaoCategoryInfo"/>����ϸ��Ϣ</returns>
        ForumCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
        IList<ForumCategoryInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="ForumCategoryInfo"/></returns>
        IList<ForumCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="ForumCategoryQueryInfo"/></returns>
        IList<ForumCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region ����:IsExistByParent(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExistByParent(string id);
        #endregion

        #region ����:FetchCategoryIndex(string whereClause)
        /// <summary>���ݷ���״̬��ȡ��������</summary>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="applicationTag">�����ʶ</param>
        /// <returns>������������</returns>
        ICategoryIndex FetchCategoryIndex(string whereClause);
        #endregion

        #region ����:IsCategoryAdministrator(string categoryId)
        /// <summary>
        /// �жϵ�ǰ�û��Ƿ��Ǹð��Ĺ���Ա
        /// <param name="categoryId">�����</param>
        /// <returns>�������</returns>
        bool IsCategoryAdministrator(string categoryId);
        #endregion

        #region ����:IsCategoryAuthority(string categoryId)
        /// <summary>
        /// ���ݰ���ʶ��ѯ��ǰ�û��Ƿ�ӵ�и�Ȩ��
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        bool IsCategoryAuthority(string categoryId);
        #endregion

        #region ����:GetCategoryAdminName(string categoryId)
        /// <summary>
        /// ��ѯ��������
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="applicationTag"></param>
        /// <returns></returns>
        string GetCategoryAdminName(string categoryId);
        #endregion

        // -------------------------------------------------------
        // ��Ȩ��Χ����
        // -------------------------------------------------------

        #region ����:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>�ж��û��Ƿ�ӵ��ʵ������Ȩ����Ϣ</summary>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns>����ֵ</returns>
        bool HasAuthority(string entityId, string authorityName, IAccountInfo account);
        #endregion

        #region ����:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>��ʵ������Ȩ����Ϣ</summary>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText);
        #endregion

        #region ����:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>��ѯʵ������Ȩ����Ϣ</summary>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName);
        #endregion
    }
}
