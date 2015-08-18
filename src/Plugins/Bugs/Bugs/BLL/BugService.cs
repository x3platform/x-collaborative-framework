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
      // 创建对象构建器(Spring.NET)
      string springObjectFile = BugConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IBugProvider>(typeof(IBugProvider));
    }

    public BugInfo this[string id]
    {
      get { return this.FindOne(id); }
    }

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(BugInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">BugInfo 实例详细信息</param>
    /// <param name="message">数据库操作返回的相关信息</param>
    /// <returns>BugInfo 实例详细信息</returns>
    public BugInfo Save(BugInfo param)
    {
      if (string.IsNullOrEmpty(param.Id)) { throw new Exception("实例标识不能为空。"); }

      bool isNewObject = !this.IsExist(param.Id);

      BugInfo originalObject = isNewObject ? null : this.FindOne(param.Id);

      string methodName = isNewObject ? "新增" : "编辑";

      if (isNewObject)
      {
        IAccountInfo account = KernelContext.Current.User;

        param.AccountId = account.Id;
        param.AccountName = account.Name;
      }

      // 指派问题负责人
      if (string.IsNullOrEmpty(param.AssignToAccountId))
      {
        param.AssignToAccountName = string.Empty;
      }
      else
      {
        param.AssignToAccountName = MembershipManagement.Instance.AccountService[param.AssignToAccountId].Name;
      }

      // 生成问题编号
      if (string.IsNullOrEmpty(param.Code))
      {
        string code = string.Empty;

        // 所属类别信息
        ApplicationInfo application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

        // 编号的相关实体数据表
        string entityTableName = BugConfigurationView.Instance.DigitalNumberEntityTableName;

        // 编号的前缀编码
        string prefixCode = BugConfigurationView.Instance.DigitalNumberPrefixCodeRule
                                .Replace("{ApplicationPinYin}", application.PinYin);

        // 编号的自增流水号长度
        int incrementCodeLength = BugConfigurationView.Instance.DigitalNumberIncrementCodeLength;

        GenericSqlCommand command = this.provider.CreateGenericSqlCommand();

        code = DigitalNumberContext.GenerateDateCodeByPrefixCode(command, entityTableName, prefixCode, incrementCodeLength);

        param.Code = code.ToUpper();
      }

      // 过滤 Cross Site Script
      param = StringHelper.ToSafeXSS<BugInfo>(param);

      this.provider.Save(param);

      param = this.FindOne(param.Id);

      if (isNewObject) { originalObject = param; }

      // 记录问题状态信息
      BugHistoryInfo history = new BugHistoryInfo();

      history.Id = StringHelper.ToGuid();
      history.BugId = param.Id;

      if (isNewObject)
      {
        // 0.新问题 | 1.确认中 | 2.处理中 | 3.已解决 | 4.已关闭
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

      // 发送邮件提醒
      // 新问题状态发送给负责人, 已解决状态发送给提交人
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
            // 已解决的问题, 发邮件通知提交人
            member = MembershipManagement.Instance.MemberService.FindOneByAccountId(param.AccountId);
          }
          else
          {
            member = MembershipManagement.Instance.MemberService.FindOneByAccountId(param.AssignToAccountId);
          }

          if (member != null && !string.IsNullOrEmpty(member.Email))
          {
            VelocityContext context = new VelocityContext();

            // 加载当前实体数据信息
            context.Put("kernelConfiguration", KernelConfigurationView.Instance);
            // 加载当前实体数据信息
            context.Put("param", param);

            mailTo = member.Email;

            mailSubject = mailBody = string.Empty;

            if (param.Status == 0)
            {
              // 0.新的问题
              mailSubject = "您有一个新问题【" + param.Title + "】，等待处理。";

              mailBody = VelocityManager.Instance.ParseTemplateVirtualPath(context, "/resources/email/Bug/bugzilla-new-mail.vm");
            }
            else if (param.Status == 3)
            {
              // 3.已解决
              mailSubject = param.AssignToAccountId + "已将问题【" + param.Title + "】解决，请确认。";

              mailBody = VelocityManager.Instance.ParseTemplateVirtualPath(context, "/resources/email/Bug/bugzilla-resolved-mail.vm");
            }

            EmailClientContext.Instance.Send(mailTo, mailSubject, mailBody, EmailFormat.Html);
          }
        }
      }

      return param;
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="keys">标识,多个以逗号隔开</param>
    public int Delete(string ids)
    {
      return this.provider.Delete(ids);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">问题标识</param>
    /// <returns>返回一个实例<see cref="BugInfo"/>的详细信息</returns>
    public BugInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region 函数:FindOneByCode(string code)
    /// <summary>查询某条记录</summary>
    /// <param name="code">问题编号</param>
    /// <returns>返回一个 BugInfo 实例的详细信息</returns>
    public BugInfo FindOneByCode(string code)
    {
      return this.provider.FindOneByCode(code);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="BugInfo"/>的详细信息</returns>
    public IList<BugInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="BugInfo"/>的详细信息</returns>
    public IList<BugInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="BugInfo"/>的详细信息</returns>
    public IList<BugInfo> FindAll(string whereClause, int length)
    {
      // 验证管理员身份
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
    public IList<BugInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      // 验证管理员身份
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

    #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例</returns>
    public IList<BugQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      // 验证管理员身份
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

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">会员标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      return this.provider.IsExist(id);
    }
    #endregion

    #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
    /// <summary>查询实体对象的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <returns></returns>
    public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
    {
      return this.provider.GetAuthorizationScopeObjects(entityId, authorityName);
    }
    #endregion

    // -------------------------------------------------------
    // 权限设置
    // -------------------------------------------------------

    #region 函数:HasAuthorizationReadObject(BugInfo param)
    /// <summary>验证对象的权限</summary>
    /// <param name="param">需验证的对象</param>
    /// <returns>对象</returns>
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

    #region 函数:BindAuthorizationScopeSQL(string whereClause)
    /// <summary>绑定SQL查询条件</summary>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <returns></returns>
    private string BindAuthorizationScopeSQL(string whereClause)
    {
      string tablePrefix = BugConfigurationView.Instance.ProjectDataTablePrefix;

      // 项目的成员 也可以看到此问题的信息
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
