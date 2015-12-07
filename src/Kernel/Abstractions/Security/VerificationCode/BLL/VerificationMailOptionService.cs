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
    public class VerificationMailOptionService : IVerificationMailOptionService
    {
        /// <summary>数据提供器</summary>
        private IVerificationMailOptionProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, VerificationMailOptionInfo> dictionary = new Dictionary<string, VerificationMailOptionInfo>();

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:VerificationMailOptionService()
        /// <summary>构造函数</summary>
        public VerificationMailOptionService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = VerificationCodeConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(VerificationCodeConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<IVerificationMailOptionProvider>(typeof(IVerificationMailOptionProvider));
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
            return this.provider.FindOneByValidationType(validationType);
        }
        #endregion
    }
}
