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

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountBindingInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

		#region 函数:Save(AccountBindingInfo param)
		/// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountBindingInfo"/>详细信息</returns>
        public AccountBindingInfo Save(AccountBindingInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion
        
		#region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
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
        /// <returns>返回实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        public AccountBindingInfo FindOne(string id)
        {
            return this.provider.FindOne(id) ;
        }
		#endregion
        
        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        public IList<AccountBindingInfo> FindAll(DataQuery query)
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
        /// <returns>返回一个列表实例<see cref="AccountBindingInfo"/></returns> 
        public IList<AccountBindingInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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
    }
}