#region Copyright & Author
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
#endregion

namespace X3Platform.Membership.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class AccountGrantProvider : IAccountGrantProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Account_Grant";

        #region 构造函数:AccountGrantProvider()
        /// <summary>构造函数</summary>
        public AccountGrantProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
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

        #region 函数:Save(IAccountGrantInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAccountGrantInfo"/>详细信息</returns>
        public IAccountGrantInfo Save(IAccountGrantInfo param)
        {
            if (string.IsNullOrEmpty(param.Id) || !IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (IAccountGrantInfo)param;
        }
        #endregion

        #region 函数:Insert(IAccountGrantInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        public void Insert(IAccountGrantInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_Account_Grant_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IAccountGrantInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        public void Update(IAccountGrantInfo param)
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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        public IAccountGrantInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<IAccountGrantInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        public IList<IAccountGrantInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<IAccountGrantInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IAccountGrantInfo"/></returns>
        public IList<IAccountGrantInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IAccountGrantInfo> list = this.ibatisMapper.QueryForList<IAccountGrantInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

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

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>查询是否存在相关委托人的记录.</summary>
        /// <param name="grantorId">委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <returns>布尔值</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.IsExistGrantor(grantorId, grantedTimeFrom, grantedTimeTo, string.Empty);
        }
        #endregion

        #region 函数:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>查询是否存在相关委托人的记录.</summary>
        /// <param name="grantorId">委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <param name="ignoreIds">忽略委托标识</param>
        /// <returns>布尔值</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        {
            if (string.IsNullOrEmpty(grantorId)) { throw new Exception("委托人标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(ignoreIds))
            {
                args.Add("WhereClause", string.Format(@" GrantorId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' ))
AND IsAborted = 0 ", StringHelper.ToSafeSQL(grantorId), grantedTimeFrom, grantedTimeTo));
            }
            else
            {
                args.Add("WhereClause", string.Format(@" GrantorId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' )) 
AND IsAborted = 0 AND Id NOT IN ('{3}') ", StringHelper.ToSafeSQL(grantorId), grantedTimeFrom, grantedTimeTo, ignoreIds.Replace(",", "','")));
            }

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>查询是否存在相关被委托人的记录.</summary>
        /// <param name="granteeId">被委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <returns>布尔值</returns>
        public bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.IsExistGrantee(granteeId, grantedTimeFrom, grantedTimeTo, string.Empty);
        }
        #endregion

        #region 函数:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>查询是否存在相关被委托人的记录.</summary>
        /// <param name="granteeId">被委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <param name="ignoreIds">忽略委托标识</param>
        /// <returns>布尔值</returns>
        public bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        {
            if (string.IsNullOrEmpty(granteeId)) { throw new Exception("被委托人标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(ignoreIds))
            {
                args.Add("WhereClause", string.Format(@" GranteeId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' )) 
AND IsAborted = 0 ", StringHelper.ToSafeSQL(granteeId), grantedTimeFrom, grantedTimeTo));
            }
            else
            {
                args.Add("WhereClause", string.Format(@" GranteeId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' )) 
AND IsAborted = 0 AND Id NOT IN ('{3}') ", StringHelper.ToSafeSQL(granteeId), grantedTimeFrom, grantedTimeTo, ignoreIds.Replace(",", "','")));
            }

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Abort(string id)
        /// <summary>中止当前委托</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        public int Abort(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Abort", tableName)), args);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(IAccountGrantInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">帐号信息</param>
        public int SyncFromPackPage(IAccountGrantInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion
    }
}