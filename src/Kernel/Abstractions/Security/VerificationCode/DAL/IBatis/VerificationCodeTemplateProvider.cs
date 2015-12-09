namespace X3Platform.Security.VerificationCode.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Security.VerificationCode.Configuration;
    using X3Platform.Security.VerificationCode.IDAL;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class VerificationCodeTemplateProvider : IVerificationCodeTemplateProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_VerificationCodeTemplate";

        #region 构造函数:VerificationCodeTemplateProvider()
        /// <summary>构造函数</summary>
        public VerificationCodeTemplateProvider()
        {
            this.ibatisMapping = VerificationCodeConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string objectType, string validationType)
        /// <summary>查询模板信息</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationCodeTemplateInfo"/>实例的详细信息</returns>
        public VerificationCodeTemplateInfo FindOne(string objectType, string validationType)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ObjectType", StringHelper.ToSafeSQL(objectType, true));
            args.Add("ValidationType", StringHelper.ToSafeSQL(validationType, true));

            return this.ibatisMapper.QueryForObject<VerificationCodeTemplateInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion
    }
}
