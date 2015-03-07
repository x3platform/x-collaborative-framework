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

namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.ActiveDirectory;
    using X3Platform.ActiveDirectory.Configuration;
    using X3Platform.Configuration;
    using X3Platform.Security.Authority;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    public class RoleService : IRoleService
    {
        private MembershipConfiguration configuration = null;

        private IRoleProvider provider = null;

        /// <summary></summary>
        public RoleService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IRoleProvider>(typeof(IRoleProvider));
        }

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">角色标识</param>
        /// <returns></returns>
        public IRoleInfo this[string id]
        {
            get { return provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(AccountInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">AccountInfo 实例详细信息</param>
        /// <returns>AccountInfo 实例详细信息</returns>
        public IRoleInfo Save(IRoleInfo param)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IRoleInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                {
                    originalObject = param;
                }

                this.SyncToActiveDirectory(param, originalObject.GlobalName, originalObject.OrganizationId);
            }

            // 设置组织全路径
            param.FullPath = this.CombineFullPath(param.Name, param.OrganizationId);

            // 设置唯一识别名称
            param.DistinguishedName = this.CombineDistinguishedName(param.Name, param.OrganizationId);

            param = provider.Save(param);

            if (param != null)
            {
                string roleId = param.Id;

                // 绑定新的关系
                if (!string.IsNullOrEmpty(roleId))
                {
                    // 1.移除非默认成员关系
                    MembershipManagement.Instance.RoleService.ClearupRelation(roleId);

                    // 2.设置新的关系
                    foreach (IAccountInfo item in param.Members)
                    {
                        MembershipManagement.Instance.RoleService.AddRelation(item.Id, roleId);
                    }
                }
            }

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
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
        public IRoleInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">角色的全局名称</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IRoleInfo FindOneByGlobalName(string globalName)
        {
            return provider.FindOneByGlobalName(globalName);
        }
        #endregion

        #region 函数:FindOneByCorporationIdAndStandardRoleId(string corporationId, string standardRoleId)
        /// <summary>递归查询某个公司下面对应某标准角色相对应的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IRoleInfo FindOneByCorporationIdAndStandardRoleId(string corporationId, string standardRoleId)
        {
            return provider.FindOneByCorporationIdAndStandardRoleId(corporationId, standardRoleId);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAll()
        {
            return provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAll(string whereClause)
        {
            return provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByParentId(string parentId)
        {
            return FindAllByParentId(parentId, 0);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId, int depth)
        /// <summary>查询某个组织节点下的所有角色信息</summary>
        /// <param name="parentId">父节标识</param>
        /// <param name="depth">深入获取的层次，0表示只获取本层次，-1表示全部获取</param>
        /// <returns>返回所有实例<see cref="IOrganizationInfo"/>的详细信息</returns>
        public IList<IRoleInfo> FindAllByParentId(string parentId, int depth)
        {
            // 结果列表
            List<IRoleInfo> list = new List<IRoleInfo>();

            //
            // 查找组织子部门的角色信息
            //

            IList<IRoleInfo> roles = provider.FindAllByParentId(parentId);

            list.AddRange(roles);

            if (depth == -1)
            {
                depth = int.MaxValue;
            }

            if (roles.Count > 0 && depth > 0)
            {
                foreach (IRoleInfo role in roles)
                {
                    list.AddRange(FindAllByParentId(role.Id, (depth - 1)));
                }
            }

            return list;
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">AccountInfo Id号</param>
        /// <returns>返回一个 AccountInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByAccountId(string accountId)
        {
            return provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllByOrganizationId(string organizationId)
        /// <summary>查询某个组织下的所有角色</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByOrganizationId(string organizationId)
        {
            return this.FindAllByOrganizationId(organizationId, 0);
        }
        #endregion

        #region 函数:FindAllByOrganizationId(string organizationId, int depth)
        /// <summary>查询某个组织节点下的所有角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="depth">深入获取的层次，0表示只获取本层次，-1表示全部获取</param>
        /// <returns>返回所有实例<see cref="IOrganizationInfo"/>的详细信息</returns>
        public IList<IRoleInfo> FindAllByOrganizationId(string organizationId, int depth)
        {
            // 结果列表
            List<IRoleInfo> list = new List<IRoleInfo>();

            // -------------------------------------------------------
            // 查找组织子部门的角色信息
            // -------------------------------------------------------

            IList<IOrganizationInfo> organizations = MembershipManagement.Instance.OrganizationService.FindAllByParentId(organizationId);

            // -------------------------------------------------------
            // 查找角色信息
            // -------------------------------------------------------

            list.AddRange(this.provider.FindAllByOrganizationId(organizationId));

            if (depth == -1)
            {
                depth = int.MaxValue;
            }

            if (organizations.Count > 0 && depth > 0)
            {
                foreach (IOrganizationInfo organization in organizations)
                {
                    list.AddRange(FindAllByOrganizationId(organization.Id, (depth - 1)));
                }
            }

            return list;
        }
        #endregion

        #region 函数:FindAllByGeneralRoleId(string generalRoleId)
        /// <summary>递归查询某个公司下面所有的角色</summary>
        /// <param name="generalRoleId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByGeneralRoleId(string generalRoleId)
        {
            return provider.FindAllByGeneralRoleId(generalRoleId);
        }
        #endregion

        #region 函数:FindAllByStandardOrganizationId(string standardOrganizationId)
        /// <summary>递归查询某个标准组织下面所有的角色</summary>
        /// <param name="standardOrganizationId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByStandardOrganizationId(string standardOrganizationId)
        {
            // 结果列表
            List<IRoleInfo> list = new List<IRoleInfo>();

            // 临时列表
            IList<IRoleInfo> temp = null;

            // 
            // 查找角色信息
            // 

            temp = provider.FindAllByStandardOrganizationId(standardOrganizationId);

            list.AddRange(temp);

            //
            // 查找组织子部门的角色信息
            //

            IList<IStandardOrganizationInfo> organizations = MembershipManagement.Instance.StandardOrganizationService.FindAllByParentId(standardOrganizationId);

            foreach (IStandardOrganizationInfo organization in organizations)
            {
                temp = FindAllByStandardOrganizationId(organization.Id);

                list.AddRange(temp);
            }

            return list;
        }
        #endregion

        #region 函数:FindAllByStandardRoleId(string standardRoleId)
        /// <summary>递归查询某个标准角色下面所有的角色</summary>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByStandardRoleId(string standardRoleId)
        {
            return provider.FindAllByStandardRoleId(standardRoleId);
        }
        #endregion

        #region 函数:FindAllByOrganizationIdAndJobId(string organizationId, string jobId)
        /// <summary>递归查询某个组织下面相关的职位对应的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="jobId">职位标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByOrganizationIdAndJobId(string organizationId, string jobId)
        {
            return provider.FindAllByOrganizationIdAndJobId(organizationId, jobId);
        }
        #endregion

        #region 函数:FindAllByAssignedJobId(string assignedJobId)
        /// <summary>递归查询某个组织下面相关的岗位对应的角色信息</summary>
        /// <param name="assignedJobId">岗位标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByAssignedJobId(string assignedJobId)
        {
            return provider.FindAllByAssignedJobId(assignedJobId);
        }
        #endregion

        #region 函数:FindAllByCorporationIdAndProjectId(string corporationIds, string projectIds)
        /// <summary>递归查询某个公司下面所有的角色和某个项目下面所有的角色</summary>
        /// <param name="corporationIds">公司标识，多个以逗号隔开</param>
        /// <param name="projectIds">项目标识，多个以逗号隔开</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByCorporationIdAndProjectId(string corporationIds, string projectIds)
        {
            List<IRoleInfo> list = new List<IRoleInfo>();

            // 查询相关公司的所有角色
            list.AddRange(this.FindAllByCorporationIds(corporationIds));

            // 查询相关项目的所有角色
            list.AddRange(this.FindAllByProjectIds(projectIds));

            return list;
        }
        #endregion

        #region 函数:FindAllByCorporationIds(string corporationIds)
        /// <summary>递归查询某个公司下面所有的角色</summary>
        /// <param name="corporationIds">公司标识，多个以逗号隔开</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByCorporationIds(string corporationIds)
        {
            List<IRoleInfo> list = new List<IRoleInfo>();

            // 查询公司的所有角色
            if (!string.IsNullOrEmpty(corporationIds))
            {
                string[] keys = corporationIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string key in keys)
                {
                    list.AddRange(this.FindAllByCorporationId(key));
                }
            }

            return list;
        }
        #endregion

        #region 函数:FindAllByCorporationId(string corporationId)
        /// <summary>递归查询某个公司下面所有的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByCorporationId(string corporationId)
        {
            // 结果列表
            List<IRoleInfo> list = new List<IRoleInfo>();

            //
            // 查找部门(公司下一级组织架构)
            //
            IList<IOrganizationInfo> organizations = MembershipManagement.Instance.OrganizationService.FindAllByParentId(corporationId);

            // 
            // 查找角色信息
            // 

            list.AddRange(FindAllByOrganizationId(corporationId));

            foreach (IOrganizationInfo organization in organizations)
            {
                // 获取项目团队以外的只能部门
                if (organization.Name.IndexOf("项目团队") == -1)
                {
                    list.AddRange(FindAllByOrganizationId(organization.Id, -1));
                }
            }

            return list;
        }
        #endregion

        #region 函数:FindAllByProjectIds(string projectIds)
        /// <summary>递归查询某个项目下面所有的角色</summary>
        /// <param name="projectIds">项目标识，多个以逗号隔开</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByProjectIds(string projectIds)
        {
            List<IRoleInfo> list = new List<IRoleInfo>();

            // 查询公司的所有角色
            if (!string.IsNullOrEmpty(projectIds))
            {
                string[] keys = projectIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string key in keys)
                {
                    list.AddRange(this.FindAllByProjectId(key));
                }
            }

            return list;
        }
        #endregion

        #region 函数:FindAllByProjectId(string projectId)
        /// <summary>递归查询某个项目下面所有的角色</summary>
        /// <param name="projectId">组织标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByProjectId(string projectId)
        {
            // 项目团队的标识 和 项目标识 保存一致
            string organizationId = projectId;

            return FindAllByOrganizationId(organizationId, -1);
        }
        #endregion

        #region 函数:FindAllByCorporationIdAndStandardRoleIds(string corporationId, string standardRoleIds)
        /// <summary>递归查询某个公司下面对应某标准角色相对应的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <param name="standardRoleIds">标准角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByCorporationIdAndStandardRoleIds(string corporationId, string standardRoleIds)
        {
            return provider.FindAllByCorporationIdAndStandardRoleIds(corporationId, standardRoleIds);
        }
        #endregion

        #region 函数:FindAllBetweenPriority(string corporationId, string standardRoleIds)
        /// <summary>根据某个组织标识查询此组织上下级之间属于某一范围权重值的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="minPriority">最小权重值</param>
        /// <param name="maxPriority">最大权重值</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllBetweenPriority(string organizationId, int minPriority, int maxPriority)
        {
            return provider.FindAllBetweenPriority(organizationId, minPriority, maxPriority);
        }
        #endregion

        #region 函数:FindAllWithoutMember(int length)
        /// <summary>返回所有没有成员的角色信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllWithoutMember(int length)
        {
            return provider.FindAllWithoutMember(length, false);
        }
        #endregion

        #region 函数:FindAllWithoutMember(int length, bool includeAllRole)
        /// <summary>返回所有没有成员的角色信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <param name="includeAllRole">包含全部角色</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllWithoutMember(int length, bool includeAllRole)
        {
            return provider.FindAllWithoutMember(length, includeAllRole);
        }
        #endregion

        #region 函数:FindForwardLeadersByOrganizationId(string organizationId)
        /// <summary>返回所有正向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindForwardLeadersByOrganizationId(string organizationId)
        {
            return provider.FindForwardLeadersByOrganizationId(organizationId, 1);
        }
        #endregion

        #region 函数:FindForwardLeadersByOrganizationId(string organizationId, int level)
        /// <summary>返回所有正向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindForwardLeadersByOrganizationId(string organizationId, int level)
        {
            return provider.FindForwardLeadersByOrganizationId(organizationId, level);
        }
        #endregion

        #region 函数:FindBackwardLeadersByOrganizationId(string organizationId)
        /// <summary>返回所有反向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindBackwardLeadersByOrganizationId(string organizationId)
        {
            return provider.FindBackwardLeadersByOrganizationId(organizationId, 1);
        }
        #endregion

        #region 函数:FindBackwardLeadersByOrganizationId(string organizationId, int level)
        /// <summary>返回所有反向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindBackwardLeadersByOrganizationId(string organizationId, int level)
        {
            return provider.FindBackwardLeadersByOrganizationId(organizationId, level);
        }
        #endregion

        #region 函数:FindStandardGeneralRolesByOrganizationId(string organizationId, int standardGeneralRoleId)
        /// <summary>返回所有父级对象为标准通用角色标识【standardGeneralRoleId】的相关角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindStandardGeneralRolesByOrganizationId(string organizationId, string standardGeneralRoleId)
        {
            return provider.FindStandardGeneralRolesByOrganizationId(organizationId, standardGeneralRoleId);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IRoleInfo"/></returns>
        public IList<IRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">组织单位名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">角色全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">角色标识</param>
        /// <param name="name">角色名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            // 检测名称是否已被使用
            if (IsExistName(name))
            {
                // 已存在相同名称对象。
                return 1;
            }

            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 对象【${Id}】不存在。
                return 2;
            }

            return provider.Rename(id, name);
        }
        #endregion

        #region 函数:GetEveryoneObject()
        /// <summary>获取所有人角色</summary>
        public IRoleInfo GetEveryoneObject()
        {
            IRoleInfo everyone = new RoleInfo();

            everyone.Id = Guid.Empty.ToString();
            everyone.Name = "所有人";

            return everyone;
        }
        #endregion

        #region 函数:CombineFullPath(string name, string organizationId)
        /// <summary>角色全路径</summary>
        /// <param name="name">角色名称</param>
        /// <param name="organizationId">所属组织标识</param>
        /// <returns></returns>
        public string CombineFullPath(string name, string organizationId)
        {
            string path = MembershipManagement.Instance.OrganizationService.GetOrganizationPathByOrganizationId(organizationId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 函数:CombineDistinguishedName(string name, string organizationId)
        /// <summary>角色唯一名称</summary>
        /// <param name="name">角色名称</param>
        /// <param name="organizationId">所属组织标识</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name, string organizationId)
        {
            string path = MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(organizationId);

            return string.Format("CN={0},{1}{2}", name, path, ActiveDirectoryConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">帐户标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetGlobalName(string id, string globalName)
        {
            if (string.IsNullOrEmpty(globalName))
            {
                // 对象【${Id}】全局名称不能为空。
                return 1;
            }

            if (IsExistGlobalName(globalName))
            {
                return 2;
            }

            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 对象【${Id}】不存在。
                return 3;
            }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IRoleInfo originalObject = FindOne(id);

                if (originalObject != null)
                {
                    // 由于外部系统直接同步到人员及权限管理的数据库中，
                    // 所以 Active Directory 上不会直接创建相关对象，需要手工设置全局名称并创建相关对象。
                    if (!string.IsNullOrEmpty(originalObject.GlobalName)
                        && ActiveDirectoryManagement.Instance.Group.IsExistName(originalObject.GlobalName))
                    {
                        ActiveDirectoryManagement.Instance.Group.Rename(originalObject.GlobalName, globalName);
                    }
                    else
                    {
                        ActiveDirectoryManagement.Instance.Group.Add(globalName, MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalObject.Id));
                    }
                }
            }

            return provider.SetGlobalName(id, globalName);
        }
        #endregion

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>设置父级角色标识</summary>
        /// <param name="id">角色标识</param>
        /// <param name="parentId">父级角色标识</param>
        /// <returns>0:代表成功</returns>
        public int SetParentId(string id, string parentId)
        {
            return provider.SetParentId(id, parentId);
        }
        #endregion

        #region 函数:SetExchangeStatus(string id, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="id">角色标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string id, int status)
        {
            return provider.SetExchangeStatus(id, status);
        }
        #endregion

        #region 函数:GetAuthorities(string roleId)
        /// <summary>获取角色的权限</summary>
        /// <param name="roleId">角色标识</param>
        public IList<AuthorityInfo> GetAuthorities(string roleId)
        {
            return provider.GetAuthorities(roleId);
        }
        #endregion

        #region 函数:GenerateStandardRoleMappingReport(string organizationId, string standardRoleType)
        /// <summary>生成标准角色映射报表</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="standardRoleType">标准角色类型</param>
        public DataTable GenerateStandardRoleMappingReport(string organizationId, string standardRoleType)
        {
            IList<IStandardRoleInfo> list = MembershipManagement.Instance.StandardRoleService.FindAllByType(Convert.ToInt32(standardRoleType));

            string standardRoleIds = null;

            foreach (IStandardRoleInfo item in list)
            {
                standardRoleIds += item.Id + ",";
            }

            standardRoleIds = standardRoleIds.TrimEnd(new char[] { ',' });

            return GenerateStandardRoleMappingReport(organizationId, standardRoleType, standardRoleIds);
        }
        #endregion

        #region 函数:GenerateStandardRoleMappingReport(string organizationId, string standardRoleType, string standardRoleIds)
        /// <summary>生成标准角色映射报表</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="standardRoleType">标准角色类型</param>
        /// <param name="standardRoleIds">标准角色标识，多个以逗号隔开</param>
        public DataTable GenerateStandardRoleMappingReport(string organizationId, string standardRoleType, string standardRoleIds)
        {
            DataTable table = new DataTable();

            table.Columns.Add("standardRoleId");
            table.Columns.Add("roleId");
            table.Columns.Add("roleName");
            table.Columns.Add("roleIsCreatedValue");

            IList<IRoleInfo> list = null;

            string[] keys = standardRoleIds.Split(new char[] { ',' });

            // bool isCreated = false;

            switch (standardRoleType)
            {
                case "0":
                case "1":
                case "11":
                case "21":
                    list = FindAllByCorporationId(organizationId);
                    break;

                case "2":
                case "12":
                case "22":
                    list = FindAllByProjectId(organizationId);
                    break;

                default:
                    list = new List<IRoleInfo>();
                    break;
            }

            int count = 0;

            for (int i = 0; i < keys.Length; i++)
            {
                foreach (IRoleInfo item in list)
                {
                    if (item.StandardRole == null)
                    {
                        continue;
                    }

                    if (item.StandardRole.Id == keys[i])
                    {
                        DataRow row = table.NewRow();

                        row["standardRoleId"] = keys[i];
                        row["roleId"] = item.Id;
                        row["roleName"] = item.Name;
                        row["roleIsCreatedValue"] = "1"; // 1:代表已创建

                        table.Rows.Add(row);

                        break;
                    }
                }

                count++;
            }

            return table;
        }
        #endregion

        #region 函数:QuickCreateRole(string standardRoleType, string corporationId, string standardRoleId, string roleName)
        /// <summary>快速创建标准角色</summary>
        /// <param name="standardRoleType">标准角色类型</param>
        /// <param name="organizationId">公司或项目标识</param>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <param name="roleName">角色名称</param>
        public virtual int QuickCreateRole(string standardRoleType, string organizationId, string standardRoleId, string roleName)
        {
            if (MembershipManagement.Instance.RoleService.IsExistName(roleName))
            {
                return 1;
            }

            IStandardRoleInfo standardRoleInfo = MembershipManagement.Instance.StandardRoleService[standardRoleId];

            if (standardRoleInfo == null)
            {
                return 2;
            }
            else
            {
                RoleInfo param = new RoleInfo();

                param.Id = StringHelper.ToGuid();
                param.Name = roleName;
                param.OrganizationId = organizationId;
                param.ParentId = StringHelper.ToGuid(Guid.Empty);
                param.StandardRoleId = standardRoleId;
                param.Status = 1;

                IList<IOrganizationInfo> organizations = null;

                IList<IRoleInfo> roles = null;

                switch (standardRoleType)
                {
                    case "0":
                    case "1":
                    case "11":
                    case "21":
                        organizations = MembershipManagement.Instance.OrganizationService.FindAllByCorporationId(organizationId);
                        roles = MembershipManagement.Instance.RoleService.FindAllByCorporationId(organizationId);
                        break;
                    case "2":
                    case "12":
                    case "22":
                        organizations = MembershipManagement.Instance.OrganizationService.FindAllByProjectId(organizationId);
                        roles = MembershipManagement.Instance.RoleService.FindAllByCorporationId(organizationId);
                        break;
                    default:
                        return 0;
                }

                foreach (IOrganizationInfo organization in organizations)
                {
                    // 根据标准组织找上级组织
                    if (standardRoleInfo.StandardOrganizationId == organization.StandardOrganizationId)
                    {
                        param.OrganizationId = organization.Id;
                    }
                }

                foreach (IRoleInfo role in roles)
                {
                    // 根据标准角色找上级角色
                    if (standardRoleInfo.ParentId == role.StandardRoleId)
                    {
                        param.ParentId = role.Id;
                    }
                }

                MembershipManagement.Instance.RoleService.Save(param);
            }

            return 0;
        }
        #endregion

        #region 函数:CreateRoleWithProjectAndStandardRole(string projectId, string standardRoleId, string roleName)
        /// <summary>新建项目类标准角色</summary>
        /// <param name="projectId">项目标识</param>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <param name="roleName">角色名称</param>
        public virtual int CreateRoleWithProjectAndStandardRole(string projectId, string standardRoleId, string roleName)
        {
            if (MembershipManagement.Instance.RoleService.IsExistName(roleName))
            {
                return 1;
            }

            IStandardRoleInfo standardRoleInfo = MembershipManagement.Instance.StandardRoleService[standardRoleId];

            if (standardRoleInfo == null)
            {
                return 2;
            }
            else
            {
                RoleInfo param = new RoleInfo();

                param.Id = StringHelper.ToGuid();
                param.Name = roleName;
                param.GlobalName = roleName;
                param.OrganizationId = projectId;
                param.ParentId = StringHelper.ToGuid(Guid.Empty);
                param.StandardRoleId = standardRoleId;
                param.Status = 1;

                IList<IOrganizationInfo> organizations = MembershipManagement.Instance.OrganizationService.FindAllByProjectId(projectId);

                foreach (IOrganizationInfo organization in organizations)
                {
                    // 根据标准组织找上级组织
                    if (standardRoleInfo.StandardOrganizationId == organization.StandardOrganizationId)
                    {
                        param.OrganizationId = organization.Id;
                    }
                }

                IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByProjectId(projectId);

                foreach (IRoleInfo role in roles)
                {
                    // 根据标准角色找上级角色
                    if (standardRoleInfo.ParentId == role.StandardRoleId)
                    {
                        param.ParentId = role.Id;
                    }
                }

                MembershipManagement.Instance.RoleService.Save(param);
            }

            return 0;
        }
        #endregion

        #region 函数:SetProjectRoleMapping(string fromProjectId, string toProjectId)
        /// <summary>设置项目角色映射关系</summary>
        /// <param name="fromProjectId">来源项目标识</param>
        /// <param name="toProjectId">目标项目标识</param>
        public DataTable SetProjectRoleMapping(string fromProjectId, string toProjectId)
        {
            DataTable table = new DataTable();

            table.Columns.Add("fromProjectOrganizationId");
            table.Columns.Add("fromProjectRoleId");
            table.Columns.Add("fromProjectRoleName");
            table.Columns.Add("fromProjectRoleAccountValue");
            table.Columns.Add("fromProjectRoleStandardRoleId");
            table.Columns.Add("toProjectRoleId");
            table.Columns.Add("toProjectRoleName");
            table.Columns.Add("toProjectRoleAccountValue");

            IList<IRoleInfo> fromProjectRoles = FindAllByProjectId(fromProjectId);

            IList<IRoleInfo> toProjectRoles = FindAllByProjectId(toProjectId);

            // 添加来源项目角色数据
            // 到fromProjectOrganizationId，fromProjectRoleId，fromProjectRoleName，fromProjectRoleAccountValue。
            foreach (IRoleInfo role in fromProjectRoles)
            {
                // 忽略没有所属标准角色的角色
                if (string.IsNullOrEmpty(role.StandardRoleId))
                    continue;

                DataRow row = table.NewRow();

                row["fromProjectOrganizationId"] = role.OrganizationId;
                row["fromProjectRoleId"] = role.Id;
                row["fromProjectRoleName"] = role.Name;
                row["fromProjectRoleAccountValue"] = SetProjectRoleMappingAccountValue(role.Id);
                row["fromProjectRoleStandardRoleId"] = role.StandardRoleId;

                table.Rows.Add(row);
            }

            // 映射目标项目角色数据
            // 到toProjectRoleId，toProjectRoleName，toProjectRoleAccountValue。
            foreach (IRoleInfo role in toProjectRoles)
            {
                // 忽略没有所属标准角色的角色
                if (string.IsNullOrEmpty(role.StandardRoleId))
                    continue;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["fromProjectRoleStandardRoleId"].ToString() == role.StandardRoleId)
                    {
                        table.Rows[i]["toProjectRoleId"] = role.Id;
                        table.Rows[i]["toProjectRoleName"] = role.Name;
                        table.Rows[i]["toProjectRoleAccountValue"] = SetProjectRoleMappingAccountValue(role.Id);
                        break;
                    }
                }
            }

            return table;
        }
        #endregion

        #region 私有函数:SetProjectRoleMappingAccountValue(string roleId)
        /// <summary>设置项目角色映射关系中的帐号数据</summary>
        /// <param name="roleId">角色标识</param>
        public string SetProjectRoleMappingAccountValue(string roleId)
        {
            StringBuilder outString = new StringBuilder();

            IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAllByRoleId(roleId);

            foreach (IAccountInfo item in list)
            {
                // 过滤禁用的用户
                if (item.Status == 0) { continue; }

                outString.AppendFormat("account#{0}#{1};", item.Id, item.Name);
            }

            if (outString.Length > 1 && outString.ToString().Substring(outString.Length - 1, 1) == ";")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            return outString.ToString();
        }
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"role\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<roles>");

            foreach (IRoleInfo item in list)
            {
                outString.Append(((RoleInfo)item).Serializable());
            }

            outString.Append("</roles>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 函数:SyncToActiveDirectory(IRoleInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">角色信息</param>
        public int SyncToActiveDirectory(IRoleInfo param)
        {
            return SyncToActiveDirectory(param, param.Name, param.OrganizationId);
        }
        #endregion

        #region 函数:SyncToActiveDirectory(IRoleInfo param, string originalGlobalName, string originalOrganizationId)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">角色信息</param>
        /// <param name="originalGlobalName">原始的全局名称</param>
        /// <param name="originalOrganizationId">原始的所属组织标识</param>
        public int SyncToActiveDirectory(IRoleInfo param, string originalGlobalName, string originalOrganizationId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.Name))
                {
                    // 角色【${Name}】名称为空，请配置相关信息。
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // 角色【${GlobalName}】名称为空，请配置相关信息。
                    return 1;
                }
                else
                {
                    // 1.原始的全局名称不为空。
                    // 2.Active Directory 上有相关对象。
                    if (!string.IsNullOrEmpty(originalGlobalName)
                        && ActiveDirectoryManagement.Instance.Group.IsExistName(originalGlobalName))
                    {
                        if (param.GlobalName != originalGlobalName)
                        {
                            // 角色【${Name}】的名称发生改变。
                            ActiveDirectoryManagement.Instance.Group.Rename(originalGlobalName, param.GlobalName);
                        }

                        if (param.OrganizationId != originalOrganizationId)
                        {
                            // 角色【${Name}】所属的组织发生变化。
                            ActiveDirectoryManagement.Instance.Group.MoveTo(param.GlobalName,
                                MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(param.OrganizationId));
                        }

                        return 0;
                    }
                    else
                    {
                        string parentPath = MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(param.OrganizationId);

                        ActiveDirectoryManagement.Instance.Group.Add(param.GlobalName, parentPath);

                        // 角色【${Name}】创建成功。
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(IRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">角色信息</param>
        public int SyncFromPackPage(IRoleInfo param)
        {
            return provider.SyncFromPackPage(param);
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和角色关系
        // -------------------------------------------------------

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountRoleRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            return provider.FindAllRelationByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllRelationByRoleId(string roleId)
        /// <summary>根据角色查询相关帐号的关系</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountRoleRelationInfo> FindAllRelationByRoleId(string roleId)
        {
            return provider.FindAllRelationByRoleId(roleId);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string roleId)
        /// <summary>添加帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        public int AddRelation(string accountId, string roleId)
        {
            return AddRelation(accountId, roleId, false, DateTime.Now, DateTime.MaxValue);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string roleId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        /// <param name="isDefault">是否是默认角色</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string roleId, bool isDefault, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                // 帐号标识不能为空
                return 1;
            }

            if (string.IsNullOrEmpty(roleId))
            {
                // 角色标识不能为空
                return 2;
            }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IRoleInfo role = MembershipManagement.Instance.RoleService[roleId];

                // 帐号对象、帐号的全局名称、帐号的登录名、角色对象、角色的全局名称等不能为空。
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && role != null && !string.IsNullOrEmpty(role.GlobalName))
                {
                    ActiveDirectoryManagement.Instance.Group.AddRelation(account.LoginName, ActiveDirectorySchemaClassType.User, role.GlobalName);
                }
            }

            return provider.AddRelation(accountId, roleId, isDefault, beginDate, endDate);
        }
        #endregion

        #region 函数:AddRelationRange(string accountIds, string roleId)
        /// <summary>添加帐号与相关角色的关系</summary>
        /// <param name="accountIds">帐号标识，多个以逗号隔开</param>
        /// <param name="roleId">角色标识</param>
        public int AddRelationRange(string accountIds, string roleId)
        {
            string[] list = accountIds.Split(new char[] { ',' });

            foreach (string accountId in list)
            {
                AddRelation(accountId, roleId);
            }

            return 0;
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string roleId, DateTime endDate)
        /// <summary>续约帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string roleId, DateTime endDate)
        {
            return provider.ExtendRelation(accountId, roleId, endDate);
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string roleId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        public int RemoveRelation(string accountId, string roleId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IRoleInfo role = MembershipManagement.Instance.RoleService[roleId];

                // 帐号对象、帐号的全局名称、帐号的登录名、角色对象、角色的全局名称等不能为空。
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && role != null && !string.IsNullOrEmpty(role.GlobalName))
                {
                    ActiveDirectoryManagement.Instance.Group.RemoveRelation(account.LoginName, ActiveDirectorySchemaClassType.User, role.GlobalName);
                }
            }

            return provider.RemoveRelation(accountId, roleId);
        }
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关角色的默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveDefaultRelation(string accountId)
        {
            return provider.RemoveDefaultRelation(accountId);
        }
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关角色的非默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            return provider.RemoveNondefaultRelation(accountId);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的角色关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            return provider.RemoveExpiredRelation(accountId);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关角色的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountRoleRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountRoleRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.RoleId);
                }

                return 0;
            }
            else
            {
                return provider.RemoveAllRelation(accountId);
            }
        }
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号的默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        public bool HasDefaultRelation(string accountId)
        {
            return provider.HasDefaultRelation(accountId);
        }
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string roleId)
        /// <summary>设置帐号的默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        public int SetDefaultRelation(string accountId, string roleId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IRoleInfo role = MembershipManagement.Instance.RoleService[roleId];

                if (account != null && role != null)
                {
                    ActiveDirectoryManagement.Instance.Group.AddRelation(account.GlobalName, ActiveDirectorySchemaClassType.User, role.Name);
                }
            }

            return provider.SetDefaultRelation(accountId, roleId);
        }
        #endregion

        #region 函数:ClearupRelation(string roleId)
        /// <summary>清理角色与帐号的关系</summary>
        /// <param name="roleId">角色标识</param>
        public int ClearupRelation(string roleId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountRoleRelationInfo> list = FindAllRelationByRoleId(roleId);

                foreach (IAccountRoleRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.RoleId);
                }

                return 0;
            }
            else
            {
                return provider.ClearupRelation(roleId);
            }
        }
        #endregion
    }
}