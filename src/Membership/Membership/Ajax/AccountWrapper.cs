#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Location.IPQuery;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public sealed class AccountWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IAccountService service = MembershipManagement.Instance.AccountService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            AccountInfo param = new AccountInfo();

            param = (AccountInfo)AjaxUtil.Deserialize(param, doc);

            string originalName = XmlHelper.Fetch("originalName", doc);

            string originalGlobalName = XmlHelper.Fetch("originalGlobalName", doc);

            string organizationText = XmlHelper.Fetch("organizationText", doc);

            string roleText = XmlHelper.Fetch("roleText", doc);

            string groupText = XmlHelper.Fetch("groupText", doc);

            if (string.IsNullOrEmpty(param.LoginName))
            {
                return "{message:{\"returnCode\":1,\"value\":\"登录名不能为空。\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // 新增

                if (this.service.IsExistGlobalName(param.GlobalName))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"此全局名称已存在。\"}}";
                }

                if (this.service.IsExistLoginNameAndGlobalName(param.LoginName, param.Name))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"此帐号或姓名已存在。\"}}";
                }

                param.Id = Guid.NewGuid().ToString();
            }
            else
            {
                // 修改

                if (param.GlobalName != originalGlobalName)
                {
                    if (this.service.IsExistGlobalName(param.GlobalName))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"此全局名称已存在。\"}}";
                    }
                }

                if (param.Name != originalName)
                {
                    if (this.service.IsExistName(param.Name))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"此姓名已存在。\"}}";
                    }
                }
            }

            param.ResetRelations("organization", organizationText);
            param.ResetRelations("role", roleText);
            param.ResetRelations("group", groupText);

            // 读取原始对象信息
            IAccountInfo originalObject = this.service.FindOne(param.Id);
            // 根据是否存在的对象，判断是否新建对象
            bool isNewObject = originalObject == null ? true : false;

            this.service.Save(param);

            // 如果没有默认, 则设置以排在第一个角色为默认角色。
            if (!MembershipManagement.Instance.RoleService.HasDefaultRelation(param.Id))
            {
                if (param.RoleRelations.Count > 0)
                {
                    MembershipManagement.Instance.MemberService.SetDefaultRole(param.Id, param.RoleRelations[0].RoleId);
                }
            }

            // 记录帐号操作日志
            string optionName = isNewObject ? "新建" : "编辑";

            IAccountInfo optionAccount = KernelContext.Current.User;

            string description = "【" + optionAccount.Name + "】" + optionName + "了帐号【" + param.Name + "】。";

            if (optionName == "编辑")
            {
                description = "【" + optionAccount.Name + "】编辑了用户【" + param.Name + "】的帐号信息。";
            }

            MembershipManagement.Instance.AccountLogService.Log(param.Id, optionName, originalObject, description, optionAccount.Id);

            return "{message:{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            // 读取原始对象信息
            IAccountInfo originalObject = this.service.FindOne(id);

            this.service.Delete(id);

            // 记录帐号操作日志
            string optionName = "删除";

            IAccountInfo optionAccount = KernelContext.Current.User;

            string description = "【" + optionAccount.Name + "】删除了帐号【" + originalObject.Name + "】。";

            MembershipManagement.Instance.AccountLogService.Log(originalObject.Id, optionName, originalObject, description, optionAccount.Id);

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取对象信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            IAccountInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindOneByLoginName(XmlDocument doc)
        /// <summary>获取对象信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        public string FindOneByLoginName(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string loginName = XmlHelper.Fetch("loginName", doc);

            IAccountInfo param = this.service.FindOneByLoginName(loginName);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>查询所有数据</summary>
        /// <returns>返回一个相关的实例列表.</returns>
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            IList<IAccountInfo> list = this.service.FindAll(whereClause);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllWithoutMemberInfo(XmlDocument doc)
        /// <summary>查询所有数据</summary>
        /// <returns>返回一个相关的实例列表.</returns>
        public string FindAllWithoutMemberInfo(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IAccountInfo> list = this.service.FindAllWithoutMemberInfo(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns>
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            // 设置当前用户权限
            if (XmlHelper.Fetch("su", doc) == "1")
            {
                paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            IList<IAccountInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            // 兼容 ExtJS 设置
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // string organizationId = XmlHelper.Fetch("organizationId", doc);

            IAccountInfo param = new AccountInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetPasswordStrength(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetPasswordStrength(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string loginName = XmlHelper.Fetch("loginName", doc);

            int result = MembershipManagement.Instance.AccountService.GetPasswordStrength(loginName);

            outString.Append("{\"data\":");

            if (result == 1)
            {
                outString.Append("{\"code\":1,\"message\":\"未知的错误信息。\"}");
            }
            else if (result == 2)
            {
                outString.Append("{\"code\":2,\"message\":\"密码中必须包含一个【0～9】数字。\"}");
            }
            else if (result == 3)
            {
                outString.Append("{\"code\":3,\"message\":\"密码中必须包含一个【A～Z或a～z】字符。\"}");
            }
            else if (result == 4)
            {
                outString.Append("{\"code\":4,\"message\":\"密码中必须包含一个【# $ @ !】特殊字符。\"}");
            }
            else if (result == 5)
            {
                outString.Append("{\"code\":5,\"message\":\"密码长度必须大于【" + MembershipConfigurationView.Instance.PasswordPolicyMinimumLength + "】'。\"}");
            }
            else if (result == 6)
            {
                outString.Append("{\"code\":6,\"message\":\"密码中相邻字符重复次数不能超过【" + MembershipConfigurationView.Instance.PasswordPolicyCharacterRepeatedTimes + "】次。\"}");
            }
            else if (result == 9)
            {
                outString.Append("{\"code\":9,\"message\":\"密码为默认密码，强烈建议修改为其他密码。\"}");
            }
            else
            {
                outString.Append("{\"code\":0,\"message\":\"此密码符合所有密码策略。\"}");
            }

            outString.Append(",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"密码强度检测成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:ChangeBasicInfo(XmlDocument doc)
        /// <summary></summary>
        public string ChangeBasicInfo(XmlDocument doc)
        {
            AccountInfo param = new AccountInfo();

            param = (AccountInfo)AjaxUtil.Deserialize(param, doc);

            // 检测帐号

            if (param.Name != (string)param.Properties["OriginalName"])
            {
                if (string.IsNullOrEmpty(param.LoginName) || this.service.IsExistName(param.Name))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"此姓名已存在。\"}}";
                }
            }

            // 插入数据 

            this.service.ChangeBasicInfo(param);

            return "{message:{\"returnCode\":0,\"value\":\"修改成功。\"}}";
        }
        #endregion

        #region 函数:ChangePassword(string xml)
        /// <summary></summary>
        public string ChangePassword(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            string loginName = XmlHelper.Fetch("loginName", doc);

            string password = XmlHelper.Fetch("password", doc);

            string originalPassword = XmlHelper.Fetch("originalPassword", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"未检测到当前用户的合法信息。\"}}";
            }

            if (optionAccount.LoginName == loginName)
            {
                int result = this.service.ChangePassword(loginName, password, originalPassword);

                if (result == 0)
                {
                    // 记录帐号操作日志
                    MembershipManagement.Instance.AccountLogService.Log(optionAccount.Id, "修改密码", "用户【" + optionAccount.Name + "】修改密码成功。");

                    return "{message:{\"returnCode\":0,\"value\":\"修改成功。\"}}";
                }
                else
                {
                    // 记录帐号操作日志
                    MembershipManagement.Instance.AccountLogService.Log(optionAccount.Id, "修改密码", "用户【" + optionAccount.Name + "】修改密码失败，【IP:" + IPQueryContext.GetClientIP() + "】。");

                    return "{message:{\"returnCode\":1,\"value\":\"修改失败, 用户或密码错误.\"}}";
                }
            }
            else
            {
                // 记录帐号操作日志
                MembershipManagement.Instance.AccountLogService.Log(account.Id, "修改密码", "【" + optionAccount.Name + "】尝试修改另一个用户【" + account.Name + "】的密码，【IP:" + IPQueryContext.GetClientIP() + "】。", optionAccount.Id);

                return "{message:{\"returnCode\":1,\"value\":\"修改失败, 用户或密码错误.\"}}";
            }
        }
        #endregion

        #region 函数:SetGlobalName(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string SetGlobalName(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            string globalName = XmlHelper.Fetch("globalName", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"未检测到当前用户的合法信息。\"}}";
            }

            int reuslt = this.service.SetGlobalName(id, globalName);

            MembershipManagement.Instance.AccountLogService.Log(account.Id, "设置全局名称", "【" + optionAccount.Name + "】将用户【" + account.Name + "】的全局名称设置为【" + globalName + "】，【IP:" + IPQueryContext.GetClientIP() + "】。", optionAccount.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置全局名称成功。\"}}";
        }
        #endregion

        #region 函数:SetPassword(XmlDocument doc)
        /// <summary>设置密码</summary>
        public string SetPassword(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            string password = XmlHelper.Fetch("password", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"未检测到当前用户的合法信息。\"}}";
            }

            this.service.SetPassword(id, password);

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "设置密码", "【" + optionAccount.Name + "】重新设置了用户【" + account.Name + "】的密码，【IP:" + IPQueryContext.GetClientIP() + "】。", optionAccount.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置密码成功。\"}}";
        }
        #endregion

        #region 函数:SetLoginName(XmlDocument doc)
        /// <summary></summary>
        public string SetLoginName(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            string loginName = XmlHelper.Fetch("loginName", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"未检测到当前用户的合法信息。\"}}";
            }

            if (string.IsNullOrEmpty(loginName))
            {
                return "{message:{\"returnCode\":1,\"value\":\"登录名不能为空。\"}}";
            }
            else if (!this.service.IsExistLoginName(loginName))
            {
                this.service.SetLoginName(id, loginName);

                // 记录帐号操作日志
                MembershipManagement.Instance.AccountLogService.Log(account.Id, "设置登录名", "【" + optionAccount.Name + "】将用户【" + account.Name + "】的登录名设置为【" + loginName + "】，【IP:" + IPQueryContext.GetClientIP() + "】。", optionAccount.Id);

                return "{message:{\"returnCode\":0,\"value\":\"设置登录名成功。\"}}";
            }
            else
            {
                return "{message:{\"returnCode\":1,\"value\":\"已存在此登录名。\"}}";
            }
        }
        #endregion

        #region 函数:SetStatus(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string SetStatus(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            int status = Convert.ToInt32(XmlHelper.Fetch("status", doc));

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"未检测到当前用户的合法信息。\"}}";
            }

            int reuslt = this.service.SetStatus(id, status);

            MembershipManagement.Instance.AccountLogService.Log(account.Id, "设置帐号状态", "【" + optionAccount.Name + "】将用户【" + account.Name + "】的状态设置为【" + (status == 0 ? "禁用" : "启用") + "】，【IP:" + IPQueryContext.GetClientIP() + "】。", optionAccount.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置帐号状态成功。\"}}";
        }
        #endregion

        #region 函数:GetAuthorizationObjects(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns>
        public string GetAuthorizationObjects(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"));

            int rowCount = -1;

            DataTable table = MembershipManagement.Instance.AuthorizationObjectService.Filter(paging.RowIndex, paging.PageSize, paging.WhereClause, paging.OrderBy, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":[");

            foreach (DataRow row in table.Rows)
            {
                outString.Append("{");
                outString.Append("\"authorizationObjectId\":\"" + StringHelper.ToSafeJson(row["AuthorizationObjectId"].ToString()) + "\",");
                outString.Append("\"authorizationObjectName\":\"" + StringHelper.ToSafeJson(row["AuthorizationObjectName"].ToString()) + "\",");
                outString.Append("\"authorizationObjectType\":\"" + StringHelper.ToSafeJson(row["AuthorizationObjectType"].ToString()) + "\",");
                outString.Append("\"accountId\":\"" + StringHelper.ToSafeJson(row["AccountId"].ToString()) + "\",");
                outString.Append("\"accountGlobalName\":\"" + StringHelper.ToSafeJson(row["AccountGlobalName"].ToString()) + "\",");
                outString.Append("\"accountLoginName\":\"" + StringHelper.ToSafeJson(row["AccountLoginName"].ToString()) + "\"},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"paging\":" + paging + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}