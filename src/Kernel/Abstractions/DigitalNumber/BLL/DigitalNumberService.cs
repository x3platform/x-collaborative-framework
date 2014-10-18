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
            configuration = DigitalNumberConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IDigitalNumberProvider>(typeof(IDigitalNumberProvider));
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
        ///<summary>保存记录</summary>
        ///<param name="param">DigitalNumberInfo 实例详细信息</param>
        ///<returns>DigitalNumberInfo 实例详细信息</returns>
        public DigitalNumberInfo Save(DigitalNumberInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string id)
        ///<summary>删除记录</summary>
        ///<param name="keys">实例的标识,多条记录以逗号分开</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string name)
        ///<summary>查询某条记录</summary>
        ///<param name="name">DigitalNumberInfo Id号</param>
        ///<returns>返回一个 DigitalNumberInfo 实例的详细信息</returns>
        public DigitalNumberInfo FindOne(string name)
        {
            return provider.FindOne(name);
        }
        #endregion

        #region 函数:FindAll()
        ///<summary>查询所有相关记录</summary>
        ///<returns>返回所有 DigitalNumberInfo 实例的详细信息</returns>
        public IList<DigitalNumberInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<returns>返回所有 DigitalNumberInfo 实例的详细信息</returns>
        public IList<DigitalNumberInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有 DigitalNumberInfo 实例的详细信息</returns>
        public IList<DigitalNumberInfo> FindAll(DataQuery query)
        {
            return provider.FindAll(whereClause, length);
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
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExistName(string name)
        ///<summary>查询是否存在相关的记录.</summary>
        ///<param name="name">名称</param>
        ///<returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 函数:Generate(string name)

        private object lockObject = new object();

        ///<summary>生成数字编码</summary>
        ///<param name="name">规则名称</param>
        ///<returns>数字编码</returns>
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

                    result = DigitalNumberScript.RunScript(param.Expression, param.UpdateDate, ref seed);

                    param.Seed = seed;

                    if (param.Name == "Key_32DigitGuid"
                        || param.Name == "Key_Guid"
                        || param.Name == "Key_Random_10"
                        || param.Name == "Key_Timestamp"
                        || param.Name == "Key_Session")
                    {
                        // 内置不需要自增的编号和更新时间的编号
                    }
                    else
                    {
                        Save(param);
                    }

                    return result;
                }
            }
        }
        #endregion

        #region 函数:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        ///<summary>根据前缀生成数字编码</summary>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="prefixCode">前缀编号</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        public string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        {
            return provider.GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);
        }
        #endregion

        #region 函数:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        ///<summary>根据前缀生成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="prefixCode">前缀编号</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        public string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        {
            return provider.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, expression);
        }
        #endregion

        #region 函数:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        ///<summary>根据类别标识成数字编码</summary>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="entityCategoryTableName">实体类别数据表</param>
        ///<param name="entityCategoryId">实体类别标识</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        public string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            return provider.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, expression);
        }
        #endregion

        #region 函数:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        ///<summary>根据类别标识成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="entityCategoryTableName">实体类别数据表</param>
        ///<param name="entityCategoryId">实体类别标识</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        public string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            return provider.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, expression);
        }
        #endregion
    }
}
