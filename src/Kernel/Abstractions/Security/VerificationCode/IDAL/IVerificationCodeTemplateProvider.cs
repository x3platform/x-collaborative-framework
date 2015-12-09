namespace X3Platform.Security.VerificationCode.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Security.VerificationCode.IDAL.IVerificationCodeTemplateProvider")]
    public interface IVerificationCodeTemplateProvider
    {
        #region 函数:FindOne(string objectType, string validationType)
        /// <summary>查询模板信息</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationCodeTemplateInfo"/>实例的详细信息</returns>
        VerificationCodeTemplateInfo FindOne(string objectType, string validationType);
        #endregion
    }
}
