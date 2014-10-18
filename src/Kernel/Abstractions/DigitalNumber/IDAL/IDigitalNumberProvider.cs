namespace X3Platform.DigitalNumber.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.DigitalNumber.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.DigitalNumber.IDAL.IDigitalNumberProvider")]
    public interface IDigitalNumberProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(DigitalNumberInfo param)
        ///<summary>保存记录</summary>
        ///<param name="param"> 实例<see cref="DigitalNumberInfo"/>详细信息</param>
        ///<returns><see cref="DigitalNumberInfo"/> 实例详细信息</returns>
        DigitalNumberInfo Save(DigitalNumberInfo param);
        #endregion

        #region 函数:Insert(DigitalNumberInfo param)
        ///<summary>添加记录</summary>
        ///<param name="param"><see cref="DigitalNumberInfo"/> 实例的详细信息</param>
        void Insert(DigitalNumberInfo param);
        #endregion

        #region 函数:Update(DigitalNumberInfo param)
        ///<summary>修改记录</summary>
        ///<param name="param"><see cref="DigitalNumberInfo"/> 实例的详细信息</param>
        void Update(DigitalNumberInfo param);
        #endregion

        #region 函数:Delete(string id)
        ///<summary>删除记录</summary>
        ///<param name="ids">实例的标识信息,多个以逗号分开.</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string name)
        ///<summary>查询某条记录</summary>
        ///<param name="name">DigitalNumberInfo Id号</param>
        ///<returns>返回一个 <see cref="DigitalNumberInfo"/> 实例的详细信息</returns>
        DigitalNumberInfo FindOne(string name);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有实例的详细信息</returns>
        IList<DigitalNumberInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>

        /// <returns>返回一个列表</returns> 
        IList<DigitalNumberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExistName(string name)
        ///<summary>查询是否存在相关的记录.</summary>
        ///<param name="name">名称</param>
        ///<returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        ///<summary>根据前缀生成数字编码</summary>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="prefixCode">前缀编号</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression);
        #endregion

        #region 函数:GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        ///<summary>根据前缀生成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="prefixCode">前缀编号</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression);
        #endregion

        #region 函数:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        ///<summary>根据类别标识成数字编码</summary>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="entityCategoryTableName">实体类别数据表</param>
        ///<param name="entityCategoryId">实体类别标识</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression);
        #endregion

        #region 函数:GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        ///<summary>根据类别标识成数字编码</summary>
        /// <param name="command">通用SQL命令对象</param>
        ///<param name="entityTableName">实体数据表</param>
        ///<param name="entityCategoryTableName">实体类别数据表</param>
        ///<param name="entityCategoryId">实体类别标识</param>
        ///<param name="expression">规则表达式</param>
        ///<returns>数字编码</returns>
        string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression);
        #endregion
    }
}
