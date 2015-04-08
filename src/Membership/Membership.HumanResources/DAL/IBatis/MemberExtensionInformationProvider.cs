//=============================================================================
//
// Copyright (c) 2007 RuanYu
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2007-06-03
//
//=============================================================================

namespace X3Platform.Membership.HumanResources.DAL.IBatis
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Xml;
    using System.Text;

    using X3Platform;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;

    using X3Platform.Membership.HumanResources.Configuration;
    using X3Platform.Membership.HumanResources.Model;
    using X3Platform.Membership.HumanResources.IDAL;

    [DataObject]
    public class MemberExtensionInformationProvider : IMemberExtensionInformationProvider
    {
        /// <summary>配置</summary>
        private HumanResourcesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Member_ExtensionInformation";

        #region 构造函数:MemberExtensionInformationProvider()
        /// <summary>构造函数</summary>
        public MemberExtensionInformationProvider()
        {
            configuration = HumanResourcesConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // 添加 删除 修改
        //-------------------------------------------------------

        #region 函数:Save(MemberExtensionInformation param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="MemberExtensionInformation"/>详细信息</param>
        /// <returns>实例<see cref="MemberExtensionInformation"/>详细信息</returns>
        public MemberExtensionInformation Save(MemberExtensionInformation param)
        {
            if (!IsExist(param.AccountId))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (MemberExtensionInformation)param;
        }
        #endregion

        #region 函数:Insert(MemberExtensionInformation param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="MemberExtensionInformation"/>详细信息</param>
        public void Insert(MemberExtensionInformation param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(MemberExtensionInformation param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="MemberExtensionInformation"/>详细信息</param>
        public void Update(MemberExtensionInformation param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        ///<summary>删除记录</summary>
        ///<param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string id)
        ///<summary>查询某条记录</summary>
        ///<param name="accountId">标识</param>
        ///<returns>返回实例<see cref="MemberExtensionInformation"/>的详细信息</returns>
        public MemberExtensionInformation FindOneByAccountId(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            MemberExtensionInformation param = ibatisMapper.QueryForObject<MemberExtensionInformation>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByAccountId", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有实例<see cref="MemberExtensionInformation"/>的详细信息</returns>
        public IList<MemberExtensionInformation> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<MemberExtensionInformation> list = ibatisMapper.QueryForList<MemberExtensionInformation>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="MemberExtensionInformation"/></returns>
        public IList<MemberExtensionInformation> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<MemberExtensionInformation> list = ibatisMapper.QueryForList<MemberExtensionInformation>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        ///<summary>查询是否存在相关的记录.</summary>
        ///<param name="id">标识</param>
        ///<returns>布尔值</returns>
        public bool IsExist(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                throw new Exception("实例标识不能为空.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId='{0}' ", StringHelper.ToSafeSQL(accountId)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion
    }
}