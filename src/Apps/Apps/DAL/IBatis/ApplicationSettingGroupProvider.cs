namespace X3Platform.Apps.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;

    /// <summary></summary>
    [DataObject]
    public class ApplicationSettingGroupProvider : IApplicationSettingGroupProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_SettingGroup";

        #region 构造函数:ApplicationSettingGroupProvider()
        /// <summary>构造函数</summary>
        public ApplicationSettingGroupProvider()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationSettingGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationSettingGroupInfo"/>详细信息</returns>
        public ApplicationSettingGroupInfo Save(ApplicationSettingGroupInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationSettingGroupInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationSettingGroupInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        public void Insert(ApplicationSettingGroupInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Feature_Key_Code");

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationSettingGroupInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        public void Update(ApplicationSettingGroupInfo param)
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

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public ApplicationSettingGroupInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationSettingGroupInfo param = ibatisMapper.QueryForObject<ApplicationSettingGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationSettingGroupInfo> list = ibatisMapper.QueryForList<ApplicationSettingGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父级对象的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAllByParentId(string parentId)
        {
            string whereClause = string.Format(@" ParentId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(parentId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAllByApplicationId(string applicationId)
        {
            string whereClause = string.Format(@" ApplicationId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(applicationId));

            return FindAll(whereClause, 0);
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
        /// <returns>返回一个列表实例<see cref="ApplicationSettingGroupInfo"/></returns>
        public IList<ApplicationSettingGroupInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationSettingGroupInfo> list = ibatisMapper.QueryForList<ApplicationSettingGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

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
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationSettingGroupInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用参数分组信息</param>
        public void SyncFromPackPage(ApplicationSettingGroupInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
        }
        #endregion
    }
}
