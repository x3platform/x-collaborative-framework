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

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IBugCommentProvider>(typeof(IBugCommentProvider));
    }

    public BugCommentInfo this[string index]
    {
      get { return this.FindOne(index); }
    }

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(AccountInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">BugCommentInfo 实例详细信息</param>
    /// <param name="message">数据库操作返回的相关信息</param>
    /// <returns>BugCommentInfo 实例详细信息</returns>
    public BugCommentInfo Save(BugCommentInfo param)
    {
      if (string.IsNullOrEmpty(param.Id)) { throw new Exception("实例标识不能为空。"); }

      if (string.IsNullOrEmpty(param.BugId)) { throw new Exception("所属的问题标识不能为空。"); }

      bool isNewObject = !this.IsExist(param.Id);

      if (isNewObject)
      {
        IAccountInfo account = KernelContext.Current.User;

        param.AccountId = account.Id;
      }

      this.provider.Save(param);

      param = this.FindOne(param.Id);

      // 发送邮件提醒
      // 新反馈发送给提交人
      if (BugConfigurationView.Instance.SendMailAlert == "ON" && isNewObject)
      {
        string mailTo, mailSubject, mailBody;

        IMemberInfo member = MembershipManagement.Instance.MemberService.FindOneByAccountId(param.Bug.AccountId);

        if (member != null && !string.IsNullOrEmpty(member.Email))
        {
          VelocityContext context = new VelocityContext();

          // 加载当前实体数据信息
          context.Put("kernelConfiguration", KernelConfigurationView.Instance);
          // 加载当前实体数据信息
          context.Put("param", param);

          mailTo = member.Email;

          mailSubject = "您提交的问题【" + param.Bug.Title + "】有一个新的反馈。";

          mailBody = VelocityManager.Instance.ParseTemplateVirtualPath(context, "/resources/email/Bug/bugzilla-comment-reply-mail.vm");

          EmailClientContext.Instance.Send(mailTo, mailSubject, mailBody, EmailFormat.Html);
        }
      }

      return this.provider.Save(param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="keys">标识,多个以逗号隔开</param>
    public void Delete(string id)
    {
      this.provider.Delete(id);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(int id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">AccountInfo Id号</param>
    /// <returns>返回一个 AccountInfo 实例的详细信息</returns>
    public BugCommentInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
    public IList<BugCommentInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
    public IList<BugCommentInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
    public IList<BugCommentInfo> FindAll(string whereClause, int length)
    {
      return this.provider.FindAll(whereClause, length);
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例</returns> 
    public IList<BugCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">会员标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      return this.provider.IsExist(id);
    }
    #endregion

    // -------------------------------------------------------
    // 权限
    // -------------------------------------------------------

    #region 函数:GetAuthorizationObject(BugCommentInfo param)
    /// <summary>验证对象的权限</summary>
    /// <param name="param">需验证的对象</param>
    /// <returns>对象</returns>
    private BugCommentInfo GetAuthorizationObject(BugCommentInfo param)
    {
      return param;
    }
    #endregion
  }
}
