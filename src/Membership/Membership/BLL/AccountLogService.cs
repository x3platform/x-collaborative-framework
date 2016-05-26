namespace X3Platform.Membership.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class AccountLogService : IAccountLogService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IAccountLogProvider provider = null;

        #region 构造函数:AccountLogService()
        /// <summary>构造函数</summary>
        public AccountLogService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IAccountLogProvider>(typeof(IAccountLogProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountLogInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(AccountLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountLogInfo"/>详细信息</returns>
        public AccountLogInfo Save(AccountLogInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="AccountLogInfo"/>的详细信息</returns>
        public AccountLogInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        public IList<AccountLogInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        public IList<AccountLogInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        public IList<AccountLogInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        public IList<AccountLogInfo> FindAllByAccountId(string accountId)
        {
            return this.provider.FindAllByAccountId(accountId);
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
        /// <returns>返回一个列表实例<see cref="AccountLogInfo"/></returns>
        public IList<AccountLogInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:Log(string accountId, string optionName, string description)
        /// <summary>保存日志信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="optionName">操作名称:登录 编辑 删除</param>
        /// <param name="description">描述信息</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        public int Log(string accountId, string optionName, string description)
        {
            return this.Log(accountId, optionName, description, string.Empty);
        }
        #endregion

        #region 函数:Log(string accountId, string optionName, string description, string optionAccountId)
        /// <summary>保存帐号操作日志信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="optionName">操作名称: 查看</param>
        /// <param name="description">描述信息</param>
        /// <param name="optionAccountId">操作人的帐号标识</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        public int Log(string accountId, string optionName, string description, string optionAccountId)
        {
            return this.Log(accountId, optionName, null, description, optionAccountId);
        }
        #endregion

        #region 函数:Log(string accountId, string optionName, IAccountInfo originalObject, string description, string optionAccountId)
        /// <summary>保存日志信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="optionName">操作名称:登录 编辑 删除</param>
        /// <param name="originalObject">原始的对象信息</param>
        /// <param name="description">描述信息</param>
        /// <param name="optionAccountId">操作人的帐号标识</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        public int Log(string accountId, string optionName, IAccountInfo originalObject, string description, string optionAccountId)
        {
            IAccountInfo account = KernelContext.Current.User;

            // 保存实体数据操作记录
            AccountLogInfo param = new AccountLogInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");
            param.AccountId = accountId;
            param.OperatedBy = optionAccountId;
            param.OperationName = optionName;
            param.OriginalObjectValue = originalObject == null ? string.Empty : originalObject.Serializable();
            param.Description = description;
            param.CreatedDate = DateTime.Now;

            param = this.Save(param);

            return param == null ? 1 : 0;
        }
        #endregion
    }
}
