namespace X3Platform.SMS.Client.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.SMS.Client.IDAL.ISMSProvider")]
    public interface ISMSProvider
    {
        #region 函数:Send(string serialNumber, string accountId, string phoneNumber, string messageContent, string returnCode)
        /// <summary>发送信息</summary>
        /// <param name="serialNumber">流水号</param>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="messageContent">消息内容</param>
        /// <param name="returnCode">返回编码</param>
        int Send(string serialNumber, string accountId, string phoneNumber, string messageContent, string returnCode);
        #endregion
    }
}
