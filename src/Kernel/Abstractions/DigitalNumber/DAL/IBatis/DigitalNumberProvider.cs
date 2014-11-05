namespace X3Platform.DigitalNumber.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.IDAL;
    using X3Platform.DigitalNumber.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class DigitalNumberProvider : IDigitalNumberProvider
    {
        /// <summary>配置</summary>
        private DigitalNumberConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_DigitalNumber";

        #region 构造函数:AuthorityProvider()
        /// <summary>构造函数</summary>
        public DigitalNumberProvider()
        {
            this.configuration = DigitalNumberConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;
            Console.WriteLine(this.ibatisMapping);
            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(DigitalNumberInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="DigitalNumberInfo"/> 实例详细信息</param>
        /// <returns><see cref="DigitalNumberInfo"/> 实例详细信息</returns>
        public DigitalNumberInfo Save(DigitalNumberInfo param)
        {
            if (!IsExistName(param.Name))
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

        #region 函数:Insert(DigitalNumberInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param"><see cref="DigitalNumberInfo"/> 实例的详细信息</param>
        public void Insert(DigitalNumberInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(DigitalNumberInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param"><see cref="DigitalNumberInfo"/> 实例的详细信息</param>
        public void Update(DigitalNumberInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号分开</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", id.Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">DigitalNumberInfo Id号</param>
        /// <returns>返回一个<see cref="DigitalNumberInfo"/> 实例的详细信息</returns>
        public DigitalNumberInfo FindOne(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", name);

            return this.ibatisMapper.QueryForObject<DigitalNumberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="DigitalNumberInfo"/> 实例的详细信息</returns>
        public IList<DigitalNumberInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql());
            args.Add("OrderBy", query.GetOrderBySql());
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<DigitalNumberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        public IList<DigitalNumberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql());
            args.Add("OrderBy", query.GetOrderBySql());

            IList<DigitalNumberInfo> list = this.ibatisMapper.QueryForList<DigitalNumberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }

        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("名称必须填写。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }

        #region 函数:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>根据前缀生成数字编码</summary>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="prefixCode">前缀编号</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        {
            // 获取前缀
            string prefix = DigitalNumberScript.RunPrefixScript(expression, prefixCode.ToUpper(), DateTime.Now);

            // 根据前缀信息查询当前最大的编号
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityTableName", entityTableName);
            args.Add("Prefix", prefix);

            int seed = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMaxSeedByPrefix", tableName)), args));

            return DigitalNumberScript.RunScript(expression, prefixCode, DateTime.Now, ref seed);
        }
        #endregion

        #region 函数:GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        /// <summary>根据前缀生成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="prefixCode">前缀编号</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        {
            // 获取前缀
            string prefix = DigitalNumberScript.RunPrefixScript(expression, prefixCode.ToUpper(), DateTime.Now);

            // 根据前缀信息查询当前最大的编号
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityTableName", entityTableName);
            args.Add("Prefix", prefix);

            string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMaxSeedByPrefix", tableName)), args);

            int seed = Convert.ToInt32(command.ExecuteScalar(commandText));

            return DigitalNumberScript.RunScript(expression, prefixCode, DateTime.Now, ref seed);
        }
        #endregion

        #region 函数:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>根据类别标识成数字编码</summary>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="entityCategoryTableName">实体类别数据表</param>
        /// <param name="entityCategoryId">实体类别标识</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityCategoryTableName", entityCategoryTableName);
            args.Add("EntityCategoryId", entityCategoryId);

            string prefixCode = (string)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPrefixCodeByCategoryId", tableName)), args);

            return GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);
        }
        #endregion

        #region 函数:GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>根据类别标识成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="entityCategoryTableName">实体类别数据表</param>
        /// <param name="entityCategoryId">实体类别标识</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityCategoryTableName", entityCategoryTableName);
            args.Add("EntityCategoryId", entityCategoryId);

            string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPrefixCodeByCategoryId", tableName)), args);

            string prefixCode = (string)command.ExecuteScalar(commandText);

            return GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);
        }
        #endregion
    }
}