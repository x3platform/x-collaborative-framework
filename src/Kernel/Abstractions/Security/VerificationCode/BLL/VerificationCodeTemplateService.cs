namespace X3Platform.Security.VerificationCode.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using Common.Logging;

    using X3Platform.CacheBuffer;
    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Security.VerificationCode.Configuration;
    using X3Platform.Security.VerificationCode.IBLL;
    using X3Platform.Security.VerificationCode.IDAL;
    using X3Platform.Util;
    #endregion

    /// <summary>权限服务</summary>
    public class VerificationCodeTemplateService : IVerificationCodeTemplateService
    {
        /// <summary>数据提供器</summary>
        private IVerificationCodeTemplateProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, VerificationCodeTemplateInfo> dictionary = new Dictionary<string, VerificationCodeTemplateInfo>();

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:VerificationCodeTemplateService()
        /// <summary>构造函数</summary>
        public VerificationCodeTemplateService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = VerificationCodeConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(VerificationCodeConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<IVerificationCodeTemplateProvider>(typeof(IVerificationCodeTemplateProvider));
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
            return this.provider.FindOne(objectType, validationType);
        }
        #endregion
    }
}
