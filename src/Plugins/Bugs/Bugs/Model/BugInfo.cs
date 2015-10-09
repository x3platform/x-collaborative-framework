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
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.Model
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using X3Platform.Apps;
  using X3Platform.Membership;
  using X3Platform.Membership.Scope;
  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Security.Authority;
  #endregion

  /// <summary></summary>
  public class BugInfo : EntityClass
  {
    public BugInfo() { }

    #region ����:Id
    private string m_Id;

    /// <summary>��ʶ</summary>
    public string Id
    {
      get { return m_Id; }
      set { m_Id = value; }
    }
    #endregion

    #region ����:Code
    private string m_Code = string.Empty;

    /// <summary>���</summary>
    public string Code
    {
      get { return this.m_Code; }
      set { this.m_Code = value; }
    }
    #endregion

    #region ����:AccountId
    private string m_AccountId = string.Empty;

    /// <summary>�ύ�˱�ʶ</summary>
    public string AccountId
    {
      get { return m_AccountId; }
      set { m_AccountId = value; }
    }
    #endregion

    #region ����:AccountName
    private string m_AccountName = string.Empty;

    /// <summary>�ύ������</summary>
    public string AccountName
    {
      get { return m_AccountName; }
      set { m_AccountName = value; }
    }
    #endregion

    #region ����:ProjectId
    private string m_ProjectId = string.Empty;

    /// <summary>��Ŀ��ʶ</summary>
    public string ProjectId
    {
      get { return m_ProjectId; }
      set { m_ProjectId = value; }
    }
    #endregion

    #region ����:ProjectName
    private string m_ProjectName = string.Empty;

    /// <summary>��Ŀ����</summary>
    public string ProjectName
    {
      get { return m_ProjectName; }
      set { m_ProjectName = value; }
    }
    #endregion

    #region ����:CategoryId
    private string m_CategoryId;

    /// <summary>��Ŀ��ʶ</summary>
    public string CategoryId
    {
      get { return m_CategoryId; }
      set { m_CategoryId = value; }
    }
    #endregion

    #region ����:CategoryIndex
    private string m_CategoryIndex = string.Empty;

    /// <summary>�������</summary>
    public string CategoryIndex
    {
      get { return m_CategoryIndex; }
      set { m_CategoryIndex = value; }
    }
    #endregion

    #region ����:Title
    private string m_Title = string.Empty;

    /// <summary>����</summary>
    public string Title
    {
      get { return m_Title; }
      set { m_Title = value; }
    }
    #endregion

    #region ����:Content
    private string m_Content = string.Empty;

    /// <summary>����</summary>
    public string Content
    {
      get { return m_Content; }
      set { m_Content = value; }
    }
    #endregion

    #region ����:Tags
    private string m_Tags = string.Empty;

    /// <summary>��ǩ</summary>
    public string Tags
    {
      get { return m_Tags; }
      set { m_Tags = value; }
    }
    #endregion

    #region ����:IsInternal
    private bool m_IsInternal;

    /// <summary>�Ƿ��ڲ�����</summary>
    public bool IsInternal
    {
      get { return m_IsInternal; }
      set { m_IsInternal = value; }
    }
    #endregion

    #region ����:AssignToAccountId
    private string m_AssignToAccountId = string.Empty;

    /// <summary>ָ���ʺű�ʶ</summary>
    public string AssignToAccountId
    {
      get { return m_AssignToAccountId; }
      set { m_AssignToAccountId = value; }
    }
    #endregion

    #region ����:AssignToAccountName

    private string m_AssignToAccountName = string.Empty;
    /// <summary>ָ���ʺ�����</summary>
    public string AssignToAccountName
    {
      get { return m_AssignToAccountName; }
      set { m_AssignToAccountName = value; }
    }
    #endregion

    #region ����:SimilarBugIds
    private string m_SimilarBugIds = string.Empty;

    /// <summary>���������ʶ</summary>
    public string SimilarBugIds
    {
      get { return m_SimilarBugIds; }
      set { m_SimilarBugIds = value; }
    }
    #endregion

    #region ����:Priority
    private int m_Priority;

    /// <summary>Ȩ��ֵ</summary>
    public int Priority
    {
      get { return m_Priority; }
      set { m_Priority = value; }
    }
    #endregion

    #region ����:PriorityView
    private string m_PriorityView = null;

    /// <summary>Ȩ��ֵ��ͼ</summary>
    public string PriorityView
    {
      get
      {
        if (string.IsNullOrEmpty(m_PriorityView))
        {
          this.m_PriorityView = AppsContext.Instance.ApplicationSettingService.GetText(
             AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
             "Ӧ�ù���_Эͬƽ̨_�������_�������ȼ�",
             this.Priority.ToString());
        }

        return m_PriorityView;
      }
    }
    #endregion

    #region ����:Status
    private int m_Status;

    /// <summary>״̬: 0.������ | 1.ȷ���� | 2.������ | 3.�ѽ�� | 4.�ѹر�</summary>
    public int Status
    {
      get { return m_Status; }
      set { m_Status = value; m_StatusView = null; }
    }
    #endregion

    #region ����:StatusView
    private string m_StatusView = null;

    /// <summary>״ֵ̬</summary>
    public string StatusView
    {
      get
      {
        if (string.IsNullOrEmpty(m_StatusView))
        {
          this.m_StatusView = AppsContext.Instance.ApplicationSettingService.GetText(
             AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
             "Ӧ�ù���_Эͬƽ̨_�������_����״̬",
             this.Status.ToString());
        }

        return m_StatusView;
      }
    }
    #endregion

    #region ����:OrderId
    private int m_OrderId;

    /// <summary>������</summary>
    public int OrderId
    {
      get { return m_OrderId; }
      set { m_OrderId = value; }
    }
    #endregion

    #region ����:ModifiedDate
    private DateTime m_ModifiedDate;

    /// <summary>�޸�����</summary>
    public DateTime ModifiedDate
    {
      get { return m_ModifiedDate; }
      set { m_ModifiedDate = value; }
    }
    #endregion

    #region ����:CreatedDate
    private DateTime m_CreatedDate;

    /// <summary>��������</summary>
    public DateTime CreatedDate
    {
      get { return m_CreatedDate; }
      set { m_CreatedDate = value; }
    }
    #endregion

    // -------------------------------------------------------
    // ��չ����
    // -------------------------------------------------------

    private IList<BugCommentInfo> m_Comments = null;

    /// <summary>����</summary>
    public IList<BugCommentInfo> Comments
    {
      get
      {
        if (!string.IsNullOrEmpty(Id) && m_Comments == null)
        {
          string whereClause = string.Format(" BugId=##{0}## ORDER BY CreatedDate", Id);

          m_Comments = BugContext.Instance.BugCommentService.FindAll(whereClause);
        }

        return m_Comments;
      }
    }

    private IList<BugHistoryInfo> m_Histories = null;

    /// <summary>��ʷ</summary>
    public IList<BugHistoryInfo> Histories
    {
      get
      {
        if (!string.IsNullOrEmpty(Id) && m_Histories == null)
        {
          string whereClause = string.Format(" BugId = ##{0}## ORDER BY CreatedDate", Id);

          m_Histories = BugContext.Instance.BugHistoryService.FindAll(whereClause);
        }

        return m_Histories;
      }
    }

    // -------------------------------------------------------
    // ���÷�Χ
    // -------------------------------------------------------

    /// <summary>Ȩ��:Ӧ��_ͨ��_�鿴Ȩ��</summary>
    private AuthorityInfo authorizationRead
        = AuthorityContext.Instance.AuthorityService["Ӧ��_ͨ��_�鿴Ȩ��"];

    #region ����:BindAuthorizationReadScope(string scopeText)
    /// <summary>�󶨲鿴Ȩ��</summary>
    /// <param name="scopeText"></param>
    public void BindAuthorizationReadScope(string scopeText)
    {
      // ��ջ�������
      this.m_AuthorizationReadScopeObjectText = null;
      this.m_AuthorizationReadScopeObjectView = null;

      MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.AuthorizationReadScopeObjects, scopeText);
    }
    #endregion

    #region ����:AuthorizationReadScopeObjects
    private IList<MembershipAuthorizationScopeObject> m_AuthorizationReadScopeObjects = null;

    /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ</summary>
    public IList<MembershipAuthorizationScopeObject> AuthorizationReadScopeObjects
    {
      get
      {
        if (m_AuthorizationReadScopeObjects == null)
        {
          m_AuthorizationReadScopeObjects = BugContext.Instance.BugService.GetAuthorizationScopeObjects(
              this.EntityId,
              this.authorizationRead.Name);
        }

        return m_AuthorizationReadScopeObjects;
      }
    }
    #endregion

    #region ����:AuthorizationReadScopeObjectText
    private string m_AuthorizationReadScopeObjectText = null;

    /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ�ı�</summary>
    public string AuthorizationReadScopeObjectText
    {
      get
      {
        if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectText))
        {
          m_AuthorizationReadScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationReadScopeObjects);
        }

        return m_AuthorizationReadScopeObjectText;
      }
    }
    #endregion

    #region ����:AuthorizationReadScopeObjectView
    private string m_AuthorizationReadScopeObjectView = null;

    /// <summary>Ȩ�ޣ�Ӧ��_ͨ��_�鿴Ȩ�޷�Χ��ͼ</summary>
    public string AuthorizationReadScopeObjectView
    {
      get
      {
        if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectView))
        {
          m_AuthorizationReadScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationReadScopeObjects);
        }

        return m_AuthorizationReadScopeObjectView;
      }
    }
    #endregion

    //
    // ���� EntityClass ��ʶ
    // 

    #region ����:EntityId
    /// <summary>ʵ������ʶ</summary>
    public override string EntityId
    {
      get { return this.Id; }
    }
    #endregion
  }
}
