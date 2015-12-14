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
    [SpringObject("X3Platform.Membership.IBLL.IAccountBindingService")]
    public interface IAccountBindingService
    {
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
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <param name="bindingObjectId">绑定对象唯一标识</param>
        /// <param name="bindingOptions">绑定的参数信息</param>
        /// <returns></returns>
        int Bind(string accountId, string bindingType, string bindingObjectId, string bindingOptions);
        #endregion
    }
}
