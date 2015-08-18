namespace X3Platform.Plugins.Bugs.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Data;
  using X3Platform.DigitalNumber;
  using X3Platform.Membership;
  using X3Platform.Membership.Scope;
  using X3Platform.Spring;
  using X3Platform.Util;

  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Plugins.Bugs.IBLL;
  using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Configuration;
  using X3Platform.Email.Client;
  using X3Platform.Velocity;
  #endregion

  public sealed class BugService : IBugService
  {
    private IBugProvider provider = null;

    public BugService()
    {
      // �������󹹽���(Spring.NET)
      string springObjectFile = BugConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

      // ���������ṩ��
      this.provider = objectBuilder.GetObject<IBugProvider>(typeof(IBugProvider));
    }

    public BugInfo this[string id]
    {
      get { return this.FindOne(id); }
    }

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(BugInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">BugInfo ʵ����ϸ��Ϣ</param>
    /// <param name="message">���ݿ�������ص������Ϣ</param>
    /// <returns>BugInfo ʵ����ϸ��Ϣ</returns>
    public BugInfo Save(BugInfo param)
    {
      if (string.IsNullOrEmpty(param.Id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

      bool isNewObject = !this.IsExist(param.Id);

      BugInfo originalObject = isNewObject ? null : this.FindOne(param.Id);

      string methodName = isNewObject ? "����" : "�༭";

      if (isNewObject)
      {
        IAccountInfo account = KernelContext.Current.User;

        param.AccountId = account.Id;
        param.AccountName = account.Name;
      }

      // ָ�����⸺����
      if (string.IsNullOrEmpty(param.AssignToAccountId))
      {
        param.AssignToAccountName = string.Empty;
      }
      else
      {
        param.AssignToAccountName = MembershipManagement.Instance.AccountService[param.AssignToAccountId].Name;
      }

      // ����������
      if (string.IsNullOrEmpty(param.Code))
      {
        string code = string.Empty;

        // ���������Ϣ
        ApplicationInfo application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

        // ��ŵ����ʵ�����ݱ�
        string entityTableName = BugConfigurationView.Instance.DigitalNumberEntityTableName;

        // ��ŵ�ǰ׺����
        string prefixCode = BugConfigurationView.Instance.DigitalNumberPrefixCodeRule
                                .Replace("{ApplicationPinYin}", application.PinYin);

        // ��ŵ�������ˮ�ų���
        int incrementCodeLength = BugConfigurationView.Instance.DigitalNumberIncrementCodeLength;

        GenericSqlCommand command = this.provider.CreateGenericSqlCommand();

        code = DigitalNumberContext.GenerateDateCodeByPrefixCode(command, entityTableName, prefixCode, incrementCodeLength);

        param.Code = code.ToUpper();
      }

      // ���� Cross Site Script
      param = StringHelper.ToSafeXSS<BugInfo>(param);

      this.provider.Save(param);

      param = this.FindOne(param.Id);

      if (isNewObject) { originalObject = param; }

      // ��¼����״̬��Ϣ
      BugHistoryInfo history = new BugHistoryInfo();

      history.Id = StringHelper.ToGuid();
      history.BugId = param.Id;

      if (isNewObject)
      {
        // 0.������ | 1.ȷ���� | 2.������ | 3.�ѽ�� | 4.�ѹر�
        history.FromStatus = 0;
        history.ToStatus = 0;

        BugContext.Instance.BugHistoryService.Save(history);
      }
      else
      {
        history.FromStatus = (param.Properties["FromStatus"] == null) ? 0 : Convert.ToInt32(param.Properties["FromStatus"].ToString());
        history.ToStatus = param.Status;

        if (history.FromStatus != history.ToStatus)
        {
          BugContext.Instance.BugHistoryService.Save(history);
        }
      }

      // �����ʼ�����
      // ������״̬���͸�������, �ѽ��״̬���͸��ύ��
      if (BugConfigurationView.Instance.SendMailAlert == "ON"
        // && isNewObject || (param.UpdateDate.AddHours(2) > originalObject.UpdateDate)
          && (param.Status == 0 || param.Status == 3))
      {
        if (isNewObject || (param.Status != originalObject.Status || param.AssignToAccountId != originalObject.AssignToAccountId))
        {
          string mailTo, mailSubject, mailBody;

          IMemberInfo member = null;

          if (param.Status == 3)
          {
            // �ѽ��������, ���ʼ�֪ͨ�ύ��
            member = MembershipManagement.Instance.MemberService.FindOneByAccountId(param.AccountId);
          }
          else
          {
            member = MembershipManagement.Instance.MemberService.FindOneByAccountId(param.AssignToAccountId);
          }

          if (member != null && !string.IsNullOrEmpty(member.Email))
          {
            VelocityContext context = new VelocityContext();

            // ���ص�ǰʵ��������Ϣ
            context.Put("kernelConfiguration", KernelConfigurationView.Instance);
            // ���ص�ǰʵ��������Ϣ
            context.Put("param", param);

            mailTo = member.Email;

            mailSubject = mailBody = string.Empty;

            if (param.Status == 0)
            {
              // 0.�µ�����
              mailSubject = "����һ�������⡾" + param.Title + "�����ȴ�����";

              mailBody = VelocityManager.Instance.ParseTemplateVirtualPath(context, "/resources/email/Bug/bugzilla-new-mail.vm");
            }
            else if (param.Status == 3)
            {
              // 3.�ѽ��
              mailSubject = param.AssignToAccountId + "�ѽ����⡾" + param.Title + "���������ȷ�ϡ�";

              mailBody = VelocityManager.Instance.ParseTemplateVirtualPath(context, "/resources/email/Bug/bugzilla-resolved-mail.vm");
            }

            EmailClientContext.Instance.Send(mailTo, mailSubject, mailBody, EmailFormat.Html);
          }
        }
      }

      return param;
    }
    #endregion

    #region ����:Delete(string ids)
    /// <summary>ɾ����¼</summary>
    /// <param name="keys">��ʶ,����Զ��Ÿ���</param>
    public int Delete(string ids)
    {
      return this.provider.Delete(ids);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">�����ʶ</param>
    /// <returns>����һ��ʵ��<see cref="BugInfo"/>����ϸ��Ϣ</returns>
    public BugInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region ����:FindOneByCode(string code)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="code">������</param>
    /// <returns>����һ�� BugInfo ʵ������ϸ��Ϣ</returns>
    public BugInfo FindOneByCode(string code)
    {
      return this.provider.FindOneByCode(code);
    }
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>��������ʵ��<see cref="BugInfo"/>����ϸ��Ϣ</returns>
    public IList<BugInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>��������ʵ��<see cref="BugInfo"/>����ϸ��Ϣ</returns>
    public IList<BugInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>��������ʵ��<see cref="BugInfo"/>����ϸ��Ϣ</returns>
    public IList<BugInfo> FindAll(string whereClause, int length)
    {
      // ��֤����Ա���
      if (AppsSecurity.IsAdministrator(KernelContext.Current.User, BugConfiguration.ApplicationName))
      {
        return this.provider.FindAll(whereClause, length);
      }
      else
      {
        return this.provider.FindAll(BindAuthorizationScopeSQL(whereClause), length);
      }
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
    /// <returns>����һ���б�ʵ��</returns>
    public IList<BugInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      // ��֤����Ա���
      // if (AppsSecurity.IsAdministrator(KernelContext.Current.User, "Bug"))
      // {
      //    return this.provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
      // }
      // else
      // {
      //    return this.provider.GetPaging(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
      // }
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
    /// <returns>����һ���б�ʵ��</returns>
    public IList<BugQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      // ��֤����Ա���
      //if (AppsSecurity.IsAdministrator(KernelContext.Current.User, "Bug"))
      //{
      //    return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
      //}
      //else
      //{
      //    return this.provider.GetQueryObjectPaging(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
      //}
    
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

    #region ����:GetAuthorizationScopeObjects(string entityId, string authorityName)
    /// <summary>��ѯʵ������Ȩ����Ϣ</summary>
    /// <param name="entityId">ʵ���ʶ</param>
    /// <param name="authorityName">Ȩ������</param>
    /// <returns></returns>
    public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
    {
      return this.provider.GetAuthorizationScopeObjects(entityId, authorityName);
    }
    #endregion

    // -------------------------------------------------------
    // Ȩ������
    // -------------------------------------------------------

    #region ����:HasAuthorizationReadObject(BugInfo param)
    /// <summary>��֤�����Ȩ��</summary>
    /// <param name="param">����֤�Ķ���</param>
    /// <returns>����</returns>
    private bool HasAuthority(BugInfo param)
    {
      IAccountInfo account = KernelContext.Current.User;

      if (AppsSecurity.IsAdministrator(account, "Bug")
          || param.AccountId == account.Id
          || param.AssignToAccountId == account.Id)
      {
        return true;
      }
      else
      {
        if (MembershipAuthorizationScopeManagement.Authenticate(param.AuthorizationReadScopeObjects, account))
        {
          return true;
        }

        return false;
      }
    }
    #endregion

    #region ����:BindAuthorizationScopeSQL(string whereClause)
    /// <summary>��SQL��ѯ����</summary>
    /// <param name="whereClause">WHERE ��ѯ����</param>
    /// <returns></returns>
    private string BindAuthorizationScopeSQL(string whereClause)
    {
      string tablePrefix = BugConfigurationView.Instance.ProjectDataTablePrefix;

      // ��Ŀ�ĳ�Ա Ҳ���Կ������������Ϣ
      string scope = string.Format(@" (
(   ( LENGTH(T.ProjectId) = 0 OR T.ProjectId IN (
        SELECT DISTINCT EntityId FROM view_AuthorizationObject_Account View1, {1}tb_Project_Scope Scope
        WHERE
            View1.GranteeId = ##{0}##
            AND View1.AuthorizationObjectId = Scope.AuthorizationObjectId
            AND View1.AuthorizationObjectType = Scope.AuthorizationObjectType))) OR
    ( T.AccountId = ##{0}## OR T.AssignToAccountId = ##{0}## )
) ", KernelContext.Current.User.Id, tablePrefix);

      if (whereClause.IndexOf(scope) == -1)
      {
        whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
      }

      return whereClause;
    }
    #endregion
  }
}
