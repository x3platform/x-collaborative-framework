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

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Util;

    /// <summary></summary>
    [DataObject]
    public class MemberProvider : IMemberProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Member";

        #region 构造函数:MemberProvider()
        /// <summary></summary>
        public MemberProvider()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除 
        // -------------------------------------------------------

        #region 函数:Save(IMemberInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">RoleInfo 实例详细信息</param>
        /// <returns>IMemberInfo 实例详细信息</returns>
        public IMemberInfo Save(IMemberInfo param)
        {
            if (!IsExist(param.Id))
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

        #region 函数:Insert(IMemberInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">RoleInfo 实例的详细信息</param>
        public void Insert(IMemberInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IMemberInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">RoleInfo 实例的详细信息</param>
        public void Update(IMemberInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">帐号标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            id = StringHelper.ToSafeSQL(id, true);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">RoleInfo Id号</param>
        /// <returns>返回一个 RoleInfo 实例的详细信息</returns>
        public IMemberInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<MemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个 MemberInfo 实例的详细信息</returns>
        public IMemberInfo FindOneByAccountId(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);

            return this.ibatisMapper.QueryForObject<MemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByAccountId", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByLoginName(string loginName)
        /// <summary>查询某条记录</summary>
        /// <param name="loginName">登录名</param>
        /// <returns>返回一个 RoleInfo 实例的详细信息</returns>
        public IMemberInfo FindOneByLoginName(string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);

            return this.ibatisMapper.QueryForObject<MemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByLoginName", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 RoleInfo 实例的详细信息</returns>
        public IList<IMemberInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<IMemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAllWithoutDefaultOrganization(int length)
        /// <summary>返回所有没有默认组织的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultOrganization(int length)
        {
            string whereClause = " Id NOT IN ( SELECT AccountId FROM tb_Member WHERE ISNULL( OrganizationId , '' ) = '' ) ";

            return FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllWithoutDefaultJob(int length)
        /// <summary>返回所有没有默认职位的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultJob(int length)
        {
            string whereClause = " ( JobId IS NULL OR JobId = '' ) ";

            return FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllWithoutDefaultAssignedJob(int length)
        /// <summary>返回所有没有默认岗位的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultAssignedJob(int length)
        {
            string whereClause = " Id NOT IN ( SELECT AccountId FROM tb_Account_AssignedJob WHERE IsDefault = 1 ) ";

            return FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllWithoutDefaultRole(int length)
        /// <summary>返回所有没有默认角色的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultRole(int length)
        {
            string whereClause = " Id NOT IN ( SELECT AccountId FROM tb_Account_Role WHERE IsDefault = 1 ) ";

            return FindAll(whereClause, length);
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
        public IList<IMemberInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IMemberInfo> list = this.ibatisMapper.QueryForList<IMemberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">角色标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:SetContactCard(string accountId, Dictionary<string,string> contactItems);
        /// <summary>设置联系卡信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="contactItems">联系项字典</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        public int SetContactCard(string accountId, Dictionary<string, string> contactItems)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            args.Add("Mobile", contactItems.ContainsKey("Mobile") ? StringHelper.ToSafeSQL(contactItems["Mobile"]) : string.Empty);
            args.Add("Telephone", contactItems.ContainsKey("Telephone") ? StringHelper.ToSafeSQL(contactItems["Telephone"]) : string.Empty);
            args.Add("QQ", contactItems.ContainsKey("QQ") ? StringHelper.ToSafeSQL(contactItems["QQ"]) : string.Empty);
            args.Add("MSN", contactItems.ContainsKey("MSN") ? StringHelper.ToSafeSQL(contactItems["MSN"]) : string.Empty);
            args.Add("Email", contactItems.ContainsKey("Email") ? StringHelper.ToSafeSQL(contactItems["Email"]) : string.Empty);
            args.Add("Rtx", contactItems.ContainsKey("Rtx") ? StringHelper.ToSafeSQL(contactItems["Rtx"]) : string.Empty);
            args.Add("PostCode", contactItems.ContainsKey("PostCode") ? StringHelper.ToSafeSQL(contactItems["PostCode"]) : string.Empty);
            args.Add("Address", contactItems.ContainsKey("Address") ? StringHelper.ToSafeSQL(contactItems["Address"]) : string.Empty);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetContactCard", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultCorporationAndDepartments(string accountId, string organizationIds)
        /// <summary>设置默认组织单位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationIds">组织单位标识，[0]公司标识，[1]一级部门标识，[2]二级部门标识，[3]三级部门标识。</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        public int SetDefaultCorporationAndDepartments(string accountId, string organizationIds)
        {
            if (string.IsNullOrEmpty(organizationIds))
            {
                throw new Exception("必须填写默认公司标识。");
            }

            string[] keys = organizationIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("CorporationId", StringHelper.ToSafeSQL(keys[0]));

            // 设置一级部门，如果为公司级负责人，则一级部门等于公司。
            if (keys.Length == 1)
            {
                args.Add("DepartmentId", StringHelper.ToSafeSQL(keys[0]));
            }
            else
            {
                args.Add("DepartmentId", StringHelper.ToSafeSQL(keys[1]));
            }

            // 设置二级部门。
            if (keys.Length >= 3)
            {
                args.Add("Department2Id", StringHelper.ToSafeSQL(keys[2]));
            }
            else
            {
                args.Add("Department2Id", string.Empty);
            }

            // 设置三级部门。
            if (keys.Length >= 4)
            {
                args.Add("Department3Id", StringHelper.ToSafeSQL(keys[3]));
            }
            else
            {
                args.Add("Department3Id", string.Empty);
            }

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultCorporationAndDepartments", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultOrganization(string accountId, string organizationId)
        /// <summary>设置默认组织单位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织单位标识</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        public int SetDefaultOrganization(string accountId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("OrganizationId", StringHelper.ToSafeSQL(organizationId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultOrganization", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultRole(string accountId, string roleId)
        /// <summary>设置默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetDefaultRole(string accountId, string roleId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("RoleId", StringHelper.ToSafeSQL(roleId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultRole", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultJob(string accountId, string jobId)
        /// <summary>设置默认职位信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="jobId">职位信息</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        public int SetDefaultJob(string accountId, string jobId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("JobId", StringHelper.ToSafeSQL(jobId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultJob", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultAssignedJob(string accountId, string assignedJobId)
        /// <summary>设置默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetDefaultAssignedJob(string accountId, string assignedJobId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("AssignedJobId", StringHelper.ToSafeSQL(assignedJobId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultAssignedJob", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDefaultJobGrade(string accountId, string jobGradeId)
        /// <summary>设置默认职级信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="jobGradeId">职级标识</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        public int SetDefaultJobGrade(string accountId, string jobGradeId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("JobGradeId", StringHelper.ToSafeSQL(jobGradeId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDefaultJobGrade", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(MemberInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">人员信息</param>
        public int SyncFromPackPage(IMemberInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion
    }
}
