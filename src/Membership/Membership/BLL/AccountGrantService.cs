namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary>帐号代理服务</summary>
    public class AccountGrantService : IAccountGrantService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IAccountGrantProvider provider = null;

        #region 构造函数:AccountGrantService()
        /// <summary>构造函数</summary>
        public AccountGrantService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IAccountGrantProvider>(typeof(IAccountGrantProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAccountGrantInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IAccountGrantInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAccountGrantInfo"/>详细信息</returns>
        public IAccountGrantInfo Save(IAccountGrantInfo param)
        {
            // 格式委托开始时间为 00:00:00
            param.GrantedTimeFrom = Convert.ToDateTime(param.GrantedTimeFrom.ToString("yyyy-MM-dd 00:00:00"));
            // 格式委托结束时间为 23:59:59，避免委托结束当天没有收到待办信息
            param.GrantedTimeTo = Convert.ToDateTime(param.GrantedTimeTo.ToString("yyyy-MM-dd 23:59:59"));

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
        /// <returns>返回实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        public IAccountGrantInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        public IList<IAccountGrantInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        public IList<IAccountGrantInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        public IList<IAccountGrantInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
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
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表实例<see cref="IAccountGrantInfo"/></returns>
        public IList<IAccountGrantInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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

        #region 函数:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>查询是否存在相关委托人的记录.</summary>
        /// <param name="grantorId">委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <returns>布尔值</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.provider.IsExistGrantor(grantorId, grantedTimeFrom, grantedTimeTo);
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
            return this.provider.IsExistGrantor(grantorId, grantedTimeFrom, grantedTimeTo, ignoreIds);
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
            return this.provider.IsExistGrantee(granteeId, grantedTimeFrom, grantedTimeTo);
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
            return this.provider.IsExistGrantee(granteeId, grantedTimeFrom, grantedTimeTo, ignoreIds);
        }
        #endregion

        #region 函数:Abort(string id)
        /// <summary>中止当前委托</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        public int Abort(string id)
        {
            return this.provider.Abort(id);
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
            return this.provider.SyncFromPackPage(param);
        }
        #endregion
    }
}