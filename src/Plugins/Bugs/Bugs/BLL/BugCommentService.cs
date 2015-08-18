namespace X3Platform.Plugins.Bugs.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;

  using X3Platform.Spring;

  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Plugins.Bugs.IBLL;
  using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Email.Client;
  using X3Platform.Velocity;
  using X3Platform.Configuration;
  using X3Platform.Membership;
  using X3Platform.Data;
  #endregion

  public sealed class BugCommentService : IBugCommentService
  {
    private BugConfiguration configuration = null;

    private IBugCommentProvider provider = null;

    public BugCommentService()
    {
      configuration = BugConfigurationView.Instance.Configuration;

      // �������󹹽���(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

      // ���������ṩ��
      this.provider = objectBuilder.GetObject<IBugCommentProvider>(typeof(IBugCommentProvider));
    }

    public BugCommentInfo this[string index]
    {
      get { return this.FindOne(index); }
    }

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(AccountInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">BugCommentInfo ʵ����ϸ��Ϣ</param>
    /// <param name="message">���ݿ�������ص������Ϣ</param>
    /// <returns>BugCommentInfo ʵ����ϸ��Ϣ</returns>
    public BugCommentInfo Save(BugCommentInfo param)
    {
      if (string.IsNullOrEmpty(param.Id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

      if (string.IsNullOrEmpty(param.BugId)) { throw new Exception("�����������ʶ����Ϊ�ա�"); }

      bool isNewObject = !this.IsExist(param.Id);

      if (isNewObject)
      {
        IAccountInfo account = KernelContext.Current.User;

        param.AccountId = account.Id;
      }

      this.provider.Save(param);

      param = this.FindOne(param.Id);

      // �����ʼ�����
      // �·������͸��ύ��
      if (BugConfigurationView.Instance.SendMailAlert == "ON" && isNewObject)
      {
        string mailTo, mailSubject, mailBody;

        IMemberInfo member = MembershipManagement.Instance.MemberService.FindOneByAccountId(param.Bug.AccountId);

        if (member != null && !string.IsNullOrEmpty(member.Email))
        {
          VelocityContext context = new VelocityContext();

          // ���ص�ǰʵ��������Ϣ
          context.Put("kernelConfiguration", KernelConfigurationView.Instance);
          // ���ص�ǰʵ��������Ϣ
          context.Put("param", param);

          mailTo = member.Email;

          mailSubject = "���ύ�����⡾" + param.Bug.Title + "����һ���µķ�����";

          mailBody = VelocityManager.Instance.ParseTemplateVirtualPath(context, "/resources/email/Bug/bugzilla-comment-reply-mail.vm");

          EmailClientContext.Instance.Send(mailTo, mailSubject, mailBody, EmailFormat.Html);
        }
      }

      return this.provider.Save(param);
    }
    #endregion

    #region ����:Delete(string ids)
    /// <summary>ɾ����¼</summary>
    /// <param name="keys">��ʶ,����Զ��Ÿ���</param>
    public void Delete(string id)
    {
      this.provider.Delete(id);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(int id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">AccountInfo Id��</param>
    /// <returns>����һ�� AccountInfo ʵ������ϸ��Ϣ</returns>
    public BugCommentInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugCommentInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugCommentInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugCommentInfo> FindAll(string whereClause, int length)
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
    /// <returns>����һ���б�ʵ��</returns> 
    public IList<BugCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
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

    // -------------------------------------------------------
    // Ȩ��
    // -------------------------------------------------------

    #region ����:GetAuthorizationObject(BugCommentInfo param)
    /// <summary>��֤�����Ȩ��</summary>
    /// <param name="param">����֤�Ķ���</param>
    /// <returns>����</returns>
    private BugCommentInfo GetAuthorizationObject(BugCommentInfo param)
    {
      return param;
    }
    #endregion
  }
}
