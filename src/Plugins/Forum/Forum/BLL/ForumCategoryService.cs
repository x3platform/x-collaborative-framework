#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumCategoryService.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;

  using X3Platform.Apps;
  using X3Platform.CategoryIndexes;
  using X3Platform.Membership;
  using X3Platform.Membership.Model;
  using X3Platform.Security.Authority;
  using X3Platform.Spring;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IBLL;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Membership.Scope;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  public class ForumCategoryService : IForumCategoryService
  {
    /// <summary>����</summary>
    private ForumConfiguration configuration = null;

    /// <summary>�����ṩ��</summary>
    private IForumCategoryProvider provider = null;

    #region ���캯��:ForumCategoryService()
    /// <summary>���캯��</summary>
    public ForumCategoryService()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      // �������󹹽���(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

      // ���������ṩ��
      this.provider = objectBuilder.GetObject<IForumCategoryProvider>(typeof(IForumCategoryProvider));
    }
    #endregion

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(ForumCategoryInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
    /// <returns>ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</returns>
    public ForumCategoryInfo Save(ForumCategoryInfo param)
    {
      return this.provider.Save(param);
    }
    #endregion

    #region ����:Delete(string id)
    /// <summary>ɾ����¼</summary>
    /// <param name="id">ʵ���ı�ʶ</param>
    public void Delete(string id)
    {
      this.provider.Delete(id);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">��ʶ</param>
    /// <returns>����ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
    public ForumCategoryInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region ����:FindOneByCategoryIndex(string categoryIndex)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="categoryIndex">��������</param>
    /// <returns>����ʵ��<see cref="DengBaoCategoryInfo"/>����ϸ��Ϣ</returns>
    public ForumCategoryInfo FindOneByCategoryIndex(string categoryIndex)
    {
      return provider.FindOneByCategoryIndex(categoryIndex);
    }
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>��������ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
    public IList<ForumCategoryInfo> FindAll()
    {
      return this.FindAll(string.Empty);
    }
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>��������ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
    public IList<ForumCategoryInfo> FindAll(string whereClause)
    {
      return this.FindAll(whereClause, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause, int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>��������ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
    public IList<ForumCategoryInfo> FindAll(string whereClause, int length)
    {
      return this.provider.FindAll(whereClause, length);
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
    /// <param name="rowCount">����</param>
    /// <returns>����һ���б�ʵ��<see cref="ForumCategoryInfo"/></returns>
    public IList<ForumCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region ����:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>��ҳ����</summary>
    /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
    /// <param name="pageSize">ҳ���С</param>
    /// <param name="whereClause">WHERE ��ѯ����</param>
    /// <param name="orderBy">ORDER BY ��������</param>
    /// <param name="rowCount">����</param>
    /// <returns>����һ���б�ʵ��<see cref="ForumCategoryQueryInfo"/></returns>
    public IList<ForumCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼.</summary>
    /// <param name="id">��ʶ</param>
    /// <returns>����ֵ</returns>
    public bool IsExist(string id)
    {
      return this.provider.IsExist(id);
    }
    #endregion

    #region ����:FetchCategoryIndex(string isStatus)
    ///<summary>���ݷ���״̬��ȡ��������</summary>
    ///<param name="isStatus">״̬</param>
    ///<returns>������������</returns>
    public ICategoryIndex FetchCategoryIndex(string isStatus)
    {
      string whereClause = string.Empty;
      if (isStatus == "1")
      {
        whereClause = " Status = 1 ";
      }
      return provider.FetchCategoryIndex(this.BindAuthorizationScopeSQL(whereClause));
    }
    #endregion

    #region ����:IsAnonymous(string id);
    /// <summary>
    /// ��ѯ�Ƿ�������������
    /// </summary>
    /// <param name="id">��ʶ</param>
    /// <returns>����ֵ</returns>
    public bool IsAnonymous(string id)
    {
      bool result = true;

      ForumCategoryInfo categoryinfo = this.FindOne(id);
      if (categoryinfo.Anonymous == 0)
      {
        result = false;
      }
      return result;
    }
    #endregion

    #region ����:IsExistByParent(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼.</summary>
    /// <param name="id">��ʶ</param>
    /// <returns>����ֵ</returns>
    public bool IsExistByParent(string id)
    {
      return provider.IsExistByParent(id);
    }
    #endregion

    #region ����:IsCategoryAdministrator(string categoryId)
    /// <summary>�жϵ�ǰ�û��Ƿ��Ǹð��Ĺ���Ա</summary>
    /// <param name="categoryId">�����</param>
    /// <returns>�������</returns>
    public bool IsCategoryAdministrator(string categoryId)
    {
      return this.provider.IsCategoryAdministrator(categoryId);
    }
    #endregion

    #region ����:IsCategoryAuthority(string categoryId)
    /// <summary>���ݰ���ʶ��ѯ��ǰ�û��Ƿ�ӵ�и�Ȩ��</summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public bool IsCategoryAuthority(string categoryId)
    {
      return this.provider.IsCategoryAuthority(categoryId);
    }
    #endregion

    #region ����:GetCategoryAdminName(string categoryId)
    /// <summary>
    /// ��ѯ��������
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="applicationTag"></param>
    /// <returns></returns>
    public string GetCategoryAdminName(string categoryId)
    {
      return this.provider.GetCategoryAdminName(categoryId);
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
      // ��֤����Ա���
      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, ForumConfiguration.ApplicationName))
      {
        IAccountInfo account = KernelContext.Current.User;
        // string tableName = ForumUtility.ToDataTablePrefix(applicationTag) + "_Category_Scope";
        string tableName = "tb_Forum_Category_Scope";
        string scope = MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeEntitySQL(
            tableName,
            account.Id,
            ContactType.Default,
            "00000000-0000-0000-0000-000000000003,00000000-0000-0000-0000-000000000001");

        scope = @"( T.Id IN ( " + scope + " ) ) ";

        if (whereClause.IndexOf(scope) == -1)
        {
          whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
        }
      }

      return whereClause;
    }
    #endregion
  }
}
