namespace X3Platform.SMS.Client.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.SMS.Client.IDAL.ISMSProvider")]
    public interface ISMSProvider
    {
        #region ����:Send(string serialNumber, string accountId, string phoneNumber, string messageContent, string returnCode)
        /// <summary>������Ϣ</summary>
        /// <param name="serialNumber">��ˮ��</param>
        /// <param name="accountId">�ʺ�Ψһ��ʶ</param>
        /// <param name="phoneNumber">�ֻ�����</param>
        /// <param name="messageContent">��Ϣ����</param>
        /// <param name="returnCode">���ر���</param>
        int Send(string serialNumber, string accountId, string phoneNumber, string messageContent, string returnCode);
        #endregion
    }
}
