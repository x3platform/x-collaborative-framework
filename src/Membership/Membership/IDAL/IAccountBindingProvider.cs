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
    [SpringObject("X3Platform.Membership.IDAL.IAccountBindingProvider")]
    public interface IAccountBindingProvider
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

		#region 函数:Save(AccountBindingInfo param)
		/// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountBindingInfo"/>详细信息</returns>
        AccountBindingInfo Save(AccountBindingInfo param);
        #endregion

        #region 函数:Insert(AccountBindingInfo param)
		/// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        void Insert(AccountBindingInfo param);
        #endregion

        #region 函数:Update(AccountBindingInfo param)
		/// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        void Update(AccountBindingInfo param);
        #endregion

		#region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string accountId, string bindingType)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <returns>返回实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        AccountBindingInfo FindOne(string accountId, string bindingType);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户的所有相关记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <returns>返回所有实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        IList<AccountBindingInfo> FindAllByAccountId(string accountId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string accountId, string bindingType)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <returns>布尔值</returns>
        bool IsExist(string accountId, string bindingType);
        #endregion

        #region 函数:Bind(string accountId, string bindingType, string bindingObjectId, string bindingOptions)
        /// <summary>绑定第三方帐号绑定关系</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <param name="bindingObjectId">绑定对象唯一标识</param>
        /// <param name="bindingOptions">绑定的参数信息</param>
        /// <returns></returns>
        int Bind(string accountId, string bindingType, string bindingObjectId, string bindingOptions);
        #endregion

        #region 函数:Unbind(string accountId, string bindingType, string bindingObjectId)
        /// <summary>解除第三方帐号绑定关系</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <param name="bindingObjectId">绑定对象唯一标识</param>
        /// <returns></returns>
        int Unbind(string accountId, string bindingType, string bindingObjectId);
        #endregion
    }
}
