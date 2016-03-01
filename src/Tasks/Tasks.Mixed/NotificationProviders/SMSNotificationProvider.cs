namespace X3Platform.Tasks.NotificationProviders
{
    using System;
    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.SMS.Client;
    using X3Platform.Tasks.Model;

    /// <summary>短信发送器</summary>
    public class SMSNotificationProvider : INotificationProvider
    {
        #region 函数:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>发送通知</summary>
        /// <param name="task">任务信息</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="options">选项</param>
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
