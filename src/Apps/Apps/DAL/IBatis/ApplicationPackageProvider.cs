
using System;
using System.Collections.Generic;
using System.ComponentModel;

using X3Platform.IBatis.DataMapper;
using X3Platform.Util;

using X3Platform.Apps.Configuration;
using X3Platform.Apps.IDAL;
using X3Platform.Apps.Model;
using X3Platform.Membership.Configuration;

namespace X3Platform.Apps.DAL.IBatis
{
    /// <summary></summary>
    [DataObject]
    public class ApplicationPackageProvider : IApplicationPackageProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_Package";

        #region 构造函数:ApplicationPackageProvider()
        /// <summary>构造函数</summary>
        public ApplicationPackageProvider()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageInfo"/>详细信息</returns>
        public ApplicationPackageInfo Save(ApplicationPackageInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationPackageInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationPackageInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        public void Insert(ApplicationPackageInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationPackageInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        public void Update(ApplicationPackageInfo param)
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

        #region 函数:DeleteAll()
        /// <summary>删除所有输出同步数据包记录</summary>
        public void DeleteAll()
        {
            string applicationId = MembershipConfigurationView.Instance.PackageStorageOutputApplicationId;

            // 1.删除数据包记录
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" [ApplicationId] = '{0}' AND [Direction] = 'Out' ",
                StringHelper.ToSafeSQL(MembershipConfigurationView.Instance.PackageStorageOutputApplicationId)));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            // 2.删除数据包发送日志记录
            AppsContext.Instance.ApplicationPackageLogService.DeleteAll();

            // 3.重置数据包编号
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_ResetPackageCode", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        public ApplicationPackageInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationPackageInfo param = ibatisMapper.QueryForObject<ApplicationPackageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        public IList<ApplicationPackageInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationPackageInfo> list = ibatisMapper.QueryForList<ApplicationPackageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

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
        /// <returns>返回一个列表实例<see cref="ApplicationPackageInfo"/></returns>
        public IList<ApplicationPackageInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " CreateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationPackageInfo> list = ibatisMapper.QueryForList<ApplicationPackageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:GetLatestPackage(string applicationId, string direction)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的编码</param>
        public ApplicationPackageInfo GetLatestPackage(string applicationId, string direction)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", StringHelper.ToSafeSQL(applicationId));

            args.Add("Direction", StringHelper.ToSafeSQL(direction));

            ApplicationPackageInfo param = ibatisMapper.QueryForObject<ApplicationPackageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetLatestPackage", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:GetPackage(string applicationId, string direction, int code)
        /// <summary>查找数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的流向: In | Out</param>
        /// <param name="code">数据包的编码</param>
        public ApplicationPackageInfo GetPackage(string applicationId, string direction, int code)
        {
            string whereClause = string.Format(@" ( ApplicationId = ##{0}## AND Direction = ##{1}## AND Code = {2} ) ",
                applicationId, direction, code);

            IList<ApplicationPackageInfo> list = FindAll(whereClause, 1);

            return list.Count > 0 ? list[0] : null;
        }
        #endregion
    }
}
