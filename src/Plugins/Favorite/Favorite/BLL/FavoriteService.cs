#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :FavoriteService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Favorite.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Plugins.Favorite.Configuration;
    using X3Platform.Plugins.Favorite.IBLL;
    using X3Platform.Plugins.Favorite.IDAL;
    using X3Platform.Plugins.Favorite.Model;
    using X3Platform.CategoryIndexes;
    using X3Platform.Apps;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class FavoriteService : IFavoriteService
    {
        /// <summary>配置</summary>
        private FavoriteConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IFavoriteProvider provider = null;

        #region 构造函数:FavoriteService()
        /// <summary>构造函数</summary>
        public FavoriteService()
        {
            this.configuration = FavoriteConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(FavoriteConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IFavoriteProvider>(typeof(IFavoriteProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FavoriteInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(FavoriteInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="FavoriteInfo"/>详细信息</param>
        /// <returns>实例<see cref="FavoriteInfo"/>详细信息</returns>
        public FavoriteInfo Save(FavoriteInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="FavoriteInfo"/>的详细信息</returns>
        public FavoriteInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="FavoriteInfo"/>的详细信息</returns>
        public IList<FavoriteInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="FavoriteInfo"/>的详细信息</returns>
        public IList<FavoriteInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="FavoriteInfo"/>的详细信息</returns>
        public IList<FavoriteInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(this.BindAuthorizationScopeSQL(whereClause), length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="FavoriteInfo"/></returns>
        public IList<FavoriteInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
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

        #region 函数:SetClick(string id)
        /// <summary>
        /// 修改访收藏夹问量
        /// </summary>
        /// <param name="id">表示</param>
        /// <returns>布尔值</returns>
        public bool SetClick(string id)
        {
            return provider.SetClick(id);
        }
        #endregion

        #region 函数:FetchCategoryIndex(string accountIds)
        ///<summary>根据用户标识获取类别索引</summary>
        ///<param name="accountIds">用户标识</param>
        ///<returns>类别索引对象</returns>
        public ICategoryIndex FetchCategoryIndex(string accountIds)
        {
            return provider.FetchCategoryIndex(accountIds);
        }
        #endregion

        #region 函数:FetchCategoryIndex(string accountIds, string prefixCategoryIndex)
        ///<summary>根据用户标识获取类别索引</summary>
        ///<param name="accountIds">用户标识</param>
        ///<param name="prefixCategoryIndex">类别索引前缀</param>
        ///<returns>类别索引对象</returns>
        public ICategoryIndex FetchCategoryIndex(string accountIds, string prefixCategoryIndex)
        {
            return provider.FetchCategoryIndex(accountIds, prefixCategoryIndex);
        }
        #endregion

        // -------------------------------------------------------
        // 权限
        // -------------------------------------------------------

        #region 私有函数:BindAuthorizationScopeSQL(string whereClause)
        /// <summary>绑定SQL查询条件</summary>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <returns></returns>
        private string BindAuthorizationScopeSQL(string whereClause)
        {
            // 普通用户能查询数据的最大范围
            // 【本人】-【AccountId】

            IAccountInfo account = KernelContext.Current.User;

            string scope = @" ( AccountId = ##" + account.Id + "## ) ";

            if (whereClause.IndexOf(scope) == -1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
            }

            return whereClause;
        }
        #endregion
    }
}
