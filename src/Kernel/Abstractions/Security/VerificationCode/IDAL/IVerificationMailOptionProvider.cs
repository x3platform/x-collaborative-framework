namespace X3Platform.Security.VerificationCode.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Security.VerificationCode.IDAL.IVerificationMailOptionProvider")]
    public interface IVerificationMailOptionProvider
    {
        #region 函数:FindOneByValidationType(string validationType)
        /// <summary>查询某条记录</summary>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationMailOptionInfo"/>实例的详细信息</returns>
        VerificationMailOptionInfo FindOneByValidationType(string validationType);
        #endregion
    }
}
