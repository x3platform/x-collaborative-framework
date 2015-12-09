namespace X3Platform.SMS.Client.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.SMS.Client.IBLL.ISMSService")]
    public interface ISMSService
    {
        #region 函数:Send(string phoneNumber, string validationType)
        /// <summary>发送信息</summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="validationType">短信验证类型</param>
        int Send(string phoneNumber, string validationType);
        #endregion

        #region 函数:Send(string accountId, string phoneNumber, string validationType)
        /// <summary>发送信息</summary>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="validationType">短信验证类型</param>
        int Send(string accountId, string phoneNumber, string validationType);
        #endregion
    }
}
