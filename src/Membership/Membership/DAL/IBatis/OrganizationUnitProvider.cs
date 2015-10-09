namespace X3Platform.Membership.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [DataObject]
    public class OrganizationUnitProvider : IOrganizationUnitProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_OrganizationUnit";

        #region 构造函数:OrganizationUnitProvider()
        /// <summary>构造函数</summary>
        public OrganizationUnitProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        /// <summary></summary>
        public IOrganizationUnitInfo this[string index]
        {
            get { return this.FindOne(index); }
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除 
        // -------------------------------------------------------

        #region 函数:Save(IOrganizationUnitInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">IOrganizationUnitInfo 实例详细信息</param>
        /// <returns>IOrganizationUnitInfo 实例详细信息</returns>
        public IOrganizationUnitInfo Save(IOrganizationUnitInfo param)
        {
            if (string.IsNullOrEmpty(param.Id) || !this.IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(IOrganizationUnitInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">IOrganizationUnitInfo 实例的详细信息</param>
        public void Insert(IOrganizationUnitInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_OrganizationUnit_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IOrganizationUnitInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">IOrganizationUnitInfo 实例的详细信息</param>
        public void Update(IOrganizationUnitInfo param)
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

                // 删除组织与帐号的关系
                this.RemoveRelation(string.Format(" OrganizationUnitId = ##{0}## ", id));

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
        /// <param name="id">组织标识</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        public IOrganizationUnitInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<OrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">组织的全局名称</param>
        /// <returns>返回一个<see cref="IOrganizationUnitInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindOneByGlobalName(string globalName)
        {
            string whereClause = string.Format(" GlobalName = ##{0}## ", StringHelper.ToSafeSQL(globalName));

            IList<IOrganizationUnitInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 函数:FindOneByRoleId(string roleId)
        /// <summary>查询某个组织下的所有相关组织</summary>
        /// <param name="roleId">组织标识</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        public IOrganizationUnitInfo FindOneByRoleId(string roleId)
        {
            string whereClause = string.Format(" Id IN ( SELECT OrganizationUnitId FROM tb_Role WHERE Id = ##{0}## ) ", StringHelper.ToSafeSQL(roleId));

            IList<IOrganizationUnitInfo> list = FindAll(whereClause, 0);

            return list.Count > 0 ? list[0] : null;
        }
        #endregion

        #region 函数:FindOneByRoleId(string roleId, int level)
        /// <summary>查询某个组织下的所有相关组织</summary>
        /// <param name="roleId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        public IOrganizationUnitInfo FindOneByRoleId(string roleId, int level)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("RoleId", StringHelper.ToSafeSQL(roleId));
            args.Add("Level", level);

            return this.ibatisMapper.QueryForObject<IOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByRoleId", tableName)), args);
        }
        #endregion

        #region 函数:FindCorporationByOrganizationUnitId(string id)
        /// <summary>查询某个组织所属的公司信息</summary>
        /// <param name="id">组织标识</param>
        /// <returns>返回所有<see cref="IOrganizationUnitInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindCorporationByOrganizationUnitId(string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            return this.ibatisMapper.QueryForObject<IOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindCorporationByOrganizationUnitId", tableName)), args);
        }
        #endregion

        #region 函数:FindDepartmentByOrganizationUnitId(string organizationId, int level)
        /// <summary>查询某个组织的所属某个上级部门信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationUnitInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindDepartmentByOrganizationUnitId(string organizationId, int level)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            args.Add("Level", level);

            return this.ibatisMapper.QueryForObject<IOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindDepartmentByOrganizationUnitId", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 IOrganizationUnitInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IOrganizationUnitInfo> list = this.ibatisMapper.QueryForList<IOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByParentId(string parentId)
        {
            string whereClause = string.Format(" ParentId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(parentId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">IOrganizationUnitInfo Id号</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByAccountId(string accountId)
        {
            string whereClause = string.Format(@" 
Id IN (
    SELECT OrganizationUnitId FROM tb_Role WHERE Id IN ( SELECT RoleId FROM tb_Account_Role WHERE AccountId = ##{0}## )
) ", StringHelper.ToSafeSQL(accountId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindCorporationsByAccountId(string accountId)
        /// <summary>查询某个帐户所属的所有公司信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IOrganizationUnitInfo"/>实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindCorporationsByAccountId(string accountId)
        {
            string whereClause = string.Format(@" 
Id IN (
    SELECT dbo.func_GetCorporationIdByOrganizationUnitId( Id ) FROM tb_OrganizationUnit WHERE Id IN (
        SELECT OrganizationUnitId FROM tb_Role WHERE Id IN ( SELECT RoleId FROM tb_Account_Role WHERE AccountId = ##{0}## )
)) ", StringHelper.ToSafeSQL(accountId));

            return FindAll(whereClause, 0);
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
        /// <returns>返回一个列表实例<see cref="IOrganizationUnitInfo"/></returns> 
        public IList<IOrganizationUnitInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));
            
            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            
            IList<IOrganizationUnitInfo> list = this.ibatisMapper.QueryForList<IOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        //#region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        ///// <summary>分页函数</summary>
        ///// <param name="startIndex">开始行索引数,由0开始统计</param>
        ///// <param name="pageSize">页面大小</param>
        ///// <param name="whereClause">WHERE 查询条件</param>
        ///// <param name="orderBy">ORDER BY 排序条件</param>
        ///// <param name="rowCount">记录行数</param>
        ///// <returns>返回一个列表</returns>
        //public IList<IOrganizationUnitInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        //{
        //    Dictionary<string, object> args = new Dictionary<string, object>();

        //    orderBy = string.IsNullOrEmpty(orderBy) ? " ModifiedDate DESC " : orderBy;

        //    args.Add("StartIndex", startIndex);
        //    args.Add("PageSize", pageSize);
        //    args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
        //    args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

        //    args.Add("RowCount", 0);

        //    IList<IOrganizationUnitInfo> list = this.ibatisMapper.QueryForList<IOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

        //    rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

        //    return list;
        //}
        //#endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">群组标识</param>
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
        /// <param name="id">组织标识</param>
        /// <param name="name">组织名称</param>
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
        /// <param name="id">帐户标识</param>
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
        /// <param name="id">组织标识</param>
        /// <param name="parentId">父级组织标识</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetParentId(string id, string parentId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("ParentId", StringHelper.ToSafeSQL(parentId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetParentId", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string accountId, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("EnableExchangeEmail", status);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetExchangeStatus", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(IOrganizationUnitInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">组织信息</param>
        public int SyncFromPackPage(IOrganizationUnitInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和组织关系
        // -------------------------------------------------------

        #region 私有函数:FindAllRelation(string whereClause)
        /// <summary>查询帐号与组织的关系</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>Table Columns：AccountId, OrganizationUnitId, IsDefault, BeginDate, EndDate</returns>
        private IList<IAccountOrganizationUnitRelationInfo> FindAllRelation(string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause))
                return null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            return this.ibatisMapper.QueryForList<IAccountOrganizationUnitRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllRelation", tableName)), args);
        }
        #endregion

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, OrganizationUnitId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationUnitRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            string whereClause = string.Format(" AccountId = ##{0}## ", accountId);

            return FindAllRelation(whereClause);
        }
        #endregion

        #region 函数:FindAllRelationByRoleId(string organizationId)
        /// <summary>根据群组查询相关帐号的关系</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>Table Columns：AccountId, OrganizationUnitId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationUnitRelationInfo> FindAllRelationByRoleId(string organizationId)
        {
            string whereClause = string.Format(" OrganizationUnitId = ##{0}## ", organizationId);

            return FindAllRelation(whereClause);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="isDefault">是否是默认组织</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("OrganizationUnitId", organizationId);
            args.Add("IsDefault", isDefault);
            args.Add("BeginDate", beginDate.ToString("yyyy-MM-dd HH:mm:ss"));
            args.Add("EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string organizationId, DateTime endDate)
        /// <summary>续约帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string organizationId, DateTime endDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("OrganizationUnitId", organizationId);
            args.Add("EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_ExtendRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 私有函数:RemoveAllRelation(string whereClause)
        /// <summary>移除帐号相关组织的关系</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        private int RemoveRelation(string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause)) { return -1; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string organizationId)
        /// <summary>移除帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        public int RemoveRelation(string accountId, string organizationId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND OrganizationUnitId = ##{1}## ) ", accountId, organizationId);

            return this.RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关组织的默认关系</summary>
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
        /// <summary>移除帐号相关组织的非默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND IsDefault = 0 ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的组织关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND EndDate < ##{1}## ) ", accountId, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关组织的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号是否有默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        public bool HasDefaultRelation(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasDefaultRelation", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:SetNullDefaultRelation(string accountId)
        /// <summary>设置帐号的空的默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        public int SetNullDefaultRelation(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetNullDefaultRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string organizationId)
        /// <summary>设置帐号的默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        public int SetDefaultRelation(string accountId, string organizationId)
        {
            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
                args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

                // 1.添加关系
                AddRelation(accountId, organizationId, true, DateTime.Now, DateTime.MaxValue);
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

        #region 函数:ClearupRelation(string organizationId)
        /// <summary>清理组织与帐号的关系</summary>
        /// <param name="organizationId">组织标识</param>
        public int ClearupRelation(string organizationId)
        {
            string whereClause = string.Format(" ( OrganizationUnitId = ##{0}## ) ", organizationId);

            return RemoveRelation(whereClause);
        }
        #endregion
    }
}
