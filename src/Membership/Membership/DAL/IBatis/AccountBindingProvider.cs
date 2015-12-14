namespace X3Platform.Membership.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public class AccountBindingProvider : IAccountBindingProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Account_Binding";

        #region 构造函数:AccountBindingProvider()
        /// <summary>构造函数</summary>
        public AccountBindingProvider()
        {
            this.ibatisMapping = MembershipConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
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

        #region 函数:Save(AccountBindingInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountBindingInfo"/>详细信息</returns>
        public AccountBindingInfo Save(AccountBindingInfo param)
        {
            if (!IsExist(param.AccountId, param.BindingType))
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

        #region 函数:Insert(AccountBindingInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        public void Insert(AccountBindingInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(AccountBindingInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        public void Update(AccountBindingInfo param)
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

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string accountId, string bindingType)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <returns>返回实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        public AccountBindingInfo FindOne(string accountId, string bindingType)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("BindingType", StringHelper.ToSafeSQL(bindingType));

            return this.ibatisMapper.QueryForObject<AccountBindingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户的所有相关记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <returns>返回所有实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        public IList<AccountBindingInfo> FindAllByAccountId(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId = '{0}' ", StringHelper.ToSafeSQL(accountId)));

            return this.ibatisMapper.QueryForList<AccountBindingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllByAccountId", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="AccountBindingInfo"/>实例的详细信息</returns>
        public IList<AccountBindingInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } });
            string orderBy = query.GetOrderBySql(" UpdateDate DESC ");

            args.Add("WhereClause", whereClause);
            args.Add("OrderBy", orderBy);
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<AccountBindingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string accountId, string bindingType)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string accountId, string bindingType)
        {
            if (string.IsNullOrEmpty(accountId)) { throw new Exception("实例标识不能为空。"); }

            if (string.IsNullOrEmpty(bindingType)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId = '{0}' AND BindingType = '{1}' ", StringHelper.ToSafeSQL(accountId), StringHelper.ToSafeSQL(bindingType)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Bind(string accountId, string bindingType, string bindingObjectId, string bindingOptions)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <param name="bindingObjectId">绑定对象唯一标识</param>
        /// <param name="bindingOptions">绑定的参数信息</param>
        /// <returns></returns>
        public int Bind(string accountId, string bindingType, string bindingObjectId, string bindingOptions)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("BindingType", StringHelper.ToSafeSQL(bindingType));
            args.Add("BindingObjectId", StringHelper.ToSafeSQL(bindingObjectId));
            args.Add("BindingOptions", StringHelper.ToSafeSQL(bindingOptions));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Bind", tableName)), args);

            return 0;
        }
        #endregion

    }
}
