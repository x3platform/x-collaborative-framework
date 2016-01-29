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
    using X3Platform.DigitalNumber;
    #endregion

    /// <summary></summary>
    public class AccountFriendProvider : IAccountFriendProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Account_Friend";

        #region 构造函数:AccountFriendProvider()
        /// <summary>构造函数</summary>
        public AccountFriendProvider()
        {
            this.ibatisMapping = MembershipConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string accountId, string friendAccountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="friendAccountId">好友的帐号唯一标识</param>
        /// <returns>返回实例<see cref="AccountFriendInfo"/>的详细信息</returns>
        public AccountFriendInfo FindOne(string accountId, string friendAccountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("FriendAccountId", StringHelper.ToSafeSQL(friendAccountId));

            return this.ibatisMapper.QueryForObject<AccountFriendInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="AccountFriendInfo"/>实例的详细信息</returns>
        public IList<AccountFriendInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } });
            string orderBy = query.GetOrderBySql(" UpdateDate DESC ");

            args.Add("WhereClause", whereClause);
            args.Add("OrderBy", orderBy);
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<AccountFriendInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
        /// <returns>返回一个列表实例<see cref="AccountFriendInfo"/></returns> 
        public IList<AccountFriendInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = "";

            if (query.Variables["scence"] == "Query")
            {
                whereClause = " Status IN (" + StringHelper.ToSafeSQL(query.Where["Status"].ToString()) + ") ";

                if (query.Where.ContainsKey("SearchText") && string.IsNullOrEmpty(query.Where.ContainsKey("SearchText").ToString()))
                {
                    whereClause = " AND T.CategoryIndex LIKE '%" + StringHelper.ToSafeSQL(query.Where["SearchText"].ToString()) + "%'";
                }

                args.Add("WhereClause", whereClause);
            }
            else if (query.Variables["scence"] == "QueryMyList")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

                whereClause = " AccountId = '" + query.Variables["accountId"] + "' ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " FriendDisplayName LIKE '" + searchText + "' ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" FriendDisplayName DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<AccountFriendInfo> list = this.ibatisMapper.QueryForList<AccountFriendInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:GetAcceptListPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="AccountFriendInfo"/></returns> 
        public DataTable GetAcceptListPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            // string whereClause = "";
            // whereClause = " AccountId = '" + query.Variables["accountId"] + "' ";

            args.Add("WhereClause", " AccountId = '" + query.Variables["accountId"] + "' ");

            args.Add("OrderBy", query.GetOrderBySql(" CreatedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_Accept_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_Accept_GetRowCount", tableName)), args));

            return table;
        }
        #endregion

        #region 函数:IsExist(string accountId, string friendAccountId)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="friendAccountId">好友帐号唯一标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string accountId, string friendAccountId)
        {
            if (string.IsNullOrEmpty(accountId)) { throw new Exception("帐号唯一标识不能为空。"); }
            if (string.IsNullOrEmpty(friendAccountId)) { throw new Exception("好友帐号唯一标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId = '{0}' AND FriendAccountId = '{1}' ", StringHelper.ToSafeSQL(accountId), StringHelper.ToSafeSQL(friendAccountId)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Invite(string accountId, string friendAccountId, string reason)
        /// <summary>邀请好友</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        /// <param name="reason">原因</param>
        public int Invite(string accountId, string friendAccountId, string reason)
        {
            accountId = StringHelper.ToSafeSQL(accountId);
            friendAccountId = StringHelper.ToSafeSQL(friendAccountId);

            if (!this.IsExist(accountId, friendAccountId))
            {
                // 加入好友
                this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)),
                    new AccountFriendInfo() { AccountId = accountId, FriendAccountId = friendAccountId, Status = 0 });

                // 加入好友邀请列表

                Dictionary<string, object> args = new Dictionary<string, object>();

                // 同意好友的邀请关系
                args.Add("Id", DigitalNumberContext.Generate("Key_Guid"));
                args.Add("AccountId", friendAccountId);
                args.Add("FriendAccountId", accountId);
                args.Add("Reason", reason);
                args.Add("Status", 0);

                this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Accept_Insert", tableName)), args);
            }

            return 0;
        }
        #endregion

        #region 函数:Accept(string accountId, string friendAccountId)
        /// <summary>同意好友邀请</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        public int Accept(string accountId, string friendAccountId)
        {
            accountId = StringHelper.ToSafeSQL(accountId);
            friendAccountId = StringHelper.ToSafeSQL(friendAccountId);

            if (!this.IsExist(accountId, friendAccountId))
            {
                Dictionary<string, object> args = new Dictionary<string, object>();

                // 同意好友的邀请关系
                args.Add("AccountId", friendAccountId);
                args.Add("FriendAccountId", accountId);
                args.Add("Status", 1);

                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", tableName)), args);

                args.Clear();

                // 同意好友的邀请关系
                args.Add("AccountId", accountId);
                args.Add("FriendAccountId", friendAccountId);
                args.Add("Status", 1);

                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Accept_SetStatus", tableName)), args);

                // 自动添加对方为好友关系
                this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)),
                    new AccountFriendInfo() { AccountId = accountId, FriendAccountId = friendAccountId, Status = 1 });
            }

            return 0;
        }
        #endregion

        #region 函数:Unfriend(string accountId, string friendAccountId)
        /// <summary>解除好友关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        public int Unfriend(string accountId, string friendAccountId)
        {
            accountId = StringHelper.ToSafeSQL(accountId);
            friendAccountId = StringHelper.ToSafeSQL(friendAccountId);

            Dictionary<string, object> args = new Dictionary<string, object>();

            // 解除自身好友关系
            args.Add("WhereClause", string.Format(" AccountId = '{0}' AND FriendAccountId = '{1}' ", accountId, friendAccountId));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Accept_Delete", tableName)), args);

            // 解除对方好友关系
            args["WhereClause"] = string.Format(" AccountId = '{0}' AND FriendAccountId = '{1}' ", friendAccountId, accountId);

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Accept_Delete", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetDisplayName(string accountId, string friendAccountId, string friendDisplayName)
        /// <summary>设置好友的显示名称</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        /// <param name="friendDisplayName">好友显示名称</param>
        /// <returns>0:代表成功</returns>
        public int SetDisplayName(string accountId, string friendAccountId, string friendDisplayName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("FriendAccountId", StringHelper.ToSafeSQL(friendAccountId));
            args.Add("FriendDisplayName", StringHelper.ToSafeSQL(friendDisplayName));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetDisplayName", tableName)), args);

            return 0;
        }
        #endregion
    }
}
