namespace X3Platform.Tasks.NotificationProviders
{
    using System;
    using X3Platform.Tasks.Model;

    /// <summary>΢�ŷ�����</summary>
    public class WeChatNotificationProvider : INotificationProvider
    {
        #region ����:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>����֪ͨ</summary>
        /// <param name="task">������Ϣ</param>
        /// <param name="receiverIds">������</param>
        /// <param name="options">ѡ��</param>
        public int Send(TaskWorkInfo task, string receiverIds, string options)
        {
            return 0;
        }
        #endregion
    }
}
