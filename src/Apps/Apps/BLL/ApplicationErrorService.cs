﻿namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationErrorService : IApplicationErrorService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationErrorProvider provider = null;

        #region 构造函数:ApplicationErrorService()
        /// <summary>构造函数</summary>
        public ApplicationErrorService()
        {
            // 读取配置信息
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationErrorProvider>(typeof(IApplicationErrorProvider));
        }
        #endregion

        #region 索引:this[string code]
        /// <summary>索引</summary>
        /// <param name="code">错误代码</param>
        /// <returns></returns>
        public ApplicationErrorInfo this[string code]
        {
            get { return this.FindOneByCode(code); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationErrorInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationErrorInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationErrorInfo"/>详细信息</returns>
        public ApplicationErrorInfo Save(ApplicationErrorInfo param)
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
        /// <returns>返回实例<see cref="ApplicationErrorInfo"/>的详细信息</returns>
        public ApplicationErrorInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByCode(string code)
        /// <summary>查询某条记录</summary>
        /// <param name="code">代码</param>
        /// <returns>返回实例<see cref="ApplicationErrorInfo"/>的详细信息</returns>
        public ApplicationErrorInfo FindOneByCode(string code)
        {
            return this.provider.FindOneByCode(code);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationErrorInfo"/>的详细信息</returns>
        public IList<ApplicationErrorInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationErrorInfo"/>的详细信息</returns>
        public IList<ApplicationErrorInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationErrorInfo"/>的详细信息</returns>
        public IList<ApplicationErrorInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationErrorInfo"/></returns>
        public IList<ApplicationErrorInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion
    }
}
