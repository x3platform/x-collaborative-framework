namespace X3Platform.Membership.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using System.Data;
    #endregion

    /// <summary></summary>
    public class AccountFriendService : IAccountFriendService
    {
        /// <summary>数据提供器</summary>
        private IAccountFriendProvider provider = null;

        #region 构造函数:AccountFriendService()
        /// <summary>构造函数</summary>
        public AccountFriendService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = MembershipConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IAccountFriendProvider>(typeof(IAccountFriendProvider));
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
            return this.provider.FindOne(accountId, friendAccountId);
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有实例<see cref="AccountFriendInfo"/>的详细信息</returns>
        public IList<AccountFriendInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
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
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
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
            return this.provider.GetAcceptListPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string accountId, string friendAccountId)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="friendAccountId">好友帐号唯一标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string accountId, string friendAccountId)
        {
            return this.provider.IsExist(accountId, friendAccountId);
        }
        #endregion

        #region 函数:Invite(string accountId, string friendAccountId, string reason)
        /// <summary>邀请好友</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        /// <param name="reason">原因</param>
        public int Invite(string accountId, string friendAccountId, string reason)
        {
            if (string.IsNullOrEmpty(accountId)) { throw new Exception("帐号标识不能为空."); }

            if (accountId == friendAccountId) { throw new Exception("自己的好友帐号不能为自己本身."); }

            return this.provider.Invite(accountId, friendAccountId, reason);
        }
        #endregion

        #region 函数:Accept(string accountId, string friendAccountId)
        /// <summary>同意好友邀请</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        public int Accept(string accountId, string friendAccountId)
        {
            if (string.IsNullOrEmpty(accountId)) { throw new Exception("帐号标识不能为空."); }

            if (accountId == friendAccountId) { throw new Exception("自己的好友帐号不能为自己本身."); }

            return this.provider.Accept(accountId, friendAccountId);
        }
        #endregion

        #region 函数:Unfriend(string accountId, string friendAccountId)
        /// <summary>解除好友关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="friendAccountId">帐号标识</param>
        public int Unfriend(string accountId, string friendAccountId)
        {
            if (string.IsNullOrEmpty(accountId)) { throw new Exception("帐号标识不能为空."); }

            if (accountId == friendAccountId) { throw new Exception("自己的好友帐号不能为自己本身."); }

            return this.provider.Unfriend(accountId, friendAccountId);
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
            return this.provider.SetDisplayName(accountId, friendAccountId, friendDisplayName);
        }
        #endregion
    }
}