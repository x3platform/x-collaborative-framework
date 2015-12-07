namespace X3Platform.Membership.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    using X3Platform.Data;
    using X3Platform.Spring;
    
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IAccountFriendService")]
    public interface IAccountFriendService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountFriendInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

		#region 函数:Save(AccountFriendInfo param)
		/// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountFriendInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountFriendInfo"/>详细信息</returns>
        AccountFriendInfo Save(AccountFriendInfo param);
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

		#region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:Invite(string accountId, string friendAccountId)
        /// <summary>邀请好友</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        int Invite(string accountId, string friendAccountId);
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
