namespace X3Platform.Plugins.Forum.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Text;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;
  using X3Platform.Security.Authority;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  using X3Platform.CategoryIndexes;
  using X3Platform.Membership.Scope;
  using X3Platform.Membership;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class ForumCategoryProvider : IForumCategoryProvider
  {
    /// <summary>����</summary>
    private ForumConfiguration configuration = null;

    /// <summary>IBatisӳ���ļ�</summary>
    private string ibatisMapping = null;

    /// <summary>IBatisӳ�����</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>���ݱ���</summary>
    private string tableName = "tb_Forum_Category";

    #region ���캯��:ForumCategoryProvider()
    /// <summary>���캯��</summary>
    public ForumCategoryProvider()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // ����֧��
    // -------------------------------------------------------

    #region ����:BeginTransaction()
    /// <summary>��������</summary>
    public void BeginTransaction()
    {
      this.ibatisMapper.BeginTransaction();
    }
    #endregion

    #region ����:BeginTransaction(IsolationLevel isolationLevel)
    /// <summary>��������</summary>
    /// <param name="isolationLevel">������뼶��</param>
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
      this.ibatisMapper.BeginTransaction(isolationLevel);
    }
    #endregion

    #region ����:CommitTransaction()
    /// <summary>�ύ����</summary>
    public void CommitTransaction()
    {
      this.ibatisMapper.CommitTransaction();
    }
    #endregion

    #region ����:RollBackTransaction()
    /// <summary>�ع�����</summary>
    public void RollBackTransaction()
    {
      this.ibatisMapper.RollBackTransaction();
    }
    #endregion

    // -------------------------------------------------------
    // ��� ɾ�� �޸�
    // -------------------------------------------------------

    #region ����:Save(ForumCategoryInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
    /// <returns>ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</returns>
    public ForumCategoryInfo Save(ForumCategoryInfo param)
    {
      if (!IsExist(param.Id))
      {
        Insert(param);
      }
      else
      {
        Update(param);
      }

      return param;
    }
    #endregion

    #region ����:Insert(ForumCategoryInfo param)
    /// <summary>��Ӽ�¼</summary>
    /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
    public void Insert(ForumCategoryInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region ����:Update(ForumCategoryInfo param)
    /// <summary>�޸ļ�¼</summary>
    /// <param name="param">ʵ��<see cref="ForumCategoryInfo"/>��ϸ��Ϣ</param>
    public void Update(ForumCategoryInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_UpdateThread", tableName)), param);
    }
    #endregion

    #region ����:Delete(string ids)
    /// <summary>ɾ����¼</summary>
    /// <param name="ids">��ʶ,����Զ��Ÿ���.</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids)) { return; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete(PhysicallyRemoved)", tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));

      return this.ibatisMapper.QueryForObject<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
    }
    #endregion

    #region ����:FindOneByCategoryIndex(string categoryIndex)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="categoryIndex">��������</param>
    /// <returns>����ʵ��<see cref="DengBaoCategoryInfo"/>����ϸ��Ϣ</returns>
    public ForumCategoryInfo FindOneByCategoryIndex(string categoryIndex)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("CategoryIndex", StringHelper.ToSafeSQL(categoryIndex));

      return ibatisMapper.QueryForObject<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCategoryIndex", tableName)), args);
    }
    #endregion

    #region ����:FindAll(string whereClause, int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>��������ʵ��<see cref="ForumCategoryInfo"/>����ϸ��Ϣ</returns>
    public IList<ForumCategoryInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumCategoryInfo> list = this.ibatisMapper.QueryForList<ForumCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<ForumCategoryQueryInfo> list = this.ibatisMapper.QueryForList<ForumCategoryQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼.</summary>
    /// <param name="id">��ʶ</param>
    /// <returns>����ֵ</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        throw new Exception("ʵ����ʶ����Ϊ�ա�");
      }

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region ����:IsExistByParent(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼.</summary>
    /// <param name="id">��ʶ</param>
    /// <returns>����ֵ</returns>
    public bool IsExistByParent(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        throw new Exception("ʵ����ʶ����Ϊ�ա�");
      }

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("WhereClause", string.Format(" ParentId = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region ����:FetchCategoryIndex(string whereClause)
    /// <summary>���ݷ���״̬��ȡ��������</summary>
    /// <param name="whereClause">WHERE ��ѯ����</param>
    /// <param name="applicationTag">�����ʶ</param>
    /// <returns>������������</returns>
    public ICategoryIndex FetchCategoryIndex(string whereClause)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      //ApplicationInfo application = AppsContext.Instance.ApplicationService["Forum"];
      //if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      //{
      //    StringBuilder bindScope = new StringBuilder();
      //    bindScope.Append(string.Format(" Id IN (SELECT distinct EntityId from {0}_Category_Scope S,view_AuthorizationObject_Account A", ForumUtility.ToDataTablePrefix(applicationTag)));
      //    bindScope.Append(" where  s.AuthorizationObjectId=a.AuthorizationObjectId");
      //    bindScope.Append(" and s.AuthorizationObjectType = a.AuthorizationObjectType");
      //    bindScope.Append(" and a.AccountId = '" + KernelContext.Current.User.Id + "')");
      //    args.Add("BindScope", bindScope);
      //}
      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

      IList<string> list = ibatisMapper.QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_FetchCategoryIndex", tableName)), args);

      CategoryIndexWriter writer = new CategoryIndexWriter("ѡ�����");

      foreach (string item in list)
      {
        if (item != "")
        {
          writer.Read(item);
        }
      }

      return writer.Write();
    }
    #endregion

    #region ����:IsCategoryAdministrator(string categoryId)
    /// <summary>
    /// �жϵ�ǰ�û��Ƿ��Ǹð��Ĺ���Ա
    /// <param name="categoryId">�����</param>
    /// <returns>�������</returns>
    public bool IsCategoryAdministrator(string categoryId)
    {
      AuthorityInfo moderator = AuthorityContext.Instance.AuthorityService["Ӧ��_Ĭ��_����Ա"];

      Dictionary<string, object> args = new Dictionary<string, object>();
      // args.Add("ApplicationStore", ForumUtility.ToDataTablePrefix(applicationTag));

      args.Add("CategoryId", StringHelper.ToSafeSQL(categoryId));
      args.Add("AccountId", StringHelper.ToSafeSQL(KernelContext.Current.User.Id));
      args.Add("AuthorityId", StringHelper.ToSafeSQL(moderator.Id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsCategoryAdministrator", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region ����:IsCategoryAuthority(string categoryId)
    /// <summary>���ݰ���ʶ��ѯ��ǰ�û��Ƿ�ӵ�и�Ȩ��</summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public bool IsCategoryAuthority(string categoryId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("CategoryId", StringHelper.ToSafeSQL(categoryId));
      args.Add("AccountId", StringHelper.ToSafeSQL(KernelContext.Current.User.Id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsCategoryAuthority", tableName)), args)) == 0) ? false : true;
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
      ForumCategoryInfo info = this.FindOne(categoryId);
      string categoryAdminName = string.Empty;
      if (info != null)
      {
        // categoryAdminName = info.ModeratorScopeObjectView;
      }
      return categoryAdminName;

    }
    #endregion

    // -------------------------------------------------------
    // ��Ȩ��Χ����
    // -------------------------------------------------------

    #region ����:HasAuthority(string entityId, string authorityName, IAccountInfo account)
    /// <summary>�ж��û��Ƿ�ӵ����Ȩ����Ϣ</summary>
    /// <param name="entityId">ʵ���ʶ</param>
    /// <param name="authorityName">Ȩ������</param>
    /// <param name="account">�ʺ�</param>
    /// <returns>����ֵ</returns>
    public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
    {
      return MembershipManagement.Instance.AuthorizationObjectService.HasAuthority(
          this.ibatisMapper.CreateGenericSqlCommand(),
          string.Format("{0}_Scope", this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(ForumCategoryInfo)),
          authorityName,
          account);
    }
    #endregion

    #region ����:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    /// <summary>�������ݵ�Ȩ����Ϣ</summary>
    /// <param name="entityId">ʵ���ʶ</param>
    /// <param name="authorityName">Ȩ������</param>
    /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
    public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    {
      MembershipManagement.Instance.AuthorizationObjectService.BindAuthorizationScopeObjects(
          this.ibatisMapper.CreateGenericSqlCommand(),
          string.Format("{0}_Scope", this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(ForumCategoryInfo)),
          authorityName,
          scopeText);
    }
    #endregion

    #region ����:GetAuthorizationScopeObjects(string entityId, string authorityName)
    /// <summary>��ѯӦ�õ�Ȩ����Ϣ</summary>
    /// <param name="entityId">ʵ���ʶ</param>
    /// <param name="authorityName">Ȩ������</param>
    /// <returns></returns>
    public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
    {
      return MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeObjects(
          this.ibatisMapper.CreateGenericSqlCommand(),
          string.Format("{0}_Scope", this.tableName),
          entityId,
          KernelContext.ParseObjectType(typeof(ForumCategoryInfo)),
          authorityName);
    }
    #endregion
  }
}
