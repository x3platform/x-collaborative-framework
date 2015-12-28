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
    #endregion

    /// <summary></summary>
    public class AccountBindingService : IAccountBindingService
    {
        /// <summary>数据提供器</summary>
        private IAccountBindingProvider provider = null;

        #region 构造函数:AccountBindingService()
        /// <summary>构造函数</summary>
        public AccountBindingService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = MembershipConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IAccountBindingProvider>(typeof(IAccountBindingProvider));
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
            return this.provider.FindOne(accountId, bindingType);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户的所有相关记录</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <returns>返回所有实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        public IList<AccountBindingInfo> FindAllByAccountId(string accountId)
        {
            return this.provider.FindAllByAccountId(accountId);
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
            return this.provider.IsExist(accountId, bindingType);
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
            return this.provider.Bind(accountId, bindingType, bindingObjectId, bindingOptions);
        }
        #endregion

        #region 函数:Unbind(string accountId, string bindingType, string bindingObjectId)
        /// <summary>解除第三方帐号绑定关系</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="bindingType">绑定类型</param>
        /// <param name="bindingObjectId">绑定对象唯一标识</param>
        /// <returns></returns>
        public int Unbind(string accountId, string bindingType, string bindingObjectId)
        {
            return this.provider.Unbind(accountId, bindingType, bindingObjectId);
        }
        #endregion
    }
}