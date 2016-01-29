namespace X3Platform.Location.Regions.DAL.IBatis
{
    using System;
    using System.Collections.Generic;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Location.Regions.Configuration;
    using X3Platform.Location.Regions.Model;
    using X3Platform.Location.Regions.IDAL;
    using X3Platform.Data;

    public class RegionProvider : IRegionProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Region";

        public RegionProvider()
        {
            this.ibatisMapping = RegionsConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(RegionInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>详细信息</param>
        /// <returns>实例<see cref="RegionInfo"/>详细信息</returns>
        public RegionInfo Save(RegionInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(RegionInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>的详细信息</param>
        public void Insert(RegionInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(RegionInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>的详细信息</param>
        public void Update(RegionInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            // 删除帐号信息
            args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">RegionInfo Id号</param>
        /// <returns>返回一个 实例<see cref="RegionInfo"/>的详细信息</returns>
        public RegionInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<RegionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 实例<see cref="RegionInfo"/>的详细信息</returns>
        public IList<RegionInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } });

            args.Add("WhereClause", whereClause);
            args.Add("OrderBy", "Id");
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<RegionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<RegionInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" Name "));

            args.Add("RowCount", 0);

            IList<RegionInfo> list = this.ibatisMapper.QueryForList<RegionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">实例<see cref="RegionInfo"/>详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion
    }
}
