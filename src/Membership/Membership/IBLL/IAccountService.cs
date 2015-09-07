﻿namespace X3Platform.Membership.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform;
    using X3Platform.Spring;
    using X3Platform.Membership.Scope;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IAccountService")]
    public interface IAccountService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">帐号标识</param>
        /// <returns></returns>
        IAccountInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IAccount param)
        /// <summary>保存记录</summary>
        /// <param name="param">IAccount 实例详细信息</param>
        /// <returns>IAccount 实例详细信息</returns>
        IAccountInfo Save(IAccountInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">帐号标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">IAccount id号</param>
        /// <returns>返回一个 IAccount 实例的详细信息</returns>
        IAccountInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">帐号的全局名称</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        IAccountInfo FindOneByGlobalName(string globalName);
        #endregion

        #region 函数:FindOneByLoginName(string loginName)
        /// <summary>查询某条记录</summary>
        /// <param name="loginName">登录名</param>
        /// <returns>返回一个 IAccount 实例的详细信息</returns>
        IAccountInfo FindOneByLoginName(string loginName);
        #endregion

        #region 函数:FindOneByCertifiedTelephone(string certifiedTelephone)
        /// <summary>根据已验证的手机号查询某条记录</summary>
        /// <param name="certifiedTelephone">已验证的手机号</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        IAccountInfo FindOneByCertifiedTelephone(string certifiedTelephone);
        #endregion

        #region 函数:FindOneByCertifiedEmail(string certifiedEmail)
        /// <summary>根据已验证的邮箱地址查询某条记录</summary>
        /// <param name="certifiedEmail">已验证的邮箱地址</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        IAccountInfo FindOneByCertifiedEmail(string certifiedEmail);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByOrganizationId(string organizationId)
        /// <summary>查询某个组织下的所有相关帐号</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAllByOrganizationId(string organizationId);
        #endregion

        #region 函数:FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation)
        /// <summary>查询某个组织下的所有相关帐号</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="defaultOrganizationRelation">默认组织关系</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation);
        #endregion

        #region 函数:FindAllByRoleId(string roleId)
        /// <summary>查询某个角色下的所有相关帐号</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回一个<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAllByRoleId(string roleId);
        #endregion

        #region 函数:FindAllByGroupId(string groupId)
        /// <summary>查询某个群组下的所有相关帐号</summary>
        /// <param name="groupId">群组标识</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAllByGroupId(string groupId);
        #endregion

        #region 函数:FindAllWithoutMemberInfo(int length)
        /// <summary>返回所有没有成员信息的帐号信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindAllWithoutMemberInfo(int length);
        #endregion

        #region 函数:FindForwardLeaderAccountsByOrganizationId(string organizationId)
        /// <summary>返回所有正向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationId(string organizationId);
        #endregion

        #region 函数:FindForwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>返回所有正向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationId(string organizationId, int level);
        #endregion

        #region 函数:FindBackwardLeaderAccountsByOrganizationId(string organizationId)
        /// <summary>返回所有反向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationId(string organizationId);
        #endregion

        #region 函数:FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>返回所有反向领导的帐号信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IAccountInfo"/>实例的详细信息</returns>
        IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query,  out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表<see cref="IAccountInfo"/></returns>
        IList<IAccountInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录.</summary>
        /// <param name="id">帐号标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistLoginNameAndGlobalName(string loginName, string name);
        /// <summary>检测是否存在相关的记录,登录名和姓名两者都不能重复.</summary>
        /// <param name="loginName">登录名</param>
        /// <param name="name">姓名</param>
        /// <returns>布尔值</returns>
        bool IsExistLoginNameAndGlobalName(string loginName, string name);
        #endregion

        #region 函数:IsExistLoginName(string loginName)
        /// <summary>检测是否存在相关的记录, 用户中心, 登录名不能重复. [添加帐号]</summary>
        /// <param name="loginName">登录名</param>
        /// <returns>布尔值</returns>
        bool IsExistLoginName(string loginName);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录, 由于在同一个OU下面,所以姓名不能重复. 修改姓名时</summary>
        /// <param name="nickName">姓名</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string nickName);
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">组织单位全局名称</param>
        /// <returns>布尔值</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 函数:IsExistCertifiedTelephone(string certifiedTelephone)
        /// <summary>检测是否存在相关的手机号</summary>
        /// <param name="certifiedTelephone">已验证的手机号</param>
        /// <returns>布尔值</returns>
        bool IsExistCertifiedTelephone(string certifiedTelephone);
        #endregion

        #region 函数:IsExistCertifiedEmail(string certifiedEmail)
        /// <summary>检测是否存在相关的邮箱</summary>
        /// <param name="certifiedEmail">已验证的邮箱</param>
        /// <returns>布尔值</returns>
        bool IsExistCertifiedEmail(string certifiedEmail);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">帐号标识</param>
        /// <param name="name">帐号名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:CreateEmptyAccount(string accountId)
        /// <summary>创建空的帐号信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns></returns>
        IAccountInfo CreateEmptyAccount(string accountId);
        #endregion

        #region 函数:CombineDistinguishedName(string globalName)
        /// <summary>组合唯一名称</summary>
        /// <param name="globalName">帐号全局名称</param>
        /// <returns></returns>
        string CombineDistinguishedName(string globalName);
        #endregion

        // -------------------------------------------------------
        // 管理员功能
        // -------------------------------------------------------

        #region 函数:SetGlobalName(string accountId, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetGlobalName(string accountId, string globalName);
        #endregion

        #region 函数:GetPassword(string loginName)
        /// <summary>获取密码</summary>
        /// <param name="loginName">帐号</param>
        string GetPassword(string loginName);
        #endregion

        #region 函数:GetPasswordStrength(string loginName)
        /// <summary>获取密码强度</summary>
        /// <param name="loginName">帐号</param>
        /// <returns>0 密码强度正常 | 1 密码强度低 | 2 密码长度不符合规范 | 3 密码为存数字 | 9 密码为默认密码</returns>
        int GetPasswordStrength(string loginName);
        #endregion

        #region 函数:GetPasswordChangedDate(string loginName)
        /// <summary>获取密码更新时间</summary>
        /// <param name="loginName">帐号</param>
        DateTime GetPasswordChangedDate(string loginName);
        #endregion

        #region 函数:SetPassword(string accountId, string password)
        /// <summary>设置帐号密码.(管理员)</summary>
        /// <param name="accountId">编号</param>
        /// <param name="password">密码</param>
        /// <returns>修改成功, 返回 0, 旧密码不匹配, 返回 1.</returns>
        int SetPassword(string accountId, string password);
        #endregion

        #region 函数:SetLoginName(string accountId, string loginName)
        /// <summary>设置登录名</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="loginName">登录名</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetLoginName(string accountId, string loginName);
        #endregion

        #region 函数:SetCertifiedTelephone(string accountId, string telephone)
        /// <summary>设置已验证的联系电话</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="telephone">联系电话</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetCertifiedTelephone(string accountId, string telephone);
        #endregion

        #region 函数:SetCertifiedEmail(string accountId, string email)
        /// <summary>设置已验证的邮箱</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="email">邮箱</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetCertifiedEmail(string accountId, string email);
        #endregion

        #region 函数:SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        /// <summary>设置已验证的头像</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="avatarVirtualPath">头像的虚拟路径</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetCertifiedAvatar(string accountId, string avatarVirtualPath);
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        int SetExchangeStatus(string accountId, int status);
        #endregion

        #region 函数:SetStatus(string accountId, int status)
        /// <summary>设置用户状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        int SetStatus(string accountId, int status);
        #endregion

        #region 函数:SetIPAndLoginDate(string accountId, string ip, string loginDate)
        /// <summary>设置登录名</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="ip">登录名</param>
        /// <param name="loginDate">登录时间</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetIPAndLoginDate(string accountId, string ip, string loginDate);
        #endregion

        // -------------------------------------------------------
        // 普通用户功能
        // -------------------------------------------------------

        #region 函数:ValidatePasswordPolicy(string password)
        /// <summary>校验密码是否符合密码策略</summary>
        /// <param name="password">密码</param>
        /// <returns>0 表示成功 1</returns>
        int ValidatePasswordPolicy(string password);
        #endregion

        #region 函数:ConfirmPassword(string accountId, string passwordType, string password)
        /// <summary>确认密码</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="passwordType">密码类型: default 默认, query 查询密码, trader 交易密码</param>
        /// <param name="password">密码</param>
        /// <returns>返回值: 0 成功 | 1 失败</returns>
        int ConfirmPassword(string accountId, string passwordType, string password);
        #endregion

        #region 函数:LoginCheck(string loginName, string password)
        /// <summary>登陆检测</summary>
        /// <param name="loginName">帐号</param>
        /// <param name="password">密码</param>
        /// <returns>IAccount 实例</returns>
        IAccountInfo LoginCheck(string loginName, string password);
        #endregion

        #region 函数:ChangeBasicInfo(IAccount param)
        /// <summary>修改基本信息</summary>
        /// <param name="param">IAccount 实例的详细信息</param>
        void ChangeBasicInfo(IAccountInfo param);
        #endregion

        #region 函数:ChangePassword(string loginName, string password, string originalPassword)
        /// <summary>修改密码</summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">新密码</param>
        /// <param name="originalPassword">原始密码</param>
        /// <returns>修改成功, 返回 0, 旧密码不匹配, 返回 1.</returns>
        int ChangePassword(string loginName, string password, string originalPassword);
        #endregion

        #region 函数:RefreshUpdateDate(string accountId)
        /// <summary>刷新帐号的更新时间</summary>
        /// <param name="accountId">帐户标识</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        int RefreshUpdateDate(string accountId);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(IAccount account)
        /// <summary>获取帐号相关的权限对象</summary>
        /// <param name="account">IAccount 实例的详细信息</param>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(IAccountInfo account);
        #endregion

        #region 函数:SyncToActiveDirectory(IAccountInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">帐号信息</param>
        int SyncToActiveDirectory(IAccountInfo param);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(IAccountInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">帐号信息</param>
        int SyncFromPackPage(IAccountInfo param);
        #endregion
    }
}
