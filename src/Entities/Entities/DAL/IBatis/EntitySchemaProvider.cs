namespace X3Platform.Entities.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class EntitySchemaProvider : IEntitySchemaProvider
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Entity_Schema";

        #region 构造函数:EntitySchemaProvider()
        /// <summary>构造函数</summary>
        public EntitySchemaProvider()
        {
            this.configuration = EntitiesConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
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

        #region 函数:Save(EntitySchemaInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        /// <returns>实例<see cref="EntitySchemaInfo"/>详细信息</returns>
        public EntitySchemaInfo Save(EntitySchemaInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (EntitySchemaInfo)param;
        }
        #endregion

        #region 函数:Insert(EntitySchemaInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        public void Insert(EntitySchemaInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(EntitySchemaInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="EntitySchemaInfo"/>详细信息</param>
        public void Update(EntitySchemaInfo param)
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

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            EntitySchemaInfo param = this.ibatisMapper.QueryForObject<EntitySchemaInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOneByName(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", StringHelper.ToSafeSQL(name));

            return this.ibatisMapper.QueryForObject<EntitySchemaInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", this.tableName)), args);
        }
        #endregion

        #region 函数:FindOneByEntityClassName(string entityClassName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOneByEntityClassName(string entityClassName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityClassName", StringHelper.ToSafeSQL(entityClassName));

            return this.ibatisMapper.QueryForObject<EntitySchemaInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByEntityClassName", this.tableName)), args);
        }
        #endregion

        #region 函数:FindOneByEntityClassFullName(string entityClassFullName)
        /// <summary>查询某条记录</summary>
        /// <param name="entityClassFullName">实体类全称</param>
        /// <returns>返回实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public EntitySchemaInfo FindOneByEntityClassFullName(string entityClassFullName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityClassFullName", StringHelper.ToSafeSQL(entityClassFullName));

            return this.ibatisMapper.QueryForObject<EntitySchemaInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByEntityClassFullName", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public IList<EntitySchemaInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<EntitySchemaInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAllByIds(string ids)
        /// <summary>查询所有相关记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        /// <returns>返回所有实例<see cref="EntitySchemaInfo"/>的详细信息</returns>
        public IList<EntitySchemaInfo> FindAllByIds(string ids)
        {
            string whereClause = string.Format(" Id IN (##{0}## ) ", ids.Replace(",", "## ,##"));

            return this.FindAll(whereClause, 0);
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
        /// <returns>返回一个列表实例<see cref="EntitySchemaInfo"/></returns> 
        public IList<EntitySchemaInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = "";

            if (query.Variables["scence"] == "Query")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause = " Name LIKE '" + searchText + "' ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<EntitySchemaInfo> list = this.ibatisMapper.QueryForList<EntitySchemaInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空."); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistCode(string code, string ignoreIds = null)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="code">代码</param>
        /// <param name="ignoreIds">忽略对象的标识，多个以逗号隔开</param>
        /// <returns>布尔值</returns>
        public bool IsExistCode(string code, string ignoreIds = null)
        {
            if (string.IsNullOrEmpty(code)) { throw new Exception("代码不能为空."); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" Code = '{0}' ", StringHelper.ToSafeSQL(code));

            if (!string.IsNullOrEmpty(ignoreIds))
            {
                whereClause += string.Format(" AND Id NOT IN ('{0}')", StringHelper.ToSafeSQL(ignoreIds).Replace(",", "','"));
            }

            args.Add("WhereClause", whereClause);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name, string ignoreIds = null)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">名称</param>
        /// <param name="ignoreIds">忽略对象的标识，多个以逗号隔开</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name, string ignoreIds = null)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("名称不能为空."); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name));

            if (!string.IsNullOrEmpty(ignoreIds))
            {
                whereClause += string.Format(" AND Id NOT IN ('{0}')", StringHelper.ToSafeSQL(ignoreIds).Replace(",", "','"));
            }

            args.Add("WhereClause", whereClause);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistEntityClassName(string entityClassName, string ignoreIds = null)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="ignoreIds">忽略对象的标识，多个以逗号隔开</param>
        /// <returns>布尔值</returns>
        public bool IsExistEntityClassName(string entityClassName, string ignoreIds = null)
        {
            if (string.IsNullOrEmpty(entityClassName)) { throw new Exception("实体类名称不能为空."); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = string.Format(" EntityClassName = '{0}' ", StringHelper.ToSafeSQL(entityClassName));

            if (!string.IsNullOrEmpty(ignoreIds))
            {
                whereClause += string.Format(" AND Id NOT IN ('{0}')", StringHelper.ToSafeSQL(ignoreIds).Replace(",", "','"));
            }

            args.Add("WhereClause", whereClause);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:SetCode(string id, string code)
        /// <summary>设置对象的代码</summary>
        /// <param name="id">标识</param>
        /// <param name="code">代码</param>
        /// <returns>布尔值</returns>
        public int SetCode(string id, string code)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("Code", StringHelper.ToSafeSQL(code));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCode", tableName)), args);

            return 0;
        }
        #endregion
    }
}
