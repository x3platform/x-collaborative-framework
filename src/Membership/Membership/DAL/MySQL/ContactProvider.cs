// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IContactProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;

using X3Platform.IBatis.DataMapper;
using X3Platform.Util;

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;

namespace X3Platform.Membership.DAL.MySQL
{
    [DataObject]
    public class ContactProvider : IContactProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Contact";

        #region 构造函数:ContactProvider()
        /// <summary>构造函数</summary>
        public ContactProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ContactInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ContactInfo"/>详细信息</param>
        /// <returns>实例<see cref="ContactInfo"/>详细信息</returns>
        public ContactInfo Save(ContactInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ContactInfo)param;
        }
        #endregion

        #region 函数:Insert(ContactInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ContactInfo"/>详细信息</param>
        public void Insert(ContactInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ContactInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ContactInfo"/>详细信息</param>
        public void Update(ContactInfo param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format("Id IN ('{0}')", ids.Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ContactInfo"/>的详细信息</returns>
        public ContactInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ContactInfo param = ibatisMapper.QueryForObject<ContactInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ContactInfo"/>的详细信息</returns>
        public IList<ContactInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ContactInfo> list = ibatisMapper.QueryForList<ContactInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
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
        /// <returns>返回一个列表实例<see cref="ContactInfo"/></returns>
        public IList<ContactInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ContactInfo> list = ibatisMapper.QueryForList<ContactInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion
    }
}
