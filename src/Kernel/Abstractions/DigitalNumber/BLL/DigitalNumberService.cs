namespace X3Platform.DigitalNumber.BLL
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using X3Platform.Configuration;
    using X3Platform.Data;
    using X3Platform.Membership;
    using X3Platform.Security.Authority;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.IBLL;
    using X3Platform.DigitalNumber.IDAL;
    using X3Platform.DigitalNumber.Model;
    #endregion

    public class DigitalNumberService : IDigitalNumberService
    {
        private DigitalNumberConfiguration configuration = null;

        private IDigitalNumberProvider provider = null;

        public DigitalNumberService()
        {
            this.configuration = DigitalNumberConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(DigitalNumberConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.provider = objectBuilder.GetObject<IDigitalNumberProvider>(typeof(IDigitalNumberProvider));
        }

        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DigitalNumberInfo this[string name]
        {
            get { return this.FindOne(name); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(DigitalNumberInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="DigitalNumberInfo"/> 实例详细信息</param>
        /// <returns><see cref="DigitalNumberInfo"/> 实例详细信息</returns>
        public DigitalNumberInfo Save(DigitalNumberInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string name)
        /// <summary>删除记录</summary>
        /// <param name="name">名称</param>
        public void Delete(string name)
        {
            this.provider.Delete(name);
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
            return this.provider.FindOne(name);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有<see cref="DigitalNumberInfo"/> 实例的详细信息</returns>
        public IList<DigitalNumberInfo> FindAll()
        {
            return FindAll(new DataQuery() { Length = 1000 });
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="DigitalNumberInfo"/> 实例的详细信息</returns>
        public IList<DigitalNumberInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <returns>返回一个列表</returns>
        public IList<DigitalNumberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:Generate(string name)

        private object lockObject = new object();

        /// <summary>生成数字编码</summary>
        /// <param name="name">规则名称</param>
        /// <returns>数字编码</returns>
        public string Generate(string name)
        {
            string result = null;

            lock (lockObject)
            {
                DigitalNumberInfo param = FindOne(name);

                if (param == null)
                {
                    throw new Exception(string.Format("未找到相关配置信息，请联系管理员配置相关编号【{0}】参数信息。", name));
                }
                else
                {
                    int seed = param.Seed;

                    result = DigitalNumberScript.RunScript(param.Expression, param.ModifiedDate, ref seed);

                    param.Seed = seed;

                    // 忽略不需要自增的编号和更新时间的编号
                    if (!(DigitalNumberConfigurationView.Instance.IgnoreIncrementSeed.IndexOf(param.Name) > -1 || param.Seed == -1))
                    {
                        Save(param);
                    }

                    return result;
                }
            }
        }
        #endregion

        #region 函数:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>根据前缀生成数字编码</summary>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="prefixCode">前缀编号</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        {
            string code = string.Empty;

            int count = 0;

            // 有可能生成编号失败，所以 while。
            while (string.IsNullOrEmpty(code))
            {
                code = this.provider.GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);

                if (count++ > 10) { break; }
            }

            return code;
        }
        #endregion

        #region 函数:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>根据前缀生成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="prefixCode">前缀编号</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        {
            string code = string.Empty;

            int count = 0;

            // 有可能生成编号失败，所以 while。
            while (string.IsNullOrEmpty(code))
            {
                code = this.provider.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, expression);

                if (count++ > 10) { break; }
            }

            return code;
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
            string code = string.Empty;

            int count = 0;

            // 有可能生成编号失败，所以 while。
            while (string.IsNullOrEmpty(code))
            {
                code = this.provider.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, expression);

                if (count++ > 10) { break; }
            }

            return code;
        }
        #endregion

        #region 函数:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>根据类别标识成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="entityTableName">实体数据表</param>
        /// <param name="entityCategoryTableName">实体类别数据表</param>
        /// <param name="entityCategoryId">实体类别标识</param>
        /// <param name="expression">规则表达式</param>
        /// <returns>数字编码</returns>
        public string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            string code = string.Empty;

            int count = 0;

            // 有可能生成编号失败，所以 while。
            while (string.IsNullOrEmpty(code))
            {
                code = this.provider.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, expression);

                if (count++ > 10) { break; }
            }

            return code;
        }
        #endregion
    }
}
