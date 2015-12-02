namespace X3Platform.Membership.IDAL
{
    using System;
    using System.Collections.Generic;
    using X3Platform.Membership;
    using X3Platform.Security.Authority;
    using X3Platform.Spring;
    using System.Data;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IRoleProvider")]
    public interface IRoleProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IAccount param)
        /// <summary>保存记录</summary>
        /// <param name="param">IAccount 实例详细信息</param>
        /// <returns>IAccount 实例详细信息</returns>
        IRoleInfo Save(IRoleInfo param);
        #endregion

        #region 函数:Insert(IAccount param)
        /// <summary>添加记录</summary>
        /// <param name="param">IAccount 实例的详细信息</param>
        void Insert(IRoleInfo param);
        #endregion

        #region 函数:Update(IAccount param)
        /// <summary>修改记录</summary>
        /// <param name="param">IAccount 实例的详细信息</param>
        void Update(IRoleInfo param);
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
        /// <param name="id">IAccount id号</param>
        /// <returns>返回一个 IAccount 实例的详细信息</returns>
        IRoleInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">角色的全局名称</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        IRoleInfo FindOneByGlobalName(string globalName);
        #endregion

        #region 函数:FindOneByCorporationIdAndStandardRoleId(string corporationId, string standardRoleId)
        /// <summary>递归查询某个公司下面对应某标准角色相对应的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        IRoleInfo FindOneByCorporationIdAndStandardRoleId(string corporationId, string standardRoleId);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="IRoleInfo" />实例的详细信息</returns>
        IList<IRoleInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        IList<IRoleInfo> FindAllByParentId(string parentId);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        IList<IRoleInfo> FindAllByAccountId(string accountId);
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId)
        /// <summary>查询某个组织下面所有的角色</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        IList<IRoleInfo> FindAllByOrganizationUnitId(string organizationId);
        #endregion

        #region 函数:FindAllByGeneralRoleId(string generalRoleId)
        /// <summary>递归查询某个公司下面所有的角色</summary>
        /// <param name="generalRoleId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllByGeneralRoleId(string generalRoleId);
        #endregion

        #region 函数:FindAllByStandardOrganizationUnitId(string standardOrganizationUnitId)
        /// <summary>递归查询某个标准组织下面所有的角色</summary>
        /// <param name="standardOrganizationUnitId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllByStandardOrganizationUnitId(string standardOrganizationUnitId);
        #endregion

        #region 函数:FindAllByStandardRoleId(string standardRoleId)
        /// <summary>递归查询某个标准角色下面所有的角色</summary>
        /// <param name="standardRoleId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllByStandardRoleId(string standardRoleId);
        #endregion

        #region 函数:FindAllByOrganizationUnitIdAndJobId(string organizationId, string jobId)
        /// <summary>递归查询某个组织下面相关的职位对应的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="jobId">职位标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllByOrganizationUnitIdAndJobId(string organizationId, string jobId);
        #endregion

        #region 函数:FindAllByAssignedJobId(string assignedJobId)
        /// <summary>递归查询某个组织下面相关的岗位对应的角色信息</summary>
        /// <param name="assignedJobId">岗位标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllByAssignedJobId(string assignedJobId);
        #endregion

        #region 函数:FindAllByCorporationIdAndStandardRoleIds(string corporationId, string standardRoleIds)
        /// <summary>递归查询某个公司下面对应某标准角色相对应的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <param name="standardRoleIds">标准角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllByCorporationIdAndStandardRoleIds(string corporationId, string standardRoleIds);
        #endregion

        #region 函数:FindAllBetweenPriority(string corporationId, string standardRoleIds)
        /// <summary>根据某个组织标识查询此组织上下级之间属于某一范围权重值的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="minPriority">最小权重值</param>
        /// <param name="maxPriority">最大权重值</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllBetweenPriority(string organizationId, int minPriority, int maxPriority);
        #endregion

        #region 函数:FindAllWithoutMember(int length, bool includeAllRole)
        /// <summary>返回所有没有成员信息的角色信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <param name="includeAllRole">包含全部角色</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindAllWithoutMember(int length, bool includeAllRole);
        #endregion

        #region 函数:FindForwardLeadersByOrganizationUnitId(string organizationId, int level)
        /// <summary>返回所有正向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindForwardLeadersByOrganizationUnitId(string organizationId, int level);
        #endregion

        #region 函数:FindBackwardLeadersByOrganizationUnitId(string organizationId, int level)
        /// <summary>返回所有反向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindBackwardLeadersByOrganizationUnitId(string organizationId, int level);
        #endregion

        #region 函数:FindStandardGeneralRolesByOrganizationUnitId(string organizationId, string standardGeneralRoleId)
        /// <summary>返回所有父级对象为标准通用角色标识【standardGeneralRoleId】的相关角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        IList<IRoleInfo> FindStandardGeneralRolesByOrganizationUnitId(string organizationId, string standardGeneralRoleId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        IList<IRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录.</summary>
        /// <param name="id">帐号标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录.</summary>
        /// <param name="name">角色名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">角色全局名称</param>
        /// <returns>布尔值</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">角色标识</param>
        /// <param name="name">角色名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">角色标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetGlobalName(string id, string globalName);
        #endregion

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">角色标识</param>
        /// <param name="parentId">父级角色标识</param>
        /// <returns>0:成功 | 1:失败</returns>
        int SetParentId(string id, string parentId);
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        int SetExchangeStatus(string accountId, int status);
        #endregion

        #region 函数:GetAuthorities(string roleId)
        /// <summary>获取角色的权限</summary>
        /// <param name="roleId">角色标识</param>
        IList<AuthorityInfo> GetAuthorities(string roleId);
        #endregion

        #region 函数:SyncFromPackPage(IRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">角色信息</param>
        int SyncFromPackPage(IRoleInfo param);
        #endregion

        // -------------------------------------------------------
        // 设置帐号和角色关系
        // -------------------------------------------------------

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        IList<IAccountRoleRelationInfo> FindAllRelationByAccountId(string accountId);
        #endregion

        #region 函数:FindAllRelationByRoleId(string roleId)
        /// <summary>根据角色查询相关帐号的关系</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        IList<IAccountRoleRelationInfo> FindAllRelationByRoleId(string roleId);
        #endregion

        #region 函数:AddRelation(string accountId, string roleId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        /// <param name="isDefault">是否是默认角色</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        int AddRelation(string accountId, string roleId, bool isDefault, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:ExtendRelation(string accountId, string roleId, DateTime endDate)
        /// <summary>续约帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        /// <param name="endDate">新的截止时间</param>
        int ExtendRelation(string accountId, string roleId, DateTime endDate);
        #endregion

        #region 函数:RemoveRelation(string accountId, string roleId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        int RemoveRelation(string accountId, string roleId);
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关角色的默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveDefaultRelation(string accountId);
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关角色的非默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveNondefaultRelation(string accountId);
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的角色关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveExpiredRelation(string accountId);
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关角色的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveAllRelation(string accountId);
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号是否有默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        bool HasDefaultRelation(string accountId);
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string roleId)
        /// <summary>设置帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        int SetDefaultRelation(string accountId, string roleId);
        #endregion

        #region 函数:ClearupRelation(string roleId)
        /// <summary>清理角色与帐号的关系</summary>
        /// <param name="roleId">角色标识</param>
        int ClearupRelation(string roleId);
        #endregion
    }
}
