﻿#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalShortcutGroupProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Navigation.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Navigation.Configuration;
    using X3Platform.Navigation.IDAL;
    using X3Platform.Navigation.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class NavigationPortalShortcutGroupProvider : INavigationPortalShortcutGroupProvider
    {
        /// <summary>配置</summary>
        private NavigationConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Navigation_Portal_ShortcutGroup";

        #region 构造函数:NavigationPortalShortcutGroupProvider()
        /// <summary>构造函数</summary>
        public NavigationPortalShortcutGroupProvider()
        {
            this.configuration = NavigationConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(NavigationPortalShortcutGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="NavigationPortalShortcutGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="NavigationPortalShortcutGroupInfo"/>详细信息</returns>
        public NavigationPortalShortcutGroupInfo Save(NavigationPortalShortcutGroupInfo param)
        {
            if (!this.IsExist(param.Id))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(NavigationPortalShortcutGroupInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="NavigationPortalShortcutGroupInfo"/>详细信息</param>
        public void Insert(NavigationPortalShortcutGroupInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(NavigationPortalShortcutGroupInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="NavigationPortalShortcutGroupInfo"/>详细信息</param>
        public void Update(NavigationPortalShortcutGroupInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="NavigationPortalShortcutGroupInfo"/>的详细信息</returns>
        public NavigationPortalShortcutGroupInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<NavigationPortalShortcutGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="NavigationPortalShortcutGroupInfo"/>的详细信息</returns>
        public IList<NavigationPortalShortcutGroupInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<NavigationPortalShortcutGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAllByPortalId(string portalId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="portalId">所属门户标识</param>
        /// <returns>返回所有实例<see cref="NavigationPortalShortcutGroupInfo"/>的详细信息</returns>
        public IList<NavigationPortalShortcutGroupInfo> FindAllByPortalId(string portalId)
        {
            string whereClause = string.Format(" PortalId = ##{0}## ORDER BY OrderId ", portalId);

            return this.FindAll(whereClause, 0);
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
        /// <returns>返回一个列表实例<see cref="NavigationPortalShortcutGroupInfo"/></returns>
        public IList<NavigationPortalShortcutGroupInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<NavigationPortalShortcutGroupInfo> list = this.ibatisMapper.QueryForList<NavigationPortalShortcutGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("实例标识不能为空。");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;
        }
        #endregion
    }
}
