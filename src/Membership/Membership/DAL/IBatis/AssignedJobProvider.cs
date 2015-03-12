// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IAssignedJobProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [DataObject]
    public class AssignedJobProvider : IAssignedJobProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_AssignedJob";

        #region 构造函数:AssignedJobProvider()
        /// <summary>构造函数</summary>
        public AssignedJobProvider()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(IAssignedJobInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAssignedJobInfo"/>详细信息</returns>
        public IAssignedJobInfo Save(IAssignedJobInfo param)
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

        #region 函数:Insert(IAssignedJobInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        public void Insert(IAssignedJobInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IAssignedJobInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        public void Update(IAssignedJobInfo param)
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

                // 删除群组与帐号的关系
                this.RemoveRelation(string.Format(" AssignedJobId = ##{0}## ", id));

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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IAssignedJobInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<IAssignedJobInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IList<IAssignedJobInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<IAssignedJobInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户的所有岗位信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByAccountId(string accountId)
        {
            string whereClause = " Id IN ( SELECT AssignedJobId FROM [tb_Account_AssignedJob] WHERE AccountId = ##" + accountId + "## ) ";

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByOrganizationId(string organizationId)
        /// <summary>查询某个组织下面所有的角色</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IRoleInfo 实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByOrganizationId(string organizationId)
        {
            string whereClause = string.Format(" OrganizationId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(organizationId));

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllPartTimeJobsByAccountId(string accountId)
        /// <summary>查询某个用户的所有岗位信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllPartTimeJobsByAccountId(string accountId)
        {
            string whereClause = " Id IN ( SELECT AssignedJobId FROM tb_Account_AssignedJob WHERE AccountId = ##" + accountId + "## AND IsDefault = 0 ) ";

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByRoleId(string roleId)
        /// <summary>查询某个角色的所对应岗位信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByRoleId(string roleId)
        {
            string whereClause = " JobId IN ( SELECT JobId FROM tb_Job_StandardRole WHERE StandardRoleId = ##" + roleId + "## ) AND OrganizationId IN (SELECT OrganizationId FROM tb_Role WHERE Id = ##" + roleId + "##) ";

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
        /// <returns>返回一个列表实例<see cref="IAssignedJobInfo"/></returns>
        public IList<IAssignedJobInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

            args.Add("RowCount", 0);

            IList<IAssignedJobInfo> list = this.ibatisMapper.QueryForList<IAssignedJobInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">岗位标识</param>
        /// <param name="name">岗位名称</param>
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

        #region 函数:SetJobId(string assignedJobId, string jobId)
        /// <summary>设置岗位与相关职位的关系</summary>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="jobId">职位标识</param>
        public int SetJobId(string assignedJobId, string jobId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", assignedJobId);
            args.Add("JobId", jobId);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetJobId", tableName)), args);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和岗位关系
        // -------------------------------------------------------

        #region 函数:AddRelation(string accountId, string assignedJobId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关岗位的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="isDefault">是否是默认岗位</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string assignedJobId, bool isDefault, DateTime beginDate, DateTime endDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("AssignedJobId", assignedJobId);
            args.Add("IsDefault", isDefault);
            args.Add("BeginDate", beginDate);
            args.Add("EndDate", endDate);

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string roleId, DateTime endDate)
        /// <summary>续约帐号与相关岗位的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string assignedJobId, DateTime endDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("AssignedJobId", assignedJobId);
            args.Add("EndDate", endDate);

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_ExtendRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 私有函数:RemoveAllRelation(string whereClause)
        /// <summary>移除帐号相关岗位的关系</summary>
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

        #region 函数:RemoveRelation(string accountId, string assignedJobId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        public int RemoveRelation(string accountId, string assignedJobId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND AssignedJobId = ##{1}## ) ", accountId, assignedJobId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关岗位的默认关系(默认岗位)</summary>
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
        /// <summary>移除帐号相关岗位的非默认关系(兼职岗位)</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND IsDefault = 0 ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的岗位关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND EndDate < ##{1}## ) ", accountId, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            return this.RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关岗位的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        public bool HasDefaultRelation(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasDefaultRelation", tableName)), args)) == 0) ? false : true;
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

        #region 函数:SetDefaultRelation(string accountId, string assignedJobId)
        /// <summary>设置帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        public int SetDefaultRelation(string accountId, string assignedJobId)
        {
            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
                args.Add("AssignedJobId", StringHelper.ToSafeSQL(assignedJobId));

                // 1.添加关系
                AddRelation(accountId, assignedJobId, true, DateTime.Now, DateTime.MaxValue);
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

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(IAssignedJobInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">岗位信息</param>
        public int SyncFromPackPage(IAssignedJobInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion
    }
}
