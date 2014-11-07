
using System;
using System.Collections.Generic;
using System.ComponentModel;

using X3Platform.IBatis.DataMapper;
using X3Platform.Util;

using X3Platform.Apps.Configuration;
using X3Platform.Apps.IDAL;
using X3Platform.Apps.Model;

namespace X3Platform.Apps.DAL.IBatis
{
    /// <summary></summary>
    [DataObject]
    public class ApplicationPackageLogProvider : IApplicationPackageLogProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_PackageLog";

        #region 构造函数:ApplicationPackageLogProvider()
        /// <summary>构造函数</summary>
        public ApplicationPackageLogProvider()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageLogInfo"/>详细信息</returns>
        public ApplicationPackageLogInfo Save(ApplicationPackageLogInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationPackageLogInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationPackageLogInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        public void Insert(ApplicationPackageLogInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationPackageLogInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        public void Update(ApplicationPackageLogInfo param)
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
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion


        #region 函数:DeleteByApplicationId(string applicationId)
        /// <summary>删除某个应用的同步数据包发送记录</summary>
        /// <param name="applicationId">应用标识</param>
        public void DeleteByApplicationId(string applicationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" ApplicationId = '{0}' ", StringHelper.ToSafeSQL(applicationId)));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        #region 函数:DeleteAll()
        /// <summary>删除全部应用的同步数据包发送记录</summary>
        public void DeleteAll()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Date < '{0}' ", DateTime.Now.AddDays(1)));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public ApplicationPackageLogInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationPackageLogInfo param = ibatisMapper.QueryForObject<ApplicationPackageLogInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public IList<ApplicationPackageLogInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationPackageLogInfo> list = ibatisMapper.QueryForList<ApplicationPackageLogInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
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
        /// <returns>返回一个列表实例<see cref="ApplicationPackageLogInfo"/></returns>
        public IList<ApplicationPackageLogInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationPackageLogInfo> list = ibatisMapper.QueryForList<ApplicationPackageLogInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

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
                throw new Exception("实例标识不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:GetLatestPackageLogByApplicationId(string applicationId)
        /// <summary>根据应用标识查询最新的应用包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public ApplicationPackageLogInfo GetLatestPackageLogByApplicationId(string applicationId)
        {
            string whereClause = string.Format(" ApplicationId = ##{0}## ORDER BY PackageCode DESC, Date DESC ", applicationId);

            IList<ApplicationPackageLogInfo> list = FindAll(whereClause, 1);

            return list.Count > 0 ? list[0] : null;
        }
        #endregion
    }
}
