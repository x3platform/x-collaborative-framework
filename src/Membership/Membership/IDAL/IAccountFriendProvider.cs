namespace X3Platform.Membership.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    
    using X3Platform.Data;
    using X3Platform.Spring;
    
    using X3Platform.Membership.Model;
    #endregion
    
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IAccountFriendProvider")]
    public interface IAccountFriendProvider
    {
        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        void BeginTransaction();
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        void CommitTransaction();
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

		#region 函数:Save(AccountFriendInfo param)
		/// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountFriendInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountFriendInfo"/>详细信息</returns>
        AccountFriendInfo Save(AccountFriendInfo param);
        #endregion

        #region 函数:Insert(AccountFriendInfo param)
		/// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="AccountFriendInfo"/>详细信息</param>
        void Insert(AccountFriendInfo param);
        #endregion

        #region 函数:Update(AccountFriendInfo param)
		/// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="AccountFriendInfo"/>详细信息</param>
        void Update(AccountFriendInfo param);
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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="AccountFriendInfo"/>的详细信息</returns>
        AccountFriendInfo FindOne(string id);
		#endregion
        
        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有实例<see cref="AccountFriendInfo"/>的详细信息</returns>
        IList<AccountFriendInfo> FindAll(DataQuery query);
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
        IList<AccountFriendInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetAcceptListPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="AccountFriendInfo"/></returns> 
        DataTable GetAcceptListPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string accountId, string friendAccountId)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="friendAccountId">好友帐号唯一标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string accountId, string friendAccountId);
        #endregion

        #region 函数:Invite(string accountId, string friendAccountId, string reason)
        /// <summary>邀请好友</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        /// <param name="reason">原因</param>
        int Invite(string accountId, string friendAccountId, string reason);
        #endregion

        #region 函数:Accept(string accountId, string friendAccountId)
        /// <summary>同意好友邀请</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        int Accept(string accountId, string friendAccountId);
        #endregion

        #region 函数:Unfriend(string accountId, string friendAccountId)
        /// <summary>解除好友关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        int Unfriend(string accountId, string friendAccountId);
        #endregion
    }
}
