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

namespace X3Platform.Membership.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Security.Authority;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    [DataObject]
    public class RoleProvider : IRoleProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Role";

        #region 构造函数:RoleProvider()
        /// <summary>构造函数</summary>
        public RoleProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除 
        // -------------------------------------------------------

        #region 函数:Save(IRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">IRoleInfo 实例详细信息</param>
        /// <returns>IRoleInfo 实例详细信息</returns>
        public IRoleInfo Save(IRoleInfo param)
        {
            if (!this.IsExist(param.Id))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(IRoleInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">IRoleInfo 实例的详细信息</param>
        public void Insert(IRoleInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_Role_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);

            // 绑定关系
            // BindRelation(param);
        }
        #endregion

        #region 函数:Update(IRoleInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">IRoleInfo 实例的详细信息</param>
        public void Update(IRoleInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            id = StringHelper.ToSafeSQL(id, true);

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                // 删除角色与帐号的关系
                this.RemoveRelation(string.Format(" RoleId = ##{0}## ", id));

                // 删除角色信息
                args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch (DataException ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">角色标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        public IRoleInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<RoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">角色的全局名称</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IRoleInfo FindOneByGlobalName(string globalName)
        {
            string whereClause = string.Format(" GlobalName = ##{0}## ", StringHelper.ToSafeSQL(globalName));

            IList<IRoleInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 函数:FindOneByCorporationIdAndStandardRoleId(string corporationId, string standardRoleId)
        /// <summary>递归查询某个公司下面对应某标准角色相对应的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IRoleInfo FindOneByCorporationIdAndStandardRoleId(string corporationId, string standardRoleId)
        {
            IList<IRoleInfo> list = FindAllByCorporationIdAndStandardRoleIds(corporationId, standardRoleId);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 IRoleInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IRoleInfo> list = this.ibatisMapper.QueryForList<IRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByParentId(string parentId)
        {
            string whereClause = string.Format(" ParentId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(parentId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个 AccountInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByAccountId(string accountId)
        {
            string whereClause = string.Format(" T.Id IN ( SELECT RoleId FROM tb_Account_Role WHERE AccountId = ##{0}## ) ORDER BY OrderId ", StringHelper.ToSafeSQL(accountId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByOrganizationId(string organizationId)
        /// <summary>查询某个组织下面所有的角色</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByOrganizationId(string organizationId)
        {
            string whereClause = string.Format(" OrganizationId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(organizationId));

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByGeneralRoleId(string generalRoleId)
        /// <summary>递归查询某个公司下面所有的角色</summary>
        /// <param name="generalRoleId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByGeneralRoleId(string generalRoleId)
        {
            string whereClause = string.Format(" GeneralRoleId = ##{0}## ", generalRoleId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByStandardOrganizationId(string standardOrganizationId)
        /// <summary>递归查询某个标准组织下面所有的角色</summary>
        /// <param name="standardOrganizationId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByStandardOrganizationId(string standardOrganizationId)
        {
            string whereClause = string.Format(" StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE StandardOrganizationId = ##{0}## ) ", standardOrganizationId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByStandardRoleId(string standardRoleId)
        /// <summary>递归查询某个标准角色下面所有的角色</summary>
        /// <param name="standardRoleId">标准角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByStandardRoleId(string standardRoleId)
        {
            string whereClause = string.Format(" StandardRoleId = ##{0}## ", standardRoleId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByOrganizationIdAndJobId(string organizationId, string jobId)
        /// <summary>递归查询某个组织下面相关的职位对应的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="jobId">职位标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByOrganizationIdAndJobId(string organizationId, string jobId)
        {
            string whereClause = string.Format(@" 
OrganizationId = ##{0}## AND StandardRoleId IN ( 
    SELECT StandardRoleId FROM tb_Job_StandardRole WHERE JobId = ##{1}## ) 
", organizationId, jobId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByAssignedJobId(string assignedJobId)
        /// <summary>递归查询某个组织下面相关的岗位对应的角色信息</summary>
        /// <param name="assignedJobId">岗位标识</param>
        /// <returns>返回一个<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByAssignedJobId(string assignedJobId)
        {
            string whereClause = string.Format(@" 
OrganizationId IN ( SELECT OrganizationId FROM tb_AssignedJob WHERE Id = ##{0}## ) 
AND StandardRoleId IN ( SELECT StandardRoleId FROM tb_Job_StandardRole WHERE JobId IN ( SELECT JobId FROM tb_AssignedJob WHERE Id = ##{0}## ) ) 
", assignedJobId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByCorporationIdAndStandardRoleIds(string corporationId, string standardRoleIds)
        /// <summary>递归查询某个公司下面对应某标准角色相对应的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <param name="standardRoleIds">标准角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllByCorporationIdAndStandardRoleIds(string corporationId, string standardRoleIds)
        {
            string whereClause = string.Format("( dbo.func_GetCorporationIdByOrganizationId(OrganizationId) = ##{0}## AND StandardRoleId IN ({1}) )",
                                    corporationId,
                                    string.Format(" ##{0}## ", standardRoleIds.Replace(",", "## , ##")));

            return FindAll(whereClause, 0);
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
            string whereClause = null;

            for (int i = 1; i < 10; i++)
            {
                whereClause += (i == 1 ? " ( " : " OR ")
                            + string.Format(" OrganizationId = dbo.func_GetDepartmentIdByOrganizationId( ##{0}## , {1} ) ", organizationId, i)
                            + ((i + 1) == 10 ? " ) " : string.Empty);
            }

            whereClause += string.Format(@" AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority BETWEEN {0} AND {1} ) ", minPriority, maxPriority);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllWithoutMember(int length, bool includeAllRole)
        /// <summary>返回所有没有成员的角色信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <param name="includeAllRole">包含全部角色</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindAllWithoutMember(int length, bool includeAllRole)
        {
            string whereClause = null;

            if (includeAllRole)
            {
                whereClause = " Id NOT IN ( SELECT RoleId FROM tb_Account_Role ) ";
            }
            else
            {
                whereClause = " Id NOT IN ( SELECT RoleId FROM tb_Account_Role ) AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE IsKey = 1 ) ";
            }

            return FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindForwardLeadersByOrganizationId(string organizationId, int level)
        /// <summary>返回所有正向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindForwardLeadersByOrganizationId(string organizationId, int level)
        {
            string whereClause = string.Format(@" 
OrganizationId = dbo.func_GetDepartmentIdByOrganizationId( ##{0}## , {1} ) 
AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority >= 40 )
", organizationId, level);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindBackwardLeadersByOrganizationId(string organizationId, int level)
        /// <summary>返回所有反向领导的角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindBackwardLeadersByOrganizationId(string organizationId, int level)
        {
            string whereClause = string.Format(@" 
OrganizationId IN ( dbo.func_GetDepartmentIdByOrganizationId( ##{0}## , ( dbo.func_GetDepartmentLevelByOrganizationId( ##{0}## ) - {1} ) ) )
AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority >= 40 )
", organizationId, level);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindStandardGeneralRolesByOrganizationId(string organizationId, string standardGeneralRoleId)
        /// <summary>返回所有父级对象为标准通用角色标识【standardGeneralRoleId】的相关角色信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IRoleInfo> FindStandardGeneralRolesByOrganizationId(string organizationId, string standardGeneralRoleId)
        {
            string whereClause = null;

            for (int i = 1; i < 10; i++)
            {
                whereClause += (i == 1 ? " ( " : " OR ")
                            + string.Format(" OrganizationId = dbo.func_GetDepartmentIdByOrganizationId( ##{0}## , {1} ) ", organizationId, i)
                            + ((i + 1) == 10 ? " ) " : string.Empty);
            }

            whereClause += string.Format(@" AND Id IN ( SELECT RoleId FROM tb_StandardGeneralRole_Mapping WHERE StandardGeneralRoleId = ##{0}## ) ", standardGeneralRoleId);

            return this.FindAll(whereClause, 0);
        }
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
        public IList<IRoleInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " OrderId DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IRoleInfo> list = ibatisMapper.QueryForList<IRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new ArgumentException("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">群组名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">组织全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            if (string.IsNullOrEmpty(globalName)) { throw new Exception("实例全局名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" GlobalName = '{0}' ", StringHelper.ToSafeSQL(globalName)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">角色标识</param>
        /// <param name="name">角色名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("Name", StringHelper.ToSafeSQL(name));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">角色标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetGlobalName(string id, string globalName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("GlobalName", StringHelper.ToSafeSQL(globalName));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetGlobalName", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">角色标识</param>
        /// <param name="parentId">父级角色标识</param>
        /// <returns>0:代表成功</returns>
        public int SetParentId(string id, string parentId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("ParentId", StringHelper.ToSafeSQL(parentId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetParentId", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetExchangeStatus(string id, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="id">角色标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string id, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("EnableExchangeEmail", status);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetExchangeStatus", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:GetAuthorities(string roleId)
        /// <summary>获取角色的权限</summary>
        /// <param name="roleId">角色标识</param>
        public IList<AuthorityInfo> GetAuthorities(string roleId)
        {
            IList<AuthorityInfo> list = new List<AuthorityInfo>();

            AuthorityInfo item = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("RoleId", StringHelper.ToSafeSQL(roleId));

            IList<string> ids = this.ibatisMapper.QueryForList<string>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorities", tableName)), args);

            foreach (string id in ids)
            {
                item = AuthorityContext.Instance.AuthorityService[id];

                list.Add(item);
            }

            return list;
        }
        #endregion

        #region 函数:SyncFromPackPage(IRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">角色信息</param>
        public int SyncFromPackPage(IRoleInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和角色关系
        // -------------------------------------------------------

        #region 私有函数:FindAllRelation(string whereClause)
        /// <summary>查询帐号与角色的关系</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        private IList<IAccountRoleRelationInfo> FindAllRelation(string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause))
                return null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            return this.ibatisMapper.QueryForList<IAccountRoleRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllRelation", tableName)), args);
        }
        #endregion

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountRoleRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            string whereClause = string.Format(" AccountId = ##{0}## ", accountId);

            return FindAllRelation(whereClause);
        }
        #endregion

        #region 函数:FindAllRelationByRoleId(string roleId)
        /// <summary>根据群组查询相关帐号的关系</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>Table Columns：AccountId, RoleId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountRoleRelationInfo> FindAllRelationByRoleId(string roleId)
        {
            string whereClause = string.Format(" RoleId = ##{0}## ", roleId);

            return FindAllRelation(whereClause);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("RoleId", roleId);
            args.Add("IsDefault", isDefault);
            args.Add("BeginDate", beginDate);
            args.Add("EndDate", endDate);

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddRelation", tableName)), args);

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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("RoleId", roleId);
            args.Add("EndDate", endDate);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_ExtendRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 私有函数:RemoveAllRelation(string whereClause)
        /// <summary>移除帐号相关角色的关系</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        private int RemoveRelation(string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause))
                return -1;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string roleId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        public int RemoveRelation(string accountId, string roleId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND RoleId = ##{1}## ) ", accountId, roleId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关角色的默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveDefaultRelation(string accountId)
        {
            try
            {
                this.ibatisMapper.BeginTransaction();

                int result = 0;

                string whereClause = string.Format(" ( AccountId = ##{0}## AND IsDefault = 0 ) ", accountId);

                SetNullDefaultRelation(accountId);

                result = RemoveRelation(whereClause);

                this.ibatisMapper.CommitTransaction();

                return result;
            }
            catch (DataException ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关角色的非默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND IsDefault = 0 ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的角色关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND EndDate < ##{1}## ) ", accountId, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关角色的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号是否有默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        public bool HasDefaultRelation(string accountId)
        {
            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasDefaultRelation", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:SetNullDefaultRelation(string accountId)
        /// <summary>设置帐号的空的默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        public int SetNullDefaultRelation(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetNullDefaultRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string roleId)
        /// <summary>设置帐号的默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        public int SetDefaultRelation(string accountId, string roleId)
        {
            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
                args.Add("RoleId", StringHelper.ToSafeSQL(roleId));

                // 1.添加关系
                AddRelation(accountId, roleId, true, DateTime.Now, DateTime.MaxValue);
                // 2.清空默认关系
                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_ResetRelation", tableName)), args);
                // 3.设置 tb_Member 表的默认属性
                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultRelation", tableName)), args);
                // 4.设置 IsDefault = 1
                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultRelationIsDefault", tableName)), args);

                this.ibatisMapper.CommitTransaction();

                return 0;
            }
            catch (DataException ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        #region 函数:ClearupRelation(string roleId)
        /// <summary>清理角色与帐号的关系</summary>
        /// <param name="roleId">角色标识</param>
        public int ClearupRelation(string roleId)
        {
            string whereClause = string.Format(" ( RoleId = ##{0}## ) ", roleId);

            return RemoveRelation(whereClause);
        }
        #endregion
    }
}
