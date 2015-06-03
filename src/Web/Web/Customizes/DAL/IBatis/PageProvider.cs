namespace X3Platform.Web.Customizes.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customizes.Model;
    using X3Platform.Web.Customizes.IDAL;
    #endregion

    [DataObject]
    public class PageProvider : IPageProvider
    {
      /// <summary>配置</summary>
        private WebConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Customize_Page";

        public PageProvider()
        {
            this.configuration = WebConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }

        public PageInfo this[string index]
        {
            get { return this.FindOne(index); }
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(PageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">PageInfo 实例详细信息</param>
        /// <returns>PageInfo 实例详细信息</returns>
        public PageInfo Save(PageInfo param)
        {
            if (!IsExistName(param.AuthorizationObjectType, param.AuthorizationObjectId, param.Name))
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

        #region 函数:Insert(PageInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">PageInfo 实例的详细信息</param>
        public void Insert(PageInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(PageInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">PageInfo 实例的详细信息</param>
        public void Update(PageInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">PageInfo Id号</param>
        /// <returns>返回一个 PageInfo 实例的详细信息</returns>
        public PageInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询某条记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个 PageInfo 实例的详细信息</returns>
        public PageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
            args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));
            args.Add("Name", name);

            PageInfo param = this.ibatisMapper.QueryForObject<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 PageInfo 实例的详细信息</returns>
        public IList<PageInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
        /// <returns>返回一个列表实例</returns> 
        public IList<PageInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<PageInfo> list = this.ibatisMapper.QueryForList<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">PageInfo 实例详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("实例标识不能为空。");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AuthorizationObjectType = '{0}' AND AuthorizationObjectId = '{1}' AND Name = '{2}' ", StringHelper.ToSafeSQL(authorizationObjectType), StringHelper.ToSafeSQL(authorizationObjectId), StringHelper.ToSafeSQL(name)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询某条记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个 PageInfo 实例的详细信息</returns>
        public string TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
            args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));
            args.Add("Name", name);

            PageInfo param = this.ibatisMapper.QueryForObject<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_TryParseHtml", tableName)), args);

            return param == null ? string.Empty : param.Html;
        }
        #endregion
    }
}
