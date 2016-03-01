namespace X3Platform.Tasks.NotificationProviders
{
    using System;
    using X3Platform.Tasks.Model;

    /// <summary>微信发送器</summary>
    public class WeChatNotificationProvider : INotificationProvider
    {
        #region 函数:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>发送通知</summary>
        /// <param name="task">任务信息</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="options">选项</param>
        public int Send(TaskWorkInfo task, string receiverIds, string options)
        {
            return 0;
        }
        #endregion
    }
}
