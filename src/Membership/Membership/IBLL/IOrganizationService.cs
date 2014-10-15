// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Spring;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IOrganizationService")]
    public interface IOrganizationService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">组织标识</param>
        /// <returns></returns>
        IOrganizationInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IOrganization param)
        /// <summary>保存记录</summary>
        /// <param name="param">IOrganization 实例详细信息</param>
        /// <returns>IOrganization 实例详细信息</returns>
        IOrganizationInfo Save(IOrganizationInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">组织单位标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        IOrganizationInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">组织的全局名称</param>
        /// <returns>返回一个<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IOrganizationInfo FindOneByGlobalName(string globalName);
        #endregion

        #region 函数:FindOneByRoleId(string roleId)
        /// <summary>查询某个角色所属的组织信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IOrganizationInfo FindOneByRoleId(string roleId);
        #endregion

        #region 函数:FindOneByRoleId(string roleId, int level)
        /// <summary>查询某个角色所属的某一级次的组织信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IOrganizationInfo FindOneByRoleId(string roleId, int level);
        #endregion

        #region 函数:FindCorporationByOrganizationId(string id)
        /// <summary>查询某个组织所属的公司信息</summary>
        /// <param name="id">组织标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IOrganizationInfo FindCorporationByOrganizationId(string id);
        #endregion

        #region 函数:FindDepartmentByOrganizationId(string organizationId, int level)
        /// <summary>查询某个组织的所属某个上级部门信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IOrganizationInfo FindDepartmentByOrganizationId(string organizationId, int level);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有<see cref="IOrganizationInfo" />实例的详细信息</returns>
        IList<IOrganizationInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有<see cref="IOrganizationInfo" />实例的详细信息</returns>
        IList<IOrganizationInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="IOrganizationInfo" />实例的详细信息</returns>
        IList<IOrganizationInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IList<IOrganizationInfo> FindAllByParentId(string parentId);
        #endregion

        #region 函数:FindAllByParentId(string parentId, int depth)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <param name="depth">深入获取的层次，0表示只获取本层次，-1表示全部获取</param>
        /// <returns>返回所有实例<see cref="IOrganizationInfo"/>的详细信息</returns>
        IList<IOrganizationInfo> FindAllByParentId(string parentId, int depth);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户所在的所有组织单位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        IList<IOrganizationInfo> FindAllByAccountId(string accountId);
        #endregion

        #region 函数:FindAllByRoleIds(string roleIds)
        /// <summary>查询某个角色的所属相关组织</summary>
        /// <param name="roleIds">角色标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IList<IOrganizationInfo> FindAllByRoleIds(string roleIds);
        #endregion

        #region 函数:FindAllByRoleIds(string roleIds, int level)
        /// <summary>查询某个角色的所属相关组织</summary>
        /// <param name="roleIds">角色标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IList<IOrganizationInfo> FindAllByRoleIds(string roleIds, int level);
        #endregion

        #region 函数:FindAllByCorporationId(string corporationId)
        /// <summary>递归查询某个公司下面所有的组织</summary>
        /// <param name="corporationId">公司标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IList<IOrganizationInfo> FindAllByCorporationId(string corporationId);
        #endregion

        #region 函数:FindAllByProjectId(string projectId)
        /// <summary>递归查询某个项目下面所有的组织</summary>
        /// <param name="projectId">项目标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IList<IOrganizationInfo> FindAllByProjectId(string projectId);
        #endregion

        #region 函数:FindCorporationsByAccountId(string accountId)
        /// <summary>查询某个帐户所属的所有公司信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        IList<IOrganizationInfo> FindCorporationsByAccountId(string accountId);
        #endregion
     
        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        IList<IOrganizationInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">组织单位标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">组织单位名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">组织单位全局名称</param>
        /// <returns>布尔值</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">组织标识</param>
        /// <param name="name">组织名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:CombineFullPath(string name, string parentId)
        /// <summary>组合全路径</summary>
        /// <param name="name">组织名称</param>
        /// <param name="parentId">父级对象标识</param>
        /// <returns></returns>
        string CombineFullPath(string name, string parentId);
        #endregion

        #region 函数:GetOrganizationPathByOrganizationId(string organizationId)
        /// <summary>根据组织标识计算组织的全路径</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns></returns>
        string GetOrganizationPathByOrganizationId(string organizationId);
        #endregion

        #region 函数:CombineDistinguishedName(string globalName, string id)
        /// <summary>组合唯一名称</summary>
        /// <param name="globalName">组织全局名称</param>
        /// <param name="id">对象标识</param>
        /// <returns></returns>
        string CombineDistinguishedName(string globalName, string id);
        #endregion

        #region 函数:GetActiveDirectoryOUPathByOrganizationId(string organizationId)
        /// <summary>根据组织标识计算 Active Directory OU 路径</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns></returns>
        string GetActiveDirectoryOUPathByOrganizationId(string organizationId);
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">组织标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetGlobalName(string id, string globalName);
        #endregion

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">组织标识</param>
        /// <param name="parentId">父级组织标识</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetParentId(string id, string parentId);
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        int SetExchangeStatus(string accountId, int status);
        #endregion

        #region 函数:GetChildNodes(string organizationId)
        /// <summary>获取组织的子成员</summary>
        /// <param name="organizationId">组织单位标识</param>
        IList<IAuthorizationObject> GetChildNodes(string organizationId);
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:SyncToActiveDirectory(IOrganizationInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">组织信息</param>
        int SyncToActiveDirectory(IOrganizationInfo param);
        #endregion

        #region 函数:SyncFromPackPage(IOrganizationInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">组织单位信息</param>
        int SyncFromPackPage(IOrganizationInfo param);
        #endregion

        // -------------------------------------------------------
        // 设置帐号和组织关系
        // -------------------------------------------------------

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, OrganizationId, IsDefault, BeginDate, EndDate</returns>
        IList<IAccountOrganizationRelationInfo> FindAllRelationByAccountId(string accountId);
        #endregion

        #region 函数:FindAllRelationByRoleId(string organizationId)
        /// <summary>根据组织查询相关帐号的关系</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>Table Columns：AccountId, OrganizationId, IsDefault, BeginDate, EndDate</returns>
        IList<IAccountOrganizationRelationInfo> FindAllRelationByRoleId(string organizationId);
        #endregion

        #region 函数:AddRelation(string accountId, string organizationId)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        int AddRelation(string accountId, string organizationId);
        #endregion

        #region 函数:AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="isDefault">是否是默认组织</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        int AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:AddRelationRange(string accountIds, string organizationId)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountIds">帐号标识，多个以逗号隔开</param>
        /// <param name="organizationId">组织标识</param>
        int AddRelationRange(string accountIds, string organizationId);
        #endregion

        #region 函数:AddParentRelations(string accountId, string organizationId)
        /// <summary>添加帐号与相关组织的父级组织关系(递归)</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        int AddParentRelations(string accountId, string organizationId);
        #endregion

        #region 函数:ExtendRelation(string accountId, string organizationId, DateTime endDate)
        /// <summary>续约帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="endDate">新的截止时间</param>
        int ExtendRelation(string accountId, string organizationId, DateTime endDate);
        #endregion

        #region 函数:RemoveRelation(string accountId, string organizationId)
        /// <summary>移除帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        int RemoveRelation(string accountId, string organizationId);
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关组织的默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveDefaultRelation(string accountId);
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关组织的非默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveNondefaultRelation(string accountId);
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的组织关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveExpiredRelation(string accountId);
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关组织的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveAllRelation(string accountId);
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号是否有默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        bool HasDefaultRelation(string accountId);
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string organizationId)
        /// <summary>设置帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        int SetDefaultRelation(string accountId, string organizationId);
        #endregion

        #region 函数:ClearupRelation(string organizationId)
        /// <summary>清理组织与帐号的关系</summary>
        /// <param name="organizationId">组织标识</param>
        int ClearupRelation(string organizationId);
        #endregion
    }
}
