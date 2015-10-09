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

    #region 属性:Id
    private string m_Id;

    /// <summary>标识</summary>
    public string Id
    {
      get { return m_Id; }
      set { m_Id = value; }
    }
    #endregion

    #region 属性:Code
    private string m_Code = string.Empty;

    /// <summary>编号</summary>
    public string Code
    {
      get { return this.m_Code; }
      set { this.m_Code = value; }
    }
    #endregion

    #region 属性:AccountId
    private string m_AccountId = string.Empty;

    /// <summary>提交人标识</summary>
    public string AccountId
    {
      get { return m_AccountId; }
      set { m_AccountId = value; }
    }
    #endregion

    #region 属性:AccountName
    private string m_AccountName = string.Empty;

    /// <summary>提交人姓名</summary>
    public string AccountName
    {
      get { return m_AccountName; }
      set { m_AccountName = value; }
    }
    #endregion

    #region 属性:ProjectId
    private string m_ProjectId = string.Empty;

    /// <summary>项目标识</summary>
    public string ProjectId
    {
      get { return m_ProjectId; }
      set { m_ProjectId = value; }
    }
    #endregion

    #region 属性:ProjectName
    private string m_ProjectName = string.Empty;

    /// <summary>项目名称</summary>
    public string ProjectName
    {
      get { return m_ProjectName; }
      set { m_ProjectName = value; }
    }
    #endregion

    #region 属性:CategoryId
    private string m_CategoryId;

    /// <summary>项目标识</summary>
    public string CategoryId
    {
      get { return m_CategoryId; }
      set { m_CategoryId = value; }
    }
    #endregion

    #region 属性:CategoryIndex
    private string m_CategoryIndex = string.Empty;

    /// <summary>类别索引</summary>
    public string CategoryIndex
    {
      get { return m_CategoryIndex; }
      set { m_CategoryIndex = value; }
    }
    #endregion

    #region 属性:Title
    private string m_Title = string.Empty;

    /// <summary>标题</summary>
    public string Title
    {
      get { return m_Title; }
      set { m_Title = value; }
    }
    #endregion

    #region 属性:Content
    private string m_Content = string.Empty;

    /// <summary>内容</summary>
    public string Content
    {
      get { return m_Content; }
      set { m_Content = value; }
    }
    #endregion

    #region 属性:Tags
    private string m_Tags = string.Empty;

    /// <summary>标签</summary>
    public string Tags
    {
      get { return m_Tags; }
      set { m_Tags = value; }
    }
    #endregion

    #region 属性:IsInternal
    private bool m_IsInternal;

    /// <summary>是否内部问题</summary>
    public bool IsInternal
    {
      get { return m_IsInternal; }
      set { m_IsInternal = value; }
    }
    #endregion

    #region 属性:AssignToAccountId
    private string m_AssignToAccountId = string.Empty;

    /// <summary>指派帐号标识</summary>
    public string AssignToAccountId
    {
      get { return m_AssignToAccountId; }
      set { m_AssignToAccountId = value; }
    }
    #endregion

    #region 属性:AssignToAccountName

    private string m_AssignToAccountName = string.Empty;
    /// <summary>指派帐号名称</summary>
    public string AssignToAccountName
    {
      get { return m_AssignToAccountName; }
      set { m_AssignToAccountName = value; }
    }
    #endregion

    #region 属性:SimilarBugIds
    private string m_SimilarBugIds = string.Empty;

    /// <summary>类似问题标识</summary>
    public string SimilarBugIds
    {
      get { return m_SimilarBugIds; }
      set { m_SimilarBugIds = value; }
    }
    #endregion

    #region 属性:Priority
    private int m_Priority;

    /// <summary>权重值</summary>
    public int Priority
    {
      get { return m_Priority; }
      set { m_Priority = value; }
    }
    #endregion

    #region 属性:PriorityView
    private string m_PriorityView = null;

    /// <summary>权重值视图</summary>
    public string PriorityView
    {
      get
      {
        if (string.IsNullOrEmpty(m_PriorityView))
        {
          this.m_PriorityView = AppsContext.Instance.ApplicationSettingService.GetText(
             AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
             "应用管理_协同平台_问题跟踪_问题优先级",
             this.Priority.ToString());
        }

        return m_PriorityView;
      }
    }
    #endregion

    #region 属性:Status
    private int m_Status;

    /// <summary>状态: 0.新问题 | 1.确认中 | 2.处理中 | 3.已解决 | 4.已关闭</summary>
    public int Status
    {
      get { return m_Status; }
      set { m_Status = value; m_StatusView = null; }
    }
    #endregion

    #region 属性:StatusView
    private string m_StatusView = null;

    /// <summary>状态值</summary>
    public string StatusView
    {
      get
      {
        if (string.IsNullOrEmpty(m_StatusView))
        {
          this.m_StatusView = AppsContext.Instance.ApplicationSettingService.GetText(
             AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName].Id,
             "应用管理_协同平台_问题跟踪_问题状态",
             this.Status.ToString());
        }

        return m_StatusView;
      }
    }
    #endregion

    #region 属性:OrderId
    private int m_OrderId;

    /// <summary>排序编号</summary>
    public int OrderId
    {
      get { return m_OrderId; }
      set { m_OrderId = value; }
    }
    #endregion

    #region 属性:ModifiedDate
    private DateTime m_ModifiedDate;

    /// <summary>修改日期</summary>
    public DateTime ModifiedDate
    {
      get { return m_ModifiedDate; }
      set { m_ModifiedDate = value; }
    }
    #endregion

    #region 属性:CreatedDate
    private DateTime m_CreatedDate;

    /// <summary>创建日期</summary>
    public DateTime CreatedDate
    {
      get { return m_CreatedDate; }
      set { m_CreatedDate = value; }
    }
    #endregion

    // -------------------------------------------------------
    // 扩展属性
    // -------------------------------------------------------

    private IList<BugCommentInfo> m_Comments = null;

    /// <summary>评论</summary>
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

    /// <summary>历史</summary>
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
    // 设置范围
    // -------------------------------------------------------

    /// <summary>权限:应用_通用_查看权限</summary>
    private AuthorityInfo authorizationRead
        = AuthorityContext.Instance.AuthorityService["应用_通用_查看权限"];

    #region 函数:BindAuthorizationReadScope(string scopeText)
    /// <summary>绑定查看权限</summary>
    /// <param name="scopeText"></param>
    public void BindAuthorizationReadScope(string scopeText)
    {
      // 清空缓存数据
      this.m_AuthorizationReadScopeObjectText = null;
      this.m_AuthorizationReadScopeObjectView = null;

      MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.AuthorizationReadScopeObjects, scopeText);
    }
    #endregion

    #region 属性:AuthorizationReadScopeObjects
    private IList<MembershipAuthorizationScopeObject> m_AuthorizationReadScopeObjects = null;

    /// <summary>权限：应用_通用_查看权限范围</summary>
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

    #region 属性:AuthorizationReadScopeObjectText
    private string m_AuthorizationReadScopeObjectText = null;

    /// <summary>权限：应用_通用_查看权限范围文本</summary>
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

    #region 属性:AuthorizationReadScopeObjectView
    private string m_AuthorizationReadScopeObjectView = null;

    /// <summary>权限：应用_通用_查看权限范围视图</summary>
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
    // 设置 EntityClass 标识
    // 

    #region 属性:EntityId
    /// <summary>实体对象标识</summary>
    public override string EntityId
    {
      get { return this.Id; }
    }
    #endregion
  }
}
