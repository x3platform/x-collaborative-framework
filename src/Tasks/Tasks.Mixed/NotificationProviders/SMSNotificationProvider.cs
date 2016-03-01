namespace X3Platform.Tasks.NotificationProviders
{
    using System;
    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.SMS.Client;
    using X3Platform.Tasks.Model;

    /// <summary>���ŷ�����</summary>
    public class SMSNotificationProvider : INotificationProvider
    {
        #region ����:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>����֪ͨ</summary>
        /// <param name="task">������Ϣ</param>
        /// <param name="receiverIds">������</param>
        /// <param name="options">ѡ��</param>
        public int Send(TaskWorkInfo task, string receiverIds, string options)
        {
            JsonData data = JsonMapper.ToObject(options);

            string validationType = data["validationType"].ToString();

            string[] keys = receiverIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService[key];

                    if (account != null && !string.IsNullOrEmpty(account.CertifiedMobile))
                    {
                        SMSContext.Instance.SMSService.Send(key, account.CertifiedMobile, validationType);
                    }
                }
            }

            return 0;
        }
        #endregion
    }
}
