#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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

namespace X3Platform.Plugins.Bugs.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CategoryIndexes;
    using X3Platform.DigitalNumber;
    using X3Platform.Entities;
    using X3Platform.Entities.Model;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Web.Component.Combobox;

    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Plugins.Bugs.Model;
    using X3Platform.Plugins.Bugs.IBLL;
    using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class BugCategoryService : IBugCategoryService
    {
        private BugConfiguration configuration = null;

        private IBugCategoryProvider provider = null;

        /// <summary></summary>
        public BugCategoryService()
        {
            this.configuration = BugConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IBugCategoryProvider>(typeof(IBugCategoryProvider));
        }

        /// <summary></summary>
        public BugCategoryInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(BugCategoryInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="BugCategoryInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="BugCategoryInfo"/>��ϸ��Ϣ</returns>
        public BugCategoryInfo Save(BugCategoryInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new Exception("ʵ����ʶ����Ϊ�ա�");
            }

            bool isNewObject = !this.IsExist(param.Id);

            string methodName = isNewObject ? "����" : "�༭";

            IAccountInfo account = KernelContext.Current.User;

            if (methodName == "����")
            {
                param.AccountId = account.Id;
                param.AccountName = account.Name;
            }

            this.provider.Save(param);

            // ����ʵ�����ݲ�����¼
            EntityLifeHistoryInfo history = new EntityLifeHistoryInfo();

            history.Id = DigitalNumberContext.Generate("Key_Guid");
            history.AccountId = account.Id;
            history.MethodName = methodName;
            history.EntityId = param.Id;
            history.EntityClassName = KernelContext.ParseObjectType(typeof(BugInfo));
            history.ContextDiffLog = string.Empty;

            EntitiesManagement.Instance.EntityLifeHistoryService.Save(history);

            return param;
        }
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        public int Delete(string id)
        {
            return this.provider.Delete(id);
        }
        #endregion

        #region ����:CanDelete(string id)
        /// <summary>�ж�����Ƿ��ܹ�������ɾ��</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        /// <returns></returns>
        public bool CanDelete(string id)
        {
            return this.provider.CanDelete(id);
        }
        #endregion

        #region ����:Remove(string id)
        /// <summary>����ɾ����¼</summary>
        /// <param name="id">ʵ��������ʶ</param>
        public int Remove(string id)
        {
            return this.provider.Remove(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="BugCategoryInfo"/>����ϸ��Ϣ</returns>
        public BugCategoryInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region ����:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">�������</param>
        /// <returns>����ʵ��<see cref="BugCategoryInfo"/>����ϸ��Ϣ</returns>
        public BugCategoryInfo FindOneByCategoryIndex(string categoryIndex)
        {
            return this.provider.FindOneByCategoryIndex(categoryIndex);
        }
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="BugCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<BugCategoryInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="BugCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<BugCategoryInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="BugCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<BugCategoryInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region ����:FindAllQueryObject(string whereClause, int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="BugCategoryQueryInfo"/>����ϸ��Ϣ</returns>
        public IList<BugCategoryQueryInfo> FindAllQueryObject(string whereClause, int length)
        {
            return this.provider.FindAllQueryObject(whereClause, length);
        }
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
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns> 
        public IList<BugCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region ����:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="BugCategoryQueryInfo"/></returns>
        public IList<BugCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">��Ա��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region ����:SetStatus(int status)
        /// <summary>�������״̬(ͣ��/����)</summary>
        /// <param name="id">��������ʶ</param>
        /// <param name="status">1 ��ͣ�õ�������ã�0 �����õ����ͣ��</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            return this.provider.SetStatus(id, status);
        }
        #endregion

        #region ����:GetComboboxByWhereClause(string whereClause, string selectedValue)
        /// <summary>��ȡ��������Դ</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="selectedValue">Ĭ��ѡ���ֵ</param>
        /// <returns></returns>
        public IList<ComboboxItem> GetComboboxByWhereClause(string whereClause, string selectedValue)
        {
            return this.provider.GetComboboxByWhereClause(whereClause, selectedValue);
        }
        #endregion

        #region ����:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        /// <summary>��ȡ�첽���ɵ���</summary>
        /// <param name="treeName">��</param>
        /// <param name="parentId">�����ڵ��ʶ</param>
        /// <param name="url">���ӵ�ַ</param>
        /// <param name="enabledLeafClick">ֻ������Ҷ�ӽڵ�</param>
        /// <param name="elevatedPrivileges">����Ȩ��</param>
        /// <returns>����ֵ</returns>
        public DynamicTreeView GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        {
            string searchPath = (parentId == "0") ? string.Empty : parentId.Replace(@"$", @"\");

            IList<DynamicTreeNode> list = this.provider.GetDynamicTreeNodes(searchPath, string.Empty);

            DynamicTreeView treeView = new DynamicTreeView(treeName, parentId);

            Dictionary<string, DynamicTreeNode> dictionary = new Dictionary<string, DynamicTreeNode>();

            foreach (DynamicTreeNode node in list)
            {
                node.ParentId = parentId;
                node.Url = url;

                if (!dictionary.ContainsKey(node.Id))
                {
                    if (node.HasChildren && enabledLeafClick)
                    {
                        node.Disabled = 1;
                    }

                    treeView.Add(node);

                    dictionary.Add(node.Id, node);
                }
            }

            return treeView;
        }
        #endregion

        #region ����:GetDynamicTreeNodes(string searchPath, string whereClause)
        /// <summary>��ȡ���Ľڵ��б�</summary>
        /// <param name="searchPath">��ѯ·��</param>
        /// <param name="whereClause">SQL����</param>
        /// <returns>���Ľڵ�</returns>
        public IList<DynamicTreeNode> GetDynamicTreeNodes(string searchPath, string whereClause)
        {
            return this.provider.GetDynamicTreeNodes(searchPath, whereClause);
        }
        #endregion

        // -------------------------------------------------------
        // ��Ȩ��Χ����
        // -------------------------------------------------------

        #region ����:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>�ж��û��Ƿ�ӵӦ�ò˵���Ȩ����Ϣ</summary>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="account">�ʺ�</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
        {
            return this.provider.HasAuthority(entityId, authorityName, account);
        }
        #endregion

        #region ����:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>����Ӧ�ò˵���Ȩ����Ϣ</summary>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            this.provider.BindAuthorizationScopeObjects(entityId, authorityName, scopeText);
        }
        #endregion

        #region ����:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>��ѯӦ�ò˵���Ȩ����Ϣ</summary>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return this.provider.GetAuthorizationScopeObjects(entityId, authorityName);
        }
        #endregion

        // -------------------------------------------------------
        // Ȩ��
        // -------------------------------------------------------

        #region ˽�к���:BindAuthorizationScopeSQL(string whereClause)
        /// <summary>��SQL��ѯ����</summary>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <returns></returns>
        private string BindAuthorizationScopeSQL(string whereClause)
        {
            string scope = string.Format(@" (
(   Id IN ( 
        SELECT DISTINCT EntityId FROM view_AuthorizationObject_Account View1, tb_Bug_Category_Scope Scope
        WHERE 
            View1.AccountId = ##{0}##
            AND View1.AuthorizationObjectId = Scope.AuthorizationObjectId
            AND View1.AuthorizationObjectType = Scope.AuthorizationObjectType)) 
) ", KernelContext.Current.User.Id);

            if (whereClause.IndexOf(scope) == -1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
            }

            return whereClause;
        }
        #endregion
    }
}
