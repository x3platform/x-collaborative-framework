﻿// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityImplementationService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Entities.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;

    /// <summary></summary>
    public class EntityImplementationService : IEntityImplementationService
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IEntityImplementationProvider provider = null;

        #region 构造函数:EntityImplementationService()
        /// <summary>构造函数</summary>
        public EntityImplementationService()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IEntityImplementationProvider>(typeof(IEntityImplementationProvider));
        }
        #endregion

        //-------------------------------------------------------
        // 保存 删除
        //-------------------------------------------------------

        #region 函数:Save(string customTableName, EntityImplementationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="param">实例<see cref="EntityImplementationInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntityImplementationInfo"/>详细信息</returns>
        public EntityImplementationInfo Save(string customTableName, EntityImplementationInfo param)
        {
            return provider.Save(customTableName, param);
        }
        #endregion

        #region 函数:Delete(string customTableName, string id)
        /// <summary>删除记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">记录标识</param>
        public void Delete(string customTableName, string id)
        {
            provider.Delete(customTableName, id);
        }
        #endregion

        #region 函数:Delete(string customTableName, string entityId, string entityClassName)
        /// <summary>删除记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        public void Delete(string customTableName, string entityId, string entityClassName)
        {
            provider.Delete(customTableName, entityId, entityClassName);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindAll(string customTableName, string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntityImplementationInfo"/>的详细信息</returns>
        public IList<EntityImplementationInfo> FindAll(string customTableName, string whereClause, int length)
        {
            return provider.FindAll(customTableName, whereClause, length);
        }
        #endregion


        #region 函数:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回所有实例<see cref="EntityImplementationInfo"/>的详细信息</returns>
        public IList<EntityImplementationInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        {
            return provider.FindAllByEntityId(customTableName, entityId, entityClassName);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:IsExist(string customTableName, string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="customTableName">自定义数据表名称</param>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string customTableName, string id)
        {
            return provider.IsExist(customTableName, id);
        }
        #endregion
    }
}
