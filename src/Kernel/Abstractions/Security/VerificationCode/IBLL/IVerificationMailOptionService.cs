namespace X3Platform.Security.VerificationCode.IBLL
{
    #region Using Libraries
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Security.VerificationCode.IBLL.IVerificationMailOptionService")]
    public interface IVerificationMailOptionService
    {
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOneByValidationType(string validationType)
        /// <summary>查询某条记录</summary>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationMailOptionInfo"/>实例的详细信息</returns>
        VerificationMailOptionInfo FindOneByValidationType(string validationType);
        #endregion
    }
}
