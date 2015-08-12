#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Membership.Scope;
    using X3Platform.Membership;
    using X3Platform.Plugins.Bugs.Model;
    using X3Platform.Security.Authority;
    using X3Platform.Web.Component.Combobox;
    using X3Platform.CategoryIndexes;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IDAL.IBugCategoryProvider")]
    public interface IBugCategoryProvider
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

        #region ����:Save(BugCategoryInfo param)
        /// <summary>
        /// �����¼
        /// </summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns></returns>
        BugCategoryInfo Save(BugCategoryInfo param);
        #endregion

        #region ����:Insert(BugCategoryInfo param)
        /// <summary>
        /// ��Ӽ�¼
        /// </summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        void Insert(BugCategoryInfo param);
        #endregion

        #region ����:Update(BugCategoryInfo param)
        /// <summary>
        /// �޸ļ�¼
        /// </summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        void Update(BugCategoryInfo param);
        #endregion

        #region ����:CanDelete(string id)
        /// <summary>�ж�����Ƿ��ܹ�������ɾ��</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        /// <returns></returns>
        bool CanDelete(string id);
        #endregion

        #region ����:Delete(string id)
        /// <summary>�߼�ɾ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        int Delete(string id);
        #endregion

        #region ����:Remove(string id)
        /// <summary>����ɾ����¼</summary>
        /// <param name="id">ʵ��������ʶ</param>
        int Remove(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>
        /// ��ѯĳ����¼
        /// </summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ������ϸ��Ϣ</returns>
        BugCategoryInfo FindOne(string id);
        #endregion

        #region ����:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">�������</param>
        /// <returns>����ʵ��<see cref="BugCategoryInfo"/>����ϸ��Ϣ</returns>
        BugCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>
        /// ��ѯ������ؼ�¼
        /// </summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        IList<BugCategoryInfo> FindAll(string whereClause, int length);
        #endregion

        #region ����:FindAllQueryObject(string whereClause, int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="BugCategoryQueryInfo"/>����ϸ��Ϣ</returns>
        IList<BugCategoryQueryInfo> FindAllQueryObject(string whereClause, int length);
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
        /// <returns></returns>
        IList<BugCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="BugCategoryQueryInfo"/></returns>
        IList<BugCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region ����:SetStatus(string id, int status)
        /// <summary>�������״̬(ͣ��/����)</summary>
        /// <param name="id">��������ʶ</param>
        /// <param name="status">1 ��ͣ�õ�������ã�0 �����õ����ͣ��</param>
        /// <returns></returns>
        bool SetStatus(string id, int status);
        #endregion

        #region ����:GetComboboxByWhereClause(string whereClause, string selectedValue)
        /// <summary>��ȡ��������Դ</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="selectedValue">Ĭ��ѡ���ֵ</param>
        /// <returns></returns>
        IList<ComboboxItem> GetComboboxByWhereClause(string whereClause, string selectedValue);
        #endregion

        #region ����:GetDynamicTreeNodes(string searchPath, string whereClause)
        /// <summary>��ȡ���Ľڵ��б�</summary>
        /// <param name="searchPath">��ѯ·��</param>
        /// <param name="whereClause">SQL����</param>
        /// <returns>���Ľڵ�</returns>
        IList<DynamicTreeNode> GetDynamicTreeNodes(string searchPath, string whereClause);
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
