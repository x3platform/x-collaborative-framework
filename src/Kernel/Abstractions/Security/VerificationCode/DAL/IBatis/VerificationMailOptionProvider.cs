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
    public class VerificationMailOptionProvider : IVerificationMailOptionProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_VerificationMailOption";

        #region 构造函数:VerificationMailOptionProvider()
        /// <summary>构造函数</summary>
        public VerificationMailOptionProvider()
        {
            this.ibatisMapping = VerificationCodeConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOneByValidationType(string validationType)
        /// <summary>查询某条记录</summary>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationMailOptionInfo"/>实例的详细信息</returns>
        public VerificationMailOptionInfo FindOneByValidationType(string validationType)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ValidationType", StringHelper.ToSafeSQL(validationType));

            return this.ibatisMapper.QueryForObject<VerificationMailOptionInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByValidationType", tableName)), args);
        }
        #endregion
    }
}
