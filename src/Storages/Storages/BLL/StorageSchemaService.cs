#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :StorageSchemaService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

#region Using Libraries
using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

//using X3Platform.Apps;
//using X3Platform.Apps.Model;

using X3Platform.Storages.Configuration;
using X3Platform.Storages.IBLL;
using X3Platform.Storages.IDAL;
using X3Platform.Storages.Model;
#endregion

namespace X3Platform.Storages.BLL
{
    /// <summary></summary>
    public class StorageSchemaService : IStorageSchemaService
    {
        /// <summary>配置</summary>
        private StoragesConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IStorageSchemaProvider provider = null;

        #region 构造函数:StorageSchemaService()
        /// <summary>构造函数</summary>
        public StorageSchemaService()
        {
            this.configuration = StoragesConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(StoragesConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IStorageSchemaProvider>(typeof(IStorageSchemaProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StorageSchemaInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(StorageSchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="StorageSchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="StorageSchemaInfo"/>详细信息</returns>
        public StorageSchemaInfo Save(StorageSchemaInfo param)
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
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        public StorageSchemaInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByApplicationId(string applicationId)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        public StorageSchemaInfo FindOneByApplicationId(string applicationId)
        {
            return this.provider.FindOneByApplicationId(applicationId);
        }
        #endregion

        #region 函数:FindOneByApplicationName(string applicationName)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationName">所属应用名称</param>
        /// <returns>返回实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        public StorageSchemaInfo FindOneByApplicationName(string applicationName)
        {
            return this.provider.FindOneByApplicationName(applicationName);
            
            // ApplicationInfo application = AppsContext.Instance.ApplicationService[applicationName];
           
            // return this.FindOneByApplicationId(application.Id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        public IList<StorageSchemaInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        public IList<StorageSchemaInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="StorageSchemaInfo"/>的详细信息</returns>
        public IList<StorageSchemaInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
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
        /// <returns>返回一个列表实例<see cref="StorageSchemaInfo"/></returns>
        public IList<StorageSchemaInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
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
