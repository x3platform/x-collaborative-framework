namespace X3Platform.Security.Authority.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using Common.Logging;

    using X3Platform.CacheBuffer;
    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Security.Authority.Configuration;
    using X3Platform.Security.Authority.IBLL;
    using X3Platform.Security.Authority.IDAL;
    #endregion

    /// <summary>权限服务</summary>
    public class AuthorityService : IAuthorityService
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>配置</summary>
        private AuthorityConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IAuthorityProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, AuthorityInfo> dictionary = new Dictionary<string, AuthorityInfo>();

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:AuthorityService()
        /// <summary>构造函数</summary>
        public AuthorityService()
        {
            this.configuration = AuthorityConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AuthorityConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<IAuthorityProvider>(typeof(IAuthorityProvider));
        }
        #endregion

        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name">权限名称</param>
        /// <returns></returns>
        public AuthorityInfo this[string name]
        {
            get
            {
                AuthorityInfo authority = this.FindOneByName(name);

                if (logger.IsDebugEnabled) { logger.Debug(authority); }

                if (authority == null)
                {
                    throw new NullReferenceException("未找到【" + name + "】权限信息。");
                }

                return authority;
            }
        }
        #endregion

        //-------------------------------------------------------
        // 保存 删除
        //-------------------------------------------------------

        #region 函数:Save(AuthorityInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="AuthorityInfo"/>详细信息</param>
        /// <returns>AuthorityInfo 实例详细信息</returns>
        public AuthorityInfo Save(AuthorityInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号分开.</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AuthorityInfo id号</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
        public AuthorityInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">权限名称</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
        public AuthorityInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        public IList<AuthorityInfo> FindAll()
        {
            return this.FindAll(new DataQuery());
        }
        #endregion

        #region 属性:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="AuthorityInfo"/>实例的详细信息</returns>
        public IList<AuthorityInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 属性:Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个<see cref="AuthorityInfo"/>列表实例</returns> 
        public IList<AuthorityInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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
